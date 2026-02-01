namespace RMS.Controls
{
    partial class SidebarControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Panel userInfoPanel;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUserRole;
        private System.Windows.Forms.FlowLayoutPanel navFlowPanel;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnTables;
        private System.Windows.Forms.Button btnOrders;
        private System.Windows.Forms.Button btnKitchen;
        private System.Windows.Forms.Button btnInventory;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Panel footerPanel;
        private System.Windows.Forms.Button btnLogout;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblAppName = new System.Windows.Forms.Label();
            this.userInfoPanel = new System.Windows.Forms.Panel();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblUserRole = new System.Windows.Forms.Label();
            this.navFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnTables = new System.Windows.Forms.Button();
            this.btnOrders = new System.Windows.Forms.Button();
            this.btnKitchen = new System.Windows.Forms.Button();
            this.btnInventory = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.footerPanel = new System.Windows.Forms.Panel();
            this.btnLogout = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // 
            // SidebarControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(35, 35, 45);
            this.Size = new System.Drawing.Size(220, 600);
            this.Padding = new System.Windows.Forms.Padding(0);

            // 
            // headerPanel - App branding
            // 
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Height = 70;
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(25, 25, 35);
            this.headerPanel.Padding = new System.Windows.Forms.Padding(16, 20, 16, 12);

            this.lblAppName.Text = "🍽️ RMS";
            this.lblAppName.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblAppName.ForeColor = System.Drawing.Color.White;
            this.lblAppName.AutoSize = true;
            this.lblAppName.Location = new System.Drawing.Point(16, 22);
            this.headerPanel.Controls.Add(this.lblAppName);

            // 
            // userInfoPanel - Current user info
            // 
            this.userInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.userInfoPanel.Height = 60;
            this.userInfoPanel.BackColor = System.Drawing.Color.FromArgb(45, 45, 55);
            this.userInfoPanel.Padding = new System.Windows.Forms.Padding(16, 10, 16, 10);

            this.lblUserName.Text = "John Doe";
            this.lblUserName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUserName.ForeColor = System.Drawing.Color.White;
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(16, 12);

            this.lblUserRole.Text = "Service";
            this.lblUserRole.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblUserRole.ForeColor = System.Drawing.Color.FromArgb(150, 150, 150);
            this.lblUserRole.AutoSize = true;
            this.lblUserRole.Location = new System.Drawing.Point(16, 32);

            this.userInfoPanel.Controls.Add(this.lblUserName);
            this.userInfoPanel.Controls.Add(this.lblUserRole);

            // 
            // navFlowPanel - Navigation buttons
            // 
            this.navFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.navFlowPanel.WrapContents = false;
            this.navFlowPanel.Padding = new System.Windows.Forms.Padding(8, 16, 8, 8);
            this.navFlowPanel.AutoScroll = true;
            this.navFlowPanel.BackColor = System.Drawing.Color.FromArgb(35, 35, 45);

            // 
            // Navigation Buttons - styled consistently
            // 
            ConfigureNavButton(this.btnDashboard, "📊  Dashboard");
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);

            ConfigureNavButton(this.btnTables, "🪑  Tables");
            this.btnTables.Click += new System.EventHandler(this.btnTables_Click);

            ConfigureNavButton(this.btnOrders, "📝  Orders");
            this.btnOrders.Click += new System.EventHandler(this.btnOrders_Click);

            ConfigureNavButton(this.btnKitchen, "🍳  Kitchen");
            this.btnKitchen.Click += new System.EventHandler(this.btnKitchen_Click);

            ConfigureNavButton(this.btnInventory, "📦  Inventory");
            this.btnInventory.Click += new System.EventHandler(this.btnInventory_Click);

            ConfigureNavButton(this.btnReports, "📈  Reports");
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);

            ConfigureNavButton(this.btnSettings, "⚙️  Settings");
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);

            this.navFlowPanel.Controls.Add(this.btnDashboard);
            this.navFlowPanel.Controls.Add(this.btnTables);
            this.navFlowPanel.Controls.Add(this.btnOrders);
            this.navFlowPanel.Controls.Add(this.btnKitchen);
            this.navFlowPanel.Controls.Add(this.btnInventory);
            this.navFlowPanel.Controls.Add(this.btnReports);
            this.navFlowPanel.Controls.Add(this.btnSettings);

            // 
            // footerPanel - Logout button
            // 
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerPanel.Height = 60;
            this.footerPanel.BackColor = System.Drawing.Color.FromArgb(25, 25, 35);
            this.footerPanel.Padding = new System.Windows.Forms.Padding(8);

            this.btnLogout.Text = "🚪  Logout";
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.btnLogout.BackColor = System.Drawing.Color.Transparent;
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            this.footerPanel.Controls.Add(this.btnLogout);

            // Add panels in correct order (reverse for Dock)
            this.Controls.Add(this.navFlowPanel);
            this.Controls.Add(this.userInfoPanel);
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.footerPanel);

            this.Name = "SidebarControl";
            this.ResumeLayout(false);
        }

        private void ConfigureNavButton(System.Windows.Forms.Button btn, string text)
        {
            btn.Text = text;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(55, 55, 65);
            btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(65, 65, 75);
            btn.Font = new System.Drawing.Font("Segoe UI", 10F);
            btn.ForeColor = System.Drawing.Color.FromArgb(200, 200, 200);
            btn.BackColor = System.Drawing.Color.Transparent;
            btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btn.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            btn.Margin = new System.Windows.Forms.Padding(4);
            btn.Width = 196;
            btn.Height = 42;
            btn.Cursor = System.Windows.Forms.Cursors.Hand;
        }
    }
}