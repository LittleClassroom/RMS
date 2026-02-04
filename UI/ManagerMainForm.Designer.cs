namespace RMS.UI
{
    partial class ManagerMainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dbCredentialsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewLogsToolStripMenuItem;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel kpiPanel;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabOperations;
        private System.Windows.Forms.TabPage tabMenu;
        private System.Windows.Forms.TabPage tabStaff;
        private System.Windows.Forms.TabPage tabReports;
        private System.Windows.Forms.TabPage tabOrders;
        private System.Windows.Forms.TabPage tabInventory;
        private RMS.Controls.TablesView managerOperationsTablesView;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Label lblKpiRevenue;
        private System.Windows.Forms.Label lblKpiOrders;
        private System.Windows.Forms.Label lblKpiGuests;
        private System.Windows.Forms.Label lblKpiAvgWait;
        private System.Windows.Forms.Label lblKpiLowStock;
        private System.Windows.Forms.TabPage tabTables;
        private RMS.Controls.StaffOrderView managerStaffOrderView;
        private RMS.Controls.ManageTablesView manageTablesView;
        private RMS.Controls.InventoryView managerInventoryView;

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
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            dbCredentialsToolStripMenuItem = new ToolStripMenuItem();
            usersToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            viewLogsToolStripMenuItem = new ToolStripMenuItem();
            headerPanel = new Panel();
            lblTitle = new Label();
            lblWelcome = new Label();
            btnLogout = new Button();
            kpiPanel = new Panel();
            lblKpiRevenue = new Label();
            lblKpiOrders = new Label();
            lblKpiGuests = new Label();
            lblKpiAvgWait = new Label();
            lblKpiLowStock = new Label();
            tabMain = new TabControl();
            tabOperations = new TabPage();
            managerOperationsTablesView = new RMS.Controls.TablesView();
            tabOrders = new TabPage();
            managerStaffOrderView = new RMS.Controls.StaffOrderView();
            tabTables = new TabPage();
            manageTablesView = new RMS.Controls.ManageTablesView();
            tabMenu = new TabPage();
            tabStaff = new TabPage();
            tabReports = new TabPage();
            tabInventory = new TabPage();
            managerInventoryView = new RMS.Controls.InventoryView();
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            menuStrip.SuspendLayout();
            headerPanel.SuspendLayout();
            tabMain.SuspendLayout();
            tabOperations.SuspendLayout();
            tabOrders.SuspendLayout();
            tabTables.SuspendLayout();
            tabInventory.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, settingsToolStripMenuItem, helpToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1280, 24);
            menuStrip.TabIndex = 4;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(92, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { dbCredentialsToolStripMenuItem, usersToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(61, 20);
            settingsToolStripMenuItem.Text = "Settings";
            // 
            // dbCredentialsToolStripMenuItem
            // 
            dbCredentialsToolStripMenuItem.Name = "dbCredentialsToolStripMenuItem";
            dbCredentialsToolStripMenuItem.Size = new Size(160, 22);
            dbCredentialsToolStripMenuItem.Text = "DB Credentials...";
            dbCredentialsToolStripMenuItem.Click += dbCredentialsToolStripMenuItem_Click;
            // 
            // usersToolStripMenuItem
            // 
            usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            usersToolStripMenuItem.Size = new Size(160, 22);
            usersToolStripMenuItem.Text = "Users...";
            usersToolStripMenuItem.Click += usersToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem, viewLogsToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(136, 22);
            aboutToolStripMenuItem.Text = "About...";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // viewLogsToolStripMenuItem
            // 
            viewLogsToolStripMenuItem.Name = "viewLogsToolStripMenuItem";
            viewLogsToolStripMenuItem.Size = new Size(136, 22);
            viewLogsToolStripMenuItem.Text = "View Logs...";
            viewLogsToolStripMenuItem.Click += viewLogsToolStripMenuItem_Click;
            // 
            // headerPanel
            // 
            headerPanel.BackColor = Color.FromArgb(25, 118, 210);
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(lblWelcome);
            headerPanel.Controls.Add(btnLogout);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 24);
            headerPanel.Name = "headerPanel";
            headerPanel.Padding = new Padding(24, 12, 24, 12);
            headerPanel.Size = new Size(1280, 80);
            headerPanel.TabIndex = 2;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(24, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(324, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🏢 Manager Dashboard";
            // 
            // lblWelcome
            // 
            lblWelcome.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Segoe UI", 10F);
            lblWelcome.ForeColor = Color.FromArgb(200, 220, 255);
            lblWelcome.Location = new Point(73, 49);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(126, 19);
            lblWelcome.TabIndex = 1;
            lblWelcome.Text = "Welcome, Manager";
            lblWelcome.Click += lblWelcome_Click;
            // 
            // btnLogout
            // 
            btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogout.BackColor = Color.FromArgb(198, 40, 40);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 9F);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(1150, 24);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(80, 32);
            btnLogout.TabIndex = 2;
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // kpiPanel
            // 
            kpiPanel.BackColor = Color.White;
            kpiPanel.Dock = DockStyle.Top;
            kpiPanel.Location = new Point(0, 104);
            kpiPanel.Name = "kpiPanel";
            kpiPanel.Padding = new Padding(24, 16, 24, 16);
            kpiPanel.Size = new Size(1280, 10);
            kpiPanel.TabIndex = 1;
            // 
            // lblKpiRevenue
            // 
            lblKpiRevenue.Location = new Point(0, 0);
            lblKpiRevenue.Name = "lblKpiRevenue";
            lblKpiRevenue.Size = new Size(100, 23);
            lblKpiRevenue.TabIndex = 0;
            // 
            // lblKpiOrders
            // 
            lblKpiOrders.Location = new Point(0, 0);
            lblKpiOrders.Name = "lblKpiOrders";
            lblKpiOrders.Size = new Size(100, 23);
            lblKpiOrders.TabIndex = 0;
            // 
            // lblKpiGuests
            // 
            lblKpiGuests.Location = new Point(0, 0);
            lblKpiGuests.Name = "lblKpiGuests";
            lblKpiGuests.Size = new Size(100, 23);
            lblKpiGuests.TabIndex = 0;
            // 
            // lblKpiAvgWait
            // 
            lblKpiAvgWait.Location = new Point(0, 0);
            lblKpiAvgWait.Name = "lblKpiAvgWait";
            lblKpiAvgWait.Size = new Size(100, 23);
            lblKpiAvgWait.TabIndex = 0;
            // 
            // lblKpiLowStock
            // 
            lblKpiLowStock.Location = new Point(0, 0);
            lblKpiLowStock.Name = "lblKpiLowStock";
            lblKpiLowStock.Size = new Size(100, 23);
            lblKpiLowStock.TabIndex = 0;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tabOperations);
            tabMain.Controls.Add(tabInventory);
            tabMain.Controls.Add(tabOrders);
            tabMain.Controls.Add(tabTables);
            tabMain.Controls.Add(tabMenu);
            tabMain.Controls.Add(tabStaff);
            tabMain.Controls.Add(tabReports);
            tabMain.Dock = DockStyle.Fill;
            tabMain.Font = new Font("Segoe UI", 10F);
            tabMain.Location = new Point(0, 114);
            tabMain.Name = "tabMain";
            tabMain.Padding = new Point(16, 6);
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1280, 664);
            tabMain.TabIndex = 0;
            tabMain.SelectedIndexChanged += tabMain_SelectedIndexChanged;
            // 
            // tabOperations
            // 
            tabOperations.BackColor = Color.White;
            tabOperations.Controls.Add(managerOperationsTablesView);
            tabOperations.Location = new Point(4, 32);
            tabOperations.Name = "tabOperations";
            tabOperations.Padding = new Padding(12);
            tabOperations.Size = new Size(1272, 628);
            tabOperations.TabIndex = 1;
            tabOperations.Text = "🍽️ Operations";
            // 
            // managerOperationsTablesView
            // 
            managerOperationsTablesView.BackColor = Color.White;
            managerOperationsTablesView.Dock = DockStyle.Fill;
            managerOperationsTablesView.Location = new Point(12, 12);
            managerOperationsTablesView.Name = "managerOperationsTablesView";
            managerOperationsTablesView.Size = new Size(1248, 604);
            managerOperationsTablesView.TabIndex = 0;
            // 
            // tabInventory
            // 
            tabInventory.BackColor = Color.White;
            tabInventory.Controls.Add(managerInventoryView);
            tabInventory.Location = new Point(4, 32);
            tabInventory.Name = "tabInventory";
            tabInventory.Padding = new Padding(12);
            tabInventory.Size = new Size(1272, 628);
            tabInventory.TabIndex = 2;
            tabInventory.Text = "📦 Inventory";
            // 
            // managerInventoryView
            // 
            managerInventoryView.Dock = DockStyle.Fill;
            managerInventoryView.Location = new Point(12, 12);
            managerInventoryView.Name = "managerInventoryView";
            managerInventoryView.Size = new Size(1248, 604);
            managerInventoryView.TabIndex = 0;
            // 
            // tabOrders
            // 
            tabOrders.BackColor = Color.White;
            tabOrders.Controls.Add(managerStaffOrderView);
            tabOrders.Location = new Point(4, 32);
            tabOrders.Name = "tabOrders";
            tabOrders.Padding = new Padding(12);
            tabOrders.Size = new Size(192, 64);
            tabOrders.TabIndex = 6;
            tabOrders.Text = "\U0001f6d2 Orders";
            // 
            // managerStaffOrderView
            // 
            managerStaffOrderView.Dock = DockStyle.Fill;
            managerStaffOrderView.Location = new Point(12, 12);
            managerStaffOrderView.Name = "managerStaffOrderView";
            managerStaffOrderView.Size = new Size(168, 40);
            managerStaffOrderView.TabIndex = 0;
            // 
            // tabTables
            // 
            tabTables.BackColor = Color.White;
            tabTables.Controls.Add(manageTablesView);
            tabTables.Location = new Point(4, 32);
            tabTables.Name = "tabTables";
            tabTables.Padding = new Padding(12);
            tabTables.Size = new Size(192, 64);
            tabTables.TabIndex = 7;
            tabTables.Text = "📅 Tables";
            // 
            // manageTablesView
            // 
            manageTablesView.Dock = DockStyle.Fill;
            manageTablesView.Location = new Point(12, 12);
            manageTablesView.Name = "manageTablesView";
            manageTablesView.Size = new Size(168, 40);
            manageTablesView.TabIndex = 0;
            // 
            // tabMenu
            // 
            tabMenu.BackColor = Color.White;
            tabMenu.Location = new Point(4, 32);
            tabMenu.Name = "tabMenu";
            tabMenu.Padding = new Padding(12);
            tabMenu.Size = new Size(192, 64);
            tabMenu.TabIndex = 3;
            tabMenu.Text = "🍽️ Menu";
            // 
            // tabStaff
            // 
            tabStaff.BackColor = Color.White;
            tabStaff.Location = new Point(4, 32);
            tabStaff.Name = "tabStaff";
            tabStaff.Padding = new Padding(12);
            tabStaff.Size = new Size(192, 64);
            tabStaff.TabIndex = 4;
            tabStaff.Text = "👥 Staff";
            // 
            // tabReports
            // 
            tabReports.BackColor = Color.White;
            tabReports.Location = new Point(4, 32);
            tabReports.Name = "tabReports";
            tabReports.Padding = new Padding(12);
            tabReports.Size = new Size(192, 64);
            tabReports.TabIndex = 5;
            tabReports.Text = "📈 Reports";
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
            statusStrip.Location = new Point(0, 778);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1280, 22);
            statusStrip.TabIndex = 3;
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(238, 17);
            statusLabel.Text = "Connected to database | Last sync: Just now";
            // 
            // ManagerMainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(240, 240, 240);
            ClientSize = new Size(1280, 800);
            Controls.Add(tabMain);
            Controls.Add(kpiPanel);
            Controls.Add(headerPanel);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            MinimumSize = new Size(1100, 700);
            Name = "ManagerMainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RMS - Manager Dashboard";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            tabMain.ResumeLayout(false);
            tabOperations.ResumeLayout(false);
            tabOrders.ResumeLayout(false);
            tabTables.ResumeLayout(false);
            tabInventory.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.Panel CreateKPICard(string title, string initialValue, System.Drawing.Color accentColor, out System.Windows.Forms.Label valueLabel)
         {
             var card = new System.Windows.Forms.Panel
             {
                 Width = 180,
                 Height = 85,
                 Margin = new System.Windows.Forms.Padding(0, 0, 24, 0),
                 BackColor = System.Drawing.Color.FromArgb(250, 250, 250),
                 Padding = new System.Windows.Forms.Padding(12)
             };

             var accentBar = new System.Windows.Forms.Panel
             {
                 Dock = System.Windows.Forms.DockStyle.Left,
                 Width = 4,
                 BackColor = accentColor
             };

             var lblTitle = new System.Windows.Forms.Label
             {
                 Text = title,
                 Font = new System.Drawing.Font("Segoe UI", 9F),
                 ForeColor = System.Drawing.Color.Gray,
                 AutoSize = true,
                 Location = new System.Drawing.Point(16, 8)
             };

             valueLabel = new System.Windows.Forms.Label
             {
                 Text = initialValue,
                 Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold),
                 ForeColor = System.Drawing.Color.FromArgb(33, 33, 33),
                 AutoSize = true,
                 Location = new System.Drawing.Point(16, 36)
             };

             card.Controls.Add(accentBar);
             card.Controls.Add(lblTitle);
             card.Controls.Add(valueLabel);

             return card;
         }
    }
}
