using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.Models
{
    public class SecurityCheck
    {
        [Key]
        public int SecurityCheckId { get; set; }

        [ForeignKey("DraftApplication")]
        public int DraftId { get; set; }
        public DraftApplication DraftApplication { get; set; }

        public string CheckType { get; set; } // e.g., "IDENTITY_CHECK", "AML_CHECK"

        public string Status { get; set; } = "PENDING"; // e.g., "PENDING", "PASSED", "FAILED"

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }
    }
}
