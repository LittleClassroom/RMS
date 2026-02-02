using RMS.Models;
using RMS.UI;
using System;
using System.Windows.Forms;
using NLog;
using System.IO;

namespace RMS
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configure NLog (loads nlog.config from app root)
            try
            {
                var cfgFile = Path.Combine(AppContext.BaseDirectory ?? string.Empty, "nlog.config");
                if (System.IO.File.Exists(cfgFile))
                {
                    // load user-provided config file
                    LogManager.LoadConfiguration(cfgFile);
                }
                else
                {
                    // create a simple file target programmatically as a fallback
                    var logsDir = Path.Combine(AppContext.BaseDirectory ?? string.Empty, "logs");
                    try { Directory.CreateDirectory(logsDir); } catch { }

                    var config = new NLog.Config.LoggingConfiguration();
                    var logfile = new NLog.Targets.FileTarget("logfile")
                    {
                        FileName = Path.Combine(logsDir, "app.${shortdate}.log"),
                        Layout = "${longdate}|${level:uppercase=true}|${logger}|${message} ${exception:format=ToString}",
                        ConcurrentWrites = true,
                        KeepFileOpen = false,
                    };
                    config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logfile);
                    LogManager.Configuration = config;
                }
            }
            catch (Exception ex)
            {
                // best-effort: show a non-blocking message but continue
                try { MessageBox.Show("Failed to initialize logger: " + ex.Message, "Logger Init", MessageBoxButtons.OK, MessageBoxIcon.Warning); } catch { }
            }

            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                logger.Info("Application started. BaseDirectory={0}", AppContext.BaseDirectory);
            }
            catch { }

            // Global exception handlers to capture otherwise silent crashes
            Application.ThreadException += (s, e) =>
            {
                logger.Error(e.Exception, "Unhandled UI thread exception");
                MessageBox.Show("An unexpected error occurred:\n" + e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                logger.Fatal(ex, "Unhandled non-UI exception");
                MessageBox.Show("A fatal error occurred. See logs for details.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            ApplicationConfiguration.Initialize();

            // Load stored credentials or prompt user
            var ok = Global.LoadFromStore();
            if (!ok)
            {
                using var dlg = new DbCredentialsForm();
                var res = dlg.ShowDialog();
                if (res != DialogResult.OK)
                {
                    // user cancelled - exit app
                    return;
                }
            }

            // Login loop - returns to login if user logs out
            while (true)
            {
                using var signIn = new SignIn();
                var loginResult = signIn.ShowDialog();

                if (loginResult != DialogResult.OK)
                {
                    // User cancelled login - exit app
                    break;
                }

                string userName = signIn.AuthenticatedUser ?? "User";
                UserRole userRole = signIn.AuthenticatedRole;

                Form mainForm = userRole == UserRole.Manager
                    ? new ManagerMainForm(userName)
                    : new StaffMainForm(userName, userRole);

                var result = mainForm.ShowDialog();
                mainForm.Dispose();

                // If closed normally (not logout), exit the app
                if (result != DialogResult.Abort)
                {
                    break;
                }
                // DialogResult.Abort means logout - loop back to login
            }
        }
    }
}