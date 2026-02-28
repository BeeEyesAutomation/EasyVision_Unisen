using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

public static class DependencyScanner
{
    private const uint DONT_RESOLVE_DLL_REFERENCES = 0x00000001;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);

    [DllImport("kernel32.dll")]
    private static extern bool FreeLibrary(IntPtr hModule);

    public class ScanResult
    {
        public string File;
        public List<string> ManagedMissing = new List<string>();
        public Exception ManagedError;       // lỗi khi parse managed
        public string NativeLoadError;       // lỗi load native (Win32)
        public bool IsManaged;               // file này là .NET assembly hay không
    }

    // ✅ Check có phải .NET assembly không
    private static bool IsDotNetAssembly(string file)
    {
        try
        {
            AssemblyName.GetAssemblyName(file); // native sẽ throw
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static void ScanAndLog(string folder)
    {
        var results = new List<ScanResult>();
        var allDlls = Directory.GetFiles(folder, "*.dll");

        // Map assembly name -> file path (chỉ managed mới map được)
        var asmMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var file in allDlls)
        {
            try
            {
                var an = AssemblyName.GetAssemblyName(file);
                if (!string.IsNullOrEmpty(an.Name))
                    asmMap[an.Name] = file;
            }
            catch { }
        }

        // Resolver cho ReflectionOnlyLoadFrom khi DLL tham chiếu DLL khác trong folder
        AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += (s, e) =>
        {
            var name = new AssemblyName(e.Name).Name;
            if (name != null && asmMap.TryGetValue(name, out var path))
                return Assembly.ReflectionOnlyLoadFrom(path);

            return null; // không resolve được
        };

        foreach (var dll in allDlls)
        {
            var result = new ScanResult { File = dll };

            // =========================
            // 1) ✅ Check Managed (.NET) - chỉ làm nếu file là .NET assembly
            // =========================
            result.IsManaged = IsDotNetAssembly(dll);
            if (result.IsManaged)
            {
                try
                {
                    var asm = Assembly.ReflectionOnlyLoadFrom(dll);
                    foreach (var refAsm in asm.GetReferencedAssemblies())
                    {
                        // thiếu trong folder (rule đơn giản)
                        if (!string.IsNullOrEmpty(refAsm.Name) && !asmMap.ContainsKey(refAsm.Name))
                            result.ManagedMissing.Add(refAsm.Name);
                    }
                }
                catch (Exception ex)
                {
                    result.ManagedError = ex;
                }
            }

            // =========================
            // 2) ✅ Check Native load (C/C++ DLL + cả mixed-mode)
            // =========================
            IntPtr h = LoadLibraryEx(dll, IntPtr.Zero, 0); // 0 = load thật, resolve dependency
            if (h == IntPtr.Zero)
            {
                int err = Marshal.GetLastWin32Error(); // 126 thường là thiếu dep
                result.NativeLoadError = $"Win32 Error: {err}";
            }
            else
            {
                FreeLibrary(h);
            }
          

            results.Add(result);
        }

        WriteLog(folder, results);
    }
    private static void WriteLog(string folder, List<ScanResult> results)
    {
        var logPath = Path.Combine(folder, "DependencyLog.txt");
        var sb = new StringBuilder();

        sb.AppendLine("========== DEPENDENCY SCAN ==========");
        sb.AppendLine("Time: " + DateTime.Now);
        sb.AppendLine("Folder: " + folder);
        sb.AppendLine();

        foreach (var r in results)
        {
            sb.AppendLine("=====================================");
            sb.AppendLine("DLL: " + Path.GetFileName(r.File));
            sb.AppendLine("Type: " + (r.IsManaged ? ".NET (Managed/Mixed)" : "Native (Unmanaged)"));

            if (r.IsManaged)
            {
                if (r.ManagedError != null)
                    sb.AppendLine("Managed Error: " + r.ManagedError.Message);

                if (r.ManagedMissing.Count > 0)
                {
                    sb.AppendLine("Missing Managed References:");
                    foreach (var m in r.ManagedMissing)
                        sb.AppendLine("  - " + m);
                }
                else
                {
                    sb.AppendLine("Managed: OK");
                }
            }

            if (r.NativeLoadError != null)
                sb.AppendLine("Native Load Failed: " + r.NativeLoadError);
            else
                sb.AppendLine("Native Load: OK");

            sb.AppendLine();
        }

        File.WriteAllText(logPath, sb.ToString(), Encoding.UTF8);
    }
    
}