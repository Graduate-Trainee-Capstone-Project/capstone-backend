using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OnboardingPlatform.Core.DTOs.Responses
{
    public class StockBrokingAccountDetailResponse
    {
        public Guid StockBrokingAccountId { get; set; }
        public Guid CustomerProductId { get; set; }
        public DateOnly DateOpened { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}