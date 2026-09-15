using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Core.Enums;

namespace OnboardingPlatform.Services.Interfaces
{
    public interface IConsentService
    {
        Task<ConsentResponse> RecordConsentAsync(Guid customerId, Guid productId, Channel channel);
        Task<List<ConsentResponse>> GetByCustomerAsync(Guid customerId);
    }
}
