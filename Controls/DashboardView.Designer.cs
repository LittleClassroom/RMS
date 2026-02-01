namespace RMS.Controls
{
    partial class DashboardView
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.FlowLayoutPanel cardsPanel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblUpdatedAt;
        private System.Windows.Forms.Label lblTablesValue;
        private System.Windows.Forms.Label lblOrdersValue;
        private System.Windows.Forms.Label lblKitchenValue;
        private System.Windows.Forms.Label lblSalesValue;
        private System.Windows.Forms.ListView lvActivity;

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
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblUpdatedAt = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.cardsPanel = new System.Windows.Forms.FlowLayoutPanel();

            this.SuspendLayout();

            // 
            // headerPanel
            // 
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Height = 64;
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.headerPanel.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);

            // 
            // lblTitle
            // 
            this.lblTitle.Text = "📊 Dashboard Overview";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(16, 14);

            // 
            // lblUpdatedAt
            // 
            this.lblUpdatedAt.Text = "Updated --";
            this.lblUpdatedAt.AutoSize = true;
            this.lblUpdatedAt.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblUpdatedAt.ForeColor = System.Drawing.Color.Gray;
            this.lblUpdatedAt.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this.lblUpdatedAt.Location = new System.Drawing.Point(460, 24);

            // 
            // btnRefresh
            // 
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Width = 90;
            this.btnRefresh.Height = 30;
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnRefresh.Location = new System.Drawing.Point(640, 18);

            // 
            // btnLogout
            // 
            this.btnLogout.Text = "Logout";
            this.btnLogout.Width = 90;
            this.btnLogout.Height = 30;
            this.btnLogout.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnLogout.Location = new System.Drawing.Point(742, 18);
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.FlatAppearance.BorderSize = 0;

            this.headerPanel.Controls.Add(this.btnLogout);
            this.headerPanel.Controls.Add(this.btnRefresh);
            this.headerPanel.Controls.Add(this.lblUpdatedAt);
            this.headerPanel.Controls.Add(this.lblTitle);

            // 
            // cardsPanel - Dashboard cards grid
            // 
            this.cardsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardsPanel.AutoScroll = true;
            this.cardsPanel.Padding = new System.Windows.Forms.Padding(16);
            this.cardsPanel.BackColor = System.Drawing.Color.White;
            this.cardsPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.cardsPanel.WrapContents = true;

            // Add dashboard cards
            AddDashboardCards();

            // 
            // DashboardView
            // 
            this.Controls.Add(this.cardsPanel);
            this.Controls.Add(this.headerPanel);
            this.Name = "DashboardView";
            this.Size = new System.Drawing.Size(860, 520);
            this.BackColor = System.Drawing.Color.White;
            this.ResumeLayout(false);
        }

        private void AddDashboardCards()
        {
            this.cardsPanel.Controls.Add(CreateStatCard("🪑 Tables", "Loading...", System.Drawing.Color.FromArgb(33, 150, 243), out this.lblTablesValue));
            this.cardsPanel.Controls.Add(CreateStatCard("📝 Open Orders", "Loading...", System.Drawing.Color.FromArgb(255, 152, 0), out this.lblOrdersValue));
            this.cardsPanel.Controls.Add(CreateStatCard("🍳 Kitchen Queue", "Loading...", System.Drawing.Color.FromArgb(244, 67, 54), out this.lblKitchenValue));
            this.cardsPanel.Controls.Add(CreateStatCard("💰 Today's Sales", "Loading...", System.Drawing.Color.FromArgb(76, 175, 80), out this.lblSalesValue));

            // Add a separator
            var separator = new System.Windows.Forms.Panel
            {
                Width = 800,
                Height = 1,
                BackColor = System.Drawing.Color.FromArgb(230, 230, 230),
                Margin = new System.Windows.Forms.Padding(8, 16, 8, 16)
            };
            this.cardsPanel.Controls.Add(separator);

            // Recent Activity Section
            var activityPanel = CreateActivityPanel();
            this.cardsPanel.Controls.Add(activityPanel);
        }

        private System.Windows.Forms.Panel CreateStatCard(string title, string initialValue, System.Drawing.Color color, out System.Windows.Forms.Label valueLabel)
        {
            var card = new System.Windows.Forms.Panel
            {
                Width = 200,
                Height = 110,
                Margin = new System.Windows.Forms.Padding(8),
                BackColor = System.Drawing.Color.White,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            };

            var colorBar = new System.Windows.Forms.Panel
            {
                Dock = System.Windows.Forms.DockStyle.Top,
                Height = 4,
                BackColor = color
            };

            var lblTitle = new System.Windows.Forms.Label
            {
                Text = title,
                Font = new System.Drawing.Font("Segoe UI", 10F),
                ForeColor = System.Drawing.Color.Gray,
                Location = new System.Drawing.Point(12, 16),
                AutoSize = true
            };

            valueLabel = new System.Windows.Forms.Label
            {
                Text = initialValue,
                Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(33, 33, 33),
                Location = new System.Drawing.Point(12, 52),
                AutoSize = true
            };

            card.Controls.Add(colorBar);
            card.Controls.Add(lblTitle);
            card.Controls.Add(valueLabel);

            return card;
        }

        private System.Windows.Forms.Panel CreateActivityPanel()
        {
            var panel = new System.Windows.Forms.Panel
            {
                Width = 800,
                Height = 280,
                Margin = new System.Windows.Forms.Padding(8),
                BackColor = System.Drawing.Color.White,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            };

            var lblHeader = new System.Windows.Forms.Label
            {
                Text = "📋 Recent Activity",
                Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(16, 12),
                AutoSize = true
            };

            this.lvActivity = new System.Windows.Forms.ListView
            {
                Location = new System.Drawing.Point(16, 44),
                Size = new System.Drawing.Size(760, 218),
                View = System.Windows.Forms.View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };

            this.lvActivity.Columns.Add("Time", 100);
            this.lvActivity.Columns.Add("Action", 180);
            this.lvActivity.Columns.Add("Details", 320);
            this.lvActivity.Columns.Add("User", 140);

            panel.Controls.Add(lblHeader);
            panel.Controls.Add(this.lvActivity);

            return panel;
        }
    }
}