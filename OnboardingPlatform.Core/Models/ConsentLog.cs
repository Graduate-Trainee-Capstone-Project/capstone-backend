using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.Models
{
    public class ConsentLog
    {
        [Key]
        public int ConsentId { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string ConsentType { get; set; } = "REUSE_KYC_DATA";

        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;

        public string Channel { get; set; } = "Web";
    }
}
