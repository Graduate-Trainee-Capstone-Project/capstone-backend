using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OnboardingPlatform.Services.Interfaces
{
    public interface IIdentityService
    {
        /// <summary>
        /// SHA-256 hash a raw identifier value. Never store raw BVN/NIN.
        /// </summary>
        string HashIdentifier(string rawValue);

        /// <summary>
        /// Mask a raw identifier for safe display. e.g. "•••••1234"
        /// </summary>
        string MaskIdentifier(string rawValue);

        /// <summary>
        /// Core lookup: finds an existing customer by hashed identifier.
        /// Returns null if no match.
        /// </summary>
        Task<Guid?> FindExistingCustomerAsync(string identifierType, string identifierHash, string? secondaryType = null, string? secondaryHash = null);
    }
}
