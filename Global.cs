using System;
using Microsoft.Data.SqlClient;
using RMS.Utils;
using System.IO;

namespace RMS
{
    internal static class Global
    {
        public enum StorageBackend
        {
            Auto,
            DPAPI,
            AESGcm
        }

        public static SqlConnection? SqlCon { get; private set; }
        public static string? CurrentConnectionString { get; private set; }

        public static StorageBackend PreferredBackend { get; set; } = StorageBackend.Auto;

        private static readonly string CredFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CSharpAssignment", "db.creds");

        public static bool LoadFromStore()
        {
            try
            {
                var cs = CredentialStore.LoadCredentials();
                if (string.IsNullOrWhiteSpace(cs)) return false;

                CurrentConnectionString = cs;
                SqlCon = new SqlConnection(cs);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TestConnection(string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();
                conn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void SetConnection(string connectionString)
        {
            CurrentConnectionString = connectionString;
            SqlCon = new SqlConnection(connectionString);
        }

        public static bool SaveAndPersistConnection(string connectionString, string? passphrase = null)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(CredFilePath) ?? string.Empty);

                switch (PreferredBackend)
                {
                    case StorageBackend.AESGcm:
                        if (!string.IsNullOrEmpty(passphrase))
                        {
                            SecureConfig.SaveEncryptedConnectionString(connectionString, CredFilePath, passphrase);
                        }
                        else
                        {
                            SecureConfig.SaveEncryptedConnectionString(connectionString, CredFilePath);
                        }
                        break;

                    case StorageBackend.DPAPI:
                        CredentialStore.SaveCredentials(connectionString, forceDpapi: true);
                        break;

                    case StorageBackend.Auto:
                    default:
                        CredentialStore.SaveCredentials(connectionString);
                        break;
                }

                SetConnection(connectionString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void ClearStoredCredentials()
        {
            try
            {
                CredentialStore.ClearCredentials();
            }
            catch
            {
            }

            CurrentConnectionString = null;
            SqlCon = null;
        }

        public static string GetCredentialFilePath() => CredFilePath;
    }
}

