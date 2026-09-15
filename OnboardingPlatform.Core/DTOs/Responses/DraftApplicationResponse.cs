using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.DTOs.Responses
{
    public class DraftApplicationResponse
    {
        public Guid DraftId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? CustomerId { get; set; }
        public string PrimaryIdentifierType { get; set; } = string.Empty;
        public string? SecondaryIdentifierType { get; set; }
        public string CurrentStep { get; set; } = string.Empty;
        public Dictionary<string, object?> FormData { get; set; } = new();
        public string Channel { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
