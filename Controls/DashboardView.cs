using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RMS.Data.SqlServer;
using RMS.Models;

namespace RMS.Controls
{
    public partial class DashboardView : UserControl
    {
        private RmsRepository? _repository;
        private bool _isRefreshing;

        public event EventHandler? LogoutRequested;
        public event EventHandler<DashboardStatsEventArgs>? StatsRefreshed;

        public DashboardView()
        {
            InitializeComponent();
            btnRefresh.Click += (s, e) => RefreshData();
            btnLogout.Click += (s, e) => LogoutRequested?.Invoke(this, EventArgs.Empty);
        }

        public void ConfigureRepository(RmsRepository? repository)
        {
            _repository = repository;
        }

        public void RefreshData()
        {
            if (_repository == null)
            {
                lblUpdatedAt.Text = "Database connection unavailable.";
                lvActivity.Items.Clear();
                return;
            }

            if (_isRefreshing) return;

            try
            {
                _isRefreshing = true;
                btnRefresh.Enabled = false;
                var snapshot = _repository.GetDashboardSnapshot();
                ApplyStats(snapshot.Stats);
                PopulateActivity(snapshot.Activities);
                lblUpdatedAt.Text = $"Updated {DateTime.Now:t}";
                StatsRefreshed?.Invoke(this, new DashboardStatsEventArgs(snapshot.Stats));
            }
            catch (Exception ex)
            {
                lblUpdatedAt.Text = "Failed: " + ex.Message;
            }
            finally
            {
                _isRefreshing = false;
                btnRefresh.Enabled = true;
            }
        }

        private void ApplyStats(DashboardStats stats)
        {
            lblTablesValue.Text = stats.TotalTables == 0
                ? "0 / 0 Occupied"
                : $"{stats.OccupiedTables} / {stats.TotalTables} Occupied";
            lblOrdersValue.Text = $"{stats.OpenOrders} Active";
            lblKitchenValue.Text = $"{stats.KitchenQueue} Tickets";
            lblSalesValue.Text = stats.TodaysSales.ToString("C");
        }

        private void PopulateActivity(IEnumerable<DashboardActivity> activities)
        {
            lvActivity.BeginUpdate();
            lvActivity.Items.Clear();
            foreach (var act in activities)
            {
                var item = new ListViewItem(new[]
                {
                    act.TimestampUtc.ToLocalTime().ToString("HH:mm"),
                    act.Action,
                    act.Details,
                    act.PerformedBy
                });
                lvActivity.Items.Add(item);
            }

            if (lvActivity.Items.Count == 0)
            {
                lvActivity.Items.Add(new ListViewItem(new[]
                {
                    string.Empty,
                    "No recent activity",
                    string.Empty,
                    string.Empty
                }));
            }
            lvActivity.EndUpdate();
        }
    }

    public class DashboardStatsEventArgs : EventArgs
    {
        public DashboardStats Stats { get; }
        public DashboardStatsEventArgs(DashboardStats stats)
        {
            Stats = stats;
        }
    }
}