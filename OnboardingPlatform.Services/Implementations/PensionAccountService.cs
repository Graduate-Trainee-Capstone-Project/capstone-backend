using Microsoft.EntityFrameworkCore;
using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Core.Enums;
using OnboardingPlatform.Core.Mappers;
using OnboardingPlatform.Core.Models;
using OnboardingPlatform.Data.Implementations;
using OnboardingPlatform.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OnboardingPlatform.Services.Implementations
{
    public class PensionAccountService : IPensionAccountService
    {
        private readonly AppDbContext _context;
        private static readonly Random Rng = new();

        public PensionAccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PensionAccountDetailResponse> CreateAsync(Guid customerProductId, DraftFormData formData)
        {
            // NOTE: DraftFormData has no ContributionScheme/EmployerName fields yet —
            // defaulting for now. Add them to DraftFormData later if this product needs them.
            var scheme = ContributionScheme.MandatoryCPS;
            string? employerName = null;

            var account = new PensionAccountDetail
            {
                PensionAccountId = Guid.NewGuid(),
                CustomerProductId = customerProductId,
                RsaPin = "PEN" + Rng.Next(10_000_000, 99_999_999),
                PfaName = "Stanbic IBTC Pension Managers Limited",
                EmployerName = employerName,
                ContributionScheme = scheme,
                DateRegistered = DateOnly.FromDateTime(DateTime.Now),
                Status = AccountStatus.Active
            };

            _context.PensionAccountDetails.Add(account);
            await _context.SaveChangesAsync();

            return account.ToResponse();
        }

        public async Task<PensionAccountDetailResponse?> GetByCustomerProductIdAsync(Guid customerProductId)
        {
            var account = await _context.PensionAccountDetails
                .FirstOrDefaultAsync(p => p.CustomerProductId == customerProductId);

            return account?.ToResponse();
        }
    }
}