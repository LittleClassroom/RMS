using System;
using System.Windows.Forms;
using RMS.Controls;
using RMS.Models;
using RMS.Data.SqlServer;

namespace RMS.UI
{
    public partial class ManagerMainForm : Form
    {
        private readonly string _userName;
        private readonly RmsRepository? _repository;
        private DashboardView? _dashboardView;
        private ManageMenuView? _menuView;

        public ManagerMainForm(string userName)
        {
            _userName = userName;
            if (!string.IsNullOrWhiteSpace(RMS.Global.CurrentConnectionString))
            {
                _repository = new RmsRepository(RMS.Global.CurrentConnectionString);
            }
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            lblWelcome.Text = $"Welcome, {_userName}";
            // Load content for the currently selected tab on startup
            tabMain_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            var page = tabMain.SelectedTab;
            if (page == tabOperations)
            {
                managerOperationsTablesView?.RefreshTables();
                return;
            }
            if (page == tabOrders)
            {
                managerStaffOrderView?.Refresh();
                return;
            }
            if (page == tabTables)
            {
                _ = manageTablesView?.ReloadDataAsync();
                return;
            }
            if (page == tabMenu)
            {
                LoadMenuTab();
                return;
            }
            if (page == tabStaff)
            {
                LoadStaffTab();
                return;
            }
            if (page == tabReports)
            {
                LoadReportsTab();
                return;
            }
        }

        private void LoadDashboardTab()
        {
            if (_dashboardView != null)
            {
                _dashboardView.LogoutRequested -= DashboardView_LogoutRequested;
                _dashboardView.StatsRefreshed -= DashboardView_StatsRefreshed;
            }

            _dashboardView = new DashboardView { Dock = DockStyle.Fill };
            _dashboardView.LogoutRequested += DashboardView_LogoutRequested;
            _dashboardView.StatsRefreshed += DashboardView_StatsRefreshed;
            _dashboardView.ConfigureRepository(_repository);
            _dashboardView.RefreshData();
        }

        private void LoadMenuTab()
        {
            tabMenu.Controls.Clear();
            _menuView = new ManageMenuView { Dock = DockStyle.Fill };
            _menuView.ConfigureRepository(_repository);
            tabMenu.Controls.Add(_menuView);
        }

        private void DashboardView_StatsRefreshed(object? sender, DashboardStatsEventArgs e)
        {
            UpdateKpiCards(e.Stats);
        }

        private void DashboardView_LogoutRequested(object? sender, EventArgs e)
        {
            btnLogout.PerformClick();
        }

        private void UpdateKpiCards(DashboardStats stats)
        {
            lblKpiRevenue.Text = stats.TodaysSales.ToString("C");
            lblKpiOrders.Text = stats.OrdersToday.ToString();
            lblKpiGuests.Text = stats.GuestsToday.ToString();
            lblKpiAvgWait.Text = stats.AvgSessionMinutesToday.HasValue
                ? $"{Math.Round(stats.AvgSessionMinutesToday.Value):0} min"
                : "N/A";
            lblKpiLowStock.Text = stats.LowStockItems.ToString();
        }

        private void LoadStaffTab()
        {
            tabStaff.Controls.Clear();
            var staffView = new ManageStaffView { Dock = DockStyle.Fill };
            tabStaff.Controls.Add(staffView);
        }

        private void LoadReportsTab()
        {
            tabReports.Controls.Clear();
            var reports = new ReportsView { Dock = DockStyle.Fill };
            tabReports.Controls.Add(reports);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DialogResult = DialogResult.Abort;
                Close();
            }
        }

        private void dbCredentialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var dlg = new DbCredentialsForm();
            dlg.ShowDialog(this);
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadStaffTab();
            tabMain.SelectedTab = tabStaff;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this,
                "RMS - Restaurant Management System\nVersion 1.0\n\n© 2024",
                "About RMS",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void viewLogsToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            using var dlg = new LogsForm();
            dlg.ShowDialog(this);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
