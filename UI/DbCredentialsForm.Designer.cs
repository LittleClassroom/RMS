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
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelDataSource = new System.Windows.Forms.Label();
            this.labelDatabase = new System.Windows.Forms.Label();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.tbDatabase = new System.Windows.Forms.TextBox();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.chkIntegrated = new System.Windows.Forms.CheckBox();
            this.chkEncrypt = new System.Windows.Forms.CheckBox();
            this.chkTrust = new System.Windows.Forms.CheckBox();
            this.btnSaveData = new System.Windows.Forms.Button();
            this.btnLoadData = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lbStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(12, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(153, 21);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Server Configuration";
            // 
            // labelDataSource
            // 
            this.labelDataSource.AutoSize = true;
            this.labelDataSource.Location = new System.Drawing.Point(20, 48);
            this.labelDataSource.Name = "labelDataSource";
            this.labelDataSource.Size = new System.Drawing.Size(73, 15);
            this.labelDataSource.TabIndex = 1;
            this.labelDataSource.Text = "Data Source:";
            // 
            // labelDatabase
            // 
            this.labelDatabase.AutoSize = true;
            this.labelDatabase.Location = new System.Drawing.Point(20, 90);
            this.labelDatabase.Name = "labelDatabase";
            this.labelDatabase.Size = new System.Drawing.Size(60, 15);
            this.labelDatabase.TabIndex = 2;
            this.labelDatabase.Text = "Database:";
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(20, 132);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(49, 15);
            this.labelUser.TabIndex = 3;
            this.labelUser.Text = "User ID:";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(20, 174);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(60, 15);
            this.labelPassword.TabIndex = 4;
            this.labelPassword.Text = "Password:";
            // 
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(130, 45);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(380, 23);
            this.tbServer.TabIndex = 5;
            this.tbServer.PlaceholderText = "Server (e.g. .\\SQLEXPRESS)";
            // 
            // tbDatabase
            // 
            this.tbDatabase.Location = new System.Drawing.Point(130, 87);
            this.tbDatabase.Name = "tbDatabase";
            this.tbDatabase.Size = new System.Drawing.Size(380, 23);
            this.tbDatabase.TabIndex = 6;
            this.tbDatabase.PlaceholderText = "Database";
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(130, 129);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(380, 23);
            this.tbUser.TabIndex = 7;
            this.tbUser.PlaceholderText = "User ID";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(130, 171);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(380, 23);
            this.tbPassword.TabIndex = 8;
            this.tbPassword.UseSystemPasswordChar = true;
            this.tbPassword.PlaceholderText = "Password";
            // 
            // chkIntegrated
            // 
            this.chkIntegrated.AutoSize = true;
            this.chkIntegrated.Location = new System.Drawing.Point(130, 203);
            this.chkIntegrated.Name = "chkIntegrated";
            this.chkIntegrated.Size = new System.Drawing.Size(155, 19);
            this.chkIntegrated.TabIndex = 9;
            this.chkIntegrated.Text = "Use Windows Authentication";
            this.chkIntegrated.UseVisualStyleBackColor = true;
            this.chkIntegrated.CheckedChanged += new System.EventHandler(this.chkIntegrated_CheckedChanged);
            // 
            // chkEncrypt
            // 
            this.chkEncrypt.AutoSize = true;
            this.chkEncrypt.Location = new System.Drawing.Point(130, 230);
            this.chkEncrypt.Name = "chkEncrypt";
            this.chkEncrypt.Size = new System.Drawing.Size(111, 19);
            this.chkEncrypt.TabIndex = 10;
            this.chkEncrypt.Text = "Encrypt Connection";
            this.chkEncrypt.UseVisualStyleBackColor = true;
            this.chkEncrypt.Checked = true;
            // 
            // chkTrust
            // 
            this.chkTrust.AutoSize = true;
            this.chkTrust.Location = new System.Drawing.Point(260, 230);
            this.chkTrust.Name = "chkTrust";
            this.chkTrust.Size = new System.Drawing.Size(119, 19);
            this.chkTrust.TabIndex = 11;
            this.chkTrust.Text = "Trust Server Certificate";
            this.chkTrust.UseVisualStyleBackColor = true;
            // 
            // btnSaveData
            // 
            this.btnSaveData.Location = new System.Drawing.Point(24, 270);
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(100, 30);
            this.btnSaveData.TabIndex = 12;
            this.btnSaveData.Text = "Save Data";
            this.btnSaveData.UseVisualStyleBackColor = true;
            this.btnSaveData.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoadData
            // 
            this.btnLoadData.Location = new System.Drawing.Point(140, 270);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(100, 30);
            this.btnLoadData.TabIndex = 13;
            this.btnLoadData.Text = "Load Data";
            this.btnLoadData.UseVisualStyleBackColor = true;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(260, 270);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(120, 30);
            this.btnTest.TabIndex = 14;
            this.btnTest.Text = "Test Connection";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(400, 270);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(110, 30);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(20, 315);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(45, 15);
            this.lbStatus.TabIndex = 16;
            this.lbStatus.Text = "Status:";
            // 
            // DbCredentialsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 360);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnLoadData);
            this.Controls.Add(this.btnSaveData);
            this.Controls.Add(this.chkTrust);
            this.Controls.Add(this.chkEncrypt);
            this.Controls.Add(this.chkIntegrated);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbUser);
            this.Controls.Add(this.tbDatabase);
            this.Controls.Add(this.tbServer);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.labelUser);
            this.Controls.Add(this.labelDatabase);
            this.Controls.Add(this.labelDataSource);
            this.Controls.Add(this.labelTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DbCredentialsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();
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
