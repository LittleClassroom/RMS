namespace RMS.Controls
{
    partial class OrderPanel
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.TextBox tbMenuSearch;
        private System.Windows.Forms.Label lblMenuHeader;
        private System.Windows.Forms.ListView lvMenuItems;
        private System.Windows.Forms.Label lblOrderHeader;
        private System.Windows.Forms.ListView lvOrderLines;
        private System.Windows.Forms.Panel totalsPanel;
        private System.Windows.Forms.Label lblSubtotalLabel;
        private System.Windows.Forms.Label lblSubtotalValue;
        private System.Windows.Forms.Label lblTaxLabel;
        private System.Windows.Forms.Label lblTaxValue;
        private System.Windows.Forms.Label lblGrandTotalLabel;
        private System.Windows.Forms.Label lblGrandTotalValue;
        private System.Windows.Forms.FlowLayoutPanel actionsPanel;
        private System.Windows.Forms.Button btnSubmitOrder;
        private System.Windows.Forms.Button btnCheckout;
        private System.Windows.Forms.Button btnCancelOrder;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            splitMain = new SplitContainer();
            lvMenuItems = new ListView();
            tbMenuSearch = new TextBox();
            lblMenuHeader = new Label();
            lvOrderLines = new ListView();
            totalsPanel = new Panel();
            lblGrandTotalValue = new Label();
            lblGrandTotalLabel = new Label();
            lblTaxValue = new Label();
            lblTaxLabel = new Label();
            lblSubtotalValue = new Label();
            lblSubtotalLabel = new Label();
            actionsPanel = new FlowLayoutPanel();
            btnSubmitOrder = new Button();
            btnCheckout = new Button();
            btnCancelOrder = new Button();
            lblOrderHeader = new Label();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            totalsPanel.SuspendLayout();
            actionsPanel.SuspendLayout();
            SuspendLayout();
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 0);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(lvMenuItems);
            splitMain.Panel1.Controls.Add(tbMenuSearch);
            splitMain.Panel1.Controls.Add(lblMenuHeader);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(lvOrderLines);
            splitMain.Panel2.Controls.Add(totalsPanel);
            splitMain.Panel2.Controls.Add(actionsPanel);
            splitMain.Panel2.Controls.Add(lblOrderHeader);
            splitMain.Size = new Size(1000, 650);
            splitMain.SplitterDistance = 420;
            splitMain.TabIndex = 0;
            // 
            // lvMenuItems
            // 
            lvMenuItems.Dock = DockStyle.Fill;
            lvMenuItems.FullRowSelect = true;
            lvMenuItems.GridLines = true;
            lvMenuItems.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvMenuItems.HideSelection = false;
            lvMenuItems.Location = new Point(0, 64);
            lvMenuItems.MultiSelect = false;
            lvMenuItems.Name = "lvMenuItems";
            lvMenuItems.Size = new Size(420, 586);
            lvMenuItems.TabIndex = 2;
            lvMenuItems.UseCompatibleStateImageBehavior = false;
            lvMenuItems.View = View.Details;
            // 
            // tbMenuSearch
            // 
            tbMenuSearch.Dock = DockStyle.Top;
            tbMenuSearch.Font = new Font("Segoe UI", 10F);
            tbMenuSearch.Location = new Point(0, 32);
            tbMenuSearch.Name = "tbMenuSearch";
            tbMenuSearch.PlaceholderText = "Search menu items...";
            tbMenuSearch.Size = new Size(420, 25);
            tbMenuSearch.TabIndex = 1;
            // 
            // lblMenuHeader
            // 
            lblMenuHeader.Dock = DockStyle.Top;
            lblMenuHeader.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblMenuHeader.Location = new Point(0, 0);
            lblMenuHeader.Name = "lblMenuHeader";
            lblMenuHeader.Size = new Size(420, 32);
            lblMenuHeader.TabIndex = 0;
            lblMenuHeader.Text = "Menu";
            lblMenuHeader.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lvOrderLines
            // 
            lvOrderLines.Dock = DockStyle.Fill;
            lvOrderLines.FullRowSelect = true;
            lvOrderLines.GridLines = true;
            lvOrderLines.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            lvOrderLines.HideSelection = false;
            lvOrderLines.Location = new Point(0, 40);
            lvOrderLines.MultiSelect = false;
            lvOrderLines.Name = "lvOrderLines";
            lvOrderLines.Size = new Size(576, 500);
            lvOrderLines.TabIndex = 1;
            lvOrderLines.UseCompatibleStateImageBehavior = false;
            lvOrderLines.View = View.Details;
            // 
            // totalsPanel
            // 
            totalsPanel.Controls.Add(lblGrandTotalValue);
            totalsPanel.Controls.Add(lblGrandTotalLabel);
            totalsPanel.Controls.Add(lblTaxValue);
            totalsPanel.Controls.Add(lblTaxLabel);
            totalsPanel.Controls.Add(lblSubtotalValue);
            totalsPanel.Controls.Add(lblSubtotalLabel);
            totalsPanel.Dock = DockStyle.Bottom;
            totalsPanel.Location = new Point(0, 540);
            totalsPanel.Name = "totalsPanel";
            totalsPanel.Padding = new Padding(8);
            totalsPanel.Size = new Size(576, 70);
            totalsPanel.TabIndex = 2;
            // 
            // lblGrandTotalValue
            // 
            lblGrandTotalValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblGrandTotalValue.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblGrandTotalValue.Location = new Point(401, 46);
            lblGrandTotalValue.Name = "lblGrandTotalValue";
            lblGrandTotalValue.Size = new Size(164, 20);
            lblGrandTotalValue.TabIndex = 5;
            lblGrandTotalValue.Text = "$0.00";
            lblGrandTotalValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblGrandTotalLabel
            // 
            lblGrandTotalLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblGrandTotalLabel.Location = new Point(11, 46);
            lblGrandTotalLabel.Name = "lblGrandTotalLabel";
            lblGrandTotalLabel.Size = new Size(180, 20);
            lblGrandTotalLabel.TabIndex = 4;
            lblGrandTotalLabel.Text = "Grand Total:";
            // 
            // lblTaxValue
            // 
            lblTaxValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTaxValue.Location = new Point(401, 26);
            lblTaxValue.Name = "lblTaxValue";
            lblTaxValue.Size = new Size(164, 20);
            lblTaxValue.TabIndex = 3;
            lblTaxValue.Text = "$0.00";
            lblTaxValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTaxLabel
            // 
            lblTaxLabel.Location = new Point(11, 26);
            lblTaxLabel.Name = "lblTaxLabel";
            lblTaxLabel.Size = new Size(180, 20);
            lblTaxLabel.TabIndex = 2;
            lblTaxLabel.Text = "Tax (10%):";
            // 
            // lblSubtotalValue
            // 
            lblSubtotalValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSubtotalValue.Location = new Point(401, 6);
            lblSubtotalValue.Name = "lblSubtotalValue";
            lblSubtotalValue.Size = new Size(164, 20);
            lblSubtotalValue.TabIndex = 1;
            lblSubtotalValue.Text = "$0.00";
            lblSubtotalValue.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblSubtotalLabel
            // 
            lblSubtotalLabel.Location = new Point(11, 6);
            lblSubtotalLabel.Name = "lblSubtotalLabel";
            lblSubtotalLabel.Size = new Size(180, 20);
            lblSubtotalLabel.TabIndex = 0;
            lblSubtotalLabel.Text = "Subtotal:";
            // 
            // actionsPanel
            // 
            actionsPanel.Controls.Add(btnSubmitOrder);
            actionsPanel.Controls.Add(btnCheckout);
            actionsPanel.Controls.Add(btnCancelOrder);
            actionsPanel.Dock = DockStyle.Bottom;
            actionsPanel.FlowDirection = FlowDirection.RightToLeft;
            actionsPanel.Location = new Point(0, 610);
            actionsPanel.Name = "actionsPanel";
            actionsPanel.Padding = new Padding(8);
            actionsPanel.Size = new Size(576, 40);
            actionsPanel.TabIndex = 3;
            // 
            // btnSubmitOrder
            // 
            btnSubmitOrder.AutoSize = true;
            btnSubmitOrder.Location = new Point(463, 11);
            btnSubmitOrder.Name = "btnSubmitOrder";
            btnSubmitOrder.Size = new Size(100, 23);
            btnSubmitOrder.TabIndex = 0;
            btnSubmitOrder.Text = "Submit Order";
            btnSubmitOrder.UseVisualStyleBackColor = true;
            // 
            // btnCheckout
            // 
            btnCheckout.AutoSize = true;
            btnCheckout.Location = new Point(357, 11);
            btnCheckout.Name = "btnCheckout";
            btnCheckout.Size = new Size(100, 23);
            btnCheckout.TabIndex = 2;
            btnCheckout.Text = "Checkout";
            btnCheckout.UseVisualStyleBackColor = true;
            // 
            // btnCancelOrder
            // 
            btnCancelOrder.AutoSize = true;
            btnCancelOrder.Location = new Point(251, 11);
            btnCancelOrder.Name = "btnCancelOrder";
            btnCancelOrder.Size = new Size(100, 23);
            btnCancelOrder.TabIndex = 3;
            btnCancelOrder.Text = "Clear";
            btnCancelOrder.UseVisualStyleBackColor = true;
            // 
            // lblOrderHeader
            // 
            lblOrderHeader.Dock = DockStyle.Top;
            lblOrderHeader.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblOrderHeader.Location = new Point(0, 0);
            lblOrderHeader.Name = "lblOrderHeader";
            lblOrderHeader.Padding = new Padding(0, 0, 0, 8);
            lblOrderHeader.Size = new Size(576, 40);
            lblOrderHeader.TabIndex = 0;
            lblOrderHeader.Text = "Select a table";
            lblOrderHeader.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // OrderPanel (UserControl)
            // 
            BackColor = SystemColors.Control;
            Controls.Add(splitMain);
            Name = "OrderPanel";
            Size = new Size(1000, 650);
            Load += OrderPanel_Load;
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel1.PerformLayout();
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            totalsPanel.ResumeLayout(false);
            actionsPanel.ResumeLayout(false);
            actionsPanel.PerformLayout();
            ResumeLayout(false);
        }
    }
}
