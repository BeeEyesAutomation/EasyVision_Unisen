using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface
{
    public static class AppRestart
    {
        public static void Now(params string[] extraArgs)
        {
            // exe hiện tại (OK cho WinForms/.NET Framework)
            string exe = Application.ExecutablePath;

            // ghép lại args hiện có + extra (nếu cần)
            var args = string.Join(" ",
                Environment.GetCommandLineArgs().Skip(1).Concat(extraArgs).Select(Quote));

            var psi = new ProcessStartInfo(exe, args)
            {
                UseShellExecute = true,
                WorkingDirectory = AppContext.BaseDirectory
            };
            Process.Start(psi);

            // nếu có gì cần lưu, gọi trước khi thoát
            // lp?.SaveNow();

            Application.Exit();     // đóng message loop
            Environment.Exit(0);    // chốt hạ
        }
        public static void Delayed(int delayMs = 1000, params string[] extraArgs)
        {
            string exe = Application.ExecutablePath;
            var argsStr = string.Join(" ",
                Environment.GetCommandLineArgs().Skip(1).Concat(extraArgs).Select(Quote));

            int sec = Math.Max(1, (int)Math.Ceiling(delayMs / 1000.0));
            string cmd = $"/C timeout /T {sec} /NOBREAK >nul & start \"\" \"{exe}\" {argsStr}";

            var psi = new ProcessStartInfo("cmd.exe", cmd)
            {
                UseShellExecute = false,                // <-- tắt shell
                CreateNoWindow = true,                  // <-- không mở cửa sổ
                WorkingDirectory = AppContext.BaseDirectory
            };
            Process.Start(psi);

            Application.Exit();
            Environment.Exit(0);
        }

      
        private static string Quote(string s)
        {
            if (string.IsNullOrEmpty(s)) return "\"\"";
            return s.Any(ch => char.IsWhiteSpace(ch) || ch == '"')
                ? "\"" + s.Replace("\"", "\"\"") + "\""
                : s;
        }
    }
}
