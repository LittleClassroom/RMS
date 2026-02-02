using System;
using System.Windows.Forms;

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
    }
}
