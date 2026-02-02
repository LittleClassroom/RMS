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

        private void InitializeComponent()
        {
            lblSubtotal = new Label();
            lblTax = new Label();
            lblTotal = new Label();
            btnConfirm = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblSubtotal
            // 
            lblSubtotal.AutoSize = true;
            lblSubtotal.Location = new Point(30, 30);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.Size = new Size(84, 15);
            lblSubtotal.TabIndex = 0;
            lblSubtotal.Text = "Subtotal: $0.00";
            // 
            // lblTax
            // 
            lblTax.AutoSize = true;
            lblTax.Location = new Point(30, 60);
            lblTax.Name = "lblTax";
            lblTax.Size = new Size(57, 15);
            lblTax.TabIndex = 1;
            lblTax.Text = "Tax: $0.00";
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotal.Location = new Point(30, 90);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(71, 15);
            lblTotal.TabIndex = 2;
            lblTotal.Text = "Total: $0.00";
            // 
            // btnConfirm
            // 
            btnConfirm.DialogResult = DialogResult.OK;
            btnConfirm.Location = new Point(30, 140);
            btnConfirm.Name = "btnConfirm";
            btnConfirm.Size = new Size(120, 30);
            btnConfirm.TabIndex = 3;
            btnConfirm.Text = "Confirm Payment";
            btnConfirm.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(170, 140);
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
            ClientSize = new Size(320, 200);
            Controls.Add(lblSubtotal);
            Controls.Add(lblTax);
            Controls.Add(lblTotal);
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
