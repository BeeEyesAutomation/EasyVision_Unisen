using BeeCore;
using BeeGlobal;
using BeeInterface;

namespace BeeUi
{
    partial class EditTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditTool));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbLicence = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbCam = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbFrameRate = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripPort = new System.Windows.Forms.ToolStripStatusLabel();
            this.pEdit = new System.Windows.Forms.Panel();
            this.pRight = new System.Windows.Forms.Panel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pCamera = new System.Windows.Forms.Panel();
            this.btnLive = new BeeInterface.RJButton();
            this.numCam = new System.Windows.Forms.NumericUpDown();
            this.imgLive = new Cyotek.Windows.Forms.ImageBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.pName = new System.Windows.Forms.Panel();
            this.lbTool = new BeeInterface.AutoFontLabel();
            this.layInforTool = new System.Windows.Forms.TableLayoutPanel();
            this.lbRsTool = new BeeInterface.AutoFontLabel();
            this.autoFontLabel3 = new BeeInterface.AutoFontLabel();
            this.lbCTTool = new BeeInterface.AutoFontLabel();
            this.autoFontLabel2 = new BeeInterface.AutoFontLabel();
            this.iconTool = new System.Windows.Forms.PictureBox();
            this.split5 = new System.Windows.Forms.Splitter();
            this.split31 = new System.Windows.Forms.Splitter();
            this.split2 = new System.Windows.Forms.Splitter();
            this.LayoutEnd = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.split6 = new System.Windows.Forms.Splitter();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.pTop = new System.Windows.Forms.Panel();
            this.btnRJBtn = new BeeInterface.RJButton();
            this.lbBypass = new System.Windows.Forms.Label();
            this.split4 = new System.Windows.Forms.Splitter();
            this.autoFontLabel1 = new BeeInterface.AutoFontLabel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.btnLogo = new System.Windows.Forms.PictureBox();
            this.split1 = new System.Windows.Forms.Splitter();
            this.pView = new System.Windows.Forms.Panel();
            this.mouseLeft = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnNew = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsTool = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileTool = new System.Windows.Forms.ToolStripMenuItem();
            this.saveImageTool = new System.Windows.Forms.ToolStripMenuItem();
            this.btnFull = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnShowTop = new System.Windows.Forms.ToolStripMenuItem();
            this.btnShowDashBoard = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.btnShowToolBar = new System.Windows.Forms.ToolStripMenuItem();
            this.showsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.girdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.areaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Developer = new System.Windows.Forms.ToolStripMenuItem();
            this.debugTool = new System.Windows.Forms.ToolStripMenuItem();
            this.openImageTool = new System.Windows.Forms.ToolStripMenuItem();
            this.playTool = new System.Windows.Forms.ToolStripMenuItem();
            this.stopTool = new System.Windows.Forms.ToolStripMenuItem();
            this.webCamTool = new System.Windows.Forms.ToolStripMenuItem();
            this.customUIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UnlockSpiltter = new System.Windows.Forms.ToolStripMenuItem();
            this.screenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x600ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x768ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x1440ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetUI = new System.Windows.Forms.ToolStripMenuItem();
            this.customGuiTool = new System.Windows.Forms.ToolStripMenuItem();
            this.customFormLoadTool = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importTool = new System.Windows.Forms.ToolStripMenuItem();
            this.exportTool = new System.Windows.Forms.ToolStripMenuItem();
            this.split0 = new System.Windows.Forms.Splitter();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.workLoadFile = new System.ComponentModel.BackgroundWorker();
            this.tmReLoadSplit = new System.Windows.Forms.Timer(this.components);
            this.pLeft = new System.Windows.Forms.Panel();
            this.split3 = new System.Windows.Forms.Splitter();
            this.tmLoad = new System.Windows.Forms.Timer(this.components);
            this.workLive = new System.ComponentModel.BackgroundWorker();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.rjButton1 = new BeeInterface.RJButton();
            this.pInfor = new BeeUi.ViewHost();
            this.pEditTool = new BeeUi.ViewHost();
            this.BarRight = new BeeUi.Unit.Cameras();
            this.hideBar = new BeeUi.Unit.HideBar();
            this.pHeader = new BeeUi.Common.Header();
            this.BtnHeaderBar = new BeeUi.Unit.BtnHeaderBar();
            this.statusStrip1.SuspendLayout();
            this.pEdit.SuspendLayout();
            this.pRight.SuspendLayout();
            this.pCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCam)).BeginInit();
            this.pName.SuspendLayout();
            this.layInforTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconTool)).BeginInit();
            this.LayoutEnd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogo)).BeginInit();
            this.mouseLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbLicence,
            this.lbCam,
            this.lbFrameRate,
            this.toolStripPort});
            this.statusStrip1.Location = new System.Drawing.Point(193, 0);
            this.statusStrip1.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusStrip1.Size = new System.Drawing.Size(1532, 33);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbLicence
            // 
            this.lbLicence.Name = "lbLicence";
            this.lbLicence.Size = new System.Drawing.Size(47, 28);
            this.lbLicence.Text = "Licence";
            this.lbLicence.Visible = false;
            // 
            // lbCam
            // 
            this.lbCam.Image = global::BeeUi.Properties.Resources.CameraNotConnect;
            this.lbCam.Name = "lbCam";
            this.lbCam.Size = new System.Drawing.Size(48, 28);
            this.lbCam.Text = "0 fps";
            // 
            // lbFrameRate
            // 
            this.lbFrameRate.Name = "lbFrameRate";
            this.lbFrameRate.Size = new System.Drawing.Size(34, 28);
            this.lbFrameRate.Text = "0 Fps";
            // 
            // toolStripPort
            // 
            this.toolStripPort.Image = global::BeeUi.Properties.Resources.PortNotConnect;
            this.toolStripPort.Name = "toolStripPort";
            this.toolStripPort.Size = new System.Drawing.Size(102, 28);
            this.toolStripPort.Text = "PLC Not Ready";
            this.toolStripPort.Click += new System.EventHandler(this.toolStripPort_Click);
            // 
            // pEdit
            // 
            this.pEdit.AutoScroll = true;
            this.pEdit.BackColor = System.Drawing.SystemColors.Control;
            this.pEdit.Controls.Add(this.pRight);
            this.pEdit.Controls.Add(this.split5);
            this.pEdit.Controls.Add(this.BarRight);
            this.pEdit.Controls.Add(this.split31);
            this.pEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.pEdit.Location = new System.Drawing.Point(1740, 136);
            this.pEdit.MaximumSize = new System.Drawing.Size(1000, 0);
            this.pEdit.Name = "pEdit";
            this.pEdit.Size = new System.Drawing.Size(500, 1231);
            this.pEdit.TabIndex = 14;
            // 
            // pRight
            // 
            this.pRight.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pRight.Controls.Add(this.pEditTool);
            this.pRight.Controls.Add(this.splitter2);
            this.pRight.Controls.Add(this.pCamera);
            this.pRight.Controls.Add(this.pName);
            this.pRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pRight.Location = new System.Drawing.Point(0, 75);
            this.pRight.Name = "pRight";
            this.pRight.Padding = new System.Windows.Forms.Padding(4, 3, 1, 1);
            this.pRight.Size = new System.Drawing.Size(500, 1156);
            this.pRight.TabIndex = 5;
            this.pRight.SizeChanged += new System.EventHandler(this.pRight_SizeChanged);
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(4, 820);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(495, 3);
            this.splitter2.TabIndex = 2;
            this.splitter2.TabStop = false;
            // 
            // pCamera
            // 
            this.pCamera.BackColor = System.Drawing.SystemColors.Control;
            this.pCamera.Controls.Add(this.btnLive);
            this.pCamera.Controls.Add(this.numCam);
            this.pCamera.Controls.Add(this.imgLive);
            this.pCamera.Controls.Add(this.flowLayoutPanel1);
            this.pCamera.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pCamera.Location = new System.Drawing.Point(4, 823);
            this.pCamera.Name = "pCamera";
            this.pCamera.Size = new System.Drawing.Size(495, 332);
            this.pCamera.TabIndex = 0;
            this.pCamera.Visible = false;
            // 
            // btnLive
            // 
            this.btnLive.AutoFont = true;
            this.btnLive.AutoFontHeightRatio = 0.75F;
            this.btnLive.AutoFontMax = 100F;
            this.btnLive.AutoFontMin = 6F;
            this.btnLive.AutoFontWidthRatio = 0.92F;
            this.btnLive.AutoImage = true;
            this.btnLive.AutoImageMaxRatio = 0.75F;
            this.btnLive.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnLive.AutoImageTint = true;
            this.btnLive.BackColor = System.Drawing.SystemColors.Control;
            this.btnLive.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnLive.BorderColor = System.Drawing.SystemColors.Control;
            this.btnLive.BorderRadius = 14;
            this.btnLive.BorderSize = 1;
            this.btnLive.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnLive.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnLive.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnLive.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnLive.Corner = BeeGlobal.Corner.Both;
            this.btnLive.DebounceResizeMs = 16;
            this.btnLive.FlatAppearance.BorderSize = 0;
            this.btnLive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.203125F);
            this.btnLive.ForeColor = System.Drawing.Color.Black;
            this.btnLive.Image = null;
            this.btnLive.ImageDisabled = null;
            this.btnLive.ImageHover = null;
            this.btnLive.ImageNormal = null;
            this.btnLive.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnLive.ImagePressed = null;
            this.btnLive.ImageTextSpacing = 6;
            this.btnLive.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnLive.ImageTintHover = System.Drawing.Color.Empty;
            this.btnLive.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnLive.ImageTintOpacity = 0.5F;
            this.btnLive.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnLive.IsCLick = false;
            this.btnLive.IsNotChange = false;
            this.btnLive.IsRect = false;
            this.btnLive.IsTouch = false;
            this.btnLive.IsUnGroup = true;
            this.btnLive.Location = new System.Drawing.Point(369, 7);
            this.btnLive.Multiline = false;
            this.btnLive.Name = "btnLive";
            this.btnLive.Size = new System.Drawing.Size(124, 31);
            this.btnLive.TabIndex = 4;
            this.btnLive.Text = "Live";
            this.btnLive.TextColor = System.Drawing.Color.Black;
            this.btnLive.UseVisualStyleBackColor = false;
            this.btnLive.Click += new System.EventHandler(this.btnLiveCam_Click);
            // 
            // numCam
            // 
            this.numCam.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numCam.Location = new System.Drawing.Point(270, 7);
            this.numCam.Name = "numCam";
            this.numCam.Size = new System.Drawing.Size(82, 31);
            this.numCam.TabIndex = 3;
            this.numCam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // imgLive
            // 
            this.imgLive.GridDisplayMode = Cyotek.Windows.Forms.ImageBoxGridDisplayMode.None;
            this.imgLive.Location = new System.Drawing.Point(2, 44);
            this.imgLive.Name = "imgLive";
            this.imgLive.Size = new System.Drawing.Size(490, 284);
            this.imgLive.SizeMode = Cyotek.Windows.Forms.ImageBoxSizeMode.Stretch;
            this.imgLive.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(270, 802);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // pName
            // 
            this.pName.BackColor = System.Drawing.Color.Transparent;
            this.pName.Controls.Add(this.lbTool);
            this.pName.Controls.Add(this.layInforTool);
            this.pName.Controls.Add(this.iconTool);
            this.pName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pName.Location = new System.Drawing.Point(4, 3);
            this.pName.Name = "pName";
            this.pName.Size = new System.Drawing.Size(495, 44);
            this.pName.TabIndex = 0;
            this.pName.Visible = false;
            // 
            // lbTool
            // 
            this.lbTool.AutoFont = true;
            this.lbTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTool.Font = new System.Drawing.Font("Microsoft Sans Serif", 29.03125F);
            this.lbTool.Location = new System.Drawing.Point(54, 0);
            this.lbTool.Name = "lbTool";
            this.lbTool.Size = new System.Drawing.Size(262, 44);
            this.lbTool.TabIndex = 1;
            this.lbTool.Text = "Tool";
            // 
            // layInforTool
            // 
            this.layInforTool.ColumnCount = 2;
            this.layInforTool.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.11173F));
            this.layInforTool.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.88827F));
            this.layInforTool.Controls.Add(this.lbRsTool, 1, 1);
            this.layInforTool.Controls.Add(this.autoFontLabel3, 0, 1);
            this.layInforTool.Controls.Add(this.lbCTTool, 1, 0);
            this.layInforTool.Controls.Add(this.autoFontLabel2, 0, 0);
            this.layInforTool.Dock = System.Windows.Forms.DockStyle.Right;
            this.layInforTool.Location = new System.Drawing.Point(316, 0);
            this.layInforTool.Name = "layInforTool";
            this.layInforTool.RowCount = 2;
            this.layInforTool.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layInforTool.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layInforTool.Size = new System.Drawing.Size(179, 44);
            this.layInforTool.TabIndex = 2;
            // 
            // lbRsTool
            // 
            this.lbRsTool.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbRsTool.AutoFont = true;
            this.lbRsTool.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbRsTool.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.67969F);
            this.lbRsTool.ForeColor = System.Drawing.Color.White;
            this.lbRsTool.Location = new System.Drawing.Point(38, 22);
            this.lbRsTool.Name = "lbRsTool";
            this.lbRsTool.Size = new System.Drawing.Size(138, 22);
            this.lbRsTool.TabIndex = 3;
            this.lbRsTool.Text = "NC";
            this.lbRsTool.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoFontLabel3
            // 
            this.autoFontLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoFontLabel3.AutoFont = true;
            this.autoFontLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.21973F);
            this.autoFontLabel3.Location = new System.Drawing.Point(3, 22);
            this.autoFontLabel3.Name = "autoFontLabel3";
            this.autoFontLabel3.Size = new System.Drawing.Size(29, 22);
            this.autoFontLabel3.TabIndex = 2;
            this.autoFontLabel3.Text = "RS";
            // 
            // lbCTTool
            // 
            this.lbCTTool.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCTTool.AutoFont = true;
            this.lbCTTool.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbCTTool.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.67969F);
            this.lbCTTool.Location = new System.Drawing.Point(38, 0);
            this.lbCTTool.Name = "lbCTTool";
            this.lbCTTool.Size = new System.Drawing.Size(138, 22);
            this.lbCTTool.TabIndex = 1;
            this.lbCTTool.Text = "---";
            this.lbCTTool.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autoFontLabel2
            // 
            this.autoFontLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoFontLabel2.AutoFont = true;
            this.autoFontLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.80371F);
            this.autoFontLabel2.Location = new System.Drawing.Point(3, 0);
            this.autoFontLabel2.Name = "autoFontLabel2";
            this.autoFontLabel2.Size = new System.Drawing.Size(29, 22);
            this.autoFontLabel2.TabIndex = 0;
            this.autoFontLabel2.Text = "CT";
            this.autoFontLabel2.Click += new System.EventHandler(this.autoFontLabel2_Click);
            // 
            // iconTool
            // 
            this.iconTool.BackgroundImage = global::BeeUi.Properties.Resources.Add;
            this.iconTool.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.iconTool.Dock = System.Windows.Forms.DockStyle.Left;
            this.iconTool.Location = new System.Drawing.Point(0, 0);
            this.iconTool.Name = "iconTool";
            this.iconTool.Size = new System.Drawing.Size(54, 44);
            this.iconTool.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.iconTool.TabIndex = 0;
            this.iconTool.TabStop = false;
            // 
            // split5
            // 
            this.split5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.split5.Dock = System.Windows.Forms.DockStyle.Top;
            this.split5.Location = new System.Drawing.Point(0, 72);
            this.split5.Name = "split5";
            this.split5.Size = new System.Drawing.Size(500, 3);
            this.split5.TabIndex = 4;
            this.split5.TabStop = false;
            // 
            // split31
            // 
            this.split31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.split31.Dock = System.Windows.Forms.DockStyle.Top;
            this.split31.Enabled = false;
            this.split31.Location = new System.Drawing.Point(0, 0);
            this.split31.Name = "split31";
            this.split31.Size = new System.Drawing.Size(500, 5);
            this.split31.TabIndex = 25;
            this.split31.TabStop = false;
            // 
            // split2
            // 
            this.split2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.split2.Dock = System.Windows.Forms.DockStyle.Right;
            this.split2.Location = new System.Drawing.Point(1735, 136);
            this.split2.MaximumSize = new System.Drawing.Size(200, 2000);
            this.split2.Name = "split2";
            this.split2.Size = new System.Drawing.Size(5, 1231);
            this.split2.TabIndex = 2;
            this.split2.TabStop = false;
            // 
            // LayoutEnd
            // 
            this.LayoutEnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(0)))), ((int)(((byte)(30)))), ((int)(((byte)(116)))));
            this.LayoutEnd.Controls.Add(this.statusStrip1);
            this.LayoutEnd.Controls.Add(this.label1);
            this.LayoutEnd.Controls.Add(this.split6);
            this.LayoutEnd.Controls.Add(this.hideBar);
            this.LayoutEnd.Controls.Add(this.label3);
            this.LayoutEnd.Controls.Add(this.pictureBox1);
            this.LayoutEnd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LayoutEnd.Location = new System.Drawing.Point(0, 1367);
            this.LayoutEnd.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.LayoutEnd.Name = "LayoutEnd";
            this.LayoutEnd.Size = new System.Drawing.Size(2240, 33);
            this.LayoutEnd.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(132, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 33);
            this.label1.TabIndex = 5;
            this.label1.Text = "1.1.8";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // split6
            // 
            this.split6.BackColor = System.Drawing.SystemColors.Control;
            this.split6.Dock = System.Windows.Forms.DockStyle.Right;
            this.split6.Location = new System.Drawing.Point(1725, 0);
            this.split6.Name = "split6";
            this.split6.Size = new System.Drawing.Size(5, 33);
            this.split6.TabIndex = 4;
            this.split6.TabStop = false;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(55)))), ((int)(((byte)(125)))));
            this.label3.Location = new System.Drawing.Point(121, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 33);
            this.label3.TabIndex = 2;
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::BeeUi.Properties.Resources.EASY_LONG;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(121, 33);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(247, 23);
            this.button1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(0, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(247, 23);
            this.button2.TabIndex = 0;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(247, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(247, 23);
            this.button3.TabIndex = 0;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(206, 0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(206, 23);
            this.button4.TabIndex = 0;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(206, 0);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(206, 23);
            this.button5.TabIndex = 0;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(0, 0);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(206, 23);
            this.button6.TabIndex = 0;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(0, 0);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(206, 23);
            this.button7.TabIndex = 0;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(412, 0);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(206, 23);
            this.button8.TabIndex = 0;
            // 
            // pTop
            // 
            this.pTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(30)))), ((int)(((byte)(116)))));
            this.pTop.Controls.Add(this.btnRJBtn);
            this.pTop.Controls.Add(this.lbBypass);
            this.pTop.Controls.Add(this.split4);
            this.pTop.Controls.Add(this.autoFontLabel1);
            this.pTop.Controls.Add(this.BtnHeaderBar);
            this.pTop.Controls.Add(this.splitter1);
            this.pTop.Controls.Add(this.btnLogo);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(2240, 51);
            this.pTop.TabIndex = 22;
            // 
            // btnRJBtn
            // 
            this.btnRJBtn.AutoFont = true;
            this.btnRJBtn.AutoFontHeightRatio = 0.75F;
            this.btnRJBtn.AutoFontMax = 100F;
            this.btnRJBtn.AutoFontMin = 6F;
            this.btnRJBtn.AutoFontWidthRatio = 0.92F;
            this.btnRJBtn.AutoImage = true;
            this.btnRJBtn.AutoImageMaxRatio = 0.75F;
            this.btnRJBtn.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnRJBtn.AutoImageTint = true;
            this.btnRJBtn.BackColor = System.Drawing.SystemColors.Control;
            this.btnRJBtn.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnRJBtn.BorderColor = System.Drawing.SystemColors.Control;
            this.btnRJBtn.BorderRadius = 14;
            this.btnRJBtn.BorderSize = 1;
            this.btnRJBtn.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnRJBtn.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnRJBtn.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnRJBtn.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnRJBtn.Corner = BeeGlobal.Corner.None;
            this.btnRJBtn.DebounceResizeMs = 6;
            this.btnRJBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRJBtn.FlatAppearance.BorderSize = 0;
            this.btnRJBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRJBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.8125F);
            this.btnRJBtn.ForeColor = System.Drawing.Color.Black;
            this.btnRJBtn.Image = null;
            this.btnRJBtn.ImageDisabled = null;
            this.btnRJBtn.ImageHover = null;
            this.btnRJBtn.ImageNormal = null;
            this.btnRJBtn.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnRJBtn.ImagePressed = null;
            this.btnRJBtn.ImageTextSpacing = 6;
            this.btnRJBtn.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnRJBtn.ImageTintHover = System.Drawing.Color.Empty;
            this.btnRJBtn.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnRJBtn.ImageTintOpacity = 0.5F;
            this.btnRJBtn.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnRJBtn.IsCLick = false;
            this.btnRJBtn.IsNotChange = true;
            this.btnRJBtn.IsRect = false;
            this.btnRJBtn.IsTouch = false;
            this.btnRJBtn.IsUnGroup = true;
            this.btnRJBtn.Location = new System.Drawing.Point(1503, 0);
            this.btnRJBtn.Multiline = false;
            this.btnRJBtn.Name = "btnRJBtn";
            this.btnRJBtn.Size = new System.Drawing.Size(129, 51);
            this.btnRJBtn.TabIndex = 15;
            this.btnRJBtn.Text = "KeyBoard";
            this.btnRJBtn.TextColor = System.Drawing.Color.Black;
            this.btnRJBtn.UseVisualStyleBackColor = false;
            this.btnRJBtn.Click += new System.EventHandler(this.btnRJBtn_Click);
            // 
            // lbBypass
            // 
            this.lbBypass.BackColor = System.Drawing.Color.LightGreen;
            this.lbBypass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbBypass.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbBypass.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBypass.ForeColor = System.Drawing.Color.White;
            this.lbBypass.Location = new System.Drawing.Point(1632, 0);
            this.lbBypass.Name = "lbBypass";
            this.lbBypass.Size = new System.Drawing.Size(98, 51);
            this.lbBypass.TabIndex = 11;
            this.lbBypass.Text = "ByPass";
            this.lbBypass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbBypass.Visible = false;
            // 
            // split4
            // 
            this.split4.Dock = System.Windows.Forms.DockStyle.Right;
            this.split4.Location = new System.Drawing.Point(1730, 0);
            this.split4.MaximumSize = new System.Drawing.Size(200, 2000);
            this.split4.Name = "split4";
            this.split4.Size = new System.Drawing.Size(5, 51);
            this.split4.TabIndex = 10;
            this.split4.TabStop = false;
            // 
            // autoFontLabel1
            // 
            this.autoFontLabel1.AutoFont = true;
            this.autoFontLabel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.autoFontLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 32.24316F, System.Drawing.FontStyle.Bold);
            this.autoFontLabel1.ForeColor = System.Drawing.Color.White;
            this.autoFontLabel1.Location = new System.Drawing.Point(57, 0);
            this.autoFontLabel1.Name = "autoFontLabel1";
            this.autoFontLabel1.Size = new System.Drawing.Size(309, 51);
            this.autoFontLabel1.TabIndex = 9;
            this.autoFontLabel1.Text = "Easy Vision";
            this.autoFontLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(54, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 51);
            this.splitter1.TabIndex = 13;
            this.splitter1.TabStop = false;
            // 
            // btnLogo
            // 
            this.btnLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnLogo.Image = global::BeeUi.Properties.Resources.EASY1;
            this.btnLogo.Location = new System.Drawing.Point(0, 0);
            this.btnLogo.Name = "btnLogo";
            this.btnLogo.Size = new System.Drawing.Size(54, 51);
            this.btnLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnLogo.TabIndex = 14;
            this.btnLogo.TabStop = false;
            this.btnLogo.Click += new System.EventHandler(this.btnLogo_Click_1);
            // 
            // split1
            // 
            this.split1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.split1.Dock = System.Windows.Forms.DockStyle.Top;
            this.split1.Location = new System.Drawing.Point(0, 136);
            this.split1.Name = "split1";
            this.split1.Size = new System.Drawing.Size(1735, 5);
            this.split1.TabIndex = 24;
            this.split1.TabStop = false;
            // 
            // pView
            // 
            this.pView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pView.Location = new System.Drawing.Point(5, 252);
            this.pView.Margin = new System.Windows.Forms.Padding(1);
            this.pView.Name = "pView";
            this.pView.Size = new System.Drawing.Size(1730, 1115);
            this.pView.TabIndex = 27;
            this.pView.SizeChanged += new System.EventHandler(this.pView_SizeChanged);
            // 
            // mouseLeft
            // 
            this.mouseLeft.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mouseLeft.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.mouseLeft.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.saveToolStrip,
            this.saveAsTool,
            this.openFileTool,
            this.saveImageTool,
            this.btnFull,
            this.toolStripMenuItem1,
            this.showsToolStripMenuItem,
            this.Developer,
            this.customUIToolStripMenuItem,
            this.fileToolStripMenuItem});
            this.mouseLeft.Name = "contextMenuStrip2";
            this.mouseLeft.Size = new System.Drawing.Size(297, 396);
            // 
            // btnNew
            // 
            this.btnNew.Image = global::BeeUi.Properties.Resources.Add;
            this.btnNew.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.btnNew.Name = "btnNew";
            this.btnNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.btnNew.Size = new System.Drawing.Size(296, 32);
            this.btnNew.Text = "New";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // saveToolStrip
            // 
            this.saveToolStrip.Image = global::BeeUi.Properties.Resources.Save;
            this.saveToolStrip.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.saveToolStrip.Name = "saveToolStrip";
            this.saveToolStrip.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStrip.Size = new System.Drawing.Size(296, 32);
            this.saveToolStrip.Text = "Save";
            this.saveToolStrip.Click += new System.EventHandler(this.saveToolStrip_Click);
            // 
            // saveAsTool
            // 
            this.saveAsTool.Image = global::BeeUi.Properties.Resources.Save_as;
            this.saveAsTool.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.saveAsTool.Name = "saveAsTool";
            this.saveAsTool.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsTool.Size = new System.Drawing.Size(296, 32);
            this.saveAsTool.Text = "Save As";
            this.saveAsTool.Click += new System.EventHandler(this.saveAsToolStripMenuItem1_Click);
            // 
            // openFileTool
            // 
            this.openFileTool.Image = global::BeeUi.Properties.Resources.Image;
            this.openFileTool.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.openFileTool.Name = "openFileTool";
            this.openFileTool.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openFileTool.Size = new System.Drawing.Size(296, 32);
            this.openFileTool.Text = "Open Image";
            this.openFileTool.Click += new System.EventHandler(this.openFileTool_Click);
            // 
            // saveImageTool
            // 
            this.saveImageTool.Image = global::BeeUi.Properties.Resources.HD_1080p;
            this.saveImageTool.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.saveImageTool.Name = "saveImageTool";
            this.saveImageTool.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Space)));
            this.saveImageTool.Size = new System.Drawing.Size(296, 32);
            this.saveImageTool.Text = "Save Image";
            this.saveImageTool.Click += new System.EventHandler(this.saveImageTool_Click);
            // 
            // btnFull
            // 
            this.btnFull.Image = global::BeeUi.Properties.Resources.Full_Screen;
            this.btnFull.Name = "btnFull";
            this.btnFull.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.btnFull.Size = new System.Drawing.Size(296, 32);
            this.btnFull.Text = "Full Screen";
            this.btnFull.Click += new System.EventHandler(this.btnFull_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnShowTop,
            this.btnShowDashBoard,
            this.btnMenu,
            this.btnShowToolBar});
            this.toolStripMenuItem1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(296, 32);
            this.toolStripMenuItem1.Text = "View";
            // 
            // btnShowTop
            // 
            this.btnShowTop.Name = "btnShowTop";
            this.btnShowTop.Size = new System.Drawing.Size(180, 26);
            this.btnShowTop.Text = "Top Bar";
            this.btnShowTop.Click += new System.EventHandler(this.btnShowTop_Click);
            // 
            // btnShowDashBoard
            // 
            this.btnShowDashBoard.Name = "btnShowDashBoard";
            this.btnShowDashBoard.Size = new System.Drawing.Size(180, 26);
            this.btnShowDashBoard.Text = "Dash Board";
            this.btnShowDashBoard.Click += new System.EventHandler(this.btnShowDashBoard_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(180, 26);
            this.btnMenu.Text = "Menu Bar";
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click_1);
            // 
            // btnShowToolBar
            // 
            this.btnShowToolBar.Name = "btnShowToolBar";
            this.btnShowToolBar.Size = new System.Drawing.Size(180, 26);
            this.btnShowToolBar.Text = "Tool Bar";
            this.btnShowToolBar.Click += new System.EventHandler(this.btnShowToolBar_Click);
            // 
            // showsToolStripMenuItem
            // 
            this.showsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.centerToolStripMenuItem,
            this.girdToolStripMenuItem,
            this.areaToolStripMenuItem});
            this.showsToolStripMenuItem.Name = "showsToolStripMenuItem";
            this.showsToolStripMenuItem.Size = new System.Drawing.Size(296, 32);
            this.showsToolStripMenuItem.Text = "Shows";
            // 
            // centerToolStripMenuItem
            // 
            this.centerToolStripMenuItem.Name = "centerToolStripMenuItem";
            this.centerToolStripMenuItem.Size = new System.Drawing.Size(137, 26);
            this.centerToolStripMenuItem.Text = "Center";
            // 
            // girdToolStripMenuItem
            // 
            this.girdToolStripMenuItem.Name = "girdToolStripMenuItem";
            this.girdToolStripMenuItem.Size = new System.Drawing.Size(137, 26);
            this.girdToolStripMenuItem.Text = "Gird";
            // 
            // areaToolStripMenuItem
            // 
            this.areaToolStripMenuItem.Name = "areaToolStripMenuItem";
            this.areaToolStripMenuItem.Size = new System.Drawing.Size(137, 26);
            this.areaToolStripMenuItem.Text = "Area";
            // 
            // Developer
            // 
            this.Developer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debugTool,
            this.openImageTool,
            this.playTool,
            this.stopTool,
            this.webCamTool});
            this.Developer.Image = ((System.Drawing.Image)(resources.GetObject("Developer.Image")));
            this.Developer.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.Developer.Name = "Developer";
            this.Developer.Size = new System.Drawing.Size(296, 32);
            this.Developer.Text = "Developer";
            // 
            // debugTool
            // 
            this.debugTool.Name = "debugTool";
            this.debugTool.Size = new System.Drawing.Size(205, 32);
            this.debugTool.Text = "Debug";
            this.debugTool.Click += new System.EventHandler(this.debugTool_Click);
            // 
            // openImageTool
            // 
            this.openImageTool.Image = global::BeeUi.Properties.Resources.Folder;
            this.openImageTool.Name = "openImageTool";
            this.openImageTool.Size = new System.Drawing.Size(205, 32);
            this.openImageTool.Text = "Open Images";
            this.openImageTool.Click += new System.EventHandler(this.openImageTool_Click);
            // 
            // playTool
            // 
            this.playTool.Enabled = false;
            this.playTool.Image = global::BeeUi.Properties.Resources.Play;
            this.playTool.Name = "playTool";
            this.playTool.Size = new System.Drawing.Size(205, 32);
            this.playTool.Text = "Play";
            this.playTool.Click += new System.EventHandler(this.playTool_Click);
            // 
            // stopTool
            // 
            this.stopTool.Image = global::BeeUi.Properties.Resources.Stop;
            this.stopTool.Name = "stopTool";
            this.stopTool.Size = new System.Drawing.Size(205, 32);
            this.stopTool.Text = "Stop";
            this.stopTool.Click += new System.EventHandler(this.stopTool_Click);
            // 
            // webCamTool
            // 
            this.webCamTool.CheckOnClick = true;
            this.webCamTool.Image = global::BeeUi.Properties.Resources.Camera1;
            this.webCamTool.Name = "webCamTool";
            this.webCamTool.Size = new System.Drawing.Size(205, 32);
            this.webCamTool.Text = "WebCam";
            this.webCamTool.Click += new System.EventHandler(this.webCamTool_Click);
            // 
            // customUIToolStripMenuItem
            // 
            this.customUIToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UnlockSpiltter,
            this.screenToolStripMenuItem,
            this.resetUI,
            this.customGuiTool,
            this.customFormLoadTool});
            this.customUIToolStripMenuItem.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.customUIToolStripMenuItem.Name = "customUIToolStripMenuItem";
            this.customUIToolStripMenuItem.Size = new System.Drawing.Size(296, 32);
            this.customUIToolStripMenuItem.Text = "Custom UI";
            // 
            // UnlockSpiltter
            // 
            this.UnlockSpiltter.Name = "UnlockSpiltter";
            this.UnlockSpiltter.Size = new System.Drawing.Size(239, 26);
            this.UnlockSpiltter.Text = "Unlock";
            this.UnlockSpiltter.Click += new System.EventHandler(this.UnlockSpiltter_Click);
            // 
            // screenToolStripMenuItem
            // 
            this.screenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.x600ToolStripMenuItem,
            this.x768ToolStripMenuItem,
            this.x1440ToolStripMenuItem});
            this.screenToolStripMenuItem.Enabled = false;
            this.screenToolStripMenuItem.Name = "screenToolStripMenuItem";
            this.screenToolStripMenuItem.Size = new System.Drawing.Size(239, 26);
            this.screenToolStripMenuItem.Text = "Screen";
            // 
            // x600ToolStripMenuItem
            // 
            this.x600ToolStripMenuItem.Name = "x600ToolStripMenuItem";
            this.x600ToolStripMenuItem.Size = new System.Drawing.Size(177, 26);
            this.x600ToolStripMenuItem.Text = "800x600";
            // 
            // x768ToolStripMenuItem
            // 
            this.x768ToolStripMenuItem.Name = "x768ToolStripMenuItem";
            this.x768ToolStripMenuItem.Size = new System.Drawing.Size(177, 26);
            this.x768ToolStripMenuItem.Text = "1024x768";
            // 
            // x1440ToolStripMenuItem
            // 
            this.x1440ToolStripMenuItem.Name = "x1440ToolStripMenuItem";
            this.x1440ToolStripMenuItem.Size = new System.Drawing.Size(177, 26);
            this.x1440ToolStripMenuItem.Text = "1920x1440";
            // 
            // resetUI
            // 
            this.resetUI.Name = "resetUI";
            this.resetUI.Size = new System.Drawing.Size(239, 26);
            this.resetUI.Text = "Reset Default";
            this.resetUI.Click += new System.EventHandler(this.resetUI_Click);
            // 
            // customGuiTool
            // 
            this.customGuiTool.Name = "customGuiTool";
            this.customGuiTool.Size = new System.Drawing.Size(239, 26);
            this.customGuiTool.Text = "Custom Gui";
            this.customGuiTool.Click += new System.EventHandler(this.customGuiTool_Click);
            // 
            // customFormLoadTool
            // 
            this.customFormLoadTool.Name = "customFormLoadTool";
            this.customFormLoadTool.Size = new System.Drawing.Size(239, 26);
            this.customFormLoadTool.Text = "Custom FormLoad";
            this.customFormLoadTool.Click += new System.EventHandler(this.customFormLoadTool_Click);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importTool,
            this.exportTool});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(296, 32);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // importTool
            // 
            this.importTool.Name = "importTool";
            this.importTool.Size = new System.Drawing.Size(135, 26);
            this.importTool.Text = "Import";
            this.importTool.Click += new System.EventHandler(this.importTool_Click);
            // 
            // exportTool
            // 
            this.exportTool.Name = "exportTool";
            this.exportTool.Size = new System.Drawing.Size(135, 26);
            this.exportTool.Text = "Export";
            this.exportTool.Click += new System.EventHandler(this.exportTool_Click);
            // 
            // split0
            // 
            this.split0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.split0.Dock = System.Windows.Forms.DockStyle.Top;
            this.split0.Location = new System.Drawing.Point(0, 51);
            this.split0.Name = "split0";
            this.split0.Size = new System.Drawing.Size(2240, 5);
            this.split0.TabIndex = 28;
            this.split0.TabStop = false;
            // 
            // openFile
            // 
            this.openFile.FileName = "openFile";
            // 
            // workLoadFile
            // 
            this.workLoadFile.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workLoadFile_DoWork);
            this.workLoadFile.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workLoadFile_RunWorkerCompleted);
            // 
            // tmReLoadSplit
            // 
            this.tmReLoadSplit.Interval = 1000;
            this.tmReLoadSplit.Tick += new System.EventHandler(this.tmReLoadSplit_Tick);
            // 
            // pLeft
            // 
            this.pLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pLeft.Location = new System.Drawing.Point(0, 141);
            this.pLeft.Name = "pLeft";
            this.pLeft.Size = new System.Drawing.Size(5, 1226);
            this.pLeft.TabIndex = 29;
            // 
            // split3
            // 
            this.split3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.split3.Dock = System.Windows.Forms.DockStyle.Top;
            this.split3.Location = new System.Drawing.Point(5, 247);
            this.split3.Name = "split3";
            this.split3.Size = new System.Drawing.Size(1730, 5);
            this.split3.TabIndex = 30;
            this.split3.TabStop = false;
            // 
            // tmLoad
            // 
            this.tmLoad.Interval = 5000;
            this.tmLoad.Tick += new System.EventHandler(this.tmLoad_Tick);
            // 
            // workLive
            // 
            this.workLive.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workLive_DoWork);
            this.workLive.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workLive_RunWorkerCompleted);
            // 
            // rjButton1
            // 
            this.rjButton1.AutoFont = false;
            this.rjButton1.AutoFontHeightRatio = 0.6F;
            this.rjButton1.AutoFontMax = 100F;
            this.rjButton1.AutoFontMin = 6F;
            this.rjButton1.AutoFontWidthRatio = 0.92F;
            this.rjButton1.AutoImage = true;
            this.rjButton1.AutoImageMaxRatio = 0.75F;
            this.rjButton1.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton1.AutoImageTint = true;
            this.rjButton1.BackColor = System.Drawing.Color.Transparent;
            this.rjButton1.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton1.BorderColor = System.Drawing.Color.Gray;
            this.rjButton1.BorderRadius = 0;
            this.rjButton1.BorderSize = 0;
            this.rjButton1.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.rjButton1.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.rjButton1.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.rjButton1.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton1.Corner = BeeGlobal.Corner.Both;
            this.rjButton1.DebounceResizeMs = 16;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.ForeColor = System.Drawing.Color.White;
            this.rjButton1.Image = null;
            this.rjButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton1.ImageDisabled = null;
            this.rjButton1.ImageHover = null;
            this.rjButton1.ImageNormal = null;
            this.rjButton1.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton1.ImagePressed = null;
            this.rjButton1.ImageTextSpacing = 6;
            this.rjButton1.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton1.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton1.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton1.ImageTintOpacity = 0.5F;
            this.rjButton1.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton1.IsCLick = false;
            this.rjButton1.IsNotChange = false;
            this.rjButton1.IsRect = false;
            this.rjButton1.IsTouch = false;
            this.rjButton1.IsUnGroup = false;
            this.rjButton1.Location = new System.Drawing.Point(35, 4);
            this.rjButton1.Multiline = false;
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(150, 40);
            this.rjButton1.TabIndex = 2;
            this.rjButton1.Text = "rjButton1";
            this.rjButton1.TextColor = System.Drawing.Color.White;
            this.rjButton1.UseVisualStyleBackColor = false;
            // 
            // pInfor
            // 
            this.pInfor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.pInfor.Dock = System.Windows.Forms.DockStyle.Top;
            this.pInfor.Location = new System.Drawing.Point(5, 141);
            this.pInfor.Margin = new System.Windows.Forms.Padding(1);
            this.pInfor.Name = "pInfor";
            this.pInfor.Size = new System.Drawing.Size(1730, 106);
            this.pInfor.TabIndex = 25;
            this.pInfor.SizeChanged += new System.EventHandler(this.pInfor_SizeChanged);
            // 
            // pEditTool
            // 
            this.pEditTool.BackColor = System.Drawing.Color.Transparent;
            this.pEditTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pEditTool.Location = new System.Drawing.Point(4, 47);
            this.pEditTool.Name = "pEditTool";
            this.pEditTool.Size = new System.Drawing.Size(495, 773);
            this.pEditTool.TabIndex = 1;
            // 
            // BarRight
            // 
            this.BarRight.BackColor = System.Drawing.Color.White;
            this.BarRight.Dock = System.Windows.Forms.DockStyle.Top;
            this.BarRight.Location = new System.Drawing.Point(0, 5);
            this.BarRight.Margin = new System.Windows.Forms.Padding(0);
            this.BarRight.Name = "BarRight";
            this.BarRight.Size = new System.Drawing.Size(500, 67);
            this.BarRight.TabIndex = 3;
            // 
            // hideBar
            // 
            this.hideBar.BackColor = System.Drawing.Color.White;
            this.hideBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.hideBar.Location = new System.Drawing.Point(1730, 0);
            this.hideBar.Name = "hideBar";
            this.hideBar.Size = new System.Drawing.Size(510, 33);
            this.hideBar.TabIndex = 3;
            // 
            // pHeader
            // 
            this.pHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.pHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pHeader.Location = new System.Drawing.Point(0, 56);
            this.pHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pHeader.MaximumSize = new System.Drawing.Size(3000, 90);
            this.pHeader.Name = "pHeader";
            this.pHeader.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.pHeader.Size = new System.Drawing.Size(2240, 80);
            this.pHeader.TabIndex = 22;
            // 
            // BtnHeaderBar
            // 
            this.BtnHeaderBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.BtnHeaderBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnHeaderBar.Location = new System.Drawing.Point(1735, 0);
            this.BtnHeaderBar.Margin = new System.Windows.Forms.Padding(0);
            this.BtnHeaderBar.Name = "BtnHeaderBar";
            this.BtnHeaderBar.Size = new System.Drawing.Size(505, 51);
            this.BtnHeaderBar.TabIndex = 8;
            // 
            // EditTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.Controls.Add(this.pView);
            this.Controls.Add(this.split3);
            this.Controls.Add(this.pInfor);
            this.Controls.Add(this.pLeft);
            this.Controls.Add(this.split1);
            this.Controls.Add(this.split2);
            this.Controls.Add(this.pEdit);
            this.Controls.Add(this.LayoutEnd);
            this.Controls.Add(this.pHeader);
            this.Controls.Add(this.split0);
            this.Controls.Add(this.pTop);
            this.DoubleBuffered = true;
            this.Name = "EditTool";
            this.Size = new System.Drawing.Size(2240, 1400);
            this.Load += new System.EventHandler(this.EditTool_Load);
            this.SizeChanged += new System.EventHandler(this.EditTool_SizeChanged);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pEdit.ResumeLayout(false);
            this.pRight.ResumeLayout(false);
            this.pCamera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numCam)).EndInit();
            this.pName.ResumeLayout(false);
            this.layInforTool.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconTool)).EndInit();
            this.LayoutEnd.ResumeLayout(false);
            this.LayoutEnd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnLogo)).EndInit();
            this.mouseLeft.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ToolStripStatusLabel lbCam;
        public System.Windows.Forms.ToolStripStatusLabel toolStripPort;
        public System.Windows.Forms.ToolStripStatusLabel lbFrameRate;
        public System.Windows.Forms.Panel pEdit;
        public ViewHost pEditTool;
        public System.Windows.Forms.Panel pName;
        public System.Windows.Forms.PictureBox iconTool;
        private System.Windows.Forms.PictureBox pictureBox1;
        public Unit.BtnHeaderBar BtnHeaderBar;
        private System.Windows.Forms.Label label3;

        private RJButton rjButton1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Splitter split2;
        public System.Windows.Forms.Panel LayoutEnd;
        private AutoFontLabel autoFontLabel1;
        public Common.Header pHeader;
        private System.Windows.Forms.Splitter split1;
        public System.Windows.Forms.Panel pView;
        private System.Windows.Forms.Splitter split0;
        private System.Windows.Forms.Splitter split4;
        public System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.ToolStripStatusLabel lbLicence;
        public Unit.HideBar hideBar;
        private System.Windows.Forms.Splitter split6;
        private System.Windows.Forms.Splitter split5;
        public AutoFontLabel lbTool;
        private System.Windows.Forms.ContextMenuStrip mouseLeft;
        private System.Windows.Forms.ToolStripMenuItem btnNew;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem btnShowTop;
        private System.Windows.Forms.ToolStripMenuItem btnShowDashBoard;
        private System.Windows.Forms.ToolStripMenuItem btnMenu;
        private System.Windows.Forms.ToolStripMenuItem btnShowToolBar;
        private System.Windows.Forms.ToolStripMenuItem customUIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UnlockSpiltter;
        private System.Windows.Forms.ToolStripMenuItem screenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x600ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x768ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x1440ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetUI;
        private System.Windows.Forms.ToolStripMenuItem saveAsTool;
        private System.Windows.Forms.ToolStripMenuItem saveToolStrip;
        private System.Windows.Forms.ToolStripMenuItem showsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem girdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem areaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnFull;
        private System.Windows.Forms.ToolStripMenuItem saveImageTool;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ToolStripMenuItem openFileTool;
        private System.Windows.Forms.ToolStripMenuItem Developer;
        private System.Windows.Forms.ToolStripMenuItem openImageTool;
        private System.Windows.Forms.ToolStripMenuItem playTool;
        private System.Windows.Forms.ToolStripMenuItem debugTool;
        private System.Windows.Forms.ToolStripMenuItem stopTool;
        private System.ComponentModel.BackgroundWorker workLoadFile;
        public Unit.Cameras BarRight;
        public System.Windows.Forms.Timer tmReLoadSplit;
        private System.Windows.Forms.Panel pLeft;
        public ViewHost pInfor;
        private System.Windows.Forms.Splitter split3;
        private System.Windows.Forms.Timer tmLoad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel pCamera;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.NumericUpDown numCam;
        private Cyotek.Windows.Forms.ImageBox imgLive;
        private System.ComponentModel.BackgroundWorker workLive;
        private RJButton btnLive;
        private System.Windows.Forms.ToolStripMenuItem webCamTool;
        private AutoFontLabel autoFontLabel2;
        private AutoFontLabel autoFontLabel3;
        public AutoFontLabel lbCTTool;
        public AutoFontLabel lbRsTool;
        public System.Windows.Forms.TableLayoutPanel layInforTool;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter split31;
        public System.Windows.Forms.Panel pRight;
        private System.Windows.Forms.ToolStripMenuItem customGuiTool;
        private System.Windows.Forms.ToolTip toolTip;
        public System.Windows.Forms.Label lbBypass;
        public System.Windows.Forms.PictureBox btnLogo;
        private System.Windows.Forms.ToolStripMenuItem customFormLoadTool;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importTool;
        private System.Windows.Forms.ToolStripMenuItem exportTool;
        private RJButton btnRJBtn;
    }
}
