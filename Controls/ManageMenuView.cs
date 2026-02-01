using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RMS.Data.SqlServer;
using RMS.Models;
using RMS.UI;

namespace RMS.Controls
{
    public class ManageMenuView : UserControl
    {
        private IContainer components;
        private TableLayoutPanel layout;
        private FlowLayoutPanel actionPanel;
        private TextBox tbSearch;
        private CheckBox chkShowInactive;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private SplitContainer split;
        private ListBox lstCategories;
        private TableLayoutPanel rightLayout;
        private ListView lvItems;
        private Panel infoPanel;
        private PictureBox pbPreview;
        private Label lblDetails;
        private Label lblStatus;
        private ImageList imageListSmall;

        private readonly List<MenuCategory> _categories = new List<MenuCategory>();
        private readonly List<MenuItemEntity> _items = new List<MenuItemEntity>();
        private RmsRepository? _repository;

        public ManageMenuView()
        {
            InitializeComponent();
        }

        public void ConfigureRepository(RmsRepository? repository)
        {
            _repository = repository;
            _ = RefreshAllAsync();
        }

        private void InitializeComponent()
        {
            components = new Container();
            layout = new TableLayoutPanel();
            actionPanel = new FlowLayoutPanel();
            tbSearch = new TextBox();
            chkShowInactive = new CheckBox();
            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            split = new SplitContainer();
            lstCategories = new ListBox();
            rightLayout = new TableLayoutPanel();
            lvItems = new ListView();
            imageListSmall = new ImageList(components);
            infoPanel = new Panel();
            pbPreview = new PictureBox();
            lblDetails = new Label();
            lblStatus = new Label();
            layout.SuspendLayout();
            actionPanel.SuspendLayout();
            ((ISupportInitialize)split).BeginInit();
            split.Panel1.SuspendLayout();
            split.Panel2.SuspendLayout();
            split.SuspendLayout();
            rightLayout.SuspendLayout();
            infoPanel.SuspendLayout();
            ((ISupportInitialize)pbPreview).BeginInit();
            SuspendLayout();
            // 
            // layout
            // 
            layout.ColumnCount = 1;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            layout.Controls.Add(actionPanel, 0, 0);
            layout.Controls.Add(split, 0, 1);
            layout.Controls.Add(lblStatus, 0, 2);
            layout.Dock = DockStyle.Fill;
            layout.Location = new Point(0, 0);
            layout.Name = "layout";
            layout.RowCount = 3;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            layout.Size = new Size(1270, 512);
            layout.TabIndex = 0;
            // 
            // actionPanel
            // 
            actionPanel.Controls.Add(tbSearch);
            actionPanel.Controls.Add(chkShowInactive);
            actionPanel.Controls.Add(btnAdd);
            actionPanel.Controls.Add(btnEdit);
            actionPanel.Controls.Add(btnDelete);
            actionPanel.Controls.Add(btnRefresh);
            actionPanel.Dock = DockStyle.Top;
            actionPanel.Location = new Point(3, 3);
            actionPanel.Name = "actionPanel";
            actionPanel.Padding = new Padding(8);
            actionPanel.Size = new Size(1264, 42);
            actionPanel.TabIndex = 0;
            actionPanel.WrapContents = false;
            // 
            // tbSearch
            // 
            tbSearch.Location = new Point(12, 12);
            tbSearch.Margin = new Padding(4);
            tbSearch.Name = "tbSearch";
            tbSearch.Size = new Size(260, 23);
            tbSearch.TabIndex = 0;
            tbSearch.TextChanged += TbSearch_TextChanged;
            // 
            // chkShowInactive
            // 
            chkShowInactive.AutoSize = true;
            chkShowInactive.Location = new Point(282, 14);
            chkShowInactive.Margin = new Padding(6);
            chkShowInactive.Name = "chkShowInactive";
            chkShowInactive.Size = new Size(99, 19);
            chkShowInactive.TabIndex = 1;
            chkShowInactive.Text = "Show inactive";
            chkShowInactive.CheckedChanged += ChkShowInactive_CheckedChanged;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(393, 14);
            btnAdd.Margin = new Padding(6);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(90, 23);
            btnAdd.TabIndex = 2;
            btnAdd.Text = "Add";
            btnAdd.Click += BtnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.Enabled = false;
            btnEdit.Location = new Point(495, 14);
            btnEdit.Margin = new Padding(6);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(90, 23);
            btnEdit.TabIndex = 3;
            btnEdit.Text = "Edit";
            btnEdit.Click += BtnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.Enabled = false;
            btnDelete.Location = new Point(597, 14);
            btnDelete.Margin = new Padding(6);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(90, 23);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "Delete";
            btnDelete.Click += BtnDelete_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(699, 14);
            btnRefresh.Margin = new Padding(6);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(90, 23);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "Refresh";
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // split
            // 
            split.Dock = DockStyle.Fill;
            split.Location = new Point(3, 51);
            split.Name = "split";
            // 
            // split.Panel1
            // 
            split.Panel1.Controls.Add(lstCategories);
            split.Panel1Collapsed = true;
            // 
            // split.Panel2
            // 
            split.Panel2.Controls.Add(rightLayout);
            split.Size = new Size(1264, 434);
            split.TabIndex = 1;
            // 
            // lstCategories
            // 
            lstCategories.DisplayMember = "Name";
            lstCategories.Dock = DockStyle.Fill;
            lstCategories.IntegralHeight = false;
            lstCategories.Location = new Point(0, 0);
            lstCategories.Name = "lstCategories";
            lstCategories.Size = new Size(50, 100);
            lstCategories.TabIndex = 0;
            lstCategories.SelectedIndexChanged += LstCategories_SelectedIndexChanged;
            // 
            // rightLayout
            // 
            rightLayout.ColumnCount = 1;
            rightLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            rightLayout.Controls.Add(lvItems, 0, 0);
            rightLayout.Controls.Add(infoPanel, 0, 1);
            rightLayout.Dock = DockStyle.Fill;
            rightLayout.Location = new Point(0, 0);
            rightLayout.Name = "rightLayout";
            rightLayout.RowCount = 2;
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 65F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            rightLayout.Size = new Size(1264, 434);
            rightLayout.TabIndex = 0;
            // 
            // lvItems
            // 
            lvItems.Dock = DockStyle.Fill;
            lvItems.FullRowSelect = true;
            lvItems.Location = new Point(3, 3);
            lvItems.MultiSelect = false;
            lvItems.Name = "lvItems";
            lvItems.Size = new Size(194, 59);
            lvItems.SmallImageList = imageListSmall;
            lvItems.TabIndex = 0;
            lvItems.UseCompatibleStateImageBehavior = false;
            lvItems.View = View.Details;
            lvItems.GridLines = true;
            // ensure columns are created so items show in Details view
            lvItems.Columns.Clear();
            lvItems.Columns.Add("Name", 200);
            lvItems.Columns.Add("Category", 120);
            lvItems.Columns.Add("Price", 90);
            lvItems.Columns.Add("Size", 90);
            lvItems.Columns.Add("Active", 70);
            lvItems.Resize += LvItems_Resize;
            lvItems.SelectedIndexChanged += LvItems_SelectedIndexChanged;
            lvItems.DoubleClick += LvItems_DoubleClick;
            // 
            // imageListSmall
            // 
            imageListSmall.ColorDepth = ColorDepth.Depth32Bit;
            imageListSmall.ImageSize = new Size(48, 48);
            imageListSmall.TransparentColor = Color.Transparent;
            // 
            // infoPanel
            // 
            infoPanel.Controls.Add(pbPreview);
            infoPanel.Controls.Add(lblDetails);
            infoPanel.Dock = DockStyle.Fill;
            infoPanel.Location = new Point(3, 285);
            infoPanel.Name = "infoPanel";
            infoPanel.Padding = new Padding(8);
            infoPanel.Size = new Size(1258, 146);
            infoPanel.TabIndex = 1;
            // 
            // pbPreview
            // 
            pbPreview.BorderStyle = BorderStyle.FixedSingle;
            pbPreview.Dock = DockStyle.Left;
            pbPreview.Location = new Point(8, 8);
            pbPreview.Name = "pbPreview";
            pbPreview.Size = new Size(220, 130);
            pbPreview.SizeMode = PictureBoxSizeMode.Zoom;
            pbPreview.TabIndex = 0;
            pbPreview.TabStop = false;
            // 
            // lblDetails
            // 
            lblDetails.AutoEllipsis = true;
            lblDetails.Dock = DockStyle.Fill;
            lblDetails.Location = new Point(8, 8);
            lblDetails.Name = "lblDetails";
            lblDetails.Padding = new Padding(8);
            lblDetails.Size = new Size(1242, 130);
            lblDetails.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.Location = new Point(3, 488);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(1264, 24);
            lblStatus.TabIndex = 2;
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ManageMenuView
            // 
            Controls.Add(layout);
            Name = "ManageMenuView";
            Size = new Size(1270, 512);
            layout.ResumeLayout(false);
            actionPanel.ResumeLayout(false);
            actionPanel.PerformLayout();
            split.Panel1.ResumeLayout(false);
            split.Panel2.ResumeLayout(false);
            ((ISupportInitialize)split).EndInit();
            split.ResumeLayout(false);
            rightLayout.ResumeLayout(false);
            infoPanel.ResumeLayout(false);
            ((ISupportInitialize)pbPreview).EndInit();
            ResumeLayout(false);
        }

        private async void BtnRefresh_Click(object? sender, EventArgs e)
        {
            await RefreshAllAsync();
        }

        private async Task RefreshAllAsync()
        {
            if (_repository == null)
            {
                lblStatus.Text = "Database connection not available.";
                return;
            }

            try
            {
                var cats = await Task.Run(() => _repository.GetMenuCategories().ToList());
                var items = await Task.Run(() => _repository.GetMenuItemsByCategory(null).ToList());

                // If no items found, try the alternate simple query (GetMenuItems) to detect possible filtering/parameter issues
                if (items.Count == 0)
                {
                    try
                    {
                        var simple = await Task.Run(() => _repository.GetMenuItems().ToList());
                        if (simple.Count > 0)
                        {
                            // map simple result to MenuItemEntity so UI can show them
                            items = simple.Select(s => new MenuItemEntity
                            {
                                MenuItemId = s.Id,
                                Name = s.Name,
                                Price = s.Price,
                                Size = string.IsNullOrEmpty(s.Size) ? null : s.Size,
                                ImageFile = string.IsNullOrEmpty(s.ImageFile) ? null : s.ImageFile,
                                IsActive = true
                            }).ToList();
                        }
                        else
                        {
                            // still empty — surface helpful debug info
                            try { MessageBox.Show(this, "Menu query returned no rows. Check your database seed/data.\nSQL: SELECT TOP 20 MenuItemId, Name, IsActive FROM dbo.MenuItems", "No menu items", MessageBoxButtons.OK, MessageBoxIcon.Information); } catch { }
                        }
                    }
                    catch
                    {
                        // ignore secondary failure
                    }
                }

                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        _categories.Clear();
                        _categories.AddRange(cats);
                        var categoriesWithAll = new List<MenuCategory> { new MenuCategory { CategoryId = -1, Name = "All", SortOrder = -1 } };
                        categoriesWithAll.AddRange(_categories);
                        lstCategories.DataSource = categoriesWithAll;

                        _items.Clear();
                        _items.AddRange(items);
                        PopulateItems();
                        lblStatus.Text = _items.Count == 0 ? "No menu items found." : $"Loaded {_items.Count} menu items.";
                    }));
                }
                else
                {
                    _categories.Clear();
                    _categories.AddRange(cats);
                    var categoriesWithAll = new List<MenuCategory> { new MenuCategory { CategoryId = -1, Name = "All", SortOrder = -1 } };
                    categoriesWithAll.AddRange(_categories);
                    lstCategories.DataSource = categoriesWithAll;

                    _items.Clear();
                    _items.AddRange(items);
                    PopulateItems();
                    lblStatus.Text = _items.Count == 0 ? "No menu items found." : $"Loaded {_items.Count} menu items.";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Failed to load menu items: " + ex.Message;
            }
        }

        private void TbSearch_TextChanged(object? sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ChkShowInactive_CheckedChanged(object? sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void LvItems_DoubleClick(object? sender, EventArgs e)
        {
            BtnEdit_Click(sender, e);
        }

        private void LvItems_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateSelectionDetails();
        }

        private void LstCategories_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // categories panel hidden by default; if shown, selecting a category will filter
            if (split.Panel1Collapsed) return;
            _ = RefreshAllAsync();
        }

        private void PopulateItems()
        {
            lvItems.BeginUpdate();
            lvItems.Items.Clear();
            foreach (var item in _items)
            {
                var catName = "Uncategorized";
                if (item.CategoryId.HasValue)
                {
                    var cat = _categories.FirstOrDefault(c => c.CategoryId == item.CategoryId.Value);
                    if (cat != null) catName = cat.Name;
                }
                var lvi = new ListViewItem(new string[] { item.Name ?? string.Empty, catName, item.Price.ToString("C"), item.Size ?? string.Empty, item.IsActive ? "Yes" : "No" }) { Tag = item };
                lvItems.Items.Add(lvi);
            }
            lvItems.EndUpdate();
            // Ensure columns render correctly and refresh UI
            try
            {
                AdjustColumns();
            }
            catch { }
            lvItems.Refresh();
            UpdateSelectionDetails();
        }

        private void LvItems_Resize(object? sender, EventArgs e)
        {
            AdjustColumns();
        }

        private void AdjustColumns()
        {
            try
            {
                var total = Math.Max(1, lvItems.ClientSize.Width - SystemInformation.VerticalScrollBarWidth);
                // proportions: Name 40%, Category 20%, Price 12%, Size 18%, Active 10%
                var nameW = (int)(total * 0.40);
                var catW = (int)(total * 0.20);
                var priceW = (int)(total * 0.12);
                var sizeW = (int)(total * 0.18);
                var activeW = total - nameW - catW - priceW - sizeW;
                if (lvItems.Columns.Count >= 5)
                {
                    lvItems.Columns[0].Width = nameW;
                    lvItems.Columns[1].Width = catW;
                    lvItems.Columns[2].Width = priceW;
                    lvItems.Columns[3].Width = sizeW;
                    lvItems.Columns[4].Width = activeW;
                }
            }
            catch { }
        }

        private void ApplyFilter()
        {
            var filter = tbSearch.Text?.Trim().ToLowerInvariant() ?? string.Empty;
            var showInactive = chkShowInactive.Checked;
            lvItems.BeginUpdate();
            lvItems.Items.Clear();
            foreach (var item in _items)
            {
                if (!showInactive && !item.IsActive) continue;
                if (!string.IsNullOrEmpty(filter))
                {
                    var name = (item.Name ?? string.Empty).ToLowerInvariant();
                    var desc = (item.Description ?? string.Empty).ToLowerInvariant();
                    if (!name.Contains(filter) && !desc.Contains(filter)) continue;
                }

                var lvi = new ListViewItem(new string[] { item.Name ?? string.Empty, item.Price.ToString("C"), item.Size ?? string.Empty, item.IsActive ? "Yes" : "No" }) { Tag = item };
                lvItems.Items.Add(lvi);
            }
            lvItems.EndUpdate();
            UpdateSelectionDetails();
        }

        private void UpdateSelectionDetails()
        {
            if (lvItems.SelectedItems.Count == 0)
            {
                lblDetails.Text = "Select a menu item to view details.";
                pbPreview.Image = null;
                pbPreview.ImageLocation = null;
                btnEdit.Enabled = btnDelete.Enabled = false;
                return;
            }

            btnEdit.Enabled = btnDelete.Enabled = true;
            var item = (MenuItemEntity)lvItems.SelectedItems[0].Tag;
            lblDetails.Text = $"Price: {item.Price:C}\nSize: {item.Size ?? "N/A"}\nActive: {(item.IsActive ? "Yes" : "No")}\n\n{item.Description}";
            LoadPreviewImage(item.ImageFile);
        }

        private void LoadPreviewImage(string? imageFile)
        {
            pbPreview.Image = null;
            pbPreview.ImageLocation = null;
            if (string.IsNullOrWhiteSpace(imageFile)) return;
            var path = ResolveImagePath(imageFile);
            if (!File.Exists(path)) return;
            try
            {
                pbPreview.ImageLocation = path;
            }
            catch
            {
            }
        }

        private static string ResolveImagePath(string fileName)
        {
            if (Path.IsPathRooted(fileName)) return fileName;
            var baseDir = AppContext.BaseDirectory ?? string.Empty;
            var potential = Path.Combine(baseDir, "Resources", "img", fileName);
            return File.Exists(potential) ? potential : Path.Combine(baseDir, fileName);
        }

        private bool EnsureRepo()
        {
            if (_repository == null)
            {
                if (lblStatus != null) lblStatus.Text = "Database connection not available.";
                return false;
            }
            return true;
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            if (!EnsureRepo()) return;
            var dlg = new EditMenuItemDialog(_categories);
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ResultItem != null)
            {
                try
                {
                    var newId = _repository!.InsertMenuItem(dlg.ResultItem);
                    dlg.ResultItem.MenuItemId = newId;
                    _ = RefreshAllAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Failed to add menu item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (!EnsureRepo()) return;
            if (lvItems.SelectedItems.Count == 0) return;
            var entity = (MenuItemEntity)lvItems.SelectedItems[0].Tag;
            var clone = new MenuItemEntity
            {
                MenuItemId = entity.MenuItemId,
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Size = entity.Size,
                ImageFile = entity.ImageFile,
                IsActive = entity.IsActive
            };
            var dlg = new EditMenuItemDialog(_categories, clone);
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ResultItem != null)
            {
                try
                {
                    _repository!.UpdateMenuItem(dlg.ResultItem);
                    _ = RefreshAllAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Failed to update menu item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (!EnsureRepo()) return;
            if (lvItems.SelectedItems.Count == 0) return;
            var entity = (MenuItemEntity)lvItems.SelectedItems[0].Tag;
            if (MessageBox.Show(this, $"Delete '{entity.Name}'?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            try
            {
                _repository!.DeleteMenuItem(entity.MenuItemId);
                _ = RefreshAllAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to delete menu item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
