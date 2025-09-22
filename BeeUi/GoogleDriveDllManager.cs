using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi
{
  
    public class GoogleDriveDllManager
    {
        private DriveService service;

        public GoogleDriveDllManager()
        {
            string[] scopes = { DriveService.Scope.DriveReadonly };
            var credential = GoogleCredential.FromFile("credentials.json").CreateScoped(scopes);

            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "DownloadFolder",
            });
        }

        public async Task<List<(string FileName, string FileId, Version NewVersion, Version OldVersion)>> CheckForUpdatesAsync(
        string folderId,
        string localFolder,
        IProgress<(int percent, string status)> progress = null)
        {
            var request = service.Files.List();
            request.Q = $"'{folderId}' in parents and trashed=false";
            var files = (await request.ExecuteAsync()).Files;

            var updateList = new List<(string, string, Version, Version)>();

            int count = 0;
            int total = files.Count(f => f.Name.EndsWith(".bin", StringComparison.OrdinalIgnoreCase));

            foreach (var file in files)
            {
                if (!file.Name.EndsWith(".bin", StringComparison.OrdinalIgnoreCase))
                    continue;

                count++;
                string dllName = Path.ChangeExtension(file.Name, ".dll");
                string tempBin = Path.Combine(Path.GetTempPath(), file.Name);
                string tempDll = Path.Combine(Path.GetTempPath(), dllName);

                progress?.Report((count * 100 / total, $"Đang kiểm tra: {dllName} ({count}/{total})"));

                try
                {
                    using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30))) // Timeout 30s
                    using (var stream = new MemoryStream())
                    {
                        await service.Files.Get(file.Id).DownloadAsync(stream, cancellationToken: cts.Token);
                        File.WriteAllBytes(tempBin, stream.ToArray());
                    }

                    // Đổi .bin -> .dll để đọc version
                    if (File.Exists(tempDll)) File.Delete(tempDll);
                    File.Move(tempBin, tempDll);

                    Version newVersion = GetDllVersion(tempDll);
                    string localPath = Path.Combine(Environment.CurrentDirectory, dllName);
                    Version oldVersion = File.Exists(localPath) ? GetDllVersion(localPath) : null;

                    if (oldVersion == null || (newVersion != null && newVersion > oldVersion))
                        updateList.Add((dllName, file.Id, newVersion, oldVersion));

                    File.Delete(tempDll);
                }
                catch (TaskCanceledException)
                {
                    progress?.Report((count * 100 / total, $"Lỗi: Timeout khi kiểm tra {dllName}"));
                }
                catch (Exception ex)
                {
                    progress?.Report((count * 100 / total, $"Lỗi tải {dllName}: {ex.Message}"));
                }
            }

            progress?.Report((100, "Kiểm tra version hoàn tất!"));
            return updateList;
        }

        public async Task UpdateDllsAsync(
      List<(string FileName, string FileId, Version NewVersion, Version OldVersion)> dllsToUpdate,
      string localFolder,
      IProgress<(int percent, string status)> progress = null)
        {
            int count = 0;
            int total = dllsToUpdate.Count;

            foreach (var dll in dllsToUpdate)
            {
                progress?.Report((count * 100 / total, $"Đang cập nhật: {dll.FileName} ({count + 1}/{total})"));

                string localPath = Path.Combine(localFolder, dll.FileName);
                string tempBin = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(dll.FileName, ".bin"));

                try
                {
                    using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60))) // Timeout 60s
                    using (var stream = new MemoryStream())
                    {
                        await service.Files.Get(dll.FileId).DownloadAsync(stream, cancellationToken: cts.Token);
                        File.WriteAllBytes(tempBin, stream.ToArray());
                    }

                    // Đổi .bin -> .dll rồi copy vào thư mục đích
                    if (File.Exists(localPath)) File.Delete(localPath);
                    File.Move(tempBin, localPath);

                    count++;
                    int percent = (int)((double)count / total * 100);
                    progress?.Report((percent, $"Đã cập nhật: {dll.FileName} ({count}/{total})"));
                }
                catch (TaskCanceledException)
                {
                    progress?.Report((count * 100 / total, $"Lỗi: Timeout khi tải {dll.FileName}"));
                }
                catch (Exception ex)
                {
                    progress?.Report((count * 100 / total, $"Lỗi tải {dll.FileName}: {ex.Message}"));
                }
            }

            progress?.Report((100, "Cập nhật version hoàn tất!"));
        }
        public bool UpdateDllsWithRestart(string updateFolder, string appFolder, out bool needRestart, List<string> updatedDlls)
        {
            needRestart = false;
            updatedDlls = new List<string>();

            if (!Directory.Exists(updateFolder) || !Directory.Exists(appFolder))
                return false;

            var updateFiles = Directory.GetFiles(updateFolder, "*.dll", SearchOption.TopDirectoryOnly);

            foreach (var updateFile in updateFiles)
            {
                string fileName = Path.GetFileName(updateFile);
                string targetFile = Path.Combine(appFolder, fileName);

                Version newVersion = GetDllVersion(updateFile);
                Version oldVersion = File.Exists(targetFile) ? GetDllVersion(targetFile) : null;

                // Nếu DLL mới có version cao hơn
                if (oldVersion == null || (newVersion != null && newVersion > oldVersion))
                {
                    try
                    {
                        // Thử copy trực tiếp
                        File.Copy(updateFile, targetFile, true);
                        updatedDlls.Add($"{fileName}: {oldVersion?.ToString() ?? "N/A"} → {newVersion}");
                    }
                    catch (IOException)
                    {
                        // DLL đang bị lock => copy thành .dll.new để batch xử lý
                        needRestart = true;
                        string newFilePath = targetFile + ".new";
                        File.Copy(updateFile, newFilePath, true);
                        updatedDlls.Add($"{fileName} (Pending Restart): {oldVersion?.ToString() ?? "N/A"} → {newVersion}");
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Trường hợp file đang được ứng dụng sử dụng
                        needRestart = true;
                        string newFilePath = targetFile + ".new";
                        File.Copy(updateFile, newFilePath, true);
                        updatedDlls.Add($"{fileName} (Pending Restart): {oldVersion?.ToString() ?? "N/A"} → {newVersion}");
                    }
                }
            }

            return true;
        }
        public void CreateRestartBatch(string appFolder)
        {
            string exeName = Path.GetFileName(Application.ExecutablePath);
            string appName = Path.GetFileNameWithoutExtension(exeName);
            string batPath = Path.Combine(appFolder, "update_and_restart.bat");

            string batContent = $@"
            @echo off
            set LOGFILE={appFolder}\\update_log.txt

            echo ==== START UPDATE: %date% %time% ==== >> ""%LOGFILE%""
            set APPNAME={appName}

            tasklist | find /I ""%APPNAME%.exe"" >nul
            if %errorlevel%==0 (
                echo Application running, killing process... >> ""%LOGFILE%""
                taskkill /F /IM ""%APPNAME%.exe"" >> ""%LOGFILE%"" 2>&1
                timeout /t 2 >nul
            ) else (
                echo Application is not running. >> ""%LOGFILE%""
            )

            for %%f in (""{appFolder}\\*.dll.new"") do (
                if exist ""%%f"" (
                    echo Found new DLL: %%f >> ""%LOGFILE%""
                    if exist ""%%~dpnf"" (
                        echo Deleting old DLL: %%~dpnf >> ""%LOGFILE%""
                        del ""%%~dpnf"" >> ""%LOGFILE%"" 2>&1
                    )
                    echo Renaming %%f to %%~dpnf >> ""%LOGFILE%""
                    move /Y ""%%f"" ""%%~dpnf"" >> ""%LOGFILE%"" 2>&1
                ) else (
                    echo No .dll.new found for %%f >> ""%LOGFILE%""
                )
            )

            echo Restarting application... >> ""%LOGFILE%""
            start """" ""{Path.Combine(appFolder, exeName)}""

            echo ==== END UPDATE: %date% %time% ==== >> ""%LOGFILE%""
            echo. >> ""%LOGFILE%""

            del ""%~f0""
            ";

            File.WriteAllText(batPath, batContent);
        }
        private Version GetDllVersion(string filePath)
        {
            try
            {
                var info = FileVersionInfo.GetVersionInfo(filePath);
                return new Version(info.FileMajorPart, info.FileMinorPart, info.FileBuildPart, info.FilePrivatePart);
            }
            catch
            {
                return null;
            }
        }
    }

}
