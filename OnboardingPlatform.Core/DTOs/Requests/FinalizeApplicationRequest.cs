using System.ComponentModel.DataAnnotations;

namespace OnboardingPlatform.Core.DTOs.Requests
{
    public class FinalizeApplicationRequest
    {
        [Required]
        public Guid DraftId { get; set; }

        // Full form data from Emmanuel's DraftApplications.FormDataJson
        public Dictionary<string, object> FormData { get; set; } = new();
    }
}
