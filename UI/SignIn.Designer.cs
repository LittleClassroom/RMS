namespace RMS.UI
{
    partial class SignIn
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignIn));
            tbUsername = new TextBox();
            panel1 = new Panel();
            panel2 = new Panel();
            textBox1 = new TextBox();
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // tbUsername
            // 
            tbUsername.BorderStyle = BorderStyle.None;
            tbUsername.Location = new Point(20, 9);
            tbUsername.Name = "tbUsername";
            tbUsername.PlaceholderText = "Username";
            tbUsername.Size = new Size(219, 20);
            tbUsername.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(tbUsername);
            panel1.Location = new Point(111, 227);
            panel1.Name = "panel1";
            panel1.Size = new Size(259, 40);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(textBox1);
            panel2.Location = new Point(111, 291);
            panel2.Name = "panel2";
            panel2.Size = new Size(259, 40);
            panel2.TabIndex = 1;
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Location = new Point(20, 9);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Password";
            textBox1.Size = new Size(219, 20);
            textBox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(176, 363);
            button1.Name = "button1";
            button1.Size = new Size(137, 39);
            button1.TabIndex = 2;
            button1.Text = "Login";
            button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Constantia", 16.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(196, 103);
            label1.Name = "label1";
            label1.Size = new Size(95, 35);
            label1.TabIndex = 6;
            label1.Text = "Login ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Constantia", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ActiveCaptionText;
            label2.Location = new Point(117, 155);
            label2.Name = "label2";
            label2.Size = new Size(251, 24);
            label2.TabIndex = 7;
            label2.Text = "Fill your information below";
            // 
            // SignIn
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Artboard_1;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(485, 600);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(panel2);
            Controls.Add(panel1);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SignIn";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SignIn";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox tbUsername;
        private Panel panel1;
        private Panel panel2;
        private TextBox textBox1;
        private Button button1;
        private Label label1;
        private Label label2;
    }
}