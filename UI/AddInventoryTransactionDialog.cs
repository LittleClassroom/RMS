using System;
using System.Windows.Forms;

namespace RMS.UI
{
    public class AddInventoryTransactionDialog : Form
    {
        private NumericUpDown numQty;
        private TextBox tbReference;
        private TextBox tbSourceType;
        private ComboBox cbType;
        private Button btnOk;
        private Button btnCancel;
        public decimal Quantity { get; private set; }
        public string? Reference { get; private set; }
        public string? SourceType { get; private set; }
        public byte TransactionType { get; private set; }

        public AddInventoryTransactionDialog(string itemName, byte defaultType = 2, bool allowTypeSelection = false)
        {
            Text = $"Inventory Transaction - {itemName}";
            ClientSize = new System.Drawing.Size(420, 200);
            StartPosition = FormStartPosition.CenterParent;

            var lblType = new Label { Text = "Type:", Location = new System.Drawing.Point(12, 16), AutoSize = true };
            cbType = new ComboBox { Location = new System.Drawing.Point(100, 12), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList, Enabled = allowTypeSelection };
            cbType.Items.Add(new TransactionTypeItem("Receipt (Add)", 2));
            cbType.Items.Add(new TransactionTypeItem("Usage (Remove)", 1));
            cbType.Items.Add(new TransactionTypeItem("Adjustment", 0));
            cbType.SelectedIndex = defaultType switch
            {
                1 => 1,
                0 => 2,
                _ => 0
            };

            var lblQty = new Label { Text = "Quantity:", Location = new System.Drawing.Point(12, 48), AutoSize = true };
            numQty = new NumericUpDown { Location = new System.Drawing.Point(100, 44), DecimalPlaces = 3, Minimum = 0, Maximum = 100000, Width = 120 };
            var lblRef = new Label { Text = "Reference:", Location = new System.Drawing.Point(12, 80), AutoSize = true };
            tbReference = new TextBox { Location = new System.Drawing.Point(100, 76), Width = 280 };
            var lblSource = new Label { Text = "Source:", Location = new System.Drawing.Point(12, 112), AutoSize = true };
            tbSourceType = new TextBox { Location = new System.Drawing.Point(100, 108), Width = 280 };

            btnOk = new Button { Text = "OK", Location = new System.Drawing.Point(220, 150), DialogResult = DialogResult.OK };
            btnCancel = new Button { Text = "Cancel", Location = new System.Drawing.Point(308, 150), DialogResult = DialogResult.Cancel };

            Controls.Add(lblType);
            Controls.Add(cbType);
            Controls.Add(lblQty);
            Controls.Add(numQty);
            Controls.Add(lblRef);
            Controls.Add(tbReference);
            Controls.Add(lblSource);
            Controls.Add(tbSourceType);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);

            btnOk.Click += (s, e) =>
            {
                if (numQty.Value <= 0)
                {
                    MessageBox.Show(this, "Quantity must be greater than zero.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.None;
                    return;
                }

                Quantity = numQty.Value;
                Reference = tbReference.Text?.Trim();
                SourceType = tbSourceType.Text?.Trim();
                TransactionType = (cbType.SelectedItem as TransactionTypeItem)?.Type ?? defaultType;
                Close();
            };
            TransactionType = defaultType;
        }

        private sealed class TransactionTypeItem
        {
            public TransactionTypeItem(string name, byte type)
            {
                Name = name;
                Type = type;
            }

            public string Name { get; }
            public byte Type { get; }
            public override string ToString() => Name;
        }
    }
}
