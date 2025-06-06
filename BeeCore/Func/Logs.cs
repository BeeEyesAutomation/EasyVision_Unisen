using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore.Func
{
   public  class Logs
    {
        private static readonly object _logLock = new object();
        String logPath = "Logs/LogResult"+DateTime.Now.ToString("yyyy_MM_dd")+".txt";
        void ThreadSafeLog(string message)
        {
            lock (_logLock)
            {
                File.AppendAllText(logPath, $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            }
        }
    }
}
