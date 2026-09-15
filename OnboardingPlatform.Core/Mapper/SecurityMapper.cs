using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.Mapper
{
    public static class SecurityMapper
    {
        public static SecurityCheckResponse ToResponse(this SecurityCheck check)
        {
            return new SecurityCheckResponse
            {
                SecurityCheckId = check.SecurityCheckId,
                CheckType = check.CheckType.ToString(),
                Status = check.Status.ToString(),
                CompletedAt = check.CompletedAt
            };
        }
    }
}
