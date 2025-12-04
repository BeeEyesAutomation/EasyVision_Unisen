using BeeInterface;
using BeeUi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeIV2
{
    class NativeBootstrap
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool SetDefaultDllDirectories(uint DirectoryFlags);
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern IntPtr AddDllDirectory(string NewDirectory);
        const uint LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000;

        public static void Init()
        {
            var baseDir = AppContext.BaseDirectory;
            var pylonDir = System.IO.Path.Combine(baseDir, "pylon", "Win64");
            var ocvDir = System.IO.Path.Combine(baseDir, "opencv");

            SetDefaultDllDirectories(LOAD_LIBRARY_SEARCH_DEFAULT_DIRS);
            AddDllDirectory(pylonDir);
            AddDllDirectory(ocvDir);

            // optional: prepend PATH để các thư viện khác (OpenCvSharp, v.v.) cũng thấy
            var cur = Environment.GetEnvironmentVariable("PATH") ?? "";
            Environment.SetEnvironmentVariable("PATH", pylonDir + ";" + ocvDir + ";" + cur);
        }
    }
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
            //ThreadPool.SetMaxThreads(100, 100);
            //int cores = Environment.ProcessorCount;
            //Console.WriteLine($"Số lõi CPU: {cores}");
            //var options = new ParallelOptions { MaxDegreeOfParallelism = cores };
            //Parallel.For(0, 1000, options, i => { ... });
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GlobalIconManager.Init("logo.ico",true); // tìm trong thư mục exe
            if (PriorProcess() == null)
            { // --- Set priority process lên High ---
                Process current = Process.GetCurrentProcess();
                current.PriorityClass = ProcessPriorityClass.High;   // hoặc RealTime nhưng nên cẩn thận

                // (tuỳ chọn) tăng ưu tiên cho thread UI
                Thread.CurrentThread.Priority = ThreadPriority.Highest;

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
