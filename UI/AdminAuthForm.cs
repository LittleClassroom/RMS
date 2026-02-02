using System;
using System.Windows.Forms;
using RMS.Models;

namespace RMS.UI
{
    public partial class AdminAuthForm : Form
    {
        private TextBox tbUser;
        private TextBox tbPassword;
        private Button btnOk;
        private Button btnCancel;
        private CheckBox chkShow;
        private Label lblUser;
        private Label lblPass;

        public AdminAuthForm()
        {
tbUser = new TextBox { Left = 120, Top = 12, Width = 220 };
            tbPassword = new TextBox { Left = 120, Top = 44, Width = 220, UseSystemPasswordChar = true };
            btnOk = new Button { Text = "OK", Left = 120, Top = 96, Width = 100 };
            btnCancel = new Button { Text = "Cancel", Left = 240, Top = 96, Width = 100 };
            chkShow = new CheckBox { Text = "Show password", Left = 120, Top = 72, AutoSize = true };
            lblUser = new Label { Text = "Admin username:", Left = 12, Top = 15, AutoSize = true };
            lblPass = new Label { Text = "Password:", Left = 12, Top = 47, AutoSize = true };

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            chkShow.CheckedChanged += (s, e) => tbPassword.UseSystemPasswordChar = !chkShow.Checked;

            AcceptButton = btnOk;
            CancelButton = btnCancel;

            this.ClientSize = new System.Drawing.Size(360, 136);
            this.Controls.Add(tbUser);
            this.Controls.Add(tbPassword);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);
            this.Controls.Add(chkShow);
            this.Controls.Add(lblUser);
            this.Controls.Add(lblPass);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Admin Authentication";        }

        private void InitializeComponent()
        {
            tbUser = new TextBox { Left = 120, Top = 12, Width = 220 };
            tbPassword = new TextBox { Left = 120, Top = 44, Width = 220, UseSystemPasswordChar = true };
            btnOk = new Button { Text = "OK", Left = 120, Top = 96, Width = 100 };
            btnCancel = new Button { Text = "Cancel", Left = 240, Top = 96, Width = 100 };
            chkShow = new CheckBox { Text = "Show password", Left = 120, Top = 72, AutoSize = true };
            lblUser = new Label { Text = "Admin username:", Left = 12, Top = 15, AutoSize = true };
            lblPass = new Label { Text = "Password:", Left = 12, Top = 47, AutoSize = true };

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            chkShow.CheckedChanged += (s, e) => tbPassword.UseSystemPasswordChar = !chkShow.Checked;

            AcceptButton = btnOk;
            CancelButton = btnCancel;

            this.ClientSize = new System.Drawing.Size(360, 136);
            this.Controls.Add(tbUser);
            this.Controls.Add(tbPassword);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);
            this.Controls.Add(chkShow);
            this.Controls.Add(lblUser);
            this.Controls.Add(lblPass);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Admin Authentication";
        }

        private void BtnOk_Click(object? sender, EventArgs e)
        {
            var user = tbUser.Text?.Trim().ToLower() ?? string.Empty;
            var pass = tbPassword.Text ?? string.Empty;

            // Demo validation: accept manager/admin123. Replace with DB check in production.
            if (user == "manager" && pass == "admin123")
            {
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            MessageBox.Show(this, "Invalid admin credentials.", "Unauthorized", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            tbPassword.SelectAll();
            tbPassword.Focus();
        }
    }
}
