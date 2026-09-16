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
    public class StockBrokingAccountService : IStockBrokingAccountService
    {
        private readonly AppDbContext _context;
        private static readonly Random Rng = new();

        public StockBrokingAccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StockBrokingAccountDetailResponse> CreateAsync(Guid customerProductId, DraftFormData formData)
        {
            var account = new StockBrokingAccountDetail
            {
                StockBrokingAccountId = Guid.NewGuid(),
                CustomerProductId = customerProductId,
                CscsNumber = "CSC" + Rng.Next(100_000_000, 999_999_999),
                BrokerageFirm = "Stanbic IBTC Stockbrokers Limited",
                TradingAccountNumber = "TR" + Rng.Next(1_000_000, 9_999_999),
                DateOpened = DateOnly.FromDateTime(DateTime.Now),
                Status = AccountStatus.Active
            };

            _context.StockBrokingAccountDetails.Add(account);
            await _context.SaveChangesAsync();

            return account.ToResponse();
        }

        public async Task<StockBrokingAccountDetailResponse?> GetByCustomerProductIdAsync(Guid customerProductId)
        {
            var account = await _context.StockBrokingAccountDetails
                .FirstOrDefaultAsync(s => s.CustomerProductId == customerProductId);

            return account?.ToResponse();
        }
    }
}