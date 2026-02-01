using System;
using System.Drawing;
using System.Windows.Forms;
using RMS.Models;

namespace RMS.Controls
{
    public partial class SidebarControl : UserControl
    {
        public event EventHandler? DashboardClicked;
        public event EventHandler? TablesClicked;
        public event EventHandler? OrdersClicked;
        public event EventHandler? KitchenClicked;
        public event EventHandler? InventoryClicked;
        public event EventHandler? ReportsClicked;
        public event EventHandler? SettingsClicked;
        public event EventHandler? LogoutClicked;

        private Button? _selectedButton;
        private UserRole _currentRole = UserRole.Service;

        public SidebarControl()
        {
            InitializeComponent();
        }

        public void SetUserRole(UserRole role)
        {
            _currentRole = role;
            UpdateButtonVisibility();
        }

        public void SetUserInfo(string displayName, UserRole role)
        {
            _currentRole = role;
            lblUserName.Text = displayName;
            lblUserRole.Text = role.ToString();
            UpdateButtonVisibility();
        }

        private void UpdateButtonVisibility()
        {
            // Dashboard visible only to Manager
            btnDashboard.Visible = _currentRole == UserRole.Manager;
            btnTables.Visible = true;
            btnOrders.Visible = true;

            // Kitchen visible to Chef and Manager
            btnKitchen.Visible = _currentRole == UserRole.Chef || _currentRole == UserRole.Manager;

            // Inventory visible to Chef and Manager
            btnInventory.Visible = _currentRole == UserRole.Chef || _currentRole == UserRole.Manager;

            // Reports visible only to Manager
            btnReports.Visible = _currentRole == UserRole.Manager;

            // Settings visible to all roles (allow staff to view DB info)
            btnSettings.Visible = true;
        }

        public void SelectButton(string buttonName)
        {
            Button? btn = buttonName.ToLower() switch
            {
                "dashboard" => btnDashboard,
                "tables" => btnTables,
                "orders" => btnOrders,
                "kitchen" => btnKitchen,
                "inventory" => btnInventory,
                "reports" => btnReports,
                "settings" => btnSettings,
                _ => null
            };

            if (btn != null)
            {
                SetSelectedButton(btn);
            }
        }

        private void SetSelectedButton(Button btn)
        {
            // Reset previous selection
            if (_selectedButton != null)
            {
                _selectedButton.BackColor = Color.Transparent;
                _selectedButton.ForeColor = Color.FromArgb(200, 200, 200);
            }

            // Set new selection
            _selectedButton = btn;
            _selectedButton.BackColor = Color.FromArgb(55, 55, 65);
            _selectedButton.ForeColor = Color.White;
        }

        private void btnDashboard_Click(object? sender, EventArgs e)
        {
            SetSelectedButton(btnDashboard);
            DashboardClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnTables_Click(object? sender, EventArgs e)
        {
            SetSelectedButton(btnTables);
            TablesClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnOrders_Click(object? sender, EventArgs e)
        {
            SetSelectedButton(btnOrders);
            OrdersClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnKitchen_Click(object? sender, EventArgs e)
        {
            SetSelectedButton(btnKitchen);
            KitchenClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnInventory_Click(object? sender, EventArgs e)
        {
            SetSelectedButton(btnInventory);
            InventoryClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnReports_Click(object? sender, EventArgs e)
        {
            SetSelectedButton(btnReports);
            ReportsClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnSettings_Click(object? sender, EventArgs e)
        {
            SetSelectedButton(btnSettings);
            SettingsClicked?.Invoke(this, EventArgs.Empty);
        }

        private void btnLogout_Click(object? sender, EventArgs e)
        {
            LogoutClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}