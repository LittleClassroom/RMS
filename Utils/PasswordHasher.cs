using System;
using System.Security.Cryptography;
using System.Text;

namespace RMS.Utils
{
    internal static class PasswordHasher
    {
        // Legacy PBKDF2 parameters (kept for compatibility)
        private const int LegacySaltSize = 16;
        private const int LegacyKeySize = 32;
        private const int LegacyIterations = 100_000;

        // Bcrypt work factor for new hashes
        private const int BcryptWorkFactor = 12;

        // Stored formats:
        // - Legacy PBKDF2: "{iterations}.{saltBase64}.{keyBase64}"
        // - Bcrypt: the standard "$2a$..." / "$2b$..." string produced by BCrypt.Net

        public static string Hash(string password)
        {
            if (password is null) throw new ArgumentNullException(nameof(password));
            // Use bcrypt for new hashes
            return BCrypt.Net.BCrypt.HashPassword(password, BcryptWorkFactor);
        }

        public static bool Verify(string password, string hash)
        {
            if (password is null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(hash)) return false;

            // Detect bcrypt hashes (start with $2a/$2b/$2y)
            if (hash.StartsWith("$2"))
            {
                try
                {
                    return BCrypt.Net.BCrypt.Verify(password, hash);
                }
                catch
                {
                    return false;
                }
            }

            // Fallback: legacy PBKDF2 format iterations.salt.key
            var parts = hash.Split('.', 3);
            if (parts.Length != 3) return false;
            if (!int.TryParse(parts[0], out int it)) return false;
            byte[] salt, key;
            try
            {
                salt = Convert.FromBase64String(parts[1]);
                key = Convert.FromBase64String(parts[2]);
            }
            catch
            {
                return false;
            }

            using var kdf = new Rfc2898DeriveBytes(password, salt, it, HashAlgorithmName.SHA256);
            var candidate = kdf.GetBytes(key.Length);
            return CryptographicOperations.FixedTimeEquals(candidate, key);
        }

        // Helper to determine whether a stored hash should be upgraded to bcrypt
        public static bool IsLegacyHash(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash)) return false;
            return !hash.StartsWith("$2");
        }
    }
}
