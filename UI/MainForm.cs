using System;
using System.Windows.Forms;
using RMS.Utils;

namespace RMS.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            try
            {
                if (string.IsNullOrEmpty(Global.CurrentConnectionString))
                {
                    toolStripStatusLabel1.Text = "Not connected to DB";
                    toolStripStatusLabelDetails.Text = string.Empty;
                    return;
                }

                var b = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(Global.CurrentConnectionString);
                toolStripStatusLabel1.Text = "Connected to DB";
                toolStripStatusLabelDetails.Text = $"Server={b.DataSource}; DB={b.InitialCatalog}; Auth={(b.IntegratedSecurity ? "Windows" : "SQL")}; CredFile={Global.GetCredentialFilePath()}";
            }
            catch
            {
                toolStripStatusLabel1.Text = "Not connected to DB";
                toolStripStatusLabelDetails.Text = "(invalid connection)";
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dbCredentialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var dlg = new DbCredentialsForm();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                UpdateStatus();
            }
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Users management not implemented yet.", "Users", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void newOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Open new order screen (not implemented).", "New Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void viewOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Open orders view (not implemented).", "View Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void salesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Generate sales report (not implemented).", "Sales Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void kitchenReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Open kitchen report (not implemented).", "Kitchen Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
