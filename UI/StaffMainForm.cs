using System;
using System.Linq;
using System.Windows.Forms;
using RMS.Models;
using RMS.Controls;
using RMS.Data.SqlServer;

namespace RMS.UI
{
    public partial class StaffMainForm : Form
    {
        private readonly UserRole _userRole;
        private readonly string _userName;
        private UserControl? _currentView;

        public StaffMainForm(string userName, UserRole userRole)
        {
            _userName = userName;
            _userRole = userRole;
            InitializeComponent();
            SetupSidebar();
            ShowTables();
        }

        private void SetupSidebar()
        {
            sidebar.SetUserInfo(_userName, _userRole);
            sidebar.DashboardClicked += (s, e) => ShowDashboard();
            sidebar.TablesClicked += (s, e) => ShowTables();
            sidebar.OrdersClicked += (s, e) => ShowOrders();
            sidebar.KitchenClicked += (s, e) => ShowKitchen();
            sidebar.InventoryClicked += (s, e) => ShowInventory();
            sidebar.ReportsClicked += (s, e) => ShowReports();
            sidebar.SettingsClicked += (s, e) => ShowSettings();
            sidebar.LogoutClicked += (s, e) => HandleLogout();

            sidebar.SelectButton("tables");
        }

        private void ShowView(UserControl view)
        {
            if (_currentView != null)
            {
                contentPanel.Controls.Remove(_currentView);
                _currentView.Dispose();
            }

            _currentView = view;
            _currentView.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(_currentView);
            UpdateTitle(view);
        }

        private void UpdateTitle(UserControl view)
        {
            // Update the form and label titles based on the current view
            string viewName = view.GetType().Name.Replace("View", "");
            this.Text = $"RMS - {viewName}";
            lblPageTitle.Text = viewName;
        }

        private void ShowDashboard()
        {
            // Display the dashboard view
            ShowView(new DashboardView());
            sidebar.SelectButton("dashboard");
        }

        private void ShowTables()
        {
            // Display the tables view
            var tv = new TablesView();
            ShowView(tv);
            try { tv.RefreshTables(); } catch { }
            sidebar.SelectButton("tables");
        }

        private void ShowOrders()
        {
            // Display the staff order view
            ShowView(new StaffOrderView());
            sidebar.SelectButton("orders");
        }

        private void ShowKitchen()
        {
            // Display the kitchen view
            ShowView(new KitchenView());
            sidebar.SelectButton("kitchen");
        }

        private void ShowInventory()
        {
            // Display the inventory view
            var inv = new InventoryView();
            // configure repository if connection available so inventory history and transactions load
            try
            {
                if (!string.IsNullOrWhiteSpace(RMS.Global.CurrentConnectionString))
                {
                    var repo = new RmsRepository(RMS.Global.CurrentConnectionString);
                    inv.ConfigureRepository(repo);
                }
            }
            catch { }

            ShowView(inv);
            sidebar.SelectButton("inventory");
        }

        private void ShowReports()
        {
            // Display the reports view
            ShowView(new ReportsView());
            sidebar.SelectButton("reports");
        }

        private void ShowSettings()
        {
            // embed SettingsView into main panel instead of modal dialog
            var settings = new SettingsView(_userRole);
            ShowView(settings);
        }

        private void HandleLogout()
        {
            // Handle the logout process
            var result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        private void btnNewOrder_Click(object sender, EventArgs e)
        {
            // Shortcut button to display the orders view
            ShowOrders();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Dispose the current view when the form is closing
            _currentView?.Dispose();
            base.OnFormClosing(e);
        }

        private void sidebar_Load(object sender, EventArgs e)
        {

        }

        private void contentPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LoadOrderingView()
        {
            // Clear existing content and add ordering interface
            // (Assuming there's a main panel or container in StaffMainForm)
            var existingContent = this.Controls.OfType<Panel>().FirstOrDefault();
            if (existingContent != null)
            {
                existingContent.Controls.Clear();
                
                var staffOrderView = new RMS.Controls.StaffOrderView { Dock = DockStyle.Fill };
                existingContent.Controls.Add(staffOrderView);
            }
        }
    }
}
