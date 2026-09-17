using Microsoft.EntityFrameworkCore;
using OnboardingPlatform.Core.Enums;
using OnboardingPlatform.Data.Implementations;
using OnboardingPlatform.Services.Interfaces;
using System;
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
            // Search by HASH ONLY — ignore identifier type. A customer who
            // registered with their BVN for savings can be found the same way
            // when they type that same BVN for pension, stockbroking, etc.
            var primary = await _db.CustomerIdentifiers
                .FirstOrDefaultAsync(i => i.IdentifierValueHash == identifierHash);

            if (primary == null) return null;

            // Secondary is informational only — if the customer already has it
            // stored, great; if not, we still recognize them by primary (BVN)
            // and the secondary (e.g. NIN) gets captured fresh at finalize.
            // We don't block recognition on it.
            return primary.CustomerId;
        }
    }
}