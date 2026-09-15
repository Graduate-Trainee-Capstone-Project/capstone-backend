using OnboardingPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.Models
{
    public class Product
    {
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public ProductCode ProductCode { get; set; }
        public string ProductName { get; set; } = string.Empty;

        // Stored as JSON array e.g. ["BVN"] or ["NIN","PHONE"]
        public string RequiredIdentifiers { get; set; } = "[]";

        // Lightweight JSON schema for PRODUCT_SPECIFIC_INFO step
        public string? AdditionalFieldsSchema { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<CustomerProduct> CustomerProducts { get; set; } = new List<CustomerProduct>();
    }
}
