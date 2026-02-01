using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using RMS.Models;

namespace RMS.UI
{
    public class ManageStaffView : UserControl
    {
        private readonly List<User> _users = new();
        private ListView lv;
        private TextBox tbSearch;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private Label lblInfo;

        public ManageStaffView()
        {
            InitializeComponent();
            Load += ManageStaffView_Load;
        }

        private void InitializeComponent()
        {
            Dock = DockStyle.Fill;

            var topPanel = new Panel { Dock = DockStyle.Top, Height = 48, Padding = new Padding(8, 8, 8, 0) };
            tbSearch = new TextBox { PlaceholderText = "Search username or display name", Width = 260, Dock = DockStyle.Left };
            tbSearch.TextChanged += (s, e) => ApplyFilter();

            btnRefresh = new Button { Text = "Refresh", Width = 80, Height = 27, Dock = DockStyle.Right };
            btnRefresh.Click += (s, e) => RefreshList();

            topPanel.Controls.Add(btnRefresh);
            topPanel.Controls.Add(tbSearch);

            lv = new ListView
            {
                View = View.Details,
                Dock = DockStyle.Fill,
                FullRowSelect = true,
                HideSelection = false,
                MultiSelect = false
            };
            lv.Columns.Add("ID", 60);
            lv.Columns.Add("Username", 150);
            lv.Columns.Add("Display Name", 200);
            lv.Columns.Add("Role", 100);
            lv.Columns.Add("Active", 60);
            lv.DoubleClick += (s, e) => BtnEdit_Click(s, e);

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(8)
            };

            btnAdd = new Button { Text = "Add", Width = 90, Height = 32 };
            btnEdit = new Button { Text = "Edit", Width = 90, Height = 32 };
            btnDelete = new Button { Text = "Delete", Width = 90, Height = 32 };

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;

            buttonPanel.Controls.Add(btnAdd);
            buttonPanel.Controls.Add(btnEdit);
            buttonPanel.Controls.Add(btnDelete);

            lblInfo = new Label
            {
                Text = "Connect to the database to manage staff.",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Gray,
                Visible = false
            };

            Controls.Add(lv);
            Controls.Add(lblInfo);
            Controls.Add(buttonPanel);
            Controls.Add(topPanel);
        }

        private void ManageStaffView_Load(object? sender, EventArgs e)
        {
            RefreshList();
        }

        private bool EnsureConnection()
        {
            if (string.IsNullOrWhiteSpace(RMS.Global.CurrentConnectionString))
            {
                lblInfo.Text = "Database connection not configured. Configure DB credentials first.";
                lblInfo.Visible = true;
                lv.Visible = false;
                SetButtonsEnabled(false);
                return false;
            }

            lblInfo.Visible = false;
            lv.Visible = true;
            SetButtonsEnabled(true);
            return true;
        }

        private void SetButtonsEnabled(bool enabled)
        {
            btnAdd.Enabled = enabled;
            btnEdit.Enabled = enabled && lv.SelectedItems.Count > 0;
            btnDelete.Enabled = enabled && lv.SelectedItems.Count > 0;
            btnRefresh.Enabled = enabled;
        }

        private void RefreshList()
        {
            if (!EnsureConnection()) return;

            _users.Clear();
            try
            {
                using var con = new SqlConnection(RMS.Global.CurrentConnectionString);
                con.Open();
                using var cmd = new SqlCommand("SELECT UserId, Username, DisplayName, Role, IsActive FROM dbo.Users ORDER BY UserId", con);
                using var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    _users.Add(new User
                    {
                        UserId = rdr.GetInt32(0),
                        Username = rdr.GetString(1),
                        DisplayName = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2),
                        Role = (UserRole)rdr.GetByte(3),
                        IsActive = rdr.GetBoolean(4)
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to load users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            lv.BeginUpdate();
            lv.Items.Clear();
            var filter = tbSearch.Text?.Trim() ?? string.Empty;
            IEnumerable<User> items = _users;
            if (!string.IsNullOrEmpty(filter))
            {
                var f = filter.ToLowerInvariant();
                items = items.Where(u => (u.Username?.ToLowerInvariant().Contains(f) ?? false) ||
                                         (u.DisplayName?.ToLowerInvariant().Contains(f) ?? false));
            }

            foreach (var user in items)
            {
                var item = new ListViewItem(new[]
                {
                    user.UserId.ToString(),
                    user.Username,
                    user.DisplayName ?? string.Empty,
                    user.Role.ToString(),
                    user.IsActive ? "Yes" : "No"
                }) { Tag = user.UserId };
                lv.Items.Add(item);
            }
            lv.EndUpdate();
            SetButtonsEnabled(lv.Items.Count > 0 && lv.SelectedItems.Count > 0);
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (!EnsureConnection()) return;
            using var dlg = new EditStaffDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                RefreshList();
            }
        }

        private int? SelectedId() => lv.SelectedItems.Count == 0 ? null : (int?)lv.SelectedItems[0].Tag;

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (!EnsureConnection()) return;
            var id = SelectedId();
            if (id == null) return;
            using var dlg = new EditStaffDialog(id.Value);
            if (dlg.ShowDialog(this) == DialogResult.OK) RefreshList();
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (!EnsureConnection()) return;
            var id = SelectedId();
            if (id == null) return;

            if (MessageBox.Show(this, "Delete selected user?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                using var con = new SqlConnection(RMS.Global.CurrentConnectionString);
                con.Open();
                using var cmd = new SqlCommand("DELETE FROM dbo.Users WHERE UserId = @id", con);
                cmd.Parameters.AddWithValue("@id", id.Value);
                cmd.ExecuteNonQuery();
                RefreshList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to delete user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            lv.SelectedIndexChanged += (s, args) => SetButtonsEnabled(lv.SelectedItems.Count > 0);
        }
    }
}
