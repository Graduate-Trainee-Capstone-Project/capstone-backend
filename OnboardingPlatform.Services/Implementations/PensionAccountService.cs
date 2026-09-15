using Microsoft.EntityFrameworkCore;
using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Core.Enums;
using OnboardingPlatform.Core.Mappers;
using OnboardingPlatform.Core.Models;
using OnboardingPlatform.Data.Implementations;
using OnboardingPlatform.Services.Interfaces;

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

        public async Task<PensionAccountDetailResponse> CreateAsync(Guid customerProductId, Dictionary<string, object?> formData)
        {
            var schemeRaw = formData.TryGetValue("contributionScheme", out var sch) ? sch?.ToString() : "MandatoryCPS";
            Enum.TryParse<ContributionScheme>(schemeRaw, true, out var scheme);

            var account = new PensionAccountDetail
            {
                PensionAccountId = Guid.NewGuid(),
                CustomerProductId = customerProductId,
                RsaPin = "PEN" + Rng.Next(10_000_000, 99_999_999),
                PfaName = "Stanbic IBTC Pension Managers Limited",
                EmployerName = formData.TryGetValue("employerName", out var emp) ? emp?.ToString() : null,
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