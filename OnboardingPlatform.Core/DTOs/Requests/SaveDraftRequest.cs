using Microsoft.AspNetCore.Http;
using OnboardingPlatform.Core.DTOs.Responses;

namespace OnboardingPlatform.Core.DTOs.Requests
{
    public class SaveDraftRequest
    {
        public string CurrentStep { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;

        // Personal info
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AccountType { get; set; }
        public decimal? InitialDeposit { get; set; }
        public string? Currency { get; set; }
        public string? PreferredBranch { get; set; }
        public bool? CheckBookRequested { get; set; }

        // Address (flattened)
        public string? HouseNumber { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }

        // Document 1
        public string? DocumentType { get; set; }
        public IFormFile? DocumentFile { get; set; }

        // Document 2 — some products require two documents in one step
        // (e.g. ID card + proof of address)
        public string? SecondDocumentType { get; set; }
        public IFormFile? SecondDocumentFile { get; set; }
    }

    public class InternalSaveDraftRequest
    {
        public string CurrentStep { get; set; } = string.Empty;
        public DraftFormData FormData { get; set; } = new();
        public string Channel { get; set; } = string.Empty;
    }
}