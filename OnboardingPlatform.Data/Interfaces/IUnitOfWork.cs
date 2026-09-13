using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnboardingPlatform.Core.Models;

namespace OnboardingPlatform.Data.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> LogCustomerDetails(Customer request, Customer response);
    }
}
