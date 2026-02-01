using System;
using System.Windows.Forms;
using RMS.Models;
using Microsoft.Data.SqlClient;
using RMS.Utils;

namespace RMS.UI
{
    public partial class SignIn : Form
    {
        public string? AuthenticatedUser { get; private set; }
        public UserRole AuthenticatedRole { get; private set; }

        public SignIn()
        {
            InitializeComponent();
            button1.Click += btnLogin_Click;
            textBox1.UseSystemPasswordChar = true;
            
            // Allow Enter key to submit
            tbUsername.KeyPress += (s, e) => { if (e.KeyChar == (char)Keys.Enter) btnLogin_Click(s, e); };
            textBox1.KeyPress += (s, e) => { if (e.KeyChar == (char)Keys.Enter) btnLogin_Click(s, e); };
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            string username = tbUsername.Text.Trim();
            string password = textBox1.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Attempt DB authentication
            try
            {
                if (string.IsNullOrWhiteSpace(RMS.Global.CurrentConnectionString))
                {
                    MessageBox.Show(this, "Database connection is not configured. Open Settings -> DB Credentials.", "No DB", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using var con = new SqlConnection(RMS.Global.CurrentConnectionString);
                con.Open();
                using var cmd = new SqlCommand("SELECT UserId, Username, PasswordHash, DisplayName, Role, IsActive FROM dbo.Users WHERE LOWER(Username) = LOWER(@u)", con);
                cmd.Parameters.AddWithValue("@u", username);
                using var rdr = cmd.ExecuteReader();
                if (!rdr.Read())
                {
                    // not found
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Clear();
                    textBox1.Focus();
                    return;
                }

                var userId = rdr.GetInt32(0);
                var dbUsername = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1);
                var passwordHash = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2);
                var display = rdr.IsDBNull(3) ? dbUsername : rdr.GetString(3);
                var role = (UserRole)rdr.GetByte(4);
                var isActive = rdr.GetBoolean(5);

                if (!isActive)
                {
                    MessageBox.Show(this, "This account is disabled.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var ok = PasswordHasher.Verify(password, passwordHash);
                if (!ok)
                {
                    MessageBox.Show(this, "Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Clear();
                    textBox1.Focus();
                    return;
                }

                // If legacy hash, re-hash with bcrypt and update DB
                if (PasswordHasher.IsLegacyHash(passwordHash))
                {
                    try
                    {
                        var newHash = PasswordHasher.Hash(password);
                        using var upd = new SqlCommand("UPDATE dbo.Users SET PasswordHash = @h WHERE UserId = @id", con);
                        upd.Parameters.AddWithValue("@h", newHash);
                        upd.Parameters.AddWithValue("@id", userId);
                        upd.ExecuteNonQuery();
                    }
                    catch
                    {
                        // ignore; not critical
                    }
                }

                AuthenticatedUser = display;
                AuthenticatedRole = role;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to authenticate: " + ex.Message, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
