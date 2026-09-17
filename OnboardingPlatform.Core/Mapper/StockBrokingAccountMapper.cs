using Microsoft.AspNetCore.Http;
using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Core.Models;

namespace OnboardingPlatform.Core.Mappers
{
    public static class StockBrokingAccountMapper
    {
        public static StockBrokingAccountDetailResponse ToResponse(this StockBrokingAccountDetail s) => new()
        {
            StockBrokingAccountId = s.StockBrokingAccountId,
            CustomerProductId = s.CustomerProductId,
            DateOpened = s.DateOpened,
            Status = s.Status.ToString()
        };
    }
}