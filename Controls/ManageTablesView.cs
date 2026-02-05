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
    public partial class ManageTablesView : UserControl
    {
        private readonly BindingSource _binding = new();
        private readonly List<TableInfo> _allTables = new();
        private bool _isLoading;

        public ManageTablesView()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                Load += ManageTablesView_Load;
            }

            dgvTables.AutoGenerateColumns = false;
            dgvTables.DataSource = _binding;
            dgvTables.SelectionChanged += (_, __) => UpdateButtonStates();
            tbSearch.TextChanged += (_, __) => ApplyFilter();
            cbStatusFilter.SelectedIndexChanged += (_, __) => ApplyFilter();
            btnRefresh.Click += async (_, __) => await LoadTablesAsync();
            btnAdd.Click += async (_, __) => await AddTableAsync();
            btnEdit.Click += async (_, __) => await EditTableAsync();
            btnDelete.Click += async (_, __) => await DeleteTableAsync();
            btnApplyStatus.Click += async (_, __) => await ApplyStatusAsync();

            try { tbSearch.PlaceholderText = "Search code, location or notes"; } catch { }

            PopulateStatusCombos();
        }

        private void ManageTablesView_Load(object? sender, EventArgs e)
        {
            _ = LoadTablesAsync();
        }

        public Task ReloadDataAsync() => LoadTablesAsync();

        private async Task LoadTablesAsync()
        {
            if (_isLoading)
            {
                return;
            }

            if (!TryGetConnection(out var connectionString))
            {
                ShowStatus("Configure database credentials to manage tables.");
                _binding.DataSource = null;
                UpdateButtonStates();
                return;
            }

            try
            {
                _isLoading = true;
                ShowStatus("Loading tables...");
                ToggleInputs(false);
                var repo = new RmsRepository(connectionString);
                var data = await Task.Run(() => repo.GetTables().OrderBy(t => t.Code).ToList());
                _allTables.Clear();
                _allTables.AddRange(data);
                // Ensure status filter is set to "All Statuses" and show all rows on initial load
                try { if (cbStatusFilter.Items.Count > 0) cbStatusFilter.SelectedIndex = 0; } catch { }
                // Bind all tables directly (no filtering) so the grid shows everything on load
                var list = new BindingList<TableInfo>(_allTables);
                _binding.DataSource = list;
                try { dgvTables.DataSource = null; dgvTables.DataSource = _binding; dgvTables.ClearSelection(); if (dgvTables.Rows.Count > 0) dgvTables.CurrentCell = null; } catch { }
                // fallback manual populate if necessary
                try { if (dgvTables.Rows.Count == 0 && _allTables.Count > 0) { dgvTables.Rows.Clear(); foreach (var t in _allTables) { var idx = dgvTables.Rows.Add(t.Code, t.Location, t.Capacity, t.Status.ToString(), t.Notes ?? string.Empty); dgvTables.Rows[idx].Tag = t; } } } catch { }
                ShowStatus(data.Count == 1 ? "1 table" : $"{data.Count} tables");
            }
            catch (Exception ex)
            {
                ShowStatus("Failed to load tables: " + ex.Message);
            }
            finally
            {
                ToggleInputs(true);
                _isLoading = false;
                // Ensure UI is refreshed with the loaded data after loading completes
                try { ApplyFilter(); } catch { }
            }
        }

        private async Task AddTableAsync()
        {
            using var dlg = new EditTableDialog();
            if (dlg.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            await SaveTableAsync(dlg.Result, isNew: true);
        }

        private async Task EditTableAsync()
        {
            var selected = GetSelectedTable();
            if (selected == null)
            {
                return;
            }

            using var dlg = new EditTableDialog(selected);
            if (dlg.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            await SaveTableAsync(dlg.Result, isNew: false);
        }

        private async Task DeleteTableAsync()
        {
            var selected = GetSelectedTable();
            if (selected == null)
            {
                return;
            }

            if (MessageBox.Show(this, $"Delete table {selected.Code}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            if (!TryGetConnection(out var connectionString))
            {
                return;
            }

            try
            {
                ToggleInputs(false);
                var repo = new RmsRepository(connectionString);
                await Task.Run(() => repo.DeleteTable(selected.TableId));
                await LoadTablesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to delete table: " + ex.Message, "Tables", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ToggleInputs(true);
            }
        }

        private async Task ApplyStatusAsync()
        {
            var selected = GetSelectedTable();
            if (selected == null)
            {
                return;
            }

            if (cbStatusAction.SelectedItem is not StatusOption target)
            {
                return;
            }

            if (!TryGetConnection(out var connectionString))
            {
                return;
            }

            try
            {
                ToggleInputs(false);
                var repo = new RmsRepository(connectionString);
                await Task.Run(() => repo.UpdateTableStatus(selected.TableId, target.Status!.Value));
                await LoadTablesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to update status: " + ex.Message, "Tables", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ToggleInputs(true);
            }
        }

        private async Task SaveTableAsync(TableInfo table, bool isNew)
        {
            if (!TryGetConnection(out var connectionString))
            {
                return;
            }

            try
            {
                ToggleInputs(false);
                var repo = new RmsRepository(connectionString);
                if (isNew)
                {
                    await Task.Run(() => repo.InsertTable(table));
                }
                else
                {
                    await Task.Run(() => repo.UpdateTable(table));
                }
                await LoadTablesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to save table: " + ex.Message, "Tables", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ToggleInputs(true);
            }
        }

        private void ApplyFilter()
        {
            if (_isLoading)
            {
                return;
            }

            IEnumerable<TableInfo> query = _allTables;
            var term = tbSearch.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(term))
            {
                var lowered = term.ToLowerInvariant();
                query = query.Where(t =>
                    (t.Code ?? string.Empty).ToLowerInvariant().Contains(lowered) ||
                    (t.Location ?? string.Empty).ToLowerInvariant().Contains(lowered) ||
                    (t.Notes ?? string.Empty).ToLowerInvariant().Contains(lowered));
            }

            if (cbStatusFilter.SelectedItem is StatusOption option && option.Status.HasValue)
            {
                query = query.Where(t => t.Status == option.Status.Value);
            }

            var filtered = query.ToList();
            // Update binding source and force DataGridView refresh — reassign DataSource to ensure UI updates
            var list = new BindingList<TableInfo>(filtered);
            _binding.DataSource = list;
            // Rebind the grid explicitly to force a refresh
            try
            {
                dgvTables.DataSource = null;
                dgvTables.DataSource = _binding;
                dgvTables.ClearSelection();
                if (dgvTables.Rows.Count > 0)
                {
                    dgvTables.CurrentCell = null;
                }
            }
            catch { }
            // If binding did not render rows for some reason, fallback to manual fill to ensure UI shows data
            try
            {
                if (dgvTables.Rows.Count == 0 && filtered.Count > 0)
                {
                    dgvTables.Rows.Clear();
                    foreach (var t in filtered)
                    {
                        var idx = dgvTables.Rows.Add(t.Code, t.Location, t.Capacity, t.Status.ToString(), t.Notes ?? string.Empty);
                        dgvTables.Rows[idx].Tag = t;
                    }
                }
            }
            catch { }
            lblSummary.Text = filtered.Count == 1 ? "1 table" : $"{filtered.Count} tables";
            UpdateButtonStates();
        }

        private TableInfo? GetSelectedTable()
        {
            if (dgvTables.CurrentRow?.DataBoundItem is TableInfo info)
            {
                return info;
            }
            return null;
        }

        private void ToggleInputs(bool enabled)
        {
            btnRefresh.Enabled = enabled;
            btnAdd.Enabled = enabled;
            btnEdit.Enabled = enabled;
            btnDelete.Enabled = enabled;
            btnApplyStatus.Enabled = enabled;
            cbStatusAction.Enabled = enabled;
        }

        private void UpdateButtonStates()
        {
            var hasSelection = GetSelectedTable() != null;
            btnEdit.Enabled = hasSelection && !_isLoading;
            btnDelete.Enabled = hasSelection && !_isLoading;
            btnApplyStatus.Enabled = hasSelection && !_isLoading;
        }

        private void ShowStatus(string text)
        {
            lblSummary.Text = text;
        }

        private void PopulateStatusCombos()
        {
            var options = new List<StatusOption>
            {
                new StatusOption("All Statuses", null)
            };
            options.AddRange(Enum.GetValues<TableStatus>().Select(s => new StatusOption(s.ToString(), s)));
            cbStatusFilter.DataSource = options.ToList();
            cbStatusAction.DataSource = Enum.GetValues<TableStatus>().Select(s => new StatusOption(s.ToString(), s)).ToList();
        }

        private bool TryGetConnection(out string connection)
        {
            connection = Global.CurrentConnectionString ?? string.Empty;
            if (string.IsNullOrWhiteSpace(connection))
            {
                MessageBox.Show(this, "Database connection is not configured.", "Tables", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private sealed class StatusOption
        {
            public string Label { get; }
            public TableStatus? Status { get; }

            public StatusOption(string label, TableStatus? status)
            {
                Label = label;
                Status = status;
            }

            public override string ToString() => Label;
        }
    }
}
