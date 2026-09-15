using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.DTOs.Requests
{
    public class SecurityCheckRequest
    {
        public string CheckType { get; set; } = string.Empty; // "SECURITY_QUESTION" | "FACIAL_RECOGNITION" | "OTP"
    }
}
