using RMS.Models;
using RMS.UI;
using System;
using System.Windows.Forms;

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