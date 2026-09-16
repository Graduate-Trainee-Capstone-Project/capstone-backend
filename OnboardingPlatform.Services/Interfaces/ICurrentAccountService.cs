using OnboardingPlatform.Core.DTOs.Responses;
using System;
using System.Threading.Tasks;

namespace OnboardingPlatform.Services.Interfaces
{
    public interface ICurrentAccountService
    {
        Task<CurrentAccountDetailResponse> CreateAsync(Guid customerProductId, DraftFormData formData);
        Task<CurrentAccountDetailResponse?> GetByCustomerProductIdAsync(Guid customerProductId);
    }
}