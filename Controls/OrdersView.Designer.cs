using System.Windows.Forms;

namespace RMS.Controls
{
    partial class OrdersView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            mainLayout = new TableLayoutPanel();
            lblTitle = new Label();
            splitCont = new SplitContainer();
            menuLayout = new TableLayoutPanel();
            tableFlowLayout = new FlowLayoutPanel();
            lblSelectTable = new Label();
            cmbTables = new ComboBox();
            txtSearchMenu = new TextBox();
            lvMenu = new ListView();
            orderLayout = new TableLayoutPanel();
            lblCurrentOrder = new Label();
            dgvOrderItems = new DataGridView();
            summaryLayout = new TableLayoutPanel();
            lblSubtotalLabel = new Label();
            lblTaxLabel = new Label();
            lblTotalLabel = new Label();
            lblSubtotal = new Label();
            lblTax = new Label();
            lblTotal = new Label();
            actionButtonsLayout = new FlowLayoutPanel();
            btnSubmitOrder = new Button();
            btnCancelOrder = new Button();
            mainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitCont).BeginInit();
            splitCont.Panel1.SuspendLayout();
            splitCont.Panel2.SuspendLayout();
            splitCont.SuspendLayout();
            menuLayout.SuspendLayout();
            tableFlowLayout.SuspendLayout();
            orderLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrderItems).BeginInit();
            summaryLayout.SuspendLayout();
            actionButtonsLayout.SuspendLayout();
            SuspendLayout();
            // 
            // mainLayout
            // 
            mainLayout.ColumnCount = 1;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainLayout.Controls.Add(lblTitle, 0, 0);
            mainLayout.Controls.Add(splitCont, 0, 1);
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.Location = new Point(0, 0);
            mainLayout.Name = "mainLayout";
            mainLayout.RowCount = 2;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainLayout.Size = new Size(800, 600);
            mainLayout.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.Location = new Point(3, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(794, 40);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Create New Order";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // splitCont
            // 
            splitCont.Dock = DockStyle.Fill;
            splitCont.Location = new Point(3, 43);
            splitCont.Name = "splitCont";
            // 
            // splitCont.Panel1
            // 
            splitCont.Panel1.Controls.Add(menuLayout);
            // 
            // splitCont.Panel2
            // 
            splitCont.Panel2.Controls.Add(orderLayout);
            splitCont.Size = new Size(794, 554);
            splitCont.SplitterDistance = 350;
            splitCont.TabIndex = 1;
            // 
            // menuLayout
            // 
            menuLayout.ColumnCount = 1;
            menuLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            menuLayout.Controls.Add(tableFlowLayout, 0, 0);
            menuLayout.Controls.Add(txtSearchMenu, 0, 1);
            menuLayout.Controls.Add(lvMenu, 0, 2);
            menuLayout.Dock = DockStyle.Fill;
            menuLayout.Location = new Point(0, 0);
            menuLayout.Name = "menuLayout";
            menuLayout.RowCount = 3;
            menuLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            menuLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            menuLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            menuLayout.Size = new Size(350, 554);
            menuLayout.TabIndex = 0;
            // 
            // tableFlowLayout
            // 
            tableFlowLayout.Controls.Add(lblSelectTable);
            tableFlowLayout.Controls.Add(cmbTables);
            tableFlowLayout.Dock = DockStyle.Fill;
            tableFlowLayout.Location = new Point(3, 3);
            tableFlowLayout.Name = "tableFlowLayout";
            tableFlowLayout.Size = new Size(344, 29);
            tableFlowLayout.TabIndex = 0;
            // 
            // lblSelectTable
            // 
            lblSelectTable.AutoSize = true;
            lblSelectTable.Location = new Point(3, 5);
            lblSelectTable.Margin = new Padding(3, 5, 3, 0);
            lblSelectTable.Name = "lblSelectTable";
            lblSelectTable.Size = new Size(38, 15);
            lblSelectTable.TabIndex = 0;
            lblSelectTable.Text = "Table:";
            // 
            // cmbTables
            // 
            cmbTables.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTables.FormattingEnabled = true;
            cmbTables.Location = new Point(47, 3);
            cmbTables.Name = "cmbTables";
            cmbTables.Size = new Size(121, 23);
            cmbTables.TabIndex = 1;
            // 
            // txtSearchMenu
            // 
            txtSearchMenu.Dock = DockStyle.Fill;
            txtSearchMenu.Location = new Point(3, 38);
            txtSearchMenu.Name = "txtSearchMenu";
            txtSearchMenu.PlaceholderText = "Search menu...";
            txtSearchMenu.Size = new Size(344, 23);
            txtSearchMenu.TabIndex = 1;
            // 
            // lvMenu
            // 
            lvMenu.Dock = DockStyle.Fill;
            lvMenu.Location = new Point(3, 73);
            lvMenu.Name = "lvMenu";
            lvMenu.Size = new Size(344, 478);
            lvMenu.TabIndex = 2;
            lvMenu.UseCompatibleStateImageBehavior = false;
            // 
            // orderLayout
            // 
            orderLayout.ColumnCount = 1;
            orderLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            orderLayout.Controls.Add(lblCurrentOrder, 0, 0);
            orderLayout.Controls.Add(dgvOrderItems, 0, 1);
            orderLayout.Controls.Add(summaryLayout, 0, 2);
            orderLayout.Controls.Add(actionButtonsLayout, 0, 3);
            orderLayout.Dock = DockStyle.Fill;
            orderLayout.Location = new Point(0, 0);
            orderLayout.Name = "orderLayout";
            orderLayout.RowCount = 4;
            orderLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            orderLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            orderLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            orderLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            orderLayout.Size = new Size(440, 554);
            orderLayout.TabIndex = 0;
            // 
            // lblCurrentOrder
            // 
            lblCurrentOrder.AutoSize = true;
            lblCurrentOrder.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblCurrentOrder.Location = new Point(3, 0);
            lblCurrentOrder.Name = "lblCurrentOrder";
            lblCurrentOrder.Size = new Size(114, 21);
            lblCurrentOrder.TabIndex = 0;
            lblCurrentOrder.Text = "Current Order";
            // 
            // dgvOrderItems
            // 
            dgvOrderItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrderItems.Dock = DockStyle.Fill;
            dgvOrderItems.Location = new Point(3, 33);
            dgvOrderItems.Name = "dgvOrderItems";
            dgvOrderItems.Size = new Size(434, 398);
            dgvOrderItems.TabIndex = 1;
            // 
            // summaryLayout
            // 
            summaryLayout.ColumnCount = 2;
            summaryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            summaryLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            summaryLayout.Controls.Add(lblSubtotalLabel, 0, 0);
            summaryLayout.Controls.Add(lblTaxLabel, 0, 1);
            summaryLayout.Controls.Add(lblTotalLabel, 0, 2);
            summaryLayout.Controls.Add(lblSubtotal, 1, 0);
            summaryLayout.Controls.Add(lblTax, 1, 1);
            summaryLayout.Controls.Add(lblTotal, 1, 2);
            summaryLayout.Dock = DockStyle.Fill;
            summaryLayout.Location = new Point(3, 437);
            summaryLayout.Name = "summaryLayout";
            summaryLayout.RowCount = 3;
            summaryLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            summaryLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            summaryLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            summaryLayout.Size = new Size(434, 74);
            summaryLayout.TabIndex = 2;
            // 
            // lblSubtotalLabel
            // 
            lblSubtotalLabel.AutoSize = true;
            lblSubtotalLabel.Dock = DockStyle.Fill;
            lblSubtotalLabel.Location = new Point(3, 0);
            lblSubtotalLabel.Name = "lblSubtotalLabel";
            lblSubtotalLabel.Size = new Size(211, 24);
            lblSubtotalLabel.TabIndex = 0;
            lblSubtotalLabel.Text = "Subtotal:";
            lblSubtotalLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTaxLabel
            // 
            lblTaxLabel.AutoSize = true;
            lblTaxLabel.Dock = DockStyle.Fill;
            lblTaxLabel.Location = new Point(3, 24);
            lblTaxLabel.Name = "lblTaxLabel";
            lblTaxLabel.Size = new Size(211, 24);
            lblTaxLabel.TabIndex = 1;
            lblTaxLabel.Text = "Tax (10%):";
            lblTaxLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTotalLabel
            // 
            lblTotalLabel.AutoSize = true;
            lblTotalLabel.Dock = DockStyle.Fill;
            lblTotalLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotalLabel.Location = new Point(3, 48);
            lblTotalLabel.Name = "lblTotalLabel";
            lblTotalLabel.Size = new Size(211, 26);
            lblTotalLabel.TabIndex = 2;
            lblTotalLabel.Text = "Total:";
            lblTotalLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblSubtotal
            // 
            lblSubtotal.AutoSize = true;
            lblSubtotal.Dock = DockStyle.Fill;
            lblSubtotal.Location = new Point(220, 0);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.Size = new Size(211, 24);
            lblSubtotal.TabIndex = 3;
            lblSubtotal.Text = "$0.00";
            lblSubtotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTax
            // 
            lblTax.AutoSize = true;
            lblTax.Dock = DockStyle.Fill;
            lblTax.Location = new Point(220, 24);
            lblTax.Name = "lblTax";
            lblTax.Size = new Size(211, 24);
            lblTax.TabIndex = 4;
            lblTax.Text = "$0.00";
            lblTax.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Dock = DockStyle.Fill;
            lblTotal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotal.Location = new Point(220, 48);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(211, 26);
            lblTotal.TabIndex = 5;
            lblTotal.Text = "$0.00";
            lblTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // actionButtonsLayout
            // 
            actionButtonsLayout.Controls.Add(btnSubmitOrder);
            actionButtonsLayout.Controls.Add(btnCancelOrder);
            actionButtonsLayout.Dock = DockStyle.Fill;
            actionButtonsLayout.FlowDirection = FlowDirection.RightToLeft;
            actionButtonsLayout.Location = new Point(3, 517);
            actionButtonsLayout.Name = "actionButtonsLayout";
            actionButtonsLayout.Size = new Size(434, 34);
            actionButtonsLayout.TabIndex = 3;
            // 
            // btnSubmitOrder
            // 
            btnSubmitOrder.Location = new Point(356, 3);
            btnSubmitOrder.Name = "btnSubmitOrder";
            btnSubmitOrder.Size = new Size(75, 23);
            btnSubmitOrder.TabIndex = 0;
            btnSubmitOrder.Text = "Submit";
            btnSubmitOrder.UseVisualStyleBackColor = true;
            // 
            // btnCancelOrder
            // 
            btnCancelOrder.Location = new Point(275, 3);
            btnCancelOrder.Name = "btnCancelOrder";
            btnCancelOrder.Size = new Size(75, 23);
            btnCancelOrder.TabIndex = 1;
            btnCancelOrder.Text = "Cancel";
            btnCancelOrder.UseVisualStyleBackColor = true;
            // 
            // OrdersView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(mainLayout);
            Name = "OrdersView";
            Size = new Size(800, 600);
            mainLayout.ResumeLayout(false);
            mainLayout.PerformLayout();
            splitCont.Panel1.ResumeLayout(false);
            splitCont.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitCont).EndInit();
            splitCont.ResumeLayout(false);
            menuLayout.ResumeLayout(false);
            menuLayout.PerformLayout();
            tableFlowLayout.ResumeLayout(false);
            tableFlowLayout.PerformLayout();
            orderLayout.ResumeLayout(false);
            orderLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvOrderItems).EndInit();
            summaryLayout.ResumeLayout(false);
            summaryLayout.PerformLayout();
            actionButtonsLayout.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel mainLayout;
        private Label lblTitle;
        private SplitContainer splitCont;
        private TableLayoutPanel menuLayout;
        private FlowLayoutPanel tableFlowLayout;
        private Label lblSelectTable;
        private ComboBox cmbTables;
        private TextBox txtSearchMenu;
        private ListView lvMenu;
        private TableLayoutPanel orderLayout;
        private Label lblCurrentOrder;
        private DataGridView dgvOrderItems;
        private TableLayoutPanel summaryLayout;
        private Label lblSubtotalLabel;
        private Label lblTaxLabel;
        private Label lblTotalLabel;
        private Label lblSubtotal;
        private Label lblTax;
        private Label lblTotal;
        private FlowLayoutPanel actionButtonsLayout;
        private Button btnSubmitOrder;
        private Button btnCancelOrder;
    }
}
