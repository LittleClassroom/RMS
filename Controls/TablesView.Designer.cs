namespace RMS.Controls
{
    partial class TablesView
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnAutoRefresh;
        private System.Windows.Forms.FlowLayoutPanel tablesFlowPanel;
        private System.Windows.Forms.FlowLayoutPanel legendFlow;
        private System.Windows.Forms.Panel legendPanel;

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
            headerPanel = new Panel();
            lblTitle = new Label();
            btnRefresh = new Button();
            btnAutoRefresh = new Button();
            legendPanel = new Panel();
            legendFlow = new FlowLayoutPanel();
            tablesFlowPanel = new FlowLayoutPanel();
            headerPanel.SuspendLayout();
            legendPanel.SuspendLayout();
            SuspendLayout();
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.FromArgb(245, 245, 245);
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(btnRefresh);
            headerPanel.Controls.Add(btnAutoRefresh);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Name = "headerPanel";
            headerPanel.Padding = new Padding(16, 12, 16, 12);
            headerPanel.Size = new Size(800, 60);
            headerPanel.TabIndex = 2;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Dock = DockStyle.Left;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.Location = new Point(16, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(191, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "\U0001fa91 Table Status";
            // 
            // btnRefresh
            // 
            btnRefresh.AutoSize = true;
            btnRefresh.Dock = DockStyle.Right;
            btnRefresh.FlatStyle = FlatStyle.System;
            btnRefresh.Location = new Point(609, 12);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 36);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Refresh";
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // btnAutoRefresh
            // 
            btnAutoRefresh.AutoSize = true;
            btnAutoRefresh.Dock = DockStyle.Right;
            btnAutoRefresh.FlatStyle = FlatStyle.System;
            btnAutoRefresh.Location = new Point(684, 12);
            btnAutoRefresh.Name = "btnAutoRefresh";
            btnAutoRefresh.Size = new Size(100, 36);
            btnAutoRefresh.TabIndex = 2;
            btnAutoRefresh.Text = "Auto: Off";
            btnAutoRefresh.Click += BtnAutoRefresh_Click;
            // 
            // legendPanel
            // 
            legendPanel.BackColor = Color.White;
            legendPanel.Controls.Add(legendFlow);
            legendPanel.Dock = DockStyle.Top;
            legendPanel.Location = new Point(0, 60);
            legendPanel.Name = "legendPanel";
            legendPanel.Padding = new Padding(16, 8, 16, 8);
            legendPanel.Size = new Size(800, 40);
            legendPanel.TabIndex = 1;
            // 
            // legendFlow
            // 
            legendFlow.Location = new Point(0, 0);
            legendFlow.Name = "legendFlow";
            legendFlow.Size = new Size(200, 100);
            legendFlow.TabIndex = 0;
            // 
            // tablesFlowPanel
            // 
            tablesFlowPanel.AutoScroll = true;
            tablesFlowPanel.BackColor = Color.White;
            tablesFlowPanel.Dock = DockStyle.Fill;
            tablesFlowPanel.Location = new Point(0, 100);
            tablesFlowPanel.Name = "tablesFlowPanel";
            tablesFlowPanel.Padding = new Padding(16);
            tablesFlowPanel.Size = new Size(800, 400);
            tablesFlowPanel.TabIndex = 0;
            // 
            // TablesView
            // 
            BackColor = Color.White;
            Controls.Add(tablesFlowPanel);
            Controls.Add(legendPanel);
            Controls.Add(headerPanel);
            Name = "TablesView";
            Size = new Size(800, 500);
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            legendPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void AddLegendItem(System.Windows.Forms.FlowLayoutPanel panel, string text, System.Drawing.Color color)
        {
            var colorBox = new System.Windows.Forms.Panel
            {
                Width = 16,
                Height = 16,
                BackColor = color,
                Margin = new System.Windows.Forms.Padding(8, 4, 4, 4)
            };
            var label = new System.Windows.Forms.Label
            {
                Text = text,
                AutoSize = true,
                Margin = new System.Windows.Forms.Padding(0, 4, 16, 4),
                Font = new System.Drawing.Font("Segoe UI", 9F)
            };
            panel.Controls.Add(colorBox);
            panel.Controls.Add(label);
        }
    }
}
