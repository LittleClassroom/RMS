namespace RMS.UI
{
    partial class CheckoutDialog : Form
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblSubtotal;
        private Label lblTax;
        private Label lblTotal;
        private Button btnConfirm;
        private Button btnCancel;
        private Label lblPaymentType;
        private ComboBox cmbPaymentType;
        private Label lblReference;
        private TextBox tbReference;

        private void InitializeComponent()
        {
            lblSubtotal = new Label();
            lblTax = new Label();
            lblTotal = new Label();
            btnConfirm = new Button();
            btnCancel = new Button();
            lblPaymentType = new Label();
            cmbPaymentType = new ComboBox();
            lblReference = new Label();
            tbReference = new TextBox();
            SuspendLayout();
            // 
            // lblSubtotal
            // 
            lblSubtotal.AutoSize = true;
            lblSubtotal.Location = new Point(30, 20);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.Size = new Size(84, 15);
            lblSubtotal.TabIndex = 0;
            lblSubtotal.Text = "Subtotal: $0.00";
            // 
            // lblTax
            // 
            lblTax.AutoSize = true;
            lblTax.Location = new Point(30, 50);
            lblTax.Name = "lblTax";
            lblTax.Size = new Size(57, 15);
            lblTax.TabIndex = 1;
            lblTax.Text = "Tax: $0.00";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotal.Location = new Point(30, 80);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(71, 15);
            lblTotal.TabIndex = 2;
            lblTotal.Text = "Total: $0.00";
            // 
            // lblPaymentType
            // 
            lblPaymentType.AutoSize = true;
            lblPaymentType.Location = new Point(30, 115);
            lblPaymentType.Name = "lblPaymentType";
            lblPaymentType.Size = new Size(82, 15);
            lblPaymentType.TabIndex = 5;
            lblPaymentType.Text = "Payment Type:";
            // 
            // cmbPaymentType
            // 
            cmbPaymentType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaymentType.Items.AddRange(new object[] { "Cash", "Card", "QR/Wallet" });
            cmbPaymentType.SelectedIndex = 0;
            cmbPaymentType.Location = new Point(120, 111);
            cmbPaymentType.Name = "cmbPaymentType";
            cmbPaymentType.Size = new Size(160, 23);
            cmbPaymentType.TabIndex = 6;
            // 
            // lblReference
            // 
            lblReference.AutoSize = true;
            lblReference.Location = new Point(30, 150);
            lblReference.Name = "lblReference";
            lblReference.Size = new Size(60, 15);
            lblReference.TabIndex = 7;
            lblReference.Text = "Reference:";
            // 
            // tbReference
            // 
            tbReference.Location = new Point(120, 146);
            tbReference.Name = "tbReference";
            tbReference.Size = new Size(160, 23);
            tbReference.TabIndex = 8;
            // 
            // btnConfirm
            // 
            // DialogResult will be set programmatically after payment succeeds
            btnConfirm.DialogResult = DialogResult.None;
            btnConfirm.Location = new Point(40, 190);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(120, 30);
            btnConfirm.TabIndex = 3;
            btnConfirm.Text = "Confirm Payment";
            btnConfirm.UseVisualStyleBackColor = true;
            btnConfirm.Click += BtnConfirm_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(180, 190);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 30);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // CheckoutDialog
            // 
            AcceptButton = btnConfirm;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(320, 240);
            Controls.Add(lblSubtotal);
            Controls.Add(lblTax);
            Controls.Add(lblTotal);
            Controls.Add(lblPaymentType);
            Controls.Add(cmbPaymentType);
            Controls.Add(lblReference);
            Controls.Add(tbReference);
            Controls.Add(btnConfirm);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CheckoutDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Checkout";
            Load += CheckoutDialog_Load;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
