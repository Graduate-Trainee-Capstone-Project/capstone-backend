using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Core.DTOs.Requests
{
    public class SaveDraftRequest
    {
        public string CurrentStep { get; set; } = string.Empty;
        public Dictionary<string, object?> FormData { get; set; } = new();
        public string Channel { get; set; } = string.Empty;
    }
}
