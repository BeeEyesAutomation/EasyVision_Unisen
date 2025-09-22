﻿using BeeCore;
using BeeGlobal;
using BeeInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi.Unit
{
    public partial class EditProg : UserControl
    {
        public EditProg()
        {
            InitializeComponent();
            G.EditProg=this;
        }
        bool IsSaveAs = false;
        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            saveFile.InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\Program";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                Global.Project = Path.GetFileNameWithoutExtension(saveFile.FileName);
                Directory.CreateDirectory("Program\\" + Global.Project);
                Access.SaveProg("Program\\" + Global.Project + "\\" + Global.Project + ".prog", BeeCore.Common.PropetyTools);
              //  Global.Project = Path.GetFileNameWithoutExtension(saveFile.FileName);

               G.Header.RefreshListPJ();
                Global.IsLoadProgFist = true;
                if (!G.Header.workLoadProgram.IsBusy)
                    G.Header.workLoadProgram.RunWorkerAsync();
                //PathFile = files.Select(a => Path.GetFileName(a)).ToArray();
                //items = PathFile.ToList();
                //IsLoad=true;
                //G.listProgram.DataSource = PathFile;
                //txtQrCode.Text = G.listProgram.SelectedValue.ToString();
                //if (G.listProgram.FindStringExact(Properties.Settings.Default.programCurrent) == 0)
                //{
                //    this.LoadProg("Program\\" + G.listProgram.SelectedValue.ToString());
                //}
                //  G.listProgram.SelectedIndex=G.listProgram.FindStringExact(saveFile.FileName);

                //    G.listProgram.SelectedValue = G.listProgram.SelectedIndex;
                // File.Copy("Program\\" + G.NameProject.Trim(), saveFile.FileName,true);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            BeeCore.Common.PropetyTools[Global.IndexChoose] = new List<BeeCore.PropetyTool>();
            saveFile.InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\Program";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                Global.Project = Path.GetFileNameWithoutExtension(saveFile.FileName);
               Directory.CreateDirectory("Program\\"+Global.Project);
                Access.SaveProg("Program\\" + Global.Project+"\\"+ Global.Project+ ".prog", BeeCore.Common.PropetyTools);
                G.Header.RefreshListPJ();
                if (!G.Header.workLoadProgram.IsBusy)
                    G.Header.workLoadProgram.RunWorkerAsync();
            }
        }

        private void btnDelect_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn muốn Xóa  Model này ?", " Xóa  Model"+Global.Project, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string path = Path.Combine("Program", Global.Project);

                if (Directory.Exists(path))
                {
                    try
                    {
                        foreach (var file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
                            File.SetAttributes(file, FileAttributes.Normal);

                        Directory.Delete(path, true);
                    }
                    catch(Exception)
                    {

                    }
                }
                //int indexCur = G.listProgram.Items.IndexOf(Global.Project);
                //if(G.listProgram.Items.Count>0)
                //G.listProgram.SelectedIndex = indexCur - 1;
               // Directory.Delete("Program\\" + Global.Project,true);
                G.Header.RefreshListPJ();
              //  string[] files = Directory.GetFiles("Program", "*.prog", SearchOption.TopDirectoryOnly);
              //String[]  PathFile = files.Select(a => Path.GetFileName(a)).ToArray();
              //  G.listProgram.DataSource = PathFile; IsSaveAs = true;
                if (G.listProgram.Items.Count == 0) return;
                Global.Project = G.listProgram.Items[0].ToString();
                Properties.Settings.Default.programCurrent = Global.Project;
                Properties.Settings.Default.Save();
                G.Header.RefreshListPJ(); Global.IsLoadProgFist = true;
                if (!G.Header.workLoadProgram.IsBusy)
                    G.Header.workLoadProgram.RunWorkerAsync();

            }

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            if (!G.Header.workSaveProject.IsBusy)
                G.Header.workSaveProject.RunWorkerAsync();

            //MessageBox.Show("Save Program Success");
        }
      public  int OldWidth = 0;
        private void btnMenu_Click(object sender, EventArgs e)
        {
           // pMenu.Visible = !pMenu.Visible;
            if (btnMenu.IsCLick)
            {
                OldWidth = this.Width;
                btnMenu.Corner = Corner.Right;
               
               this.Width=btnMenu.Width-1;
                btnMenu.Text = "";
                btnMenu.Image = Properties.Resources.Show;
            }
            else
            {
                btnMenu.Corner = Corner.Right;
                if (OldWidth <= btnMenu.Width + 20) OldWidth = 300;
                this.Width = OldWidth;
                // btnMenu.Parent.Width = 150;
                //  btnMenu.Text = "Menu Program";
                btnMenu.Image = Properties.Resources.Hide;
            }
        }

       

        

        private void EditProg_SizeChanged(object sender, EventArgs e)
        {
            if (btnMenu.IsCLick)
            {
                Global.Config.WidthEditProg = this.Width;
                SaveData.Config(Global.Config);
            }




       }

        private void pMenu_SizeChanged(object sender, EventArgs e)
        {
            //if (Global.EditTool == null) return;
            // BeeCore.CustomGui.RoundRg(pMenu,Global.Config.RoundRad);
        }

        private void EditProg_Load(object sender, EventArgs e)
        {

            //if(Global.EditTool == null)return;   
            //btnMenu.PerformClick();
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(" Rename Prog To " + G.Header.txtQrCode.Text+ " .Are you sure!", "Rename Prog", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
               if( Directory.Exists("Program\\"+Global.Project))
                {
                    String newName = G.Header.txtQrCode.Text.Trim();
                    Directory.Move("Program\\" + Global.Project, "Program\\" + newName);
                   
                    if (File.Exists("Program\\" + newName + "\\"+Global.Project+ ".cam"))
                        File.Move("Program\\" + newName + "\\" + Global.Project + ".cam", "Program\\" + newName + "\\" + newName + ".cam");
                    if (File.Exists("Program\\" + newName + "\\" + Global.Project + ".para"))
                        File.Move("Program\\" + newName + "\\" + Global.Project + ".para", "Program\\" + newName + "\\" + newName + ".para");
                    if (File.Exists("Program\\" + newName + "\\" + Global.Project + ".prog"))
                        File.Move("Program\\" + newName + "\\" + Global.Project + ".prog", "Program\\" + newName + "\\" + newName + ".prog");
             
                   
                    G.listProgram.Visible = false;
                    G.Header.RefreshListPJ();

                    Global.Project = newName;
                    Properties.Settings.Default.programCurrent = Global.Project;
                    Properties.Settings.Default.Save();
                }    
            }
        }
    }
}
