using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RMS.Data.SqlServer;
using RMS.Models;
using RMS.UI;

namespace RMS.Controls
{
    public partial class OrderPanel : UserControl
    {
        // Event raised when an order is completed. ManagerMainForm expects this.
        public event EventHandler<OrderCompletedEventArgs>? OrderCompleted;

        private readonly List<MenuItemEntity> _menuItems = new();
        private readonly List<OrderLineEntry> _orderLines = new();
        private TableInfo? _currentTable;
        private OrderSummary? _activeOrder;
        private bool _initialized;
        private bool _menuLoaded;
        private bool _menuLoading;
        private const decimal TaxRate = 0.10m;
        private ContextMenuStrip? _orderContextMenu;

        public OrderPanel()
        {
            InitializeComponent();
        }

        private void OrderPanel_Load(object sender, EventArgs e)
        {
            EnsureInitialized();
            _ = EnsureMenuLoadedAsync();
        }

        private void EnsureInitialized()
        {
            if (_initialized)
            {
                return;
            }

            ConfigureMenuList();
            ConfigureOrderList();
            WireUpEvents();
            UpdateActionButtons();
            _initialized = true;
        }

        private void WireUpEvents()
        {
            tbMenuSearch.TextChanged += (_, __) => ApplyMenuFilter();
            lvMenuItems.DoubleClick += LvMenuItems_DoubleClick;
            lvOrderLines.KeyDown += LvOrderLines_KeyDown;
            lvOrderLines.DoubleClick += LvOrderLines_DoubleClick;
            btnCancelOrder.Click += BtnCancelOrder_Click;
            btnSubmitOrder.Click += BtnSubmitOrder_Click;
            btnCheckout.Click += BtnCheckout_Click;
        }

        private void ConfigureMenuList()
        {
            if (lvMenuItems.Columns.Count == 0)
            {
                lvMenuItems.Columns.Add("Item", 280, HorizontalAlignment.Left);
                lvMenuItems.Columns.Add("Price", 90, HorizontalAlignment.Right);
            }
        }

        private void ConfigureOrderList()
        {
            if (lvOrderLines.Columns.Count == 0)
            {
                lvOrderLines.Columns.Add("Item", 240, HorizontalAlignment.Left);
                lvOrderLines.Columns.Add("Qty", 60, HorizontalAlignment.Center);
                lvOrderLines.Columns.Add("Price", 90, HorizontalAlignment.Right);
                lvOrderLines.Columns.Add("Total", 110, HorizontalAlignment.Right);
            }

            if (_orderContextMenu == null)
            {
                _orderContextMenu = new ContextMenuStrip();
                _orderContextMenu.Items.Add("Increase Quantity", null, (_, __) => AdjustSelectedLine(1));
                _orderContextMenu.Items.Add("Decrease Quantity", null, (_, __) => AdjustSelectedLine(-1));
                _orderContextMenu.Items.Add(new ToolStripSeparator());
                _orderContextMenu.Items.Add("Remove Item", null, (_, __) => RemoveSelectedLine());
                lvOrderLines.ContextMenuStrip = _orderContextMenu;
            }
        }

        // Loads or prepares the order view for the provided table.
        public async Task LoadTableOrder(TableInfo table)
        {
            EnsureInitialized();
            _currentTable = table;
            var location = string.IsNullOrWhiteSpace(table.Location) ? string.Empty : $" • {table.Location}";
            lblOrderHeader.Text = $"Table {table.Code}{location}";
            ResetOrderLines();
            _activeOrder = null;
            await EnsureMenuLoadedAsync();
            await LoadExistingOrderAsync(table);
        }

        private async Task EnsureMenuLoadedAsync()
        {
            if (_menuLoaded || _menuLoading)
            {
                while (_menuLoading)
                {
                    await Task.Delay(50);
                }
                return;
            }

            await LoadMenuItemsAsync();
        }

        private async Task LoadMenuItemsAsync()
        {
            if (!TryGetConnection(out var connectionString))
            {
                return;
            }

            try
            {
                _menuLoading = true;
                var repo = new RmsRepository(connectionString);
                var items = await Task.Run(() => repo.GetMenuItemsByCategory(null));
                _menuItems.Clear();
                _menuItems.AddRange(items.Where(i => i.IsActive).OrderBy(i => i.Name));
                _menuLoaded = true;
                ApplyMenuFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to load menu items: " + ex.Message, "Menu Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _menuLoading = false;
            }
        }

        private async Task LoadExistingOrderAsync(TableInfo table)
        {
            if (!TryGetConnection(out var connectionString))
            {
                return;
            }

            try
            {
                var repo = new RmsRepository(connectionString);
                var summary = await Task.Run(() => repo.GetOpenOrderForTable(table.TableId));
                _activeOrder = summary;
                ResetOrderLines();

                if (summary != null)
                {
                    var lines = await Task.Run(() => repo.GetOrderLines(summary.OrderId));
                    foreach (var line in lines)
                    {
                        _orderLines.Add(new OrderLineEntry
                        {
                            MenuItemId = line.MenuItemId,
                            Name = line.Name,
                            UnitPrice = line.UnitPrice,
                            Quantity = line.Quantity,
                            IsPersisted = true
                        });
                    }
                }

                RefreshOrderList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to load existing order: " + ex.Message, "Order Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool TryGetConnection(out string connectionString)
        {
            connectionString = Global.CurrentConnectionString ?? string.Empty;
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                MessageBox.Show(this, "Database connection is not configured.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ApplyMenuFilter()
        {
            var filter = tbMenuSearch.Text?.Trim().ToLowerInvariant() ?? string.Empty;
            IEnumerable<MenuItemEntity> query = _menuItems;

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(i => i.Name.ToLowerInvariant().Contains(filter));
            }

            lvMenuItems.BeginUpdate();
            lvMenuItems.Items.Clear();
            foreach (var menuItem in query)
            {
                var lvi = new ListViewItem(menuItem.Name)
                {
                    Tag = menuItem
                };
                lvi.SubItems.Add(menuItem.Price.ToString("C"));
                lvMenuItems.Items.Add(lvi);
            }
            lvMenuItems.EndUpdate();
        }

        private void LvMenuItems_DoubleClick(object? sender, EventArgs e)
        {
            if (lvMenuItems.SelectedItems.Count == 0)
            {
                return;
            }

            if (lvMenuItems.SelectedItems[0].Tag is MenuItemEntity menuItem)
            {
                AddMenuItemToOrder(menuItem);
            }
        }

        private void AddMenuItemToOrder(MenuItemEntity menuItem)
        {
            var existing = _orderLines.FirstOrDefault(l => !l.IsPersisted && l.MenuItemId == menuItem.MenuItemId);
            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                _orderLines.Add(new OrderLineEntry
                {
                    MenuItemId = menuItem.MenuItemId,
                    Name = menuItem.Name,
                    UnitPrice = menuItem.Price,
                    Quantity = 1
                });
            }

            RefreshOrderList();
        }

        private void RefreshOrderList()
        {
            lvOrderLines.BeginUpdate();
            lvOrderLines.Items.Clear();

            foreach (var line in _orderLines)
            {
                var item = new ListViewItem(line.Name)
                {
                    Tag = line
                };
                item.SubItems.Add(line.Quantity.ToString());
                item.SubItems.Add(line.UnitPrice.ToString("C"));
                item.SubItems.Add(line.LineTotal.ToString("C"));
                lvOrderLines.Items.Add(item);
            }

            lvOrderLines.EndUpdate();
            UpdateTotals();
            UpdateActionButtons();
        }

        private void UpdateTotals()
        {
            var subtotal = _orderLines.Sum(l => l.LineTotal);
            var tax = Math.Round(subtotal * TaxRate, 2, MidpointRounding.AwayFromZero);
            var total = subtotal + tax;

            lblSubtotalValue.Text = subtotal.ToString("C");
            lblTaxValue.Text = tax.ToString("C");
            lblGrandTotalValue.Text = total.ToString("C");
        }

        private void UpdateActionButtons()
        {
            var hasItems = _orderLines.Count > 0;
            var hasNewItems = _orderLines.Any(l => !l.IsPersisted);
            btnSubmitOrder.Enabled = _currentTable != null && ((_activeOrder == null && hasItems) || (_activeOrder != null && hasNewItems));
            btnCancelOrder.Enabled = _activeOrder == null ? hasItems : hasNewItems;
            btnCheckout.Enabled = _activeOrder != null && !_orderLines.Any(l => !l.IsPersisted);
        }

        private void LvOrderLines_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveSelectedLine();
                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
            {
                AdjustSelectedLine(1);
                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus)
            {
                AdjustSelectedLine(-1);
                e.Handled = true;
            }
        }

        private void LvOrderLines_DoubleClick(object? sender, EventArgs e)
        {
            AdjustSelectedLine(1);
        }

        private void AdjustSelectedLine(int delta)
        {
            var line = GetSelectedLine();
            if (line == null)
            {
                return;
            }

            if (line.IsPersisted)
            {
                MessageBox.Show(this, "Submitted items cannot be edited here.", "Read Only", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            line.Quantity += delta;
            if (line.Quantity <= 0)
            {
                _orderLines.Remove(line);
            }

            RefreshOrderList();
        }

        private void RemoveSelectedLine()
        {
            var line = GetSelectedLine();
            if (line == null)
            {
                return;
            }

            if (line.IsPersisted)
            {
                MessageBox.Show(this, "Submitted items cannot be removed.", "Read Only", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _orderLines.Remove(line);
            RefreshOrderList();
        }

        private OrderLineEntry? GetSelectedLine()
        {
            if (lvOrderLines.SelectedItems.Count == 0)
            {
                return null;
            }

            return lvOrderLines.SelectedItems[0].Tag as OrderLineEntry;
        }

        private void BtnCancelOrder_Click(object? sender, EventArgs e)
        {
            if (_orderLines.Count == 0)
            {
                return;
            }

            if (_activeOrder == null)
            {
                var confirmNew = MessageBox.Show(this, "Clear all items from this order?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmNew == DialogResult.Yes)
                {
                    ResetOrderLines(true);
                }
                return;
            }

            if (!_orderLines.Any(l => !l.IsPersisted))
            {
                MessageBox.Show(this, "No pending items to clear.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(this, "Remove the new (unsent) items?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                _orderLines.RemoveAll(l => !l.IsPersisted);
                RefreshOrderList();
            }
        }

        private async void BtnSubmitOrder_Click(object? sender, EventArgs e)
        {
            if (_currentTable == null)
            {
                MessageBox.Show(this, "Select a table before submitting.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!TryGetConnection(out var connectionString))
            {
                return;
            }

            if (_activeOrder == null)
            {
                if (_orderLines.Count == 0)
                {
                    MessageBox.Show(this, "Add at least one item to the order.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                await CreateNewOrderAsync(connectionString);
            }
            else
            {
                var pending = _orderLines.Where(l => !l.IsPersisted).ToList();
                if (pending.Count == 0)
                {
                    MessageBox.Show(this, "There are no new items to submit.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                await AppendToExistingOrderAsync(connectionString, pending);
            }
        }

        private async Task CreateNewOrderAsync(string connectionString)
        {
            var subtotal = _orderLines.Sum(l => l.LineTotal);
            var tax = Math.Round(subtotal * TaxRate, 2, MidpointRounding.AwayFromZero);
            var total = subtotal + tax;
            var lines = _orderLines.Select(l => (l.MenuItemId, l.Quantity, l.UnitPrice)).ToList();

            btnSubmitOrder.Enabled = false;
            try
            {
                var repo = new RmsRepository(connectionString);
                var orderId = await Task.Run(() => repo.CreateOrder(_currentTable!.TableId, subtotal, tax, total, lines));
                await Task.Run(() => repo.UpdateTableStatus(_currentTable!.TableId, TableStatus.Occupied));

                MessageBox.Show(this, $"Order #{orderId} submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CompleteOrderAndClose(orderId, OrderCompletionType.Submitted);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to submit order: " + ex.Message, "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateActionButtons();
            }
        }

        private async Task AppendToExistingOrderAsync(string connectionString, List<OrderLineEntry> newLines)
        {
            btnSubmitOrder.Enabled = false;
            try
            {
                var repo = new RmsRepository(connectionString);
                await Task.Run(() => repo.AddOrderLines(_activeOrder!.OrderId, newLines.Select(l => (l.MenuItemId, l.Quantity, l.UnitPrice))));

                foreach (var line in newLines)
                {
                    line.IsPersisted = true;
                }

                var subtotal = _orderLines.Sum(l => l.LineTotal);
                var tax = Math.Round(subtotal * TaxRate, 2, MidpointRounding.AwayFromZero);
                var total = subtotal + tax;
                await Task.Run(() => repo.UpdateOrderTotals(_activeOrder!.OrderId, subtotal, tax, total));

                _activeOrder!.Subtotal = subtotal;
                _activeOrder.Tax = tax;
                _activeOrder.Total = total;

                RefreshOrderList();
                MessageBox.Show(this, "Order updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to update order: " + ex.Message, "Order Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateActionButtons();
            }
        }

        private async void BtnCheckout_Click(object? sender, EventArgs e)
        {
            if (_activeOrder == null || _currentTable == null)
            {
                MessageBox.Show(this, "Select a table with an open order first.", "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_orderLines.Any(l => !l.IsPersisted))
            {
                MessageBox.Show(this, "Please submit the pending items before checkout.", "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var dlg = new CheckoutDialog(_activeOrder.OrderId, _activeOrder.Subtotal, _activeOrder.Tax);
            if (dlg.ShowDialog(FindForm()) != DialogResult.OK)
            {
                return;
            }

            if (!TryGetConnection(out var connectionString))
            {
                return;
            }

            try
            {
                var repo = new RmsRepository(connectionString);
                await Task.Run(() => repo.MarkOrderPaid(_activeOrder.OrderId));
                await Task.Run(() => repo.UpdateTableStatus(_currentTable.TableId, TableStatus.Available));

                var closedOrderId = _activeOrder.OrderId;
                MessageBox.Show(this, "Checkout completed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CompleteOrderAndClose(closedOrderId, OrderCompletionType.Checkout);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to checkout order: " + ex.Message, "Checkout Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateActionButtons();
            }
        }

        private void CompleteOrderAndClose(int? orderId, OrderCompletionType completionType)
        {
            _activeOrder = null;
            ResetOrderLines(true);
            OnOrderCompleted(orderId, completionType);
            CloseHostedPanelIfNeeded();
        }

        private void CloseHostedPanelIfNeeded()
        {
            try
            {
                var frm = FindForm();
                // If hosted inside a modal dialog (ShowDialog) or a transient form, close that form
                // but do not close main application windows (ManagerMainForm/StaffMainForm).
                if (frm != null && !(frm is UI.ManagerMainForm) && !(frm is UI.StaffMainForm))
                {
                    try { frm.DialogResult = DialogResult.OK; } catch { }
                    try { frm.Close(); return; } catch { }
                }

                // Otherwise, if hosted inside another container control, remove this control
                if (Parent is Control parent && parent.Controls.Contains(this))
                {
                    parent.Controls.Remove(this);
                }
            }
            catch
            {
                // swallow exceptions to avoid breaking UI flow
            }
        }

        private void ResetOrderLines(bool clearSearch = false)
        {
            _orderLines.Clear();
            if (clearSearch)
            {
                tbMenuSearch.Clear();
            }
            RefreshOrderList();
        }

        // Helper to raise the OrderCompleted event. Other parts of the control
        // can call this when an order finishes.
        protected virtual void OnOrderCompleted(int? orderId, OrderCompletionType completionType)
        {
            OrderCompleted?.Invoke(this, new OrderCompletedEventArgs(orderId, completionType));
        }

        private class OrderLineEntry
        {
            public int MenuItemId { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
            public bool IsPersisted { get; set; }
            public decimal LineTotal => UnitPrice * Quantity;
        }
    }

    // Event args used when signalling order completion.
    public class OrderCompletedEventArgs : EventArgs
    {
        public int? OrderId { get; }
        public OrderCompletionType CompletionType { get; }

        public OrderCompletedEventArgs(int? orderId, OrderCompletionType completionType)
        {
            OrderId = orderId;
            CompletionType = completionType;
        }
    }

    public enum OrderCompletionType
    {
        Submitted,
        Checkout,
        Cancelled
    }
}
