namespace RMS.Controls
{
    partial class ReportsView
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.FlowLayoutPanel reportsPanel;

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
            this.lblTitle = new System.Windows.Forms.Label();
            this.reportsPanel = new System.Windows.Forms.FlowLayoutPanel();

            this.SuspendLayout();

            // 
            // headerPanel
            // 
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Height = 60;
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.headerPanel.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);

            // 
            // lblTitle
            // 
            this.lblTitle.Text = "📈 Reports & Analytics";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(16, 14);
            this.headerPanel.Controls.Add(this.lblTitle);

            // 
            // reportsPanel
            // 
            this.reportsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportsPanel.AutoScroll = true;
            this.reportsPanel.Padding = new System.Windows.Forms.Padding(16);
            this.reportsPanel.BackColor = System.Drawing.Color.White;
            this.reportsPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;

            // Add report cards
            AddReportCards();

            // 
            // ReportsView
            // 
            this.Controls.Add(this.reportsPanel);
            this.Controls.Add(this.headerPanel);
            this.Name = "ReportsView";
            this.Size = new System.Drawing.Size(800, 500);
            this.BackColor = System.Drawing.Color.White;
            this.ResumeLayout(false);
        }

        private void AddReportCards()
        {
            reportsPanel.Controls.Add(CreateReportCard("💰 Sales Report", 
                "Daily, weekly, monthly revenue breakdown", 
                System.Drawing.Color.FromArgb(76, 175, 80)));
            
            reportsPanel.Controls.Add(CreateReportCard("📊 Menu Performance", 
                "Best sellers, slow movers, profit margins", 
                System.Drawing.Color.FromArgb(33, 150, 243)));
            
            reportsPanel.Controls.Add(CreateReportCard("📦 Inventory Report", 
                "Stock levels, usage trends, waste analysis", 
                System.Drawing.Color.FromArgb(255, 152, 0)));
            
            reportsPanel.Controls.Add(CreateReportCard("👥 Staff Performance", 
                "Sales per server, shifts worked, tips", 
                System.Drawing.Color.FromArgb(156, 39, 176)));
            
            reportsPanel.Controls.Add(CreateReportCard("🪑 Table Utilization", 
                "Turnover rate, peak hours, reservations", 
                System.Drawing.Color.FromArgb(0, 150, 136)));
            
            reportsPanel.Controls.Add(CreateReportCard("📋 Audit Log", 
                "System activity, user actions, changes", 
                System.Drawing.Color.FromArgb(96, 125, 139)));
        }

        private System.Windows.Forms.Panel CreateReportCard(string title, string description, System.Drawing.Color color)
        {
            var card = new System.Windows.Forms.Panel
            {
                Width = 240,
                Height = 140,
                Margin = new System.Windows.Forms.Padding(8),
                BackColor = System.Drawing.Color.White,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Cursor = System.Windows.Forms.Cursors.Hand
            };

            var colorBar = new System.Windows.Forms.Panel
            {
                Dock = System.Windows.Forms.DockStyle.Top,
                Height = 6,
                BackColor = color
            };

            var lblTitle = new System.Windows.Forms.Label
            {
                Text = title,
                Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(33, 33, 33),
                Location = new System.Drawing.Point(12, 20),
                AutoSize = true
            };

            var lblDesc = new System.Windows.Forms.Label
            {
                Text = description,
                Font = new System.Drawing.Font("Segoe UI", 9F),
                ForeColor = System.Drawing.Color.Gray,
                Location = new System.Drawing.Point(12, 50),
                MaximumSize = new System.Drawing.Size(216, 0),
                AutoSize = true
            };

            var btnGenerate = new System.Windows.Forms.Button
            {
                Text = "Generate →",
                Font = new System.Drawing.Font("Segoe UI", 9F),
                ForeColor = color,
                BackColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Size = new System.Drawing.Size(90, 28),
                Location = new System.Drawing.Point(138, 100),
                Cursor = System.Windows.Forms.Cursors.Hand
            };
            btnGenerate.FlatAppearance.BorderColor = color;

            card.Controls.Add(colorBar);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblDesc);
            card.Controls.Add(btnGenerate);

            return card;
        }
    }
}