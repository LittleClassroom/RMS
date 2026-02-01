using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using RMS.Models;
using RMS.Utils;

namespace RMS.UI
{
    public class EditStaffDialog : Form
    {
        private TextBox tbUser;
        private TextBox tbDisplay;
        private TextBox tbPassword;
        private ComboBox cbRole;
        private CheckBox chkActive;
        private Button btnOk;
        private Button btnCancel;
        private readonly int? _userId;

        public EditStaffDialog(int? userId = null)
        {
            _userId = userId;
            InitializeComponent();
            Load += EditStaffDialog_Load;
        }

        private void InitializeComponent()
        {
            this.ClientSize = new System.Drawing.Size(420, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = _userId.HasValue ? "Edit Staff" : "Add Staff";

            var lblUser = new Label { Left = 12, Top = 12, Text = "Username:" };
            tbUser = new TextBox { Left = 120, Top = 8, Width = 260 };

            var lblDisplay = new Label { Left = 12, Top = 44, Text = "Display Name:" };
            tbDisplay = new TextBox { Left = 120, Top = 40, Width = 260 };

            var lblPass = new Label { Left = 12, Top = 76, Text = "Password:" };
            tbPassword = new TextBox { Left = 120, Top = 72, Width = 260, UseSystemPasswordChar = true };

            var lblRole = new Label { Left = 12, Top = 108, Text = "Role:" };
            cbRole = new ComboBox { Left = 120, Top = 104, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            cbRole.Items.AddRange(Enum.GetNames(typeof(UserRole)));

            chkActive = new CheckBox { Left = 120, Top = 136, Text = "Active", Checked = true };

            btnOk = new Button { Text = "Save", Left = 120, Top = 164, Width = 100 };
            btnCancel = new Button { Text = "Cancel", Left = 240, Top = 164, Width = 100 };

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            this.Controls.Add(lblUser);
            this.Controls.Add(tbUser);
            this.Controls.Add(lblDisplay);
            this.Controls.Add(tbDisplay);
            this.Controls.Add(lblPass);
            this.Controls.Add(tbPassword);
            this.Controls.Add(lblRole);
            this.Controls.Add(cbRole);
            this.Controls.Add(chkActive);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);
        }

        private void EditStaffDialog_Load(object? sender, EventArgs e)
        {
            if (!_userId.HasValue) return;

            try
            {
                using var con = new SqlConnection(RMS.Global.CurrentConnectionString ?? string.Empty);
                con.Open();
                using var cmd = new SqlCommand("SELECT Username, DisplayName, Role, IsActive FROM dbo.Users WHERE UserId = @id", con);
                cmd.Parameters.AddWithValue("@id", _userId.Value);
                using var rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    tbUser.Text = rdr.GetString(0);
                    tbDisplay.Text = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1);
                    cbRole.SelectedIndex = rdr.GetByte(2);
                    chkActive.Checked = rdr.GetBoolean(3);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to load user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnOk_Click(object? sender, EventArgs e)
        {
            var username = tbUser.Text?.Trim() ?? string.Empty;
            var display = tbDisplay.Text?.Trim() ?? string.Empty;
            var pass = tbPassword.Text ?? string.Empty;
            var role = (UserRole)cbRole.SelectedIndex;
            var active = chkActive.Checked;

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show(this, "Username required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var con = new SqlConnection(RMS.Global.CurrentConnectionString ?? string.Empty);
                con.Open();

                if (_userId.HasValue)
                {
                    // update
                    using var cmd = new SqlCommand("UPDATE dbo.Users SET Username=@u, DisplayName=@d, Role=@r, IsActive=@a" +
                        (string.IsNullOrEmpty(pass) ? "" : ", PasswordHash=@p") +
                        " WHERE UserId=@id", con);
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@d", display);
                    cmd.Parameters.AddWithValue("@r", (byte)role);
                    cmd.Parameters.AddWithValue("@a", active);
                    if (!string.IsNullOrEmpty(pass)) cmd.Parameters.AddWithValue("@p", PasswordHasher.Hash(pass));
                    cmd.Parameters.AddWithValue("@id", _userId.Value);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    // create
                    if (string.IsNullOrEmpty(pass))
                    {
                        MessageBox.Show(this, "Password required for new user.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    using var cmd = new SqlCommand("INSERT INTO dbo.Users (Username, PasswordHash, DisplayName, Role, IsActive) VALUES (@u,@p,@d,@r,@a)", con);
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", PasswordHasher.Hash(pass));
                    cmd.Parameters.AddWithValue("@d", display);
                    cmd.Parameters.AddWithValue("@r", (byte)role);
                    cmd.Parameters.AddWithValue("@a", active);
                    cmd.ExecuteNonQuery();
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (SqlException sx) when (sx.Number == 2627)
            {
                MessageBox.Show(this, "Username already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to save user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
