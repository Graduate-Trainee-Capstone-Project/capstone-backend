using Microsoft.AspNetCore.Http.HttpResults;
using OnboardingPlatform.Core.Enums;
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
        public Guid DraftId { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public Guid? CustomerId { get; set; }
        public IdentifierType PrimaryIdentifierType { get; set; }
        public string PrimaryIdentifierValueHash { get; set; } = string.Empty;
        public IdentifierType? SecondaryIdentifierType { get; set; }
        public string? SecondaryIdentifierValueHash { get; set; }
        public DraftStep CurrentStep { get; set; } = DraftStep.PERSONAL_INFO;
        public string FormDataJson { get; set; } = "{}";
        public Channel Channel { get; set; }
        public DraftStatus Status { get; set; } = DraftStatus.IN_PROGRESS;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiresAt { get; set; } = DateTime.Now.AddDays(30);

        // Navigation
        public Product Product { get; set; } = null!;
        public Customer? Customer { get; set; }
    }

}