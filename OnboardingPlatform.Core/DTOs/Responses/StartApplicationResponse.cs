namespace OnboardingPlatform.Core.DTOs.Responses
{
    public class StartApplicationResponse
    {
        public Guid DraftId { get; set; }
        public bool IsResumed { get; set; }
        public bool IsExistingCustomer { get; set; }
        public bool RequiresSecurityCheck { get; set; }
        public string CurrentStep { get; set; } = string.Empty;
        public Dictionary<string, object> FormData { get; set; } = new();
    }
}
