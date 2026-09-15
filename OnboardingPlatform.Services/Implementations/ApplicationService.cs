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
        private readonly ISavingsAccountService _savings;
        private readonly ICurrentAccountService _current;
        private readonly IPensionAccountService _pension;
        private readonly IStockBrokingAccountService _stockBroking;
        private readonly IConsentService _consent;

        public ApplicationService(
            AppDbContext db, 
            IIdentityService identity, 
            ISavingsAccountService savings, 
            ICurrentAccountService current,
            IPensionAccountService pension,
            IStockBrokingAccountService stockBroking,
            IConsentService consent)
        {
            _db = db;
            _identity = identity;
            _savings = savings;
            _current = current;
            _pension = pension;
            _stockBroking = stockBroking;
            _consent = consent;
        }

        // ──────────────────────────────────────────────
        // POST /applications/start
        // ──────────────────────────────────────────────
        public async Task<StartApplicationResponse> StartApplicationAsync(StartApplicationRequest request)
        {
            // Before the database query
            if (!Enum.TryParse<ProductCode>(
                    request.ProductCode,
                    ignoreCase: true,
                    out var parsedProductCode))
            {
                throw new InvalidOperationException($"Product '{request.ProductCode}' not found or inactive.");
            }

            // 1. Look up product
            var product = await _db.Products
                .FirstOrDefaultAsync(p => p.ProductCode == parsedProductCode && p.IsActive)
                ?? throw new InvalidOperationException($"Product '{request.ProductCode}' not found or inactive.");

            var requiredIdentifiers = JsonSerializer.Deserialize<List<string>>(product.RequiredIdentifiers)
                ?? new List<string>();

            // 2. Hash identifiers
            var primaryType = requiredIdentifiers[0];
            var primaryHash = _identity.HashIdentifier(request.PrimaryIdentifierValue);

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
                // Also check if this draft belongs to an existing customer
                // so we still return their pre-filled data
                ExistingCustomerData? resumedCustomerData = null;

                if (existingDraft.CustomerId.HasValue)
                {
                    var resumedCustomer = await _db.Customers
                        .Include(c => c.Addresses)
                        .FirstOrDefaultAsync(c => c.CustomerId == existingDraft.CustomerId.Value);

                    if (resumedCustomer != null)
                    {
                        resumedCustomerData = new ExistingCustomerData
                        {
                            FirstName = resumedCustomer.FirstName,
                            MiddleName = resumedCustomer.MiddleName,
                            LastName = resumedCustomer.LastName,
                            DateOfBirth = resumedCustomer.DateOfBirth?.ToString("yyyy-MM-dd"),
                            Gender = resumedCustomer.Gender,
                            Address = resumedCustomer.Addresses.FirstOrDefault()?.Street
                        };
                    }
                }

                return new StartApplicationResponse
                {
                    DraftId = existingDraft.DraftId,
                    IsResumed = true,
                    IsExistingCustomer = existingDraft.CustomerId.HasValue,
                    RequiresSecurityCheck = false, // already passed on first attempt
                    CurrentStep = existingDraft.CurrentStep.ToString(),
                    FormData = DeserializeFormData(existingDraft.FormDataJson)
                        .Where(kvp => kvp.Value is not null)
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value!),
                    ExistingCustomer = resumedCustomerData
                };
            }

            // 4. No active draft — check if customer already exists
            var existingCustomerId = await _identity.FindExistingCustomerAsync(
                primaryType, primaryHash, secondaryType, secondaryHash);

            var isExistingCustomer = existingCustomerId.HasValue;

            // 5. Determine which step to start on
            var startingStep = isExistingCustomer
                ? DraftStep.SECURITY_VERIFICATION   // must verify identity before we expose their data
                : DraftStep.PERSONAL_INFO;

            // 6. Create a fresh DraftApplication
            var draft = new DraftApplication
            {
                DraftId = Guid.NewGuid(),
                ProductId = product.ProductId,
                CustomerId = existingCustomerId,    // null for new customers, populated for existing
                PrimaryIdentifierType = Enum.Parse<IdentifierType>(primaryType),
                PrimaryIdentifierValueHash = primaryHash,
                SecondaryIdentifierType = secondaryType != null
                    ? Enum.Parse<IdentifierType>(secondaryType)
                    : null,
                SecondaryIdentifierValueHash = secondaryHash,
                CurrentStep = startingStep,
                FormDataJson = "{}",
                Channel = Enum.Parse<Channel>(request.Channel),
                Status = DraftStatus.IN_PROGRESS,
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddDays(30)
            };

            _db.DraftApplications.Add(draft);
            await _db.SaveChangesAsync();

            // 7. If existing customer, fetch their data to pre-fill the frontend
            ExistingCustomerData? existingCustomerData = null;

            if (isExistingCustomer)
            {
                var existingCustomer = await _db.Customers
                    .Include(c => c.Addresses)
                    .FirstOrDefaultAsync(c => c.CustomerId == existingCustomerId!.Value);

                if (existingCustomer != null)
                {
                    existingCustomerData = new ExistingCustomerData
                    {
                        FirstName = existingCustomer.FirstName,
                        MiddleName = existingCustomer.MiddleName,
                        LastName = existingCustomer.LastName,
                        DateOfBirth = existingCustomer.DateOfBirth?.ToString("yyyy-MM-dd"),
                        Gender = existingCustomer.Gender,
                        PhoneNumber = existingCustomer.PhoneNumber,
                        Email = existingCustomer.Email,
                        Address = existingCustomer.Addresses.FirstOrDefault()?.Street
                    };
                }
            }

            return new StartApplicationResponse
            {
                DraftId = draft.DraftId,
                IsResumed = false,
                IsExistingCustomer = isExistingCustomer,
                RequiresSecurityCheck = isExistingCustomer,
                CurrentStep = startingStep.ToString(),
                FormData = new Dictionary<string, object>(),
                ExistingCustomer = existingCustomerData  // null for new customers, populated for existing
            };
        }

        // ──────────────────────────────────────────────
        // POST /applications/{draftId}/finalize
        // ──────────────────────────────────────────────
        public async Task<FinalizeApplicationResponse> FinalizeApplicationAsync(Guid draftId)
        {
            // 1. Load the draft — read FormDataJson from DB, not from request body
            var draft = await _db.DraftApplications
                .Include(d => d.Product)
                .FirstOrDefaultAsync(d =>
                    d.DraftId == draftId &&
                    d.Status == DraftStatus.IN_PROGRESS)
                ?? throw new InvalidOperationException("Draft not found or already finalized.");

            // 2. Deserialize the form data Emmanuel's save endpoint has been building up
            var formData = DeserializeFormData(draft.FormDataJson);

            Guid customerId;

            if (draft.CustomerId == null)
            {
                // ── NEW CUSTOMER PATH ─────────────────────────────────────
                // 3a. Create the Customer record
                var customer = new Customer
                {
                    CustomerId = Guid.NewGuid(),
                    FirstName = GetString(formData, "firstName"),
                    MiddleName = GetStringOrNull(formData, "middleName"),
                    LastName = GetString(formData, "lastName"),
                    DateOfBirth = ParseDate(formData, "dateOfBirth"),
                    Gender = GetStringOrNull(formData, "gender"),
                    Nationality = GetStringOrNull(formData, "nationality") ?? "Nigerian",
                    PhoneNumber = GetStringOrNull(formData, "phoneNumber"),
                    Email = GetStringOrNull(formData, "email"),
                    Status = CustomerStatus.ACTIVE,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _db.Customers.Add(customer);

                // 3b. Save the primary identifier (the one the product required, e.g. BVN)
                _db.CustomerIdentifiers.Add(new CustomerIdentifier
                {
                    IdentifierId = Guid.NewGuid(),
                    CustomerId = customer.CustomerId,
                    IdentifierType = draft.PrimaryIdentifierType,
                    IdentifierValueHash = draft.PrimaryIdentifierValueHash,
                    IdentifierValueMasked = "(submitted)",
                    IsVerified = true,
                    VerifiedAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                });

                // 3c. Save the secondary identifier if the product required one (e.g. PHONE for Pension)
                if (draft.SecondaryIdentifierType.HasValue &&
                    !string.IsNullOrEmpty(draft.SecondaryIdentifierValueHash))
                {
                    _db.CustomerIdentifiers.Add(new CustomerIdentifier
                    {
                        IdentifierId = Guid.NewGuid(),
                        CustomerId = customer.CustomerId,
                        IdentifierType = draft.SecondaryIdentifierType.Value,
                        IdentifierValueHash = draft.SecondaryIdentifierValueHash,
                        IdentifierValueMasked = "(submitted)",
                        IsVerified = true,
                        VerifiedAt = DateTime.Now,
                        CreatedAt = DateTime.Now
                    });
                }

                // 3d. Save phone number as extra identifier if it was collected
                //     (even if the product didn't require it — this is the cross-product bridge)
                var phoneRaw = GetStringOrNull(formData, "phoneNumber");
                if (!string.IsNullOrEmpty(phoneRaw) &&
                    draft.PrimaryIdentifierType != IdentifierType.PHONE &&
                    draft.SecondaryIdentifierType != IdentifierType.PHONE)
                {
                    // Avoid duplicate — only save if PHONE wasn't already saved above
                    _db.CustomerIdentifiers.Add(new CustomerIdentifier
                    {
                        IdentifierId = Guid.NewGuid(),
                        CustomerId = customer.CustomerId,
                        IdentifierType = IdentifierType.PHONE,
                        IdentifierValueHash = _identity.HashIdentifier(phoneRaw),
                        IdentifierValueMasked = _identity.MaskIdentifier(phoneRaw),
                        IsVerified = false,
                        CreatedAt = DateTime.Now
                    });
                }

                // 3e. Save email as extra identifier if it was collected
                var emailRaw = GetStringOrNull(formData, "email");
                if (!string.IsNullOrEmpty(emailRaw) &&
                    draft.PrimaryIdentifierType != IdentifierType.EMAIL &&
                    draft.SecondaryIdentifierType != IdentifierType.EMAIL)
                {
                    _db.CustomerIdentifiers.Add(new CustomerIdentifier
                    {
                        IdentifierId = Guid.NewGuid(),
                        CustomerId = customer.CustomerId,
                        IdentifierType = IdentifierType.EMAIL,
                        IdentifierValueHash = _identity.HashIdentifier(emailRaw),
                        IdentifierValueMasked = _identity.MaskIdentifier(emailRaw),
                        IsVerified = false,
                        CreatedAt = DateTime.Now
                    });
                }

                // 3f. Save address if provided
                if (formData.TryGetValue("address", out var addrObj) && addrObj != null)
                {
                    var addrJson = addrObj is JsonElement je
                        ? je.GetRawText()
                        : JsonSerializer.Serialize(addrObj);

                    var addr = JsonSerializer.Deserialize<Dictionary<string, string>>(addrJson);

                    if (addr != null)
                    {
                        _db.CustomerAddresses.Add(new CustomerAddress
                        {
                            AddressId = Guid.NewGuid(),
                            CustomerId = customer.CustomerId,
                            HouseNumber = addr.TryGetValue("houseNumber", out var hn) ? hn : null,
                            Street = addr.TryGetValue("street", out var st) ? st : string.Empty,
                            City = addr.TryGetValue("city", out var ct) ? ct : string.Empty,
                            State = addr.TryGetValue("state", out var s) ? s : string.Empty,
                            Country = addr.TryGetValue("country", out var co) ? co : "Nigeria",
                            IsPrimary = true,
                            CreatedAt = DateTime.Now
                        });
                    }
                }

                customerId = customer.CustomerId;
                draft.CustomerId = customerId;
            }
            else
            {
                // ── EXISTING CUSTOMER PATH ────────────────────────────────
                // Customer record already exists — just use their ID
                // No need to re-create anything, their data is already in the DB
                customerId = draft.CustomerId.Value;

                // Update their record if they changed anything (e.g. new address)
                var existingCustomer = await _db.Customers
                    .Include(c => c.Addresses)
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId);

                if (existingCustomer != null)
                {
                    existingCustomer.UpdatedAt = DateTime.Now;

                    // If they provided a new address for this product application, save it
                    if (formData.TryGetValue("address", out var addrObj) && addrObj != null)
                    {
                        var addrJson = addrObj is JsonElement je2
                            ? je2.GetRawText()
                            : JsonSerializer.Serialize(addrObj);

                        var addr = JsonSerializer.Deserialize<Dictionary<string, string>>(addrJson);

                        if (addr != null && !existingCustomer.Addresses.Any())
                        {
                            // Only add if they don't already have one
                            _db.CustomerAddresses.Add(new CustomerAddress
                            {
                                AddressId = Guid.NewGuid(),
                                CustomerId = customerId,
                                HouseNumber = addr.TryGetValue("houseNumber", out var hn) ? hn : null,
                                Street = addr.TryGetValue("street", out var st) ? st : string.Empty,
                                City = addr.TryGetValue("city", out var ct) ? ct : string.Empty,
                                State = addr.TryGetValue("state", out var s) ? s : string.Empty,
                                Country = addr.TryGetValue("country", out var co) ? co : "Nigeria",
                                IsPrimary = false,  // they already have a primary from their first product
                                CreatedAt = DateTime.Now
                            });
                        }
                    }
                }
            }

            // 4. Create or reuse the CustomerProduct link
            var existingCustomerProduct = await _db.CustomerProducts
                .FirstOrDefaultAsync(cp =>
                    cp.CustomerId == customerId &&
                    cp.ProductId == draft.ProductId);

            if (existingCustomerProduct is not null)
            {
                // The customer already owns this product.
                // Mark this draft as completed instead of inserting a duplicate link.
                draft.Status = DraftStatus.SUBMITTED;
                draft.CurrentStep = DraftStep.SUBMITTED;
                draft.LastUpdatedAt = DateTime.Now;

                await _db.SaveChangesAsync();

                return new FinalizeApplicationResponse
                {
                    CustomerId = customerId,
                    CustomerProductId = existingCustomerProduct.CustomerProductId,
                    ProductAccountReference = GenerateAccountReference(draft.Product.ProductCode),
                    Status = existingCustomerProduct.Status.ToString()
                };
            }

            var customerProduct = new CustomerProduct
            {
                CustomerProductId = Guid.NewGuid(),
                CustomerId = customerId,
                ProductId = draft.ProductId,
                DraftId = draft.DraftId,
                Status = CustomerProductStatus.ACTIVE,
                CreatedAt = DateTime.Now
            };

            _db.CustomerProducts.Add(customerProduct);

            // 5. Actually provision the account in the right table, and log consent
            //    if this was an existing customer reusing their KYC data.
            if (draft.CustomerId != null)
            {
                await _consent.RecordConsentAsync(customerId, draft.ProductId, draft.Channel);
            }

            string productAccountRef = draft.Product.ProductCode switch
            {
                ProductCode.SAVINGS => (await _savings.CreateAsync(customerProduct.CustomerProductId, formData)).AccountNumber,
                ProductCode.CURRENT => (await _current.CreateAsync(customerProduct.CustomerProductId, formData)).AccountNumber,
                ProductCode.PENSION_RSA => (await _pension.CreateAsync(customerProduct.CustomerProductId, formData)).RsaPin,
                ProductCode.STOCKBROKING => (await _stockBroking.CreateAsync(customerProduct.CustomerProductId, formData)).CscsNumber,
                ProductCode.INSURANCE => "POL" + Random.Shared.Next(100_000_000, 999_999_999), // still a stub — no InsuranceAccountDetails table exists yet
                _ => Guid.NewGuid().ToString("N")[..10].ToUpper()
            };

            // 6. Mark draft as SUBMITTED
            draft.Status = DraftStatus.SUBMITTED;
            draft.CurrentStep = DraftStep.SUBMITTED;
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
            ProductCode.SAVINGS or
            ProductCode.CURRENT => "00" + Random.Shared.Next(10_000_000, 99_999_999),
            ProductCode.PENSION_RSA => "PEN" + Random.Shared.Next(100_000_000, 999_999_999),
            ProductCode.STOCKBROKING => "CSC" + Random.Shared.Next(100_000_000, 999_999_999),
            ProductCode.INSURANCE => "POL" + Random.Shared.Next(100_000_000, 999_999_999),
            _ => Guid.NewGuid().ToString("N")[..10].ToUpper()
        };

        private static Dictionary<string, object?> DeserializeFormData(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, object?>>(json) ?? new();
            }
            catch
            {
                return new();
            }
        }

        private static string GetString(Dictionary<string, object?> data, string key)
        {
            if (data.TryGetValue(key, out var v))
            {
                if (v is JsonElement je) return je.GetString() ?? string.Empty;
                return v?.ToString() ?? string.Empty;
            }
            return string.Empty;
        }

        private static string? GetStringOrNull(Dictionary<string, object?> data, string key)
        {
            if (data.TryGetValue(key, out var v))
            {
                if (v is JsonElement je) return je.GetString();
                return v?.ToString();
            }
            return null;
        }

        private static DateTime? ParseDate(Dictionary<string, object?> data, string key)
        {
            var raw = GetStringOrNull(data, key);
            return DateTime.TryParse(raw, out var result) ? result : null;
        }
    }
}
