using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DataAdmin.Forms;

namespace DataAdmin
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{8F6F0AC5-B9A1-45fd-A8CF-72F04E6BDE8F}");

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWndm, int nCmdShow);

        [STAThread]
        private static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
               
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }
            else
            {
                var current = Process.GetCurrentProcess();
                foreach (var process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        SetForegroundWindow(process.MainWindowHandle);
                        ShowWindow(process.MainWindowHandle, 5);
                        break;
                    }
                }
            }
        }

       
    }
}
