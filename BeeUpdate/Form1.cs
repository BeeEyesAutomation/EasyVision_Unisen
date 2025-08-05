using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
            string searchPattern = "Bee*.dll";

            if (!Directory.Exists(path))
            {
                Console.WriteLine("Folder không tồn tại!");
                return;
            }

            var dllFiles = Directory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);

            if (dllFiles.Length == 0)
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

        private void btnMove_Click(object sender, EventArgs e)
        {
            foreach(String s in pathName)
            {
               String name= Path.GetFileName(s);
                name = name.Replace(".dll", ".bin");
                String pathDir = "G:\\My Drive\\EasyVision\\Update\\" + name;
                File.Copy(s, pathDir, true);
            }
            MessageBox.Show("Complete!");
        }
    }
}
