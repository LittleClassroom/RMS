using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using RMS;
using RMS.Models;

namespace RMS.Controls
{
    public class TableMapView : UserControl
    {
        private IContainer components;
        private FlowLayoutPanel tilesPanel;
        private Label lblStatus;
        private Button btnRefresh;
        private TextBox tbSearch;
        private ListBox lbFloors;
        private Panel legendPanel;
        private ToolTip _toolTip;

        private readonly List<TableInfo> _tables = new List<TableInfo>();
        private TableLayoutPanel layout;
        private Panel topPanel;
        private bool _isLoading;
        private readonly bool _isDesigner;

        // Events
        public event EventHandler<TableSelectedEventArgs>? TableSelected;
        public event EventHandler<TableInfo>? TableRightClicked;

        public TableMapView()
        {
            // detect design mode early to avoid executing runtime-only code in the designer
            _isDesigner = LicenseManager.UsageMode == LicenseUsageMode.Designtime;

            InitializeComponent();

            // only hook runtime loading when not in designer
            if (!_isDesigner)
            {
                Load += TableMapView_Load;
                SetupRuntime();
            }
        }

        // runtime wiring and non-designer-friendly setup
        private void SetupRuntime()
        {
            // wire events
            tbSearch.TextChanged += TbSearch_TextChanged;
            btnRefresh.Click += BtnRefresh_Click;
            lbFloors.SelectedIndexChanged += LbFloors_SelectedIndexChanged;

            // set placeholder if supported
            try { tbSearch.PlaceholderText = "Search tables or location..."; } catch { }

            // build legend controls at runtime to avoid complex constructs in InitializeComponent
            BuildLegendControls();

            // create tooltip for controls
            try
            {
                _toolTip = new ToolTip();
                _toolTip.SetToolTip(btnRefresh, "Reload table map from database");
            }
            catch
            {
                // ignore tooltip failures in constrained environments
            }
        }

        private void InitializeComponent()
        {
            layout = new TableLayoutPanel();
            topPanel = new Panel();
            btnRefresh = new Button();
            tbSearch = new TextBox();
            lbFloors = new ListBox();
            tilesPanel = new FlowLayoutPanel();
            legendPanel = new Panel();
            lblStatus = new Label();
            layout.SuspendLayout();
            topPanel.SuspendLayout();
            SuspendLayout();
            // 
            // layout
            // 
            layout.ColumnCount = 3;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            layout.Controls.Add(topPanel, 0, 0);
            layout.Controls.Add(lbFloors, 0, 1);
            layout.Controls.Add(tilesPanel, 1, 1);
            layout.Controls.Add(legendPanel, 2, 1);
            layout.Controls.Add(lblStatus, 0, 2);
            layout.Dock = DockStyle.Fill;
            layout.Location = new Point(0, 0);
            layout.Name = "layout";
            layout.RowCount = 3;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            layout.Size = new Size(800, 600);
            layout.TabIndex = 0;
            // 
            // topPanel
            // 
            layout.SetColumnSpan(topPanel, 3);
            topPanel.Controls.Add(btnRefresh);
            topPanel.Controls.Add(tbSearch);
            topPanel.Dock = DockStyle.Fill;
            topPanel.Location = new Point(3, 3);
            topPanel.Name = "topPanel";
            topPanel.Padding = new Padding(8);
            topPanel.Size = new Size(794, 42);
            topPanel.TabIndex = 0;
            // 
            // btnRefresh
            // 
            btnRefresh.Dock = DockStyle.Right;
            btnRefresh.Location = new Point(706, 8);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(80, 26);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "Refresh";
            // 
            // tbSearch
            // 
            tbSearch.Dock = DockStyle.Left;
            tbSearch.Location = new Point(8, 8);
            tbSearch.Name = "tbSearch";
            tbSearch.Size = new Size(300, 23);
            tbSearch.TabIndex = 1;
            // 
            // lbFloors
            // 
            lbFloors.Dock = DockStyle.Fill;
            lbFloors.Location = new Point(3, 51);
            lbFloors.Name = "lbFloors";
            lbFloors.Size = new Size(174, 518);
            lbFloors.TabIndex = 1;
            // 
            // tilesPanel
            // 
            tilesPanel.AutoScroll = true;
            tilesPanel.BackColor = Color.FromArgb(250, 250, 250);
            tilesPanel.Dock = DockStyle.Fill;
            tilesPanel.Location = new Point(183, 51);
            tilesPanel.Name = "tilesPanel";
            tilesPanel.Padding = new Padding(12);
            tilesPanel.Size = new Size(454, 518);
            tilesPanel.TabIndex = 2;
            // 
            // legendPanel
            // 
            legendPanel.Dock = DockStyle.Fill;
            legendPanel.Location = new Point(643, 51);
            legendPanel.Name = "legendPanel";
            legendPanel.Padding = new Padding(8);
            legendPanel.Size = new Size(154, 518);
            legendPanel.TabIndex = 3;
            // 
            // lblStatus
            // 
            layout.SetColumnSpan(lblStatus, 3);
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.Location = new Point(3, 572);
            lblStatus.Name = "lblStatus";
            lblStatus.Padding = new Padding(6, 4, 0, 0);
            lblStatus.Size = new Size(794, 28);
            lblStatus.TabIndex = 4;
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TableMapView
            // 
            Controls.Add(layout);
            Name = "TableMapView";
            Size = new Size(800, 600);
            layout.ResumeLayout(false);
            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();
            ResumeLayout(false);
        }

        private void BuildLegendControls()
        {
            legendPanel.Controls.Clear();
            var header = new Label { Text = "Status", Font = new Font("Segoe UI", 9F, FontStyle.Bold), Dock = DockStyle.Top, Height = 20 };
            legendPanel.Controls.Add(header);

            var statuses = new (string, Color)[] {
                ("Available", Color.FromArgb(76, 175, 80)),
                ("Reserved", Color.FromArgb(255, 193, 7)),
                ("Occupied", Color.FromArgb(244, 67, 54)),
                ("Needs Cleaning", Color.FromArgb(156, 39, 176)),
                ("Out of Service", Color.FromArgb(158, 158, 158))
            };

            var y = 28;
            foreach (var (text, col) in statuses)
            {
                var swatch = new Panel { BackColor = col, Size = new Size(18, 14), Location = new Point(8, y) };
                var lbl = new Label { Text = text, Location = new Point(34, y - 2), AutoSize = true };
                legendPanel.Controls.Add(swatch);
                legendPanel.Controls.Add(lbl);
                y += 22;
            }
        }

        private async void TableMapView_Load(object? sender, EventArgs e)
        {
            await LoadTablesAsync();
        }

        private async Task LoadTablesAsync()
        {
            if (_isLoading || _isDesigner) return;

            var connString = Global.CurrentConnectionString;
            if (string.IsNullOrWhiteSpace(connString))
            {
                lblStatus.Text = "Database connection not available.";
                return;
            }

            try
            {
                _isLoading = true;
                lblStatus.Text = "Loading tables...";
                var tables = await Task.Run(() => FetchTables(connString));

                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => ApplyTables(tables)));
                }
                else
                {
                    ApplyTables(tables);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Failed to load tables: " + ex.Message;
            }
            finally
            {
                _isLoading = false;
            }
        }

        public void RefreshTables()
        {
            if (_isDesigner) return;
            _ = LoadTablesAsync();
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

        private void ApplyTables(List<TableInfo> tables)
        {
            _tables.Clear();
            _tables.AddRange(tables);
            PopulateFloorList();
            PopulateTables();
            lblStatus.Text = $"Loaded {_tables.Count} tables";
        }

        private void PopulateFloorList()
        {
            var floors = _tables.Select(t => t.Location ?? "").Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(x => x).ToList();
            floors.Insert(0, "All");
            lbFloors.BeginUpdate();
            lbFloors.Items.Clear();
            foreach (var f in floors) lbFloors.Items.Add(f);
            lbFloors.SelectedIndex = 0;
            lbFloors.EndUpdate();
        }

        private void ApplySearchFilter(string? floor = null)
        {
            var q = tbSearch.Text?.Trim().ToLowerInvariant() ?? string.Empty;
            var floorFilter = floor ?? (lbFloors.SelectedItem as string) ?? "All";
            var filtered = _tables.Where(t => (floorFilter == "All" || string.Equals(t.Location ?? string.Empty, floorFilter, StringComparison.OrdinalIgnoreCase))
                                             && (string.IsNullOrEmpty(q) || (t.Code ?? string.Empty).ToLowerInvariant().Contains(q) || (t.Notes ?? string.Empty).ToLowerInvariant().Contains(q)))
                                  .ToList();
            PopulateTables(filtered);
        }

        private void PopulateTables()
        {
            PopulateTables(_tables);
        }

        private void PopulateTables(List<TableInfo> tables)
        {
            tilesPanel.SuspendLayout();
            tilesPanel.Controls.Clear();

            foreach (var table in tables)
            {
                var tile = CreateTableTile(table);
                tilesPanel.Controls.Add(tile);
            }

            tilesPanel.ResumeLayout();
        }

        private Panel CreateTableTile(TableInfo table)
        {
            var tile = new Panel
            {
                Width = 140,
                Height = 100,
                Margin = new Padding(8),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = GetTableColor(table.Status),
                Cursor = Cursors.Hand,
                Tag = table
            };

            var lblCode = new Label
            {
                Text = table.Code,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 32
            };

            var lblDetails = new Label
            {
                Text = $"{table.Location}  👥{table.Capacity}",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 22
            };

            var lblStatusBadge = new Label
            {
                Text = GetStatusText(table.Status),
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Padding = new Padding(4)
            };

            tile.Controls.Add(lblStatusBadge);
            tile.Controls.Add(lblDetails);
            tile.Controls.Add(lblCode);

            tile.Click += (s, e) => OnTableClicked(table);
            tile.MouseEnter += (s, e) => tile.BackColor = GetHoverColor(table.Status);
            tile.MouseLeave += (s, e) => tile.BackColor = GetTableColor(table.Status);

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("View Details", null, (s, e) => OnTableRightClicked(table));
            contextMenu.Items.Add("Mark as Reserved", null, (s, e) => MarkTableReserved(table));
            contextMenu.Items.Add("Mark as Cleaning", null, (s, e) => MarkTableCleaning(table));
            tile.ContextMenuStrip = contextMenu;

            return tile;
        }

        private Color GetTableColor(TableStatus status)
        {
            return status switch
            {
                TableStatus.Available => Color.FromArgb(76, 175, 80),
                TableStatus.Occupied => Color.FromArgb(244, 67, 54),
                TableStatus.Reserved => Color.FromArgb(255, 152, 0),
                TableStatus.NeedsCleaning => Color.FromArgb(156, 39, 176),
                TableStatus.OutOfService => Color.FromArgb(158, 158, 158),
                _ => Color.FromArgb(96, 125, 139),
            };
        }

        private Color GetHoverColor(TableStatus status)
        {
            var baseColor = GetTableColor(status);
            return Color.FromArgb(
                Math.Min(255, baseColor.R + 20),
                Math.Min(255, baseColor.G + 20),
                Math.Min(255, baseColor.B + 20)
            );
        }

        private string GetStatusText(TableStatus status)
        {
            return status switch
            {
                TableStatus.Available => "Available",
                TableStatus.Occupied => "Occupied",
                TableStatus.Reserved => "Reserved",
                TableStatus.NeedsCleaning => "Needs Cleaning",
                TableStatus.OutOfService => "Out of Service",
                _ => "Unknown",
            };
        }

        private void OnTableClicked(TableInfo table)
        {
            if (table.Status == TableStatus.Available || table.Status == TableStatus.Occupied)
            {
                TableSelected?.Invoke(this, new TableSelectedEventArgs(table));
            }
            else
            {
                MessageBox.Show(this,
                    $"Table {table.Code} is {GetStatusText(table.Status)} and cannot be used for orders.",
                    "Table Not Available",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void OnTableRightClicked(TableInfo table)
        {
            TableRightClicked?.Invoke(this, table);
        }

        private void MarkTableReserved(TableInfo table)
        {
            MessageBox.Show(this, $"Table {table.Code} marked as Reserved", "Status Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MarkTableCleaning(TableInfo table)
        {
            MessageBox.Show(this, $"Table {table.Code} marked as Cleaning", "Status Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void layout_Paint(object sender, PaintEventArgs e)
        {

        }

        // runtime handlers wired by SetupRuntime
        private void TbSearch_TextChanged(object? sender, EventArgs e)
        {
            ApplySearchFilter();
        }

        private async void BtnRefresh_Click(object? sender, EventArgs e)
        {
            await LoadTablesAsync();
        }

        private void LbFloors_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lbFloors.SelectedItem is string floor)
                ApplySearchFilter(floor);
        }
    }

    public class TableSelectedEventArgs : EventArgs
    {
        public TableInfo Table { get; }

        public TableSelectedEventArgs(TableInfo table)
        {
            Table = table;
        }
    }
}