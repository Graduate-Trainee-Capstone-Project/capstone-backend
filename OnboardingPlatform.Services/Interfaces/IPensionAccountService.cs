using OnboardingPlatform.Core.DTOs.Responses;
using System;
using System.Threading.Tasks;

namespace OnboardingPlatform.Services.Interfaces
{
    public interface IPensionAccountService
    {
        Task<PensionAccountDetailResponse> CreateAsync(Guid customerProductId, DraftFormData formData);
        Task<PensionAccountDetailResponse?> GetByCustomerProductIdAsync(Guid customerProductId);
    }
}