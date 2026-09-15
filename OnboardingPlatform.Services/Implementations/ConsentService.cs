using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Core.Enums;
using OnboardingPlatform.Core.Models;
using OnboardingPlatform.Data.Implementations;
using OnboardingPlatform.Services.Interfaces;
using OnboardingPlatform.Core.Mapper;
using Microsoft.EntityFrameworkCore;

namespace OnboardingPlatform.Services.Implementations
{
    public class ConsentService : IConsentService
    {
        private readonly AppDbContext _context;

        public ConsentService(AppDbContext context) => _context = context;

        public Task RecordConsentAsync(Guid customerId, Guid productId, Channel channel)
        {
            _context.ConsentLogs.Add(new ConsentLog
            {
                ConsentId = Guid.NewGuid(),
                CustomerId = customerId,
                ProductId = productId,
                ConsentType = "REUSE_KYC_DATA",
                GrantedAt = DateTime.Now,
                Channel = channel
            });
            return Task.CompletedTask;
        }

        public async Task<List<ConsentResponse>> GetByCustomerAsync(Guid customerId)
        {
            return await _context.ConsentLogs
                .Where(c => c.CustomerId == customerId)
                .Select(c => c.ToResponse())
                .ToListAsync();
        }
    }
}
