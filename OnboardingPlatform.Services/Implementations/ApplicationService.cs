using Microsoft.EntityFrameworkCore;
using OnboardingPlatform.Core.DTOs.Requests;
using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Core.Enums;
using OnboardingPlatform.Core.Models;
using OnboardingPlatform.Data.Implementations;
using OnboardingPlatform.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnboardingPlatform.Services.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly AppDbContext _db;
        private readonly IIdentityService _identity;

        public ApplicationService(AppDbContext db, IIdentityService identity)
        {
            _db = db;
            _identity = identity;
        }

        // ──────────────────────────────────────────────
        // POST /applications/start
        // ──────────────────────────────────────────────
        public async Task<StartApplicationResponse> StartApplicationAsync(StartApplicationRequest request)
        {
            // 1. Look up product
            var product = await _db.Products
                .FirstOrDefaultAsync(p => p.ProductCode.ToString() == request.ProductCode && p.IsActive)
                ?? throw new InvalidOperationException($"Product '{request.ProductCode}' not found or inactive.");

            var requiredIdentifiers = JsonSerializer.Deserialize<List<string>>(product.RequiredIdentifiers)
                ?? new List<string>();

            // 2. Hash identifiers
            var primaryType = requiredIdentifiers[0];
            var primaryHash = _identity.HashIdentifier(request.PrimaryIdentifierValue);
            var primaryMasked = _identity.MaskIdentifier(request.PrimaryIdentifierValue);

            string? secondaryType = requiredIdentifiers.Count > 1 ? requiredIdentifiers[1] : null;
            string? secondaryHash = request.SecondaryIdentifierValue != null
                ? _identity.HashIdentifier(request.SecondaryIdentifierValue)
                : null;

            // 3. Check for existing IN_PROGRESS draft (cross-device resume)
            var existingDraft = await _db.DraftApplications
                .FirstOrDefaultAsync(d =>
                    d.ProductId == product.ProductId &&
                    d.PrimaryIdentifierValueHash == primaryHash &&
                    d.Status == DraftStatus.IN_PROGRESS);

            if (existingDraft != null)
            {
                // Resume the draft exactly where they left off
                return new StartApplicationResponse
                {
                    DraftId = existingDraft.DraftId,
                    IsResumed = true,
                    IsExistingCustomer = existingDraft.CustomerId.HasValue,
                    RequiresSecurityCheck = false,
                    CurrentStep = existingDraft.CurrentStep.ToString(),
                    FormData = DeserializeFormData(existingDraft.FormDataJson)
                };
            }

            // 4. No active draft — check if customer already exists
            var existingCustomerId = await _identity.FindExistingCustomerAsync(
                primaryType, primaryHash, secondaryType, secondaryHash);

            var isExistingCustomer = existingCustomerId.HasValue;

            // 5. Create a fresh DraftApplication (Emmanuel's table, but we CREATE it here)
            var draft = new DraftApplication
            {
                DraftId = Guid.NewGuid(),
                ProductId = product.ProductId,
                CustomerId = existingCustomerId,

                PrimaryIdentifierType = Enum.Parse<IdentifierType>(primaryType),
                PrimaryIdentifierValueHash = primaryHash,

                SecondaryIdentifierType = secondaryType != null
         ? Enum.Parse<IdentifierType>(secondaryType)
         : null,

                SecondaryIdentifierValueHash = secondaryHash,

                CurrentStep = isExistingCustomer
         ? DraftStep.PRODUCT_SPECIFIC_INFO
         : DraftStep.PERSONAL_INFO,

                FormDataJson = "{}",
                Channel = Enum.Parse<Channel>(request.Channel),
                Status = DraftStatus.IN_PROGRESS,
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddDays(30)
            };

            _db.DraftApplications.Add(draft);
            await _db.SaveChangesAsync();

            return new StartApplicationResponse
            {
                DraftId = draft.DraftId,
                IsResumed = false,
                IsExistingCustomer = isExistingCustomer,
                RequiresSecurityCheck = isExistingCustomer,
                CurrentStep = draft.CurrentStep.ToString(),
                FormData = new Dictionary<string, object>()
            };
        }

        // ──────────────────────────────────────────────
        // POST /applications/{draftId}/finalize
        // ──────────────────────────────────────────────
        public async Task<FinalizeApplicationResponse> FinalizeApplicationAsync(FinalizeApplicationRequest request)
        {
            var draft = await _db.DraftApplications
                .Include(d => d.Product)
                .FirstOrDefaultAsync(d => d.DraftId == request.DraftId && d.Status == DraftStatus.IN_PROGRESS)
                ?? throw new InvalidOperationException("Draft not found or already finalized.");

            var formData = request.FormData;

            // Step A: Create Customer if new
            // Step A: Create Customer if new
            Guid customerId;

            if (draft.CustomerId == null)
            {
                var customer = new Customer
                {
                    FirstName = GetString(formData, "firstName"),
                    MiddleName = formData.TryGetValue("middleName", out var middleName)
                        ? middleName?.ToString()
                        : null,
                    LastName = GetString(formData, "lastName"),
                    DateOfBirth = formData.TryGetValue("dateOfBirth", out var dateOfBirth)
                        ? DateTime.TryParse(dateOfBirth?.ToString(), out var parsedDate)
                            ? parsedDate
                            : null
                        : null,
                    Gender = formData.TryGetValue("gender", out var gender)
                        ? gender?.ToString()
                        : null,
                    Nationality = formData.TryGetValue("nationality", out var nationality)
                        ? nationality?.ToString()
                        : "Nigerian",
                    Status = CustomerStatus.ACTIVE,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                // Add the primary identifier through the navigation property.
                customer.Identifiers.Add(new CustomerIdentifier
                {
                    IdentifierType = draft.PrimaryIdentifierType,
                    IdentifierValueHash = draft.PrimaryIdentifierValueHash,
                    IdentifierValueMasked = "(submitted)",
                    IsVerified = true,
                    VerifiedAt = DateTime.UtcNow
                });

                if (draft.SecondaryIdentifierType.HasValue &&
                    draft.SecondaryIdentifierValueHash is not null)
                {
                    customer.Identifiers.Add(new CustomerIdentifier
                    {
                        IdentifierType = draft.SecondaryIdentifierType.Value,
                        IdentifierValueHash = draft.SecondaryIdentifierValueHash,
                        IdentifierValueMasked = "(submitted)",
                        IsVerified = true,
                        VerifiedAt = DateTime.UtcNow
                    });
                }

                _db.Customers.Add(customer);

                // CustomerId is already available because Customer uses Guid.NewGuid().
                customerId = customer.CustomerId;
                draft.CustomerId = customerId;

                if (formData.TryGetValue("address", out var addressObject))
                {
                    var addressJson = JsonSerializer.Serialize(addressObject);
                    var address = JsonSerializer.Deserialize<Dictionary<string, string>>(addressJson);

                    if (address is not null)
                    {
                        _db.CustomerAddresses.Add(new CustomerAddress
                        {
                            CustomerId = customer.CustomerId,
                            HouseNumber = address.TryGetValue("houseNumber", out var houseNumber)
                                ? houseNumber
                                : null,
                            Street = address.TryGetValue("street", out var street)
                                ? street
                                : string.Empty,
                            City = address.TryGetValue("city", out var city)
                                ? city
                                : string.Empty,
                            State = address.TryGetValue("state", out var state)
                                ? state
                                : string.Empty,
                            Country = address.TryGetValue("country", out var country)
                                ? country
                                : "Nigeria",
                            IsPrimary = true
                        });
                    }
                }
            }
            else
            {
                customerId = draft.CustomerId.Value;
            }

            // Step B: Create CustomerProduct
            var customerProduct = new CustomerProduct
            {
                CustomerId = customerId,
                ProductId = draft.ProductId,
                DraftId = draft.DraftId,
                Status = CustomerProductStatus.ACTIVE,
                CreatedAt = DateTime.Now
            };
            _db.CustomerProducts.Add(customerProduct);

            // Step C: Generate a mock account reference
            var productAccountRef = GenerateAccountReference(draft.Product.ProductCode);

            // Step D: Mark draft as SUBMITTED
            draft.Status = DraftStatus.SUBMITTED;
            draft.LastUpdatedAt = DateTime.Now;

            await _db.SaveChangesAsync();

            return new FinalizeApplicationResponse
            {
                CustomerId = customerId,
                CustomerProductId = customerProduct.CustomerProductId,
                ProductAccountReference = productAccountRef,
                Status = "ACTIVE"
            };
        }

        // ── Helpers ───────────────────────────────────
        private static string GenerateAccountReference(ProductCode code) => code switch
        {
            ProductCode.SAVINGS or ProductCode.CURRENT => "00" + Random.Shared.Next(10000000, 99999999),
            ProductCode.PENSION_RSA => "PEN" + Random.Shared.Next(100000000, 999999999),
            ProductCode.STOCKBROKING => "CSC" + Random.Shared.Next(100000000, 999999999),
            ProductCode.INSURANCE => "POL" + Random.Shared.Next(100000000, 999999999),
            _ => Guid.NewGuid().ToString("N")[..10].ToUpper()
        };

        private static string GetString(Dictionary<string, object> data, string key) =>
            data.TryGetValue(key, out var v) ? v?.ToString() ?? string.Empty : string.Empty;

        private static Dictionary<string, object> DeserializeFormData(string json)
        {
            try { return JsonSerializer.Deserialize<Dictionary<string, object>>(json) ?? new(); }
            catch { return new(); }
        }
    }
}
