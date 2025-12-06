using System;
using System.IO;
using System.Text;
namespace    BeeCore
{
    public static class TarProgramHelper
    {
        /// <summary>
        /// Folder Program mặc định: [exe]\Program
        /// </summary>
        public static string GetDefaultProgramFolder()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Program");
        }

        /// <summary>
        /// Export TOÀN BỘ folder Program mặc định thành file .tar
        /// (Program\*.* -> vào .tar không có lớp "Program" bọc ngoài)
        /// </summary>
        public static void ExportDefaultProgram(string tarFilePath)
        {
            string folder = GetDefaultProgramFolder();
            ExportFolderToTar(folder, tarFilePath);
        }

        /// <summary>
        /// Export CHỈ 1 folder con trong Program, theo tên prog nhập vào.
        /// Ví dụ progName = "ProgA" => export .\Program\ProgA thành .tar,
        /// trong .tar đường dẫn sẽ là "ProgA\...".
        /// </summary>
        public static void ExportProgramSubFolder(string progName, string tarFilePath)
        {
            if (string.IsNullOrWhiteSpace(progName))
                throw new ArgumentNullException("progName");

            string programRoot = GetDefaultProgramFolder();
            string subFolder = Path.Combine(programRoot, progName);

            if (!Directory.Exists(subFolder))
                throw new DirectoryNotFoundException("Không tìm thấy prog: " + subFolder);

            // Đảm bảo thư mục chứa file .tar tồn tại
            string fullTar = Path.GetFullPath(tarFilePath);
            string tarDir = Path.GetDirectoryName(fullTar);
            if (!string.IsNullOrEmpty(tarDir) && !Directory.Exists(tarDir))
            {
                Directory.CreateDirectory(tarDir);
            }

            using (FileStream fs = File.Create(fullTar))
            using (TarWriter tar = new TarWriter(fs))
            {
                // baseFolder = Program, currentFolder = Program\[progName]
                // => trong tar sẽ có "progName\..."
                string baseFolder = Path.GetFullPath(programRoot);
                string currentFolder = Path.GetFullPath(subFolder);
                WriteFolderRecursive(tar, baseFolder, currentFolder);
            }
        }
        public static void ExportProgramSubFolderWithRename(string progName, string tarFilePath)
        {
            string programRoot = GetDefaultProgramFolder();
            string folderPath = Path.Combine(programRoot, progName);

            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException("Không tìm thấy prog: " + folderPath);

            string newProgName = Path.GetFileNameWithoutExtension(tarFilePath);
            string fullTar = Path.GetFullPath(tarFilePath);
            string tarDir = Path.GetDirectoryName(fullTar);

            if (!Directory.Exists(tarDir))
                Directory.CreateDirectory(tarDir);

            using (FileStream fs = File.Create(fullTar))
            using (TarWriter tar = new TarWriter(fs))
            {
                foreach (string file in Directory.GetFiles(folderPath))
                {
                    string fileName = Path.GetFileName(file);

                    // nếu bắt đầu bằng progName => đổi sang newProgName
                    string newName =
                        fileName.StartsWith(progName, StringComparison.OrdinalIgnoreCase)
                        ? newProgName + Path.GetExtension(fileName)
                        : fileName;

                    string tarPath = newProgName + "/" + newName;

                    tar.WriteFile(file, tarPath);
                }

                // xử lý folder con nếu có
                foreach (string dir in Directory.GetDirectories(folderPath))
                {
                    string dirName = Path.GetFileName(dir);
                    string newDirPath = newProgName + "/" + dirName + "/";

                    tar.WriteDirectory(newDirPath, DateTime.Now);

                    // copy file trong mỗi dir con
                    CopySubDir(tar, dir, newProgName + "/" + dirName, progName, newProgName);
                }
            }
        }

        private static void CopySubDir(TarWriter tar, string currentDir, string basePathInTar, string progName, string newProgName)
        {
            foreach (string file in Directory.GetFiles(currentDir))
            {
                string fileName = Path.GetFileName(file);

                string newName =
                    fileName.StartsWith(progName, StringComparison.OrdinalIgnoreCase)
                    ? newProgName + Path.GetExtension(fileName)
                    : fileName;

                string tarPath = basePathInTar + "/" + newName;

                tar.WriteFile(file, tarPath);
            }

            foreach (string dir in Directory.GetDirectories(currentDir))
            {
                string dirName = Path.GetFileName(dir);
                string newDir = basePathInTar + "/" + dirName;

                tar.WriteDirectory(newDir + "/", DateTime.Now);

                CopySubDir(tar, dir, newDir, progName, newProgName);
            }
        }
        /// <summary>
        /// Import file .tar vào folder Program mặc định.
        /// Nếu .tar được tạo bởi ExportProgramSubFolder("X"),
        /// thì khi import sẽ ra Program\X\...
        /// </summary>
        public static void ImportToDefaultProgram(string tarFilePath)
        {
            string folder = GetDefaultProgramFolder();
            ImportTarToFolder(tarFilePath, folder);
        }

        /// <summary>
        /// Export folder bất kỳ thành file .tar (không dùng thư viện ngoài)
        /// </summary>
        public static void ExportFolderToTar(string sourceFolder, string tarFilePath)
        {
            if (string.IsNullOrEmpty(sourceFolder))
                throw new ArgumentNullException(nameof(sourceFolder));

            if (!Directory.Exists(sourceFolder))
                throw new DirectoryNotFoundException("Không tìm thấy folder: " + sourceFolder);

            string fullSource = Path.GetFullPath(sourceFolder);
            string tarDir = Path.GetDirectoryName(Path.GetFullPath(tarFilePath));
            if (!string.IsNullOrEmpty(tarDir) && !Directory.Exists(tarDir))
            {
                Directory.CreateDirectory(tarDir);
            }

            using (FileStream fs = File.Create(tarFilePath))
            using (TarWriter tar = new TarWriter(fs))
            {
                // Export cả folder này, không có lớp cha bọc ngoài
                WriteFolderRecursive(tar, fullSource, fullSource);
            }
        }

        /// <summary>
        /// Import file .tar ra folder đích
        /// </summary>
        public static void ImportTarToFolder(string tarFilePath, string targetFolder)
        {
            if (!File.Exists(tarFilePath))
                throw new FileNotFoundException("Không tìm thấy file .tar: " + tarFilePath);

            string fullTarget = Path.GetFullPath(targetFolder);
            Directory.CreateDirectory(fullTarget);

            using (FileStream fs = File.OpenRead(tarFilePath))
            {
                byte[] header = new byte[512];

                while (true)
                {
                    int read = fs.Read(header, 0, 512);
                    if (read < 512) break;

                    // Kiểm tra block 0 (kết thúc tar)
                    bool allZero = true;
                    for (int i = 0; i < 512; i++)
                    {
                        if (header[i] != 0)
                        {
                            allZero = false;
                            break;
                        }
                    }
                    if (allZero) break;

                    string name = ReadString(header, 0, 100);
                    if (string.IsNullOrEmpty(name))
                        break;

                    long size = ReadOctal(header, 124, 12);
                    char typeFlag = (char)header[156];

                    string outPath = Path.Combine(fullTarget, name.Replace('/', Path.DirectorySeparatorChar));

                    if (typeFlag == '5') // directory
                    {
                        Directory.CreateDirectory(outPath);
                    }
                    else if (typeFlag == '0' || typeFlag == '\0') // file
                    {
                        string dir = Path.GetDirectoryName(outPath);
                        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                            Directory.CreateDirectory(dir);

                        ExtractFile(fs, outPath, size);
                    }
                    else
                    {
                        // Các type khác thì bỏ qua data
                        SkipFileData(fs, size);
                    }

                    // Nhảy padding cho đủ block 512
                    long pad = (512 - (size % 512)) % 512;
                    if (pad > 0)
                    {
                        fs.Seek(pad, SeekOrigin.Current);
                    }
                }
            }
        }

        // ============ PRIVATE: EXPORT SUPPORT ============

        private static void WriteFolderRecursive(TarWriter tar, string baseFolder, string currentFolder)
        {
            string relative = currentFolder.Substring(baseFolder.Length)
                                           .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            if (!string.IsNullOrEmpty(relative))
            {
                tar.WriteDirectory(relative.Replace(Path.DirectorySeparatorChar, '/'), DateTime.Now);
            }

            // Ghi file
            string[] files = Directory.GetFiles(currentFolder);
            foreach (string file in files)
            {
                string relFile = file.Substring(baseFolder.Length)
                                     .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                tar.WriteFile(file, relFile.Replace(Path.DirectorySeparatorChar, '/'));
            }

            // Đệ quy folder con
            string[] dirs = Directory.GetDirectories(currentFolder);
            foreach (string dir in dirs)
            {
                WriteFolderRecursive(tar, baseFolder, dir);
            }
        }

        // ============ PRIVATE: IMPORT SUPPORT ============

        private static void ExtractFile(FileStream fs, string path, long size)
        {
            using (FileStream outFs = File.Create(path))
            {
                byte[] buffer = new byte[8192];
                long remain = size;

                while (remain > 0)
                {
                    int toRead = (int)Math.Min(buffer.Length, remain);
                    int read = fs.Read(buffer, 0, toRead);
                    if (read <= 0) break;

                    outFs.Write(buffer, 0, read);
                    remain -= read;
                }
            }
        }

        private static void SkipFileData(FileStream fs, long size)
        {
            if (size > 0)
            {
                fs.Seek(size, SeekOrigin.Current);
            }
        }

        private static string ReadString(byte[] buf, int offset, int length)
        {
            return Encoding.ASCII.GetString(buf, offset, length).Trim('\0', ' ');
        }

        private static long ReadOctal(byte[] buf, int offset, int length)
        {
            string s = Encoding.ASCII.GetString(buf, offset, length).Trim('\0', ' ');
            if (string.IsNullOrEmpty(s))
                return 0;
            return Convert.ToInt64(s, 8);
        }

        // ============ PRIVATE: TAR WRITER CLASS ============

        private sealed class TarWriter : IDisposable
        {
            private readonly Stream _out;
            private bool _closed = false;

            public TarWriter(Stream output)
            {
                _out = output ?? throw new ArgumentNullException(nameof(output));
            }

            public void WriteFile(string filePath, string tarEntryName)
            {
                FileInfo fi = new FileInfo(filePath);
                using (FileStream fs = File.OpenRead(filePath))
                {
                    WriteHeader(tarEntryName, fi.Length, false, fi.LastWriteTime);
                    CopyTo512(fs, _out);
                }
            }

            public void WriteDirectory(string tarEntryName, DateTime mtime)
            {
                if (!tarEntryName.EndsWith("/"))
                    tarEntryName += "/";

                WriteHeader(tarEntryName, 0, true, mtime);
            }

            private void WriteHeader(string name, long size, bool isDir, DateTime modTime)
            {
                byte[] header = new byte[512];

                WriteString(header, 0, name, 100);
                WriteOctal(header, 100, 8, isDir ? 0777 : 0644); // mode
                WriteOctal(header, 108, 8, 0); // uid
                WriteOctal(header, 116, 8, 0); // gid
                WriteOctal(header, 124, 12, size);
                WriteOctal(header, 136, 12, ToUnixTime(modTime));

                // Checksum field: tạm điền space
                for (int i = 148; i < 156; i++)
                    header[i] = 0x20; // ' '

                header[156] = (byte)(isDir ? '5' : '0');

                WriteString(header, 257, "ustar", 6);
                WriteString(header, 263, "00", 2);

                // Tính checksum
                long sum = 0;
                for (int i = 0; i < 512; i++)
                    sum += header[i];

                WriteChecksum(header, sum);

                _out.Write(header, 0, header.Length);
            }

            private static void CopyTo512(Stream input, Stream output)
            {
                byte[] buffer = new byte[8192];
                long size = 0;

                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, read);
                    size += read;
                }

                long pad = 512 - (size % 512);
                if (pad != 512)
                {
                    byte[] zeros = new byte[pad];
                    output.Write(zeros, 0, zeros.Length);
                }
            }

            private static void WriteString(byte[] buffer, int offset, string str, int length)
            {
                if (str == null) str = string.Empty;
                byte[] bytes = Encoding.ASCII.GetBytes(str);
                int n = Math.Min(bytes.Length, length);
                Array.Copy(bytes, 0, buffer, offset, n);
            }

            private static void WriteOctal(byte[] buffer, int offset, int length, long value)
            {
                string s = Convert.ToString(value, 8);
                s = s.PadLeft(length - 1, '0'); // chừa 1 byte cuối
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                Array.Copy(bytes, 0, buffer, offset, bytes.Length);
            }

            private static void WriteChecksum(byte[] header, long checksum)
            {
                string s = Convert.ToString(checksum, 8);
                if (s.Length > 6)
                    s = s.Substring(s.Length - 6);
                s = s.PadLeft(6, '0');

                byte[] bytes = Encoding.ASCII.GetBytes(s);
                Array.Copy(bytes, 0, header, 148, bytes.Length);

                header[148 + 6] = 0;          // null
                header[148 + 7] = (byte)' ';  // space
            }

            private static long ToUnixTime(DateTime dt)
            {
                return (long)(dt.ToUniversalTime()
                    .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)))
                    .TotalSeconds;
            }

            public void Close()
            {
                if (_closed) return;

                // Kết thúc tar: 2 block 512 toàn 0
                byte[] zeros = new byte[1024];
                _out.Write(zeros, 0, zeros.Length);

                _closed = true;
            }

            public void Dispose()
            {
                Close();
            }
        }
    }
}
