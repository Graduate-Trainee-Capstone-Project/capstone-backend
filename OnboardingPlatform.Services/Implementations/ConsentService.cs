using Microsoft.EntityFrameworkCore;
using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Core.Enums;
using OnboardingPlatform.Core.Mapper;
using OnboardingPlatform.Core.Models;
using OnboardingPlatform.Data.Implementations;
using OnboardingPlatform.Services.Interfaces;

namespace OnboardingPlatform.Services.Implementations
{
    public class ConsentService : IConsentService
    {
        private readonly AppDbContext _context;

        public ConsentService(AppDbContext context) => _context = context;

        public async Task<ConsentResponse> RecordConsentAsync(Guid customerId, Guid productId, Channel channel)
        {
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == customerId);
            if (!customerExists)
                throw new KeyNotFoundException($"Customer '{customerId}' was not found.");

            var productExists = await _context.Products.AnyAsync(p => p.ProductId == productId);
            if (!productExists)
                throw new KeyNotFoundException($"Product '{productId}' was not found.");

            var consent = new ConsentLog
            {
                ConsentId = Guid.NewGuid(),
                CustomerId = customerId,
                ProductId = productId,
                ConsentType = "REUSE_KYC_DATA",
                GrantedAt = DateTime.Now,
                Channel = channel
            };

            _context.ConsentLogs.Add(consent);
            await _context.SaveChangesAsync();

            return consent.ToResponse();
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
