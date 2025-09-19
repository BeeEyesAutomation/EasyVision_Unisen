using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUpdate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            path = Properties.Settings.Default.path;
            textBox1.Text = path;
        }
        String path = "";
        private void btnfolder_Click(object sender, EventArgs e)
        {
            if (folder.ShowDialog() == DialogResult.OK)
            {
                path =   folder.SelectedPath;
                textBox1.Text = path;
                Properties.Settings.Default.path = path;
             Properties.Settings.Default.Save();
            }

        }
        List<string> pathName = new List<string>();
        private void button1_Click(object sender, EventArgs e)
        {
            var regex = new Regex(@"^(Bee.*\.dll|OKNG\.dll|PylonCli\.dll)$", RegexOptions.IgnoreCase);//|GCBase_MD_VC141_v3_1_Basler_pylon\.dll|GenApi_MD_VC141_v3_1_Basler_pylon\.dll|Log_MD_VC141_v3_1_Basler_pylon\.dll|MathParser_MD_VC141_v3_1_Basler_pylon\.dll|XmlParser_MD_VC141_v3_1_Basler_pylon\.dll|PylonC_v6_1\.dll|PLC_Communication\.dll
            List<string> dllFiles = Directory
                .GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly)
                .Where(f => regex.IsMatch(Path.GetFileName(f)))
                .ToList();

            if (dllFiles.Count == 0)
            {
                Console.WriteLine("Không tìm thấy file DLL nào!");
                return;
            }
            lbInfor.Text = ""; pathName = new List<string>();
            foreach (var dll in dllFiles)
            {
                try
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(dll);
                    pathName.Add(dll);
                    lbInfor.Text+=$"{Path.GetFileName(dll)} - Version: {versionInfo.FileVersion ?? "Không có thông tin"}"+'\n';
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{Path.GetFileName(dll)} - Lỗi đọc version: {ex.Message}");
                }
            }
        }

        private async void btnMove_Click(object sender, EventArgs e)
        {
            foreach(String s in pathName)
            {
               String name= Path.GetFileName(s);
                name = name.Replace(".dll", ".bin");
                String pathDir = "G:\\My Drive\\EasyVision\\v2\\" + name;
              
                
                File.Copy(s, pathDir, true);
              
             
            }
            await Task.Delay(10000);
            foreach (String s in pathName)
            {
                String name = Path.GetFileName(s);
                name = name.Replace(".dll", ".bin");
             
                String pathDirOld = "G:\\My Drive\\EasyVision\\v2\\" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                if (!Directory.Exists(pathDirOld)) Directory.CreateDirectory(pathDirOld);
              
             
                File.Copy(s, pathDirOld + "\\" + name, true);
            }
            MessageBox.Show("Complete!");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
