using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using RMS.Models;

namespace RMS.Controls
{
    public partial class OrdersView : UserControl
    {
        private List<MenuItemEntity> _menuItems = new List<MenuItemEntity>();
        private List<TableInfo> _tables = new List<TableInfo>();
        private BindingList<OrderItem> _orderItems = new BindingList<OrderItem>();

        public OrdersView()
        {
            InitializeComponent();
            SetupDataGridView();
            WireUpEvents();
            LoadData();
        }

        private void WireUpEvents()
        {
            lvMenu.DoubleClick += LvMenu_DoubleClick;
            txtSearchMenu.TextChanged += TxtSearchMenu_TextChanged;
            dgvOrderItems.CellValueChanged += DgvOrderItems_CellValueChanged;
            dgvOrderItems.UserDeletingRow += DgvOrderItems_UserDeletingRow;
            btnSubmitOrder.Click += BtnSubmitOrder_Click;
            btnCancelOrder.Click += BtnCancelOrder_Click;
        }

        private void SetupDataGridView()
        {
            dgvOrderItems.AutoGenerateColumns = false;
            _orderItems = new BindingList<OrderItem>();
            dgvOrderItems.DataSource = _orderItems;

            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "MenuItemId", DataPropertyName = "MenuItemId", Visible = false });
            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "ItemName", HeaderText = "Item", DataPropertyName = "Name", ReadOnly = true, Width = 150 });
            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quantity", HeaderText = "Qty", DataPropertyName = "Quantity", ReadOnly = false, Width = 50 });
            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "Price", DataPropertyName = "Price", ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Format = "c" }, Width = 70 });
            dgvOrderItems.Columns.Add(new DataGridViewTextBoxColumn { Name = "Total", HeaderText = "Total", DataPropertyName = "Total", ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Format = "c" }, Width = 70 });
        }

        private void LoadData()
        {
            LoadTables();
            LoadMenu();
        }

        private void LoadTables()
        {
            _tables.Clear();
            try
            {
                using (var cn = new SqlConnection(Global.CurrentConnectionString))
                {
                    cn.Open();
                    var cmd = new SqlCommand("SELECT TableId, Code, Status FROM dbo.Tables WHERE Status IN (0, 2)", cn); // Available or Occupied
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            _tables.Add(new TableInfo { TableId = rdr.GetInt32(0), Code = rdr.GetString(1), Status = (TableStatus)rdr.GetByte(2) });
                        }
                    }
                }
                cmbTables.DataSource = _tables;
                cmbTables.DisplayMember = "Code";
                cmbTables.ValueMember = "TableId";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load tables: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMenu()
        {
            _menuItems.Clear();
            try
            {
                using (var cn = new SqlConnection(Global.CurrentConnectionString))
                {
                    cn.Open();
                    var cmd = new SqlCommand("SELECT MenuItemId, Name, Price FROM dbo.MenuItems WHERE IsActive = 1", cn);
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            _menuItems.Add(new MenuItemEntity { MenuItemId = rdr.GetInt32(0), Name = rdr.GetString(1), Price = rdr.GetDecimal(2) });
                        }
                    }
                }
                PopulateMenuListView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load menu items: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateMenuListView()
        {
            lvMenu.Items.Clear();
            lvMenu.View = View.Details;
            if (lvMenu.Columns.Count == 0)
            {
                lvMenu.Columns.Add("Name", 200);
                lvMenu.Columns.Add("Price", 70);
            }

            var filter = txtSearchMenu.Text.Trim().ToLowerInvariant();

            foreach (var item in _menuItems)
            {
                if (string.IsNullOrEmpty(filter) || item.Name.ToLowerInvariant().Contains(filter))
                {
                    var lvi = new ListViewItem(item.Name);
                    lvi.SubItems.Add(item.Price.ToString("C"));
                    lvi.Tag = item;
                    lvMenu.Items.Add(lvi);
                }
            }
        }

        private void LvMenu_DoubleClick(object sender, EventArgs e)
        {
            if (lvMenu.SelectedItems.Count == 0) return;

            var selectedMenuItem = (MenuItemEntity)lvMenu.SelectedItems[0].Tag;
            var existingItem = _orderItems.FirstOrDefault(i => i.MenuItemId == selectedMenuItem.MenuItemId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                _orderItems.Add(new OrderItem(selectedMenuItem));
            }
            _orderItems.ResetBindings();
            UpdateTotals();
        }

        private void TxtSearchMenu_TextChanged(object sender, EventArgs e)
        {
            PopulateMenuListView();
        }

        private void DgvOrderItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvOrderItems.Columns[e.ColumnIndex].Name == "Quantity")
            {
                var item = _orderItems[e.RowIndex];
                if (item.Quantity <= 0)
                {
                    _orderItems.RemoveAt(e.RowIndex);
                }
                else
                {
                     _orderItems.ResetItem(e.RowIndex);
                }
                UpdateTotals();
            }
        }

        private void DgvOrderItems_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            // This event is tricky, let's re-calculate totals after a slight delay
            BeginInvoke(new Action(UpdateTotals));
        }

        private void UpdateTotals()
        {
            decimal subtotal = _orderItems.Sum(i => i.Total);
            decimal tax = subtotal * 0.10m; // 10% tax
            decimal total = subtotal + tax;

            lblSubtotal.Text = subtotal.ToString("C");
            lblTax.Text = tax.ToString("C");
            lblTotal.Text = total.ToString("C");
        }

        private void BtnSubmitOrder_Click(object sender, EventArgs e)
        {
            if (cmbTables.SelectedValue == null)
            {
                MessageBox.Show("Please select a table.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_orderItems.Count == 0)
            {
                MessageBox.Show("Please add items to the order.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SubmitOrder();
        }

        private void SubmitOrder()
        {
            var tableId = (int)cmbTables.SelectedValue;
            decimal subtotal = _orderItems.Sum(i => i.Total);
            decimal tax = subtotal * 0.10m;
            decimal total = subtotal + tax;

            using (var cn = new SqlConnection(Global.CurrentConnectionString))
            {
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    try
                    {
                        var sql = @"INSERT INTO dbo.Orders (TableId, Subtotal, Tax, Total, Status, CreatedAtUtc, IsPaid)
                                    OUTPUT INSERTED.OrderId
                                    VALUES (@TableId, @Subtotal, @Tax, @Total, 1, SYSUTCDATETIME(), 0);"; // Status 1 = InKitchen
                        var cmd = new SqlCommand(sql, cn, tx);
                        cmd.Parameters.AddWithValue("@TableId", tableId);
                        cmd.Parameters.AddWithValue("@Subtotal", subtotal);
                        cmd.Parameters.AddWithValue("@Tax", tax);
                        cmd.Parameters.AddWithValue("@Total", total);

                        var orderId = (int)cmd.ExecuteScalar();

                        foreach (var item in _orderItems)
                        {
                            var lineSql = @"INSERT INTO dbo.OrderLines (OrderId, MenuItemId, Quantity, UnitPrice, Status)
                                            VALUES (@OrderId, @MenuItemId, @Quantity, @UnitPrice, 0);"; // Status 0 = Pending
                            var lineCmd = new SqlCommand(lineSql, cn, tx);
                            lineCmd.Parameters.AddWithValue("@OrderId", orderId);
                            lineCmd.Parameters.AddWithValue("@MenuItemId", item.MenuItemId);
                            lineCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                            lineCmd.Parameters.AddWithValue("@UnitPrice", item.Price);
                            lineCmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                        MessageBox.Show($"Order #{orderId} submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearForm();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        MessageBox.Show("Failed to submit order: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnCancelOrder_Click(object sender, EventArgs e)
        {
            if (_orderItems.Any())
            {
                var result = MessageBox.Show("Are you sure you want to cancel this order?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    ClearForm();
                }
            }
            else
            {
                ClearForm();
            }
        }

        private void ClearForm()
        {
            _orderItems.Clear();
            txtSearchMenu.Clear();
            cmbTables.SelectedIndex = -1;
            UpdateTotals();
        }
    }

    public class OrderItem : INotifyPropertyChanged
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public decimal Total => Price * Quantity;

        public OrderItem(MenuItemEntity menuItem)
        {
            MenuItemId = menuItem.MenuItemId;
            Name = menuItem.Name;
            Price = menuItem.Price;
            Quantity = 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
