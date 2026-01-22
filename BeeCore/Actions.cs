using BeeGlobal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeCore
{
    public  class Actions
    {
        public  static  void Shuttown()
        {
            SaveData.Config(Global.Config);
            try
            {
                foreach (Camera camera in BeeCore.Common.listCamera)
                    if (camera != null)
                        camera.DestroyAll();
               
            }
            catch (Exception ex)
            {
            }
          
           
            Process.Start("shutdown", "/s /t 3"); // t=0 để tắt ngay sau khi gọi lệnh
            Application.Exit();
            Process.GetCurrentProcess().Kill(); // Tắt tiến trình hiện tại (hơi "mạnh tay")

        }
        public static void Empty(System.IO.DirectoryInfo directory)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
            directory.Delete();
        }

        public static void DeleteData()
        {
            string resourceDir = Path.Combine(Environment.CurrentDirectory, "Report");
            string[] files = Directory.GetFiles(resourceDir, "*.mdf");
            DateTime dtNow = DateTime.Now;
            foreach (string path2 in files)
            {
                if (path2.Contains("Default.mdf"))
                    continue;
                String Date = Path.GetFileNameWithoutExtension(path2);
                DateTime date2 = DateTime.ParseExact(Date, "yyyyMMdd", null);
                TimeSpan sp = dtNow - date2;
                if (sp.TotalDays > Global.Config.LimitDateSave)
                {

                    File.Delete(path2);
                    String pathLog = path2.Replace(".mdf", "_log.ldf");
                    if(File.Exists(pathLog))
                    File.Delete(pathLog);
                    System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo("Report//" + Date);
                    if (Directory.Exists("Report//" + Date))
                        Empty(directory);

                }

            }
        }
    }
}
