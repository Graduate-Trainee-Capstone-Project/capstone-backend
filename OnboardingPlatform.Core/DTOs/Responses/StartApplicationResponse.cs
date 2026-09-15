using OnboardingPlatform.Core.Enums;
using OnboardingPlatform.Core.Models;

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

        // pre-filled customer data for existing customers
        public ExistingCustomerData? ExistingCustomer { get; set; }
    }

    public class ExistingCustomerData
    {
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}

