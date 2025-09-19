
using BeeGlobal;
using BeeInterface;
using System.Windows.Forms;

namespace BeeUi
{
    partial class ScanCCD
    {
 
            /// <summary>
            /// Required designer variable.
            /// </summary>
            private System.ComponentModel.IContainer components = null;

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScanCCD));
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.work = new System.ComponentModel.BackgroundWorker();
            this.workConAll = new System.ComponentModel.BackgroundWorker();
            this.cbReSolution = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btnPylon = new BeeInterface.RJButton();
            this.btnUSB2_0 = new BeeInterface.RJButton();
            this.btnGigE = new BeeInterface.RJButton();
            this.btnCameraTiny = new BeeInterface.RJButton();
            this.cbCCD = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisConnect = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.lbCamera = new System.Windows.Forms.Label();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(446, 44);
            this.label2.TabIndex = 7;
            this.label2.Text = "          Setup Camera";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnClose.BackgroundImage = global::BeeUi.Properties.Resources.Delete;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnClose.Location = new System.Drawing.Point(455, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(53, 36);
            this.btnClose.TabIndex = 38;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // openFile
            // 
            this.openFile.FileName = "openFile";
            this.openFile.Filter = "KeyAcitve|*.bee";
            // 
            // work
            // 
            this.work.DoWork += new System.ComponentModel.DoWorkEventHandler(this.work_DoWork);
            this.work.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.work_RunWorkerCompleted);
            // 
            // workConAll
            // 
            this.workConAll.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workConAll_DoWork);
            this.workConAll.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workConAll_RunWorkerCompleted);
            // 
            // cbReSolution
            // 
            this.cbReSolution.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cbReSolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbReSolution.FormattingEnabled = true;
            this.cbReSolution.Items.AddRange(new object[] {
            "320x240 (0.08 MP)",
            "640x480 (0.3 MP)",
            "800x600 (0,5 MP)",
            "1024x768 (0.8 MP)",
            "1280x720 (1.3 MP)",
            "1600x1200 (1.9 MP)",
            "1920x1080 (2.1 MP)",
            "2048x1536 (3.1 MP)",
            "2592x1944 (5 MP)",
            "3840x2748 (10MP)",
            "7300 x5475 (40MP)"});
            this.cbReSolution.Location = new System.Drawing.Point(533, 225);
            this.cbReSolution.Name = "cbReSolution";
            this.cbReSolution.Size = new System.Drawing.Size(246, 32);
            this.cbReSolution.TabIndex = 42;
            this.cbReSolution.SelectedIndexChanged += new System.EventHandler(this.cbReSolution_SelectedIndexChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.btnPylon, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnUSB2_0, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnGigE, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnCameraTiny, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(136, 283);
            this.tableLayoutPanel2.TabIndex = 50;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 115F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(517, 345);
            this.tableLayoutPanel3.TabIndex = 51;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnClose, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(511, 44);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.78865F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.21135F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.panel4, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 53);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(511, 289);
            this.tableLayoutPanel5.TabIndex = 33;
            // 
            // btnPylon
            // 
            this.btnPylon.AutoFont = true;
            this.btnPylon.AutoFontHeightRatio = 0.75F;
            this.btnPylon.AutoFontMax = 100F;
            this.btnPylon.AutoFontMin = 8F;
            this.btnPylon.AutoFontWidthRatio = 0.92F;
            this.btnPylon.AutoImage = true;
            this.btnPylon.AutoImageMaxRatio = 0.75F;
            this.btnPylon.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnPylon.AutoImageTint = true;
            this.btnPylon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnPylon.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnPylon.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPylon.BackgroundImage")));
            this.btnPylon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPylon.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPylon.BorderRadius = 5;
            this.btnPylon.BorderSize = 0;
            this.btnPylon.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnPylon.Corner = BeeGlobal.Corner.Both;
            this.btnPylon.DebounceResizeMs = 16;
            this.btnPylon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPylon.FlatAppearance.BorderSize = 0;
            this.btnPylon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPylon.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnPylon.ForeColor = System.Drawing.Color.Black;
            this.btnPylon.Image = global::BeeUi.Properties.Resources.Camera1;
            this.btnPylon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPylon.ImageDisabled = null;
            this.btnPylon.ImageHover = null;
            this.btnPylon.ImageNormal = null;
            this.btnPylon.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnPylon.ImagePressed = null;
            this.btnPylon.ImageTextSpacing = 6;
            this.btnPylon.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnPylon.ImageTintHover = System.Drawing.Color.Empty;
            this.btnPylon.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnPylon.ImageTintOpacity = 0.5F;
            this.btnPylon.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnPylon.IsCLick = false;
            this.btnPylon.IsNotChange = false;
            this.btnPylon.IsRect = true;
            this.btnPylon.IsUnGroup = false;
            this.btnPylon.Location = new System.Drawing.Point(5, 145);
            this.btnPylon.Margin = new System.Windows.Forms.Padding(5);
            this.btnPylon.Multiline = false;
            this.btnPylon.Name = "btnPylon";
            this.btnPylon.Size = new System.Drawing.Size(126, 60);
            this.btnPylon.TabIndex = 50;
            this.btnPylon.Text = "Pylon";
            this.btnPylon.TextColor = System.Drawing.Color.Black;
            this.btnPylon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPylon.UseVisualStyleBackColor = false;
            this.btnPylon.Click += new System.EventHandler(this.btnPylon_Click);
            // 
            // btnUSB2_0
            // 
            this.btnUSB2_0.AutoFont = true;
            this.btnUSB2_0.AutoFontHeightRatio = 0.75F;
            this.btnUSB2_0.AutoFontMax = 100F;
            this.btnUSB2_0.AutoFontMin = 6F;
            this.btnUSB2_0.AutoFontWidthRatio = 0.92F;
            this.btnUSB2_0.AutoImage = true;
            this.btnUSB2_0.AutoImageMaxRatio = 0.75F;
            this.btnUSB2_0.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnUSB2_0.AutoImageTint = true;
            this.btnUSB2_0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnUSB2_0.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnUSB2_0.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUSB2_0.BackgroundImage")));
            this.btnUSB2_0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUSB2_0.BorderColor = System.Drawing.Color.DarkGray;
            this.btnUSB2_0.BorderRadius = 5;
            this.btnUSB2_0.BorderSize = 0;
            this.btnUSB2_0.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnUSB2_0.Corner = BeeGlobal.Corner.Both;
            this.btnUSB2_0.DebounceResizeMs = 16;
            this.btnUSB2_0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUSB2_0.FlatAppearance.BorderSize = 0;
            this.btnUSB2_0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUSB2_0.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.btnUSB2_0.ForeColor = System.Drawing.Color.Black;
            this.btnUSB2_0.Image = global::BeeUi.Properties.Resources.CameraNotConnect;
            this.btnUSB2_0.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUSB2_0.ImageDisabled = null;
            this.btnUSB2_0.ImageHover = null;
            this.btnUSB2_0.ImageNormal = null;
            this.btnUSB2_0.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnUSB2_0.ImagePressed = null;
            this.btnUSB2_0.ImageTextSpacing = 0;
            this.btnUSB2_0.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnUSB2_0.ImageTintHover = System.Drawing.Color.Empty;
            this.btnUSB2_0.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnUSB2_0.ImageTintOpacity = 0.5F;
            this.btnUSB2_0.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnUSB2_0.IsCLick = true;
            this.btnUSB2_0.IsNotChange = false;
            this.btnUSB2_0.IsRect = true;
            this.btnUSB2_0.IsUnGroup = false;
            this.btnUSB2_0.Location = new System.Drawing.Point(5, 5);
            this.btnUSB2_0.Margin = new System.Windows.Forms.Padding(5);
            this.btnUSB2_0.Multiline = false;
            this.btnUSB2_0.Name = "btnUSB2_0";
            this.btnUSB2_0.Size = new System.Drawing.Size(126, 60);
            this.btnUSB2_0.TabIndex = 47;
            this.btnUSB2_0.Text = "USB 2.0";
            this.btnUSB2_0.TextColor = System.Drawing.Color.Black;
            this.btnUSB2_0.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUSB2_0.UseVisualStyleBackColor = false;
            this.btnUSB2_0.Click += new System.EventHandler(this.btnUSB2_0_Click);
            // 
            // btnGigE
            // 
            this.btnGigE.AutoFont = true;
            this.btnGigE.AutoFontHeightRatio = 0.75F;
            this.btnGigE.AutoFontMax = 100F;
            this.btnGigE.AutoFontMin = 8F;
            this.btnGigE.AutoFontWidthRatio = 0.92F;
            this.btnGigE.AutoImage = true;
            this.btnGigE.AutoImageMaxRatio = 0.75F;
            this.btnGigE.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnGigE.AutoImageTint = true;
            this.btnGigE.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnGigE.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnGigE.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGigE.BackgroundImage")));
            this.btnGigE.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnGigE.BorderColor = System.Drawing.Color.DarkGray;
            this.btnGigE.BorderRadius = 5;
            this.btnGigE.BorderSize = 0;
            this.btnGigE.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnGigE.Corner = BeeGlobal.Corner.Both;
            this.btnGigE.DebounceResizeMs = 16;
            this.btnGigE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGigE.FlatAppearance.BorderSize = 0;
            this.btnGigE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGigE.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnGigE.ForeColor = System.Drawing.Color.Black;
            this.btnGigE.Image = global::BeeUi.Properties.Resources.Camera1;
            this.btnGigE.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGigE.ImageDisabled = null;
            this.btnGigE.ImageHover = null;
            this.btnGigE.ImageNormal = null;
            this.btnGigE.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnGigE.ImagePressed = null;
            this.btnGigE.ImageTextSpacing = 0;
            this.btnGigE.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnGigE.ImageTintHover = System.Drawing.Color.Empty;
            this.btnGigE.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnGigE.ImageTintOpacity = 0.5F;
            this.btnGigE.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnGigE.IsCLick = false;
            this.btnGigE.IsNotChange = false;
            this.btnGigE.IsRect = true;
            this.btnGigE.IsUnGroup = false;
            this.btnGigE.Location = new System.Drawing.Point(5, 75);
            this.btnGigE.Margin = new System.Windows.Forms.Padding(5);
            this.btnGigE.Multiline = false;
            this.btnGigE.Name = "btnGigE";
            this.btnGigE.Size = new System.Drawing.Size(126, 60);
            this.btnGigE.TabIndex = 48;
            this.btnGigE.Text = "MVS";
            this.btnGigE.TextColor = System.Drawing.Color.Black;
            this.btnGigE.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGigE.UseVisualStyleBackColor = false;
            this.btnGigE.Click += new System.EventHandler(this.btnGigE_Click);
            // 
            // btnCameraTiny
            // 
            this.btnCameraTiny.AutoFont = true;
            this.btnCameraTiny.AutoFontHeightRatio = 0.75F;
            this.btnCameraTiny.AutoFontMax = 100F;
            this.btnCameraTiny.AutoFontMin = 6F;
            this.btnCameraTiny.AutoFontWidthRatio = 0.92F;
            this.btnCameraTiny.AutoImage = true;
            this.btnCameraTiny.AutoImageMaxRatio = 0.75F;
            this.btnCameraTiny.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCameraTiny.AutoImageTint = true;
            this.btnCameraTiny.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCameraTiny.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCameraTiny.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCameraTiny.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCameraTiny.BorderRadius = 5;
            this.btnCameraTiny.BorderSize = 0;
            this.btnCameraTiny.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCameraTiny.Corner = BeeGlobal.Corner.Both;
            this.btnCameraTiny.DebounceResizeMs = 16;
            this.btnCameraTiny.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCameraTiny.Enabled = false;
            this.btnCameraTiny.FlatAppearance.BorderSize = 0;
            this.btnCameraTiny.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCameraTiny.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.btnCameraTiny.ForeColor = System.Drawing.Color.Black;
            this.btnCameraTiny.Image = global::BeeUi.Properties.Resources.CameraConnected;
            this.btnCameraTiny.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCameraTiny.ImageDisabled = null;
            this.btnCameraTiny.ImageHover = null;
            this.btnCameraTiny.ImageNormal = null;
            this.btnCameraTiny.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCameraTiny.ImagePressed = null;
            this.btnCameraTiny.ImageTextSpacing = 0;
            this.btnCameraTiny.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCameraTiny.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCameraTiny.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCameraTiny.ImageTintOpacity = 0.5F;
            this.btnCameraTiny.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCameraTiny.IsCLick = false;
            this.btnCameraTiny.IsNotChange = false;
            this.btnCameraTiny.IsRect = true;
            this.btnCameraTiny.IsUnGroup = false;
            this.btnCameraTiny.Location = new System.Drawing.Point(5, 215);
            this.btnCameraTiny.Margin = new System.Windows.Forms.Padding(5);
            this.btnCameraTiny.Multiline = false;
            this.btnCameraTiny.Name = "btnCameraTiny";
            this.btnCameraTiny.Size = new System.Drawing.Size(126, 63);
            this.btnCameraTiny.TabIndex = 49;
            this.btnCameraTiny.Text = "Tiny";
            this.btnCameraTiny.TextColor = System.Drawing.Color.Black;
            this.btnCameraTiny.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCameraTiny.UseVisualStyleBackColor = false;
            this.btnCameraTiny.Click += new System.EventHandler(this.btnCameraTiny_Click);
            // 
            // cbCCD
            // 
            this.cbCCD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCCD.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCCD.FormattingEnabled = true;
            this.cbCCD.ItemHeight = 24;
            this.cbCCD.Location = new System.Drawing.Point(10, 30);
            this.cbCCD.Margin = new System.Windows.Forms.Padding(10, 3, 5, 3);
            this.cbCCD.Name = "cbCCD";
            this.cbCCD.Size = new System.Drawing.Size(348, 100);
            this.cbCCD.TabIndex = 55;
            this.cbCCD.SelectedIndexChanged += new System.EventHandler(this.cbCCD_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnDisConnect, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnConnect, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 233);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(363, 50);
            this.tableLayoutPanel1.TabIndex = 52;
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.Location = new System.Drawing.Point(186, 5);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(5);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(172, 40);
            this.btnConnect.TabIndex = 33;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisConnect
            // 
            this.btnDisConnect.BackColor = System.Drawing.Color.Silver;
            this.btnDisConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDisConnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDisConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisConnect.ForeColor = System.Drawing.Color.White;
            this.btnDisConnect.Location = new System.Drawing.Point(5, 5);
            this.btnDisConnect.Margin = new System.Windows.Forms.Padding(5);
            this.btnDisConnect.Name = "btnDisConnect";
            this.btnDisConnect.Size = new System.Drawing.Size(171, 40);
            this.btnDisConnect.TabIndex = 34;
            this.btnDisConnect.Text = "DisConnect";
            this.btnDisConnect.UseVisualStyleBackColor = false;
            this.btnDisConnect.Click += new System.EventHandler(this.btnDisConnect_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.panel4.Controls.Add(this.tableLayoutPanel6);
            this.panel4.Controls.Add(this.tableLayoutPanel1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(145, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(363, 283);
            this.panel4.TabIndex = 49;
            this.panel4.Paint += new System.Windows.Forms.PaintEventHandler(this.panel4_Paint);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.lbCamera, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.cbCCD, 0, 1);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 3;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Size = new System.Drawing.Size(363, 233);
            this.tableLayoutPanel6.TabIndex = 53;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(10, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(350, 27);
            this.label3.TabIndex = 57;
            this.label3.Text = "List camera ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbCamera
            // 
            this.lbCamera.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCamera.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCamera.Location = new System.Drawing.Point(10, 143);
            this.lbCamera.Margin = new System.Windows.Forms.Padding(10);
            this.lbCamera.Name = "lbCamera";
            this.lbCamera.Size = new System.Drawing.Size(343, 80);
            this.lbCamera.TabIndex = 58;
            this.lbCamera.Text = "----";
            this.lbCamera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ScanCCD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.ClientSize = new System.Drawing.Size(527, 355);
            this.Controls.Add(this.cbReSolution);
            this.Controls.Add(this.tableLayoutPanel3);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ScanCCD";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "FormActive";
            this.Load += new System.EventHandler(this.ScanCCD_Load);
            this.Shown += new System.EventHandler(this.ScanCCD_Shown);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.ResumeLayout(false);

            }

            #endregion
            public System.Windows.Forms.Label label2;
            private System.Windows.Forms.Button btnClose;
            private System.Windows.Forms.OpenFileDialog openFile;
        private System.ComponentModel.BackgroundWorker work;
        private RJButton btnSimulation;
        public RJButton btnGigE;
        public RJButton btnUSB2_0;
        public RJButton btnCameraTiny;
        public System.Windows.Forms.ComboBox cbReSolution;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public System.ComponentModel.BackgroundWorker workConAll;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        public RJButton btnPylon;
        private ListBox cbCCD;
        private Panel panel4;
        private TableLayoutPanel tableLayoutPanel6;
        private Label label3;
        private TableLayoutPanel tableLayoutPanel1;
        private Button btnDisConnect;
        private Button btnConnect;
        private Label lbCamera;
    }
    }