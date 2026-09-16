using OnboardingPlatform.Core.DTOs.Responses;
using System;
using System.Threading.Tasks;

namespace OnboardingPlatform.Services.Interfaces
{
    public interface ISavingsAccountService
    {
        Task<SavingsAccountDetailResponse> CreateAsync(Guid customerProductId, DraftFormData formData);
        Task<SavingsAccountDetailResponse?> GetByCustomerProductIdAsync(Guid customerProductId);
    }
}