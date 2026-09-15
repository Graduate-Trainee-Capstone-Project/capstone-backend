using OnboardingPlatform.Core.Enums;

namespace OnboardingPlatform.Core.Models;

public class CustomerIdentifier
{
    public Guid IdentifierId { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }

    public IdentifierType IdentifierType { get; set; }

    // SHA-256 hash of the raw value.
    // Never store the raw BVN or NIN.
    public string IdentifierValueHash { get; set; } = string.Empty;

    public string IdentifierValueMasked { get; set; } = string.Empty;

    public bool IsVerified { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Customer Customer { get; set; } = null!;
}