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
    public static class ConsentMapper
    {
        public static ConsentResponse ToResponse(this ConsentLog consent)
        {
            return new ConsentResponse
            {
                ConsentId = consent.ConsentId,
                CustomerId = consent.CustomerId,
                ProductId = consent.ProductId,
                ConsentType = consent.ConsentType,
                GrantedAt = consent.GrantedAt,
                Channel = consent.Channel.ToString()
            };
        }
    }


}
