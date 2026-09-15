using OnboardingPlatform.Core.Models;

namespace OnboardingPlatform.API.Mappers
{
    public static class CustomerMapper
    {
        public static object ToSummary(Customer customer) => new
        {
            customer.CustomerId,
            customer.FirstName,
            customer.MiddleName,
            customer.LastName,
            customer.DateOfBirth,
            customer.Gender,
            customer.Nationality,
            customer.PhoneNumber,
            customer.Email,
            customer.Status,
            Addresses = customer.Addresses.Select(a => new
            {
                a.Street,
                a.City,
                a.State,
                a.Country
            })
        };
    }
}
