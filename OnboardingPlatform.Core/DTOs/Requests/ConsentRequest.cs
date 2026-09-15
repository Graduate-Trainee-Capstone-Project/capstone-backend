using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.DTOs.Requests
{
    public class ConsentRequest
    {
        public Guid ProductId { get; set; }
        public string ConsentType { get; set; } = "REUSE_KYC_DATA";
        public string Channel { get; set; } = string.Empty;
    }
}
