using Microsoft.EntityFrameworkCore;
using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Core.Enums;
using OnboardingPlatform.Core.Models;
using OnboardingPlatform.Data.Implementations;
using OnboardingPlatform.Services.Interfaces;
using OnboardingPlatform.Core.Mappers;


namespace OnboardingPlatform.Services.Implementations
{
    public class SavingsAccountService : ISavingsAccountService
    {
        private readonly AppDbContext _context;
        private static readonly Random Rng = new();

        public SavingsAccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SavingsAccountDetailResponse> CreateAsync(Guid customerProductId, Dictionary<string, object?> formData)
        {
            var account = new SavingsAccountDetail
            {
                SavingsAccountId = Guid.NewGuid(),
                CustomerProductId = customerProductId,
                AccountNumber = "0" + Rng.Next(100_000_000, 999_999_999),
                Currency = "NGN",
                Balance = 0m,
                DateOpened = DateOnly.FromDateTime(DateTime.Now),
                Status = AccountStatus.Active
            };

            _context.SavingsAccountDetails.Add(account);
            await _context.SaveChangesAsync();

            return account.ToResponse();
        }

        public async Task<SavingsAccountDetailResponse?> GetByCustomerProductIdAsync(Guid customerProductId)
        {
            var account = await _context.SavingsAccountDetails
                .FirstOrDefaultAsync(s => s.CustomerProductId == customerProductId);

            return account?.ToResponse();
        }
    }
}
