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
            this.pHeader = new System.Windows.Forms.Panel();
            this.btnHeaderBar1 = new BeeUi.Unit.BtnHeaderBar();
            this.label2 = new System.Windows.Forms.Label();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.LayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.inforBar1 = new BeeUi.Unit.InforBar();
            this.LayOutShow = new System.Windows.Forms.TableLayoutPanel();
            this.pEdit = new System.Windows.Forms.Panel();
            this.pEditTool = new System.Windows.Forms.Panel();
            this.pName = new System.Windows.Forms.Panel();
            this.lbNameStep = new System.Windows.Forms.Label();
            this.lbNumStep = new System.Windows.Forms.Label();
            this.lbTool = new System.Windows.Forms.Label();
            this.iconTool = new System.Windows.Forms.PictureBox();
            this.LayoutEnd = new System.Windows.Forms.TableLayoutPanel();
            this.btnShuttdown = new RJButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnExit = new RJButton();
            this.header1 = new BeeUi.Common.Header();
            this.tmReaPLC = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.pHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.LayoutMain.SuspendLayout();
            this.LayOutShow.SuspendLayout();
            this.pEdit.SuspendLayout();
            this.pName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconTool)).BeginInit();
            this.LayoutEnd.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            this.statusStrip1.Location = new System.Drawing.Point(458, 0);
            this.statusStrip1.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusStrip1.Size = new System.Drawing.Size(309, 43);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbCam
            // 
            this.lbCam.Image = global::BeeUi.Properties.Resources.CameraNotConnect;
            this.lbCam.Name = "lbCam";
            this.lbCam.Size = new System.Drawing.Size(48, 38);
            this.lbCam.Text = "0 fps";
            // 
            // lbFrameRate
            // 
            this.lbFrameRate.Name = "lbFrameRate";
            this.lbFrameRate.Size = new System.Drawing.Size(34, 38);
            this.lbFrameRate.Text = "0 Fps";
            // 
            // toolStripPort
            // 
            this.toolStripPort.Image = global::BeeUi.Properties.Resources.PortNotConnect;
            this.toolStripPort.Name = "toolStripPort";
            this.toolStripPort.Size = new System.Drawing.Size(79, 38);
            this.toolStripPort.Text = "PLC Ready";
            this.toolStripPort.Click += new System.EventHandler(this.toolStripPort_Click);
            this.toolStripPort.DoubleClick += new System.EventHandler(this.toolStripPort_DoubleClick);
            // 
            // lbNamefile
            // 
            this.lbNamefile.Name = "lbNamefile";
            this.lbNamefile.Size = new System.Drawing.Size(22, 38);
            this.lbNamefile.Text = "---";
            // 
            // txtRecept
            // 
            this.txtRecept.Name = "txtRecept";
            this.txtRecept.Size = new System.Drawing.Size(22, 38);
            this.txtRecept.Text = "---";
            // 
            // lbLicence
            // 
            this.lbLicence.BackColor = System.Drawing.Color.Transparent;
            this.lbLicence.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLicence.Location = new System.Drawing.Point(770, 0);
            this.lbLicence.Name = "lbLicence";
            this.lbLicence.Size = new System.Drawing.Size(97, 37);
            this.lbLicence.TabIndex = 1;
            this.lbLicence.Text = "Licence :";
            this.lbLicence.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbLicence.Click += new System.EventHandler(this.lbLicence_Click);
            this.lbLicence.DoubleClick += new System.EventHandler(this.lbLicence_DoubleClick);
            // 
            // pView
            // 
            this.pView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pView.BackColor = System.Drawing.Color.Transparent;
            this.pView.Location = new System.Drawing.Point(3, 3);
            this.pView.Name = "pView";
            this.pView.Size = new System.Drawing.Size(738, 615);
            this.pView.TabIndex = 16;
            this.pView.SizeChanged += new System.EventHandler(this.pView_SizeChanged);
            // 
            // pHeader
            // 
            this.pHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.pHeader.Controls.Add(this.btnHeaderBar1);
            this.pHeader.Controls.Add(this.label2);
            this.pHeader.Controls.Add(this.picLogo);
            this.pHeader.Location = new System.Drawing.Point(0, 0);
            this.pHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pHeader.Name = "pHeader";
            this.pHeader.Size = new System.Drawing.Size(1150, 55);
            this.pHeader.TabIndex = 19;
            // 
            // btnHeaderBar1
            // 
            this.btnHeaderBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnHeaderBar1.Location = new System.Drawing.Point(715, 0);
            this.btnHeaderBar1.Margin = new System.Windows.Forms.Padding(0);
            this.btnHeaderBar1.Name = "btnHeaderBar1";
            this.btnHeaderBar1.Size = new System.Drawing.Size(435, 55);
            this.btnHeaderBar1.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(70, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(296, 55);
            this.label2.TabIndex = 0;
            this.label2.Text = "Vision Sensor";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Click += new System.EventHandler(this.label2_Click_1);
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.Transparent;
            this.picLogo.Dock = System.Windows.Forms.DockStyle.Left;
            this.picLogo.Image = global::BeeUi.Properties.Resources.UNISEN_icon1;
            this.picLogo.Location = new System.Drawing.Point(0, 0);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(70, 55);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 7;
            this.picLogo.TabStop = false;
            // 
            // LayoutMain
            // 
            this.LayoutMain.ColumnCount = 1;
            this.LayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutMain.Controls.Add(this.inforBar1, 0, 3);
            this.LayoutMain.Controls.Add(this.pHeader, 0, 0);
            this.LayoutMain.Controls.Add(this.LayOutShow, 0, 2);
            this.LayoutMain.Controls.Add(this.LayoutEnd, 0, 4);
            this.LayoutMain.Controls.Add(this.header1, 0, 1);
            this.LayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutMain.Location = new System.Drawing.Point(0, 0);
            this.LayoutMain.Name = "LayoutMain";
            this.LayoutMain.RowCount = 5;
            this.LayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.LayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.LayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.LayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.LayoutMain.Size = new System.Drawing.Size(1150, 865);
            this.LayoutMain.TabIndex = 21;
            this.LayoutMain.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // inforBar1
            // 
            this.inforBar1.BackColor = System.Drawing.Color.White;
            this.inforBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inforBar1.Location = new System.Drawing.Point(3, 775);
            this.inforBar1.Name = "inforBar1";
            this.inforBar1.Size = new System.Drawing.Size(1144, 39);
            this.inforBar1.TabIndex = 0;
            this.inforBar1.Visible = false;
            // 
            // LayOutShow
            // 
            this.LayOutShow.BackColor = System.Drawing.Color.Transparent;
            this.LayOutShow.ColumnCount = 2;
            this.LayOutShow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayOutShow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.LayOutShow.Controls.Add(this.pEdit, 1, 0);
            this.LayOutShow.Controls.Add(this.pView, 0, 0);
            this.LayOutShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayOutShow.Location = new System.Drawing.Point(3, 148);
            this.LayOutShow.Name = "LayOutShow";
            this.LayOutShow.RowCount = 1;
            this.LayOutShow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayOutShow.Size = new System.Drawing.Size(1144, 621);
            this.LayOutShow.TabIndex = 20;
            // 
            // pEdit
            // 
            this.pEdit.BackColor = System.Drawing.Color.Transparent;
            this.pEdit.Controls.Add(this.pEditTool);
            this.pEdit.Controls.Add(this.pName);
            this.pEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pEdit.Location = new System.Drawing.Point(747, 3);
            this.pEdit.Name = "pEdit";
            this.pEdit.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.pEdit.Size = new System.Drawing.Size(394, 615);
            this.pEdit.TabIndex = 14;
            // 
            // pEditTool
            // 
            this.pEditTool.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pEditTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pEditTool.Location = new System.Drawing.Point(0, 64);
            this.pEditTool.Name = "pEditTool";
            this.pEditTool.Size = new System.Drawing.Size(394, 546);
            this.pEditTool.TabIndex = 1;
            this.pEditTool.Paint += new System.Windows.Forms.PaintEventHandler(this.pEditTool_Paint_1);
            // 
            // pName
            // 
            this.pName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pName.Controls.Add(this.lbNameStep);
            this.pName.Controls.Add(this.lbNumStep);
            this.pName.Controls.Add(this.lbTool);
            this.pName.Controls.Add(this.iconTool);
            this.pName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pName.Location = new System.Drawing.Point(0, 0);
            this.pName.Name = "pName";
            this.pName.Size = new System.Drawing.Size(394, 64);
            this.pName.TabIndex = 0;
            this.pName.Visible = false;
            this.pName.Paint += new System.Windows.Forms.PaintEventHandler(this.pName_Paint);
            // 
            // lbNameStep
            // 
            this.lbNameStep.AutoSize = true;
            this.lbNameStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNameStep.Location = new System.Drawing.Point(81, 5);
            this.lbNameStep.Name = "lbNameStep";
            this.lbNameStep.Size = new System.Drawing.Size(139, 25);
            this.lbNameStep.TabIndex = 3;
            this.lbNameStep.Text = "Tool Setting";
            // 
            // lbNumStep
            // 
            this.lbNumStep.AutoSize = true;
            this.lbNumStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNumStep.Location = new System.Drawing.Point(13, 5);
            this.lbNumStep.Name = "lbNumStep";
            this.lbNumStep.Size = new System.Drawing.Size(62, 20);
            this.lbNumStep.TabIndex = 2;
            this.lbNumStep.Text = "Step 3";
            // 
            // lbTool
            // 
            this.lbTool.AutoSize = true;
            this.lbTool.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTool.Location = new System.Drawing.Point(47, 40);
            this.lbTool.Name = "lbTool";
            this.lbTool.Size = new System.Drawing.Size(72, 20);
            this.lbTool.TabIndex = 1;
            this.lbTool.Text = "OutLine";
            // 
            // iconTool
            // 
            this.iconTool.BackgroundImage = global::BeeUi.Properties.Resources.Add;
            this.iconTool.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.iconTool.Location = new System.Drawing.Point(17, 36);
            this.iconTool.Name = "iconTool";
            this.iconTool.Size = new System.Drawing.Size(24, 24);
            this.iconTool.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.iconTool.TabIndex = 0;
            this.iconTool.TabStop = false;
            // 
            // LayoutEnd
            // 
            this.LayoutEnd.BackColor = System.Drawing.Color.Transparent;
            this.LayoutEnd.ColumnCount = 5;
            this.LayoutEnd.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 448F));
            this.LayoutEnd.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutEnd.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.LayoutEnd.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 137F));
            this.LayoutEnd.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 137F));
            this.LayoutEnd.Controls.Add(this.btnShuttdown, 3, 0);
            this.LayoutEnd.Controls.Add(this.lbLicence, 2, 0);
            this.LayoutEnd.Controls.Add(this.statusStrip1, 1, 0);
            this.LayoutEnd.Controls.Add(this.panel1, 0, 0);
            this.LayoutEnd.Controls.Add(this.btnExit, 4, 0);
            this.LayoutEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutEnd.Location = new System.Drawing.Point(3, 820);
            this.LayoutEnd.Name = "LayoutEnd";
            this.LayoutEnd.RowCount = 1;
            this.LayoutEnd.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.LayoutEnd.Size = new System.Drawing.Size(1144, 42);
            this.LayoutEnd.TabIndex = 21;
            // 
            // btnShuttdown
            // 
            this.btnShuttdown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(173)))), ((int)(((byte)(245)))));
            this.btnShuttdown.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(173)))), ((int)(((byte)(245)))));
            this.btnShuttdown.BorderColor = System.Drawing.Color.MidnightBlue;
            this.btnShuttdown.BorderRadius = 5;
            this.btnShuttdown.BorderSize = 2;
            this.btnShuttdown.ButtonImage = null;
            this.btnShuttdown.Corner =Corner.Both;
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
            this.btnShuttdown.Location = new System.Drawing.Point(873, 3);
            this.btnShuttdown.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.btnShuttdown.Name = "btnShuttdown";
            this.btnShuttdown.Size = new System.Drawing.Size(124, 37);
            this.btnShuttdown.TabIndex = 8;
            this.btnShuttdown.Text = "Shutdown";
            this.btnShuttdown.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnShuttdown.TextColor = System.Drawing.Color.Black;
            this.btnShuttdown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnShuttdown.UseVisualStyleBackColor = false;
            this.btnShuttdown.Click += new System.EventHandler(this.btnShuttdown_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(435, 37);
            this.panel1.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(111)))), ((int)(((byte)(111)))));
            this.label3.Location = new System.Drawing.Point(138, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 37);
            this.label3.TabIndex = 2;
            this.label3.Text = " Sensing the future";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(111)))), ((int)(((byte)(111)))));
            this.label4.Location = new System.Drawing.Point(305, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 37);
            this.label4.TabIndex = 3;
            this.label4.Text = "ver Tiger 2.0";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::BeeUi.Properties.Resources.UNISEN_LOGO__1_;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(138, 37);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(173)))), ((int)(((byte)(245)))));
            this.btnExit.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(173)))), ((int)(((byte)(245)))));
            this.btnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExit.BackgroundImage")));
            this.btnExit.BorderColor = System.Drawing.Color.MidnightBlue;
            this.btnExit.BorderRadius = 5;
            this.btnExit.BorderSize = 2;
            this.btnExit.ButtonImage = null;
            this.btnExit.Corner =Corner.Both;
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
            this.btnExit.Location = new System.Drawing.Point(1010, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(131, 37);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "EXit";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.TextColor = System.Drawing.Color.Black;
            this.btnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // header1
            // 
            this.header1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.header1.BackColor = System.Drawing.Color.Transparent;
            this.header1.Location = new System.Drawing.Point(0, 55);
            this.header1.Margin = new System.Windows.Forms.Padding(0);
            this.header1.Name = "header1";
            this.header1.Size = new System.Drawing.Size(1150, 90);
            this.header1.TabIndex = 22;
            // 
            // tmReaPLC
            // 
            this.tmReaPLC.Interval = 1;
            this.tmReaPLC.Tick += new System.EventHandler(this.tmReaPLC_Tick);
            // 
            // EditTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.LayoutMain);
            this.Name = "EditTool";
            this.Size = new System.Drawing.Size(1150, 865);
            this.Load += new System.EventHandler(this.EditTool_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.LayoutMain.ResumeLayout(false);
            this.LayOutShow.ResumeLayout(false);
            this.pEdit.ResumeLayout(false);
            this.pName.ResumeLayout(false);
            this.pName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconTool)).EndInit();
            this.LayoutEnd.ResumeLayout(false);
            this.LayoutEnd.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private System.Windows.Forms.Label label2;
        public Unit.InforBar inforBar1;
        public System.Windows.Forms.Panel pHeader;
        public System.Windows.Forms.Panel pEdit;
        public System.Windows.Forms.Panel pEditTool;
        public System.Windows.Forms.Panel pName;
        public System.Windows.Forms.Label lbNameStep;
        public System.Windows.Forms.Label lbNumStep;
        public System.Windows.Forms.Label lbTool;
        public System.Windows.Forms.PictureBox iconTool;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;

        public System.Windows.Forms.TableLayoutPanel LayOutShow;
        public System.Windows.Forms.TableLayoutPanel LayoutEnd;
        public System.Windows.Forms.TableLayoutPanel LayoutMain;
        public System.Windows.Forms.Timer tmReaPLC;
        private Common.Header header1;
        public Unit.BtnHeaderBar btnHeaderBar1;
        private RJButton btnShuttdown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.ToolStripStatusLabel lbNamefile;
    }
}
