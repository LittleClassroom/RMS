using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using RMS.Models;

namespace RMS.UI
{
    public class EditTableDialog : Form
    {
        private readonly TableInfo? _original;
        private TextBox tbCode = null!;
        private TextBox tbLocation = null!;
        private NumericUpDown nudCapacity = null!;
        private ComboBox cbStatus = null!;
        private TextBox tbNotes = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;

        public TableInfo Result { get; private set; }

        public EditTableDialog(TableInfo? table = null)
        {
            _original = table;
            Result = table != null ? Clone(table) : new TableInfo();
            InitializeComponent();
            PopulateFields();
        }

        private static TableInfo Clone(TableInfo source)
        {
            return new TableInfo
            {
                TableId = source.TableId,
                Code = source.Code,
                Location = source.Location,
                Capacity = source.Capacity,
                Status = source.Status,
                Notes = source.Notes,
                SeatedCount = source.SeatedCount
            };
        }

        private void InitializeComponent()
        {
            Text = _original == null ? "Add Table" : "Edit Table";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(360, 280);
            MaximizeBox = false;
            MinimizeBox = false;
            AutoScaleMode = AutoScaleMode.Font;

            var lblCode = new Label { Text = "Code", Left = 12, Top = 16, AutoSize = true };
            tbCode = new TextBox { Left = 120, Top = 12, Width = 200 };

            var lblLocation = new Label { Text = "Location", Left = 12, Top = 52, AutoSize = true };
            tbLocation = new TextBox { Left = 120, Top = 48, Width = 200 };

            var lblCapacity = new Label { Text = "Capacity", Left = 12, Top = 88, AutoSize = true };
            nudCapacity = new NumericUpDown { Left = 120, Top = 84, Width = 120, Minimum = 1, Maximum = 50, Value = 4 };

            var lblStatus = new Label { Text = "Status", Left = 12, Top = 124, AutoSize = true };
            cbStatus = new ComboBox { Left = 120, Top = 120, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cbStatus.Items.AddRange(Enum.GetNames(typeof(TableStatus)));

            var lblNotes = new Label { Text = "Notes", Left = 12, Top = 160, AutoSize = true };
            tbNotes = new TextBox { Left = 120, Top = 156, Width = 200, Height = 60, Multiline = true };

            btnSave = new Button { Text = "Save", Left = 120, Top = 228, Width = 90 };
            btnCancel = new Button { Text = "Cancel", Left = 230, Top = 228, Width = 90 };

            btnSave.Click += (_, __) => Save();
            btnCancel.Click += (_, __) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[]
            {
                lblCode, tbCode,
                lblLocation, tbLocation,
                lblCapacity, nudCapacity,
                lblStatus, cbStatus,
                lblNotes, tbNotes,
                btnSave, btnCancel
            });
        }

        private void PopulateFields()
        {
            tbCode.Text = Result.Code;
            tbLocation.Text = Result.Location ?? string.Empty;
            nudCapacity.Value = Result.Capacity > 0 ? Result.Capacity : 2;
            cbStatus.SelectedIndex = cbStatus.Items.Cast<string>().ToList().IndexOf(Result.Status.ToString());
            if (cbStatus.SelectedIndex < 0)
            {
                cbStatus.SelectedIndex = 0;
            }
            tbNotes.Text = Result.Notes ?? string.Empty;
        }

        private void Save()
        {
            var code = tbCode.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show(this, "Code is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbStatus.SelectedIndex < 0)
            {
                MessageBox.Show(this, "Select a status.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Result.Code = code;
            Result.Location = tbLocation.Text?.Trim();
            Result.Capacity = (int)nudCapacity.Value;
            Result.Status = Enum.Parse<TableStatus>(cbStatus.SelectedItem!.ToString()!);
            Result.Notes = string.IsNullOrWhiteSpace(tbNotes.Text) ? null : tbNotes.Text.Trim();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
