using BeeCore;
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
            if (MessageBox.Show("Bạn chắc chắn muốn Xóa  Model này ?", " Xóa  Model", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int indexCur = G.listProgram.Items.IndexOf(Global.Project);
                if(G.listProgram.Items.Count>0)
                G.listProgram.SelectedIndex = indexCur - 1;
                File.Delete("Program\\" + G.listProgram.Items[indexCur].ToString());
                string[] files = Directory.GetFiles("Program", "*.prog", SearchOption.TopDirectoryOnly);
              String[]  PathFile = files.Select(a => Path.GetFileName(a)).ToArray();
                G.listProgram.DataSource = PathFile; IsSaveAs = true;
                if (G.listProgram.Items.Count == 0) return;
                Global.Project = G.listProgram.Items[0].ToString();
                Properties.Settings.Default.programCurrent = Global.Project;
                Properties.Settings.Default.Save();
                G.Header.RefreshListPJ();
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
        int OldWidth = 0;
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditProg));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnMenu = new BeeInterface.RJButton();
            this.pMenu = new System.Windows.Forms.TableLayoutPanel();
            this.btnDelect = new BeeInterface.RJButton();
            this.btnSave = new BeeInterface.RJButton();
            this.btnSaveAs = new BeeInterface.RJButton();
            this.btnAdd = new BeeInterface.RJButton();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.pMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(500, 56);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnMenu
            // 
            this.btnMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMenu.AutoFont = false;
            this.btnMenu.AutoFontHeightRatio = 0.75F;
            this.btnMenu.AutoFontMax = 100F;
            this.btnMenu.AutoFontMin = 6F;
            this.btnMenu.AutoFontWidthRatio = 0.92F;
            this.btnMenu.AutoImage = true;
            this.btnMenu.AutoImageMaxRatio = 0.75F;
            this.btnMenu.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMenu.AutoImageTint = true;
            this.btnMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnMenu.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnMenu.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnMenu.BorderRadius = 8;
            this.btnMenu.BorderSize = 1;
            this.btnMenu.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMenu.Corner = BeeGlobal.Corner.Right;
            this.btnMenu.DebounceResizeMs = 16;
            this.btnMenu.FlatAppearance.BorderSize = 0;
            this.btnMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu.ForeColor = System.Drawing.Color.Black;
            this.btnMenu.Image = ((System.Drawing.Image)(resources.GetObject("btnMenu.Image")));
            this.btnMenu.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenu.ImageDisabled = null;
            this.btnMenu.ImageHover = null;
            this.btnMenu.ImageNormal = null;
            this.btnMenu.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnMenu.ImagePressed = null;
            this.btnMenu.ImageTextSpacing = 6;
            this.btnMenu.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnMenu.ImageTintHover = System.Drawing.Color.Empty;
            this.btnMenu.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnMenu.ImageTintOpacity = 0.5F;
            this.btnMenu.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnMenu.IsCLick = false;
            this.btnMenu.IsNotChange = false;
            this.btnMenu.IsRect = false;
            this.btnMenu.IsUnGroup = true;
            this.btnMenu.Location = new System.Drawing.Point(470, 0);
            this.btnMenu.Margin = new System.Windows.Forms.Padding(0);
            this.btnMenu.Multiline = false;
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(30, 56);
            this.btnMenu.TabIndex = 26;
            this.btnMenu.TextColor = System.Drawing.Color.Black;
            this.btnMenu.UseVisualStyleBackColor = false;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // pMenu
            // 
            this.pMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.pMenu.ColumnCount = 4;
            this.pMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.pMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
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
            this.pMenu.Size = new System.Drawing.Size(470, 56);
            this.pMenu.TabIndex = 25;
            this.pMenu.SizeChanged += new System.EventHandler(this.pMenu_SizeChanged);
            // 
            // btnDelect
            // 
            this.btnDelect.AutoFont = false;
            this.btnDelect.AutoFontHeightRatio = 0.75F;
            this.btnDelect.AutoFontMax = 100F;
            this.btnDelect.AutoFontMin = 5F;
            this.btnDelect.AutoFontWidthRatio = 0.92F;
            this.btnDelect.AutoImage = true;
            this.btnDelect.AutoImageMaxRatio = 0.65F;
            this.btnDelect.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnDelect.AutoImageTint = true;
            this.btnDelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDelect.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDelect.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelect.BackgroundImage")));
            this.btnDelect.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDelect.BorderRadius = 4;
            this.btnDelect.BorderSize = 1;
            this.btnDelect.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnDelect.Corner = BeeGlobal.Corner.Both;
            this.btnDelect.DebounceResizeMs = 16;
            this.btnDelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelect.FlatAppearance.BorderSize = 0;
            this.btnDelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelect.ForeColor = System.Drawing.Color.Black;
            this.btnDelect.Image = ((System.Drawing.Image)(resources.GetObject("btnDelect.Image")));
            this.btnDelect.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDelect.ImageDisabled = null;
            this.btnDelect.ImageHover = null;
            this.btnDelect.ImageNormal = null;
            this.btnDelect.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnDelect.ImagePressed = null;
            this.btnDelect.ImageTextSpacing = 2;
            this.btnDelect.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnDelect.ImageTintHover = System.Drawing.Color.Empty;
            this.btnDelect.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnDelect.ImageTintOpacity = 0.5F;
            this.btnDelect.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnDelect.IsCLick = false;
            this.btnDelect.IsNotChange = true;
            this.btnDelect.IsRect = false;
            this.btnDelect.IsUnGroup = true;
            this.btnDelect.Location = new System.Drawing.Point(356, 4);
            this.btnDelect.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.btnDelect.Multiline = false;
            this.btnDelect.Name = "btnDelect";
            this.btnDelect.Size = new System.Drawing.Size(106, 48);
            this.btnDelect.TabIndex = 13;
            this.btnDelect.Text = "Delete ";
            this.btnDelect.TextColor = System.Drawing.Color.Black;
            this.btnDelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelect.UseVisualStyleBackColor = false;
            this.btnDelect.Click += new System.EventHandler(this.btnDelect_Click);
            // 
            // btnSave
            // 
            this.btnSave.AutoFont = false;
            this.btnSave.AutoFontHeightRatio = 0.75F;
            this.btnSave.AutoFontMax = 100F;
            this.btnSave.AutoFontMin = 5F;
            this.btnSave.AutoFontWidthRatio = 0.92F;
            this.btnSave.AutoImage = true;
            this.btnSave.AutoImageMaxRatio = 0.65F;
            this.btnSave.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSave.AutoImageTint = true;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSave.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSave.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSave.BorderRadius = 4;
            this.btnSave.BorderSize = 1;
            this.btnSave.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSave.Corner = BeeGlobal.Corner.Both;
            this.btnSave.DebounceResizeMs = 16;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.ImageDisabled = null;
            this.btnSave.ImageHover = null;
            this.btnSave.ImageNormal = null;
            this.btnSave.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSave.ImagePressed = null;
            this.btnSave.ImageTextSpacing = 2;
            this.btnSave.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSave.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSave.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSave.ImageTintOpacity = 0.5F;
            this.btnSave.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSave.IsCLick = false;
            this.btnSave.IsNotChange = true;
            this.btnSave.IsRect = false;
            this.btnSave.IsUnGroup = true;
            this.btnSave.Location = new System.Drawing.Point(124, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.btnSave.Multiline = false;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(106, 48);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Save ";
            this.btnSave.TextColor = System.Drawing.Color.Black;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.AutoFont = false;
            this.btnSaveAs.AutoFontHeightRatio = 0.75F;
            this.btnSaveAs.AutoFontMax = 100F;
            this.btnSaveAs.AutoFontMin = 5F;
            this.btnSaveAs.AutoFontWidthRatio = 0.92F;
            this.btnSaveAs.AutoImage = true;
            this.btnSaveAs.AutoImageMaxRatio = 0.65F;
            this.btnSaveAs.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSaveAs.AutoImageTint = true;
            this.btnSaveAs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSaveAs.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSaveAs.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveAs.BackgroundImage")));
            this.btnSaveAs.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSaveAs.BorderRadius = 4;
            this.btnSaveAs.BorderSize = 1;
            this.btnSaveAs.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSaveAs.Corner = BeeGlobal.Corner.Both;
            this.btnSaveAs.DebounceResizeMs = 16;
            this.btnSaveAs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveAs.FlatAppearance.BorderSize = 0;
            this.btnSaveAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveAs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveAs.ForeColor = System.Drawing.Color.Black;
            this.btnSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAs.Image")));
            this.btnSaveAs.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSaveAs.ImageDisabled = null;
            this.btnSaveAs.ImageHover = null;
            this.btnSaveAs.ImageNormal = null;
            this.btnSaveAs.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSaveAs.ImagePressed = null;
            this.btnSaveAs.ImageTextSpacing = 2;
            this.btnSaveAs.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSaveAs.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSaveAs.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSaveAs.ImageTintOpacity = 0.5F;
            this.btnSaveAs.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSaveAs.IsCLick = false;
            this.btnSaveAs.IsNotChange = true;
            this.btnSaveAs.IsRect = false;
            this.btnSaveAs.IsUnGroup = true;
            this.btnSaveAs.Location = new System.Drawing.Point(240, 4);
            this.btnSaveAs.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.btnSaveAs.Multiline = false;
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(106, 48);
            this.btnSaveAs.TabIndex = 15;
            this.btnSaveAs.Text = "Copy";
            this.btnSaveAs.TextColor = System.Drawing.Color.Black;
            this.btnSaveAs.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveAs.UseVisualStyleBackColor = false;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.AutoFont = false;
            this.btnAdd.AutoFontHeightRatio = 0.75F;
            this.btnAdd.AutoFontMax = 100F;
            this.btnAdd.AutoFontMin = 5F;
            this.btnAdd.AutoFontWidthRatio = 0.92F;
            this.btnAdd.AutoImage = true;
            this.btnAdd.AutoImageMaxRatio = 0.65F;
            this.btnAdd.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnAdd.AutoImageTint = true;
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAdd.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
            this.btnAdd.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAdd.BorderRadius = 4;
            this.btnAdd.BorderSize = 1;
            this.btnAdd.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnAdd.Corner = BeeGlobal.Corner.Both;
            this.btnAdd.DebounceResizeMs = 16;
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.Black;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAdd.ImageDisabled = null;
            this.btnAdd.ImageHover = null;
            this.btnAdd.ImageNormal = null;
            this.btnAdd.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnAdd.ImagePressed = null;
            this.btnAdd.ImageTextSpacing = 2;
            this.btnAdd.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnAdd.ImageTintHover = System.Drawing.Color.Empty;
            this.btnAdd.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnAdd.ImageTintOpacity = 0.5F;
            this.btnAdd.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnAdd.IsCLick = false;
            this.btnAdd.IsNotChange = true;
            this.btnAdd.IsRect = false;
            this.btnAdd.IsUnGroup = true;
            this.btnAdd.Location = new System.Drawing.Point(8, 4);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.btnAdd.Multiline = false;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(106, 48);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "New";
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
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "EditProg";
            this.Size = new System.Drawing.Size(500, 56);
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
            //if (Global.EditTool == null) return;
            // BeeCore.CustomGui.RoundRg(pMenu,Global.Config.RoundRad);
        }

        private void EditProg_Load(object sender, EventArgs e)
        {
            if(Global.EditTool == null)return;   
            btnMenu.PerformClick();
        }
    }
}
