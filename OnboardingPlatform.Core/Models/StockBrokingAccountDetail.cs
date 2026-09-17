using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using OnboardingPlatform.Core.Enums;

namespace OnboardingPlatform.Core.Models
{
    public class StockBrokingAccountDetail
    {
        [Key]
        public Guid StockBrokingAccountId { get; set; } = Guid.NewGuid();

        [ForeignKey("CustomerProduct")]
        public Guid CustomerProductId { get; set; }
        public CustomerProduct CustomerProduct { get; set; } = null!;
        public DateOnly DateOpened { get; set; }
        public AccountStatus Status { get; set; }
    }
}