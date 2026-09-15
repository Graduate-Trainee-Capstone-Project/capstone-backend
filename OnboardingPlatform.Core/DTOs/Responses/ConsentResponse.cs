using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.DTOs.Responses
{
    public class ConsentResponse
    {
        public Guid ConsentId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public string ConsentType { get; set; } = string.Empty;
        public DateTime GrantedAt { get; set; }
        public string Channel { get; set; } = string.Empty;
    }
}
