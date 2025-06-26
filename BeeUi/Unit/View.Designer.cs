using BeeCore;

namespace BeeUi
{
    partial class View
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.imgView = new Cyotek.Windows.Forms.ImageBox();
            this.ckProcess = new System.Windows.Forms.CheckBox();
            this.pMenu = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnDropColor = new BeeUi.Common.RJButton();
            this.btnShowCenter = new BeeUi.Common.RJButton();
            this.btnZoomOut = new BeeUi.Common.RJButton();
            this.btnZoomIn = new BeeUi.Common.RJButton();
            this.btnFull = new BeeUi.Common.RJButton();
            this.btnGird = new BeeUi.Common.RJButton();
            this.btnShowArea = new BeeUi.Common.RJButton();
            this.pView = new System.Windows.Forms.Panel();
            this.workUndo = new System.ComponentModel.BackgroundWorker();
            this.tmTool = new System.Windows.Forms.Timer(this.components);
            this.workPlay = new System.ComponentModel.BackgroundWorker();
            this.tmPlay = new System.Windows.Forms.Timer(this.components);
            this.workReadCCD = new System.ComponentModel.BackgroundWorker();
            this.workShow = new System.ComponentModel.BackgroundWorker();
            this.tmTrig = new System.Windows.Forms.Timer(this.components);
            this.workGetColor = new System.ComponentModel.BackgroundWorker();
            this.workInsert = new System.ComponentModel.BackgroundWorker();
            this.tmCheckPort = new System.Windows.Forms.Timer(this.components);
            this.tmCheckCCD = new System.Windows.Forms.Timer(this.components);
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.tmRefresh = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmOut = new System.Windows.Forms.Timer(this.components);
            this.LayOutMain = new System.Windows.Forms.TableLayoutPanel();
            this.pBtn = new System.Windows.Forms.TableLayoutPanel();
            this.btnDeleteFile = new System.Windows.Forms.Button();
            this.btnPlayStep = new System.Windows.Forms.Button();
            this.btnFile = new System.Windows.Forms.Button();
            this.btnTypeTrig = new BeeUi.Common.RJButton();
            this.btnCap = new BeeUi.Common.RJButton();
            this.btnRecord = new BeeUi.Common.RJButton();
            this.btnLive = new BeeUi.Common.RJButton();
            this.btnMenu = new System.Windows.Forms.Button();
            this.btnShowSetting = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnFolder = new System.Windows.Forms.Button();
            this.btnRunSim = new BeeUi.Common.RJButton();
            this.LayOutShow = new System.Windows.Forms.TableLayoutPanel();
            this.pHeader = new System.Windows.Forms.Panel();
            this.tmContinuous = new System.Windows.Forms.Timer(this.components);
            this.workTrig = new System.ComponentModel.BackgroundWorker();
            this.tmPress = new System.Windows.Forms.Timer(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tmSimulation = new System.Windows.Forms.Timer(this.components);
            this.w1 = new System.ComponentModel.BackgroundWorker();
            this.w2 = new System.ComponentModel.BackgroundWorker();
            this.w3 = new System.ComponentModel.BackgroundWorker();
            this.w4 = new System.ComponentModel.BackgroundWorker();
            this.pMenu.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pView.SuspendLayout();
            this.LayOutMain.SuspendLayout();
            this.pBtn.SuspendLayout();
            this.LayOutShow.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgView
            // 
            this.imgView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imgView.AutoCenter = false;
            this.imgView.AutoPan = false;
            this.imgView.GridColor = System.Drawing.Color.Transparent;
            this.imgView.GridScale = Cyotek.Windows.Forms.ImageBoxGridScale.None;
            this.imgView.Location = new System.Drawing.Point(0, 0);
            this.imgView.Name = "imgView";
            this.imgView.PanMode = Cyotek.Windows.Forms.ImageBoxPanMode.Middle;
            this.imgView.ShortcutsEnabled = false;
            this.imgView.Size = new System.Drawing.Size(1287, 425);
            this.imgView.TabIndex = 1;
            this.imgView.TextBackColor = System.Drawing.Color.White;
            this.imgView.TextDisplayMode = Cyotek.Windows.Forms.ImageBoxGridDisplayMode.None;
            this.imgView.Visible = false;
            this.imgView.ZoomChanged += new System.EventHandler(this.imgView_ZoomChanged);
            this.imgView.Click += new System.EventHandler(this.imgView_Click_1);
            this.imgView.Paint += new System.Windows.Forms.PaintEventHandler(this.imgView_Paint);
            this.imgView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imgView_MouseDown);
            this.imgView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imgView_MouseMove);
            this.imgView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imgView_MouseUp);
            // 
            // ckProcess
            // 
            this.ckProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ckProcess.AutoSize = true;
            this.ckProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckProcess.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ckProcess.Location = new System.Drawing.Point(572, 3);
            this.ckProcess.Name = "ckProcess";
            this.ckProcess.Size = new System.Drawing.Size(271, 48);
            this.ckProcess.TabIndex = 13;
            this.ckProcess.Text = "Trigger 2";
            this.ckProcess.UseVisualStyleBackColor = true;
            this.ckProcess.CheckedChanged += new System.EventHandler(this.ckProcess_CheckedChanged);
            // 
            // pMenu
            // 
            this.pMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pMenu.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pMenu.Controls.Add(this.tableLayoutPanel1);
            this.pMenu.Location = new System.Drawing.Point(1296, 3);
            this.pMenu.Name = "pMenu";
            this.pMenu.Size = new System.Drawing.Size(47, 425);
            this.pMenu.TabIndex = 28;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnDropColor, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnShowCenter, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnZoomOut, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnZoomIn, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnFull, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnGird, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnShowArea, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(47, 325);
            this.tableLayoutPanel1.TabIndex = 37;
            // 
            // btnDropColor
            // 
            this.btnDropColor.BackColor = System.Drawing.Color.Transparent;
            this.btnDropColor.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnDropColor.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnDropColor.BorderRadius = 0;
            this.btnDropColor.BorderSize = 0;
            this.btnDropColor.ButtonImage = null;
            this.btnDropColor.Corner = BeeCore.Corner.Both;
            this.btnDropColor.FlatAppearance.BorderSize = 0;
            this.btnDropColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDropColor.ForeColor = System.Drawing.Color.White;
            this.btnDropColor.Image = global::BeeUi.Properties.Resources.Rectangular_1;
            this.btnDropColor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDropColor.IsCLick = false;
            this.btnDropColor.IsNotChange = false;
            this.btnDropColor.IsRect = false;
            this.btnDropColor.IsUnGroup = true;
            this.btnDropColor.Location = new System.Drawing.Point(3, 281);
            this.btnDropColor.Name = "btnDropColor";
            this.btnDropColor.Size = new System.Drawing.Size(40, 32);
            this.btnDropColor.TabIndex = 43;
            this.btnDropColor.TextColor = System.Drawing.Color.White;
            this.btnDropColor.UseVisualStyleBackColor = false;
            this.btnDropColor.Visible = false;
            // 
            // btnShowCenter
            // 
            this.btnShowCenter.BackColor = System.Drawing.Color.Transparent;
            this.btnShowCenter.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnShowCenter.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnShowCenter.BorderRadius = 0;
            this.btnShowCenter.BorderSize = 0;
            this.btnShowCenter.ButtonImage = null;
            this.btnShowCenter.Corner = BeeCore.Corner.Both;
            this.btnShowCenter.FlatAppearance.BorderSize = 0;
            this.btnShowCenter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowCenter.ForeColor = System.Drawing.Color.White;
            this.btnShowCenter.Image = global::BeeUi.Properties.Resources.Center_of_Gravity_1;
            this.btnShowCenter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShowCenter.IsCLick = false;
            this.btnShowCenter.IsNotChange = false;
            this.btnShowCenter.IsRect = false;
            this.btnShowCenter.IsUnGroup = true;
            this.btnShowCenter.Location = new System.Drawing.Point(3, 141);
            this.btnShowCenter.Name = "btnShowCenter";
            this.btnShowCenter.Size = new System.Drawing.Size(40, 40);
            this.btnShowCenter.TabIndex = 40;
            this.btnShowCenter.TextColor = System.Drawing.Color.White;
            this.btnShowCenter.UseVisualStyleBackColor = false;
            this.btnShowCenter.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.BackColor = System.Drawing.Color.Transparent;
            this.btnZoomOut.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnZoomOut.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnZoomOut.BorderRadius = 0;
            this.btnZoomOut.BorderSize = 0;
            this.btnZoomOut.ButtonImage = null;
            this.btnZoomOut.Corner = BeeCore.Corner.Both;
            this.btnZoomOut.FlatAppearance.BorderSize = 0;
            this.btnZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomOut.ForeColor = System.Drawing.Color.White;
            this.btnZoomOut.Image = global::BeeUi.Properties.Resources.Zoom_Out;
            this.btnZoomOut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnZoomOut.IsCLick = false;
            this.btnZoomOut.IsNotChange = true;
            this.btnZoomOut.IsRect = false;
            this.btnZoomOut.IsUnGroup = true;
            this.btnZoomOut.Location = new System.Drawing.Point(3, 95);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(40, 40);
            this.btnZoomOut.TabIndex = 39;
            this.btnZoomOut.TextColor = System.Drawing.Color.White;
            this.btnZoomOut.UseVisualStyleBackColor = false;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.BackColor = System.Drawing.Color.Transparent;
            this.btnZoomIn.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnZoomIn.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnZoomIn.BorderRadius = 0;
            this.btnZoomIn.BorderSize = 0;
            this.btnZoomIn.ButtonImage = null;
            this.btnZoomIn.Corner = BeeCore.Corner.Both;
            this.btnZoomIn.FlatAppearance.BorderSize = 0;
            this.btnZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomIn.ForeColor = System.Drawing.Color.White;
            this.btnZoomIn.Image = global::BeeUi.Properties.Resources.Zoom_In;
            this.btnZoomIn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnZoomIn.IsCLick = false;
            this.btnZoomIn.IsNotChange = true;
            this.btnZoomIn.IsRect = false;
            this.btnZoomIn.IsUnGroup = true;
            this.btnZoomIn.Location = new System.Drawing.Point(3, 49);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(40, 40);
            this.btnZoomIn.TabIndex = 38;
            this.btnZoomIn.TextColor = System.Drawing.Color.White;
            this.btnZoomIn.UseVisualStyleBackColor = false;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnFull
            // 
            this.btnFull.BackColor = System.Drawing.Color.Transparent;
            this.btnFull.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnFull.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnFull.BorderRadius = 0;
            this.btnFull.BorderSize = 0;
            this.btnFull.ButtonImage = null;
            this.btnFull.Corner = BeeCore.Corner.Both;
            this.btnFull.FlatAppearance.BorderSize = 0;
            this.btnFull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFull.ForeColor = System.Drawing.Color.White;
            this.btnFull.Image = global::BeeUi.Properties.Resources.Full_Screen;
            this.btnFull.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFull.IsCLick = false;
            this.btnFull.IsNotChange = true;
            this.btnFull.IsRect = false;
            this.btnFull.IsUnGroup = true;
            this.btnFull.Location = new System.Drawing.Point(3, 3);
            this.btnFull.Name = "btnFull";
            this.btnFull.Size = new System.Drawing.Size(40, 40);
            this.btnFull.TabIndex = 37;
            this.btnFull.TextColor = System.Drawing.Color.White;
            this.btnFull.UseVisualStyleBackColor = false;
            this.btnFull.Click += new System.EventHandler(this.btnFull_Click);
            // 
            // btnGird
            // 
            this.btnGird.BackColor = System.Drawing.Color.Transparent;
            this.btnGird.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnGird.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnGird.BorderRadius = 0;
            this.btnGird.BorderSize = 0;
            this.btnGird.ButtonImage = null;
            this.btnGird.Corner = BeeCore.Corner.Both;
            this.btnGird.FlatAppearance.BorderSize = 0;
            this.btnGird.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGird.ForeColor = System.Drawing.Color.White;
            this.btnGird.Image = global::BeeUi.Properties.Resources.Prison_1;
            this.btnGird.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGird.IsCLick = false;
            this.btnGird.IsNotChange = false;
            this.btnGird.IsRect = false;
            this.btnGird.IsUnGroup = true;
            this.btnGird.Location = new System.Drawing.Point(3, 198);
            this.btnGird.Name = "btnGird";
            this.btnGird.Size = new System.Drawing.Size(40, 39);
            this.btnGird.TabIndex = 41;
            this.btnGird.TextColor = System.Drawing.Color.White;
            this.btnGird.UseVisualStyleBackColor = false;
            this.btnGird.Click += new System.EventHandler(this.btnGird_Click_1);
            // 
            // btnShowArea
            // 
            this.btnShowArea.BackColor = System.Drawing.Color.Transparent;
            this.btnShowArea.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnShowArea.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnShowArea.BorderRadius = 0;
            this.btnShowArea.BorderSize = 0;
            this.btnShowArea.ButtonImage = null;
            this.btnShowArea.Corner = BeeCore.Corner.Both;
            this.btnShowArea.FlatAppearance.BorderSize = 0;
            this.btnShowArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowArea.ForeColor = System.Drawing.Color.White;
            this.btnShowArea.Image = global::BeeUi.Properties.Resources.Rectangular_1;
            this.btnShowArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShowArea.IsCLick = false;
            this.btnShowArea.IsNotChange = false;
            this.btnShowArea.IsRect = false;
            this.btnShowArea.IsUnGroup = true;
            this.btnShowArea.Location = new System.Drawing.Point(3, 243);
            this.btnShowArea.Name = "btnShowArea";
            this.btnShowArea.Size = new System.Drawing.Size(40, 32);
            this.btnShowArea.TabIndex = 42;
            this.btnShowArea.TextColor = System.Drawing.Color.White;
            this.btnShowArea.UseVisualStyleBackColor = false;
            // 
            // pView
            // 
            this.pView.AutoScroll = true;
            this.pView.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pView.Controls.Add(this.imgView);
            this.pView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pView.Location = new System.Drawing.Point(3, 3);
            this.pView.Name = "pView";
            this.pView.Size = new System.Drawing.Size(1287, 425);
            this.pView.TabIndex = 6;
            this.pView.MouseLeave += new System.EventHandler(this.pView_MouseLeave);
            this.pView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pView_MouseMove);
            // 
            // workUndo
            // 
            this.workUndo.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workUndo_DoWork);
            this.workUndo.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workUndo_RunWorkerCompleted);
            // 
            // tmTool
            // 
            this.tmTool.Interval = 1000;
            this.tmTool.Tick += new System.EventHandler(this.tmTool_Tick);
            // 
            // workPlay
            // 
            this.workPlay.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workPlay_DoWork);
            this.workPlay.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workPlay_RunWorkerCompleted);
            // 
            // tmPlay
            // 
            this.tmPlay.Interval = 200;
            this.tmPlay.Tick += new System.EventHandler(this.tmPlay_Tick);
            // 
            // workReadCCD
            // 
            this.workReadCCD.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workReadCCD_DoWork);
            this.workReadCCD.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workReadCCD_RunWorkerCompleted);
            // 
            // workShow
            // 
            this.workShow.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workShow_DoWork);
            this.workShow.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workShow_RunWorkerCompleted);
            // 
            // tmTrig
            // 
            this.tmTrig.Interval = 1000;
            this.tmTrig.Tick += new System.EventHandler(this.tmTrig_Tick);
            // 
            // workGetColor
            // 
            this.workGetColor.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workGetColor_DoWork);
            this.workGetColor.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workGetColor_RunWorkerCompleted);
            // 
            // workInsert
            // 
            this.workInsert.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workInsert_DoWork);
            this.workInsert.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workInsert_RunWorkerCompleted);
            // 
            // tmCheckPort
            // 
            this.tmCheckPort.Interval = 2000;
            this.tmCheckPort.Tick += new System.EventHandler(this.tmCheckPort_Tick);
            // 
            // tmCheckCCD
            // 
            this.tmCheckCCD.Interval = 1000;
            this.tmCheckCCD.Tick += new System.EventHandler(this.tmCheckCCD_Tick);
            // 
            // openFile
            // 
            this.openFile.FileName = "openFile";
            // 
            // tmRefresh
            // 
            this.tmRefresh.Interval = 30000;
            this.tmRefresh.Tick += new System.EventHandler(this.tmRefresh_Tick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // tmOut
            // 
            this.tmOut.Tick += new System.EventHandler(this.tmOut_Tick);
            // 
            // LayOutMain
            // 
            this.LayOutMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LayOutMain.ColumnCount = 1;
            this.LayOutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayOutMain.Controls.Add(this.pBtn, 0, 1);
            this.LayOutMain.Controls.Add(this.LayOutShow, 0, 2);
            this.LayOutMain.Controls.Add(this.pHeader, 0, 0);
            this.LayOutMain.Location = new System.Drawing.Point(0, 0);
            this.LayOutMain.Name = "LayOutMain";
            this.LayOutMain.RowCount = 3;
            this.LayOutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.LayOutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.LayOutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayOutMain.Size = new System.Drawing.Size(1352, 607);
            this.LayOutMain.TabIndex = 29;
            // 
            // pBtn
            // 
            this.pBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pBtn.ColumnCount = 13;
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 149F));
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pBtn.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pBtn.Controls.Add(this.btnDeleteFile, 5, 0);
            this.pBtn.Controls.Add(this.btnPlayStep, 6, 0);
            this.pBtn.Controls.Add(this.btnFile, 8, 0);
            this.pBtn.Controls.Add(this.btnTypeTrig, 3, 0);
            this.pBtn.Controls.Add(this.btnCap, 0, 0);
            this.pBtn.Controls.Add(this.btnRecord, 1, 0);
            this.pBtn.Controls.Add(this.btnLive, 2, 0);
            this.pBtn.Controls.Add(this.ckProcess, 4, 0);
            this.pBtn.Controls.Add(this.btnMenu, 12, 0);
            this.pBtn.Controls.Add(this.btnShowSetting, 11, 0);
            this.pBtn.Controls.Add(this.btnSave, 10, 0);
            this.pBtn.Controls.Add(this.btnFolder, 9, 0);
            this.pBtn.Controls.Add(this.btnRunSim, 7, 0);
            this.pBtn.Location = new System.Drawing.Point(5, 113);
            this.pBtn.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.pBtn.Name = "pBtn";
            this.pBtn.RowCount = 1;
            this.pBtn.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.pBtn.Size = new System.Drawing.Size(1342, 54);
            this.pBtn.TabIndex = 1;
            this.pBtn.SizeChanged += new System.EventHandler(this.pBtn_SizeChanged);
            this.pBtn.Paint += new System.Windows.Forms.PaintEventHandler(this.pBtn_Paint);
            // 
            // btnDeleteFile
            // 
            this.btnDeleteFile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            this.btnDeleteFile.Image = global::BeeUi.Properties.Resources.Erase;
            this.btnDeleteFile.Location = new System.Drawing.Point(849, 3);
            this.btnDeleteFile.Name = "btnDeleteFile";
            this.btnDeleteFile.Size = new System.Drawing.Size(55, 48);
            this.btnDeleteFile.TabIndex = 44;
            this.btnDeleteFile.UseVisualStyleBackColor = true;
            this.btnDeleteFile.Click += new System.EventHandler(this.btnDeleteFile_Click);
            // 
            // btnPlayStep
            // 
            this.btnPlayStep.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            this.btnPlayStep.Image = global::BeeUi.Properties.Resources.End;
            this.btnPlayStep.Location = new System.Drawing.Point(910, 3);
            this.btnPlayStep.Name = "btnPlayStep";
            this.btnPlayStep.Size = new System.Drawing.Size(60, 48);
            this.btnPlayStep.TabIndex = 43;
            this.btnPlayStep.UseVisualStyleBackColor = true;
            this.btnPlayStep.Click += new System.EventHandler(this.btnPlayStep_Click);
            // 
            // btnFile
            // 
            this.btnFile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            this.btnFile.Image = global::BeeUi.Properties.Resources.File;
            this.btnFile.Location = new System.Drawing.Point(1048, 3);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(60, 48);
            this.btnFile.TabIndex = 41;
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // btnTypeTrig
            // 
            this.btnTypeTrig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTypeTrig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnTypeTrig.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnTypeTrig.BorderColor = System.Drawing.Color.DarkGray;
            this.btnTypeTrig.BorderRadius = 5;
            this.btnTypeTrig.BorderSize = 1;
            this.btnTypeTrig.ButtonImage = null;
            this.btnTypeTrig.Corner = BeeCore.Corner.Both;
            this.btnTypeTrig.FlatAppearance.BorderSize = 0;
            this.btnTypeTrig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTypeTrig.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTypeTrig.ForeColor = System.Drawing.Color.Black;
            this.btnTypeTrig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTypeTrig.IsCLick = false;
            this.btnTypeTrig.IsNotChange = true;
            this.btnTypeTrig.IsRect = false;
            this.btnTypeTrig.IsUnGroup = true;
            this.btnTypeTrig.Location = new System.Drawing.Point(425, 5);
            this.btnTypeTrig.Margin = new System.Windows.Forms.Padding(5);
            this.btnTypeTrig.Name = "btnTypeTrig";
            this.btnTypeTrig.Size = new System.Drawing.Size(139, 44);
            this.btnTypeTrig.TabIndex = 39;
            this.btnTypeTrig.Text = "Trig Internal";
            this.btnTypeTrig.TextColor = System.Drawing.Color.Black;
            this.btnTypeTrig.UseVisualStyleBackColor = false;
            this.btnTypeTrig.Click += new System.EventHandler(this.btnTypeTrig_Click);
            // 
            // btnCap
            // 
            this.btnCap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCap.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnCap.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCap.BorderRadius = 5;
            this.btnCap.BorderSize = 1;
            this.btnCap.ButtonImage = null;
            this.btnCap.Corner = BeeCore.Corner.Both;
            this.btnCap.FlatAppearance.BorderSize = 0;
            this.btnCap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCap.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCap.ForeColor = System.Drawing.Color.Black;
            this.btnCap.Image = global::BeeUi.Properties.Resources.Natural_User_Interface_2;
            this.btnCap.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCap.IsCLick = false;
            this.btnCap.IsNotChange = false;
            this.btnCap.IsRect = false;
            this.btnCap.IsUnGroup = true;
            this.btnCap.Location = new System.Drawing.Point(5, 5);
            this.btnCap.Margin = new System.Windows.Forms.Padding(5);
            this.btnCap.Name = "btnCap";
            this.btnCap.Size = new System.Drawing.Size(130, 44);
            this.btnCap.TabIndex = 9;
            this.btnCap.Text = "Trigger";
            this.btnCap.TextColor = System.Drawing.Color.Black;
            this.btnCap.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCap.UseVisualStyleBackColor = false;
            this.btnCap.Click += new System.EventHandler(this.btnCap_Click);
            // 
            // btnRecord
            // 
            this.btnRecord.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnRecord.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnRecord.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRecord.BorderRadius = 5;
            this.btnRecord.BorderSize = 1;
            this.btnRecord.ButtonImage = null;
            this.btnRecord.Corner = BeeCore.Corner.Both;
            this.btnRecord.FlatAppearance.BorderSize = 0;
            this.btnRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.btnRecord.ForeColor = System.Drawing.Color.Black;
            this.btnRecord.Image = global::BeeUi.Properties.Resources.Play_2;
            this.btnRecord.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRecord.IsCLick = false;
            this.btnRecord.IsNotChange = false;
            this.btnRecord.IsRect = false;
            this.btnRecord.IsUnGroup = true;
            this.btnRecord.Location = new System.Drawing.Point(145, 5);
            this.btnRecord.Margin = new System.Windows.Forms.Padding(5);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(130, 44);
            this.btnRecord.TabIndex = 10;
            this.btnRecord.Text = "Continuous";
            this.btnRecord.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRecord.TextColor = System.Drawing.Color.Black;
            this.btnRecord.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRecord.UseVisualStyleBackColor = false;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // btnLive
            // 
            this.btnLive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnLive.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnLive.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLive.BorderRadius = 5;
            this.btnLive.BorderSize = 1;
            this.btnLive.ButtonImage = null;
            this.btnLive.Corner = BeeCore.Corner.Both;
            this.btnLive.Enabled = false;
            this.btnLive.FlatAppearance.BorderSize = 0;
            this.btnLive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLive.ForeColor = System.Drawing.Color.Black;
            this.btnLive.Image = global::BeeUi.Properties.Resources.Record_2;
            this.btnLive.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLive.IsCLick = false;
            this.btnLive.IsNotChange = false;
            this.btnLive.IsRect = false;
            this.btnLive.IsUnGroup = true;
            this.btnLive.Location = new System.Drawing.Point(285, 5);
            this.btnLive.Margin = new System.Windows.Forms.Padding(5);
            this.btnLive.Name = "btnLive";
            this.btnLive.Size = new System.Drawing.Size(130, 44);
            this.btnLive.TabIndex = 11;
            this.btnLive.Tag = "";
            this.btnLive.Text = "Live";
            this.btnLive.TextColor = System.Drawing.Color.Black;
            this.btnLive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLive.UseVisualStyleBackColor = false;
            this.btnLive.Click += new System.EventHandler(this.btnSer_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(173)))), ((int)(((byte)(245)))));
            this.btnMenu.Image = global::BeeUi.Properties.Resources.Menu;
            this.btnMenu.Location = new System.Drawing.Point(1296, 3);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(43, 48);
            this.btnMenu.TabIndex = 38;
            this.btnMenu.UseVisualStyleBackColor = false;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click_1);
            // 
            // btnShowSetting
            // 
            this.btnShowSetting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            this.btnShowSetting.Image = global::BeeUi.Properties.Resources.setting;
            this.btnShowSetting.Location = new System.Drawing.Point(1240, 3);
            this.btnShowSetting.Name = "btnShowSetting";
            this.btnShowSetting.Size = new System.Drawing.Size(50, 48);
            this.btnShowSetting.TabIndex = 35;
            this.btnShowSetting.UseVisualStyleBackColor = true;
            this.btnShowSetting.Click += new System.EventHandler(this.btnShowSetting_Click);
            // 
            // btnSave
            // 
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            this.btnSave.Image = global::BeeUi.Properties.Resources.Save_1;
            this.btnSave.Location = new System.Drawing.Point(1180, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(54, 48);
            this.btnSave.TabIndex = 33;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnFolder
            // 
            this.btnFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            this.btnFolder.Image = global::BeeUi.Properties.Resources.Folder;
            this.btnFolder.Location = new System.Drawing.Point(1114, 3);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(60, 48);
            this.btnFolder.TabIndex = 34;
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnImg_Click);
            // 
            // btnRunSim
            // 
            this.btnRunSim.BackColor = System.Drawing.Color.Transparent;
            this.btnRunSim.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnRunSim.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnRunSim.BorderRadius = 0;
            this.btnRunSim.BorderSize = 0;
            this.btnRunSim.ButtonImage = null;
            this.btnRunSim.Corner = BeeCore.Corner.Both;
            this.btnRunSim.Enabled = false;
            this.btnRunSim.FlatAppearance.BorderSize = 0;
            this.btnRunSim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunSim.ForeColor = System.Drawing.Color.White;
            this.btnRunSim.Image = global::BeeUi.Properties.Resources.Play_2;
            this.btnRunSim.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRunSim.IsCLick = false;
            this.btnRunSim.IsNotChange = false;
            this.btnRunSim.IsRect = false;
            this.btnRunSim.IsUnGroup = true;
            this.btnRunSim.Location = new System.Drawing.Point(976, 3);
            this.btnRunSim.Name = "btnRunSim";
            this.btnRunSim.Size = new System.Drawing.Size(66, 48);
            this.btnRunSim.TabIndex = 45;
            this.btnRunSim.TextColor = System.Drawing.Color.White;
            this.btnRunSim.UseVisualStyleBackColor = false;
            this.btnRunSim.Click += new System.EventHandler(this.btnRunSim_Click_1);
            // 
            // LayOutShow
            // 
            this.LayOutShow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LayOutShow.ColumnCount = 2;
            this.LayOutShow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayOutShow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.LayOutShow.Controls.Add(this.pMenu, 1, 0);
            this.LayOutShow.Controls.Add(this.pView, 0, 0);
            this.LayOutShow.Location = new System.Drawing.Point(3, 173);
            this.LayOutShow.Name = "LayOutShow";
            this.LayOutShow.RowCount = 1;
            this.LayOutShow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayOutShow.Size = new System.Drawing.Size(1346, 431);
            this.LayOutShow.TabIndex = 2;
            // 
            // pHeader
            // 
            this.pHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pHeader.BackColor = System.Drawing.Color.White;
            this.pHeader.Location = new System.Drawing.Point(5, 5);
            this.pHeader.Margin = new System.Windows.Forms.Padding(5);
            this.pHeader.Name = "pHeader";
            this.pHeader.Size = new System.Drawing.Size(1342, 100);
            this.pHeader.TabIndex = 3;
            this.pHeader.SizeChanged += new System.EventHandler(this.pHeader_SizeChanged);
            // 
            // tmContinuous
            // 
            this.tmContinuous.Interval = 1000;
            this.tmContinuous.Tick += new System.EventHandler(this.tmContinuous_Tick);
            // 
            // workTrig
            // 
            this.workTrig.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workTrig_DoWork);
            // 
            // tmPress
            // 
            this.tmPress.Tick += new System.EventHandler(this.tmPress_Tick);
            // 
            // tmSimulation
            // 
            this.tmSimulation.Interval = 1000;
            this.tmSimulation.Tick += new System.EventHandler(this.tmSimulation_Tick);
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.LayOutMain);
            this.Name = "View";
            this.Size = new System.Drawing.Size(1352, 607);
            this.Load += new System.EventHandler(this.View_Load);
            this.MouseHover += new System.EventHandler(this.View_MouseHover);
            this.pMenu.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pView.ResumeLayout(false);
            this.LayOutMain.ResumeLayout(false);
            this.pBtn.ResumeLayout(false);
            this.pBtn.PerformLayout();
            this.LayOutShow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Panel pView;
        private System.ComponentModel.BackgroundWorker workUndo;
        private System.Windows.Forms.Timer tmTool;
        private System.Windows.Forms.Timer tmPlay;
        private System.ComponentModel.BackgroundWorker workShow;
        private System.Windows.Forms.Timer tmTrig;
        private System.ComponentModel.BackgroundWorker workGetColor;
        public Common.RJButton btnCap;
        public Common.RJButton btnRecord;
        private System.Windows.Forms.CheckBox ckProcess;
        public Common.RJButton btnLive;
        private System.ComponentModel.BackgroundWorker workInsert;
        public System.Windows.Forms.Timer tmCheckPort;
        public System.Windows.Forms.Timer tmCheckCCD;
        private System.Windows.Forms.OpenFileDialog openFile;
        public System.ComponentModel.BackgroundWorker workReadCCD;
        public Cyotek.Windows.Forms.ImageBox imgView;
        private System.Windows.Forms.Timer tmRefresh;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        public System.Windows.Forms.Timer tmOut;
        public System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.Button btnFolder;
        public System.Windows.Forms.Button btnShowSetting;
        public System.Windows.Forms.Panel pMenu;
        public System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.TableLayoutPanel LayOutShow;
        public System.Windows.Forms.TableLayoutPanel LayOutMain;
        public System.Windows.Forms.TableLayoutPanel pBtn;
        public System.Windows.Forms.Panel pHeader;
        public System.Windows.Forms.Timer tmContinuous;
        private System.ComponentModel.BackgroundWorker workTrig;
        public System.Windows.Forms.Timer tmPress;
        public System.ComponentModel.BackgroundWorker workPlay;
        public Common.RJButton btnTypeTrig;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        public System.Windows.Forms.Button btnFile;
        public System.Windows.Forms.Timer tmSimulation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Common.RJButton btnGird;
        public Common.RJButton btnFull;
        public Common.RJButton btnZoomIn;
        public Common.RJButton btnZoomOut;
        public Common.RJButton btnShowCenter;
        public System.Windows.Forms.Button btnDeleteFile;
        public System.Windows.Forms.Button btnPlayStep;
        private Common.RJButton btnRunSim;
        public System.ComponentModel.BackgroundWorker w1;
        public System.ComponentModel.BackgroundWorker w2;
        public System.ComponentModel.BackgroundWorker w3;
        public System.ComponentModel.BackgroundWorker w4;
        private Common.RJButton btnShowArea;
        private Common.RJButton btnDropColor;
    }
}
