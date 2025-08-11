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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbLicence = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbCam = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbFrameRate = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripPort = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbNamefile = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtRecept = new System.Windows.Forms.ToolStripStatusLabel();
            this.pEdit = new System.Windows.Forms.Panel();
            this.pName = new System.Windows.Forms.Panel();
            this.iconTool = new System.Windows.Forms.PictureBox();
            this.splitter7 = new System.Windows.Forms.Splitter();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.LayoutEnd = new System.Windows.Forms.Panel();
            this.splitter6 = new System.Windows.Forms.Splitter();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tmReaPLC = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.pTop = new System.Windows.Forms.Panel();
            this.splitter5 = new System.Windows.Forms.Splitter();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.pView = new System.Windows.Forms.Panel();
            this.splitter4 = new System.Windows.Forms.Splitter();
            this.lbTool = new BeeInterface.AutoFontLabel();
            this.autoFontLabel1 = new BeeInterface.AutoFontLabel();
            this.rjButton1 = new BeeInterface.RJButton();
            this.pInfor = new BeeUi.ViewHost();
            this.pEditTool = new BeeUi.ViewHost();
            this.CameraBar = new BeeUi.Unit.Cameras();
            this.hideBar = new BeeUi.Unit.HideBar();
            this.pHeader = new BeeUi.Common.Header();
            this.btnHeaderBar = new BeeUi.Unit.BtnHeaderBar();
            this.statusStrip1.SuspendLayout();
            this.pEdit.SuspendLayout();
            this.pName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconTool)).BeginInit();
            this.LayoutEnd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
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
            this.toolStripPort,
            this.lbNamefile,
            this.txtRecept});
            this.statusStrip1.Location = new System.Drawing.Point(305, 0);
            this.statusStrip1.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusStrip1.Size = new System.Drawing.Size(1190, 33);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbLicence
            // 
            this.lbLicence.Name = "lbLicence";
            this.lbLicence.Size = new System.Drawing.Size(47, 28);
            this.lbLicence.Text = "Licence";
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
            this.toolStripPort.Size = new System.Drawing.Size(79, 28);
            this.toolStripPort.Text = "PLC Ready";
            this.toolStripPort.Click += new System.EventHandler(this.toolStripPort_Click);
            this.toolStripPort.DoubleClick += new System.EventHandler(this.toolStripPort_DoubleClick);
            // 
            // lbNamefile
            // 
            this.lbNamefile.Name = "lbNamefile";
            this.lbNamefile.Size = new System.Drawing.Size(22, 28);
            this.lbNamefile.Text = "---";
            // 
            // txtRecept
            // 
            this.txtRecept.Name = "txtRecept";
            this.txtRecept.Size = new System.Drawing.Size(22, 28);
            this.txtRecept.Text = "---";
            // 
            // pEdit
            // 
            this.pEdit.AutoScroll = true;
            this.pEdit.BackColor = System.Drawing.Color.White;
            this.pEdit.Controls.Add(this.pEditTool);
            this.pEdit.Controls.Add(this.pName);
            this.pEdit.Controls.Add(this.splitter7);
            this.pEdit.Controls.Add(this.CameraBar);
            this.pEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.pEdit.Location = new System.Drawing.Point(1740, 136);
            this.pEdit.MaximumSize = new System.Drawing.Size(500, 0);
            this.pEdit.Name = "pEdit";
            this.pEdit.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.pEdit.Size = new System.Drawing.Size(500, 1231);
            this.pEdit.TabIndex = 14;
            // 
            // pName
            // 
            this.pName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pName.Controls.Add(this.lbTool);
            this.pName.Controls.Add(this.iconTool);
            this.pName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pName.Location = new System.Drawing.Point(0, 70);
            this.pName.Name = "pName";
            this.pName.Size = new System.Drawing.Size(500, 44);
            this.pName.TabIndex = 0;
            this.pName.Visible = false;
            this.pName.Paint += new System.Windows.Forms.PaintEventHandler(this.pName_Paint);
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
            // splitter7
            // 
            this.splitter7.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter7.Location = new System.Drawing.Point(0, 67);
            this.splitter7.Name = "splitter7";
            this.splitter7.Size = new System.Drawing.Size(500, 3);
            this.splitter7.TabIndex = 4;
            this.splitter7.TabStop = false;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(1735, 136);
            this.splitter1.MaximumSize = new System.Drawing.Size(200, 2000);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 1231);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // LayoutEnd
            // 
            this.LayoutEnd.BackColor = System.Drawing.Color.WhiteSmoke;
            this.LayoutEnd.Controls.Add(this.statusStrip1);
            this.LayoutEnd.Controls.Add(this.splitter6);
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
            // splitter6
            // 
            this.splitter6.BackColor = System.Drawing.SystemColors.Control;
            this.splitter6.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter6.Location = new System.Drawing.Point(1495, 0);
            this.splitter6.Name = "splitter6";
            this.splitter6.Size = new System.Drawing.Size(5, 33);
            this.splitter6.TabIndex = 4;
            this.splitter6.TabStop = false;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(111)))), ((int)(((byte)(111)))));
            this.label3.Location = new System.Drawing.Point(138, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 33);
            this.label3.TabIndex = 2;
            this.label3.Text = " Sensing the future";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::BeeUi.Properties.Resources.UNISEN_LOGO;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(138, 33);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tmReaPLC
            // 
            this.tmReaPLC.Interval = 1;
            this.tmReaPLC.Tick += new System.EventHandler(this.tmReaPLC_Tick);
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
            this.pTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.pTop.Controls.Add(this.splitter5);
            this.pTop.Controls.Add(this.autoFontLabel1);
            this.pTop.Controls.Add(this.btnHeaderBar);
            this.pTop.Controls.Add(this.picLogo);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(2240, 51);
            this.pTop.TabIndex = 22;
            // 
            // splitter5
            // 
            this.splitter5.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter5.Location = new System.Drawing.Point(1730, 0);
            this.splitter5.MaximumSize = new System.Drawing.Size(200, 2000);
            this.splitter5.Name = "splitter5";
            this.splitter5.Size = new System.Drawing.Size(5, 51);
            this.splitter5.TabIndex = 10;
            this.splitter5.TabStop = false;
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.Transparent;
            this.picLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.picLogo.Image = global::BeeUi.Properties.Resources.UNISEN_icon1;
            this.picLogo.Location = new System.Drawing.Point(0, 0);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(70, 51);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 7;
            this.picLogo.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(0, 136);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(1735, 5);
            this.splitter2.TabIndex = 24;
            this.splitter2.TabStop = false;
            // 
            // splitter3
            // 
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter3.Location = new System.Drawing.Point(0, 247);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(1735, 5);
            this.splitter3.TabIndex = 26;
            this.splitter3.TabStop = false;
            // 
            // pView
            // 
            this.pView.BackColor = System.Drawing.Color.Transparent;
            this.pView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pView.Location = new System.Drawing.Point(0, 252);
            this.pView.Margin = new System.Windows.Forms.Padding(1);
            this.pView.Name = "pView";
            this.pView.Size = new System.Drawing.Size(1735, 1115);
            this.pView.TabIndex = 27;
            this.pView.SizeChanged += new System.EventHandler(this.pView_SizeChanged);
            // 
            // splitter4
            // 
            this.splitter4.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter4.Location = new System.Drawing.Point(0, 51);
            this.splitter4.Name = "splitter4";
            this.splitter4.Size = new System.Drawing.Size(2240, 5);
            this.splitter4.TabIndex = 28;
            this.splitter4.TabStop = false;
            // 
            // lbTool
            // 
            this.lbTool.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTool.AutoFont = true;
            this.lbTool.Font = new System.Drawing.Font("Microsoft Sans Serif", 29.23047F);
            this.lbTool.Location = new System.Drawing.Point(56, -1);
            this.lbTool.Name = "lbTool";
            this.lbTool.Size = new System.Drawing.Size(444, 44);
            this.lbTool.TabIndex = 1;
            this.lbTool.Text = "Tool";
            // 
            // autoFontLabel1
            // 
            this.autoFontLabel1.AutoFont = true;
            this.autoFontLabel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.autoFontLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 31.9375F, System.Drawing.FontStyle.Bold);
            this.autoFontLabel1.ForeColor = System.Drawing.Color.White;
            this.autoFontLabel1.Location = new System.Drawing.Point(70, 0);
            this.autoFontLabel1.Name = "autoFontLabel1";
            this.autoFontLabel1.Size = new System.Drawing.Size(365, 51);
            this.autoFontLabel1.TabIndex = 9;
            this.autoFontLabel1.Text = "Vision Sensor ";
            this.autoFontLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.rjButton1.IsUnGroup = false;
            this.rjButton1.Location = new System.Drawing.Point(35, 4);
            this.rjButton1.Multiline = false;
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(150, 40);
            this.rjButton1.TabIndex = 2;
            this.rjButton1.Text = "rjButton1";
            this.rjButton1.TextColor = System.Drawing.Color.White;
            this.rjButton1.UseVisualStyleBackColor = false;
            this.rjButton1.Click += new System.EventHandler(this.rjButton1_Click);
            // 
            // pInfor
            // 
            this.pInfor.BackColor = System.Drawing.Color.Transparent;
            this.pInfor.Dock = System.Windows.Forms.DockStyle.Top;
            this.pInfor.Location = new System.Drawing.Point(0, 141);
            this.pInfor.Margin = new System.Windows.Forms.Padding(1);
            this.pInfor.Name = "pInfor";
            this.pInfor.Size = new System.Drawing.Size(1735, 106);
            this.pInfor.TabIndex = 25;
            this.pInfor.SizeChanged += new System.EventHandler(this.pInfor_SizeChanged);
            // 
            // pEditTool
            // 
            this.pEditTool.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pEditTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pEditTool.Location = new System.Drawing.Point(0, 114);
            this.pEditTool.Name = "pEditTool";
            this.pEditTool.Size = new System.Drawing.Size(500, 1112);
            this.pEditTool.TabIndex = 1;
            this.pEditTool.Paint += new System.Windows.Forms.PaintEventHandler(this.pEditTool_Paint_1);
            // 
            // CameraBar
            // 
            this.CameraBar.BackColor = System.Drawing.Color.White;
            this.CameraBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.CameraBar.Location = new System.Drawing.Point(0, 0);
            this.CameraBar.Name = "CameraBar";
            this.CameraBar.Size = new System.Drawing.Size(500, 67);
            this.CameraBar.TabIndex = 3;
            // 
            // hideBar
            // 
            this.hideBar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.hideBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.hideBar.Location = new System.Drawing.Point(1500, 0);
            this.hideBar.Name = "hideBar";
            this.hideBar.Size = new System.Drawing.Size(740, 33);
            this.hideBar.TabIndex = 3;
            // 
            // pHeader
            // 
            this.pHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pHeader.Location = new System.Drawing.Point(0, 56);
            this.pHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pHeader.Name = "pHeader";
            this.pHeader.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.pHeader.Size = new System.Drawing.Size(2240, 80);
            this.pHeader.TabIndex = 22;
            // 
            // btnHeaderBar
            // 
            this.btnHeaderBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.btnHeaderBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnHeaderBar.Location = new System.Drawing.Point(1735, 0);
            this.btnHeaderBar.Margin = new System.Windows.Forms.Padding(0);
            this.btnHeaderBar.Name = "btnHeaderBar";
            this.btnHeaderBar.Size = new System.Drawing.Size(505, 51);
            this.btnHeaderBar.TabIndex = 8;
            this.btnHeaderBar.Load += new System.EventHandler(this.btnHeaderBar_Load);
            // 
            // EditTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pView);
            this.Controls.Add(this.splitter3);
            this.Controls.Add(this.pInfor);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pEdit);
            this.Controls.Add(this.LayoutEnd);
            this.Controls.Add(this.pHeader);
            this.Controls.Add(this.splitter4);
            this.Controls.Add(this.pTop);
            this.DoubleBuffered = true;
            this.Name = "EditTool";
            this.Size = new System.Drawing.Size(2240, 1400);
            this.Load += new System.EventHandler(this.EditTool_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pEdit.ResumeLayout(false);
            this.pName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconTool)).EndInit();
            this.LayoutEnd.ResumeLayout(false);
            this.LayoutEnd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ToolStripStatusLabel lbCam;
        public System.Windows.Forms.ToolStripStatusLabel toolStripPort;
        public System.Windows.Forms.ToolStripStatusLabel txtRecept;
        public System.Windows.Forms.ToolStripStatusLabel lbFrameRate;
        private System.Windows.Forms.PictureBox picLogo;
        public System.Windows.Forms.Panel pEdit;
        public ViewHost pEditTool;
        public System.Windows.Forms.Panel pName;
        public System.Windows.Forms.PictureBox iconTool;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Timer tmReaPLC;
        public Unit.BtnHeaderBar btnHeaderBar;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ToolStripStatusLabel lbNamefile;

        private RJButton rjButton1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Splitter splitter1;
        private Unit.Cameras CameraBar;
        public System.Windows.Forms.Panel LayoutEnd;
        private AutoFontLabel autoFontLabel1;
        public Common.Header pHeader;
        private System.Windows.Forms.Splitter splitter2;
        public ViewHost pInfor;
        private System.Windows.Forms.Splitter splitter3;
        public System.Windows.Forms.Panel pView;
        private System.Windows.Forms.Splitter splitter4;
        private System.Windows.Forms.Splitter splitter5;
        public System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.ToolStripStatusLabel lbLicence;
        public Unit.HideBar hideBar;
        private System.Windows.Forms.Splitter splitter6;
        private System.Windows.Forms.Splitter splitter7;
        public AutoFontLabel lbTool;
    }
}
