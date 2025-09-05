using BeeCore;
using BeeGlobal;
using System.Windows.Forms;

namespace BeeInterface
{
    partial class ToolMeasure
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolMeasure));
            this.threadProcess = new System.ComponentModel.BackgroundWorker();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabP1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.trackScore = new BeeInterface.AdjustBarEx();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton10 = new BeeInterface.RJButton();
            this.cbDirect = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton3 = new BeeInterface.RJButton();
            this.cbMethord = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton2 = new BeeInterface.RJButton();
            this.cbMeasure = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton1 = new BeeInterface.RJButton();
            this.cb8 = new System.Windows.Forms.ComboBox();
            this.cb7 = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton13 = new BeeInterface.RJButton();
            this.cb6 = new System.Windows.Forms.ComboBox();
            this.cb5 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton12 = new BeeInterface.RJButton();
            this.cb4 = new System.Windows.Forms.ComboBox();
            this.cb3 = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton11 = new BeeInterface.RJButton();
            this.cb2 = new System.Windows.Forms.ComboBox();
            this.cb1 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnTest = new BeeInterface.RJButton();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.pCany = new System.Windows.Forms.Panel();
            this.btnAreaBlack = new BeeInterface.RJButton();
            this.btnAreaWhite = new BeeInterface.RJButton();
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.tabControl2.SuspendLayout();
            this.tabP1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel16.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pCany.SuspendLayout();
            this.SuspendLayout();
            // 
            // threadProcess
            // 
            this.threadProcess.WorkerReportsProgress = true;
            this.threadProcess.DoWork += new System.ComponentModel.DoWorkEventHandler(this.threadProcess_DoWork);
            this.threadProcess.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.threadProcess_RunWorkerCompleted);
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabP1);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(462, 881);
            this.tabControl2.TabIndex = 17;
            // 
            // tabP1
            // 
            this.tabP1.BackColor = System.Drawing.SystemColors.Control;
            this.tabP1.Controls.Add(this.tableLayoutPanel1);
            this.tabP1.Location = new System.Drawing.Point(4, 34);
            this.tabP1.Name = "tabP1";
            this.tabP1.Padding = new System.Windows.Forms.Padding(3);
            this.tabP1.Size = new System.Drawing.Size(454, 843);
            this.tabP1.TabIndex = 0;
            this.tabP1.Text = "Basic";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.trackScore, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel16, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel15, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.btnTest, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 13;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(448, 837);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // trackScore
            // 
            this.trackScore.AutoShowTextbox = true;
            this.trackScore.AutoSizeTextbox = true;
            this.trackScore.BackColor = System.Drawing.SystemColors.Control;
            this.trackScore.BarLeftGap = 20;
            this.trackScore.BarRightGap = 6;
            this.trackScore.ChromeGap = 8;
            this.trackScore.ChromeWidthRatio = 0.14F;
            this.trackScore.ColorBorder = System.Drawing.Color.DarkGray;
            this.trackScore.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.trackScore.ColorScale = System.Drawing.Color.DarkGray;
            this.trackScore.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackScore.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackScore.ColorTrack = System.Drawing.Color.DarkGray;
            this.trackScore.Decimals = 0;
            this.trackScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackScore.EdgePadding = 2;
            this.trackScore.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackScore.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackScore.KeyboardStep = 1F;
            this.trackScore.Location = new System.Drawing.Point(3, 495);
            this.trackScore.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.trackScore.MatchTextboxFontToThumb = true;
            this.trackScore.Max = 100F;
            this.trackScore.MaxTextboxWidth = 0;
            this.trackScore.MaxThumb = 1000;
            this.trackScore.MaxTrackHeight = 1000;
            this.trackScore.Min = 0F;
            this.trackScore.MinChromeWidth = 64;
            this.trackScore.MinimumSize = new System.Drawing.Size(140, 36);
            this.trackScore.MinTextboxWidth = 32;
            this.trackScore.MinThumb = 30;
            this.trackScore.MinTrackHeight = 8;
            this.trackScore.Name = "trackScore";
            this.trackScore.Radius = 8;
            this.trackScore.ShowValueOnThumb = true;
            this.trackScore.Size = new System.Drawing.Size(442, 45);
            this.trackScore.SnapToStep = true;
            this.trackScore.StartWithTextboxHidden = true;
            this.trackScore.Step = 1F;
            this.trackScore.TabIndex = 70;
            this.trackScore.TextboxFontSize = 20F;
            this.trackScore.TextboxSidePadding = 10;
            this.trackScore.TextboxWidth = 600;
            this.trackScore.ThumbDiameterRatio = 2F;
            this.trackScore.ThumbValueBold = true;
            this.trackScore.ThumbValueFontScale = 1.5F;
            this.trackScore.ThumbValuePadding = 0;
            this.trackScore.TightEdges = true;
            this.trackScore.TrackHeightRatio = 0.45F;
            this.trackScore.TrackWidthRatio = 1F;
            this.trackScore.UnitText = "";
            this.trackScore.Value = 0F;
            this.trackScore.WheelStep = 1F;
            this.trackScore.ValueChanged += new System.Action<float>(this.trackScore_ValueChanged);
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel16.ColumnCount = 2;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.71429F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.28572F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel16.Controls.Add(this.rjButton10, 0, 0);
            this.tableLayoutPanel16.Controls.Add(this.cbDirect, 1, 0);
            this.tableLayoutPanel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel16.Location = new System.Drawing.Point(5, 55);
            this.tableLayoutPanel16.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 1;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(440, 45);
            this.tableLayoutPanel16.TabIndex = 54;
            // 
            // rjButton10
            // 
            this.rjButton10.AutoFont = true;
            this.rjButton10.AutoFontHeightRatio = 0.75F;
            this.rjButton10.AutoFontMax = 100F;
            this.rjButton10.AutoFontMin = 6F;
            this.rjButton10.AutoFontWidthRatio = 0.92F;
            this.rjButton10.AutoImage = true;
            this.rjButton10.AutoImageMaxRatio = 0.75F;
            this.rjButton10.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton10.AutoImageTint = true;
            this.rjButton10.BackColor = System.Drawing.Color.LightGray;
            this.rjButton10.BackgroundColor = System.Drawing.Color.LightGray;
            this.rjButton10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton10.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton10.BorderRadius = 10;
            this.rjButton10.BorderSize = 1;
            this.rjButton10.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton10.Corner = BeeGlobal.Corner.Left;
            this.rjButton10.DebounceResizeMs = 16;
            this.rjButton10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton10.FlatAppearance.BorderSize = 0;
            this.rjButton10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.rjButton10.ForeColor = System.Drawing.Color.Black;
            this.rjButton10.Image = null;
            this.rjButton10.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton10.ImageDisabled = null;
            this.rjButton10.ImageHover = null;
            this.rjButton10.ImageNormal = null;
            this.rjButton10.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton10.ImagePressed = null;
            this.rjButton10.ImageTextSpacing = 6;
            this.rjButton10.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton10.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton10.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton10.ImageTintOpacity = 0.5F;
            this.rjButton10.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton10.IsCLick = false;
            this.rjButton10.IsNotChange = false;
            this.rjButton10.IsRect = false;
            this.rjButton10.IsUnGroup = false;
            this.rjButton10.Location = new System.Drawing.Point(0, 0);
            this.rjButton10.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton10.Multiline = false;
            this.rjButton10.Name = "rjButton10";
            this.rjButton10.Size = new System.Drawing.Size(201, 45);
            this.rjButton10.TabIndex = 33;
            this.rjButton10.Text = "Direct Measure";
            this.rjButton10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rjButton10.TextColor = System.Drawing.Color.Black;
            this.rjButton10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton10.UseVisualStyleBackColor = false;
            // 
            // cbDirect
            // 
            this.cbDirect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbDirect.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDirect.FormattingEnabled = true;
            this.cbDirect.Location = new System.Drawing.Point(203, 2);
            this.cbDirect.Margin = new System.Windows.Forms.Padding(2);
            this.cbDirect.Name = "cbDirect";
            this.cbDirect.Size = new System.Drawing.Size(235, 39);
            this.cbDirect.TabIndex = 2;
            this.cbDirect.SelectedIndexChanged += new System.EventHandler(this.cbDirect_SelectedIndexChanged);
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel15.ColumnCount = 2;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.71429F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.28572F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel15.Controls.Add(this.rjButton3, 0, 0);
            this.tableLayoutPanel15.Controls.Add(this.cbMethord, 1, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(5, 105);
            this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(440, 45);
            this.tableLayoutPanel15.TabIndex = 53;
            // 
            // rjButton3
            // 
            this.rjButton3.AutoFont = true;
            this.rjButton3.AutoFontHeightRatio = 0.75F;
            this.rjButton3.AutoFontMax = 100F;
            this.rjButton3.AutoFontMin = 6F;
            this.rjButton3.AutoFontWidthRatio = 0.92F;
            this.rjButton3.AutoImage = true;
            this.rjButton3.AutoImageMaxRatio = 0.75F;
            this.rjButton3.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton3.AutoImageTint = true;
            this.rjButton3.BackColor = System.Drawing.Color.LightGray;
            this.rjButton3.BackgroundColor = System.Drawing.Color.LightGray;
            this.rjButton3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton3.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton3.BorderRadius = 10;
            this.rjButton3.BorderSize = 1;
            this.rjButton3.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton3.Corner = BeeGlobal.Corner.Left;
            this.rjButton3.DebounceResizeMs = 16;
            this.rjButton3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton3.FlatAppearance.BorderSize = 0;
            this.rjButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.rjButton3.ForeColor = System.Drawing.Color.Black;
            this.rjButton3.Image = null;
            this.rjButton3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton3.ImageDisabled = null;
            this.rjButton3.ImageHover = null;
            this.rjButton3.ImageNormal = null;
            this.rjButton3.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton3.ImagePressed = null;
            this.rjButton3.ImageTextSpacing = 6;
            this.rjButton3.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton3.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton3.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton3.ImageTintOpacity = 0.5F;
            this.rjButton3.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton3.IsCLick = false;
            this.rjButton3.IsNotChange = false;
            this.rjButton3.IsRect = false;
            this.rjButton3.IsUnGroup = false;
            this.rjButton3.Location = new System.Drawing.Point(0, 0);
            this.rjButton3.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton3.Multiline = false;
            this.rjButton3.Name = "rjButton3";
            this.rjButton3.Size = new System.Drawing.Size(201, 45);
            this.rjButton3.TabIndex = 33;
            this.rjButton3.Text = "Methord Measure";
            this.rjButton3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rjButton3.TextColor = System.Drawing.Color.Black;
            this.rjButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton3.UseVisualStyleBackColor = false;
            // 
            // cbMethord
            // 
            this.cbMethord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbMethord.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMethord.FormattingEnabled = true;
            this.cbMethord.Location = new System.Drawing.Point(203, 2);
            this.cbMethord.Margin = new System.Windows.Forms.Padding(2);
            this.cbMethord.Name = "cbMethord";
            this.cbMethord.Size = new System.Drawing.Size(235, 39);
            this.cbMethord.TabIndex = 2;
            this.cbMethord.SelectedIndexChanged += new System.EventHandler(this.cbMethord_SelectedIndexChanged);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.71429F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.28572F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Controls.Add(this.rjButton2, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.cbMeasure, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(440, 45);
            this.tableLayoutPanel6.TabIndex = 52;
            // 
            // rjButton2
            // 
            this.rjButton2.AutoFont = true;
            this.rjButton2.AutoFontHeightRatio = 0.75F;
            this.rjButton2.AutoFontMax = 100F;
            this.rjButton2.AutoFontMin = 6F;
            this.rjButton2.AutoFontWidthRatio = 0.92F;
            this.rjButton2.AutoImage = true;
            this.rjButton2.AutoImageMaxRatio = 0.75F;
            this.rjButton2.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton2.AutoImageTint = true;
            this.rjButton2.BackColor = System.Drawing.Color.LightGray;
            this.rjButton2.BackgroundColor = System.Drawing.Color.LightGray;
            this.rjButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton2.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton2.BorderRadius = 10;
            this.rjButton2.BorderSize = 1;
            this.rjButton2.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton2.Corner = BeeGlobal.Corner.Left;
            this.rjButton2.DebounceResizeMs = 16;
            this.rjButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton2.FlatAppearance.BorderSize = 0;
            this.rjButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.rjButton2.ForeColor = System.Drawing.Color.Black;
            this.rjButton2.Image = null;
            this.rjButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton2.ImageDisabled = null;
            this.rjButton2.ImageHover = null;
            this.rjButton2.ImageNormal = null;
            this.rjButton2.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton2.ImagePressed = null;
            this.rjButton2.ImageTextSpacing = 6;
            this.rjButton2.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton2.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton2.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton2.ImageTintOpacity = 0.5F;
            this.rjButton2.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton2.IsCLick = false;
            this.rjButton2.IsNotChange = false;
            this.rjButton2.IsRect = false;
            this.rjButton2.IsUnGroup = false;
            this.rjButton2.Location = new System.Drawing.Point(0, 0);
            this.rjButton2.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton2.Multiline = false;
            this.rjButton2.Name = "rjButton2";
            this.rjButton2.Size = new System.Drawing.Size(201, 45);
            this.rjButton2.TabIndex = 33;
            this.rjButton2.Text = "Type Measure";
            this.rjButton2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rjButton2.TextColor = System.Drawing.Color.Black;
            this.rjButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton2.UseVisualStyleBackColor = false;
            // 
            // cbMeasure
            // 
            this.cbMeasure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbMeasure.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMeasure.FormattingEnabled = true;
            this.cbMeasure.Location = new System.Drawing.Point(203, 2);
            this.cbMeasure.Margin = new System.Windows.Forms.Padding(2);
            this.cbMeasure.Name = "cbMeasure";
            this.cbMeasure.Size = new System.Drawing.Size(235, 39);
            this.cbMeasure.TabIndex = 2;
            this.cbMeasure.SelectedIndexChanged += new System.EventHandler(this.cbMeasure_SelectedIndexChanged);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.09091F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.13636F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.54546F));
            this.tableLayoutPanel5.Controls.Add(this.rjButton1, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.cb8, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.cb7, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(5, 396);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(440, 51);
            this.tableLayoutPanel5.TabIndex = 51;
            // 
            // rjButton1
            // 
            this.rjButton1.AutoFont = true;
            this.rjButton1.AutoFontHeightRatio = 0.75F;
            this.rjButton1.AutoFontMax = 100F;
            this.rjButton1.AutoFontMin = 6F;
            this.rjButton1.AutoFontWidthRatio = 0.92F;
            this.rjButton1.AutoImage = true;
            this.rjButton1.AutoImageMaxRatio = 0.75F;
            this.rjButton1.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton1.AutoImageTint = true;
            this.rjButton1.BackColor = System.Drawing.Color.LightGray;
            this.rjButton1.BackgroundColor = System.Drawing.Color.LightGray;
            this.rjButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton1.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton1.BorderRadius = 10;
            this.rjButton1.BorderSize = 1;
            this.rjButton1.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton1.Corner = BeeGlobal.Corner.Left;
            this.rjButton1.DebounceResizeMs = 16;
            this.rjButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.48438F);
            this.rjButton1.ForeColor = System.Drawing.Color.Black;
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
            this.rjButton1.Location = new System.Drawing.Point(0, 0);
            this.rjButton1.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton1.Multiline = false;
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(128, 51);
            this.rjButton1.TabIndex = 33;
            this.rjButton1.Text = "Point 2";
            this.rjButton1.TextColor = System.Drawing.Color.Black;
            this.rjButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton1.UseVisualStyleBackColor = false;
            // 
            // cb8
            // 
            this.cb8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb8.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb8.FormattingEnabled = true;
            this.cb8.Location = new System.Drawing.Point(290, 8);
            this.cb8.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cb8.Name = "cb8";
            this.cb8.Size = new System.Drawing.Size(147, 39);
            this.cb8.TabIndex = 2;
            this.cb8.SelectedIndexChanged += new System.EventHandler(this.cb8_SelectedIndexChanged);
            // 
            // cb7
            // 
            this.cb7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb7.FormattingEnabled = true;
            this.cb7.Location = new System.Drawing.Point(131, 8);
            this.cb7.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cb7.Name = "cb7";
            this.cb7.Size = new System.Drawing.Size(153, 39);
            this.cb7.TabIndex = 1;
            this.cb7.SelectionChangeCommitted += new System.EventHandler(this.cb7_SelectionChangeCommitted);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.54545F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.68182F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.54546F));
            this.tableLayoutPanel4.Controls.Add(this.rjButton13, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.cb6, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.cb5, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(5, 344);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(440, 47);
            this.tableLayoutPanel4.TabIndex = 50;
            // 
            // rjButton13
            // 
            this.rjButton13.AutoFont = true;
            this.rjButton13.AutoFontHeightRatio = 0.75F;
            this.rjButton13.AutoFontMax = 100F;
            this.rjButton13.AutoFontMin = 6F;
            this.rjButton13.AutoFontWidthRatio = 0.92F;
            this.rjButton13.AutoImage = true;
            this.rjButton13.AutoImageMaxRatio = 0.75F;
            this.rjButton13.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton13.AutoImageTint = true;
            this.rjButton13.BackColor = System.Drawing.Color.LightGray;
            this.rjButton13.BackgroundColor = System.Drawing.Color.LightGray;
            this.rjButton13.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton13.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton13.BorderRadius = 10;
            this.rjButton13.BorderSize = 1;
            this.rjButton13.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton13.Corner = BeeGlobal.Corner.Left;
            this.rjButton13.DebounceResizeMs = 16;
            this.rjButton13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton13.FlatAppearance.BorderSize = 0;
            this.rjButton13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton13.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.rjButton13.ForeColor = System.Drawing.Color.Black;
            this.rjButton13.Image = null;
            this.rjButton13.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton13.ImageDisabled = null;
            this.rjButton13.ImageHover = null;
            this.rjButton13.ImageNormal = null;
            this.rjButton13.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton13.ImagePressed = null;
            this.rjButton13.ImageTextSpacing = 6;
            this.rjButton13.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton13.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton13.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton13.ImageTintOpacity = 0.5F;
            this.rjButton13.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton13.IsCLick = true;
            this.rjButton13.IsNotChange = false;
            this.rjButton13.IsRect = false;
            this.rjButton13.IsUnGroup = false;
            this.rjButton13.Location = new System.Drawing.Point(0, 0);
            this.rjButton13.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton13.Multiline = false;
            this.rjButton13.Name = "rjButton13";
            this.rjButton13.Size = new System.Drawing.Size(130, 47);
            this.rjButton13.TabIndex = 32;
            this.rjButton13.Text = "Point 1";
            this.rjButton13.TextColor = System.Drawing.Color.Black;
            this.rjButton13.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton13.UseVisualStyleBackColor = false;
            // 
            // cb6
            // 
            this.cb6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb6.FormattingEnabled = true;
            this.cb6.Location = new System.Drawing.Point(290, 8);
            this.cb6.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cb6.Name = "cb6";
            this.cb6.Size = new System.Drawing.Size(147, 39);
            this.cb6.TabIndex = 2;
            this.cb6.SelectedIndexChanged += new System.EventHandler(this.cb6_SelectedIndexChanged);
            // 
            // cb5
            // 
            this.cb5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb5.FormattingEnabled = true;
            this.cb5.Location = new System.Drawing.Point(133, 8);
            this.cb5.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cb5.Name = "cb5";
            this.cb5.Size = new System.Drawing.Size(151, 39);
            this.cb5.TabIndex = 1;
            this.cb5.SelectionChangeCommitted += new System.EventHandler(this.cb5_SelectionChangeCommitted);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(5, 320);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 20, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 24);
            this.label5.TabIndex = 49;
            this.label5.Text = "Line 2";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.09091F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.13636F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.54546F));
            this.tableLayoutPanel3.Controls.Add(this.rjButton12, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.cb4, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.cb3, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 247);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(440, 53);
            this.tableLayoutPanel3.TabIndex = 48;
            // 
            // rjButton12
            // 
            this.rjButton12.AutoFont = true;
            this.rjButton12.AutoFontHeightRatio = 0.75F;
            this.rjButton12.AutoFontMax = 100F;
            this.rjButton12.AutoFontMin = 6F;
            this.rjButton12.AutoFontWidthRatio = 0.92F;
            this.rjButton12.AutoImage = true;
            this.rjButton12.AutoImageMaxRatio = 0.75F;
            this.rjButton12.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton12.AutoImageTint = true;
            this.rjButton12.BackColor = System.Drawing.Color.LightGray;
            this.rjButton12.BackgroundColor = System.Drawing.Color.LightGray;
            this.rjButton12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton12.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton12.BorderRadius = 10;
            this.rjButton12.BorderSize = 1;
            this.rjButton12.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton12.Corner = BeeGlobal.Corner.Left;
            this.rjButton12.DebounceResizeMs = 16;
            this.rjButton12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton12.FlatAppearance.BorderSize = 0;
            this.rjButton12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton12.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.21875F);
            this.rjButton12.ForeColor = System.Drawing.Color.Black;
            this.rjButton12.Image = null;
            this.rjButton12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton12.ImageDisabled = null;
            this.rjButton12.ImageHover = null;
            this.rjButton12.ImageNormal = null;
            this.rjButton12.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton12.ImagePressed = null;
            this.rjButton12.ImageTextSpacing = 6;
            this.rjButton12.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton12.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton12.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton12.ImageTintOpacity = 0.5F;
            this.rjButton12.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton12.IsCLick = false;
            this.rjButton12.IsNotChange = false;
            this.rjButton12.IsRect = false;
            this.rjButton12.IsUnGroup = false;
            this.rjButton12.Location = new System.Drawing.Point(0, 0);
            this.rjButton12.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton12.Multiline = false;
            this.rjButton12.Name = "rjButton12";
            this.rjButton12.Size = new System.Drawing.Size(128, 53);
            this.rjButton12.TabIndex = 33;
            this.rjButton12.Text = "Point 2";
            this.rjButton12.TextColor = System.Drawing.Color.Black;
            this.rjButton12.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton12.UseVisualStyleBackColor = false;
            // 
            // cb4
            // 
            this.cb4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb4.FormattingEnabled = true;
            this.cb4.Location = new System.Drawing.Point(290, 8);
            this.cb4.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cb4.Name = "cb4";
            this.cb4.Size = new System.Drawing.Size(147, 39);
            this.cb4.TabIndex = 2;
            this.cb4.SelectedIndexChanged += new System.EventHandler(this.cb4_SelectedIndexChanged);
            // 
            // cb3
            // 
            this.cb3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb3.FormattingEnabled = true;
            this.cb3.Location = new System.Drawing.Point(131, 8);
            this.cb3.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cb3.Name = "cb3";
            this.cb3.Size = new System.Drawing.Size(153, 39);
            this.cb3.TabIndex = 1;
            this.cb3.SelectionChangeCommitted += new System.EventHandler(this.cb3_SelectionChangeCommitted);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.54545F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.68182F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.54546F));
            this.tableLayoutPanel2.Controls.Add(this.rjButton11, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cb2, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.cb1, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 195);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(440, 47);
            this.tableLayoutPanel2.TabIndex = 47;
            // 
            // rjButton11
            // 
            this.rjButton11.AutoFont = true;
            this.rjButton11.AutoFontHeightRatio = 0.75F;
            this.rjButton11.AutoFontMax = 100F;
            this.rjButton11.AutoFontMin = 6F;
            this.rjButton11.AutoFontWidthRatio = 0.92F;
            this.rjButton11.AutoImage = true;
            this.rjButton11.AutoImageMaxRatio = 0.75F;
            this.rjButton11.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton11.AutoImageTint = true;
            this.rjButton11.BackColor = System.Drawing.Color.LightGray;
            this.rjButton11.BackgroundColor = System.Drawing.Color.LightGray;
            this.rjButton11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton11.BorderColor = System.Drawing.Color.Transparent;
            this.rjButton11.BorderRadius = 10;
            this.rjButton11.BorderSize = 1;
            this.rjButton11.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton11.Corner = BeeGlobal.Corner.Left;
            this.rjButton11.DebounceResizeMs = 16;
            this.rjButton11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton11.FlatAppearance.BorderSize = 0;
            this.rjButton11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton11.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.rjButton11.ForeColor = System.Drawing.Color.Black;
            this.rjButton11.Image = null;
            this.rjButton11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton11.ImageDisabled = null;
            this.rjButton11.ImageHover = null;
            this.rjButton11.ImageNormal = null;
            this.rjButton11.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton11.ImagePressed = null;
            this.rjButton11.ImageTextSpacing = 6;
            this.rjButton11.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton11.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton11.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton11.ImageTintOpacity = 0.5F;
            this.rjButton11.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton11.IsCLick = true;
            this.rjButton11.IsNotChange = false;
            this.rjButton11.IsRect = false;
            this.rjButton11.IsUnGroup = false;
            this.rjButton11.Location = new System.Drawing.Point(0, 0);
            this.rjButton11.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton11.Multiline = false;
            this.rjButton11.Name = "rjButton11";
            this.rjButton11.Size = new System.Drawing.Size(130, 47);
            this.rjButton11.TabIndex = 32;
            this.rjButton11.Text = "Point 1";
            this.rjButton11.TextColor = System.Drawing.Color.Black;
            this.rjButton11.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton11.UseVisualStyleBackColor = false;
            // 
            // cb2
            // 
            this.cb2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb2.FormattingEnabled = true;
            this.cb2.Location = new System.Drawing.Point(290, 8);
            this.cb2.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cb2.Name = "cb2";
            this.cb2.Size = new System.Drawing.Size(147, 39);
            this.cb2.TabIndex = 2;
            this.cb2.SelectedIndexChanged += new System.EventHandler(this.cb2_SelectedIndexChanged);
            this.cb2.SelectionChangeCommitted += new System.EventHandler(this.cb2_SelectionChangeCommitted);
            // 
            // cb1
            // 
            this.cb1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb1.FormattingEnabled = true;
            this.cb1.Location = new System.Drawing.Point(133, 8);
            this.cb1.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.cb1.Name = "cb1";
            this.cb1.Size = new System.Drawing.Size(151, 39);
            this.cb1.TabIndex = 1;
            this.cb1.SelectionChangeCommitted += new System.EventHandler(this.cb1_SelectionChangeCommitted);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(5, 467);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 20, 3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(440, 25);
            this.label8.TabIndex = 45;
            this.label8.Text = "Score";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTest
            // 
            this.btnTest.AutoFont = true;
            this.btnTest.AutoFontHeightRatio = 0.75F;
            this.btnTest.AutoFontMax = 100F;
            this.btnTest.AutoFontMin = 6F;
            this.btnTest.AutoFontWidthRatio = 0.92F;
            this.btnTest.AutoImage = true;
            this.btnTest.AutoImageMaxRatio = 0.75F;
            this.btnTest.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnTest.AutoImageTint = true;
            this.btnTest.BackColor = System.Drawing.SystemColors.Control;
            this.btnTest.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnTest.BorderColor = System.Drawing.Color.Transparent;
            this.btnTest.BorderRadius = 1;
            this.btnTest.BorderSize = 1;
            this.btnTest.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnTest.Corner = BeeGlobal.Corner.Both;
            this.btnTest.DebounceResizeMs = 16;
            this.btnTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 70.99219F, System.Drawing.FontStyle.Bold);
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
            this.btnTest.Location = new System.Drawing.Point(20, 570);
            this.btnTest.Margin = new System.Windows.Forms.Padding(20);
            this.btnTest.Multiline = false;
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(408, 247);
            this.btnTest.TabIndex = 37;
            this.btnTest.Text = "Inspect";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(5, 170);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 20, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 25);
            this.label7.TabIndex = 38;
            this.label7.Text = "Line 1";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(454, 843);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
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
            // oK_Cancel1
            // 
            this.oK_Cancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oK_Cancel1.Location = new System.Drawing.Point(0, 887);
            this.oK_Cancel1.Name = "oK_Cancel1";
            this.oK_Cancel1.Size = new System.Drawing.Size(462, 52);
            this.oK_Cancel1.TabIndex = 18;
            // 
            // ToolMeasure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.oK_Cancel1);
            this.Controls.Add(this.pCany);
            this.Controls.Add(this.tabControl2);
            this.DoubleBuffered = true;
            this.Name = "ToolMeasure";
            this.Size = new System.Drawing.Size(462, 939);
            this.Load += new System.EventHandler(this.ToolCalculator_Load);
            this.VisibleChanged += new System.EventHandler(this.ToolOutLine_VisibleChanged);
            this.tabControl2.ResumeLayout(false);
            this.tabP1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel16.ResumeLayout(false);
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pCany.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabP1;
        private System.Windows.Forms.TabPage tabPage4;
        public System.ComponentModel.BackgroundWorker threadProcess;
        private System.Windows.Forms.Panel pCany;
        private RJButton btnAreaBlack;
        private RJButton btnAreaWhite;
        private RJButton btnTest;
        private System.Windows.Forms.Label label7;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label8;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox cb1;
        private System.Windows.Forms.ComboBox cb2;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ComboBox cb4;
        private System.Windows.Forms.ComboBox cb3;
        private RJButton rjButton12;
        private RJButton rjButton11;
        private System.Windows.Forms.Label label5;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private RJButton rjButton13;
        private System.Windows.Forms.ComboBox cb6;
        private System.Windows.Forms.ComboBox cb5;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private RJButton rjButton1;
        private System.Windows.Forms.ComboBox cb8;
        private System.Windows.Forms.ComboBox cb7;
        private GroupControl.OK_Cancel oK_Cancel1;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private RJButton rjButton2;
        private System.Windows.Forms.ComboBox cbMeasure;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel16;
        private RJButton rjButton10;
        private System.Windows.Forms.ComboBox cbDirect;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private RJButton rjButton3;
        private System.Windows.Forms.ComboBox cbMethord;
        private AdjustBarEx trackScore;
    }
}
