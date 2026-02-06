namespace RMS.UI
{
    partial class StaffMainForm
    {
        private System.ComponentModel.IContainer components = null;
        private RMS.Controls.SidebarControl sidebar;
        private System.Windows.Forms.Panel mainContainer;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Button btnNewOrder;
        private System.Windows.Forms.Panel contentPanel;

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
            sidebar = new RMS.Controls.SidebarControl();
            mainContainer = new Panel();
            contentPanel = new Panel();
            headerPanel = new Panel();
            lblPageTitle = new Label();
            btnNewOrder = new Button();
            mainContainer.SuspendLayout();
            headerPanel.SuspendLayout();
            SuspendLayout();
            // 
            // sidebar
            // 
            sidebar.BackColor = Color.FromArgb(35, 35, 45);
            sidebar.Dock = DockStyle.Left;
            sidebar.Location = new Point(0, 0);
            sidebar.Name = "sidebar";
            sidebar.Size = new Size(220, 700);
            sidebar.TabIndex = 1;
            sidebar.Load += sidebar_Load;
            // 
            // mainContainer
            // 
            mainContainer.Controls.Add(contentPanel);
            mainContainer.Controls.Add(headerPanel);
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.Location = new Point(220, 0);
            mainContainer.Name = "mainContainer";
            mainContainer.Size = new Size(980, 700);
            mainContainer.TabIndex = 0;
            // 
            // contentPanel
            // 
            contentPanel.BackColor = Color.White;
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(0, 70);
            contentPanel.Name = "contentPanel";
            contentPanel.Padding = new Padding(16);
            contentPanel.Size = new Size(980, 630);
            contentPanel.TabIndex = 0;
            contentPanel.Paint += contentPanel_Paint;
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.White;
            headerPanel.Controls.Add(lblPageTitle);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Name = "headerPanel";
            headerPanel.Padding = new Padding(24, 16, 24, 16);
            headerPanel.Size = new Size(980, 70);
            headerPanel.TabIndex = 2;
            // 
            // lblPageTitle
            // 
            lblPageTitle.AutoSize = true;
            lblPageTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblPageTitle.ForeColor = Color.FromArgb(33, 33, 33);
            lblPageTitle.Location = new Point(24, 18);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Size = new Size(157, 37);
            lblPageTitle.TabIndex = 0;
            lblPageTitle.Text = "Dashboard";
            // 
            // btnNewOrder
            // 
            btnNewOrder.Location = new Point(0, 0);
            btnNewOrder.Name = "btnNewOrder";
            btnNewOrder.Size = new Size(75, 23);
            btnNewOrder.TabIndex = 0;
            // 
            // StaffMainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 245, 245);
            ClientSize = new Size(1200, 700);
            Controls.Add(mainContainer);
            Controls.Add(sidebar);
            MinimumSize = new Size(1000, 600);
            Name = "StaffMainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RMS - Staff Dashboard";
            Load += StaffMainForm_Load;
            mainContainer.ResumeLayout(false);
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Label CreateStatBadge(string text, System.Drawing.Color color)
        {
            return new System.Windows.Forms.Label
            {
                Text = text,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                ForeColor = color,
                BackColor = System.Drawing.Color.FromArgb(240, 240, 240),
                AutoSize = false,
                Width = 160,
                Height = 30,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Margin = new System.Windows.Forms.Padding(0, 2, 16, 2),
                Padding = new System.Windows.Forms.Padding(8, 4, 8, 4)
            };
        }
    }
}
