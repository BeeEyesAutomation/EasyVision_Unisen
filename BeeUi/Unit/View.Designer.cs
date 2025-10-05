using BeeCore;
using BeeGlobal;
using BeeInterface;

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
            this.pMenu = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnZoomOut = new BeeInterface.RJButton();
            this.btnZoomIn = new BeeInterface.RJButton();
            this.btnFull = new BeeInterface.RJButton();
            this.btnPan = new BeeInterface.RJButton();
            this.btnShowCenter = new BeeInterface.RJButton();
            this.btnGird = new BeeInterface.RJButton();
            this.btnShowArea = new BeeInterface.RJButton();
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
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.tmRefresh = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmOut = new System.Windows.Forms.Timer(this.components);
            this.tmContinuous = new System.Windows.Forms.Timer(this.components);
            this.workTrig = new System.ComponentModel.BackgroundWorker();
            this.tmPress = new System.Windows.Forms.Timer(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tmSimulation = new System.Windows.Forms.Timer(this.components);
            this.tmLive = new System.Windows.Forms.Timer(this.components);
            this.tmShow = new System.Windows.Forms.Timer(this.components);
            this.split5 = new System.Windows.Forms.Splitter();
            this.pBtn = new System.Windows.Forms.Panel();
            this.split4 = new System.Windows.Forms.Splitter();
            this.btnTypeTrig = new BeeInterface.RJButton();
            this.btnMenu = new System.Windows.Forms.Button();
            this.split3 = new System.Windows.Forms.Splitter();
            this.btnLive = new BeeInterface.RJButton();
            this.split2 = new System.Windows.Forms.Splitter();
            this.btnContinuous = new BeeInterface.RJButton();
            this.split1 = new System.Windows.Forms.Splitter();
            this.btnCap = new BeeInterface.RJButton();
            this.tmEnableControl = new System.Windows.Forms.Timer(this.components);
            this.workLiveWebcam = new System.ComponentModel.BackgroundWorker();
            this.pMenu.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pView.SuspendLayout();
            this.pBtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgView
            // 
            this.imgView.AllowClickZoom = true;
            this.imgView.AllowDoubleClick = true;
            this.imgView.AllowDrop = true;
            this.imgView.AllowUnfocusedMouseWheel = true;
            this.imgView.AlwaysShowHScroll = true;
            this.imgView.AlwaysShowVScroll = true;
            this.imgView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imgView.GridColor = System.Drawing.Color.Transparent;
            this.imgView.GridScale = Cyotek.Windows.Forms.ImageBoxGridScale.None;
            this.imgView.Location = new System.Drawing.Point(0, 0);
            this.imgView.Name = "imgView";
            this.imgView.PanMode = Cyotek.Windows.Forms.ImageBoxPanMode.Left;
            this.imgView.ShortcutsEnabled = false;
            this.imgView.Size = new System.Drawing.Size(1429, 697);
            this.imgView.TabIndex = 1;
            this.imgView.TextBackColor = System.Drawing.Color.White;
            this.imgView.TextDisplayMode = Cyotek.Windows.Forms.ImageBoxGridDisplayMode.None;
            this.imgView.Visible = false;
            this.imgView.ZoomChanged += new System.EventHandler(this.imgView_ZoomChanged);
            this.imgView.Scroll += new System.Windows.Forms.ScrollEventHandler(this.imgView_Scroll);
            this.imgView.Click += new System.EventHandler(this.imgView_Click_1);
            this.imgView.Paint += new System.Windows.Forms.PaintEventHandler(this.imgView_Paint);
            this.imgView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imgView_MouseDown);
            this.imgView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imgView_MouseMove);
            this.imgView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imgView_MouseUp);
            // 
            // pMenu
            // 
            this.pMenu.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pMenu.Controls.Add(this.tableLayoutPanel1);
            this.pMenu.Dock = System.Windows.Forms.DockStyle.Right;
            this.pMenu.Location = new System.Drawing.Point(1429, 62);
            this.pMenu.Name = "pMenu";
            this.pMenu.Size = new System.Drawing.Size(65, 697);
            this.pMenu.TabIndex = 28;
            this.pMenu.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnZoomOut, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnZoomIn, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnFull, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnPan, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnShowCenter, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnGird, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnShowArea, 0, 8);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(65, 470);
            this.tableLayoutPanel1.TabIndex = 37;
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.AutoFont = false;
            this.btnZoomOut.AutoFontHeightRatio = 0.75F;
            this.btnZoomOut.AutoFontMax = 100F;
            this.btnZoomOut.AutoFontMin = 6F;
            this.btnZoomOut.AutoFontWidthRatio = 0.92F;
            this.btnZoomOut.AutoImage = true;
            this.btnZoomOut.AutoImageMaxRatio = 0.75F;
            this.btnZoomOut.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnZoomOut.AutoImageTint = true;
            this.btnZoomOut.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnZoomOut.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnZoomOut.BorderColor = System.Drawing.Color.Gray;
            this.btnZoomOut.BorderRadius = 0;
            this.btnZoomOut.BorderSize = 0;
            this.btnZoomOut.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnZoomOut.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnZoomOut.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnZoomOut.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnZoomOut.Corner = BeeGlobal.Corner.Both;
            this.btnZoomOut.DebounceResizeMs = 16;
            this.btnZoomOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnZoomOut.FlatAppearance.BorderSize = 0;
            this.btnZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomOut.ForeColor = System.Drawing.Color.White;
            this.btnZoomOut.Image = global::BeeUi.Properties.Resources.Zoom_Out;
            this.btnZoomOut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnZoomOut.ImageDisabled = null;
            this.btnZoomOut.ImageHover = null;
            this.btnZoomOut.ImageNormal = null;
            this.btnZoomOut.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnZoomOut.ImagePressed = null;
            this.btnZoomOut.ImageTextSpacing = 6;
            this.btnZoomOut.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnZoomOut.ImageTintHover = System.Drawing.Color.Empty;
            this.btnZoomOut.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnZoomOut.ImageTintOpacity = 0.5F;
            this.btnZoomOut.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnZoomOut.IsCLick = false;
            this.btnZoomOut.IsNotChange = true;
            this.btnZoomOut.IsRect = false;
            this.btnZoomOut.IsUnGroup = true;
            this.btnZoomOut.Location = new System.Drawing.Point(3, 115);
            this.btnZoomOut.Multiline = false;
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(59, 50);
            this.btnZoomOut.TabIndex = 39;
            this.btnZoomOut.TextColor = System.Drawing.Color.White;
            this.btnZoomOut.UseVisualStyleBackColor = false;
            this.btnZoomOut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnZoomOut_MouseDown);
            this.btnZoomOut.MouseLeave += new System.EventHandler(this.btnZoomOut_MouseLeave);
            this.btnZoomOut.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnZoomOut_MouseUp);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.AutoFont = false;
            this.btnZoomIn.AutoFontHeightRatio = 0.75F;
            this.btnZoomIn.AutoFontMax = 100F;
            this.btnZoomIn.AutoFontMin = 6F;
            this.btnZoomIn.AutoFontWidthRatio = 0.92F;
            this.btnZoomIn.AutoImage = true;
            this.btnZoomIn.AutoImageMaxRatio = 0.75F;
            this.btnZoomIn.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnZoomIn.AutoImageTint = true;
            this.btnZoomIn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnZoomIn.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnZoomIn.BorderColor = System.Drawing.Color.Gray;
            this.btnZoomIn.BorderRadius = 0;
            this.btnZoomIn.BorderSize = 0;
            this.btnZoomIn.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnZoomIn.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnZoomIn.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnZoomIn.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnZoomIn.Corner = BeeGlobal.Corner.Both;
            this.btnZoomIn.DebounceResizeMs = 16;
            this.btnZoomIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnZoomIn.FlatAppearance.BorderSize = 0;
            this.btnZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomIn.ForeColor = System.Drawing.Color.White;
            this.btnZoomIn.Image = global::BeeUi.Properties.Resources.Zoom_In;
            this.btnZoomIn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnZoomIn.ImageDisabled = null;
            this.btnZoomIn.ImageHover = null;
            this.btnZoomIn.ImageNormal = null;
            this.btnZoomIn.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnZoomIn.ImagePressed = null;
            this.btnZoomIn.ImageTextSpacing = 6;
            this.btnZoomIn.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnZoomIn.ImageTintHover = System.Drawing.Color.Empty;
            this.btnZoomIn.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnZoomIn.ImageTintOpacity = 0.5F;
            this.btnZoomIn.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnZoomIn.IsCLick = false;
            this.btnZoomIn.IsNotChange = true;
            this.btnZoomIn.IsRect = false;
            this.btnZoomIn.IsUnGroup = true;
            this.btnZoomIn.Location = new System.Drawing.Point(3, 59);
            this.btnZoomIn.Multiline = false;
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(59, 50);
            this.btnZoomIn.TabIndex = 38;
            this.btnZoomIn.TextColor = System.Drawing.Color.White;
            this.btnZoomIn.UseVisualStyleBackColor = false;
            this.btnZoomIn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnZoomIn_MouseDown);
            this.btnZoomIn.MouseLeave += new System.EventHandler(this.btnZoomIn_MouseLeave);
            this.btnZoomIn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnZoomIn_MouseUp);
            // 
            // btnFull
            // 
            this.btnFull.AutoFont = false;
            this.btnFull.AutoFontHeightRatio = 0.75F;
            this.btnFull.AutoFontMax = 100F;
            this.btnFull.AutoFontMin = 6F;
            this.btnFull.AutoFontWidthRatio = 0.92F;
            this.btnFull.AutoImage = true;
            this.btnFull.AutoImageMaxRatio = 0.75F;
            this.btnFull.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnFull.AutoImageTint = true;
            this.btnFull.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnFull.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnFull.BorderColor = System.Drawing.Color.Gray;
            this.btnFull.BorderRadius = 0;
            this.btnFull.BorderSize = 0;
            this.btnFull.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnFull.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnFull.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnFull.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnFull.Corner = BeeGlobal.Corner.Both;
            this.btnFull.DebounceResizeMs = 16;
            this.btnFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFull.FlatAppearance.BorderSize = 0;
            this.btnFull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFull.ForeColor = System.Drawing.Color.White;
            this.btnFull.Image = global::BeeUi.Properties.Resources.Full_Screen;
            this.btnFull.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFull.ImageDisabled = null;
            this.btnFull.ImageHover = null;
            this.btnFull.ImageNormal = null;
            this.btnFull.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnFull.ImagePressed = null;
            this.btnFull.ImageTextSpacing = 6;
            this.btnFull.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnFull.ImageTintHover = System.Drawing.Color.Empty;
            this.btnFull.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnFull.ImageTintOpacity = 0.5F;
            this.btnFull.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnFull.IsCLick = false;
            this.btnFull.IsNotChange = true;
            this.btnFull.IsRect = false;
            this.btnFull.IsUnGroup = true;
            this.btnFull.Location = new System.Drawing.Point(3, 3);
            this.btnFull.Multiline = false;
            this.btnFull.Name = "btnFull";
            this.btnFull.Size = new System.Drawing.Size(59, 50);
            this.btnFull.TabIndex = 37;
            this.btnFull.TextColor = System.Drawing.Color.White;
            this.btnFull.UseVisualStyleBackColor = false;
            this.btnFull.Click += new System.EventHandler(this.btnFull_Click);
            // 
            // btnPan
            // 
            this.btnPan.AutoFont = false;
            this.btnPan.AutoFontHeightRatio = 0.75F;
            this.btnPan.AutoFontMax = 100F;
            this.btnPan.AutoFontMin = 6F;
            this.btnPan.AutoFontWidthRatio = 0.92F;
            this.btnPan.AutoImage = true;
            this.btnPan.AutoImageMaxRatio = 0.75F;
            this.btnPan.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnPan.AutoImageTint = true;
            this.btnPan.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnPan.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnPan.BorderColor = System.Drawing.Color.Gray;
            this.btnPan.BorderRadius = 0;
            this.btnPan.BorderSize = 0;
            this.btnPan.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnPan.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnPan.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnPan.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnPan.Corner = BeeGlobal.Corner.Both;
            this.btnPan.DebounceResizeMs = 16;
            this.btnPan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPan.FlatAppearance.BorderSize = 0;
            this.btnPan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPan.ForeColor = System.Drawing.Color.White;
            this.btnPan.Image = global::BeeUi.Properties.Resources.Hand;
            this.btnPan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPan.ImageDisabled = null;
            this.btnPan.ImageHover = null;
            this.btnPan.ImageNormal = null;
            this.btnPan.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnPan.ImagePressed = null;
            this.btnPan.ImageTextSpacing = 6;
            this.btnPan.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnPan.ImageTintHover = System.Drawing.Color.Empty;
            this.btnPan.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnPan.ImageTintOpacity = 0.5F;
            this.btnPan.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnPan.IsCLick = true;
            this.btnPan.IsNotChange = false;
            this.btnPan.IsRect = false;
            this.btnPan.IsUnGroup = true;
            this.btnPan.Location = new System.Drawing.Point(3, 171);
            this.btnPan.Multiline = false;
            this.btnPan.Name = "btnPan";
            this.btnPan.Size = new System.Drawing.Size(59, 50);
            this.btnPan.TabIndex = 43;
            this.btnPan.TextColor = System.Drawing.Color.White;
            this.btnPan.UseVisualStyleBackColor = false;
            this.btnPan.Click += new System.EventHandler(this.btnPan_Click);
            // 
            // btnShowCenter
            // 
            this.btnShowCenter.AutoFont = false;
            this.btnShowCenter.AutoFontHeightRatio = 0.75F;
            this.btnShowCenter.AutoFontMax = 100F;
            this.btnShowCenter.AutoFontMin = 6F;
            this.btnShowCenter.AutoFontWidthRatio = 0.92F;
            this.btnShowCenter.AutoImage = true;
            this.btnShowCenter.AutoImageMaxRatio = 0.75F;
            this.btnShowCenter.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnShowCenter.AutoImageTint = true;
            this.btnShowCenter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnShowCenter.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnShowCenter.BorderColor = System.Drawing.Color.Gray;
            this.btnShowCenter.BorderRadius = 0;
            this.btnShowCenter.BorderSize = 0;
            this.btnShowCenter.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnShowCenter.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnShowCenter.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnShowCenter.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnShowCenter.Corner = BeeGlobal.Corner.Both;
            this.btnShowCenter.DebounceResizeMs = 16;
            this.btnShowCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowCenter.FlatAppearance.BorderSize = 0;
            this.btnShowCenter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowCenter.ForeColor = System.Drawing.Color.White;
            this.btnShowCenter.Image = global::BeeUi.Properties.Resources.Center_of_Gravity_1;
            this.btnShowCenter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShowCenter.ImageDisabled = null;
            this.btnShowCenter.ImageHover = null;
            this.btnShowCenter.ImageNormal = null;
            this.btnShowCenter.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnShowCenter.ImagePressed = null;
            this.btnShowCenter.ImageTextSpacing = 6;
            this.btnShowCenter.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnShowCenter.ImageTintHover = System.Drawing.Color.Empty;
            this.btnShowCenter.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnShowCenter.ImageTintOpacity = 0.5F;
            this.btnShowCenter.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnShowCenter.IsCLick = false;
            this.btnShowCenter.IsNotChange = false;
            this.btnShowCenter.IsRect = false;
            this.btnShowCenter.IsUnGroup = true;
            this.btnShowCenter.Location = new System.Drawing.Point(3, 227);
            this.btnShowCenter.Multiline = false;
            this.btnShowCenter.Name = "btnShowCenter";
            this.btnShowCenter.Size = new System.Drawing.Size(59, 50);
            this.btnShowCenter.TabIndex = 40;
            this.btnShowCenter.TextColor = System.Drawing.Color.White;
            this.btnShowCenter.UseVisualStyleBackColor = false;
            this.btnShowCenter.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnGird
            // 
            this.btnGird.AutoFont = false;
            this.btnGird.AutoFontHeightRatio = 0.75F;
            this.btnGird.AutoFontMax = 100F;
            this.btnGird.AutoFontMin = 6F;
            this.btnGird.AutoFontWidthRatio = 0.92F;
            this.btnGird.AutoImage = true;
            this.btnGird.AutoImageMaxRatio = 0.75F;
            this.btnGird.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnGird.AutoImageTint = true;
            this.btnGird.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnGird.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnGird.BorderColor = System.Drawing.Color.Gray;
            this.btnGird.BorderRadius = 0;
            this.btnGird.BorderSize = 0;
            this.btnGird.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnGird.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnGird.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnGird.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnGird.Corner = BeeGlobal.Corner.Both;
            this.btnGird.DebounceResizeMs = 16;
            this.btnGird.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGird.FlatAppearance.BorderSize = 0;
            this.btnGird.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGird.ForeColor = System.Drawing.Color.White;
            this.btnGird.Image = global::BeeUi.Properties.Resources.Prison_1;
            this.btnGird.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGird.ImageDisabled = null;
            this.btnGird.ImageHover = null;
            this.btnGird.ImageNormal = null;
            this.btnGird.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnGird.ImagePressed = null;
            this.btnGird.ImageTextSpacing = 6;
            this.btnGird.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnGird.ImageTintHover = System.Drawing.Color.Empty;
            this.btnGird.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnGird.ImageTintOpacity = 0.5F;
            this.btnGird.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnGird.IsCLick = false;
            this.btnGird.IsNotChange = false;
            this.btnGird.IsRect = false;
            this.btnGird.IsUnGroup = true;
            this.btnGird.Location = new System.Drawing.Point(3, 283);
            this.btnGird.Multiline = false;
            this.btnGird.Name = "btnGird";
            this.btnGird.Size = new System.Drawing.Size(59, 50);
            this.btnGird.TabIndex = 41;
            this.btnGird.TextColor = System.Drawing.Color.White;
            this.btnGird.UseVisualStyleBackColor = false;
            this.btnGird.Click += new System.EventHandler(this.btnGird_Click_1);
            // 
            // btnShowArea
            // 
            this.btnShowArea.AutoFont = false;
            this.btnShowArea.AutoFontHeightRatio = 0.75F;
            this.btnShowArea.AutoFontMax = 100F;
            this.btnShowArea.AutoFontMin = 6F;
            this.btnShowArea.AutoFontWidthRatio = 0.92F;
            this.btnShowArea.AutoImage = true;
            this.btnShowArea.AutoImageMaxRatio = 0.75F;
            this.btnShowArea.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnShowArea.AutoImageTint = true;
            this.btnShowArea.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnShowArea.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnShowArea.BorderColor = System.Drawing.Color.Gray;
            this.btnShowArea.BorderRadius = 0;
            this.btnShowArea.BorderSize = 0;
            this.btnShowArea.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnShowArea.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnShowArea.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnShowArea.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnShowArea.Corner = BeeGlobal.Corner.Both;
            this.btnShowArea.DebounceResizeMs = 16;
            this.btnShowArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowArea.FlatAppearance.BorderSize = 0;
            this.btnShowArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowArea.ForeColor = System.Drawing.Color.White;
            this.btnShowArea.Image = global::BeeUi.Properties.Resources.Rectangular_1;
            this.btnShowArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShowArea.ImageDisabled = null;
            this.btnShowArea.ImageHover = null;
            this.btnShowArea.ImageNormal = null;
            this.btnShowArea.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnShowArea.ImagePressed = null;
            this.btnShowArea.ImageTextSpacing = 6;
            this.btnShowArea.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnShowArea.ImageTintHover = System.Drawing.Color.Empty;
            this.btnShowArea.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnShowArea.ImageTintOpacity = 0.5F;
            this.btnShowArea.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnShowArea.IsCLick = false;
            this.btnShowArea.IsNotChange = false;
            this.btnShowArea.IsRect = false;
            this.btnShowArea.IsUnGroup = true;
            this.btnShowArea.Location = new System.Drawing.Point(3, 339);
            this.btnShowArea.Multiline = false;
            this.btnShowArea.Name = "btnShowArea";
            this.btnShowArea.Size = new System.Drawing.Size(59, 50);
            this.btnShowArea.TabIndex = 42;
            this.btnShowArea.TextColor = System.Drawing.Color.White;
            this.btnShowArea.UseVisualStyleBackColor = false;
            this.btnShowArea.Click += new System.EventHandler(this.btnShowArea_Click);
            // 
            // pView
            // 
            this.pView.AutoScroll = true;
            this.pView.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pView.Controls.Add(this.imgView);
            this.pView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pView.Location = new System.Drawing.Point(0, 62);
            this.pView.Name = "pView";
            this.pView.Size = new System.Drawing.Size(1429, 697);
            this.pView.TabIndex = 6;
            this.pView.SizeChanged += new System.EventHandler(this.pView_SizeChanged);
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
            // tmContinuous
            // 
            this.tmContinuous.Interval = 1;
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
            this.tmSimulation.Interval = 5000;
            this.tmSimulation.Tick += new System.EventHandler(this.tmSimulation_Tick);
            // 
            // tmLive
            // 
            this.tmLive.Interval = 2;
            this.tmLive.Tick += new System.EventHandler(this.tmLive_Tick);
            // 
            // tmShow
            // 
            this.tmShow.Enabled = true;
            this.tmShow.Interval = 1000;
            this.tmShow.Tick += new System.EventHandler(this.tmShow_Tick);
            // 
            // split5
            // 
            this.split5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.split5.Dock = System.Windows.Forms.DockStyle.Top;
            this.split5.Location = new System.Drawing.Point(0, 57);
            this.split5.Name = "split5";
            this.split5.Size = new System.Drawing.Size(1494, 5);
            this.split5.TabIndex = 30;
            this.split5.TabStop = false;
            // 
            // pBtn
            // 
            this.pBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.pBtn.Controls.Add(this.split4);
            this.pBtn.Controls.Add(this.btnTypeTrig);
            this.pBtn.Controls.Add(this.btnMenu);
            this.pBtn.Controls.Add(this.split3);
            this.pBtn.Controls.Add(this.btnLive);
            this.pBtn.Controls.Add(this.split2);
            this.pBtn.Controls.Add(this.btnContinuous);
            this.pBtn.Controls.Add(this.split1);
            this.pBtn.Controls.Add(this.btnCap);
            this.pBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.pBtn.Location = new System.Drawing.Point(0, 0);
            this.pBtn.Name = "pBtn";
            this.pBtn.Padding = new System.Windows.Forms.Padding(5);
            this.pBtn.Size = new System.Drawing.Size(1494, 57);
            this.pBtn.TabIndex = 31;
            this.pBtn.SizeChanged += new System.EventHandler(this.pBtn_SizeChanged);
            this.pBtn.Paint += new System.Windows.Forms.PaintEventHandler(this.pBtn_Paint_1);
            // 
            // split4
            // 
            this.split4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(162)))), ((int)(((byte)(162)))));
            this.split4.Location = new System.Drawing.Point(399, 5);
            this.split4.MinExtra = 1;
            this.split4.MinSize = 10;
            this.split4.Name = "split4";
            this.split4.Size = new System.Drawing.Size(10, 47);
            this.split4.TabIndex = 40;
            this.split4.TabStop = false;
            // 
            // btnTypeTrig
            // 
            this.btnTypeTrig.AutoFont = true;
            this.btnTypeTrig.AutoFontHeightRatio = 0.75F;
            this.btnTypeTrig.AutoFontMax = 100F;
            this.btnTypeTrig.AutoFontMin = 8F;
            this.btnTypeTrig.AutoFontWidthRatio = 1F;
            this.btnTypeTrig.AutoImage = true;
            this.btnTypeTrig.AutoImageMaxRatio = 0.75F;
            this.btnTypeTrig.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnTypeTrig.AutoImageTint = true;
            this.btnTypeTrig.BackColor = System.Drawing.Color.Silver;
            this.btnTypeTrig.BackgroundColor = System.Drawing.Color.Silver;
            this.btnTypeTrig.BorderColor = System.Drawing.Color.Silver;
            this.btnTypeTrig.BorderRadius = 5;
            this.btnTypeTrig.BorderSize = 1;
            this.btnTypeTrig.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnTypeTrig.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnTypeTrig.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnTypeTrig.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnTypeTrig.Corner = BeeGlobal.Corner.Both;
            this.btnTypeTrig.DebounceResizeMs = 16;
            this.btnTypeTrig.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTypeTrig.FlatAppearance.BorderSize = 0;
            this.btnTypeTrig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTypeTrig.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.23438F);
            this.btnTypeTrig.ForeColor = System.Drawing.Color.Black;
            this.btnTypeTrig.Image = null;
            this.btnTypeTrig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTypeTrig.ImageDisabled = null;
            this.btnTypeTrig.ImageHover = null;
            this.btnTypeTrig.ImageNormal = null;
            this.btnTypeTrig.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnTypeTrig.ImagePressed = null;
            this.btnTypeTrig.ImageTextSpacing = 0;
            this.btnTypeTrig.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnTypeTrig.ImageTintHover = System.Drawing.Color.Empty;
            this.btnTypeTrig.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnTypeTrig.ImageTintOpacity = 0.5F;
            this.btnTypeTrig.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnTypeTrig.IsCLick = false;
            this.btnTypeTrig.IsNotChange = true;
            this.btnTypeTrig.IsRect = false;
            this.btnTypeTrig.IsUnGroup = true;
            this.btnTypeTrig.Location = new System.Drawing.Point(296, 5);
            this.btnTypeTrig.Margin = new System.Windows.Forms.Padding(5);
            this.btnTypeTrig.Multiline = false;
            this.btnTypeTrig.Name = "btnTypeTrig";
            this.btnTypeTrig.Size = new System.Drawing.Size(103, 47);
            this.btnTypeTrig.TabIndex = 39;
            this.btnTypeTrig.Text = "Trig Internal";
            this.btnTypeTrig.TextColor = System.Drawing.Color.Black;
            this.btnTypeTrig.UseVisualStyleBackColor = false;
            this.btnTypeTrig.Click += new System.EventHandler(this.btnTypeTrig_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnMenu.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(173)))), ((int)(((byte)(245)))));
            this.btnMenu.Image = global::BeeUi.Properties.Resources.Menu;
            this.btnMenu.Location = new System.Drawing.Point(1429, 5);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(60, 47);
            this.btnMenu.TabIndex = 38;
            this.btnMenu.UseVisualStyleBackColor = false;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click_1);
            // 
            // split3
            // 
            this.split3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(162)))), ((int)(((byte)(162)))));
            this.split3.Location = new System.Drawing.Point(291, 5);
            this.split3.MinExtra = 1;
            this.split3.MinSize = 10;
            this.split3.Name = "split3";
            this.split3.Size = new System.Drawing.Size(5, 47);
            this.split3.TabIndex = 12;
            this.split3.TabStop = false;
            // 
            // btnLive
            // 
            this.btnLive.AutoFont = true;
            this.btnLive.AutoFontHeightRatio = 0.75F;
            this.btnLive.AutoFontMax = 100F;
            this.btnLive.AutoFontMin = 8F;
            this.btnLive.AutoFontWidthRatio = 0.92F;
            this.btnLive.AutoImage = true;
            this.btnLive.AutoImageMaxRatio = 0.95F;
            this.btnLive.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnLive.AutoImageTint = true;
            this.btnLive.BackColor = System.Drawing.Color.Silver;
            this.btnLive.BackgroundColor = System.Drawing.Color.Silver;
            this.btnLive.BorderColor = System.Drawing.Color.Silver;
            this.btnLive.BorderRadius = 5;
            this.btnLive.BorderSize = 1;
            this.btnLive.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnLive.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnLive.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnLive.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnLive.Corner = BeeGlobal.Corner.Both;
            this.btnLive.DebounceResizeMs = 16;
            this.btnLive.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnLive.Enabled = false;
            this.btnLive.FlatAppearance.BorderSize = 0;
            this.btnLive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnLive.ForeColor = System.Drawing.Color.Black;
            this.btnLive.Image = global::BeeUi.Properties.Resources.Record_2;
            this.btnLive.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLive.ImageDisabled = null;
            this.btnLive.ImageHover = null;
            this.btnLive.ImageNormal = null;
            this.btnLive.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnLive.ImagePressed = null;
            this.btnLive.ImageTextSpacing = 0;
            this.btnLive.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnLive.ImageTintHover = System.Drawing.Color.Empty;
            this.btnLive.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnLive.ImageTintOpacity = 0.5F;
            this.btnLive.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnLive.IsCLick = false;
            this.btnLive.IsNotChange = false;
            this.btnLive.IsRect = false;
            this.btnLive.IsUnGroup = true;
            this.btnLive.Location = new System.Drawing.Point(181, 5);
            this.btnLive.Margin = new System.Windows.Forms.Padding(5);
            this.btnLive.Multiline = false;
            this.btnLive.Name = "btnLive";
            this.btnLive.Size = new System.Drawing.Size(110, 47);
            this.btnLive.TabIndex = 11;
            this.btnLive.Tag = "";
            this.btnLive.Text = "Live Camera";
            this.btnLive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLive.TextColor = System.Drawing.Color.Black;
            this.btnLive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLive.UseVisualStyleBackColor = false;
            this.btnLive.Click += new System.EventHandler(this.btnSer_Click);
            // 
            // split2
            // 
            this.split2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(162)))), ((int)(((byte)(162)))));
            this.split2.Location = new System.Drawing.Point(176, 5);
            this.split2.MinExtra = 1;
            this.split2.MinSize = 10;
            this.split2.Name = "split2";
            this.split2.Size = new System.Drawing.Size(5, 47);
            this.split2.TabIndex = 11;
            this.split2.TabStop = false;
            // 
            // btnContinuous
            // 
            this.btnContinuous.AutoFont = true;
            this.btnContinuous.AutoFontHeightRatio = 0.75F;
            this.btnContinuous.AutoFontMax = 100F;
            this.btnContinuous.AutoFontMin = 8F;
            this.btnContinuous.AutoFontWidthRatio = 0.92F;
            this.btnContinuous.AutoImage = true;
            this.btnContinuous.AutoImageMaxRatio = 0.75F;
            this.btnContinuous.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnContinuous.AutoImageTint = true;
            this.btnContinuous.BackColor = System.Drawing.Color.Silver;
            this.btnContinuous.BackgroundColor = System.Drawing.Color.Silver;
            this.btnContinuous.BorderColor = System.Drawing.Color.Silver;
            this.btnContinuous.BorderRadius = 5;
            this.btnContinuous.BorderSize = 1;
            this.btnContinuous.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnContinuous.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnContinuous.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnContinuous.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnContinuous.Corner = BeeGlobal.Corner.Both;
            this.btnContinuous.DebounceResizeMs = 16;
            this.btnContinuous.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnContinuous.FlatAppearance.BorderSize = 0;
            this.btnContinuous.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinuous.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnContinuous.ForeColor = System.Drawing.Color.Black;
            this.btnContinuous.Image = global::BeeUi.Properties.Resources.Play_2;
            this.btnContinuous.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnContinuous.ImageDisabled = null;
            this.btnContinuous.ImageHover = null;
            this.btnContinuous.ImageNormal = null;
            this.btnContinuous.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnContinuous.ImagePressed = null;
            this.btnContinuous.ImageTextSpacing = 0;
            this.btnContinuous.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnContinuous.ImageTintHover = System.Drawing.Color.Empty;
            this.btnContinuous.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnContinuous.ImageTintOpacity = 0.5F;
            this.btnContinuous.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnContinuous.IsCLick = false;
            this.btnContinuous.IsNotChange = false;
            this.btnContinuous.IsRect = false;
            this.btnContinuous.IsUnGroup = true;
            this.btnContinuous.Location = new System.Drawing.Point(85, 5);
            this.btnContinuous.Margin = new System.Windows.Forms.Padding(5);
            this.btnContinuous.Multiline = false;
            this.btnContinuous.Name = "btnContinuous";
            this.btnContinuous.Size = new System.Drawing.Size(91, 47);
            this.btnContinuous.TabIndex = 10;
            this.btnContinuous.Text = "Continuous";
            this.btnContinuous.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnContinuous.TextColor = System.Drawing.Color.Black;
            this.btnContinuous.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnContinuous.UseVisualStyleBackColor = false;
            this.btnContinuous.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // split1
            // 
            this.split1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(162)))), ((int)(((byte)(162)))));
            this.split1.Location = new System.Drawing.Point(80, 5);
            this.split1.MinExtra = 1;
            this.split1.MinSize = 10;
            this.split1.Name = "split1";
            this.split1.Size = new System.Drawing.Size(5, 47);
            this.split1.TabIndex = 10;
            this.split1.TabStop = false;
            // 
            // btnCap
            // 
            this.btnCap.AutoFont = true;
            this.btnCap.AutoFontHeightRatio = 0.75F;
            this.btnCap.AutoFontMax = 100F;
            this.btnCap.AutoFontMin = 8F;
            this.btnCap.AutoFontWidthRatio = 0.92F;
            this.btnCap.AutoImage = true;
            this.btnCap.AutoImageMaxRatio = 0.75F;
            this.btnCap.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCap.AutoImageTint = true;
            this.btnCap.BackColor = System.Drawing.Color.Silver;
            this.btnCap.BackgroundColor = System.Drawing.Color.Silver;
            this.btnCap.BorderColor = System.Drawing.Color.Silver;
            this.btnCap.BorderRadius = 5;
            this.btnCap.BorderSize = 1;
            this.btnCap.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnCap.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnCap.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnCap.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCap.Corner = BeeGlobal.Corner.Both;
            this.btnCap.DebounceResizeMs = 16;
            this.btnCap.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCap.FlatAppearance.BorderSize = 0;
            this.btnCap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCap.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnCap.ForeColor = System.Drawing.Color.Black;
            this.btnCap.Image = global::BeeUi.Properties.Resources.Natural_User_Interface_2;
            this.btnCap.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCap.ImageDisabled = null;
            this.btnCap.ImageHover = null;
            this.btnCap.ImageNormal = null;
            this.btnCap.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCap.ImagePressed = null;
            this.btnCap.ImageTextSpacing = 0;
            this.btnCap.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCap.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCap.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCap.ImageTintOpacity = 0.5F;
            this.btnCap.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCap.IsCLick = false;
            this.btnCap.IsNotChange = false;
            this.btnCap.IsRect = false;
            this.btnCap.IsUnGroup = true;
            this.btnCap.Location = new System.Drawing.Point(5, 5);
            this.btnCap.Margin = new System.Windows.Forms.Padding(5);
            this.btnCap.Multiline = false;
            this.btnCap.Name = "btnCap";
            this.btnCap.Size = new System.Drawing.Size(75, 47);
            this.btnCap.TabIndex = 9;
            this.btnCap.Text = "Trigger";
            this.btnCap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCap.TextColor = System.Drawing.Color.Black;
            this.btnCap.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCap.UseVisualStyleBackColor = false;
            this.btnCap.Click += new System.EventHandler(this.btnCap_Click);
            // 
            // tmEnableControl
            // 
            this.tmEnableControl.Interval = 200;
            this.tmEnableControl.Tick += new System.EventHandler(this.tmEnableControl_Tick);
            // 
            // workLiveWebcam
            // 
            this.workLiveWebcam.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workLiveWebcam_RunWorkerCompleted);
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.pView);
            this.Controls.Add(this.pMenu);
            this.Controls.Add(this.split5);
            this.Controls.Add(this.pBtn);
            this.DoubleBuffered = true;
            this.Name = "View";
            this.Size = new System.Drawing.Size(1494, 759);
            this.Load += new System.EventHandler(this.View_Load);
            this.pMenu.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pView.ResumeLayout(false);
            this.pBtn.ResumeLayout(false);
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
        public RJButton btnCap;
        public RJButton btnContinuous;
        public RJButton btnLive;
        private System.ComponentModel.BackgroundWorker workInsert;
        private System.Windows.Forms.OpenFileDialog openFile;
        public System.ComponentModel.BackgroundWorker workReadCCD;
        public Cyotek.Windows.Forms.ImageBox imgView;
        private System.Windows.Forms.Timer tmRefresh;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        public System.Windows.Forms.Timer tmOut;
        public System.Windows.Forms.Panel pMenu;
        public System.Windows.Forms.Button btnMenu;
        public System.Windows.Forms.Timer tmContinuous;
        private System.ComponentModel.BackgroundWorker workTrig;
        public System.Windows.Forms.Timer tmPress;
        public System.ComponentModel.BackgroundWorker workPlay;
        public RJButton btnTypeTrig;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        public System.Windows.Forms.Timer tmSimulation;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RJButton btnGird;
        public RJButton btnFull;
        public RJButton btnZoomIn;
        public RJButton btnZoomOut;
        public RJButton btnShowCenter;
        private RJButton btnShowArea;
        public System.Windows.Forms.Timer tmLive;
        private System.Windows.Forms.Timer tmShow;
        public System.Windows.Forms.Panel pBtn;
        private System.Windows.Forms.Timer tmEnableControl;
        public System.Windows.Forms.Splitter split5;
        public System.Windows.Forms.Splitter split4;
        public System.Windows.Forms.Splitter split3;
        public System.Windows.Forms.Splitter split2;
        public System.Windows.Forms.Splitter split1;
        private RJButton btnPan;
        public System.ComponentModel.BackgroundWorker workLiveWebcam;
    }
}
