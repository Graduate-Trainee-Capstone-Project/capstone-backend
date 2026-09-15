using Microsoft.Extensions.Logging;
using OnboardingPlatform.Data.Interfaces;
using OnboardingPlatform.Core.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace OnboardingPlatform.Data.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ILogger<UnitOfWork> _logger;
        private readonly AppDbContext _customerDb;

        public UnitOfWork(
            ILogger<UnitOfWork> logger,
            AppDbContext customerDb)
        {
            _logger = logger;
            _customerDb = customerDb;

        }

        public async Task<bool> LogCustomerDetails(Customer request, Customer response)
        {
            try
            {
                _logger.LogInformation("Logging customer details for FirstName: {FirstName} LastName: {LastName}", request.FirstName, request.LastName);

                var customer = new Customer
                {
                    FirstName = request.FirstName,
                    MiddleName = request.MiddleName,
                    LastName = request.LastName,


                    CreatedAt = DateTime.Now
                };

                _customerDb.Customers.Add(customer);
                await _customerDb.SaveChangesAsync();

                _logger.LogInformation("Successfully logged customer's details with FirstName: {FirstName} LastName: {LastName}", request.FirstName, request.LastName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging customer's details with FirstName: {FirstName} LastName: {LastName}", request.FirstName, request.LastName);
                return false;
            }
        }
    }
}
