using System.ComponentModel.DataAnnotations;

namespace OnboardingPlatform.Core.DTOs.Requests
{
    public class StartApplicationRequest
    {
        [Required]
        public string ProductCode { get; set; } = string.Empty;

        [Required]
        public string PrimaryIdentifierValue { get; set; } = string.Empty;

        public string? SecondaryIdentifierValue { get; set; }

        [Required]
        public string Channel { get; set; } = "WEB";
    }
}
