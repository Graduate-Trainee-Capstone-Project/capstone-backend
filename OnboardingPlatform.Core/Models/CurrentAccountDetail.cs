using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.Models
{
    public class CurrentAccountDetail
    {
        [Key]
        public int CurrentAccountId { get; set; }

        [ForeignKey("CustomerProduct")]
        public int CustomerProductId { get; set; }
        public CustomerProduct CustomerProduct { get; set; }

        [StringLength(10)]
        public string AccountNumber { get; set; }

        public string Currency { get; set; } = "NGN"; // Default to Nigerian Naira
        public decimal Balance { get; set; }
        public bool CheckBookRequested { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Status { get; set; }
    }
}
