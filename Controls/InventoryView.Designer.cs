namespace RMS.Controls
{
    partial class InventoryView
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel alertsPanel;
        private System.Windows.Forms.Label lblAlertsList;
        private System.Windows.Forms.TabControl tabInventoryMain;
        private System.Windows.Forms.TabPage tabStock;
        private System.Windows.Forms.TabPage tabHistory;
        private System.Windows.Forms.Panel panelStockActions;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ComboBox cbCategory;
        private System.Windows.Forms.ComboBox cbSubcategory;
        private System.Windows.Forms.Button btnAddNew;
        private System.Windows.Forms.DataGridView inventoryGrid;
        private System.Windows.Forms.ListView lvHistory;

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
            this.lblAlertsList = new System.Windows.Forms.Label();
            this.tabInventoryMain = new System.Windows.Forms.TabControl();
            this.tabStock = new System.Windows.Forms.TabPage();
            this.inventoryGrid = new System.Windows.Forms.DataGridView();
            this.panelStockActions = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cbCategory = new System.Windows.Forms.ComboBox();
            this.cbSubcategory = new System.Windows.Forms.ComboBox();
            this.btnAddNew = new System.Windows.Forms.Button();
            this.tabHistory = new System.Windows.Forms.TabPage();
            this.lvHistory = new System.Windows.Forms.ListView();

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

            this.lblAlertsList.AutoSize = true;
            this.lblAlertsList.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAlertsList.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblAlertsList.Location = new System.Drawing.Point(16, 36);
            this.lblAlertsList.Text = string.Empty;
            this.alertsPanel.Controls.Add(alertLabel);
            this.alertsPanel.Controls.Add(this.lblAlertsList);

            // 
            // tabInventoryMain
            // 
            this.tabInventoryMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabInventoryMain.Controls.Add(this.tabStock);
            this.tabInventoryMain.Controls.Add(this.tabHistory);

            // 
            // tabStock
            // 
            this.tabStock.Padding = new System.Windows.Forms.Padding(12);
            this.tabStock.Text = "Stock";
            this.tabStock.UseVisualStyleBackColor = true;

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
            this.inventoryGrid.Columns.Add("Name", "Item");
            this.inventoryGrid.Columns.Add("Category", "Category");
            this.inventoryGrid.Columns.Add("Subcategory", "Subcategory");
            this.inventoryGrid.Columns.Add("Stock", "Current Stock");
            this.inventoryGrid.Columns.Add("Unit", "Unit");
            this.inventoryGrid.Columns.Add("Reorder", "Reorder Level");
            this.inventoryGrid.Columns.Add("Status", "Status");

            // 
            // panelStockActions
            // 
            this.panelStockActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStockActions.Height = 42;
            this.panelStockActions.Padding = new System.Windows.Forms.Padding(12);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.AutoSize = true;
            this.btnRefresh.Location = new System.Drawing.Point(12, 6);
            this.panelStockActions.Controls.Add(this.btnRefresh);

            this.cbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCategory.Location = new System.Drawing.Point(92, 8);
            this.cbCategory.Size = new System.Drawing.Size(160, 23);
            this.panelStockActions.Controls.Add(this.cbCategory);

            this.cbSubcategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSubcategory.Location = new System.Drawing.Point(260, 8);
            this.cbSubcategory.Size = new System.Drawing.Size(160, 23);
            this.panelStockActions.Controls.Add(this.cbSubcategory);

            this.btnAddNew.Text = "Add Item";
            this.btnAddNew.AutoSize = true;
            this.btnAddNew.Location = new System.Drawing.Point(432, 6);
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            this.panelStockActions.Controls.Add(this.btnAddNew);

            // assemble stock tab
            this.tabStock.Controls.Add(this.inventoryGrid);
            this.tabStock.Controls.Add(this.panelStockActions);

            // 
            // tabHistory
            // 
            this.tabHistory.Padding = new System.Windows.Forms.Padding(12);
            this.tabHistory.Text = "History";

            // 
            // lvHistory
            // 
            this.lvHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvHistory.View = System.Windows.Forms.View.Details;
            this.lvHistory.FullRowSelect = true;
            this.lvHistory.Columns.Add("Date", 140);
            this.lvHistory.Columns.Add("Type", 80);
            this.lvHistory.Columns.Add("Qty", 80);
            this.lvHistory.Columns.Add("Reference", 200);
            this.lvHistory.Columns.Add("Source", 120);

            // assemble history tab
            this.tabHistory.Controls.Add(this.lvHistory);

            // 
            // InventoryView
            // 
            this.Controls.Add(this.tabInventoryMain);
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