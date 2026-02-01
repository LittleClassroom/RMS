namespace RMS.Controls
{
    partial class InventoryView
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel alertsPanel;
        private System.Windows.Forms.DataGridView inventoryGrid;

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
            this.alertsPanel = new System.Windows.Forms.Panel();
            this.inventoryGrid = new System.Windows.Forms.DataGridView();

            ((System.ComponentModel.ISupportInitialize)(this.inventoryGrid)).BeginInit();
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
            this.lblTitle.Text = "📦 Inventory Management";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(16, 14);
            this.headerPanel.Controls.Add(this.lblTitle);

            // 
            // alertsPanel - Low stock alerts
            // 
            this.alertsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.alertsPanel.Height = 80;
            this.alertsPanel.BackColor = System.Drawing.Color.FromArgb(255, 243, 224);
            this.alertsPanel.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);

            var alertLabel = new System.Windows.Forms.Label
            {
                Text = "⚠️ Low Stock Alerts",
                Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(230, 81, 0),
                AutoSize = true,
                Location = new System.Drawing.Point(16, 8)
            };

            var alertItems = new System.Windows.Forms.Label
            {
                Text = "• Tomatoes (2.5 kg remaining)  • Chicken Breast (1.2 kg remaining)  • Olive Oil (0.5 L remaining)",
                Font = new System.Drawing.Font("Segoe UI", 9F),
                ForeColor = System.Drawing.Color.FromArgb(100, 100, 100),
                AutoSize = true,
                Location = new System.Drawing.Point(16, 36)
            };

            this.alertsPanel.Controls.Add(alertLabel);
            this.alertsPanel.Controls.Add(alertItems);

            // 
            // inventoryGrid
            // 
            this.inventoryGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inventoryGrid.BackgroundColor = System.Drawing.Color.White;
            this.inventoryGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.inventoryGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.inventoryGrid.RowHeadersVisible = false;
            this.inventoryGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.inventoryGrid.AllowUserToAddRows = false;
            this.inventoryGrid.AllowUserToDeleteRows = false;
            this.inventoryGrid.ReadOnly = true;
            this.inventoryGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // Add columns
            this.inventoryGrid.Columns.Add("Name", "Ingredient");
            this.inventoryGrid.Columns.Add("Stock", "Current Stock");
            this.inventoryGrid.Columns.Add("Unit", "Unit");
            this.inventoryGrid.Columns.Add("Reorder", "Reorder Level");
            this.inventoryGrid.Columns.Add("Status", "Status");
            this.inventoryGrid.Columns.Add("LastUpdated", "Last Updated");

            // Sample data
            this.inventoryGrid.Rows.Add("Tomatoes", "2.5", "kg", "5.0", "⚠️ Low", "Today 10:30");
            this.inventoryGrid.Rows.Add("Chicken Breast", "1.2", "kg", "3.0", "⚠️ Low", "Today 09:15");
            this.inventoryGrid.Rows.Add("Olive Oil", "0.5", "L", "2.0", "⚠️ Low", "Yesterday");
            this.inventoryGrid.Rows.Add("Lettuce", "8.0", "kg", "3.0", "✅ OK", "Today 11:00");
            this.inventoryGrid.Rows.Add("Ground Beef", "12.5", "kg", "5.0", "✅ OK", "Today 08:00");
            this.inventoryGrid.Rows.Add("Pasta", "25.0", "kg", "10.0", "✅ OK", "Yesterday");
            this.inventoryGrid.Rows.Add("Cheese", "4.5", "kg", "2.0", "✅ OK", "Today 10:45");
            this.inventoryGrid.Rows.Add("Onions", "15.0", "kg", "5.0", "✅ OK", "Yesterday");

            // 
            // InventoryView
            // 
            this.Controls.Add(this.inventoryGrid);
            this.Controls.Add(this.alertsPanel);
            this.Controls.Add(this.headerPanel);
            this.Name = "InventoryView";
            this.Size = new System.Drawing.Size(800, 500);
            this.BackColor = System.Drawing.Color.White;

            ((System.ComponentModel.ISupportInitialize)(this.inventoryGrid)).EndInit();
            this.ResumeLayout(false);
        }
    }
}