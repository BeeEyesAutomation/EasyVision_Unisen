using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
namespace BeeGlobal
{


  

    public static class Batch
    {
        public static void CleanKeepFiles(string reportDir, IEnumerable<string> keepFileNames, bool dryRun = true)
        {
            if (string.IsNullOrWhiteSpace(reportDir))
                throw new ArgumentException("reportDir is empty");
            if (!Directory.Exists(reportDir))
                throw new DirectoryNotFoundException(reportDir);
            if (keepFileNames == null)
                throw new ArgumentNullException("keepFileNames");

            var keep = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var k in keepFileNames)
                if (!string.IsNullOrWhiteSpace(k)) keep.Add(k.Trim());

            // 1) Xóa tất cả folder con
            foreach (var dir in Directory.GetDirectories(reportDir, "*", SearchOption.TopDirectoryOnly))
            {
                if (dryRun) Console.WriteLine("[DRY] Delete DIR: " + dir);
                else
                {
                    try { Directory.Delete(dir, true); }
                    catch { ForceDeleteDirectory(dir); }
                }
            }

            // 2) Xóa tất cả file trừ keep list
            foreach (var file in Directory.GetFiles(reportDir, "*", SearchOption.TopDirectoryOnly))
            {
                string name = Path.GetFileName(file);
                if (keep.Contains(name)) continue;

                if (dryRun) Console.WriteLine("[DRY] Delete FILE: " + file);
                else
                {
                    try
                    {
                        var fi = new FileInfo(file);
                        if (fi.IsReadOnly) fi.IsReadOnly = false;
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Cannot delete: " + file + " | " + ex.Message);
                    }
                }
            }

            Console.WriteLine(dryRun
                ? "DRY RUN done (no changes). Set dryRun=false to execute."
                : "Clean done. Kept: " + string.Join(", ", keep));
        }

        private static void ForceDeleteDirectory(string dir)
        {
            foreach (var file in Directory.GetFiles(dir, "*", SearchOption.AllDirectories))
            {
                try
                {
                    var fi = new FileInfo(file);
                    if (fi.IsReadOnly) fi.IsReadOnly = false;
                }
                catch { }
            }
            Directory.Delete(dir, true);
        }
        public static string CopyAndRename(
        string sourceFolderPath,
        string newFolderName,bool IsChangeName=true)
        {
            if (string.IsNullOrWhiteSpace(sourceFolderPath))
                throw new ArgumentException("sourceFolderPath is empty");

            if (!Directory.Exists(sourceFolderPath))
                throw new DirectoryNotFoundException(sourceFolderPath);

            if (string.IsNullOrWhiteSpace(newFolderName))
                throw new ArgumentException("newFolderName is empty");

            string parent = Path.GetDirectoryName(sourceFolderPath);
            if (string.IsNullOrEmpty(parent))
                throw new Exception("Cannot copy from drive root.");

            string targetFolderPath = Path.Combine(parent, newFolderName);
            if (!IsChangeName)
                targetFolderPath = newFolderName;
                // Nếu trùng folder → thêm hậu tố
                if (Directory.Exists(targetFolderPath))
            {
                int i = 1;
                string candidate;
                do
                {
                    candidate = Path.Combine(parent, newFolderName + "_" + i);
                    i++;
                } while (Directory.Exists(candidate));

                targetFolderPath = candidate;
                newFolderName = Path.GetFileName(candidate);
            }

            // 1️⃣ Copy toàn bộ folder
            CopyDirectoryRecursive(sourceFolderPath, targetFolderPath);

            // 2️⃣ Rename toàn bộ file trong folder mới
            foreach (var file in Directory.GetFiles(targetFolderPath, "*", SearchOption.TopDirectoryOnly))
            {
                string dir = Path.GetDirectoryName(file);
                if (string.IsNullOrEmpty(dir)) continue;

                string ext = Path.GetExtension(file);
                string newFile = Path.Combine(dir, newFolderName + ext);
                if (!IsChangeName)
                    newFile = file;
                if (string.Equals(file, newFile, StringComparison.OrdinalIgnoreCase))
                    continue;

                File.Move(file, newFile);
            }

            return targetFolderPath;
        }

        // ===== Helper: copy folder đệ quy =====
        private static void CopyDirectoryRecursive(string src, string dst)
        {
            Directory.CreateDirectory(dst);

            foreach (var file in Directory.GetFiles(src))
            {
                string name = Path.GetFileName(file);
                string target = Path.Combine(dst, name);
                File.Copy(file, target, true);
            }

            foreach (var dir in Directory.GetDirectories(src))
            {
                string name = Path.GetFileName(dir);
                string target = Path.Combine(dst, name);
                CopyDirectoryRecursive(dir, target);
            }
        }
        /// <summary>
        /// Rename folder gốc sang newFolderName,
        /// sau đó rename tất cả file trong folder gốc (TopDirectoryOnly)
        /// thành: newFolderName + extension.
        /// </summary>
        public static void RenameRootFolderAndFiles(string rootFolderPath, string newFolderName)
        {
     
            if (!Directory.Exists(rootFolderPath))
                throw new DirectoryNotFoundException(rootFolderPath);

            string parent = Path.GetDirectoryName(rootFolderPath);
            if (string.IsNullOrEmpty(parent))
                throw new Exception("Cannot rename drive root.");

            // 1️⃣ Rename folder gốc
            string newRootPath = Path.Combine(parent, newFolderName);
            if (!string.Equals(rootFolderPath, newRootPath, StringComparison.OrdinalIgnoreCase))
            {
                Directory.Move(rootFolderPath, newRootPath);
            }

            // 2️⃣ Rename file: đổi TÊN GỐC, giữ extension
            foreach (var file in Directory.GetFiles(newRootPath))
            {
                string dir = Path.GetDirectoryName(file);
                string ext = Path.GetExtension(file);

                string newFile = Path.Combine(dir, newFolderName + ext);

                if (string.Equals(file, newFile, StringComparison.OrdinalIgnoreCase))
                    continue;

                File.Move(file, newFile);
            }
        
        }

        private static string RenameRootFolder(string rootFolderPath, string newFolderName)
        {
            string parent = Path.GetDirectoryName(rootFolderPath);
            if (string.IsNullOrEmpty(parent))
                throw new Exception("Cannot determine parent folder (maybe drive root like D:\\ ?).");

            string newPath = Path.Combine(parent, newFolderName);

            // Nếu đã đúng tên
            if (PathsEqualIgnoreCase(rootFolderPath, newPath))
                return newPath;

            // Trùng folder => thêm hậu tố
            if (Directory.Exists(newPath))
            {
                int i = 1;
                string candidate;
                do
                {
                    candidate = Path.Combine(parent, newFolderName + "_" + i);
                    i++;
                } while (Directory.Exists(candidate));

                newPath = candidate;
                // Lúc này tên folder thực tế sẽ là Path.GetFileName(newPath)
            }

            Directory.Move(rootFolderPath, newPath);
            return newPath;
        }

        private static void RenameAllFilesKeepExtension(string folder, string baseName)
        {
            foreach (var file in Directory.EnumerateFiles(folder, "*", SearchOption.TopDirectoryOnly))
            {
                string dir = Path.GetDirectoryName(file);
                if (string.IsNullOrEmpty(dir)) continue;

                string ext = Path.GetExtension(file);
                string newPath = Path.Combine(dir, baseName + ext);

                if (PathsEqualIgnoreCase(file, newPath))
                    continue;

                // Nếu trùng file => thêm hậu tố _1, _2...
                if (File.Exists(newPath))
                {
                    int i = 1;
                    while (true)
                    {
                        string candidate = Path.Combine(dir, baseName + "_" + i + ext);
                        if (!File.Exists(candidate))
                        {
                            newPath = candidate;
                            break;
                        }
                        i++;
                    }
                }

                File.Move(file, newPath);
            }
        }

        private static bool PathsEqualIgnoreCase(string a, string b)
        {
            string pa = Path.GetFullPath(a).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string pb = Path.GetFullPath(b).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return string.Equals(pa, pb, StringComparison.OrdinalIgnoreCase);
        }
    }


}
