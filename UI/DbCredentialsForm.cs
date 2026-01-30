using System;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using RMS.Utils;
using System.IO;

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

        private void btnTest_Click(object sender, EventArgs e)
        {
            var csb = BuildConnectionString();
            Cursor = Cursors.WaitCursor;
            var ok = RMS.Global.TestConnection(csb);
            Cursor = Cursors.Default;
            _lastTestSucceeded = ok;
            btnSaveData.Enabled = ok;
            lbStatus.Text = ok ? "Connection successful" : "Connection failed";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_lastTestSucceeded)
            {
                MessageBox.Show(this, "Please test the connection successfully before saving.", "Cannot Save", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lbStatus.Text = "Save prevented: test required.";
                return;
            }

            var csb = BuildConnectionString();
            Cursor = Cursors.WaitCursor;
            var ok = RMS.Global.TestConnection(csb);
            Cursor = Cursors.Default;
            if (!ok)
            {
                MessageBox.Show(this, "Cannot save: connection test failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lbStatus.Text = "Test failed. Not saved.";
                _lastTestSucceeded = false;
                btnSaveData.Enabled = false;
                return;
            }

            try
            {
                if (chkEncrypt.Checked)
                {
                    SecureConfig.SaveEncryptedConnectionString(csb, PathToCredFile());
                }
                else
                {
                    CredentialStore.SaveCredentials(csb);
                }

                RMS.Global.SetConnection(csb);
                lbStatus.Text = "Saved encrypted credentials.";
                MessageBox.Show(this, "Credentials saved.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to save credentials: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lbStatus.Text = "Save failed.";
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                var cs = CredentialStore.LoadCredentials();
                if (string.IsNullOrWhiteSpace(cs))
                {
                    lbStatus.Text = "No stored credentials found.";
                    return;
                }

                var trimmed = cs.TrimStart();
                if (trimmed.StartsWith('{'))
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(cs);
                    if (doc.RootElement.TryGetProperty("ConnectionString", out var cselt))
                        cs = cselt.GetString() ?? string.Empty;
                }

                var builder = new SqlConnectionStringBuilder(cs);
                tbServer.Text = builder.DataSource;
                tbDatabase.Text = builder.InitialCatalog;
                tbUser.Text = builder.UserID;
                tbPassword.Text = builder.Password;
                chkTrust.Checked = builder.TrustServerCertificate;
                // detect integrated security
                chkIntegrated.Checked = builder.IntegratedSecurity;
                UpdateAuthControls();
                lbStatus.Text = "Loaded credentials into form.";
            }
            catch
            {
                lbStatus.Text = "Failed to load credentials.";
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
    }
}
