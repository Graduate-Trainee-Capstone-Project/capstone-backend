using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.Models
{
    public class CustomerAddress
    {
        public Guid AddressId { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public string? HouseNumber { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = "Nigeria";
        public bool IsPrimary { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        //public Customer Customer { get; set; } = null!;
    }
}
