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
        public int PensionAccountId { get; set; }

        [ForeignKey("CustomerProduct")]
        public int CustomerProductId { get; set; }
        public CustomerProduct CustomerProduct { get; set; }

        public string RsaPin { get; set; } //mock generated for demo

        public string PfaName { get; set; } = "Stanbic IBTC Pension Managers Limited";

        public string EmployerName { get; set; }

        public string ContributionScheme { get; set; } // "MandatoryCPS", "Voluntary", "MicroPensionPlan"

        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Active";
    }
}
