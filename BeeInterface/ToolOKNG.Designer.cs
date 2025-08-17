using BeeCore;
using BeeGlobal;

namespace BeeInterface
{
    partial class ToolOKNG
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolOKNG));
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabP1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.btnTest = new BeeInterface.RJButton();
            this.pView = new System.Windows.Forms.Panel();
            this.imgOK = new Cyotek.Windows.Forms.ImageBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddNG = new BeeInterface.RJButton();
            this.btnRemoveAllNG = new BeeInterface.RJButton();
            this.btnUndoNG = new BeeInterface.RJButton();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddOK = new BeeInterface.RJButton();
            this.btnRemoveAllOK = new BeeInterface.RJButton();
            this.btnUndoOK = new BeeInterface.RJButton();
            this.pNG = new System.Windows.Forms.Panel();
            this.imgNG = new Cyotek.Windows.Forms.ImageBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnLearning = new BeeInterface.RJButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClear = new BeeInterface.RJButton();
            this.btnCropRect = new BeeInterface.RJButton();
            this.btnCropArea = new BeeInterface.RJButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.pMain = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.layLimitCouter = new System.Windows.Forms.TableLayoutPanel();
            this.btnMulti = new BeeInterface.RJButton();
            this.btnSingle = new BeeInterface.RJButton();
            this.numCPU = new BeeInterface.CustomNumericEx();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnEnResizeSample = new BeeInterface.RJButton();
            this.pCany = new System.Windows.Forms.Panel();
            this.btnAreaBlack = new BeeInterface.RJButton();
            this.btnAreaWhite = new BeeInterface.RJButton();
            this.workModel = new System.ComponentModel.BackgroundWorker();
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.trackResize = new BeeInterface.AdjustBarEx();
            this.tabControl2.SuspendLayout();
            this.tabP1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pView.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.pNG.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.pMain.SuspendLayout();
            this.layLimitCouter.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pCany.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabP1);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(418, 1047);
            this.tabControl2.TabIndex = 17;
            // 
            // tabP1
            // 
            this.tabP1.BackColor = System.Drawing.SystemColors.Control;
            this.tabP1.Controls.Add(this.tableLayoutPanel1);
            this.tabP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabP1.Location = new System.Drawing.Point(4, 34);
            this.tabP1.Name = "tabP1";
            this.tabP1.Padding = new System.Windows.Forms.Padding(3);
            this.tabP1.Size = new System.Drawing.Size(410, 1009);
            this.tabP1.TabIndex = 0;
            this.tabP1.Text = "Basic";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnTest, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.pView, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.pNG, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnLearning, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 13;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(404, 1003);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(5, 290);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(396, 25);
            this.label5.TabIndex = 69;
            this.label5.Text = "Set NG";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTest
            // 
            this.btnTest.AutoFont = false;
            this.btnTest.AutoFontHeightRatio = 0.75F;
            this.btnTest.AutoFontMax = 100F;
            this.btnTest.AutoFontMin = 6F;
            this.btnTest.AutoFontWidthRatio = 0.92F;
            this.btnTest.AutoImage = true;
            this.btnTest.AutoImageMaxRatio = 0.75F;
            this.btnTest.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnTest.AutoImageTint = true;
            this.btnTest.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnTest.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnTest.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnTest.BorderRadius = 10;
            this.btnTest.BorderSize = 1;
            this.btnTest.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnTest.Corner = BeeGlobal.Corner.Both;
            this.btnTest.DebounceResizeMs = 16;
            this.btnTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.ForeColor = System.Drawing.Color.Black;
            this.btnTest.Image = null;
            this.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTest.ImageDisabled = null;
            this.btnTest.ImageHover = null;
            this.btnTest.ImageNormal = null;
            this.btnTest.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnTest.ImagePressed = null;
            this.btnTest.ImageTextSpacing = 6;
            this.btnTest.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnTest.ImageTintHover = System.Drawing.Color.Empty;
            this.btnTest.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnTest.ImageTintOpacity = 0.5F;
            this.btnTest.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnTest.IsCLick = false;
            this.btnTest.IsNotChange = true;
            this.btnTest.IsRect = false;
            this.btnTest.IsUnGroup = true;
            this.btnTest.Location = new System.Drawing.Point(2, 569);
            this.btnTest.Margin = new System.Windows.Forms.Padding(2, 10, 2, 2);
            this.btnTest.Multiline = false;
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(400, 88);
            this.btnTest.TabIndex = 68;
            this.btnTest.Text = "Inspect";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // pView
            // 
            this.pView.AutoScroll = true;
            this.pView.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pView.Controls.Add(this.imgOK);
            this.pView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pView.Location = new System.Drawing.Point(3, 162);
            this.pView.Name = "pView";
            this.pView.Size = new System.Drawing.Size(398, 120);
            this.pView.TabIndex = 67;
            // 
            // imgOK
            // 
            this.imgOK.AllowFreePan = false;
            this.imgOK.AllowZoom = false;
            this.imgOK.AlwaysShowHScroll = true;
            this.imgOK.AlwaysShowVScroll = true;
            this.imgOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.imgOK.AutoCenter = false;
            this.imgOK.AutoPan = false;
            this.imgOK.AutoScroll = false;
            this.imgOK.GridColor = System.Drawing.Color.Transparent;
            this.imgOK.GridScale = Cyotek.Windows.Forms.ImageBoxGridScale.None;
            this.imgOK.Location = new System.Drawing.Point(0, 0);
            this.imgOK.Name = "imgOK";
            this.imgOK.PanMode = Cyotek.Windows.Forms.ImageBoxPanMode.None;
            this.imgOK.ShortcutsEnabled = false;
            this.imgOK.Size = new System.Drawing.Size(395, 137);
            this.imgOK.TabIndex = 1;
            this.imgOK.TextBackColor = System.Drawing.Color.White;
            this.imgOK.TextDisplayMode = Cyotek.Windows.Forms.ImageBoxGridDisplayMode.None;
            this.imgOK.Paint += new System.Windows.Forms.PaintEventHandler(this.imgOK_Paint);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(5, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(396, 25);
            this.label2.TabIndex = 66;
            this.label2.Text = "Set OK";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel5.Controls.Add(this.btnAddNG, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnRemoveAllNG, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnUndoNG, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(5, 315);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(396, 53);
            this.tableLayoutPanel5.TabIndex = 65;
            // 
            // btnAddNG
            // 
            this.btnAddNG.AutoFont = true;
            this.btnAddNG.AutoFontHeightRatio = 0.75F;
            this.btnAddNG.AutoFontMax = 100F;
            this.btnAddNG.AutoFontMin = 6F;
            this.btnAddNG.AutoFontWidthRatio = 0.92F;
            this.btnAddNG.AutoImage = true;
            this.btnAddNG.AutoImageMaxRatio = 0.75F;
            this.btnAddNG.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnAddNG.AutoImageTint = true;
            this.btnAddNG.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddNG.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddNG.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddNG.BorderRadius = 5;
            this.btnAddNG.BorderSize = 1;
            this.btnAddNG.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnAddNG.Corner = BeeGlobal.Corner.Both;
            this.btnAddNG.DebounceResizeMs = 16;
            this.btnAddNG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddNG.FlatAppearance.BorderSize = 0;
            this.btnAddNG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.48438F);
            this.btnAddNG.ForeColor = System.Drawing.Color.Black;
            this.btnAddNG.Image = null;
            this.btnAddNG.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddNG.ImageDisabled = null;
            this.btnAddNG.ImageHover = null;
            this.btnAddNG.ImageNormal = null;
            this.btnAddNG.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnAddNG.ImagePressed = null;
            this.btnAddNG.ImageTextSpacing = 6;
            this.btnAddNG.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnAddNG.ImageTintHover = System.Drawing.Color.Empty;
            this.btnAddNG.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnAddNG.ImageTintOpacity = 0.5F;
            this.btnAddNG.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnAddNG.IsCLick = false;
            this.btnAddNG.IsNotChange = true;
            this.btnAddNG.IsRect = false;
            this.btnAddNG.IsUnGroup = true;
            this.btnAddNG.Location = new System.Drawing.Point(0, 0);
            this.btnAddNG.Margin = new System.Windows.Forms.Padding(0);
            this.btnAddNG.Multiline = false;
            this.btnAddNG.Name = "btnAddNG";
            this.btnAddNG.Size = new System.Drawing.Size(128, 53);
            this.btnAddNG.TabIndex = 63;
            this.btnAddNG.Text = "Add NG";
            this.btnAddNG.TextColor = System.Drawing.Color.Black;
            this.btnAddNG.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddNG.UseVisualStyleBackColor = false;
            this.btnAddNG.Click += new System.EventHandler(this.btnAddNG_Click);
            // 
            // btnRemoveAllNG
            // 
            this.btnRemoveAllNG.AutoFont = true;
            this.btnRemoveAllNG.AutoFontHeightRatio = 0.75F;
            this.btnRemoveAllNG.AutoFontMax = 100F;
            this.btnRemoveAllNG.AutoFontMin = 6F;
            this.btnRemoveAllNG.AutoFontWidthRatio = 0.92F;
            this.btnRemoveAllNG.AutoImage = true;
            this.btnRemoveAllNG.AutoImageMaxRatio = 0.75F;
            this.btnRemoveAllNG.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnRemoveAllNG.AutoImageTint = true;
            this.btnRemoveAllNG.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnRemoveAllNG.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnRemoveAllNG.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnRemoveAllNG.BorderRadius = 10;
            this.btnRemoveAllNG.BorderSize = 1;
            this.btnRemoveAllNG.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnRemoveAllNG.Corner = BeeGlobal.Corner.Right;
            this.btnRemoveAllNG.DebounceResizeMs = 16;
            this.btnRemoveAllNG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemoveAllNG.FlatAppearance.BorderSize = 0;
            this.btnRemoveAllNG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveAllNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.btnRemoveAllNG.ForeColor = System.Drawing.Color.Black;
            this.btnRemoveAllNG.Image = null;
            this.btnRemoveAllNG.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveAllNG.ImageDisabled = null;
            this.btnRemoveAllNG.ImageHover = null;
            this.btnRemoveAllNG.ImageNormal = null;
            this.btnRemoveAllNG.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnRemoveAllNG.ImagePressed = null;
            this.btnRemoveAllNG.ImageTextSpacing = 6;
            this.btnRemoveAllNG.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnRemoveAllNG.ImageTintHover = System.Drawing.Color.Empty;
            this.btnRemoveAllNG.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnRemoveAllNG.ImageTintOpacity = 0.5F;
            this.btnRemoveAllNG.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnRemoveAllNG.IsCLick = false;
            this.btnRemoveAllNG.IsNotChange = true;
            this.btnRemoveAllNG.IsRect = false;
            this.btnRemoveAllNG.IsUnGroup = false;
            this.btnRemoveAllNG.Location = new System.Drawing.Point(270, 3);
            this.btnRemoveAllNG.Multiline = false;
            this.btnRemoveAllNG.Name = "btnRemoveAllNG";
            this.btnRemoveAllNG.Size = new System.Drawing.Size(123, 47);
            this.btnRemoveAllNG.TabIndex = 4;
            this.btnRemoveAllNG.Text = "Clear All";
            this.btnRemoveAllNG.TextColor = System.Drawing.Color.Black;
            this.btnRemoveAllNG.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRemoveAllNG.UseVisualStyleBackColor = false;
            this.btnRemoveAllNG.Click += new System.EventHandler(this.btnRemoveAllNG_Click);
            // 
            // btnUndoNG
            // 
            this.btnUndoNG.AutoFont = true;
            this.btnUndoNG.AutoFontHeightRatio = 0.75F;
            this.btnUndoNG.AutoFontMax = 100F;
            this.btnUndoNG.AutoFontMin = 6F;
            this.btnUndoNG.AutoFontWidthRatio = 0.92F;
            this.btnUndoNG.AutoImage = true;
            this.btnUndoNG.AutoImageMaxRatio = 0.75F;
            this.btnUndoNG.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnUndoNG.AutoImageTint = true;
            this.btnUndoNG.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnUndoNG.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnUndoNG.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnUndoNG.BorderRadius = 5;
            this.btnUndoNG.BorderSize = 1;
            this.btnUndoNG.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnUndoNG.Corner = BeeGlobal.Corner.None;
            this.btnUndoNG.DebounceResizeMs = 16;
            this.btnUndoNG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUndoNG.FlatAppearance.BorderSize = 0;
            this.btnUndoNG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUndoNG.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.btnUndoNG.ForeColor = System.Drawing.Color.Black;
            this.btnUndoNG.Image = null;
            this.btnUndoNG.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUndoNG.ImageDisabled = null;
            this.btnUndoNG.ImageHover = null;
            this.btnUndoNG.ImageNormal = null;
            this.btnUndoNG.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnUndoNG.ImagePressed = null;
            this.btnUndoNG.ImageTextSpacing = 6;
            this.btnUndoNG.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnUndoNG.ImageTintHover = System.Drawing.Color.Empty;
            this.btnUndoNG.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnUndoNG.ImageTintOpacity = 0.5F;
            this.btnUndoNG.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnUndoNG.IsCLick = false;
            this.btnUndoNG.IsNotChange = true;
            this.btnUndoNG.IsRect = false;
            this.btnUndoNG.IsUnGroup = false;
            this.btnUndoNG.Location = new System.Drawing.Point(131, 3);
            this.btnUndoNG.Multiline = false;
            this.btnUndoNG.Name = "btnUndoNG";
            this.btnUndoNG.Size = new System.Drawing.Size(133, 47);
            this.btnUndoNG.TabIndex = 3;
            this.btnUndoNG.Text = "Undo";
            this.btnUndoNG.TextColor = System.Drawing.Color.Black;
            this.btnUndoNG.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUndoNG.UseVisualStyleBackColor = false;
            this.btnUndoNG.Click += new System.EventHandler(this.btnUndoNG_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel4.Controls.Add(this.btnAddOK, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnRemoveAllOK, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnUndoOK, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(5, 109);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(396, 50);
            this.tableLayoutPanel4.TabIndex = 64;
            // 
            // btnAddOK
            // 
            this.btnAddOK.AutoFont = true;
            this.btnAddOK.AutoFontHeightRatio = 0.75F;
            this.btnAddOK.AutoFontMax = 100F;
            this.btnAddOK.AutoFontMin = 6F;
            this.btnAddOK.AutoFontWidthRatio = 0.92F;
            this.btnAddOK.AutoImage = true;
            this.btnAddOK.AutoImageMaxRatio = 0.75F;
            this.btnAddOK.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnAddOK.AutoImageTint = true;
            this.btnAddOK.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddOK.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddOK.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddOK.BorderRadius = 5;
            this.btnAddOK.BorderSize = 1;
            this.btnAddOK.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnAddOK.Corner = BeeGlobal.Corner.Both;
            this.btnAddOK.DebounceResizeMs = 16;
            this.btnAddOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddOK.FlatAppearance.BorderSize = 0;
            this.btnAddOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnAddOK.ForeColor = System.Drawing.Color.Black;
            this.btnAddOK.Image = null;
            this.btnAddOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddOK.ImageDisabled = null;
            this.btnAddOK.ImageHover = null;
            this.btnAddOK.ImageNormal = null;
            this.btnAddOK.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnAddOK.ImagePressed = null;
            this.btnAddOK.ImageTextSpacing = 6;
            this.btnAddOK.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnAddOK.ImageTintHover = System.Drawing.Color.Empty;
            this.btnAddOK.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnAddOK.ImageTintOpacity = 0.5F;
            this.btnAddOK.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnAddOK.IsCLick = false;
            this.btnAddOK.IsNotChange = true;
            this.btnAddOK.IsRect = false;
            this.btnAddOK.IsUnGroup = true;
            this.btnAddOK.Location = new System.Drawing.Point(3, 3);
            this.btnAddOK.Multiline = false;
            this.btnAddOK.Name = "btnAddOK";
            this.btnAddOK.Size = new System.Drawing.Size(122, 44);
            this.btnAddOK.TabIndex = 63;
            this.btnAddOK.Text = "Add OK";
            this.btnAddOK.TextColor = System.Drawing.Color.Black;
            this.btnAddOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddOK.UseVisualStyleBackColor = false;
            this.btnAddOK.Click += new System.EventHandler(this.btnAddOK_Click);
            // 
            // btnRemoveAllOK
            // 
            this.btnRemoveAllOK.AutoFont = true;
            this.btnRemoveAllOK.AutoFontHeightRatio = 0.75F;
            this.btnRemoveAllOK.AutoFontMax = 100F;
            this.btnRemoveAllOK.AutoFontMin = 6F;
            this.btnRemoveAllOK.AutoFontWidthRatio = 0.92F;
            this.btnRemoveAllOK.AutoImage = true;
            this.btnRemoveAllOK.AutoImageMaxRatio = 0.75F;
            this.btnRemoveAllOK.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnRemoveAllOK.AutoImageTint = true;
            this.btnRemoveAllOK.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnRemoveAllOK.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnRemoveAllOK.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnRemoveAllOK.BorderRadius = 10;
            this.btnRemoveAllOK.BorderSize = 1;
            this.btnRemoveAllOK.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnRemoveAllOK.Corner = BeeGlobal.Corner.Right;
            this.btnRemoveAllOK.DebounceResizeMs = 16;
            this.btnRemoveAllOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemoveAllOK.FlatAppearance.BorderSize = 0;
            this.btnRemoveAllOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveAllOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnRemoveAllOK.ForeColor = System.Drawing.Color.Black;
            this.btnRemoveAllOK.Image = null;
            this.btnRemoveAllOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveAllOK.ImageDisabled = null;
            this.btnRemoveAllOK.ImageHover = null;
            this.btnRemoveAllOK.ImageNormal = null;
            this.btnRemoveAllOK.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnRemoveAllOK.ImagePressed = null;
            this.btnRemoveAllOK.ImageTextSpacing = 6;
            this.btnRemoveAllOK.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnRemoveAllOK.ImageTintHover = System.Drawing.Color.Empty;
            this.btnRemoveAllOK.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnRemoveAllOK.ImageTintOpacity = 0.5F;
            this.btnRemoveAllOK.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnRemoveAllOK.IsCLick = false;
            this.btnRemoveAllOK.IsNotChange = true;
            this.btnRemoveAllOK.IsRect = false;
            this.btnRemoveAllOK.IsUnGroup = false;
            this.btnRemoveAllOK.Location = new System.Drawing.Point(270, 3);
            this.btnRemoveAllOK.Multiline = false;
            this.btnRemoveAllOK.Name = "btnRemoveAllOK";
            this.btnRemoveAllOK.Size = new System.Drawing.Size(123, 44);
            this.btnRemoveAllOK.TabIndex = 4;
            this.btnRemoveAllOK.Text = "Clear All";
            this.btnRemoveAllOK.TextColor = System.Drawing.Color.Black;
            this.btnRemoveAllOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRemoveAllOK.UseVisualStyleBackColor = false;
            this.btnRemoveAllOK.Click += new System.EventHandler(this.btnRemoveAllOK_Click);
            // 
            // btnUndoOK
            // 
            this.btnUndoOK.AutoFont = true;
            this.btnUndoOK.AutoFontHeightRatio = 0.75F;
            this.btnUndoOK.AutoFontMax = 100F;
            this.btnUndoOK.AutoFontMin = 6F;
            this.btnUndoOK.AutoFontWidthRatio = 0.92F;
            this.btnUndoOK.AutoImage = true;
            this.btnUndoOK.AutoImageMaxRatio = 0.75F;
            this.btnUndoOK.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnUndoOK.AutoImageTint = true;
            this.btnUndoOK.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnUndoOK.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnUndoOK.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnUndoOK.BorderRadius = 5;
            this.btnUndoOK.BorderSize = 1;
            this.btnUndoOK.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnUndoOK.Corner = BeeGlobal.Corner.None;
            this.btnUndoOK.DebounceResizeMs = 16;
            this.btnUndoOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUndoOK.FlatAppearance.BorderSize = 0;
            this.btnUndoOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUndoOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnUndoOK.ForeColor = System.Drawing.Color.Black;
            this.btnUndoOK.Image = null;
            this.btnUndoOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUndoOK.ImageDisabled = null;
            this.btnUndoOK.ImageHover = null;
            this.btnUndoOK.ImageNormal = null;
            this.btnUndoOK.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnUndoOK.ImagePressed = null;
            this.btnUndoOK.ImageTextSpacing = 6;
            this.btnUndoOK.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnUndoOK.ImageTintHover = System.Drawing.Color.Empty;
            this.btnUndoOK.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnUndoOK.ImageTintOpacity = 0.5F;
            this.btnUndoOK.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnUndoOK.IsCLick = false;
            this.btnUndoOK.IsNotChange = true;
            this.btnUndoOK.IsRect = false;
            this.btnUndoOK.IsUnGroup = false;
            this.btnUndoOK.Location = new System.Drawing.Point(131, 3);
            this.btnUndoOK.Multiline = false;
            this.btnUndoOK.Name = "btnUndoOK";
            this.btnUndoOK.Size = new System.Drawing.Size(133, 44);
            this.btnUndoOK.TabIndex = 3;
            this.btnUndoOK.Text = "Undo";
            this.btnUndoOK.TextColor = System.Drawing.Color.Black;
            this.btnUndoOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUndoOK.UseVisualStyleBackColor = false;
            this.btnUndoOK.Click += new System.EventHandler(this.btnUndoOK_Click);
            // 
            // pNG
            // 
            this.pNG.AutoScroll = true;
            this.pNG.Controls.Add(this.imgNG);
            this.pNG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pNG.Location = new System.Drawing.Point(3, 371);
            this.pNG.Name = "pNG";
            this.pNG.Size = new System.Drawing.Size(398, 120);
            this.pNG.TabIndex = 63;
            // 
            // imgNG
            // 
            this.imgNG.AllowFreePan = false;
            this.imgNG.AllowZoom = false;
            this.imgNG.AlwaysShowHScroll = true;
            this.imgNG.AlwaysShowVScroll = true;
            this.imgNG.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.imgNG.AutoCenter = false;
            this.imgNG.AutoPan = false;
            this.imgNG.AutoScroll = false;
            this.imgNG.GridColor = System.Drawing.Color.Transparent;
            this.imgNG.GridScale = Cyotek.Windows.Forms.ImageBoxGridScale.None;
            this.imgNG.Location = new System.Drawing.Point(2, 0);
            this.imgNG.Name = "imgNG";
            this.imgNG.PanMode = Cyotek.Windows.Forms.ImageBoxPanMode.None;
            this.imgNG.ShortcutsEnabled = false;
            this.imgNG.Size = new System.Drawing.Size(393, 141);
            this.imgNG.TabIndex = 2;
            this.imgNG.TextBackColor = System.Drawing.Color.White;
            this.imgNG.TextDisplayMode = Cyotek.Windows.Forms.ImageBoxGridDisplayMode.None;
            this.imgNG.Paint += new System.Windows.Forms.PaintEventHandler(this.imgNG_Paint);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(5, 5);
            this.label13.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(396, 24);
            this.label13.TabIndex = 58;
            this.label13.Text = "Choose Area";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLearning
            // 
            this.btnLearning.AutoFont = true;
            this.btnLearning.AutoFontHeightRatio = 0.75F;
            this.btnLearning.AutoFontMax = 100F;
            this.btnLearning.AutoFontMin = 6F;
            this.btnLearning.AutoFontWidthRatio = 0.92F;
            this.btnLearning.AutoImage = true;
            this.btnLearning.AutoImageMaxRatio = 0.75F;
            this.btnLearning.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnLearning.AutoImageTint = true;
            this.btnLearning.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnLearning.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnLearning.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLearning.BackgroundImage")));
            this.btnLearning.BorderColor = System.Drawing.Color.Transparent;
            this.btnLearning.BorderRadius = 10;
            this.btnLearning.BorderSize = 1;
            this.btnLearning.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnLearning.Corner = BeeGlobal.Corner.Both;
            this.btnLearning.DebounceResizeMs = 16;
            this.btnLearning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLearning.FlatAppearance.BorderSize = 0;
            this.btnLearning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLearning.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.6875F);
            this.btnLearning.ForeColor = System.Drawing.Color.Black;
            this.btnLearning.Image = null;
            this.btnLearning.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLearning.ImageDisabled = null;
            this.btnLearning.ImageHover = null;
            this.btnLearning.ImageNormal = null;
            this.btnLearning.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnLearning.ImagePressed = null;
            this.btnLearning.ImageTextSpacing = 6;
            this.btnLearning.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnLearning.ImageTintHover = System.Drawing.Color.Empty;
            this.btnLearning.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnLearning.ImageTintOpacity = 0.5F;
            this.btnLearning.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnLearning.IsCLick = false;
            this.btnLearning.IsNotChange = true;
            this.btnLearning.IsRect = false;
            this.btnLearning.IsUnGroup = true;
            this.btnLearning.Location = new System.Drawing.Point(20, 499);
            this.btnLearning.Margin = new System.Windows.Forms.Padding(20, 5, 5, 5);
            this.btnLearning.Multiline = false;
            this.btnLearning.Name = "btnLearning";
            this.btnLearning.Size = new System.Drawing.Size(379, 55);
            this.btnLearning.TabIndex = 5;
            this.btnLearning.Text = "Train";
            this.btnLearning.TextColor = System.Drawing.Color.Black;
            this.btnLearning.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLearning.UseVisualStyleBackColor = false;
            this.btnLearning.Click += new System.EventHandler(this.btnLearning_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.29412F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.35294F));
            this.tableLayoutPanel3.Controls.Add(this.btnClear, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCropRect, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnCropArea, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 29);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(396, 50);
            this.tableLayoutPanel3.TabIndex = 41;
            // 
            // btnClear
            // 
            this.btnClear.AutoFont = true;
            this.btnClear.AutoFontHeightRatio = 0.75F;
            this.btnClear.AutoFontMax = 100F;
            this.btnClear.AutoFontMin = 6F;
            this.btnClear.AutoFontWidthRatio = 0.92F;
            this.btnClear.AutoImage = true;
            this.btnClear.AutoImageMaxRatio = 0.75F;
            this.btnClear.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnClear.AutoImageTint = true;
            this.btnClear.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnClear.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnClear.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnClear.BorderRadius = 10;
            this.btnClear.BorderSize = 1;
            this.btnClear.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnClear.Corner = BeeGlobal.Corner.Right;
            this.btnClear.DebounceResizeMs = 16;
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClear.Enabled = false;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Image = null;
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.ImageDisabled = null;
            this.btnClear.ImageHover = null;
            this.btnClear.ImageNormal = null;
            this.btnClear.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnClear.ImagePressed = null;
            this.btnClear.ImageTextSpacing = 6;
            this.btnClear.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnClear.ImageTintHover = System.Drawing.Color.Empty;
            this.btnClear.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnClear.ImageTintOpacity = 0.5F;
            this.btnClear.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnClear.IsCLick = false;
            this.btnClear.IsNotChange = false;
            this.btnClear.IsRect = false;
            this.btnClear.IsUnGroup = false;
            this.btnClear.Location = new System.Drawing.Point(267, 0);
            this.btnClear.Margin = new System.Windows.Forms.Padding(0);
            this.btnClear.Multiline = false;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(129, 50);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Area Mask";
            this.btnClear.TextColor = System.Drawing.Color.Black;
            this.btnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnCropRect
            // 
            this.btnCropRect.AutoFont = true;
            this.btnCropRect.AutoFontHeightRatio = 0.75F;
            this.btnCropRect.AutoFontMax = 100F;
            this.btnCropRect.AutoFontMin = 6F;
            this.btnCropRect.AutoFontWidthRatio = 0.92F;
            this.btnCropRect.AutoImage = true;
            this.btnCropRect.AutoImageMaxRatio = 0.75F;
            this.btnCropRect.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCropRect.AutoImageTint = true;
            this.btnCropRect.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropRect.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropRect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCropRect.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropRect.BorderRadius = 10;
            this.btnCropRect.BorderSize = 1;
            this.btnCropRect.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropRect.Corner = BeeGlobal.Corner.Left;
            this.btnCropRect.DebounceResizeMs = 16;
            this.btnCropRect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropRect.FlatAppearance.BorderSize = 0;
            this.btnCropRect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropRect.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.34375F);
            this.btnCropRect.ForeColor = System.Drawing.Color.Black;
            this.btnCropRect.Image = null;
            this.btnCropRect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropRect.ImageDisabled = null;
            this.btnCropRect.ImageHover = null;
            this.btnCropRect.ImageNormal = null;
            this.btnCropRect.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCropRect.ImagePressed = null;
            this.btnCropRect.ImageTextSpacing = 6;
            this.btnCropRect.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCropRect.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCropRect.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCropRect.ImageTintOpacity = 0.5F;
            this.btnCropRect.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCropRect.IsCLick = false;
            this.btnCropRect.IsNotChange = false;
            this.btnCropRect.IsRect = false;
            this.btnCropRect.IsUnGroup = false;
            this.btnCropRect.Location = new System.Drawing.Point(0, 0);
            this.btnCropRect.Margin = new System.Windows.Forms.Padding(0);
            this.btnCropRect.Multiline = false;
            this.btnCropRect.Name = "btnCropRect";
            this.btnCropRect.Size = new System.Drawing.Size(128, 50);
            this.btnCropRect.TabIndex = 2;
            this.btnCropRect.Text = "Area Temp";
            this.btnCropRect.TextColor = System.Drawing.Color.Black;
            this.btnCropRect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropRect.UseVisualStyleBackColor = false;
            this.btnCropRect.Click += new System.EventHandler(this.btnCropRect_Click);
            // 
            // btnCropArea
            // 
            this.btnCropArea.AutoFont = true;
            this.btnCropArea.AutoFontHeightRatio = 0.75F;
            this.btnCropArea.AutoFontMax = 100F;
            this.btnCropArea.AutoFontMin = 6F;
            this.btnCropArea.AutoFontWidthRatio = 0.92F;
            this.btnCropArea.AutoImage = true;
            this.btnCropArea.AutoImageMaxRatio = 0.75F;
            this.btnCropArea.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnCropArea.AutoImageTint = true;
            this.btnCropArea.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropArea.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropArea.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnCropArea.BorderRadius = 5;
            this.btnCropArea.BorderSize = 1;
            this.btnCropArea.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnCropArea.Corner = BeeGlobal.Corner.None;
            this.btnCropArea.DebounceResizeMs = 16;
            this.btnCropArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCropArea.FlatAppearance.BorderSize = 0;
            this.btnCropArea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCropArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnCropArea.ForeColor = System.Drawing.Color.Black;
            this.btnCropArea.Image = null;
            this.btnCropArea.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCropArea.ImageDisabled = null;
            this.btnCropArea.ImageHover = null;
            this.btnCropArea.ImageNormal = null;
            this.btnCropArea.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnCropArea.ImagePressed = null;
            this.btnCropArea.ImageTextSpacing = 6;
            this.btnCropArea.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnCropArea.ImageTintHover = System.Drawing.Color.Empty;
            this.btnCropArea.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnCropArea.ImageTintOpacity = 0.5F;
            this.btnCropArea.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnCropArea.IsCLick = true;
            this.btnCropArea.IsNotChange = false;
            this.btnCropArea.IsRect = false;
            this.btnCropArea.IsUnGroup = false;
            this.btnCropArea.Location = new System.Drawing.Point(128, 0);
            this.btnCropArea.Margin = new System.Windows.Forms.Padding(0);
            this.btnCropArea.Multiline = false;
            this.btnCropArea.Name = "btnCropArea";
            this.btnCropArea.Size = new System.Drawing.Size(139, 50);
            this.btnCropArea.TabIndex = 3;
            this.btnCropArea.Text = "Area Check";
            this.btnCropArea.TextColor = System.Drawing.Color.Black;
            this.btnCropArea.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCropArea.UseVisualStyleBackColor = false;
            this.btnCropArea.Click += new System.EventHandler(this.btnCropArea_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.pMain);
            this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(410, 1009);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
            // 
            // pMain
            // 
            this.pMain.AutoScroll = true;
            this.pMain.BackColor = System.Drawing.SystemColors.Control;
            this.pMain.ColumnCount = 1;
            this.pMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pMain.Controls.Add(this.trackResize, 0, 1);
            this.pMain.Controls.Add(this.label3, 0, 2);
            this.pMain.Controls.Add(this.layLimitCouter, 0, 3);
            this.pMain.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.pMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMain.Location = new System.Drawing.Point(3, 3);
            this.pMain.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.pMain.Name = "pMain";
            this.pMain.RowCount = 10;
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 109F));
            this.pMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pMain.Size = new System.Drawing.Size(404, 1003);
            this.pMain.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 113);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(396, 25);
            this.label3.TabIndex = 58;
            this.label3.Text = "Option Thread CPU";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // layLimitCouter
            // 
            this.layLimitCouter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.layLimitCouter.ColumnCount = 3;
            this.layLimitCouter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layLimitCouter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layLimitCouter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layLimitCouter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layLimitCouter.Controls.Add(this.btnMulti, 1, 0);
            this.layLimitCouter.Controls.Add(this.btnSingle, 0, 0);
            this.layLimitCouter.Controls.Add(this.numCPU, 2, 0);
            this.layLimitCouter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layLimitCouter.Location = new System.Drawing.Point(10, 138);
            this.layLimitCouter.Margin = new System.Windows.Forms.Padding(10, 0, 5, 0);
            this.layLimitCouter.Name = "layLimitCouter";
            this.layLimitCouter.Padding = new System.Windows.Forms.Padding(5, 5, 5, 8);
            this.layLimitCouter.RowCount = 1;
            this.layLimitCouter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layLimitCouter.Size = new System.Drawing.Size(389, 73);
            this.layLimitCouter.TabIndex = 57;
            // 
            // btnMulti
            // 
            this.btnMulti.AutoFont = true;
            this.btnMulti.AutoFontHeightRatio = 0.75F;
            this.btnMulti.AutoFontMax = 100F;
            this.btnMulti.AutoFontMin = 6F;
            this.btnMulti.AutoFontWidthRatio = 0.92F;
            this.btnMulti.AutoImage = true;
            this.btnMulti.AutoImageMaxRatio = 0.75F;
            this.btnMulti.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMulti.AutoImageTint = true;
            this.btnMulti.BackColor = System.Drawing.Color.Gray;
            this.btnMulti.BackgroundColor = System.Drawing.Color.Gray;
            this.btnMulti.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMulti.BorderColor = System.Drawing.Color.LightGray;
            this.btnMulti.BorderRadius = 5;
            this.btnMulti.BorderSize = 1;
            this.btnMulti.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMulti.Corner = BeeGlobal.Corner.None;
            this.btnMulti.DebounceResizeMs = 16;
            this.btnMulti.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMulti.FlatAppearance.BorderSize = 0;
            this.btnMulti.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMulti.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.42188F);
            this.btnMulti.ForeColor = System.Drawing.Color.Black;
            this.btnMulti.Image = null;
            this.btnMulti.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMulti.ImageDisabled = null;
            this.btnMulti.ImageHover = null;
            this.btnMulti.ImageNormal = null;
            this.btnMulti.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnMulti.ImagePressed = null;
            this.btnMulti.ImageTextSpacing = 6;
            this.btnMulti.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnMulti.ImageTintHover = System.Drawing.Color.Empty;
            this.btnMulti.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnMulti.ImageTintOpacity = 0.5F;
            this.btnMulti.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnMulti.IsCLick = false;
            this.btnMulti.IsNotChange = false;
            this.btnMulti.IsRect = false;
            this.btnMulti.IsUnGroup = false;
            this.btnMulti.Location = new System.Drawing.Point(105, 5);
            this.btnMulti.Margin = new System.Windows.Forms.Padding(0);
            this.btnMulti.Multiline = false;
            this.btnMulti.Name = "btnMulti";
            this.btnMulti.Size = new System.Drawing.Size(100, 60);
            this.btnMulti.TabIndex = 32;
            this.btnMulti.Text = "Multi";
            this.btnMulti.TextColor = System.Drawing.Color.Black;
            this.btnMulti.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMulti.UseVisualStyleBackColor = false;
            this.btnMulti.Click += new System.EventHandler(this.btnMulti_Click);
            // 
            // btnSingle
            // 
            this.btnSingle.AutoFont = true;
            this.btnSingle.AutoFontHeightRatio = 0.75F;
            this.btnSingle.AutoFontMax = 100F;
            this.btnSingle.AutoFontMin = 6F;
            this.btnSingle.AutoFontWidthRatio = 0.92F;
            this.btnSingle.AutoImage = true;
            this.btnSingle.AutoImageMaxRatio = 0.75F;
            this.btnSingle.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSingle.AutoImageTint = true;
            this.btnSingle.BackColor = System.Drawing.Color.LightGray;
            this.btnSingle.BackgroundColor = System.Drawing.Color.LightGray;
            this.btnSingle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSingle.BorderColor = System.Drawing.Color.Transparent;
            this.btnSingle.BorderRadius = 10;
            this.btnSingle.BorderSize = 1;
            this.btnSingle.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSingle.Corner = BeeGlobal.Corner.Left;
            this.btnSingle.DebounceResizeMs = 16;
            this.btnSingle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSingle.FlatAppearance.BorderSize = 0;
            this.btnSingle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSingle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.btnSingle.ForeColor = System.Drawing.Color.Black;
            this.btnSingle.Image = null;
            this.btnSingle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSingle.ImageDisabled = null;
            this.btnSingle.ImageHover = null;
            this.btnSingle.ImageNormal = null;
            this.btnSingle.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSingle.ImagePressed = null;
            this.btnSingle.ImageTextSpacing = 6;
            this.btnSingle.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSingle.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSingle.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSingle.ImageTintOpacity = 0.5F;
            this.btnSingle.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSingle.IsCLick = true;
            this.btnSingle.IsNotChange = false;
            this.btnSingle.IsRect = false;
            this.btnSingle.IsUnGroup = false;
            this.btnSingle.Location = new System.Drawing.Point(5, 5);
            this.btnSingle.Margin = new System.Windows.Forms.Padding(0);
            this.btnSingle.Multiline = false;
            this.btnSingle.Name = "btnSingle";
            this.btnSingle.Size = new System.Drawing.Size(100, 60);
            this.btnSingle.TabIndex = 31;
            this.btnSingle.Text = "Single";
            this.btnSingle.TextColor = System.Drawing.Color.Black;
            this.btnSingle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSingle.UseVisualStyleBackColor = false;
            this.btnSingle.Click += new System.EventHandler(this.btnSingle_Click);
            // 
            // numCPU
            // 
            this.numCPU.AutoShowTextbox = false;
            this.numCPU.AutoSizeTextbox = true;
            this.numCPU.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numCPU.BackColor = System.Drawing.SystemColors.Control;
            this.numCPU.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.numCPU.BorderRadius = 6;
            this.numCPU.ButtonMaxSize = 64;
            this.numCPU.ButtonMinSize = 24;
            this.numCPU.Decimals = 0;
            this.numCPU.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numCPU.ElementGap = 6;
            this.numCPU.FillTextboxToAvailable = true;
            this.numCPU.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numCPU.InnerPadding = new System.Windows.Forms.Padding(6);
            this.numCPU.KeyboardStep = 1F;
            this.numCPU.Location = new System.Drawing.Point(205, 5);
            this.numCPU.Margin = new System.Windows.Forms.Padding(0);
            this.numCPU.Max = 16F;
            this.numCPU.MaxTextboxWidth = 0;
            this.numCPU.Min = 0F;
            this.numCPU.MinimumSize = new System.Drawing.Size(120, 32);
            this.numCPU.MinTextboxWidth = 16;
            this.numCPU.Name = "numCPU";
            this.numCPU.Size = new System.Drawing.Size(179, 60);
            this.numCPU.SnapToStep = true;
            this.numCPU.StartWithTextboxHidden = false;
            this.numCPU.Step = 1F;
            this.numCPU.TabIndex = 34;
            this.numCPU.TextboxFontSize = 28F;
            this.numCPU.TextboxSidePadding = 12;
            this.numCPU.TextboxWidth = 56;
            this.numCPU.UnitText = "";
            this.numCPU.Value = 1F;
            this.numCPU.WheelStep = 1F;
            this.numCPU.ValueChanged += new System.Action<float>(this.numCPU_ValueChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.49735F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.50265F));
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnEnResizeSample, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(398, 40);
            this.tableLayoutPanel2.TabIndex = 56;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(216, 40);
            this.label4.TabIndex = 45;
            this.label4.Text = "Resize Sample";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnEnResizeSample
            // 
            this.btnEnResizeSample.AutoFont = true;
            this.btnEnResizeSample.AutoFontHeightRatio = 0.75F;
            this.btnEnResizeSample.AutoFontMax = 100F;
            this.btnEnResizeSample.AutoFontMin = 6F;
            this.btnEnResizeSample.AutoFontWidthRatio = 0.92F;
            this.btnEnResizeSample.AutoImage = true;
            this.btnEnResizeSample.AutoImageMaxRatio = 0.75F;
            this.btnEnResizeSample.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnEnResizeSample.AutoImageTint = true;
            this.btnEnResizeSample.BackColor = System.Drawing.SystemColors.Control;
            this.btnEnResizeSample.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnEnResizeSample.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEnResizeSample.BorderColor = System.Drawing.Color.Transparent;
            this.btnEnResizeSample.BorderRadius = 1;
            this.btnEnResizeSample.BorderSize = 1;
            this.btnEnResizeSample.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnEnResizeSample.Corner = BeeGlobal.Corner.None;
            this.btnEnResizeSample.DebounceResizeMs = 16;
            this.btnEnResizeSample.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnResizeSample.FlatAppearance.BorderSize = 0;
            this.btnEnResizeSample.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnResizeSample.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.671875F);
            this.btnEnResizeSample.ForeColor = System.Drawing.Color.Black;
            this.btnEnResizeSample.Image = null;
            this.btnEnResizeSample.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnResizeSample.ImageDisabled = null;
            this.btnEnResizeSample.ImageHover = null;
            this.btnEnResizeSample.ImageNormal = null;
            this.btnEnResizeSample.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnEnResizeSample.ImagePressed = null;
            this.btnEnResizeSample.ImageTextSpacing = 6;
            this.btnEnResizeSample.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnEnResizeSample.ImageTintHover = System.Drawing.Color.Empty;
            this.btnEnResizeSample.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnEnResizeSample.ImageTintOpacity = 0.5F;
            this.btnEnResizeSample.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnEnResizeSample.IsCLick = false;
            this.btnEnResizeSample.IsNotChange = false;
            this.btnEnResizeSample.IsRect = false;
            this.btnEnResizeSample.IsUnGroup = true;
            this.btnEnResizeSample.Location = new System.Drawing.Point(219, 3);
            this.btnEnResizeSample.Multiline = false;
            this.btnEnResizeSample.Name = "btnEnResizeSample";
            this.btnEnResizeSample.Size = new System.Drawing.Size(176, 34);
            this.btnEnResizeSample.TabIndex = 2;
            this.btnEnResizeSample.Text = "Enable";
            this.btnEnResizeSample.TextColor = System.Drawing.Color.Black;
            this.btnEnResizeSample.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEnResizeSample.UseVisualStyleBackColor = false;
            this.btnEnResizeSample.Click += new System.EventHandler(this.btnEnResizeSample_Click);
            // 
            // pCany
            // 
            this.pCany.Controls.Add(this.btnAreaBlack);
            this.pCany.Controls.Add(this.btnAreaWhite);
            this.pCany.Enabled = false;
            this.pCany.Location = new System.Drawing.Point(529, 529);
            this.pCany.Name = "pCany";
            this.pCany.Size = new System.Drawing.Size(386, 50);
            this.pCany.TabIndex = 12;
            this.pCany.Visible = false;
            // 
            // btnAreaBlack
            // 
            this.btnAreaBlack.AutoFont = true;
            this.btnAreaBlack.AutoFontHeightRatio = 0.75F;
            this.btnAreaBlack.AutoFontMax = 100F;
            this.btnAreaBlack.AutoFontMin = 6F;
            this.btnAreaBlack.AutoFontWidthRatio = 0.92F;
            this.btnAreaBlack.AutoImage = true;
            this.btnAreaBlack.AutoImageMaxRatio = 0.75F;
            this.btnAreaBlack.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnAreaBlack.AutoImageTint = true;
            this.btnAreaBlack.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAreaBlack.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAreaBlack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAreaBlack.BackgroundImage")));
            this.btnAreaBlack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAreaBlack.BorderColor = System.Drawing.Color.Transparent;
            this.btnAreaBlack.BorderRadius = 5;
            this.btnAreaBlack.BorderSize = 1;
            this.btnAreaBlack.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnAreaBlack.Corner = BeeGlobal.Corner.Both;
            this.btnAreaBlack.DebounceResizeMs = 16;
            this.btnAreaBlack.FlatAppearance.BorderSize = 0;
            this.btnAreaBlack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAreaBlack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnAreaBlack.ForeColor = System.Drawing.Color.Black;
            this.btnAreaBlack.Image = null;
            this.btnAreaBlack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAreaBlack.ImageDisabled = null;
            this.btnAreaBlack.ImageHover = null;
            this.btnAreaBlack.ImageNormal = null;
            this.btnAreaBlack.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnAreaBlack.ImagePressed = null;
            this.btnAreaBlack.ImageTextSpacing = 6;
            this.btnAreaBlack.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnAreaBlack.ImageTintHover = System.Drawing.Color.Empty;
            this.btnAreaBlack.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnAreaBlack.ImageTintOpacity = 0.5F;
            this.btnAreaBlack.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnAreaBlack.IsCLick = true;
            this.btnAreaBlack.IsNotChange = false;
            this.btnAreaBlack.IsRect = false;
            this.btnAreaBlack.IsUnGroup = false;
            this.btnAreaBlack.Location = new System.Drawing.Point(3, 5);
            this.btnAreaBlack.Multiline = false;
            this.btnAreaBlack.Name = "btnAreaBlack";
            this.btnAreaBlack.Size = new System.Drawing.Size(185, 40);
            this.btnAreaBlack.TabIndex = 7;
            this.btnAreaBlack.Text = "Vùng Tối";
            this.btnAreaBlack.TextColor = System.Drawing.Color.Black;
            this.btnAreaBlack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAreaBlack.UseVisualStyleBackColor = false;
            // 
            // btnAreaWhite
            // 
            this.btnAreaWhite.AutoFont = true;
            this.btnAreaWhite.AutoFontHeightRatio = 0.75F;
            this.btnAreaWhite.AutoFontMax = 100F;
            this.btnAreaWhite.AutoFontMin = 6F;
            this.btnAreaWhite.AutoFontWidthRatio = 0.92F;
            this.btnAreaWhite.AutoImage = true;
            this.btnAreaWhite.AutoImageMaxRatio = 0.75F;
            this.btnAreaWhite.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnAreaWhite.AutoImageTint = true;
            this.btnAreaWhite.BackColor = System.Drawing.SystemColors.Control;
            this.btnAreaWhite.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnAreaWhite.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAreaWhite.BackgroundImage")));
            this.btnAreaWhite.BorderColor = System.Drawing.Color.Silver;
            this.btnAreaWhite.BorderRadius = 5;
            this.btnAreaWhite.BorderSize = 1;
            this.btnAreaWhite.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnAreaWhite.Corner = BeeGlobal.Corner.Both;
            this.btnAreaWhite.DebounceResizeMs = 16;
            this.btnAreaWhite.FlatAppearance.BorderSize = 0;
            this.btnAreaWhite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAreaWhite.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnAreaWhite.ForeColor = System.Drawing.Color.Black;
            this.btnAreaWhite.Image = null;
            this.btnAreaWhite.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAreaWhite.ImageDisabled = null;
            this.btnAreaWhite.ImageHover = null;
            this.btnAreaWhite.ImageNormal = null;
            this.btnAreaWhite.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnAreaWhite.ImagePressed = null;
            this.btnAreaWhite.ImageTextSpacing = 6;
            this.btnAreaWhite.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnAreaWhite.ImageTintHover = System.Drawing.Color.Empty;
            this.btnAreaWhite.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnAreaWhite.ImageTintOpacity = 0.5F;
            this.btnAreaWhite.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnAreaWhite.IsCLick = false;
            this.btnAreaWhite.IsNotChange = false;
            this.btnAreaWhite.IsRect = false;
            this.btnAreaWhite.IsUnGroup = false;
            this.btnAreaWhite.Location = new System.Drawing.Point(195, 5);
            this.btnAreaWhite.Multiline = false;
            this.btnAreaWhite.Name = "btnAreaWhite";
            this.btnAreaWhite.Size = new System.Drawing.Size(188, 40);
            this.btnAreaWhite.TabIndex = 9;
            this.btnAreaWhite.Text = "Vùng sáng";
            this.btnAreaWhite.TextColor = System.Drawing.Color.Black;
            this.btnAreaWhite.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAreaWhite.UseVisualStyleBackColor = false;
            // 
            // workModel
            // 
            this.workModel.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workModel_DoWork);
            this.workModel.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workModel_RunWorkerCompleted);
            // 
            // oK_Cancel1
            // 
            this.oK_Cancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oK_Cancel1.Location = new System.Drawing.Point(0, 995);
            this.oK_Cancel1.Name = "oK_Cancel1";
            this.oK_Cancel1.Size = new System.Drawing.Size(418, 52);
            this.oK_Cancel1.TabIndex = 18;
            // 
            // trackResize
            // 
            this.trackResize.AutoShowTextbox = true;
            this.trackResize.AutoSizeTextbox = true;
            this.trackResize.BackColor = System.Drawing.SystemColors.Control;
            this.trackResize.BarLeftGap = 20;
            this.trackResize.BarRightGap = 6;
            this.trackResize.ChromeGap = 8;
            this.trackResize.ChromeWidthRatio = 0.14F;
            this.trackResize.ColorBorder = System.Drawing.Color.DarkGray;
            this.trackResize.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.trackResize.ColorScale = System.Drawing.Color.DarkGray;
            this.trackResize.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackResize.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackResize.ColorTrack = System.Drawing.Color.DarkGray;
            this.trackResize.Decimals = 0;
            this.trackResize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackResize.EdgePadding = 2;
            this.trackResize.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackResize.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackResize.KeyboardStep = 1F;
            this.trackResize.Location = new System.Drawing.Point(3, 49);
            this.trackResize.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.trackResize.MatchTextboxFontToThumb = true;
            this.trackResize.Max = 100F;
            this.trackResize.MaxTextboxWidth = 0;
            this.trackResize.MaxThumb = 1000;
            this.trackResize.MaxTrackHeight = 1000;
            this.trackResize.Min = 1F;
            this.trackResize.MinChromeWidth = 64;
            this.trackResize.MinimumSize = new System.Drawing.Size(140, 36);
            this.trackResize.MinTextboxWidth = 32;
            this.trackResize.MinThumb = 30;
            this.trackResize.MinTrackHeight = 8;
            this.trackResize.Name = "trackResize";
            this.trackResize.Radius = 8;
            this.trackResize.ShowValueOnThumb = true;
            this.trackResize.Size = new System.Drawing.Size(398, 49);
            this.trackResize.SnapToStep = true;
            this.trackResize.StartWithTextboxHidden = true;
            this.trackResize.Step = 1F;
            this.trackResize.TabIndex = 70;
            this.trackResize.TextboxFontSize = 20F;
            this.trackResize.TextboxSidePadding = 10;
            this.trackResize.TextboxWidth = 600;
            this.trackResize.ThumbDiameterRatio = 2F;
            this.trackResize.ThumbValueBold = true;
            this.trackResize.ThumbValueFontScale = 1.5F;
            this.trackResize.ThumbValuePadding = 0;
            this.trackResize.TightEdges = true;
            this.trackResize.TrackHeightRatio = 0.45F;
            this.trackResize.TrackWidthRatio = 1F;
            this.trackResize.UnitText = "";
            this.trackResize.Value = 1F;
            this.trackResize.WheelStep = 1F;
            this.trackResize.ValueChanged += new System.Action<float>(this.trackResize_ValueChanged);
            // 
            // ToolOKNG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.oK_Cancel1);
            this.Controls.Add(this.pCany);
            this.Controls.Add(this.tabControl2);
            this.DoubleBuffered = true;
            this.Name = "ToolOKNG";
            this.Size = new System.Drawing.Size(418, 1047);
            this.VisibleChanged += new System.EventHandler(this.ToolOutLine_VisibleChanged);
            this.tabControl2.ResumeLayout(false);
            this.tabP1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.pView.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pNG.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.pMain.ResumeLayout(false);
            this.pMain.PerformLayout();
            this.layLimitCouter.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pCany.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private RJButton btnCropArea;
        private RJButton btnCropRect;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabP1;
        private System.Windows.Forms.TabPage tabPage4;
        private RJButton btnClear;
        private System.Windows.Forms.Panel pCany;
        private RJButton btnAreaBlack;
        private RJButton btnAreaWhite;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.ComponentModel.BackgroundWorker workModel;
        private System.Windows.Forms.Label label13;
        private GroupControl.OK_Cancel oK_Cancel1;
        private System.Windows.Forms.Panel pNG;
        private RJButton btnLearning;
        private System.Windows.Forms.TableLayoutPanel pMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private RJButton btnAddOK;
        private RJButton btnRemoveAllOK;
        private RJButton btnUndoOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private RJButton btnAddNG;
        private RJButton btnRemoveAllNG;
        private RJButton btnUndoNG;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Panel pView;
        public Cyotek.Windows.Forms.ImageBox imgOK;
        public Cyotek.Windows.Forms.ImageBox imgNG;
        private RJButton btnTest;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label4;
        private RJButton btnEnResizeSample;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel layLimitCouter;
        private RJButton btnMulti;
        private RJButton btnSingle;
        private CustomNumericEx numCPU;
        private AdjustBarEx trackResize;
    }
}
