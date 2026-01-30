using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;

namespace RMS.Utils
{
    internal static class CredentialStore
    {
        private static readonly string AppFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CSharpAssignment");
        private static readonly string CredFile = Path.Combine(AppFolder, "db.creds");
        // Optional entropy to bind the protected blob to this app version (legacy DPAPI fallback)
        private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("CSharpAssignmentEntropy_v1");

        public static bool CredentialsExist()
        {
            try
            {
                return File.Exists(CredFile);
            }
            catch
            {
                return false;
            }
        }

        // Backwards-compatible API: default behavior prefers SecureConfig (AES-GCM) storage
        public static void SaveCredentials(string connectionString)
            => SaveCredentials(connectionString, forceDpapi: false);

        // New overload: allow caller to force DPAPI storage if needed
        public static void SaveCredentials(string connectionString, bool forceDpapi)
        {
            if (connectionString is null) throw new ArgumentNullException(nameof(connectionString));
            Directory.CreateDirectory(AppFolder);

            if (!forceDpapi)
            {
                try
                {
                    // Prefer AES-GCM SecureConfig storage (does not depend on Windows user DPAPI)
                    SecureConfig.SaveEncryptedConnectionString(connectionString, CredFile);
                    return;
                }
                catch
                {
                    // If SecureConfig fails for any reason, fall back to DPAPI (legacy behaviour)
                }
            }

            var payload = JsonSerializer.Serialize(new { ConnectionString = connectionString });
            var plain = Encoding.UTF8.GetBytes(payload);
            var protectedData = ProtectedData.Protect(plain, Entropy, DataProtectionScope.CurrentUser);
            File.WriteAllBytes(CredFile, protectedData);
        }

        public static string? LoadCredentials()
        {
            if (!CredentialsExist()) return null;
            try
            {
                // First try SecureConfig (AES-GCM)
                try
                {
                    var result = SecureConfig.LoadEncryptedConnectionString(CredFile);
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        // result may be either a raw connection string or a JSON payload from older implementations
                        var trimmed = result.TrimStart();
                        if (trimmed.StartsWith('{'))
                        {
                            using var doc = JsonDocument.Parse(result);
                            if (doc.RootElement.TryGetProperty("ConnectionString", out var cs))
                                return cs.GetString();
                        }

                        return result; // treat as connection string
                    }
                }
                catch
                {
                    // SecureConfig failed; fall back to DPAPI legacy format
                }

                var protectedData = File.ReadAllBytes(CredFile);
                var plain = ProtectedData.Unprotect(protectedData, Entropy, DataProtectionScope.CurrentUser);
                var payload = Encoding.UTF8.GetString(plain);
                using var doc2 = JsonDocument.Parse(payload);
                if (doc2.RootElement.TryGetProperty("ConnectionString", out var cs2))
                {
                    return cs2.GetString();
                }
            }
            catch
            {
                // Corrupt or wrong key/user; treat as no credentials
            }
            return null;
        }

        public static void ClearCredentials()
        {
            try
            {
                if (File.Exists(CredFile)) File.Delete(CredFile);
            }
            catch
            {
                // ignore
            }
        }
    }
}
