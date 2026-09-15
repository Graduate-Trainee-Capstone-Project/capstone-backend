using Microsoft.EntityFrameworkCore;
using OnboardingPlatform.Core.DTOs.Requests;
using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Core.Enums;
using OnboardingPlatform.Core.Models;
using OnboardingPlatform.Data.Implementations;
using OnboardingPlatform.Services.Interfaces;
using OnboardingPlatform.Core.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Services.Implementations
{
    public class SecurityService : ISecurityService
    {
        private readonly AppDbContext _context;

        public SecurityService(AppDbContext context) => _context = context;

        public async Task<SecurityCheckResponse> PerformCheckAsync(Guid draftId, SecurityCheckRequest request)
        {
            var draftExists = await _context.DraftApplications.AnyAsync(d => d.DraftId == draftId);
            if (!draftExists) throw new KeyNotFoundException("Draft application not found.");

            if (!Enum.TryParse<SecurityCheckType>(request.CheckType, true, out var checkType))
                throw new ArgumentException($"Unknown check type '{request.CheckType}'.");

            var check = new SecurityCheck
            {
                SecurityCheckId = Guid.NewGuid(),
                DraftId = draftId,
                CheckType = checkType,
                Status = SecurityCheckStatus.PASSED, // mocked for the demo flow
                CreatedAt = DateTime.Now,
                CompletedAt = DateTime.Now
            };

            _context.SecurityChecks.Add(check);
            await _context.SaveChangesAsync();

            return check.ToResponse();
        }

        public async Task<List<SecurityCheckResponse>> GetChecksForDraftAsync(Guid draftId)
        {
            return await _context.SecurityChecks
                .Where(s => s.DraftId == draftId)
                .Select(s => s.ToResponse())
                .ToListAsync();
        }
    }
}
