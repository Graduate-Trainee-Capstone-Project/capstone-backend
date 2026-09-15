using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.DTOs.Responses
{
    public class SecurityCheckResponse
    {
        public Guid SecurityCheckId { get; set; }
        public string CheckType { get; set; } = string.Empty; 
        public string Status { get; set; } = string.Empty; 
        public DateTime? CompletedAt { get; set; }
    }
}
