using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.DTOs.Responses
{
    public class SaveDraftResponse
    {
        public Guid DraftId { get; set; }
        public string CurrentStep { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime LastUpdatedAt { get; set; }
    }
}
