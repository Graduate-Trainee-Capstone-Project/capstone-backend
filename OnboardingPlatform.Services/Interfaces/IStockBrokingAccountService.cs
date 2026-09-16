using OnboardingPlatform.Core.DTOs.Responses;
using System;
using System.Threading.Tasks;

namespace OnboardingPlatform.Services.Interfaces
{
    public interface IStockBrokingAccountService
    {
        Task<StockBrokingAccountDetailResponse> CreateAsync(Guid customerProductId, DraftFormData formData);
        Task<StockBrokingAccountDetailResponse?> GetByCustomerProductIdAsync(Guid customerProductId);
    }
}