using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RMS.Utils
{
    // AES-GCM based helper that does NOT rely on the current Windows user (no DPAPI).
    // NOTE: This uses a static application secret by default. For production, supply
    // a secure secret (e.g. user passphrase) and protect it appropriately.
    internal static class SecureConfig
    {
        // Replace this with a secure secret retrieved from a safe source if needed.
        private const string AppSecret = "GUI_IT3C1_Static_Secret_v1";

        private const int SaltSize = 16;   // bytes
        private const int NonceSize = 12;  // bytes for AES-GCM
        private const int TagSize = 16;    // bytes for AES-GCM tag
        private const int KeySize = 32;    // 256-bit key
        private const int DeriveIterations = 100_000; // PBKDF2 iterations

     
        public static void SaveEncryptedConnectionString(string connectionString, string filePath)
            => SaveEncryptedConnectionString(connectionString, filePath, AppSecret);

        // Load and decrypt connection string previously saved with SaveEncryptedConnectionString.
        public static string LoadEncryptedConnectionString(string filePath)
            => LoadEncryptedConnectionString(filePath, AppSecret);

        // Overload that accepts an explicit passphrase. Prefer providing a user secret in real apps.
        public static void SaveEncryptedConnectionString(string connectionString,
                            string filePath, string passphrase)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            if (passphrase == null) throw new ArgumentNullException(nameof(passphrase));

            var plain = Encoding.UTF8.GetBytes(connectionString);

            // Generate salt and nonce
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var nonce = RandomNumberGenerator.GetBytes(NonceSize);

            // Derive key from passphrase + salt
            using var kdf = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(passphrase),
                    salt, DeriveIterations, HashAlgorithmName.SHA256);
            var key = kdf.GetBytes(KeySize);

            var ciphertext = new byte[plain.Length];
            var tag = new byte[TagSize];

            // Encrypt using AES-GCM
            using var aesGcm = new AesGcm(key);
            aesGcm.Encrypt(nonce, plain, ciphertext, tag, null);

            // Write file: salt|nonce|tag|ciphertext
            using var fs = new FileStream(filePath, FileMode.Create,
                        FileAccess.Write, FileShare.None);
            fs.Write(salt, 0, salt.Length);
            fs.Write(nonce, 0, nonce.Length);
            fs.Write(tag, 0, tag.Length);
            fs.Write(ciphertext, 0, ciphertext.Length);
            fs.Flush(true);
        }

        public static string LoadEncryptedConnectionString(string filePath, string passphrase)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            if (passphrase == null) throw new ArgumentNullException(nameof(passphrase));
            if (!File.Exists(filePath)) throw new FileNotFoundException("Configuration file not found.", filePath);

            var all = File.ReadAllBytes(filePath);
            var expectedMin = SaltSize + NonceSize + TagSize;
            if (all.Length < expectedMin) throw new
                    InvalidDataException("Encrypted file is too short or corrupt.");

            var salt = new byte[SaltSize];
            var nonce = new byte[NonceSize];
            var tag = new byte[TagSize];
            var ciphertextLen = all.Length - expectedMin;
            var ciphertext = new byte[ciphertextLen];

            Buffer.BlockCopy(all, 0, salt, 0, SaltSize);
            Buffer.BlockCopy(all, SaltSize, nonce, 0, NonceSize);
            Buffer.BlockCopy(all, SaltSize + NonceSize, tag, 0, TagSize);
            if (ciphertextLen > 0)
                Buffer.BlockCopy(all, SaltSize + NonceSize + TagSize, ciphertext, 0, ciphertextLen);

            // Derive key
            using var kdf = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(passphrase), salt, DeriveIterations, HashAlgorithmName.SHA256);
            var key = kdf.GetBytes(KeySize);

            var plain = new byte[ciphertextLen];
            try
            {
                using var aesGcm = new AesGcm(key);
                aesGcm.Decrypt(nonce, ciphertext, tag, plain, null);
            }
            catch (CryptographicException ex)
            {
                throw new CryptographicException("Decryption failed — the passphrase may be incorrect or the file is corrupted.", ex);
            }

            return Encoding.UTF8.GetString(plain);
        }
    }
}
