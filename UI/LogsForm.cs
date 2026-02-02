using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RMS.UI
{
    public class LogsForm : Form
    {
        private readonly ListBox lbFiles = new ListBox();
        private readonly TextBox tbContent = new TextBox();
        private readonly Button btnRefresh = new Button();
        private readonly Button btnOpenFolder = new Button();
        private readonly string _logsDir;

        public LogsForm()
        {
            Text = "Application Logs";
            Width = 1000;
            Height = 600;
            StartPosition = FormStartPosition.CenterParent;

            _logsDir = Path.Combine(AppContext.BaseDirectory ?? string.Empty, "logs");

            lbFiles.Dock = DockStyle.Left;
            lbFiles.Width = 300;
            lbFiles.SelectedIndexChanged += (s, e) => LoadSelectedFile();

            tbContent.Dock = DockStyle.Fill;
            tbContent.Multiline = true;
            tbContent.ScrollBars = ScrollBars.Both;
            tbContent.ReadOnly = true;
            tbContent.Font = new System.Drawing.Font("Consolas", 9F);

            var topPanel = new Panel { Dock = DockStyle.Top, Height = 36 };
            btnRefresh.Text = "Refresh";
            btnRefresh.AutoSize = true;
            btnRefresh.Click += (s, e) => PopulateFileList();
            btnRefresh.Dock = DockStyle.Left;

            btnOpenFolder.Text = "Open Folder";
            btnOpenFolder.AutoSize = true;
            btnOpenFolder.Click += (s, e) => OpenLogsFolder();
            btnOpenFolder.Dock = DockStyle.Left;

            topPanel.Controls.Add(btnOpenFolder);
            topPanel.Controls.Add(btnRefresh);

            Controls.Add(tbContent);
            Controls.Add(lbFiles);
            Controls.Add(topPanel);

            Load += (s, e) => PopulateFileList();
        }

        private void PopulateFileList()
        {
            lbFiles.Items.Clear();
            if (!Directory.Exists(_logsDir))
            {
                tbContent.Text = "Logs folder not found: " + _logsDir;
                return;
            }

            try
            {
                var files = Directory.GetFiles(_logsDir, "*.log").OrderByDescending(f => File.GetLastWriteTimeUtc(f)).ToList();
                foreach (var f in files)
                {
                    lbFiles.Items.Add(Path.GetFileName(f));
                }

                if (lbFiles.Items.Count > 0)
                {
                    lbFiles.SelectedIndex = 0;
                }
                else
                {
                    tbContent.Text = "No log files found in " + _logsDir;
                }
            }
            catch (Exception ex)
            {
                tbContent.Text = "Failed to enumerate log files: " + ex.Message;
            }
        }

        private void LoadSelectedFile()
        {
            if (lbFiles.SelectedItem == null) return;
            var fileName = lbFiles.SelectedItem.ToString();
            var path = Path.Combine(_logsDir, fileName ?? string.Empty);
            try
            {
                tbContent.Text = File.ReadAllText(path);
                tbContent.SelectionStart = 0;
                tbContent.SelectionLength = 0;
            }
            catch (Exception ex)
            {
                tbContent.Text = "Failed to read file: " + ex.Message;
            }
        }

        private void OpenLogsFolder()
        {
            try
            {
                if (!Directory.Exists(_logsDir)) Directory.CreateDirectory(_logsDir);
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = _logsDir,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to open folder: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
