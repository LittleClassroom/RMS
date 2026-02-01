namespace RMS.UI
{
    partial class DbCredentialsForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            labelTitle = new Label();
            labelDataSource = new Label();
            labelDatabase = new Label();
            labelUser = new Label();
            labelPassword = new Label();
            tbServer = new TextBox();
            tbDatabase = new TextBox();
            tbUser = new TextBox();
            tbPassword = new TextBox();
            chkIntegrated = new CheckBox();
            chkEncrypt = new CheckBox();
            chkTrust = new CheckBox();
            btnSaveData = new Button();
            btnLoadData = new Button();
            btnTest = new Button();
            btnClose = new Button();
            lbStatus = new Label();
            SuspendLayout();
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            labelTitle.Location = new Point(12, 9);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(170, 21);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "Server Configuration";
            // 
            // labelDataSource
            // 
            labelDataSource.AutoSize = true;
            labelDataSource.Location = new Point(20, 48);
            labelDataSource.Name = "labelDataSource";
            labelDataSource.Size = new Size(73, 15);
            labelDataSource.TabIndex = 1;
            labelDataSource.Text = "Data Source:";
            // 
            // labelDatabase
            // 
            labelDatabase.AutoSize = true;
            labelDatabase.Location = new Point(20, 90);
            labelDatabase.Name = "labelDatabase";
            labelDatabase.Size = new Size(58, 15);
            labelDatabase.TabIndex = 2;
            labelDatabase.Text = "Database:";
            // 
            // labelUser
            // 
            labelUser.AutoSize = true;
            labelUser.Location = new Point(20, 132);
            labelUser.Name = "labelUser";
            labelUser.Size = new Size(47, 15);
            labelUser.TabIndex = 3;
            labelUser.Text = "User ID:";
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Location = new Point(20, 174);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(60, 15);
            labelPassword.TabIndex = 4;
            labelPassword.Text = "Password:";
            // 
            // tbServer
            // 
            tbServer.Location = new Point(130, 45);
            tbServer.Name = "tbServer";
            tbServer.PlaceholderText = "Server (e.g. .\\SQLEXPRESS)";
            tbServer.Size = new Size(380, 23);
            tbServer.TabIndex = 5;
            // 
            // tbDatabase
            // 
            tbDatabase.Location = new Point(130, 87);
            tbDatabase.Name = "tbDatabase";
            tbDatabase.PlaceholderText = "Database";
            tbDatabase.Size = new Size(380, 23);
            tbDatabase.TabIndex = 6;
            // 
            // tbUser
            // 
            tbUser.Location = new Point(130, 129);
            tbUser.Name = "tbUser";
            tbUser.PlaceholderText = "User ID";
            tbUser.Size = new Size(380, 23);
            tbUser.TabIndex = 7;
            // 
            // tbPassword
            // 
            tbPassword.Location = new Point(130, 171);
            tbPassword.Name = "tbPassword";
            tbPassword.PlaceholderText = "Password";
            tbPassword.Size = new Size(380, 23);
            tbPassword.TabIndex = 8;
            tbPassword.UseSystemPasswordChar = true;
            // 
            // chkIntegrated
            // 
            chkIntegrated.AutoSize = true;
            chkIntegrated.Location = new Point(130, 203);
            chkIntegrated.Name = "chkIntegrated";
            chkIntegrated.Size = new Size(179, 19);
            chkIntegrated.TabIndex = 9;
            chkIntegrated.Text = "Use Windows Authentication";
            chkIntegrated.UseVisualStyleBackColor = true;
            chkIntegrated.CheckedChanged += chkIntegrated_CheckedChanged;
            // 
            // chkEncrypt
            // 
            chkEncrypt.AutoSize = true;
            chkEncrypt.Checked = true;
            chkEncrypt.CheckState = CheckState.Checked;
            chkEncrypt.Location = new Point(130, 230);
            chkEncrypt.Name = "chkEncrypt";
            chkEncrypt.Size = new Size(131, 19);
            chkEncrypt.TabIndex = 10;
            chkEncrypt.Text = "Encrypt Connection";
            chkEncrypt.UseVisualStyleBackColor = true;
            // 
            // chkTrust
            // 
            chkTrust.AutoSize = true;
            chkTrust.Location = new Point(260, 230);
            chkTrust.Name = "chkTrust";
            chkTrust.Size = new Size(144, 19);
            chkTrust.TabIndex = 11;
            chkTrust.Text = "Trust Server Certificate";
            chkTrust.UseVisualStyleBackColor = true;
            // 
            // btnSaveData
            // 
            btnSaveData.Location = new Point(24, 270);
            btnSaveData.Name = "btnSaveData";
            btnSaveData.Size = new Size(100, 30);
            btnSaveData.TabIndex = 12;
            btnSaveData.Text = "Save Data";
            btnSaveData.UseVisualStyleBackColor = true;
            btnSaveData.Click += btnSave_Click;
            // 
            // btnLoadData
            // 
            btnLoadData.Location = new Point(140, 270);
            btnLoadData.Name = "btnLoadData";
            btnLoadData.Size = new Size(100, 30);
            btnLoadData.TabIndex = 13;
            btnLoadData.Text = "Load Data";
            btnLoadData.UseVisualStyleBackColor = true;
            btnLoadData.Click += btnLoad_Click;
            // 
            // btnTest
            // 
            btnTest.Location = new Point(260, 270);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(120, 30);
            btnTest.TabIndex = 14;
            btnTest.Text = "Test Connection";
            btnTest.UseVisualStyleBackColor = true;
            btnTest.Click += btnTest_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(400, 270);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(110, 30);
            btnClose.TabIndex = 15;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // lbStatus
            // 
            lbStatus.AutoSize = true;
            lbStatus.Location = new Point(20, 315);
            lbStatus.Name = "lbStatus";
            lbStatus.Size = new Size(42, 15);
            lbStatus.TabIndex = 16;
            lbStatus.Text = "Status:";
            // 
            // DbCredentialsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(534, 360);
            Controls.Add(lbStatus);
            Controls.Add(btnClose);
            Controls.Add(btnTest);
            Controls.Add(btnLoadData);
            Controls.Add(btnSaveData);
            Controls.Add(chkTrust);
            Controls.Add(chkEncrypt);
            Controls.Add(chkIntegrated);
            Controls.Add(tbPassword);
            Controls.Add(tbUser);
            Controls.Add(tbDatabase);
            Controls.Add(tbServer);
            Controls.Add(labelPassword);
            Controls.Add(labelUser);
            Controls.Add(labelDatabase);
            Controls.Add(labelDataSource);
            Controls.Add(labelTitle);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DbCredentialsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Configuration";
            Load += DbCredentialsForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelDataSource;
        private System.Windows.Forms.Label labelDatabase;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelPassword;
        internal System.Windows.Forms.TextBox tbServer;
        internal System.Windows.Forms.TextBox tbDatabase;
        internal System.Windows.Forms.TextBox tbUser;
        internal System.Windows.Forms.TextBox tbPassword;
        internal System.Windows.Forms.CheckBox chkIntegrated;
        private System.Windows.Forms.CheckBox chkEncrypt;
        private System.Windows.Forms.CheckBox chkTrust;
        private System.Windows.Forms.Button btnSaveData;
        private System.Windows.Forms.Button btnLoadData;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lbStatus;
    }
}
