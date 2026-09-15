using OnboardingPlatform.Core.Enums;
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
        public Guid CurrentAccountId { get; set; } = Guid.NewGuid();

        [ForeignKey("CustomerProduct")]
        public Guid CustomerProductId { get; set; }
        public CustomerProduct CustomerProduct { get; set; } = null!;

        [StringLength(10)]
        public string AccountNumber { get; set; } = string.Empty; // string(10)

        public string Currency { get; set; } = "NGN"; // Default to Nigerian Naira
        public decimal Balance { get; set; } = 0m;
        public bool CheckBookRequested { get; set; }
        public DateOnly DateOpened { get; set; }
        public AccountStatus Status { get; set; }
    }
}
