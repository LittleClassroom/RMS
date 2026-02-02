namespace RMS.Controls
{
    partial class ManageTablesView
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel layout;
        private System.Windows.Forms.FlowLayoutPanel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.ComboBox cbStatusFilter;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvTables;
        private System.Windows.Forms.FlowLayoutPanel actionsPanel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ComboBox cbStatusAction;
        private System.Windows.Forms.Button btnApplyStatus;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCapacity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNotes;

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
            components = new System.ComponentModel.Container();
            layout = new System.Windows.Forms.TableLayoutPanel();
            headerPanel = new System.Windows.Forms.FlowLayoutPanel();
            lblTitle = new System.Windows.Forms.Label();
            tbSearch = new System.Windows.Forms.TextBox();
            cbStatusFilter = new System.Windows.Forms.ComboBox();
            btnRefresh = new System.Windows.Forms.Button();
            dgvTables = new System.Windows.Forms.DataGridView();
            colCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCapacity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            actionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            btnAdd = new System.Windows.Forms.Button();
            btnEdit = new System.Windows.Forms.Button();
            btnDelete = new System.Windows.Forms.Button();
            cbStatusAction = new System.Windows.Forms.ComboBox();
            btnApplyStatus = new System.Windows.Forms.Button();
            lblSummary = new System.Windows.Forms.Label();
            layout.SuspendLayout();
            headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTables).BeginInit();
            actionsPanel.SuspendLayout();
            SuspendLayout();
            // 
            // layout
            // 
            layout.ColumnCount = 1;
            layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            layout.Controls.Add(headerPanel, 0, 0);
            layout.Controls.Add(dgvTables, 0, 1);
            layout.Controls.Add(actionsPanel, 0, 2);
            layout.Controls.Add(lblSummary, 0, 3);
            layout.Dock = System.Windows.Forms.DockStyle.Fill;
            layout.Location = new System.Drawing.Point(0, 0);
            layout.Name = "layout";
            layout.RowCount = 4;
            layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            layout.Size = new System.Drawing.Size(900, 600);
            layout.TabIndex = 0;
            // 
            // headerPanel
            // 
            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(tbSearch);
            headerPanel.Controls.Add(cbStatusFilter);
            headerPanel.Controls.Add(btnRefresh);
            headerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            headerPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            headerPanel.Location = new System.Drawing.Point(0, 0);
            headerPanel.Margin = new System.Windows.Forms.Padding(0);
            headerPanel.Name = "headerPanel";
            headerPanel.Padding = new System.Windows.Forms.Padding(12, 12, 12, 0);
            headerPanel.Size = new System.Drawing.Size(900, 56);
            headerPanel.TabIndex = 0;
            headerPanel.WrapContents = false;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblTitle.Location = new System.Drawing.Point(12, 12);
            lblTitle.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new System.Drawing.Size(168, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Table Management";
            // 
            // tbSearch
            // 
            tbSearch.Location = new System.Drawing.Point(196, 12);
            tbSearch.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            tbSearch.Name = "tbSearch";
            tbSearch.Size = new System.Drawing.Size(220, 23);
            tbSearch.TabIndex = 1;
            // 
            // cbStatusFilter
            // 
            cbStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbStatusFilter.FormattingEnabled = true;
            cbStatusFilter.Location = new System.Drawing.Point(424, 12);
            cbStatusFilter.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            cbStatusFilter.Name = "cbStatusFilter";
            cbStatusFilter.Size = new System.Drawing.Size(180, 23);
            cbStatusFilter.TabIndex = 2;
            // 
            // btnRefresh
            // 
            btnRefresh.AutoSize = true;
            btnRefresh.Location = new System.Drawing.Point(612, 12);
            btnRefresh.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(75, 27);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            // 
            // dgvTables
            // 
            dgvTables.AllowUserToAddRows = false;
            dgvTables.AllowUserToDeleteRows = false;
            dgvTables.AllowUserToResizeRows = false;
            dgvTables.BackgroundColor = System.Drawing.Color.White;
            dgvTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colCode, colLocation, colCapacity, colStatus, colNotes });
            dgvTables.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvTables.Location = new System.Drawing.Point(12, 68);
            dgvTables.Margin = new System.Windows.Forms.Padding(12, 12, 12, 0);
            dgvTables.MultiSelect = false;
            dgvTables.Name = "dgvTables";
            dgvTables.ReadOnly = true;
            dgvTables.RowHeadersVisible = false;
            dgvTables.RowTemplate.Height = 28;
            dgvTables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvTables.Size = new System.Drawing.Size(876, 458);
            dgvTables.TabIndex = 1;
            // 
            // colCode
            // 
            colCode.DataPropertyName = "Code";
            colCode.HeaderText = "Code";
            colCode.Name = "colCode";
            colCode.ReadOnly = true;
            colCode.Width = 120;
            // 
            // colLocation
            // 
            colLocation.DataPropertyName = "Location";
            colLocation.HeaderText = "Location";
            colLocation.Name = "colLocation";
            colLocation.ReadOnly = true;
            colLocation.Width = 160;
            // 
            // colCapacity
            // 
            colCapacity.DataPropertyName = "Capacity";
            colCapacity.HeaderText = "Capacity";
            colCapacity.Name = "colCapacity";
            colCapacity.ReadOnly = true;
            colCapacity.Width = 90;
            // 
            // colStatus
            // 
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 150;
            // 
            // colNotes
            // 
            colNotes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colNotes.DataPropertyName = "Notes";
            colNotes.HeaderText = "Notes";
            colNotes.Name = "colNotes";
            colNotes.ReadOnly = true;
            // 
            // actionsPanel
            // 
            actionsPanel.Controls.Add(btnAdd);
            actionsPanel.Controls.Add(btnEdit);
            actionsPanel.Controls.Add(btnDelete);
            actionsPanel.Controls.Add(cbStatusAction);
            actionsPanel.Controls.Add(btnApplyStatus);
            actionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            actionsPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            actionsPanel.Location = new System.Drawing.Point(12, 539);
            actionsPanel.Margin = new System.Windows.Forms.Padding(12, 13, 12, 0);
            actionsPanel.Name = "actionsPanel";
            actionsPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            actionsPanel.Size = new System.Drawing.Size(876, 48);
            actionsPanel.TabIndex = 2;
            // 
            // btnAdd
            // 
            btnAdd.AutoSize = true;
            btnAdd.Location = new System.Drawing.Point(0, 0);
            btnAdd.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(90, 32);
            btnAdd.TabIndex = 0;
            btnAdd.Text = "Add Table";
            btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            btnEdit.AutoSize = true;
            btnEdit.Location = new System.Drawing.Point(98, 0);
            btnEdit.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(90, 32);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            btnDelete.AutoSize = true;
            btnDelete.Location = new System.Drawing.Point(196, 0);
            btnDelete.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(90, 32);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            // 
            // cbStatusAction
            // 
            cbStatusAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbStatusAction.FormattingEnabled = true;
            cbStatusAction.Location = new System.Drawing.Point(298, 4);
            cbStatusAction.Margin = new System.Windows.Forms.Padding(0, 4, 8, 0);
            cbStatusAction.Name = "cbStatusAction";
            cbStatusAction.Size = new System.Drawing.Size(170, 23);
            cbStatusAction.TabIndex = 3;
            // 
            // btnApplyStatus
            // 
            btnApplyStatus.AutoSize = true;
            btnApplyStatus.Location = new System.Drawing.Point(476, 0);
            btnApplyStatus.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            btnApplyStatus.Name = "btnApplyStatus";
            btnApplyStatus.Size = new System.Drawing.Size(108, 32);
            btnApplyStatus.TabIndex = 4;
            btnApplyStatus.Text = "Apply Status";
            btnApplyStatus.UseVisualStyleBackColor = true;
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSummary.Location = new System.Drawing.Point(12, 587);
            lblSummary.Margin = new System.Windows.Forms.Padding(12, 0, 12, 0);
            lblSummary.Name = "lblSummary";
            lblSummary.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            lblSummary.Size = new System.Drawing.Size(876, 13);
            lblSummary.TabIndex = 3;
            lblSummary.Text = "0 tables";
            // 
            // ManageTablesView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(layout);
            Name = "ManageTablesView";
            Size = new System.Drawing.Size(900, 600);
            layout.ResumeLayout(false);
            layout.PerformLayout();
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTables).EndInit();
            actionsPanel.ResumeLayout(false);
            actionsPanel.PerformLayout();
            ResumeLayout(false);
        }
    }
}
