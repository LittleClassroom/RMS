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

            Application.Run(new MainForm());
        }
    }
}