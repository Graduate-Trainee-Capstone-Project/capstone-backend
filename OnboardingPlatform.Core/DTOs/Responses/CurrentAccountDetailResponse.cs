using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.DTOs.Responses
{
    public class CurrentAccountDetailResponse
    {
        public Guid CurrentAccountId { get; set; }
        public Guid CustomerProductId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public bool CheckBookRequested { get; set; }
        public DateOnly DateOpened { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
