using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnboardingPlatform.Core.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.Models
{
    public class SecurityCheck
    {
        [Key]
        public Guid SecurityCheckId { get; set; }

        [ForeignKey("DraftApplication")]
        public Guid DraftId { get; set; }
        public DraftApplication DraftApplication { get; set; } = null!;

        public SecurityCheckType CheckType { get; set; } 

        public SecurityCheckStatus Status { get; set; } 

        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
