namespace OnboardingPlatform.Core.DTOs.Responses
{
    public class FinalizeApplicationResponse
    {
        public Guid CustomerId { get; set; }
        public Guid CustomerProductId { get; set; }
        public string ProductAccountReference { get; set; } = string.Empty;
        public string Status { get; set; } = "ACTIVE";
    }
}
