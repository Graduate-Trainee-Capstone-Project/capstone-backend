using OnboardingPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.Models
{
    public class CustomerProduct
    {
        public Guid CustomerProductId { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? DraftId { get; set; }  // traceability back to the draft
        public CustomerProductStatus Status { get; set; } = CustomerProductStatus.ACTIVE;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public Customer Customer { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
