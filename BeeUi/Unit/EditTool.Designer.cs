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
            this.lbCam = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbFrameRate = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripPort = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbNamefile = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtRecept = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbLicence = new System.Windows.Forms.Label();
            this.pView = new System.Windows.Forms.Panel();
            this.pEdit = new System.Windows.Forms.Panel();
            this.pEditTool = new System.Windows.Forms.Panel();
            this.pName = new System.Windows.Forms.Panel();
            this.lbTool = new BeeInterface.AutoFontLabel();
            this.iconTool = new System.Windows.Forms.PictureBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.LayoutEnd = new System.Windows.Forms.Panel();
            this.btnShuttdown = new BeeInterface.RJButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnExit = new BeeInterface.RJButton();
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
            this.autoFontLabel1 = new BeeInterface.AutoFontLabel();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.rjButton1 = new BeeInterface.RJButton();
            this.CameraBar = new BeeUi.Unit.Cameras();
            this.pHeader = new BeeUi.Common.Header();
            this.btnHeaderBar1 = new BeeUi.Unit.BtnHeaderBar();
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
            this.lbCam,
            this.lbFrameRate,
            this.toolStripPort,
            this.lbNamefile,
            this.txtRecept});
            this.statusStrip1.Location = new System.Drawing.Point(435, 0);
            this.statusStrip1.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusStrip1.Size = new System.Drawing.Size(1453, 39);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbCam
            // 
            this.lbCam.Image = global::BeeUi.Properties.Resources.CameraNotConnect;
            this.lbCam.Name = "lbCam";
            this.lbCam.Size = new System.Drawing.Size(48, 34);
            this.lbCam.Text = "0 fps";
            // 
            // lbFrameRate
            // 
            this.lbFrameRate.Name = "lbFrameRate";
            this.lbFrameRate.Size = new System.Drawing.Size(34, 34);
            this.lbFrameRate.Text = "0 Fps";
            // 
            // toolStripPort
            // 
            this.toolStripPort.Image = global::BeeUi.Properties.Resources.PortNotConnect;
            this.toolStripPort.Name = "toolStripPort";
            this.toolStripPort.Size = new System.Drawing.Size(79, 34);
            this.toolStripPort.Text = "PLC Ready";
            this.toolStripPort.Click += new System.EventHandler(this.toolStripPort_Click);
            this.toolStripPort.DoubleClick += new System.EventHandler(this.toolStripPort_DoubleClick);
            // 
            // lbNamefile
            // 
            this.lbNamefile.Name = "lbNamefile";
            this.lbNamefile.Size = new System.Drawing.Size(22, 34);
            this.lbNamefile.Text = "---";
            // 
            // txtRecept
            // 
            this.txtRecept.Name = "txtRecept";
            this.txtRecept.Size = new System.Drawing.Size(22, 34);
            this.txtRecept.Text = "---";
            // 
            // lbLicence
            // 
            this.lbLicence.BackColor = System.Drawing.Color.Transparent;
            this.lbLicence.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbLicence.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLicence.Location = new System.Drawing.Point(1888, 0);
            this.lbLicence.Name = "lbLicence";
            this.lbLicence.Size = new System.Drawing.Size(97, 39);
            this.lbLicence.TabIndex = 1;
            this.lbLicence.Text = "Licence :";
            this.lbLicence.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbLicence.Click += new System.EventHandler(this.lbLicence_Click);
            this.lbLicence.DoubleClick += new System.EventHandler(this.lbLicence_DoubleClick);
            // 
            // pView
            // 
            this.pView.BackColor = System.Drawing.Color.Transparent;
            this.pView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pView.Location = new System.Drawing.Point(0, 127);
            this.pView.Margin = new System.Windows.Forms.Padding(1);
            this.pView.Name = "pView";
            this.pView.Size = new System.Drawing.Size(1735, 1234);
            this.pView.TabIndex = 16;
            this.pView.SizeChanged += new System.EventHandler(this.pView_SizeChanged);
            // 
            // pEdit
            // 
            this.pEdit.AutoScroll = true;
            this.pEdit.BackColor = System.Drawing.Color.Transparent;
            this.pEdit.Controls.Add(this.pEditTool);
            this.pEdit.Controls.Add(this.pName);
            this.pEdit.Controls.Add(this.CameraBar);
            this.pEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.pEdit.Location = new System.Drawing.Point(1740, 127);
            this.pEdit.MaximumSize = new System.Drawing.Size(500, 0);
            this.pEdit.Name = "pEdit";
            this.pEdit.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.pEdit.Size = new System.Drawing.Size(500, 1234);
            this.pEdit.TabIndex = 14;
            // 
            // pEditTool
            // 
            this.pEditTool.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pEditTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pEditTool.Location = new System.Drawing.Point(0, 112);
            this.pEditTool.Name = "pEditTool";
            this.pEditTool.Size = new System.Drawing.Size(500, 1117);
            this.pEditTool.TabIndex = 1;
            this.pEditTool.Paint += new System.Windows.Forms.PaintEventHandler(this.pEditTool_Paint_1);
            // 
            // pName
            // 
            this.pName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pName.Controls.Add(this.lbTool);
            this.pName.Controls.Add(this.iconTool);
            this.pName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pName.Location = new System.Drawing.Point(0, 64);
            this.pName.Name = "pName";
            this.pName.Size = new System.Drawing.Size(500, 48);
            this.pName.TabIndex = 0;
            this.pName.Visible = false;
            this.pName.Paint += new System.Windows.Forms.PaintEventHandler(this.pName_Paint);
            // 
            // lbTool
            // 
            this.lbTool.AutoFont = true;
            this.lbTool.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbTool.Font = new System.Drawing.Font("Microsoft Sans Serif", 31.16406F);
            this.lbTool.Location = new System.Drawing.Point(54, 0);
            this.lbTool.Name = "lbTool";
            this.lbTool.Size = new System.Drawing.Size(419, 48);
            this.lbTool.TabIndex = 2;
            this.lbTool.Text = "Tool";
            this.lbTool.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // iconTool
            // 
            this.iconTool.BackgroundImage = global::BeeUi.Properties.Resources.Add;
            this.iconTool.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.iconTool.Dock = System.Windows.Forms.DockStyle.Left;
            this.iconTool.Location = new System.Drawing.Point(0, 0);
            this.iconTool.Name = "iconTool";
            this.iconTool.Size = new System.Drawing.Size(54, 48);
            this.iconTool.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.iconTool.TabIndex = 0;
            this.iconTool.TabStop = false;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(1735, 127);
            this.splitter1.MaximumSize = new System.Drawing.Size(200, 2000);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 1234);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // LayoutEnd
            // 
            this.LayoutEnd.BackColor = System.Drawing.Color.WhiteSmoke;
            this.LayoutEnd.Controls.Add(this.statusStrip1);
            this.LayoutEnd.Controls.Add(this.lbLicence);
            this.LayoutEnd.Controls.Add(this.btnShuttdown);
            this.LayoutEnd.Controls.Add(this.label3);
            this.LayoutEnd.Controls.Add(this.label4);
            this.LayoutEnd.Controls.Add(this.pictureBox1);
            this.LayoutEnd.Controls.Add(this.btnExit);
            this.LayoutEnd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LayoutEnd.Location = new System.Drawing.Point(0, 1361);
            this.LayoutEnd.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.LayoutEnd.Name = "LayoutEnd";
            this.LayoutEnd.Size = new System.Drawing.Size(2240, 39);
            this.LayoutEnd.TabIndex = 7;
            // 
            // btnShuttdown
            // 
            this.btnShuttdown.BackColor = System.Drawing.Color.Transparent;
            this.btnShuttdown.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnShuttdown.BorderColor = System.Drawing.Color.Transparent;
            this.btnShuttdown.BorderRadius = 5;
            this.btnShuttdown.BorderSize = 2;
            this.btnShuttdown.Corner = BeeGlobal.Corner.Both;
            this.btnShuttdown.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnShuttdown.FlatAppearance.BorderSize = 0;
            this.btnShuttdown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShuttdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShuttdown.ForeColor = System.Drawing.Color.Black;
            this.btnShuttdown.Image = global::BeeUi.Properties.Resources.Shutdown;
            this.btnShuttdown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShuttdown.IsCLick = false;
            this.btnShuttdown.IsNotChange = true;
            this.btnShuttdown.IsRect = true;
            this.btnShuttdown.IsUnGroup = true;
            this.btnShuttdown.Location = new System.Drawing.Point(1985, 0);
            this.btnShuttdown.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.btnShuttdown.Name = "btnShuttdown";
            this.btnShuttdown.Size = new System.Drawing.Size(124, 39);
            this.btnShuttdown.TabIndex = 8;
            this.btnShuttdown.Text = "Shutdown";
            this.btnShuttdown.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnShuttdown.TextColor = System.Drawing.Color.Black;
            this.btnShuttdown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnShuttdown.UseVisualStyleBackColor = false;
            this.btnShuttdown.Click += new System.EventHandler(this.btnShuttdown_Click);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(111)))), ((int)(((byte)(111)))));
            this.label3.Location = new System.Drawing.Point(206, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(229, 39);
            this.label3.TabIndex = 2;
            this.label3.Text = " Sensing the future";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(111)))), ((int)(((byte)(111)))));
            this.label4.Location = new System.Drawing.Point(138, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 39);
            this.label4.TabIndex = 3;
            this.label4.Text = "ver 1.1.*";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::BeeUi.Properties.Resources.UNISEN_LOGO;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(138, 39);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExit.BackgroundImage")));
            this.btnExit.BorderColor = System.Drawing.Color.Red;
            this.btnExit.BorderRadius = 5;
            this.btnExit.BorderSize = 1;
            this.btnExit.Corner = BeeGlobal.Corner.Both;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Image = global::BeeUi.Properties.Resources.Logout_3;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.IsCLick = false;
            this.btnExit.IsNotChange = true;
            this.btnExit.IsRect = true;
            this.btnExit.IsUnGroup = true;
            this.btnExit.Location = new System.Drawing.Point(2109, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(131, 39);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "EXit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.TextColor = System.Drawing.Color.Black;
            this.btnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
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
            this.pTop.Controls.Add(this.autoFontLabel1);
            this.pTop.Controls.Add(this.btnHeaderBar1);
            this.pTop.Controls.Add(this.picLogo);
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(2240, 51);
            this.pTop.TabIndex = 22;
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
            // rjButton1
            // 
            this.rjButton1.BackColor = System.Drawing.Color.Transparent;
            this.rjButton1.BackgroundColor = System.Drawing.Color.Transparent;
            this.rjButton1.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rjButton1.BorderRadius = 0;
            this.rjButton1.BorderSize = 0;
            this.rjButton1.Corner = BeeGlobal.Corner.Both;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.ForeColor = System.Drawing.Color.White;
            this.rjButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton1.IsCLick = false;
            this.rjButton1.IsNotChange = false;
            this.rjButton1.IsRect = false;
            this.rjButton1.IsUnGroup = false;
            this.rjButton1.Location = new System.Drawing.Point(35, 4);
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(150, 40);
            this.rjButton1.TabIndex = 2;
            this.rjButton1.Text = "rjButton1";
            this.rjButton1.TextColor = System.Drawing.Color.White;
            this.rjButton1.UseVisualStyleBackColor = false;
            this.rjButton1.Click += new System.EventHandler(this.rjButton1_Click);
            // 
            // CameraBar
            // 
            this.CameraBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.CameraBar.Location = new System.Drawing.Point(0, 0);
            this.CameraBar.Name = "CameraBar";
            this.CameraBar.Size = new System.Drawing.Size(500, 64);
            this.CameraBar.TabIndex = 3;
            // 
            // pHeader
            // 
            this.pHeader.BackColor = System.Drawing.Color.Transparent;
            this.pHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pHeader.Location = new System.Drawing.Point(0, 51);
            this.pHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pHeader.Name = "pHeader";
            this.pHeader.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.pHeader.Size = new System.Drawing.Size(2240, 76);
            this.pHeader.TabIndex = 22;
            // 
            // btnHeaderBar1
            // 
            this.btnHeaderBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnHeaderBar1.Location = new System.Drawing.Point(1735, 0);
            this.btnHeaderBar1.Margin = new System.Windows.Forms.Padding(0);
            this.btnHeaderBar1.Name = "btnHeaderBar1";
            this.btnHeaderBar1.Size = new System.Drawing.Size(505, 51);
            this.btnHeaderBar1.TabIndex = 8;
            this.btnHeaderBar1.Load += new System.EventHandler(this.btnHeaderBar1_Load);
            // 
            // EditTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pView);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pEdit);
            this.Controls.Add(this.LayoutEnd);
            this.Controls.Add(this.pHeader);
            this.Controls.Add(this.pTop);
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
        public System.Windows.Forms.Panel pView;
        public System.Windows.Forms.Label lbLicence;
        private System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ToolStripStatusLabel lbCam;
        public System.Windows.Forms.ToolStripStatusLabel toolStripPort;
        public System.Windows.Forms.ToolStripStatusLabel txtRecept;
        public System.Windows.Forms.ToolStripStatusLabel lbFrameRate;
        private RJButton btnExit;
        private System.Windows.Forms.PictureBox picLogo;
        public System.Windows.Forms.Panel pEdit;
        public System.Windows.Forms.Panel pEditTool;
        public System.Windows.Forms.Panel pName;
        public System.Windows.Forms.PictureBox iconTool;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Timer tmReaPLC;
        public Unit.BtnHeaderBar btnHeaderBar1;
        private RJButton btnShuttdown;
        private System.Windows.Forms.Label label4;
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
        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.Splitter splitter1;
        private Unit.Cameras CameraBar;
        public System.Windows.Forms.Panel LayoutEnd;
        private AutoFontLabel autoFontLabel1;
        public Common.Header pHeader;
        public AutoFontLabel lbTool;
    }
}
