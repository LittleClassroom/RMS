using System;
using System.Windows.Forms;
using RMS.Utils;
using RMS.Models;

namespace RMS.UI
{
    public class SettingsView : UserControl
    {
        private Button btnServerConfig;
        private readonly UserRole _role;

        public SettingsView(UserRole role)
        {
            _role = role;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            btnServerConfig = new Button();
            var lblDesc = new Label();
            var tt = new ToolTip();
            btnServerConfig.Text = "Server Configuration";
            btnServerConfig.AutoSize = true;
            btnServerConfig.Anchor = AnchorStyles.None;
            btnServerConfig.Click += BtnServerConfig_Click;

            lblDesc.AutoSize = true;
            lblDesc.Text = _role == UserRole.Manager ? "Open server configuration (manager)" : "Open server configuration (requires admin authentication)";
            lblDesc.Top = 0;
            lblDesc.Left = 8;

            tt.SetToolTip(btnServerConfig, "Open the server credentials editor. Admin authentication will be required for non-managers.");

            this.Controls.Add(btnServerConfig);
            this.Controls.Add(lblDesc);
            this.Load += SettingsView_Load;
        }

        private void SettingsView_Load(object? sender, EventArgs e)
        {
            // center the button
            btnServerConfig.Left = (this.ClientSize.Width - btnServerConfig.Width) / 2;
            btnServerConfig.Top = (this.ClientSize.Height - btnServerConfig.Height) / 2;
            btnServerConfig.BringToFront();
            // place description slightly above
            if (this.Controls.Count > 1 && this.Controls[0] is Label lbl)
            {
                lbl.Left = (this.ClientSize.Width - lbl.Width) / 2;
                lbl.Top = btnServerConfig.Top - lbl.Height - 8;
            }
        }

        private void BtnServerConfig_Click(object? sender, EventArgs e)
        {
            // If not manager, require admin auth
            if (_role != UserRole.Manager)
            {
                using var auth = new AdminAuthForm();
                if (auth.ShowDialog(this.FindForm()) != DialogResult.OK) return;
            }

            using var dlg = new DbCredentialsForm();
            if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                // no further action, DbCredentialsForm handles saving and Global.SetConnection
            }
        }
    }
}
