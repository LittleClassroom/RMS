using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RMS.Models;

namespace RMS.UI
{
    public class EditInventoryItemDialog : Form
    {
        private readonly List<InventoryCategory> _categories;
        private readonly List<InventorySubcategory> _subcategories;
        private readonly InventoryItem _model;

        private ComboBox cbCategory = null!;
        private ComboBox cbSubcategory = null!;
        private TextBox tbName = null!;
        private TextBox tbUnit = null!;
        private NumericUpDown numStartQty = null!;
        private NumericUpDown numReorder = null!;

        public InventoryItem? ResultItem { get; private set; }

        public EditInventoryItemDialog(IEnumerable<InventoryCategory> categories,
                                       IEnumerable<InventorySubcategory> subcategories,
                                       InventoryItem? existing = null)
        {
            _categories = categories?.ToList() ?? new List<InventoryCategory>();
            _subcategories = subcategories?.ToList() ?? new List<InventorySubcategory>();
            _model = existing != null ? Clone(existing) : new InventoryItem();
            InitializeComponent();
            BindData();
        }

        private void InitializeComponent()
        {
            Text = _model.InventoryItemId == 0 ? "Add Inventory Item" : "Edit Inventory Item";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(420, 280);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                Padding = new Padding(12),
                AutoSize = true
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            cbCategory = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };
            cbCategory.SelectedIndexChanged += (_, __) => BindSubcategories();
            cbSubcategory = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };
            tbName = new TextBox { Dock = DockStyle.Fill };
            tbUnit = new TextBox { Dock = DockStyle.Fill };
            numStartQty = new NumericUpDown { Minimum = 0, Maximum = 100000, DecimalPlaces = 3, Dock = DockStyle.Left, Width = 150 };
            numReorder = new NumericUpDown { Minimum = 0, Maximum = 100000, DecimalPlaces = 3, Dock = DockStyle.Left, Width = 150 };

            layout.Controls.Add(new Label { Text = "Category", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
            layout.Controls.Add(cbCategory, 1, 0);
            layout.Controls.Add(new Label { Text = "Subcategory", AutoSize = true }, 0, 1);
            layout.Controls.Add(cbSubcategory, 1, 1);
            layout.Controls.Add(new Label { Text = "Name", AutoSize = true }, 0, 2);
            layout.Controls.Add(tbName, 1, 2);
            layout.Controls.Add(new Label { Text = "Unit", AutoSize = true }, 0, 3);
            layout.Controls.Add(tbUnit, 1, 3);
            layout.Controls.Add(new Label { Text = "Starting Stock", AutoSize = true }, 0, 4);
            layout.Controls.Add(numStartQty, 1, 4);
            layout.Controls.Add(new Label { Text = "Reorder Level", AutoSize = true }, 0, 5);
            layout.Controls.Add(numReorder, 1, 5);

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 0, 12, 12)
            };
            var btnOk = new Button { Text = "Save", Width = 90, DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "Cancel", Width = 90, DialogResult = DialogResult.Cancel };
            btnOk.Click += (s, e) => SaveAndClose();
            buttons.Controls.Add(btnOk);
            buttons.Controls.Add(btnCancel);

            Controls.Add(layout);
            Controls.Add(buttons);
        }

        private void BindData()
        {
            var categoryChoices = new List<InventoryCategory> { new InventoryCategory { CategoryId = 0, Name = "(None)" } };
            categoryChoices.AddRange(_categories);
            cbCategory.DisplayMember = nameof(InventoryCategory.Name);
            cbCategory.ValueMember = nameof(InventoryCategory.CategoryId);
            cbCategory.DataSource = categoryChoices;

            if (_model.CategoryId.HasValue)
            {
                cbCategory.SelectedValue = _model.CategoryId.Value;
            }
            else
            {
                cbCategory.SelectedIndex = 0;
            }

            BindSubcategories();
            tbName.Text = _model.Name;
            tbUnit.Text = _model.Unit ?? string.Empty;
            numStartQty.Value = ClampValue(_model.CurrentStock, numStartQty.Minimum, numStartQty.Maximum);
            numReorder.Value = ClampValue(_model.ReorderLevel, numReorder.Minimum, numReorder.Maximum);
        }

        private void BindSubcategories()
        {
            var selectedCategoryId = GetSelectedCategoryId();
            var subChoices = new List<InventorySubcategory> { new InventorySubcategory { SubcategoryId = 0, Name = "(None)", CategoryId = selectedCategoryId ?? 0 } };
            subChoices.AddRange(_subcategories.Where(s => !selectedCategoryId.HasValue || s.CategoryId == selectedCategoryId.Value));
            cbSubcategory.DisplayMember = nameof(InventorySubcategory.Name);
            cbSubcategory.ValueMember = nameof(InventorySubcategory.SubcategoryId);
            cbSubcategory.DataSource = subChoices;

            if (_model.SubcategoryId.HasValue)
            {
                cbSubcategory.SelectedValue = _model.SubcategoryId.Value;
            }
            else
            {
                cbSubcategory.SelectedIndex = 0;
            }
        }

        private static decimal ClampValue(decimal value, decimal min, decimal max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
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

        private void SaveAndClose()
        {
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show(this, "Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var entity = new InventoryItem
            {
                InventoryItemId = _model.InventoryItemId,
                CategoryId = GetSelectedCategoryId(),
                SubcategoryId = GetSelectedSubcategoryId(),
                Name = tbName.Text.Trim(),
                Unit = string.IsNullOrWhiteSpace(tbUnit.Text) ? null : tbUnit.Text.Trim(),
                CurrentStock = numStartQty.Value,
                ReorderLevel = numReorder.Value
            };

            ResultItem = entity;
            DialogResult = DialogResult.OK;
            Close();
        }

        private static InventoryItem Clone(InventoryItem source)
        {
            return new InventoryItem
            {
                InventoryItemId = source.InventoryItemId,
                CategoryId = source.CategoryId,
                SubcategoryId = source.SubcategoryId,
                Name = source.Name,
                Unit = source.Unit,
                CurrentStock = source.CurrentStock,
                ReorderLevel = source.ReorderLevel
            };
        }
    }
}
