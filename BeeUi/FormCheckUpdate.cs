using BeeInterface;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace BeeUi
{
    public partial class FormCheckUpdate : Form
    {
        public FormCheckUpdate()
        {
            InitializeComponent();
        }
        string folderId;
        private void FormCheckUpdate_Load(object sender, EventArgs e)
        {
           
           if(File.Exists("update.bin"))
                folderId= File.ReadAllText("update.bin");
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);

        }

        private GoogleDriveDllManager _manager = new GoogleDriveDllManager();
        private List<(string FileName, string FileId, Version NewVersion, Version OldVersion)> _pendingUpdates;
        public static async Task<bool> HasInternetByHttpAsync(
    string url = "https://www.google.com")
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(3);
                    using (var response = await client.GetAsync(url))
                    {
                        return response.IsSuccessStatusCode;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        private async void btnCheckUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                lbStatus.Text = "Đang kiểm tra DLL...";
                progressBar.Value = 0;
                lbList.Items.Clear();
                if(! await HasInternetByHttpAsync())
                {
                    MessageBox.Show("Please Check Internet !");
                    return;
                }    
                var progress = new Progress<(int percent, string status)>(p =>
                {
                    progressBar.Value = p.percent;
                    lbStatus.Text = p.status;
                });


                string localFolder = Path.Combine(Environment.CurrentDirectory, "Update");

                _pendingUpdates = await Task.Run(() => _manager.CheckForUpdatesAsync(folderId, localFolder, progress));

                if (_pendingUpdates.Count > 0)
                {
                    foreach (var dll in _pendingUpdates)
                    {
                        string oldVer = dll.OldVersion?.ToString() ?? "Chưa có";
                        string newVer = dll.NewVersion?.ToString() ?? "Không xác định";
                        lbList.Items.Add($"{dll.FileName}: {oldVer} → {newVer}");
                    }

                    lbStatus.Text = $"Có {_pendingUpdates.Count} DLL cần cập nhật!";
                    btnUpdate.Enabled = true;
                }
                else
                {
                    lbStatus.Text = "Tất cả version đều mới nhất!";
                    btnUpdate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_pendingUpdates == null || _pendingUpdates.Count == 0) return;

                lbStatus.Text = "Đang cập nhật DLL...";
                progressBar.Value = 0;

                var progress = new Progress<(int percent, string status)>(p =>
                {
                    progressBar.Value = p.percent;
                    lbStatus.Text = p.status;
                });

                string localFolder = Path.Combine(Environment.CurrentDirectory, "Update");
                await Task.Run(() => _manager.UpdateDllsAsync(_pendingUpdates, localFolder, progress));
                string updateFolder = Path.Combine(Environment.CurrentDirectory, "Update");
                string appFolder = AppDomain.CurrentDomain.BaseDirectory;

                progress = new Progress<(int percent, string status)>(p =>
               {
                   progressBar.Value = p.percent;
                   lbStatus.Text = p.status;
               });

                bool needRestart = true;

                List<string> updatedDlls = new List<string>();

                await Task.Run(() =>
                {
                    _manager.UpdateDllsWithRestart(updateFolder, appFolder, out needRestart, updatedDlls);
                });

                // Nếu cần restart
                if (needRestart)
                {
                    _manager.CreateRestartBatch(appFolder);
                    lbStatus.Text = "Một số DLL cần restart để update.";
                    MessageBox.Show("Ứng dụng sẽ khởi động lại để hoàn tất update.");

                    // Chạy batch trước khi thoát
                    string batchPath = Path.Combine(appFolder, "update_and_restart.bat");
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = batchPath,
                        WorkingDirectory = appFolder,
                        UseShellExecute = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    });

                    // Thoát sau khi chạy batch
                    Application.Exit();
                }
                else
                {
                    lbStatus.Text = "Update DLL hoàn tất (không cần restart).";
                }

                btnUpdate.Enabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
