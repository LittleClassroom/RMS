using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RMS;
using RMS.Data.SqlServer;
using RMS.Models;
using RMS.UI;

namespace RMS.Controls
{
    public partial class StaffOrderView : UserControl
    {
        private readonly List<ActiveOrderInfo> _orders = new();
        private bool _isLoading;
        private bool _dataLoaded;

        public StaffOrderView()
        {
            InitializeComponent();
            ConfigureFilters();
            ConfigureListView();
            WireUpEvents();
        }

        private void ConfigureFilters()
        {
            cbStatus.Items.Clear();
            cbStatus.Items.Add(new StatusFilter("All Statuses", null));
            cbStatus.Items.Add(new StatusFilter("Created", 0));
            cbStatus.Items.Add(new StatusFilter("In Kitchen", 1));
            cbStatus.Items.Add(new StatusFilter("Ready", 2));
            cbStatus.Items.Add(new StatusFilter("Closed / Paid", 3));
            cbStatus.SelectedIndex = 0;

            try { tbSearch.PlaceholderText = "Search order # or table"; } catch { }
        }

        private void ConfigureListView()
        {
            lvOrders.Columns.Clear();
            lvOrders.Columns.Add("Order #", 80, HorizontalAlignment.Left);
            lvOrders.Columns.Add("Table", 100, HorizontalAlignment.Left);
            lvOrders.Columns.Add("Location", 120, HorizontalAlignment.Left);
            lvOrders.Columns.Add("Status", 130, HorizontalAlignment.Left);
            lvOrders.Columns.Add("Subtotal", 90, HorizontalAlignment.Right);
            lvOrders.Columns.Add("Tax", 70, HorizontalAlignment.Right);
            lvOrders.Columns.Add("Total", 90, HorizontalAlignment.Right);
            lvOrders.Columns.Add("Created", 150, HorizontalAlignment.Left);
        }

        private void WireUpEvents()
        {
            cbStatus.SelectedIndexChanged += (_, __) => ApplyFilters();
            tbSearch.TextChanged += (_, __) => ApplyFilters();
            lvOrders.DoubleClick += LvOrders_DoubleClick;
            btnRefresh.Click += async (_, __) => await LoadOrdersAsync();
        }

        protected override async void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!IsInDesignMode && !_dataLoaded)
            {
                _dataLoaded = true;
                await LoadOrdersAsync();
            }
        }

        private bool IsInDesignMode => LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode;

        private async Task LoadOrdersAsync()
        {
            if (_isLoading || IsInDesignMode)
            {
                return;
            }

            if (!TryGetConnection(out var connectionString))
            {
                return;
            }

            try
            {
                _isLoading = true;
                SetLoadingState(true);
                var repo = new RmsRepository(connectionString);
                var result = await Task.Run(() => repo.GetActiveOrders());
                _orders.Clear();
                _orders.AddRange(result);
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to load orders: " + ex.Message, "Orders", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
                _isLoading = false;
            }
        }

        private void SetLoadingState(bool loading)
        {
            btnRefresh.Enabled = !loading;
            if (loading)
            {
                lblSummary.Text = "Loading orders...";
            }
        }

        private void ApplyFilters()
        {
            IEnumerable<ActiveOrderInfo> query = _orders;

            if (cbStatus.SelectedItem is StatusFilter filter && filter.Status.HasValue)
            {
                query = query.Where(o => o.Status == filter.Status.Value);
            }

            var term = tbSearch.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(term))
            {
                var lowered = term.ToLowerInvariant();
                query = query.Where(o =>
                    o.OrderId.ToString().Contains(term!, StringComparison.OrdinalIgnoreCase) ||
                    (o.TableCode ?? string.Empty).ToLowerInvariant().Contains(lowered) ||
                    (o.Location ?? string.Empty).ToLowerInvariant().Contains(lowered));
            }

            var filtered = query
                .OrderBy(o => o.IsPaid ? 1 : 0)
                .ThenByDescending(o => o.CreatedAtUtc)
                .ToList();

            RenderOrders(filtered);
        }

        private void RenderOrders(IReadOnlyList<ActiveOrderInfo> orders)
        {
            lvOrders.BeginUpdate();
            lvOrders.Items.Clear();
            foreach (var order in orders)
            {
                var item = new ListViewItem(order.OrderId.ToString()) { Tag = order };
                item.SubItems.Add(string.IsNullOrWhiteSpace(order.TableCode) ? "N/A" : order.TableCode);
                item.SubItems.Add(string.IsNullOrWhiteSpace(order.Location) ? "-" : order.Location);
                item.SubItems.Add(GetStatusText(order.Status, order.IsPaid));
                item.SubItems.Add(order.Subtotal.ToString("C"));
                item.SubItems.Add(order.Tax.ToString("C"));
                item.SubItems.Add(order.Total.ToString("C"));
                item.SubItems.Add(order.CreatedAtUtc.ToLocalTime().ToString("g"));
                lvOrders.Items.Add(item);
            }
            lvOrders.EndUpdate();

            lblEmpty.Visible = orders.Count == 0 && !_isLoading;
            if (lblEmpty.Visible)
            {
                lblEmpty.BringToFront();
            }
            else
            {
                lvOrders.BringToFront();
            }

            lblSummary.Text = orders.Count == 1 ? "1 order" : $"{orders.Count} orders";
        }

        private async void LvOrders_DoubleClick(object? sender, EventArgs e)
        {
            if (lvOrders.SelectedItems.Count == 0)
            {
                return;
            }

            if (lvOrders.SelectedItems[0].Tag is not ActiveOrderInfo info)
            {
                return;
            }

            await ShowOrderDetailsAsync(info);
        }

        private async Task ShowOrderDetailsAsync(ActiveOrderInfo order)
        {
            if (!TryGetConnection(out var connectionString))
            {
                return;
            }

            try
            {
                var repo = new RmsRepository(connectionString);
                var header = await Task.Run(() => repo.GetOrderById(order.OrderId));
                if (header == null)
                {
                    MessageBox.Show(this, "Order could not be found.", "Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadOrdersAsync();
                    return;
                }

                var lines = await Task.Run(() => repo.GetOrderLines(order.OrderId));
                using var dlg = new OrderDetailsDialog(header, lines);
                dlg.ShowDialog(FindForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to load order details: " + ex.Message, "Details", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string GetStatusText(byte status, bool isPaid) => status switch
        {
            1 => "In Kitchen",
            2 => "Ready",
            3 => isPaid ? "Closed / Paid" : "Closed",
            4 => "Cancelled",
            _ => "Created"
        };

        private bool TryGetConnection(out string connectionString)
        {
            connectionString = Global.CurrentConnectionString ?? string.Empty;
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                MessageBox.Show(this, "Database connection is not configured.", "Connection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private sealed class StatusFilter
        {
            public string Label { get; }
            public byte? Status { get; }

            public StatusFilter(string label, byte? status)
            {
                Label = label;
                Status = status;
            }

            public override string ToString() => Label;
        }
    }
}