using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RMS.Models;

namespace RMS.UI
{
    public class EditMenuItemDialog : Form
    {
        private readonly List<MenuCategory> _categories;
        private readonly MenuItemEntity _model;

        private ComboBox cbCategory = null!;
        private TextBox tbName = null!;
        private TextBox tbDescription = null!;
        private NumericUpDown numPrice = null!;
        private TextBox tbSize = null!;
        private TextBox tbImage = null!;
        private CheckBox chkActive = null!;

        public MenuItemEntity? ResultItem { get; private set; }

        public EditMenuItemDialog(IEnumerable<MenuCategory> categories, MenuItemEntity? existing = null)
        {
            _categories = categories?.ToList() ?? new List<MenuCategory>();
            _model = existing ?? new MenuItemEntity { IsActive = true };
            InitializeComponent();
            BindData();
        }

        private void InitializeComponent()
        {
            Text = _model.MenuItemId == 0 ? "Add Menu Item" : "Edit Menu Item";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(520, 420);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 7,
                Padding = new Padding(12),
                AutoSize = true
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            cbCategory = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };
            tbName = new TextBox { Dock = DockStyle.Fill };
            numPrice = new NumericUpDown { Minimum = 0, Maximum = 100000, DecimalPlaces = 2, Increment = 0.50M, Dock = DockStyle.Left, Width = 150 };
            tbSize = new TextBox { Dock = DockStyle.Fill };
            tbDescription = new TextBox { Dock = DockStyle.Fill, Multiline = true, Height = 120, ScrollBars = ScrollBars.Vertical };
            tbImage = new TextBox { Dock = DockStyle.Fill };
            chkActive = new CheckBox { Text = "Active", Checked = true };
            var btnBrowse = new Button { Text = "Browse", Width = 80 };
            btnBrowse.Click += (s, e) => BrowseImage();
            var btnClearImg = new Button { Text = "Clear", Width = 80 };
            btnClearImg.Click += (s, e) => tbImage.Text = string.Empty;

            layout.Controls.Add(new Label { Text = "Category", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
            layout.Controls.Add(cbCategory, 1, 0);
            layout.Controls.Add(new Label { Text = "Name", AutoSize = true }, 0, 1);
            layout.Controls.Add(tbName, 1, 1);
            layout.Controls.Add(new Label { Text = "Price", AutoSize = true }, 0, 2);
            layout.Controls.Add(numPrice, 1, 2);
            layout.Controls.Add(new Label { Text = "Size", AutoSize = true }, 0, 3);
            layout.Controls.Add(tbSize, 1, 3);
            layout.Controls.Add(new Label { Text = "Description", AutoSize = true }, 0, 4);
            layout.Controls.Add(tbDescription, 1, 4);

            var imgPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            imgPanel.Controls.Add(tbImage);
            imgPanel.Controls.Add(btnBrowse);
            imgPanel.Controls.Add(btnClearImg);
            layout.Controls.Add(new Label { Text = "Image", AutoSize = true }, 0, 5);
            layout.Controls.Add(imgPanel, 1, 5);

            layout.Controls.Add(chkActive, 1, 6);

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 8, 12, 8)
            };
            var btnOk = new Button { Text = "Save", DialogResult = DialogResult.OK, Width = 90 };
            var btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Width = 90 };
            btnOk.Click += (s, e) => SaveAndClose();
            buttons.Controls.Add(btnOk);
            buttons.Controls.Add(btnCancel);

            Controls.Add(layout);
            Controls.Add(buttons);
        }

        private void BindData()
        {
            cbCategory.DataSource = _categories;
            cbCategory.DisplayMember = "Name";
            cbCategory.ValueMember = "CategoryId";

            if (_model.CategoryId.HasValue)
            {
                cbCategory.SelectedValue = _model.CategoryId.Value;
            }

            tbName.Text = _model.Name;
            tbDescription.Text = _model.Description ?? string.Empty;
            numPrice.Value = _model.Price > numPrice.Maximum ? numPrice.Maximum : (_model.Price < numPrice.Minimum ? numPrice.Minimum : _model.Price);
            tbSize.Text = _model.Size ?? string.Empty;
            tbImage.Text = _model.ImageFile ?? string.Empty;
            chkActive.Checked = _model.IsActive;
        }

        private void BrowseImage()
        {
            using var ofd = new OpenFileDialog
            {
                Title = "Select menu image",
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All Files|*.*"
            };
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                tbImage.Text = ofd.FileName;
            }
        }

        private void SaveAndClose()
        {
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show(this, "Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbCategory.SelectedItem is not MenuCategory selectedCategory)
            {
                MessageBox.Show(this, "Select a category.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var entity = new MenuItemEntity
            {
                MenuItemId = _model.MenuItemId,
                CategoryId = selectedCategory.CategoryId,
                Name = tbName.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(tbDescription.Text) ? null : tbDescription.Text.Trim(),
                Price = numPrice.Value,
                Size = string.IsNullOrWhiteSpace(tbSize.Text) ? null : tbSize.Text.Trim(),
                ImageFile = PrepareImageAsset(tbImage.Text),
                IsActive = chkActive.Checked
            };

            ResultItem = entity;
            DialogResult = DialogResult.OK;
            Close();
        }

        private string? PrepareImageAsset(string? selectedPath)
        {
            if (string.IsNullOrWhiteSpace(selectedPath))
            {
                return _model.ImageFile;
            }

            if (!File.Exists(selectedPath))
            {
                return selectedPath;
            }

            var projectRoot = FindProjectRoot();
            var imagesFolder = Path.Combine(projectRoot, "Resources", "img");
            Directory.CreateDirectory(imagesFolder);
            var fileName = Path.GetFileName(selectedPath);
            var destination = Path.Combine(imagesFolder, fileName);
            try
            {
                if (!string.Equals(Path.GetFullPath(selectedPath), Path.GetFullPath(destination), StringComparison.OrdinalIgnoreCase))
                {
                    File.Copy(selectedPath, destination, true);
                }
            }
            catch
            {
                // ignore copy errors; fall back to original file
                return fileName;
            }
            return fileName;
        }

        private static string FindProjectRoot()
        {
            var dir = AppContext.BaseDirectory ?? string.Empty;
            var di = new DirectoryInfo(dir);
            while (di != null)
            {
                try
                {
                    if (di.GetFiles("*.csproj").Length > 0)
                        return di.FullName;
                }
                catch { }
                di = di.Parent;
            }
            return AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
        }
    }
}
