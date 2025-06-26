using BeeCore;
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
                G.Project = Path.GetFileNameWithoutExtension(saveFile.FileName);
                Directory.CreateDirectory("Program\\" + G.Project);
                Access.SaveProg("Program\\" + G.Project + "\\" + G.Project + ".prog", G.PropetyTools);
              //  G.Project = Path.GetFileNameWithoutExtension(saveFile.FileName);

               G.Header. IniProject();

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
            G.PropetyTools[G.indexChoose] = new List<BeeCore.PropetyTool>();
            saveFile.InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\Program";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                G.Project = Path.GetFileNameWithoutExtension(saveFile.FileName);
               Directory.CreateDirectory("Program\\"+G.Project);
                Access.SaveProg("Program\\" + G.Project+"\\"+ G.Project+ ".prog", G.PropetyTools);
                G.Header.IniProject();
                if (!G.Header.workLoadProgram.IsBusy)
                    G.Header.workLoadProgram.RunWorkerAsync();
            }
        }

        private void btnDelect_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn muốn Xóa  Model này ?", " Xóa  Model", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int indexCur = G.listProgram.SelectedIndex;

                G.listProgram.SelectedIndex = indexCur - 1;
                File.Delete("Program\\" + G.listProgram.Items[indexCur].ToString());
                string[] files = Directory.GetFiles("Program", "*.prog", SearchOption.TopDirectoryOnly);
              String[]  PathFile = files.Select(a => Path.GetFileName(a)).ToArray();
                G.listProgram.DataSource = PathFile; IsSaveAs = true;
                if (G.listProgram.Items.Count == 0) return;
                G.Project = G.listProgram.Items[0].ToString();
                Properties.Settings.Default.programCurrent = G.Project;
                Properties.Settings.Default.Save();
                G.Header.IniProject();
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
        private void btnMenu_Click(object sender, EventArgs e)
        {
           // pMenu.Visible = !pMenu.Visible;
            if (btnMenu.IsCLick)
            {
                btnMenu.Corner = Corner.Right;
               this.Width=btnMenu.Width-1;
                btnMenu.Text = "";
                btnMenu.Image = Properties.Resources.Show;
            }
            else
            {
                btnMenu.Corner = Corner.Right;
                this.Width = 400;
                // btnMenu.Parent.Width = 150;
                //  btnMenu.Text = "Menu Program";
                btnMenu.Image = Properties.Resources.Hide;
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditProg));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnMenu = new BeeUi.Common.RJButton();
            this.pMenu = new System.Windows.Forms.TableLayoutPanel();
            this.btnDelect = new BeeUi.Common.RJButton();
            this.btnSave = new BeeUi.Common.RJButton();
            this.btnSaveAs = new BeeUi.Common.RJButton();
            this.btnAdd = new BeeUi.Common.RJButton();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.pMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.btnMenu, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pMenu, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(400, 58);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnMenu
            // 
            this.btnMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnMenu.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnMenu.BorderColor = System.Drawing.Color.Transparent;
            this.btnMenu.BorderRadius = 12;
            this.btnMenu.BorderSize = 0;
            this.btnMenu.ButtonImage = null;
            this.btnMenu.Corner = BeeCore.Corner.Right;
            this.btnMenu.FlatAppearance.BorderSize = 0;
            this.btnMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu.ForeColor = System.Drawing.Color.Black;
            this.btnMenu.Image = ((System.Drawing.Image)(resources.GetObject("btnMenu.Image")));
            this.btnMenu.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenu.IsCLick = false;
            this.btnMenu.IsNotChange = false;
            this.btnMenu.IsRect = false;
            this.btnMenu.IsUnGroup = true;
            this.btnMenu.Location = new System.Drawing.Point(370, 0);
            this.btnMenu.Margin = new System.Windows.Forms.Padding(0);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(30, 58);
            this.btnMenu.TabIndex = 26;
            this.btnMenu.TextColor = System.Drawing.Color.Black;
            this.btnMenu.UseVisualStyleBackColor = false;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // pMenu
            // 
            this.pMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pMenu.ColumnCount = 4;
            this.pMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.pMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.pMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.pMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pMenu.Controls.Add(this.btnDelect, 3, 0);
            this.pMenu.Controls.Add(this.btnSave, 1, 0);
            this.pMenu.Controls.Add(this.btnSaveAs, 2, 0);
            this.pMenu.Controls.Add(this.btnAdd, 0, 0);
            this.pMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMenu.Location = new System.Drawing.Point(0, 0);
            this.pMenu.Margin = new System.Windows.Forms.Padding(0);
            this.pMenu.Name = "pMenu";
            this.pMenu.Padding = new System.Windows.Forms.Padding(3);
            this.pMenu.RowCount = 1;
            this.pMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pMenu.Size = new System.Drawing.Size(370, 58);
            this.pMenu.TabIndex = 25;
            this.pMenu.SizeChanged += new System.EventHandler(this.pMenu_SizeChanged);
            // 
            // btnDelect
            // 
            this.btnDelect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnDelect.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnDelect.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelect.BackgroundImage")));
            this.btnDelect.BorderColor = System.Drawing.Color.Transparent;
            this.btnDelect.BorderRadius = 8;
            this.btnDelect.BorderSize = 1;
            this.btnDelect.ButtonImage = null;
            this.btnDelect.Corner = BeeCore.Corner.Both;
            this.btnDelect.FlatAppearance.BorderSize = 0;
            this.btnDelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelect.ForeColor = System.Drawing.Color.Black;
            this.btnDelect.Image = ((System.Drawing.Image)(resources.GetObject("btnDelect.Image")));
            this.btnDelect.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDelect.IsCLick = false;
            this.btnDelect.IsNotChange = true;
            this.btnDelect.IsRect = false;
            this.btnDelect.IsUnGroup = true;
            this.btnDelect.Location = new System.Drawing.Point(276, 5);
            this.btnDelect.Margin = new System.Windows.Forms.Padding(3, 2, 5, 2);
            this.btnDelect.Name = "btnDelect";
            this.btnDelect.Size = new System.Drawing.Size(86, 48);
            this.btnDelect.TabIndex = 13;
            this.btnDelect.Text = "Delete";
            this.btnDelect.TextColor = System.Drawing.Color.Black;
            this.btnDelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelect.UseVisualStyleBackColor = false;
            this.btnDelect.Click += new System.EventHandler(this.btnDelect_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnSave.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSave.BorderColor = System.Drawing.Color.Transparent;
            this.btnSave.BorderRadius = 8;
            this.btnSave.BorderSize = 1;
            this.btnSave.ButtonImage = null;
            this.btnSave.Corner = BeeCore.Corner.Both;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.IsCLick = false;
            this.btnSave.IsNotChange = true;
            this.btnSave.IsRect = false;
            this.btnSave.IsUnGroup = true;
            this.btnSave.Location = new System.Drawing.Point(96, 5);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(84, 48);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Save";
            this.btnSave.TextColor = System.Drawing.Color.Black;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnSaveAs.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnSaveAs.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveAs.BackgroundImage")));
            this.btnSaveAs.BorderColor = System.Drawing.Color.Transparent;
            this.btnSaveAs.BorderRadius = 8;
            this.btnSaveAs.BorderSize = 1;
            this.btnSaveAs.ButtonImage = null;
            this.btnSaveAs.Corner = BeeCore.Corner.Both;
            this.btnSaveAs.FlatAppearance.BorderSize = 0;
            this.btnSaveAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveAs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveAs.ForeColor = System.Drawing.Color.Black;
            this.btnSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAs.Image")));
            this.btnSaveAs.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSaveAs.IsCLick = false;
            this.btnSaveAs.IsNotChange = true;
            this.btnSaveAs.IsRect = false;
            this.btnSaveAs.IsUnGroup = true;
            this.btnSaveAs.Location = new System.Drawing.Point(186, 5);
            this.btnSaveAs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(84, 48);
            this.btnSaveAs.TabIndex = 15;
            this.btnSaveAs.Text = "Copy";
            this.btnSaveAs.TextColor = System.Drawing.Color.Black;
            this.btnSaveAs.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveAs.UseVisualStyleBackColor = false;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnAdd.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
            this.btnAdd.BorderColor = System.Drawing.Color.Transparent;
            this.btnAdd.BorderRadius = 8;
            this.btnAdd.BorderSize = 1;
            this.btnAdd.ButtonImage = null;
            this.btnAdd.Corner = BeeCore.Corner.Both;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.Black;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAdd.IsCLick = false;
            this.btnAdd.IsNotChange = true;
            this.btnAdd.IsRect = false;
            this.btnAdd.IsUnGroup = true;
            this.btnAdd.Location = new System.Drawing.Point(6, 5);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(84, 48);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "Add";
            this.btnAdd.TextColor = System.Drawing.Color.Black;
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // saveFile
            // 
            this.saveFile.DefaultExt = "*.prog";
            this.saveFile.Filter = "Program | *.prog";
            this.saveFile.InitialDirectory = "Program";
            this.saveFile.Title = "Save As";
            // 
            // EditProg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "EditProg";
            this.Size = new System.Drawing.Size(400, 58);
            this.Load += new System.EventHandler(this.EditProg_Load);
            this.SizeChanged += new System.EventHandler(this.EditProg_SizeChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void EditProg_SizeChanged(object sender, EventArgs e)
        {
       
          

        
       }

        private void pMenu_SizeChanged(object sender, EventArgs e)
        {
            //if (G.EditTool == null) return;
            // BeeCore.CustomGui.RoundRg(pMenu, G.Config.RoundRad);
        }

        private void EditProg_Load(object sender, EventArgs e)
        {
            if(G.EditTool == null)return;   
            btnMenu.PerformClick();
        }
    }
}
