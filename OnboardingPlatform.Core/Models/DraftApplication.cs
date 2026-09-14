using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.Models
{
    public class DraftApplication
    {
        [Key]
        public int DraftId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public string PrimaryIdentifierValueHash { get; set; }
        public string CurrentStep { get; set; } // e.g. PersonalInfo, EmploymentInfo, FinancialInfo, etc.
        public string FormDataJson { get; set; } // JSON representation of the form data for the current step
        public string Status { get; set; } //e.g., "InProgress", "Submitted", "Completed"
        public string Channel { get; set; } = "Web";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(10);
    }
}
