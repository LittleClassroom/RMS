using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using RMS.Models;
using System.Reflection;
using RMS.Data.SqlServer;
using RMS.UI;

namespace RMS.Controls
{
    public partial class TablesView : UserControl
    {
        private List<TableInfo> _tables = new List<TableInfo>();
        private readonly bool _isDesigner;
        private bool _isLoading;
        private System.Windows.Forms.Timer _refreshTimer;
        private int _refreshSeconds = 3; // countdown interval (seconds)
        private bool _autoRefreshEnabled = false;
        private ContextMenuStrip? _tableContextMenu;
        private Button? _lastTableButton;

        public TablesView()
        {
            InitializeComponent();
             _isDesigner = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
             // reduce flicker by enabling double buffering for this control
            try
            {
                SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
                UpdateStyles();

                // also enable double buffering on the internal FlowLayoutPanel (protected property)
                var pinfo = typeof(FlowLayoutPanel).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                pinfo?.SetValue(tablesFlowPanel, true, null);
            }
            catch { }

             if (!_isDesigner)
             {
                 Load += TablesView_Load;
             }
            else
            {
                RenderDesignState();
            }

            // start timer but only perform auto-refresh when enabled
            try
            {
                _refreshTimer = new System.Windows.Forms.Timer();
                _refreshTimer.Interval = 1000; // 1s tick
                _refreshTimer.Tick += (s, e) => OnRefreshTimerTick();
                _refreshTimer.Start();
            }
            catch { }

            // create context menu for table actions (right-click)
            try
            {
                _tableContextMenu = new ContextMenuStrip();
                _tableContextMenu.Items.Add("Set Available").Tag = TableStatus.Available;
                _tableContextMenu.Items.Add("Set Reserved").Tag = TableStatus.Reserved;
                _tableContextMenu.Items.Add("Set Occupied").Tag = TableStatus.Occupied;
                _tableContextMenu.Items.Add("Set Needs Cleaning").Tag = TableStatus.NeedsCleaning;
                _tableContextMenu.Items.Add("Set Out Of Service").Tag = TableStatus.OutOfService;
                _tableContextMenu.ItemClicked += TableContextMenu_ItemClicked;
            }
            catch { }
        }

        private async void TablesView_Load(object? sender, EventArgs e)
        {
            await LoadTablesAsync();
        }

        public void RefreshTables()
        {
            if (_isDesigner)
            {
                return;
            }

            _ = LoadTablesAsync();
        }

        private async Task LoadTablesAsync()
        {
            if (_isLoading || _isDesigner) return;

            var connString = Global.CurrentConnectionString;
            if (string.IsNullOrWhiteSpace(connString))
            {
                MessageBox.Show(this, "Database connection not available.", "Tables", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _isLoading = true;
                var tables = await Task.Run(() => FetchTables(connString));
                _tables = tables;
                PopulateButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to load tables: " + ex.Message, "Tables", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _tables = new List<TableInfo>();
                PopulateButtons();
            }
            finally
            {
                _isLoading = false;
                // reset countdown
                _refreshSeconds = 3;
            }
        }

        private void RenderDesignState()
        {
            _tables = new List<TableInfo>
            {
                new TableInfo("T01", "Main", 4, TableStatus.Available),
                new TableInfo("T02", "Patio", 6, TableStatus.Occupied),
                new TableInfo("T03", "Booth", 2, TableStatus.Reserved)
            };
            PopulateButtons();
        }

        private static List<TableInfo> FetchTables(string connString)
        {
            var result = new List<TableInfo>();
            using var cn = new SqlConnection(connString);
            cn.Open();
            using var cmd = new SqlCommand("SELECT TableId, Code, Location, Capacity, Status, Notes FROM dbo.Tables ORDER BY Code", cn);
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var capacity = rdr.IsDBNull(3) ? 0 : Convert.ToInt32(rdr.GetValue(3));
                var table = new TableInfo(
                    rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1),
                    rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2),
                    capacity,
                    (TableStatus)rdr.GetByte(4))
                {
                    TableId = rdr.GetInt32(0),
                    Notes = rdr.IsDBNull(5) ? null : rdr.GetString(5)
                };
                result.Add(table);
            }
            return result;
        }

        private void PopulateButtons()
        {
            // minimize layout passes to avoid UI flicker
            tablesFlowPanel.SuspendLayout();
            try
            {
                tablesFlowPanel.Controls.Clear();
                foreach (var t in _tables)
                {
                    var btn = CreateTableButton(t);
                    tablesFlowPanel.Controls.Add(btn);
                }
            }
            finally
            {
                tablesFlowPanel.ResumeLayout(false);
                tablesFlowPanel.PerformLayout();
                tablesFlowPanel.Invalidate();
            }
        }

        private Button CreateTableButton(TableInfo t)
        {
            var btn = new Button
            {
                Width = 140,
                Height = 100,
                Margin = new Padding(8),
                BackColor = GetStatusColor(t.Status),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Tag = t,
                Text = $"{t.Code}\n{t.Location}\n👥 {t.Capacity} | {t.Status}",
                TextAlign = ContentAlignment.MiddleCenter
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.Click += TableButton_Click;
            // show context menu on right-click
            btn.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Right && _tableContextMenu != null)
                {
                    _lastTableButton = btn;
                    _tableContextMenu.Show(btn, e.Location);
                }
            };

            return btn;
        }

        private void TableButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn) return;
            if (btn.Tag is not TableInfo table) return;

            // If table is Available or Occupied - open OrderView in a side panel dialog
            if (table.Status == TableStatus.Available || table.Status == TableStatus.Occupied || table.Status == TableStatus.Reserved)
            {
                ShowOrderSplitView(table, btn);
                return;
            }

            ShowTableDialog(table, btn);
        }

        private void ShowOrderSplitView(TableInfo table, Button sourceButton)
        {
            using var dlg = new Form();
            dlg.FormBorderStyle = FormBorderStyle.Sizable;
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.ClientSize = new Size(900, 600);
            dlg.Text = $"Table {table.Code} - Orders";

            var orderPanel = new OrderPanel { Dock = DockStyle.Fill };
            dlg.Controls.Add(orderPanel);

            // load table order (fire-and-forget; method completes synchronously currently)
            _ = orderPanel.LoadTableOrder(table);

            dlg.ShowDialog(this);

            // After closing, refresh button
            UpdateButtonFromTable(sourceButton, table);
        }

        private void ShowTableDialog(TableInfo table, Button sourceButton)
        {
            using var dlg = new Form();
            dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.ClientSize = new Size(360, 220);
            dlg.Text = $"Table {table.Code}";
            dlg.MaximizeBox = false;
            dlg.MinimizeBox = false;

            var lblInfo = new Label
            {
                Text = $"Location: {table.Location}    Capacity: {table.Capacity}",
                AutoSize = true,
                Location = new Point(12, 12)
            };

            var lblStatus = new Label
            {
                Text = $"Status: {table.Status}",
                AutoSize = true,
                Location = new Point(12, 36)
            };

            var lblPeople = new Label
            {
                Text = "People seated:",
                AutoSize = true,
                Location = new Point(12, 72)
            };

            var tbPeople = new TextBox
            {
                Location = new Point(120, 68),
                Width = 200
            };

            // Populate current seated if occupied
            if (table.Status == TableStatus.Occupied && table.SeatedCount > 0)
            {
                tbPeople.Text = table.SeatedCount.ToString();
            }

            // Buttons
            var btnPrimary = new Button
            {
                Width = 100,
                Height = 30,
                Location = new Point(60, 140)
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                Width = 100,
                Height = 30,
                Location = new Point(200, 140),
                DialogResult = DialogResult.Cancel
            };

            dlg.Controls.Add(lblInfo);
            dlg.Controls.Add(lblStatus);
            dlg.Controls.Add(lblPeople);
            dlg.Controls.Add(tbPeople);
            dlg.Controls.Add(btnPrimary);
            dlg.Controls.Add(btnCancel);

            // Configure primary action based on status
            switch (table.Status)
            {
                case TableStatus.Available:
                    btnPrimary.Text = "Occupy";
                    btnPrimary.Click += (s, e) =>
                    {
                        if (!int.TryParse(tbPeople.Text.Trim(), out int seated) || seated <= 0)
                        {
                            MessageBox.Show(dlg, "Enter a valid number of people (>=1)", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (seated > table.Capacity)
                        {
                            MessageBox.Show(dlg, $"Number of people exceeds table capacity ({table.Capacity}).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        table.SeatedCount = seated;
                        table.Status = TableStatus.Occupied;
                        UpdateButtonFromTable(sourceButton, table);
                        dlg.DialogResult = DialogResult.OK;
                        dlg.Close();
                    };
                    break;
                case TableStatus.Reserved:
                    btnPrimary.Text = "Seat & Occupy";
                    btnPrimary.Click += (s, e) =>
                    {
                        if (!int.TryParse(tbPeople.Text.Trim(), out int seated) || seated <= 0)
                        {
                            MessageBox.Show(dlg, "Enter a valid number of people (>=1)", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (seated > table.Capacity)
                        {
                            MessageBox.Show(dlg, $"Number of people exceeds table capacity ({table.Capacity}).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        table.SeatedCount = seated;
                        table.Status = TableStatus.Occupied;
                        UpdateButtonFromTable(sourceButton, table);
                        dlg.DialogResult = DialogResult.OK;
                        dlg.Close();
                    };
                    break;
                case TableStatus.Occupied:
                    btnPrimary.Text = "View / Add Order";
                    btnPrimary.Click += (s, e) =>
                    {
                        dlg.DialogResult = DialogResult.OK;
                        dlg.Close();
                    };
                    break;
                case TableStatus.NeedsCleaning:
                    btnPrimary.Text = "Mark Available";
                    btnPrimary.Click += (s, e) =>
                    {
                        table.Status = TableStatus.Available;
                        table.SeatedCount = 0;
                        UpdateButtonFromTable(sourceButton, table);
                        dlg.DialogResult = DialogResult.OK;
                        dlg.Close();
                    };
                    break;
                case TableStatus.OutOfService:
                    btnPrimary.Text = "Mark Available";
                    btnPrimary.Click += (s, e) =>
                    {
                        table.Status = TableStatus.Available;
                        table.SeatedCount = 0;
                        UpdateButtonFromTable(sourceButton, table);
                        dlg.DialogResult = DialogResult.OK;
                        dlg.Close();
                    };
                    break;
                default:
                    btnPrimary.Text = "OK";
                    btnPrimary.Click += (s, e) => { dlg.Close(); };
                    break;
            }

            dlg.AcceptButton = btnPrimary;
            dlg.CancelButton = btnCancel;

            if (dlg.ShowDialog(this) == DialogResult.OK && (table.Status == TableStatus.Occupied || table.Status == TableStatus.Reserved))
            {
                // If Occupy or View order, open OrderSplitView
                ShowOrderSplitView(table, sourceButton);
            }
        }

        private void UpdateButtonFromTable(Button btn, TableInfo t)
        {
            btn.BackColor = GetStatusColor(t.Status);
            btn.Text = $"{t.Code}\n{t.Location}\n👥 {t.Capacity} | {t.Status}";
        }

        private Color GetStatusColor(TableStatus status)
        {
            return status switch
            {
                TableStatus.Available => Color.FromArgb(76, 175, 80),
                TableStatus.Reserved => Color.FromArgb(255, 193, 7),
                TableStatus.Occupied => Color.FromArgb(244, 67, 54),
                TableStatus.NeedsCleaning => Color.FromArgb(156, 39, 176),
                TableStatus.OutOfService => Color.FromArgb(158, 158, 158),
                _ => Color.Gray,
            };
        }

        private async void BtnRefresh_Click(object? sender, EventArgs e)
        {
            await LoadTablesAsync();
        }

        private void OnRefreshTimerTick()
        {
            try
            {
                // only auto-refresh when enabled
                if (!_autoRefreshEnabled) return;

                if (_refreshSeconds > 0) _refreshSeconds--;

                // perform auto-refresh when countdown reaches zero and control not already loading
                if (_refreshSeconds == 0 && !_isLoading && !_isDesigner)
                {
                    _ = LoadTablesAsync();
                    _refreshSeconds = 3; // reset
                }
            }
            catch { }
        }

        // wired from designer: toggle auto-refresh on/off
        private void BtnAutoRefresh_Click(object? sender, EventArgs e)
        {
            _autoRefreshEnabled = !_autoRefreshEnabled;
            try
            {
                btnAutoRefresh.Text = _autoRefreshEnabled ? "Auto: On" : "Auto: Off";
            }
            catch { }
            // reset countdown when enabling
            if (_autoRefreshEnabled) _refreshSeconds = 3;
        }

        private void TableContextMenu_ItemClicked(object? sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (_lastTableButton == null) return;
                if (_lastTableButton.Tag is not TableInfo table) return;
                if (e.ClickedItem?.Tag is not TableStatus status) return;

                var conn = Global.CurrentConnectionString;
                if (string.IsNullOrWhiteSpace(conn))
                {
                    MessageBox.Show(this, "Database connection required to change table status.", "Connection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var repo = new RmsRepository(conn);

                // If table currently Occupied and user tries to change to a non-Occupied status,
                // require checkout first (ensure order is paid/closed).
                if (table.Status == TableStatus.Occupied && status != TableStatus.Occupied)
                {
                    var open = repo.GetOpenOrderForTable(table.TableId);
                    if (open != null)
                    {
                        var confirm = MessageBox.Show(this, $"Table {table.Code} has an open order (#{open.OrderId}). Checkout before changing status?", "Checkout Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (confirm != DialogResult.Yes)
                        {
                            return; // user declined
                        }

                        // open checkout dialog; it records payment and marks order paid
                        using var dlg = new CheckoutDialog(open.OrderId, open.Subtotal, open.Tax);
                        if (dlg.ShowDialog(FindForm()) != DialogResult.OK)
                        {
                            // user cancelled checkout
                            return;
                        }
                    }
                }

                // perform the status update
                try
                {
                    repo.UpdateTableStatus(table.TableId, status);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Failed to update table status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // update UI state
                table.Status = status;
                UpdateButtonFromTable(_lastTableButton, table);
            }
            catch { }
        }
    }
}
