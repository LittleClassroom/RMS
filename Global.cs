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

        // Find project root by locating a .csproj file in parent directories of the app base.
        // Only the project-root-local `db.creds` will be used. If none found, operations will fail
        // instead of falling back to other stores.
        private static string? FindProjectRoot()
        {
            try
            {
                var dir = new DirectoryInfo(AppContext.BaseDirectory ?? string.Empty);
                while (dir != null)
                {
                    try
                    {
                        var files = dir.GetFiles("*.csproj");
                        if (files.Length > 0) return dir.FullName;
                    }
                    catch { }
                    dir = dir.Parent;
                }
            }
            catch { }
            return null;
        }

        // Public accessor for callers that want to know which project-local credential file would be used.
        public static string GetCredentialFilePath()
        {
            var root = FindProjectRoot();
            if (string.IsNullOrEmpty(root)) return string.Empty;
            return Path.Combine(root, "db.creds");
        }

        public static bool LoadFromStore()
        {
            try
            {
                // Only use project-root-local db.creds. Do not fall back to AppData or other stores.
                var projectRoot = FindProjectRoot();
                if (string.IsNullOrEmpty(projectRoot)) return false;
                var path = Path.Combine(projectRoot, "db.creds");
                if (!File.Exists(path)) return false;

                var csLocal = SecureConfig.LoadEncryptedConnectionString(path);
                if (string.IsNullOrWhiteSpace(csLocal)) return false;

                CurrentConnectionString = csLocal;
                SqlCon = new SqlConnection(csLocal);
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
                var projectRoot = FindProjectRoot();
                if (string.IsNullOrEmpty(projectRoot)) return false;
                var path = Path.Combine(projectRoot, "db.creds");
                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);

                // Persist only to the project-local file using AES-GCM. Do not write AppData store.
                if (!string.IsNullOrEmpty(passphrase))
                {
                    SecureConfig.SaveEncryptedConnectionString(connectionString, path, passphrase);
                }
                else
                {
                    SecureConfig.SaveEncryptedConnectionString(connectionString, path);
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
                // Remove only project-local db.creds
                var projectRoot = FindProjectRoot();
                if (!string.IsNullOrEmpty(projectRoot))
                {
                    try
                    {
                        var local = Path.Combine(projectRoot, "db.creds");
                        if (File.Exists(local)) File.Delete(local);
                    }
                    catch { }
                }
            }
            catch
            {
            }

            CurrentConnectionString = null;
            SqlCon = null;
        }
    }
}

