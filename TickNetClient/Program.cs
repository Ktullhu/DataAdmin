using System;
using System.Diagnostics;
using System.Windows.Forms;
using TickNetClient.Core;
using TickNetClient.Forms;

namespace TickNetClient
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
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            try
            {
                Application.Run(new FormMainTN());
            }
            catch (Exception ex)
            {
                RestartApplication(ex);
            }

        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            RestartApplication(e.Exception);
        }

        private static void RestartApplication(Exception ex)
        {
            // log exception somewhere, EventLog is one option
            //MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            Process.Start(Application.ExecutablePath);
            Application.Exit();
        }
    }
}
