using BeeUi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeIV2
{
    internal static class Program
    {
        public static Process PriorProcess()
        // Returns a System.Diagnostics.Process pointing to
        // a pre-existing process with the same name as the
        // current one, if any; or null if the current process
        // is unique.
        {
            Process curr = Process.GetCurrentProcess();
            Process[] procs = Process.GetProcessesByName(curr.ProcessName);
            foreach (Process p in procs)
            {
                if ((p.Id != curr.Id) &&
                    (p.MainModule.FileName == curr.MainModule.FileName))
                {
                   
                    return p;
                }
            }
            return null;
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static string logPath = @"\Logs\ErrMain.log";
        [STAThread]
        static void Main()
        {
            // Bắt lỗi ở AppDomain
           // AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Bắt lỗi từ thread
           // Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (PriorProcess() == null)
            {
                Application.Run(new BeeUi.FormLoad());
            }
           
           
        }
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            LogError("Thread Exception", e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            LogError("Unhandled Exception", ex);
        }

        static void LogError(string title, Exception ex)
        {
            string log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {title}: {ex?.Message}\n{ex?.StackTrace}\n";
            File.AppendAllText(logPath, log);
        }
    }
}
