using System;
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
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMainTN());


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                ErrorMonitor.AddError(new ErrorInfo
                                          {
                                              AdditionalInformation = "ApplicationCrashed",
                                              InvokeTime = DateTime.Now,
                                              MethodName = "Program-Main",
                                              ErrorText = ex.Message
                                          });
                Application.Exit();
            }
        }
    }
}
