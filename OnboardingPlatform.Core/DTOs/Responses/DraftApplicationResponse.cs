using OnboardingPlatform.Core.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;

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
        public DraftFormData FormData { get; set; } = new();
        public string Channel { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class DraftFormData
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public List<AddressInfo>? Address { get; set; }
        public string? AccountType { get; set; }
        public decimal? InitialDeposit { get; set; }
        public string? Currency { get; set; }
        public string? PreferredBranch { get; set; }
        public bool? CheckBookRequested { get; set; }
        public List<DocumentInfo>? Documents { get; set; }
    }

    public class DocumentInfo
    {
        public string? Type { get; set; }
        public string? Url { get; set; }
    }

    public class AddressInfo
    {
        public string? HouseNumber { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
    }
}