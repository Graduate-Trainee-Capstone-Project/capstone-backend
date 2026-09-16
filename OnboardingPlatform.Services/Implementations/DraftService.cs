using Microsoft.EntityFrameworkCore;
using OnboardingPlatform.Core.DTOs.Requests;
using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Core.Mappers;
using OnboardingPlatform.Data.Implementations;
using OnboardingPlatform.Services.Interfaces;

namespace OnboardingPlatform.Services.Implementations
{
    public class DraftService : IDraftService
    {
        private readonly AppDbContext _context;

        public DraftService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SaveDraftResponse> SaveAsync(Guid draftId, InternalSaveDraftRequest request)
        {
            var draft = await _context.DraftApplications.FindAsync(draftId)
                ?? throw new KeyNotFoundException("Draft application not found.");

            draft.ApplySave(request);
            await _context.SaveChangesAsync();

            return draft.ToSaveResponse();
        }
        public async Task<DraftApplicationResponse?> GetByIdAsync(Guid draftId)
        {
            var draft = await _context.DraftApplications
                .Include(d => d.Product)
                .FirstOrDefaultAsync(d => d.DraftId == draftId);

            return draft?.ToResponse();
        }
    }
}