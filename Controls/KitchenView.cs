using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RMS.Data.SqlServer;
using RMS.Models;
using System.Reflection;

namespace RMS.Controls
{
    public partial class KitchenView : UserControl
    {
        private readonly List<KitchenLineInfo> _lines = new();
        private readonly RmsRepository? _repo;
        private System.Windows.Forms.Timer _refreshTimer;
        // map panels by OrderId (one card per order session)
        private readonly Dictionary<int, Panel> _orderPanelMap = new();
        private ContextMenuStrip? _orderContextMenu;

        public KitchenView()
        {
            InitializeComponent();
            // try to create repository if connection available
            try
            {
                if (!string.IsNullOrWhiteSpace(RMS.Global.CurrentConnectionString))
                    _repo = new RmsRepository(RMS.Global.CurrentConnectionString);
            }
            catch { }

            // setup simple timer to refresh kitchen lines
            _refreshTimer = new System.Windows.Forms.Timer { Interval = 3000 };
            _refreshTimer.Tick += async (s, e) => await RefreshLinesAsync();
            _refreshTimer.Start();
            this.Load += async (s, e) => await RefreshLinesAsync();

            // reduce flicker on the tickets panel
            try { EnableDoubleBuffering(ticketsPanel); } catch { }
            try { ticketsPanel.WrapContents = true; ticketsPanel.FlowDirection = FlowDirection.LeftToRight; } catch { }

            // central context menu for order actions (no Close option in kitchen)
            _orderContextMenu = new ContextMenuStrip();
            var miInKitchen = new ToolStripMenuItem("Set InKitchen");
            var miReady = new ToolStripMenuItem("Set Ready");
            miInKitchen.Click += (_, __) => HandleContextMenuOrderAction(1);
            miReady.Click += (_, __) => HandleContextMenuOrderAction(2);
            _orderContextMenu.Items.AddRange(new ToolStripItem[] { miInKitchen, miReady });

            ticketsPanel.MouseUp += TicketsPanel_MouseUp;
        }

        private static void EnableDoubleBuffering(Control c)
        {
            try
            {
                var pi = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
                pi?.SetValue(c, true, null);
            }
            catch { }
        }

        private async Task RefreshLinesAsync()
        {
            if (_repo == null) return;
            // prevent overlapping refreshes
            try
            {
                _refreshTimer.Enabled = false;
                var lines = await Task.Run(() => FetchKitchenLines());
                // replace list atomically
                _lines.Clear();
                _lines.AddRange(lines);
                // update header with count
                try { if (lblTitle != null) lblTitle.BeginInvoke(() => lblTitle.Text = $"🍳 Kitchen Display — {lines.Count} line(s)"); } catch { }
                RenderLines();
            }
            catch { }
            finally
            {
                try { _refreshTimer.Enabled = true; } catch { }
            }
        }

        private List<KitchenLineInfo> FetchKitchenLines()
        {
            // delegate to repository queries — implement inline to avoid changing repo for now
            var list = new List<KitchenLineInfo>();
            try
            {
                if (string.IsNullOrWhiteSpace(RMS.Global.CurrentConnectionString))
                {
                    System.Diagnostics.Debug.WriteLine("FetchKitchenLines: no connection string configured.");
                    return list;
                }
                using var cn = new Microsoft.Data.SqlClient.SqlConnection(RMS.Global.CurrentConnectionString);
                cn.Open();
                using var cmd = new Microsoft.Data.SqlClient.SqlCommand(@"
SELECT ol.OrderLineId, ol.OrderId, o.TableId, t.Code AS TableCode, o.CreatedAtUtc, ol.MenuItemId, COALESCE(mi.Name, '') AS Name,
       CAST(ol.Quantity AS int) AS Quantity, ol.UnitPrice, ol.Status AS LineStatus, o.Status AS OrderStatus
FROM dbo.OrderLines ol
LEFT JOIN dbo.Orders o ON o.OrderId = ol.OrderId
LEFT JOIN dbo.Tables t ON t.TableId = o.TableId
LEFT JOIN dbo.MenuItems mi ON mi.MenuItemId = ol.MenuItemId
WHERE o.Status = 1 AND ol.Status IN (0,1) -- only orders InKitchen and lines Pending/Cooking
ORDER BY o.CreatedAtUtc ASC", cn);
                using var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new KitchenLineInfo
                    {
                        OrderLineId = rdr.GetInt32(0),
                        OrderId = rdr.GetInt32(1),
                        TableId = rdr.IsDBNull(2) ? 0 : rdr.GetInt32(2),
                        TableCode = rdr.IsDBNull(3) ? null : rdr.GetString(3),
                        OrderCreatedAtUtc = rdr.IsDBNull(4) ? DateTime.UtcNow : rdr.GetDateTime(4),
                        MenuItemId = rdr.IsDBNull(5) ? 0 : rdr.GetInt32(5),
                        Name = rdr.IsDBNull(6) ? string.Empty : rdr.GetString(6),
                        Quantity = rdr.IsDBNull(7) ? 0 : rdr.GetInt32(7),
                        UnitPrice = rdr.IsDBNull(8) ? 0m : rdr.GetDecimal(8),
                        LineStatus = rdr.IsDBNull(9) ? (byte)0 : rdr.GetByte(9),
                        OrderStatus = rdr.IsDBNull(10) ? (byte)0 : rdr.GetByte(10)
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("FetchKitchenLines failed: " + ex.Message);
            }
            return list;
        }

        private void RenderLines()
        {
            try
            {
                if (ticketsPanel == null) return;
                ticketsPanel.SuspendLayout();

                // Group lines by OrderId so we render one card per order session
                var groups = _lines.GroupBy(l => l.OrderId).ToList();
                var keepOrderIds = new HashSet<int>(groups.Select(g => g.Key));

                foreach (var grp in groups)
                {
                    var orderId = grp.Key;
                    var linesForOrder = grp.ToList();

                    // if all lines in order are served, remove panel
                    if (linesForOrder.All(l => l.LineStatus == 3))
                    {
                        if (_orderPanelMap.TryGetValue(orderId, out var oldPanel))
                        {
                            ticketsPanel.Controls.Remove(oldPanel);
                            _orderPanelMap.Remove(orderId);
                        }
                        continue;
                    }

                    if (_orderPanelMap.TryGetValue(orderId, out var panel))
                    {
                        UpdateOrderPanel(panel, linesForOrder);
                    }
                    else
                    {
                        var newPanel = CreateOrderPanel(linesForOrder);
                        ticketsPanel.Controls.Add(newPanel);
                        _orderPanelMap[orderId] = newPanel;
                    }
                }

                // remove stale order panels
                var stale = _orderPanelMap.Keys.Where(id => !keepOrderIds.Contains(id)).ToList();
                foreach (var id in stale)
                {
                    if (_orderPanelMap.TryGetValue(id, out var p))
                    {
                        ticketsPanel.Controls.Remove(p);
                        _orderPanelMap.Remove(id);
                    }
                }

                ticketsPanel.ResumeLayout();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("RenderLines failed: " + ex.Message);
            }
        }

        // Create a panel representing an order and its lines
        private Panel CreateOrderPanel(List<KitchenLineInfo> lines)
        {
            var orderId = lines.First().OrderId;
            var status = lines.First().OrderStatus;
            var statusColors = new[]
            {
                System.Drawing.Color.FromArgb(255, 193, 7),
                System.Drawing.Color.FromArgb(33, 150, 243),
                System.Drawing.Color.FromArgb(76, 175, 80)
            };
            var statusNames = new[] { "⏳ PENDING", "🔥 COOKING", "✅ READY" };

            var ticket = new Panel
            {
                Width = 320,
                Height = 200,
                Margin = new Padding(8),
                BackColor = System.Drawing.Color.White,
                Padding = new Padding(0),
                Tag = orderId
            };

            var headerBar = new Panel { Dock = DockStyle.Top, Height = 40, BackColor = statusColors[Math.Min(statusColors.Length - 1, status)] };
            var lblOrderInfo = new Label { Text = $"#{lines.First().OrderId}  •  {lines.First().TableCode}", Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.Color.White, Location = new System.Drawing.Point(8, 10), AutoSize = true };
            var lblTime = new Label { Text = lines.First().OrderCreatedAtUtc.ToLocalTime().ToString("t"), Font = new System.Drawing.Font("Segoe UI", 9F), ForeColor = System.Drawing.Color.White, Location = new System.Drawing.Point(170, 12), AutoSize = true };
            headerBar.Controls.Add(lblOrderInfo); headerBar.Controls.Add(lblTime);

            var itemsPanel = new Panel { Name = "itemsPanel", Location = new System.Drawing.Point(0, 40), Width = 300, Height = 96, Padding = new Padding(8), AutoScroll = true };
            int y = 8;
            foreach (var l in lines)
            {
                var lblItem = new Label { Text = $"• {l.Name} x{l.Quantity}", Font = new System.Drawing.Font("Segoe UI", 10F), Location = new System.Drawing.Point(8, y), AutoSize = true };
                itemsPanel.Controls.Add(lblItem);
                y += 22;
            }

            var footerBar = new Panel { Name = "footerBar", Dock = DockStyle.Bottom, Height = 36, BackColor = System.Drawing.Color.FromArgb(245, 245, 245) };
            var lblStatus = new Label { Text = statusNames[Math.Min(statusNames.Length - 1, status)], Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold), ForeColor = statusColors[Math.Min(statusColors.Length - 1, status)], Location = new System.Drawing.Point(8, 10), AutoSize = true };
            footerBar.Controls.Add(lblStatus);

            // actions (line-level)
            var actions = new FlowLayoutPanel { Name = "actionsPanel", Dock = DockStyle.Bottom, Height = 48, Padding = new Padding(8), FlowDirection = FlowDirection.LeftToRight, WrapContents = true, AutoSize = false }; 
            // add line-level buttons for each line inside itemsPanel
            foreach (var l in lines)
            {
                byte target = l.LineStatus == 0 ? (byte)1 : (l.LineStatus == 1 ? (byte)2 : (byte)3);
                var btn = new Button
                {
                    Text = l.LineStatus == 0 ? "Start" : (l.LineStatus == 1 ? "Ready" : "Served"),
                    AutoSize = false,
                    Height = 32,
                    Width = 84,
                    Margin = new Padding(4),
                    Tag = Tuple.Create(l.OrderLineId, target)
                };
                btn.Click += (_, __) =>
                {
                    btn.Enabled = false;
                    var t = (Tuple<int, byte>)btn.Tag!;
                    UpdateLineStatus(t.Item1, t.Item2);
                };
                actions.Controls.Add(btn);
            }
            var btnCancel = new Button { Text = "Cancel", AutoSize = false, Height = 32, Width = 84, Margin = new Padding(4) };
            btnCancel.Click += (_, __) => { btnCancel.Enabled = false; foreach (var l in lines) UpdateLineStatus(l.OrderLineId, 3); };
            actions.Controls.Add(btnCancel);

            // order-level actions
            var orderActions = new FlowLayoutPanel { Name = "orderActionsPanel", Dock = DockStyle.Bottom, Height = 48, Padding = new Padding(8), FlowDirection = FlowDirection.LeftToRight, WrapContents = true, AutoSize = false };
            var btnOrderInKitchen = new Button { Text = "InKitchen", AutoSize = false, Height = 32, Width = 84, Margin = new Padding(4) };
            btnOrderInKitchen.Click += (_, __) => { btnOrderInKitchen.Enabled = false; UpdateOrderStatus(orderId, 1); };
            orderActions.Controls.Add(btnOrderInKitchen);

            var btnOrderReady = new Button { Text = "Ready", AutoSize = false, Height = 32, Width = 84, Margin = new Padding(4) };
            btnOrderReady.Click += (_, __) => { btnOrderReady.Enabled = false; UpdateOrderStatus(orderId, 2); };
            orderActions.Controls.Add(btnOrderReady);

            // No Close button in kitchen view - chefs only mark InKitchen/Ready

            ticket.Controls.Add(headerBar);
            ticket.Controls.Add(itemsPanel);
            ticket.Controls.Add(actions);
            ticket.Controls.Add(orderActions);
            ticket.Controls.Add(footerBar);

            // enable double buffering for the ticket and subpanels
            try { EnableDoubleBuffering(ticket); EnableDoubleBuffering(itemsPanel); EnableDoubleBuffering(actions); EnableDoubleBuffering(orderActions); } catch { }

            return ticket;
        }

        private void UpdateOrderPanel(Panel panel, List<KitchenLineInfo> lines)
        {
            try
            {
                // update header, items and status label
                panel.Tag = lines.First().OrderId;
                // header
                var header = panel.Controls.OfType<Panel>().FirstOrDefault(p => p.Dock == DockStyle.Top);
                if (header != null)
                {
                    foreach (Control h in header.Controls)
                    {
                        if (h is Label headerLabel && headerLabel.Text.StartsWith("#")) headerLabel.Text = $"#{lines.First().OrderId}  •  {lines.First().TableCode}";
                        if (h is Label timeLabel && DateTime.TryParse(timeLabel.Text, out _)) timeLabel.Text = lines.First().OrderCreatedAtUtc.ToLocalTime().ToString("t");
                    }
                }

                // items panel
                var items = panel.Controls.Find("itemsPanel", true).FirstOrDefault() as Panel;
                if (items != null)
                {
                    items.Controls.Clear();
                    int y = 8;
                    foreach (var l in lines)
                    {
                        var lblItem = new Label { Text = $"• {l.Name} x{l.Quantity}", Font = new System.Drawing.Font("Segoe UI", 10F), Location = new System.Drawing.Point(8, y), AutoSize = true };
                        items.Controls.Add(lblItem);
                        y += 22;
                    }
                }

                // actions panel: rebuild buttons to ensure handlers/targets are up-to-date
                var actions = panel.Controls.Find("actionsPanel", true).FirstOrDefault() as FlowLayoutPanel;
                if (actions != null)
                {
                    actions.Controls.Clear();
                    foreach (var l in lines)
                    {
                        byte target = l.LineStatus == 0 ? (byte)1 : (l.LineStatus == 1 ? (byte)2 : (byte)3);
                        var btn = new Button
                        {
                            Text = l.LineStatus == 0 ? "Start" : (l.LineStatus == 1 ? "Ready" : "Served"),
                            AutoSize = false,
                            Height = 32,
                            Width = 84,
                            Margin = new Padding(4),
                            Tag = Tuple.Create(l.OrderLineId, target)
                        };
                        btn.Click += (_, __) =>
                        {
                            btn.Enabled = false;
                            var t = (Tuple<int, byte>)btn.Tag!;
                            UpdateLineStatus(t.Item1, t.Item2);
                        };
                        actions.Controls.Add(btn);
                    }
                    var btnCancel = new Button { Text = "Cancel", AutoSize = false, Height = 32, Width = 84, Margin = new Padding(4) };
                    btnCancel.Click += (_, __) => { btnCancel.Enabled = false; foreach (var l in lines) UpdateLineStatus(l.OrderLineId, 3); };
                    actions.Controls.Add(btnCancel);
                }

                // order actions: ensure handlers use current order id
                var orderActions = panel.Controls.Find("orderActionsPanel", true).FirstOrDefault() as FlowLayoutPanel;
                if (orderActions != null)
                {
                    orderActions.Controls.Clear();
                    var btnOrderInKitchen = new Button { Text = "InKitchen", AutoSize = false, Height = 32, Width = 84, Margin = new Padding(4) };
                    btnOrderInKitchen.Click += (_, __) => { btnOrderInKitchen.Enabled = false; UpdateOrderStatus(lines.First().OrderId, 1); };
                    orderActions.Controls.Add(btnOrderInKitchen);

                    var btnOrderReady = new Button { Text = "Ready", AutoSize = false, Height = 32, Width = 84, Margin = new Padding(4) };
                    btnOrderReady.Click += (_, __) => { btnOrderReady.Enabled = false; UpdateOrderStatus(lines.First().OrderId, 2); };
                    orderActions.Controls.Add(btnOrderReady);
                }

                // update footer/status label
                var footer = panel.Controls.Find("footerBar", true).FirstOrDefault() as Panel;
                if (footer != null && footer.Controls.Count > 0 && footer.Controls[0] is Label statusLabel)
                {
                    var status = lines.First().OrderStatus;
                    var statusNames = new[] { "⏳ PENDING", "🔥 COOKING", "✅ READY" };
                    statusLabel.Text = statusNames[Math.Min(statusNames.Length - 1, status)];
                }
            }
            catch { }
        }

        private async void UpdateLineStatus(int orderLineId, byte newStatus)
        {
            // update DB and refresh
            try
            {
                if (string.IsNullOrWhiteSpace(RMS.Global.CurrentConnectionString))
                {
                    MessageBox.Show(this, "Database connection not configured.", "Kitchen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using var cn = new Microsoft.Data.SqlClient.SqlConnection(RMS.Global.CurrentConnectionString);
                cn.Open();
                using var cmd = new Microsoft.Data.SqlClient.SqlCommand("UPDATE dbo.OrderLines SET Status = @s WHERE OrderLineId = @id", cn);
                cmd.Parameters.AddWithValue("@s", newStatus);
                cmd.Parameters.AddWithValue("@id", orderLineId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to update line status: " + ex.Message, "Kitchen Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // refresh lines after small delay to allow DB commit; run on UI context
            try
            {
                await Task.Delay(250);
                await RefreshLinesAsync();
            }
            catch { }
        }

        private async void UpdateOrderStatus(int orderId, byte newStatus)
        {
            // update DB and refresh
            try
            {
                if (string.IsNullOrWhiteSpace(RMS.Global.CurrentConnectionString))
                {
                    MessageBox.Show(this, "Database connection not configured.", "Kitchen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using var cn = new Microsoft.Data.SqlClient.SqlConnection(RMS.Global.CurrentConnectionString);
                cn.Open();
                using var cmd = new Microsoft.Data.SqlClient.SqlCommand("UPDATE dbo.Orders SET Status = @s WHERE OrderId = @id", cn);
                cmd.Parameters.AddWithValue("@s", newStatus);
                cmd.Parameters.AddWithValue("@id", orderId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to update order status: " + ex.Message, "Kitchen Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // refresh lines on UI thread
            try
            {
                await Task.Delay(250);
                await RefreshLinesAsync();
            }
            catch { }
        }

        private void TicketsPanel_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || ticketsPanel == null || _orderContextMenu == null) return;
            // find order panel under cursor
            var pt = e.Location;
            Control? found = null;
            // iterate in reverse (topmost first)
            for (int i = ticketsPanel.Controls.Count - 1; i >= 0; i--)
            {
                var ctrl = ticketsPanel.Controls[i];
                if (ctrl.Bounds.Contains(pt))
                {
                    found = ctrl;
                    break;
                }
            }

            if (found == null) return;
            // order id stored in Tag
            if (found.Tag is int orderId)
            {
                _orderContextMenu.Tag = orderId;
                try { _orderContextMenu.Show(ticketsPanel, pt); } catch { }
            }
        }

        private void HandleContextMenuOrderAction(byte newStatus)
        {
            try
            {
                if (_orderContextMenu?.Tag is int orderId)
                {
                    UpdateOrderStatus(orderId, newStatus);
                }
            }
            catch { }
        }

        private void ticketsPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
