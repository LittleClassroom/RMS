namespace RMS.Controls
{
    partial class TablesView
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.FlowLayoutPanel tablesFlowPanel;
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
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.legendPanel = new System.Windows.Forms.Panel();
            this.tablesFlowPanel = new System.Windows.Forms.FlowLayoutPanel();

            this.SuspendLayout();

            // headerPanel
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Height = 60;
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.headerPanel.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);

            // lblTitle
            this.lblTitle.Text = "🪑 Table Status";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(16, 14);
            this.headerPanel.Controls.Add(this.lblTitle);

            // legendPanel
            this.legendPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.legendPanel.Height = 40;
            this.legendPanel.BackColor = System.Drawing.Color.White;
            this.legendPanel.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);

            var legendFlow = new System.Windows.Forms.FlowLayoutPanel
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight,
                WrapContents = false
            };

            AddLegendItem(legendFlow, "Available", System.Drawing.Color.FromArgb(76, 175, 80));
            AddLegendItem(legendFlow, "Reserved", System.Drawing.Color.FromArgb(255, 193, 7));
            AddLegendItem(legendFlow, "Occupied", System.Drawing.Color.FromArgb(244, 67, 54));
            AddLegendItem(legendFlow, "Needs Cleaning", System.Drawing.Color.FromArgb(156, 39, 176));
            AddLegendItem(legendFlow, "Out of Service", System.Drawing.Color.FromArgb(158, 158, 158));

            this.legendPanel.Controls.Add(legendFlow);

            // tablesFlowPanel
            this.tablesFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablesFlowPanel.AutoScroll = true;
            this.tablesFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.tablesFlowPanel.WrapContents = true;
            this.tablesFlowPanel.Padding = new System.Windows.Forms.Padding(16);
            this.tablesFlowPanel.BackColor = System.Drawing.Color.White;

            // TablesView
            this.Controls.Add(this.tablesFlowPanel);
            this.Controls.Add(this.legendPanel);
            this.Controls.Add(this.headerPanel);
            this.Name = "TablesView";
            this.Size = new System.Drawing.Size(800, 500);
            this.BackColor = System.Drawing.Color.White;
            this.ResumeLayout(false);
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
