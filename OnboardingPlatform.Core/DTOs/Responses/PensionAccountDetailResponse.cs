using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.DTOs.Responses
{
    public class PensionAccountDetailResponse
    {
        public Guid PensionAccountId { get; set; }
        public Guid CustomerProductId { get; set; }
        public string RsaPin { get; set; } = string.Empty;
        public string PfaName { get; set; } = string.Empty;
        public string? EmployerName { get; set; }
        public string ContributionScheme { get; set; } = string.Empty;
        public DateOnly DateRegistered { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
