using System;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using RMS.Utils;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace RMS.UI
{
    public partial class DbCredentialsForm : Form
    {
        private bool _lastTestSucceeded = false;

        public DbCredentialsForm()
        {
            InitializeComponent();
            btnSaveData.Enabled = false;
            UpdateAuthControls();
        }

        private void chkIntegrated_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAuthControls();
        }

        internal void UpdateAuthControls()
        {
            var integrated = chkIntegrated.Checked;
            tbUser.Enabled = !integrated;
            tbPassword.Enabled = !integrated;
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            var csb = BuildConnectionString();
            Cursor = Cursors.WaitCursor;
            try
            {
                using var conn = new SqlConnection(csb);
                lbStatus.Text = "Testing connection...";
                await conn.OpenAsync();
                using var cmd = new SqlCommand("SELECT 1", conn);
                await cmd.ExecuteScalarAsync();

                _lastTestSucceeded = true;
                btnSaveData.Enabled = true;
                lbStatus.Text = "Connection successful";
            }
            catch (Exception ex)
            {
                _lastTestSucceeded = false;
                btnSaveData.Enabled = false;
                lbStatus.Text = "Connection failed: " + ex.Message;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private static string FindProjectRoot()
        {
            var dir = AppContext.BaseDirectory ?? string.Empty;
            var di = new DirectoryInfo(dir);
            while (di != null)
            {
                try
                {
                    var files = di.GetFiles("*.csproj");
                    if (files.Length > 0) return di.FullName;
                }
                catch { }
                di = di.Parent;
            }
            // fallback to base directory
            return AppContext.BaseDirectory ?? string.Empty;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var csb = BuildConnectionString();

                // Save to the project folder (project root detected by locating a .csproj)
                var projectFolder = FindProjectRoot();
                var path = System.IO.Path.Combine(projectFolder, "db.creds");

                SecureConfig.SaveEncryptedConnectionString(csb, path);
                lbStatus.Text = $"Saved encrypted connection to: {path}";

                // also set active connection in app
                RMS.Global.SetConnection(csb);

                // If this form was shown without an owner (startup flow), close and return OK so
                // Program.Main can continue to the login flow. If shown from Settings (owner present),
                // keep the dialog open so user stays in the app.
                if (this.Owner == null)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Save failed: " + ex.Message, "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                // Prefer the local db.creds path determined by Global
                var path = RMS.Global.GetCredentialFilePath();
                if (string.IsNullOrWhiteSpace(path) || !System.IO.File.Exists(path))
                {
                    lbStatus.Text = $"No stored credentials found at: {path}";
                    return;
                }

                var fi = new FileInfo(path);
                if (fi.Length == 0)
                {
                    // Empty file - remove and inform the user instead of showing a cryptic error
                    try { File.Delete(path); } catch { }
                    lbStatus.Text = "Credential file was empty and has been removed. Save new credentials.";
                    return;
                }

                string connStr = null;
                try
                {
                    // Try AES-GCM secured file first (new format)
                    connStr = SecureConfig.LoadEncryptedConnectionString(path);
                }
                catch (Exception ex)
                {
                    // If AES-GCM load fails, try DPAPI legacy unprotect on the same file
                    try
                    {
                        var protectedData = File.ReadAllBytes(path);
                        // Entropy used by CredentialStore
                        var entropy = Encoding.UTF8.GetBytes("CSharpAssignmentEntropy_v1");
                        var plain = ProtectedData.Unprotect(protectedData, entropy, DataProtectionScope.CurrentUser);
                        var payload = Encoding.UTF8.GetString(plain);
                        try
                        {
                            using var doc = JsonDocument.Parse(payload);
                            if (doc.RootElement.TryGetProperty("ConnectionString", out var cs))
                                connStr = cs.GetString();
                        }
                        catch
                        {
                            // not JSON — maybe raw connection string
                            connStr = payload;
                        }
                    }
                    catch
                    {
                        // both attempts failed
                        // Provide a clearer status to the user and suggest using the Save action
                        lbStatus.Text = "Failed to decrypt local credentials. The file may be corrupt or use a different format.";
                        return;
                    }
                }

                // Update Global connection and UI fields
                RMS.Global.SetConnection(connStr);

                var builder = new SqlConnectionStringBuilder(connStr);
                tbServer.Text = builder.DataSource;
                tbDatabase.Text = builder.InitialCatalog;
                tbUser.Text = builder.UserID;
                tbPassword.Text = builder.Password;
                chkTrust.Checked = builder.TrustServerCertificate;
                chkIntegrated.Checked = builder.IntegratedSecurity;
                UpdateAuthControls();

                lbStatus.Text = $"Loaded connection from: {path}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Load failed: " + ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private string BuildConnectionString()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = tbServer.Text?.Trim() ?? string.Empty,
                InitialCatalog = tbDatabase.Text?.Trim() ?? string.Empty,
                TrustServerCertificate = chkTrust.Checked
            };

            if (chkIntegrated.Checked)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.UserID = tbUser.Text?.Trim() ?? string.Empty;
                builder.Password = tbPassword.Text ?? string.Empty;
            }

            return builder.ToString();
        }

        private string PathToCredFile()
        {
            var appFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CSharpAssignment");
            Directory.CreateDirectory(appFolder);
            return System.IO.Path.Combine(appFolder, "db.creds");
        }

        private void DbCredentialsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
