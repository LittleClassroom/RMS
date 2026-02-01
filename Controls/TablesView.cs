using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RMS.Models;
using Microsoft.Data.SqlClient;

namespace RMS.Controls
{
    public partial class TablesView : UserControl
    {
        private List<TableInfo> _tables = new List<TableInfo>();

        public TablesView()
        {
            InitializeComponent();
            Load += TablesView_Load;
        }

        private async void TablesView_Load(object? sender, EventArgs e)
        {
            // Load tables from the database instead of using sample data
            var connString = Global.CurrentConnectionString;
            if (string.IsNullOrWhiteSpace(connString))
            {
                MessageBox.Show(this, "Database connection not available.", "Tables", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                _tables = await System.Threading.Tasks.Task.Run(() => FetchTables(connString));
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to load tables: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _tables = new List<TableInfo>();
            }
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
            tablesFlowPanel.Controls.Clear();

            foreach (var t in _tables)
            {
                var btn = CreateTableButton(t);
                tablesFlowPanel.Controls.Add(btn);
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
    }
}
