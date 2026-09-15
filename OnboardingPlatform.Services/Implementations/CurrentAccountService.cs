using Microsoft.EntityFrameworkCore;
using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Core.Enums;
using OnboardingPlatform.Core.Mappers;
using OnboardingPlatform.Core.Models;
using OnboardingPlatform.Data.Implementations;
using OnboardingPlatform.Services.Interfaces;

namespace OnboardingPlatform.Services.Implementations
{
    public class CurrentAccountService : ICurrentAccountService
    {
        private readonly AppDbContext _context;
        private static readonly Random Rng = new();

        public CurrentAccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CurrentAccountDetailResponse> CreateAsync(Guid customerProductId, Dictionary<string, object?> formData)
        {
            var account = new CurrentAccountDetail
            {
                CurrentAccountId = Guid.NewGuid(),
                CustomerProductId = customerProductId,
                AccountNumber = "0" + Rng.Next(100_000_000, 999_999_999),
                Currency = "NGN",
                Balance = 0m,
                CheckBookRequested = formData.TryGetValue("checkBookRequested", out var cb) && cb is bool b && b,
                DateOpened = DateOnly.FromDateTime(DateTime.Now),
                Status = AccountStatus.Active
            };

            _context.CurrentAccountDetails.Add(account);
            await _context.SaveChangesAsync();

            return account.ToResponse();
        }

        public async Task<CurrentAccountDetailResponse?> GetByCustomerProductIdAsync(Guid customerProductId)
        {
            var account = await _context.CurrentAccountDetails
                .FirstOrDefaultAsync(c => c.CustomerProductId == customerProductId);

            return account?.ToResponse();
        }
    }
}