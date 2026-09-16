using OnboardingPlatform.Core.DTOs.Responses;
using System.Text.Json;

namespace OnboardingPlatform.Core.DTOs.Requests
{
    public class SaveDraftRequest
    {
        public string CurrentStep { get; set; } = string.Empty;
        public DraftFormData FormData { get; set; } = new();
        public string Channel { get; set; } = string.Empty;
    }
}
