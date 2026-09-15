using OnboardingPlatform.Core.DTOs.Requests;
using OnboardingPlatform.Core.DTOs.Responses;

namespace OnboardingPlatform.Services.Interfaces
{
    public interface IDraftService
    {
        Task<SaveDraftResponse> SaveAsync(Guid draftId, SaveDraftRequest request);
        Task<DraftApplicationResponse?> GetByIdAsync(Guid draftId);
    }
}
