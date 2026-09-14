using Microsoft.EntityFrameworkCore;
using OnboardingPlatform.Data.Implementations;
using OnboardingPlatform.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Services.Implementations
{
    public class IdentityService : IIdentityService
    {
        private readonly AppDbContext _db;

        public IdentityService(AppDbContext db)
        {
            _db = db;
        }

        public string HashIdentifier(string rawValue)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawValue.Trim().ToUpperInvariant()));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }

        public string MaskIdentifier(string rawValue)
        {
            if (rawValue.Length <= 4)
                return new string('•', rawValue.Length);

            return new string('•', rawValue.Length - 4) + rawValue[^4..];
        }

        public async Task<Guid?> FindExistingCustomerAsync(
            string identifierType,
            string identifierHash,
            string? secondaryType = null,
            string? secondaryHash = null)
        {
            var primary = await _db.CustomerIdentifiers
                .FirstOrDefaultAsync(i =>
                    i.IdentifierType.ToString() == identifierType &&
                    i.IdentifierValueHash == identifierHash);

            if (primary == null) return null;

            // If product requires a secondary identifier (e.g. Pension needs NIN + PHONE),
            // we verify both belong to the same customer
            if (secondaryType != null && secondaryHash != null)
            {
                var secondary = await _db.CustomerIdentifiers
                    .FirstOrDefaultAsync(i =>
                        i.IdentifierType.ToString() == secondaryType &&
                        i.IdentifierValueHash == secondaryHash &&
                        i.CustomerId == primary.CustomerId);

                if (secondary == null) return null;
            }

            return primary.CustomerId;
        }
    }
}
