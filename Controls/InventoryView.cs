using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RMS.Data.SqlServer;
using RMS.Models;

namespace RMS.Controls
{
    public partial class InventoryView : UserControl
    {
        private RmsRepository? _repo;
        private ContextMenuStrip? _ctx;
        private readonly List<InventoryCategory> _categories = new();
        private readonly List<InventorySubcategory> _subcategories = new();
        private readonly List<InventoryItem> _items = new();
        private bool _suppressFilterEvents;

        public InventoryView()
        {
            InitializeComponent();
            _ctx = new ContextMenuStrip();
            _ctx.Items.Add("Refresh").Click += (_, __) => RefreshData();
            _ctx.Items.Add("View Transactions").Click += (_, __) => ViewTransactionsForSelected();
            _ctx.Items.Add(new ToolStripSeparator());
            _ctx.Items.Add("Add Receipt").Click += (_, __) => AddTransaction(2);
            _ctx.Items.Add("Add Usage").Click += (_, __) => AddTransaction(1);

            inventoryGrid.MouseUp += InventoryGrid_MouseUp;
            inventoryGrid.SelectionChanged += (_, __) => OnSelectionChanged();
            inventoryGrid.CellDoubleClick += (_, __) => AddTransaction(2, true);

            btnRefresh.Click += (_, __) => RefreshData();
            btnAddNew.Click += btnAddNew_Click;
            cbCategory.SelectedIndexChanged += (_, __) => FilterChanged();
            cbSubcategory.SelectedIndexChanged += (_, __) => RefreshData();
        }

        private void InventoryGrid_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && _ctx != null)
            {
                var hit = inventoryGrid.HitTest(e.X, e.Y);
                if (hit.RowIndex >= 0 && hit.RowIndex < inventoryGrid.Rows.Count)
                {
                    inventoryGrid.ClearSelection();
                    inventoryGrid.Rows[hit.RowIndex].Selected = true;
                }
                _ctx.Show(inventoryGrid, e.Location);
            }
        }

        private void FilterChanged()
        {
            if (_suppressFilterEvents)
            {
                return;
            }

            BindSubcategoryFilter();
            RefreshData();
        }

        // Button in designer to add new ingredient
        private void btnAddNew_Click(object? sender, EventArgs e)
        {
            if (_repo == null)
            {
                MessageBox.Show(this, "Database connection not configured.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var dlg = new RMS.UI.EditInventoryItemDialog(_categories, _subcategories);
            if (dlg.ShowDialog(this) != DialogResult.OK || dlg.ResultItem == null)
            {
                return;
            }

            try
            {
                _repo.InsertInventoryItem(dlg.ResultItem);
                RefreshData();
                MessageBox.Show(this, "Inventory item saved.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to save inventory item: " + ex.Message, "Inventory Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnSelectionChanged()
        {
            // could update alert panel or details if present
        }

        public void ConfigureRepository(RmsRepository? repo)
        {
            _repo = repo;
            LoadFilters();
            RefreshData();
        }

        public void RefreshData()
        {
            if (_repo == null)
            {
                inventoryGrid.Rows.Clear();
                lblAlertsList.Text = "Inventory unavailable.";
                return;
            }

            try
            {
                var items = _repo.GetInventoryItems(GetSelectedCategoryId(), GetSelectedSubcategoryId()).ToList();
                _items.Clear();
                _items.AddRange(items);
                inventoryGrid.Rows.Clear();
                foreach (var item in items)
                {
                    var status = item.CurrentStock <= item.ReorderLevel ? "⚠️ Low" : "✅ OK";
                    inventoryGrid.Rows.Add(
                        item.Name,
                        item.CategoryName ?? "(None)",
                        item.SubcategoryName ?? "(None)",
                        item.CurrentStock.ToString("0.###"),
                        item.Unit ?? string.Empty,
                        item.ReorderLevel.ToString("0.###"),
                        status);
                    inventoryGrid.Rows[inventoryGrid.Rows.Count - 1].Tag = item;
                }

                UpdateAlerts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to load inventory: " + ex.Message, "Inventory Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ViewTransactionsForSelected()
        {
            if (_repo == null)
            {
                MessageBox.Show(this, "Database connection not configured.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var item = GetSelectedItem();
            if (item == null)
            {
                MessageBox.Show(this, "Select an item first.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dlg = new Form();
            dlg.Text = $"Transactions - {item.Name}";
            dlg.ClientSize = new System.Drawing.Size(600, 400);
            var lv = new ListView { Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true };
            lv.Columns.Add("Date", 160);
            lv.Columns.Add("Type", 80);
            lv.Columns.Add("Qty", 80);
            lv.Columns.Add("Reference", 160);
            lv.Columns.Add("Source", 120);

            try
            {
                var txs = _repo.GetInventoryTransactions(item.InventoryItemId);
                foreach (var tx in txs)
                {
                    var lvi = new ListViewItem(new[]
                    {
                        tx.CreatedAtUtc.ToLocalTime().ToString("g"),
                        DescribeTransactionType(tx.Type),
                        tx.Quantity.ToString("0.###"),
                        tx.Reference ?? string.Empty,
                        tx.SourceType ?? string.Empty
                    });
                    lv.Items.Add(lvi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to load transactions: " + ex.Message, "Inventory Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dlg.Controls.Add(lv);
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.ShowDialog(this);
        }

        private void AddTransaction(byte defaultType, bool allowTypeOverride = false)
        {
            if (_repo == null)
            {
                MessageBox.Show(this, "Database connection not configured.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var item = GetSelectedItem();
            if (item == null)
            {
                MessageBox.Show(this, "Select an item first.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dlg = new RMS.UI.AddInventoryTransactionDialog(item.Name, defaultType, allowTypeOverride);
            if (dlg.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var typeToUse = allowTypeOverride ? dlg.TransactionType : defaultType;

            try
            {
                _repo.CreateInventoryTransaction(item.InventoryItemId, typeToUse, dlg.Quantity, dlg.Reference, dlg.SourceType, null, null);
                RefreshData();
                MessageBox.Show(this, "Transaction recorded.", "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to record transaction: " + ex.Message, "Inventory Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private InventoryItem? GetSelectedItem()
        {
            if (inventoryGrid.SelectedRows.Count == 0)
            {
                return null;
            }

            return inventoryGrid.SelectedRows[0].Tag as InventoryItem;
        }

        private void LoadFilters()
        {
            if (_repo == null)
            {
                cbCategory.DataSource = null;
                cbSubcategory.DataSource = null;
                return;
            }

            try
            {
                _categories.Clear();
                _categories.AddRange(_repo.GetInventoryCategories());
                _subcategories.Clear();
                _subcategories.AddRange(_repo.GetInventorySubcategories());
                BindCategoryFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to load inventory filters: " + ex.Message, "Inventory", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindCategoryFilter()
        {
            _suppressFilterEvents = true;
            var source = new List<InventoryCategory> { new InventoryCategory { CategoryId = 0, Name = "All Categories" } };
            source.AddRange(_categories);
            cbCategory.DisplayMember = nameof(InventoryCategory.Name);
            cbCategory.ValueMember = nameof(InventoryCategory.CategoryId);
            cbCategory.DataSource = source;
            cbCategory.SelectedIndex = 0;
            BindSubcategoryFilter();
            _suppressFilterEvents = false;
        }

        private void BindSubcategoryFilter()
        {
            _suppressFilterEvents = true;
            var selectedCategory = GetSelectedCategoryId();
            var subSource = new List<InventorySubcategory> { new InventorySubcategory { SubcategoryId = 0, Name = "All Subcategories", CategoryId = selectedCategory ?? 0 } };
            subSource.AddRange(_subcategories.Where(s => !selectedCategory.HasValue || s.CategoryId == selectedCategory.Value));
            cbSubcategory.DisplayMember = nameof(InventorySubcategory.Name);
            cbSubcategory.ValueMember = nameof(InventorySubcategory.SubcategoryId);
            cbSubcategory.DataSource = subSource;
            cbSubcategory.SelectedIndex = 0;
            _suppressFilterEvents = false;
        }

        private int? GetSelectedCategoryId()
        {
            return cbCategory.SelectedItem is InventoryCategory cat && cat.CategoryId > 0
                ? cat.CategoryId
                : null;
        }

        private int? GetSelectedSubcategoryId()
        {
            return cbSubcategory.SelectedItem is InventorySubcategory sub && sub.SubcategoryId > 0
                ? sub.SubcategoryId
                : null;
        }

        private void UpdateAlerts()
        {
            var lowItems = _items.Where(i => i.CurrentStock <= i.ReorderLevel && i.ReorderLevel > 0).Select(i => i.Name).ToList();
            if (lowItems.Count == 0)
            {
                lblAlertsList.Text = "All items are above their reorder level.";
                return;
            }

            var preview = string.Join(", ", lowItems.Take(5));
            if (lowItems.Count > 5)
            {
                preview += $" (+{lowItems.Count - 5} more)";
            }
            lblAlertsList.Text = preview;
        }

        private static string DescribeTransactionType(byte type)
        {
            return type switch
            {
                2 => "Receipt",
                1 => "Usage",
                _ => "Adjustment"
            };
        }
    }
}