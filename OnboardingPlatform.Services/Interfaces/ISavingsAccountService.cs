using OnboardingPlatform.Core.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Services.Interfaces
{
    public interface ISavingsAccountService
    {
        Task<SavingsAccountDetailResponse> CreateAsync(Guid customerProductId, Dictionary<string, object?> formData);
        Task<SavingsAccountDetailResponse?> GetByCustomerProductIdAsync(Guid customerProductId);
    }
}
