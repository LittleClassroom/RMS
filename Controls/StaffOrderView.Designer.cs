namespace RMS.Controls
{
    partial class StaffOrderView
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel layout;
        private System.Windows.Forms.FlowLayoutPanel headerPanel;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.ListView lvOrders;
        private System.Windows.Forms.Label lblEmpty;
        private System.Windows.Forms.Label lblSummary;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            layout = new TableLayoutPanel();
            headerPanel = new FlowLayoutPanel();
            cbStatus = new ComboBox();
            tbSearch = new TextBox();
            btnRefresh = new Button();
            contentPanel = new Panel();
            lvOrders = new ListView();
            lblEmpty = new Label();
            lblSummary = new Label();
            layout.SuspendLayout();
            headerPanel.SuspendLayout();
            contentPanel.SuspendLayout();
            SuspendLayout();
            // 
            // layout
            // 
            layout.ColumnCount = 1;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.Controls.Add(headerPanel, 0, 0);
            layout.Controls.Add(contentPanel, 0, 1);
            layout.Controls.Add(lblSummary, 0, 2);
            layout.Dock = DockStyle.Fill;
            layout.Location = new Point(0, 0);
            layout.Name = "layout";
            layout.RowCount = 3;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            layout.Size = new Size(900, 600);
            layout.TabIndex = 0;
            // 
            // headerPanel
            // 
            headerPanel.Controls.Add(cbStatus);
            headerPanel.Controls.Add(tbSearch);
            headerPanel.Controls.Add(btnRefresh);
            headerPanel.Dock = DockStyle.Fill;
            headerPanel.Location = new Point(3, 3);
            headerPanel.Name = "headerPanel";
            headerPanel.Padding = new Padding(12, 12, 12, 0);
            headerPanel.Size = new Size(894, 48);
            headerPanel.TabIndex = 0;
            headerPanel.WrapContents = false;
            // 
            // cbStatus
            // 
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.FormattingEnabled = true;
            cbStatus.Location = new Point(12, 12);
            cbStatus.Margin = new Padding(0, 0, 0, 4);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(170, 23);
            cbStatus.TabIndex = 1;
            // 
            // tbSearch
            // 
            tbSearch.Location = new Point(190, 12);
            tbSearch.Margin = new Padding(8, 0, 0, 4);
            tbSearch.Name = "tbSearch";
            tbSearch.Size = new Size(220, 23);
            tbSearch.TabIndex = 2;
            // 
            // btnRefresh
            // 
            btnRefresh.AutoSize = true;
            btnRefresh.Location = new Point(418, 12);
            btnRefresh.Margin = new Padding(8, 0, 0, 4);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(63, 25);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // contentPanel
            // 
            contentPanel.Controls.Add(lvOrders);
            contentPanel.Controls.Add(lblEmpty);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(3, 57);
            contentPanel.Name = "contentPanel";
            contentPanel.Padding = new Padding(12);
            contentPanel.Size = new Size(894, 510);
            contentPanel.TabIndex = 1;
            // 
            // lvOrders
            // 
            lvOrders.Dock = DockStyle.Fill;
            lvOrders.FullRowSelect = true;
            lvOrders.Location = new Point(12, 12);
            lvOrders.Margin = new Padding(0);
            lvOrders.MultiSelect = false;
            lvOrders.Name = "lvOrders";
            lvOrders.Size = new Size(870, 486);
            lvOrders.TabIndex = 0;
            lvOrders.UseCompatibleStateImageBehavior = false;
            lvOrders.View = View.Details;
            // 
            // lblEmpty
            // 
            lblEmpty.Dock = DockStyle.Fill;
            lblEmpty.Font = new Font("Segoe UI", 12F);
            lblEmpty.ForeColor = Color.Gray;
            lblEmpty.Location = new Point(12, 12);
            lblEmpty.Name = "lblEmpty";
            lblEmpty.Size = new Size(870, 486);
            lblEmpty.TabIndex = 1;
            lblEmpty.Text = "No active orders found.";
            lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
            lblEmpty.Visible = false;
            // 
            // lblSummary
            // 
            lblSummary.Dock = DockStyle.Fill;
            lblSummary.Location = new Point(3, 570);
            lblSummary.Name = "lblSummary";
            lblSummary.Padding = new Padding(12, 0, 0, 0);
            lblSummary.Size = new Size(894, 30);
            lblSummary.TabIndex = 2;
            lblSummary.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // StaffOrderView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(layout);
            Name = "StaffOrderView";
            Size = new Size(900, 600);
            layout.ResumeLayout(false);
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            contentPanel.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
