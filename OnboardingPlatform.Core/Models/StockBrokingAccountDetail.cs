using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [StringLength(20)]
        public string CscsNumber { get; set; } = string.Empty; // Central Securities Clearing System number

        public string BrokerageFirm { get; set; } = "Stanbic IBTC Stockbrokers Limited";

        [StringLength(20)]
        public string TradingAccountNumber { get; set; } = string.Empty;

        public DateOnly DateOpened { get; set; }
        public AccountStatus Status { get; set; }
    }
}