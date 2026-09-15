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
    public static class CurrentAccountMappers
    {
        public static CurrentAccountDetailResponse ToResponse(this CurrentAccountDetail c) => new()
        {
            CurrentAccountId = c.CurrentAccountId,
            CustomerProductId = c.CustomerProductId,
            AccountNumber = c.AccountNumber,
            Currency = c.Currency,
            Balance = c.Balance,
            CheckBookRequested = c.CheckBookRequested,
            DateOpened = c.DateOpened,
            Status = c.Status.ToString()
        };
    }
}
