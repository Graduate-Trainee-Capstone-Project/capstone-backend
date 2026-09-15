using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.Mappers
{
    public static class PensionAccountMapper
    {
        public static PensionAccountDetailResponse ToResponse(this PensionAccountDetail p) => new()
        {
            PensionAccountId = p.PensionAccountId,
            CustomerProductId = p.CustomerProductId,
            RsaPin = p.RsaPin,
            PfaName = p.PfaName,
            EmployerName = p.EmployerName,
            ContributionScheme = p.ContributionScheme.ToString(),
            DateRegistered = p.DateRegistered,
            Status = p.Status.ToString()
        };
    }
}
