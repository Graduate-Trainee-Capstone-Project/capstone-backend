using Microsoft.EntityFrameworkCore;
using OnboardingPlatform.Core.DTOs.Requests;
using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Core.Enums;
using OnboardingPlatform.Core.Models;
using OnboardingPlatform.Data.Implementations;
using OnboardingPlatform.Services.Interfaces;
using OnboardingPlatform.Core.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    FormData = DraftApplicationMapper.DeserializeFormData(existingDraft.FormDataJson),
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
                FormData = new DraftFormData(),   // fixed — was crashing on existingDraft.FormDataJson
                ExistingCustomer = existingCustomerData
            };
        }

        // ──────────────────────────────────────────────
        // POST /applications/{draftId}/finalize
        // ──────────────────────────────────────────────
        public async Task<FinalizeApplicationResponse> FinalizeApplicationAsync(Guid draftId)
        {
            // 1. Load the draft with all necessary relationships
            var draft = await _db.DraftApplications
                .Include(d => d.Product)
                .FirstOrDefaultAsync(d => d.DraftId == draftId)
                ?? throw new InvalidOperationException($"Draft '{draftId}' not found.");

            if (draft.Status != DraftStatus.IN_PROGRESS)
            {
                throw new InvalidOperationException($"Draft '{draftId}' is not in progress.");
            }

            var formData = DeserializeFormData(draft.FormDataJson) ?? new();
            Guid customerId;

            // 2. NEW CUSTOMER PATH or EXISTING CUSTOMER PATH
            if (!draft.CustomerId.HasValue)
            {
                // ── NEW CUSTOMER PATH ────────────────────────────────
                var customer = new Customer
                {
                    CustomerId = Guid.NewGuid(),
                    FirstName = GetString(formData, "firstName"),
                    MiddleName = GetStringOrNull(formData, "middleName"),
                    LastName = GetString(formData, "lastName"),
                    DateOfBirth = ParseDate(formData, "dateOfBirth"),
                    Gender = GetString(formData, "gender"),
                    Nationality = GetStringOrNull(formData, "nationality") ?? "Nigerian",
                    PhoneNumber = GetStringOrNull(formData, "phoneNumber"),
                    Email = GetStringOrNull(formData, "email"),
                    Status = CustomerStatus.ACTIVE,
                    CreatedAt = DateTime.Now
                };

                _db.Customers.Add(customer);

                AddCustomerAddress(
                    customer.CustomerId,
                    formData,
                    isPrimary: true);

                // Save the PRIMARY identifier (e.g. BVN) — required so future
                // /applications/start calls with this BVN can find this customer.
                var bvnExists = await _db.CustomerIdentifiers.AnyAsync(i =>
                    i.IdentifierType == draft.PrimaryIdentifierType &&
                    i.IdentifierValueHash == draft.PrimaryIdentifierValueHash);

                if (!bvnExists)
                {
                    _db.CustomerIdentifiers.Add(new CustomerIdentifier
                    {
                        IdentifierId = Guid.NewGuid(),
                        CustomerId = customer.CustomerId,
                        IdentifierType = draft.PrimaryIdentifierType,
                        IdentifierValueHash = draft.PrimaryIdentifierValueHash,
                        IdentifierValueMasked = "****" + draft.PrimaryIdentifierValueHash[^4..],
                        IsVerified = true,
                        CreatedAt = DateTime.Now
                    });
                }

                // Save the SECONDARY identifier if this product required one (e.g. NIN+PHONE products)
                if (draft.SecondaryIdentifierType.HasValue && !string.IsNullOrEmpty(draft.SecondaryIdentifierValueHash))
                {
                    _db.CustomerIdentifiers.Add(new CustomerIdentifier
                    {
                        IdentifierId = Guid.NewGuid(),
                        CustomerId = customer.CustomerId,
                        IdentifierType = draft.SecondaryIdentifierType.Value,
                        IdentifierValueHash = draft.SecondaryIdentifierValueHash,
                        IdentifierValueMasked = "****" + draft.SecondaryIdentifierValueHash[^4..],
                        IsVerified = true,
                        CreatedAt = DateTime.Now
                    });
                }

                if (!string.IsNullOrEmpty(customer.PhoneNumber))
                {
                    var phoneHash = _identity.HashIdentifier(customer.PhoneNumber);
                    var phoneExists = await _db.CustomerIdentifiers.AnyAsync(i =>
                        i.IdentifierType == IdentifierType.PHONE &&
                        i.IdentifierValueHash == phoneHash);

                    if (!phoneExists)
                    {
                        _db.CustomerIdentifiers.Add(new CustomerIdentifier
                        {
                            IdentifierId = Guid.NewGuid(),
                            CustomerId = customer.CustomerId,
                            IdentifierType = IdentifierType.PHONE,
                            IdentifierValueHash = phoneHash,
                            IdentifierValueMasked = _identity.MaskIdentifier(customer.PhoneNumber),
                            IsVerified = false,
                            CreatedAt = DateTime.Now
                        });
                    }
                }

                if (!string.IsNullOrEmpty(customer.Email))
                {
                    var emailHash = _identity.HashIdentifier(customer.Email);
                    var emailExists = await _db.CustomerIdentifiers.AnyAsync(i =>
                        i.IdentifierType == IdentifierType.EMAIL &&
                        i.IdentifierValueHash == emailHash);

                    if (!emailExists)
                    {
                        _db.CustomerIdentifiers.Add(new CustomerIdentifier
                        {
                            IdentifierId = Guid.NewGuid(),
                            CustomerId = customer.CustomerId,
                            IdentifierType = IdentifierType.EMAIL,
                            IdentifierValueHash = emailHash,
                            IdentifierValueMasked = _identity.MaskIdentifier(customer.Email),
                            IsVerified = false,
                            CreatedAt = DateTime.Now
                        });
                    }
                }

                customerId = customer.CustomerId;
                draft.CustomerId = customerId;

                await _db.SaveChangesAsync();
            }
            else
            {
                // ── EXISTING CUSTOMER PATH ────────────────────────────────
                customerId = draft.CustomerId.Value;

                var existingCustomer = await _db.Customers
                    .Include(c => c.Addresses)
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId);

                if (existingCustomer != null)
                {
                    existingCustomer.UpdatedAt = DateTime.Now;

                    if (formData.Address != null && formData.Address.Count > 0)
                    {
                        var addr = formData.Address[0];

                        if (!string.IsNullOrEmpty(addr.Street))
                        {
                            var hasSimilarAddress = existingCustomer.Addresses.Any(a =>
                                a.Street == addr.Street &&
                                a.City == addr.City);

                            if (!hasSimilarAddress)
                            {
                                _db.CustomerAddresses.Add(new CustomerAddress
                                {
                                    AddressId = Guid.NewGuid(),
                                    CustomerId = customerId,
                                    HouseNumber = addr.HouseNumber,
                                    Street = addr.Street,
                                    City = addr.City ?? string.Empty,
                                    State = addr.State ?? string.Empty,
                                    Country = string.IsNullOrWhiteSpace(addr.Country) ? "Nigeria" : addr.Country,
                                    IsPrimary = false,
                                    CreatedAt = DateTime.Now
                                });
                            }
                        }
                    }

                    await _db.SaveChangesAsync();
                }
                else
                {
                    // Draft points at a CustomerId that doesn't exist yet — treat as new customer,
                    // creating them under the SAME CustomerId the draft already references.
                    var customer = new Customer
                    {
                        CustomerId = customerId,
                        FirstName = GetString(formData, "firstName"),
                        MiddleName = GetStringOrNull(formData, "middleName"),
                        LastName = GetString(formData, "lastName"),
                        DateOfBirth = ParseDate(formData, "dateOfBirth"),
                        Gender = GetString(formData, "gender"),
                        Nationality = GetStringOrNull(formData, "nationality") ?? "Nigerian",
                        PhoneNumber = GetStringOrNull(formData, "phoneNumber"),
                        Email = GetStringOrNull(formData, "email"),
                        Status = CustomerStatus.ACTIVE,
                        CreatedAt = DateTime.Now
                    };

                    _db.Customers.Add(customer);

                    AddCustomerAddress(customer.CustomerId, formData, isPrimary: true);

                    _db.CustomerIdentifiers.Add(new CustomerIdentifier
                    {
                        IdentifierId = Guid.NewGuid(),
                        CustomerId = customer.CustomerId,
                        IdentifierType = draft.PrimaryIdentifierType,
                        IdentifierValueHash = draft.PrimaryIdentifierValueHash,
                        IdentifierValueMasked = "****" + draft.PrimaryIdentifierValueHash[^4..],
                        IsVerified = true,
                        CreatedAt = DateTime.Now
                    });

                    if (draft.SecondaryIdentifierType.HasValue && !string.IsNullOrEmpty(draft.SecondaryIdentifierValueHash))
                    {
                        _db.CustomerIdentifiers.Add(new CustomerIdentifier
                        {
                            IdentifierId = Guid.NewGuid(),
                            CustomerId = customer.CustomerId,
                            IdentifierType = draft.SecondaryIdentifierType.Value,
                            IdentifierValueHash = draft.SecondaryIdentifierValueHash,
                            IdentifierValueMasked = "****" + draft.SecondaryIdentifierValueHash[^4..],
                            IsVerified = true,
                            CreatedAt = DateTime.Now
                        });
                    }

                    if (!string.IsNullOrEmpty(customer.PhoneNumber))
                    {
                        _db.CustomerIdentifiers.Add(new CustomerIdentifier
                        {
                            IdentifierId = Guid.NewGuid(),
                            CustomerId = customer.CustomerId,
                            IdentifierType = IdentifierType.PHONE,
                            IdentifierValueHash = _identity.HashIdentifier(customer.PhoneNumber),
                            IdentifierValueMasked = _identity.MaskIdentifier(customer.PhoneNumber),
                            IsVerified = false,
                            CreatedAt = DateTime.Now
                        });
                    }

                    if (!string.IsNullOrEmpty(customer.Email))
                    {
                        _db.CustomerIdentifiers.Add(new CustomerIdentifier
                        {
                            IdentifierId = Guid.NewGuid(),
                            CustomerId = customer.CustomerId,
                            IdentifierType = IdentifierType.EMAIL,
                            IdentifierValueHash = _identity.HashIdentifier(customer.Email),
                            IdentifierValueMasked = _identity.MaskIdentifier(customer.Email),
                            IsVerified = false,
                            CreatedAt = DateTime.Now
                        });
                    }

                    await _db.SaveChangesAsync();
                }
            }

            // 4. Create or reuse the CustomerProduct link
            var existingCustomerProduct = await _db.CustomerProducts
                .FirstOrDefaultAsync(cp =>
                    cp.CustomerId == customerId &&
                    cp.ProductId == draft.ProductId);

            if (existingCustomerProduct is not null)
            {
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
            await _db.SaveChangesAsync();

            // 5. Record consent if applicable
            if (draft.CustomerId != null)
            {
                await _consent.RecordConsentAsync(customerId, draft.ProductId, draft.Channel);
            }

            // 6. Provision the account based on product type
            string productAccountRef = draft.Product.ProductCode switch
            {
                ProductCode.SAVINGS => (await _savings.CreateAsync(customerProduct.CustomerProductId, formData)).AccountNumber,
                ProductCode.CURRENT => (await _current.CreateAsync(customerProduct.CustomerProductId, formData)).AccountNumber,
                ProductCode.PENSION_RSA => (await _pension.CreateAsync(customerProduct.CustomerProductId, formData)).RsaPin,
                ProductCode.STOCKBROKING => (await _stockBroking.CreateAsync(customerProduct.CustomerProductId, formData)).StockBrokingAccountId.ToString(),
                ProductCode.INSURANCE => "POL" + Random.Shared.Next(100_000_000, 999_999_999),
                _ => Guid.NewGuid().ToString("N")[..10].ToUpper()
            };

            // 7. Mark draft as SUBMITTED
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

        private static OnboardingPlatform.Core.DTOs.Responses.DraftFormData DeserializeFormData(string json)
            => OnboardingPlatform.Core.Mappers.DraftApplicationMapper.DeserializeFormData(json);

        private static string GetString(OnboardingPlatform.Core.DTOs.Responses.DraftFormData data, string key) => key switch
        {
            "firstName" => data.FirstName ?? string.Empty,
            "lastName" => data.LastName ?? string.Empty,
            "gender" => data.Gender ?? string.Empty,
            _ => string.Empty
        };

        private static string? GetStringOrNull(OnboardingPlatform.Core.DTOs.Responses.DraftFormData data, string key) => key switch
        {
            "middleName" => data.MiddleName,
            "phoneNumber" => data.PhoneNumber,
            "email" => data.Email,
            _ => null
        };

        private static DateTime? ParseDate(OnboardingPlatform.Core.DTOs.Responses.DraftFormData data, string key)
            => key == "dateOfBirth" ? data.DateOfBirth : null;

        private void AddCustomerAddress(Guid customerId, OnboardingPlatform.Core.DTOs.Responses.DraftFormData formData, bool isPrimary)
        {
            if (formData.Address == null || formData.Address.Count == 0) return;

            var address = formData.Address[0];
            if (string.IsNullOrWhiteSpace(address.Street)) return;

            _db.CustomerAddresses.Add(new CustomerAddress
            {
                AddressId = Guid.NewGuid(),
                CustomerId = customerId,
                HouseNumber = address.HouseNumber,
                Street = address.Street,
                City = address.City ?? string.Empty,
                State = address.State ?? string.Empty,
                Country = string.IsNullOrWhiteSpace(address.Country) ? "Nigeria" : address.Country,
                IsPrimary = isPrimary,
                CreatedAt = DateTime.Now
            });
        }
    }
}
