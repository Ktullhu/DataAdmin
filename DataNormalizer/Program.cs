using System;
using System.Diagnostics;
using System.Windows.Forms;
using DataNormalizer.Forms;

namespace DataNormalizer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
            Application.ApplicationExit += ApplicationExit;
        }

        private static void ApplicationExit(object sender, EventArgs e)
        {
          Process.GetCurrentProcess().Kill();
        }
    }
}
