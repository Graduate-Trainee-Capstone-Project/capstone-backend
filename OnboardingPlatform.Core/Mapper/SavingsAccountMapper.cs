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
    public static class SavingsAccountMapper
    {
        public static SavingsAccountDetailResponse ToResponse(this SavingsAccountDetail s) => new()
        {
            SavingsAccountId = s.SavingsAccountId,
            CustomerProductId = s.CustomerProductId,
            AccountNumber = s.AccountNumber,
            Currency = s.Currency,
            Balance = s.Balance,
            DateOpened = s.DateOpened,
            Status = s.Status.ToString()
        };
    }
}
