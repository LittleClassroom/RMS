using System;
using System.Windows.Forms;
using RMS.Data.SqlServer;

namespace RMS.UI
{
    public partial class CheckoutDialog : Form
    {
        private readonly int _orderId;
        private readonly decimal _subtotal;
        private readonly decimal _tax;
        private readonly decimal _total;

        public CheckoutDialog(int orderId, decimal subtotal, decimal tax)
        {
            _orderId = orderId;
            _subtotal = subtotal;
            _tax = tax;
            _total = subtotal + tax;
            InitializeComponent();
            lblSubtotal.Text = $"Subtotal: {_subtotal:C}";
            lblTax.Text = $"Tax: {_tax:C}";
            lblTotal.Text = $"Total: {_total:C}";
        }

        private void CheckoutDialog_Load(object sender, EventArgs e)
        {

        }

        private void BtnConfirm_Click(object? sender, EventArgs e)
        {
            // perform payment recording and mark order paid
            try
            {
                var conn = RMS.Global.CurrentConnectionString;
                if (string.IsNullOrWhiteSpace(conn))
                {
                    MessageBox.Show(this, "Database connection unavailable.", "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var repo = new RmsRepository(conn);
                // payment type mapping: 0=Cash,1=Card,2=QR
                var ptype = (byte)Math.Max(0, cmbPaymentType.SelectedIndex);
                var reference = string.IsNullOrWhiteSpace(tbReference.Text) ? null : tbReference.Text.Trim();

                var paymentId = repo.CreatePayment(_orderId, null, _total, ptype, null, reference, false, null);
                repo.MarkOrderPaid(_orderId);

                MessageBox.Show(this, $"Payment recorded (id: {paymentId}). Order closed.", "Payment Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to record payment: " + ex.Message, "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
