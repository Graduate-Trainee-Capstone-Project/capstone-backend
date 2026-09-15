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
    public class PensionAccountDetail
    {
        [Key]
        public Guid PensionAccountId { get; set; } = Guid.NewGuid();

        [ForeignKey("CustomerProduct")]
        public Guid CustomerProductId { get; set; }
        public CustomerProduct CustomerProduct { get; set; } = null!;

        public string RsaPin { get; set; } = string.Empty; //mock generated for demo

        public string PfaName { get; set; } = "Stanbic IBTC Pension Managers Limited";

        public string? EmployerName { get; set; }

        public ContributionScheme ContributionScheme { get; set; }

        public DateOnly DateRegistered { get; set; }

        public AccountStatus Status { get; set; }
    }
}
