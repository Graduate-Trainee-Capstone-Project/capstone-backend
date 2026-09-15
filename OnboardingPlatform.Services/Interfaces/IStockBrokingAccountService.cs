using OnboardingPlatform.Core.DTOs.Responses;

namespace OnboardingPlatform.Services.Interfaces
{
    public interface IStockBrokingAccountService
    {
        Task<StockBrokingAccountDetailResponse> CreateAsync(Guid customerProductId, Dictionary<string, object?> formData);
        Task<StockBrokingAccountDetailResponse?> GetByCustomerProductIdAsync(Guid customerProductId);
    }
}