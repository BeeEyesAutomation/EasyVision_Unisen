using BeeCore;
using BeeGlobal;

namespace BeeInterface
{
    partial class ToolBarcode
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
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabP1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.EditRectRot1 = new BeeInterface.Group.EditRectRot();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.AdjIndexProgChoose = new BeeInterface.AdjustBarEx();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.btnModeSingle = new BeeInterface.RJButton();
            this.btnModeMulti = new BeeInterface.RJButton();
            this.layTemp = new System.Windows.Forms.TableLayoutPanel();
            this.imgTemp = new Cyotek.Windows.Forms.ImageBox();
            this.label5 = new System.Windows.Forms.Label();
            this.AdjOffSetArea = new BeeInterface.AdjustBarEx();
            this.label2 = new System.Windows.Forms.Label();
            this.trackScore = new BeeInterface.AdjustBarEx();
            this.label8 = new System.Windows.Forms.Label();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.btnChoose = new BeeInterface.RJButton();
            this.btnScan = new BeeInterface.RJButton();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton2 = new BeeInterface.RJButton();
            this.txtAdd = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tableLayoutPanel18 = new System.Windows.Forms.TableLayoutPanel();
            this.btnOffSendResult = new BeeInterface.RJButton();
            this.btnOnSendResult = new BeeInterface.RJButton();
            this.pInspect = new System.Windows.Forms.Panel();
            this.btnTest = new BeeInterface.RJButton();
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.tabControl2.SuspendLayout();
            this.tabP1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.layTemp.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel18.SuspendLayout();
            this.pInspect.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabP1);
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(489, 777);
            this.tabControl2.TabIndex = 17;
            // 
            // tabP1
            // 
            this.tabP1.BackColor = System.Drawing.SystemColors.Control;
            this.tabP1.Controls.Add(this.tableLayoutPanel1);
            this.tabP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabP1.Location = new System.Drawing.Point(4, 38);
            this.tabP1.Name = "tabP1";
            this.tabP1.Padding = new System.Windows.Forms.Padding(3);
            this.tabP1.Size = new System.Drawing.Size(481, 735);
            this.tabP1.TabIndex = 0;
            this.tabP1.Text = "Basic";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.EditRectRot1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 16);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel9, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel8, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.layTemp, 0, 17);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.AdjOffSetArea, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.trackScore, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel7, 0, 15);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.tableLayoutPanel1.RowCount = 20;
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(475, 729);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // EditRectRot1
            // 
            this.EditRectRot1._rotCurrent = null;
            this.EditRectRot1.BackColor = System.Drawing.SystemColors.Control;
            this.EditRectRot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditRectRot1.Location = new System.Drawing.Point(4, 45);
            this.EditRectRot1.Name = "EditRectRot1";
            this.EditRectRot1.rotCurrent = null;
            this.EditRectRot1.Size = new System.Drawing.Size(450, 330);
            this.EditRectRot1.TabIndex = 131;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(6, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(446, 32);
            this.label3.TabIndex = 97;
            this.label3.Text = "Choose Area";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(6, 793);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(446, 32);
            this.label9.TabIndex = 96;
            this.label9.Text = "Sample";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel9.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.AdjIndexProgChoose, 1, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(6, 470);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(446, 48);
            this.tableLayoutPanel9.TabIndex = 95;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(15, 5);
            this.label6.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(127, 38);
            this.label6.TabIndex = 59;
            this.label6.Text = "IndexProgChoose";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AdjIndexProgChoose
            // 
            this.AdjIndexProgChoose.AutoRepeatAccelDeltaMs = -5;
            this.AdjIndexProgChoose.AutoRepeatAccelerate = true;
            this.AdjIndexProgChoose.AutoRepeatEnabled = true;
            this.AdjIndexProgChoose.AutoRepeatInitialDelay = 400;
            this.AdjIndexProgChoose.AutoRepeatInterval = 60;
            this.AdjIndexProgChoose.AutoRepeatMinInterval = 20;
            this.AdjIndexProgChoose.AutoShowTextbox = true;
            this.AdjIndexProgChoose.AutoSizeTextbox = true;
            this.AdjIndexProgChoose.BackColor = System.Drawing.Color.White;
            this.AdjIndexProgChoose.BarLeftGap = 20;
            this.AdjIndexProgChoose.BarRightGap = 6;
            this.AdjIndexProgChoose.ChromeGap = 1;
            this.AdjIndexProgChoose.ChromeWidthRatio = 0.14F;
            this.AdjIndexProgChoose.ColorBorder = System.Drawing.Color.LightGray;
            this.AdjIndexProgChoose.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.AdjIndexProgChoose.ColorScale = System.Drawing.Color.LightGray;
            this.AdjIndexProgChoose.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjIndexProgChoose.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjIndexProgChoose.ColorTrack = System.Drawing.Color.LightGray;
            this.AdjIndexProgChoose.Decimals = 0;
            this.AdjIndexProgChoose.DisabledDesaturateMix = 0.3F;
            this.AdjIndexProgChoose.DisabledDimFactor = 0.9F;
            this.AdjIndexProgChoose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdjIndexProgChoose.EdgePadding = 2;
            this.AdjIndexProgChoose.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.AdjIndexProgChoose.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.AdjIndexProgChoose.KeyboardStep = 1F;
            this.AdjIndexProgChoose.Location = new System.Drawing.Point(150, 5);
            this.AdjIndexProgChoose.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.AdjIndexProgChoose.MatchTextboxFontToThumb = true;
            this.AdjIndexProgChoose.Max = 100F;
            this.AdjIndexProgChoose.MaxTextboxWidth = 0;
            this.AdjIndexProgChoose.MaxThumb = 1000;
            this.AdjIndexProgChoose.MaxTrackHeight = 1000;
            this.AdjIndexProgChoose.Min = 1F;
            this.AdjIndexProgChoose.MinChromeWidth = 64;
            this.AdjIndexProgChoose.MinimumSize = new System.Drawing.Size(140, 36);
            this.AdjIndexProgChoose.MinTextboxWidth = 32;
            this.AdjIndexProgChoose.MinThumb = 30;
            this.AdjIndexProgChoose.MinTrackHeight = 8;
            this.AdjIndexProgChoose.Name = "AdjIndexProgChoose";
            this.AdjIndexProgChoose.Radius = 8;
            this.AdjIndexProgChoose.ShowValueOnThumb = true;
            this.AdjIndexProgChoose.Size = new System.Drawing.Size(286, 38);
            this.AdjIndexProgChoose.SnapToStep = true;
            this.AdjIndexProgChoose.StartWithTextboxHidden = true;
            this.AdjIndexProgChoose.Step = 1F;
            this.AdjIndexProgChoose.TabIndex = 92;
            this.AdjIndexProgChoose.TextboxFontSize = 16F;
            this.AdjIndexProgChoose.TextboxSidePadding = 10;
            this.AdjIndexProgChoose.TextboxWidth = 600;
            this.AdjIndexProgChoose.ThumbDiameterRatio = 2F;
            this.AdjIndexProgChoose.ThumbValueBold = true;
            this.AdjIndexProgChoose.ThumbValueFontScale = 1.5F;
            this.AdjIndexProgChoose.ThumbValuePadding = 0;
            this.AdjIndexProgChoose.TightEdges = true;
            this.AdjIndexProgChoose.TrackHeightRatio = 0.45F;
            this.AdjIndexProgChoose.TrackWidthRatio = 1F;
            this.AdjIndexProgChoose.UnitText = "";
            this.AdjIndexProgChoose.Value = 1F;
            this.AdjIndexProgChoose.WheelStep = 1F;
            this.AdjIndexProgChoose.ValueChanged += new System.Action<float>(this.AdjIndexProgChoose_ValueChanged);
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.Controls.Add(this.btnModeSingle, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.btnModeMulti, 0, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(6, 420);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(446, 50);
            this.tableLayoutPanel8.TabIndex = 94;
            // 
            // btnModeSingle
            // 
            this.btnModeSingle.AutoFont = true;
            this.btnModeSingle.AutoFontHeightRatio = 0.75F;
            this.btnModeSingle.AutoFontMax = 100F;
            this.btnModeSingle.AutoFontMin = 6F;
            this.btnModeSingle.AutoFontWidthRatio = 0.92F;
            this.btnModeSingle.AutoImage = true;
            this.btnModeSingle.AutoImageMaxRatio = 0.75F;
            this.btnModeSingle.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnModeSingle.AutoImageTint = true;
            this.btnModeSingle.BackColor = System.Drawing.Color.White;
            this.btnModeSingle.BackgroundColor = System.Drawing.Color.White;
            this.btnModeSingle.BorderColor = System.Drawing.Color.White;
            this.btnModeSingle.BorderRadius = 10;
            this.btnModeSingle.BorderSize = 1;
            this.btnModeSingle.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnModeSingle.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnModeSingle.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnModeSingle.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnModeSingle.Corner = BeeGlobal.Corner.Right;
            this.btnModeSingle.DebounceResizeMs = 16;
            this.btnModeSingle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnModeSingle.FlatAppearance.BorderSize = 0;
            this.btnModeSingle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModeSingle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnModeSingle.ForeColor = System.Drawing.Color.Black;
            this.btnModeSingle.Image = null;
            this.btnModeSingle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModeSingle.ImageDisabled = null;
            this.btnModeSingle.ImageHover = null;
            this.btnModeSingle.ImageNormal = null;
            this.btnModeSingle.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnModeSingle.ImagePressed = null;
            this.btnModeSingle.ImageTextSpacing = 6;
            this.btnModeSingle.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnModeSingle.ImageTintHover = System.Drawing.Color.Empty;
            this.btnModeSingle.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnModeSingle.ImageTintOpacity = 0.5F;
            this.btnModeSingle.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnModeSingle.IsCLick = true;
            this.btnModeSingle.IsNotChange = false;
            this.btnModeSingle.IsRect = false;
            this.btnModeSingle.IsTouch = false;
            this.btnModeSingle.IsUnGroup = false;
            this.btnModeSingle.Location = new System.Drawing.Point(223, 5);
            this.btnModeSingle.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnModeSingle.Multiline = false;
            this.btnModeSingle.Name = "btnModeSingle";
            this.btnModeSingle.Size = new System.Drawing.Size(215, 40);
            this.btnModeSingle.TabIndex = 3;
            this.btnModeSingle.Text = "Single";
            this.btnModeSingle.TextColor = System.Drawing.Color.Black;
            this.btnModeSingle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnModeSingle.UseVisualStyleBackColor = false;
            this.btnModeSingle.Click += new System.EventHandler(this.btnModeSingle_Click);
            // 
            // btnModeMulti
            // 
            this.btnModeMulti.AutoFont = true;
            this.btnModeMulti.AutoFontHeightRatio = 0.75F;
            this.btnModeMulti.AutoFontMax = 100F;
            this.btnModeMulti.AutoFontMin = 6F;
            this.btnModeMulti.AutoFontWidthRatio = 0.92F;
            this.btnModeMulti.AutoImage = true;
            this.btnModeMulti.AutoImageMaxRatio = 0.75F;
            this.btnModeMulti.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnModeMulti.AutoImageTint = true;
            this.btnModeMulti.BackColor = System.Drawing.Color.White;
            this.btnModeMulti.BackgroundColor = System.Drawing.Color.White;
            this.btnModeMulti.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnModeMulti.BorderColor = System.Drawing.Color.White;
            this.btnModeMulti.BorderRadius = 10;
            this.btnModeMulti.BorderSize = 1;
            this.btnModeMulti.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnModeMulti.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnModeMulti.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnModeMulti.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnModeMulti.Corner = BeeGlobal.Corner.Left;
            this.btnModeMulti.DebounceResizeMs = 16;
            this.btnModeMulti.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnModeMulti.FlatAppearance.BorderSize = 0;
            this.btnModeMulti.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModeMulti.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnModeMulti.ForeColor = System.Drawing.Color.Black;
            this.btnModeMulti.Image = null;
            this.btnModeMulti.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModeMulti.ImageDisabled = null;
            this.btnModeMulti.ImageHover = null;
            this.btnModeMulti.ImageNormal = null;
            this.btnModeMulti.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnModeMulti.ImagePressed = null;
            this.btnModeMulti.ImageTextSpacing = 6;
            this.btnModeMulti.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnModeMulti.ImageTintHover = System.Drawing.Color.Empty;
            this.btnModeMulti.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnModeMulti.ImageTintOpacity = 0.5F;
            this.btnModeMulti.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnModeMulti.IsCLick = false;
            this.btnModeMulti.IsNotChange = false;
            this.btnModeMulti.IsRect = false;
            this.btnModeMulti.IsTouch = false;
            this.btnModeMulti.IsUnGroup = false;
            this.btnModeMulti.Location = new System.Drawing.Point(8, 5);
            this.btnModeMulti.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnModeMulti.Multiline = false;
            this.btnModeMulti.Name = "btnModeMulti";
            this.btnModeMulti.Size = new System.Drawing.Size(215, 40);
            this.btnModeMulti.TabIndex = 2;
            this.btnModeMulti.Text = "Multi";
            this.btnModeMulti.TextColor = System.Drawing.Color.Black;
            this.btnModeMulti.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnModeMulti.UseVisualStyleBackColor = false;
            this.btnModeMulti.Click += new System.EventHandler(this.rjButton2_Click_1);
            // 
            // layTemp
            // 
            this.layTemp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.layTemp.ColumnCount = 1;
            this.layTemp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layTemp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layTemp.Controls.Add(this.imgTemp, 0, 0);
            this.layTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layTemp.Location = new System.Drawing.Point(6, 825);
            this.layTemp.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.layTemp.Name = "layTemp";
            this.layTemp.RowCount = 1;
            this.layTemp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layTemp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.layTemp.Size = new System.Drawing.Size(446, 200);
            this.layTemp.TabIndex = 89;
            // 
            // imgTemp
            // 
            this.imgTemp.AllowFreePan = false;
            this.imgTemp.AutoCenter = false;
            this.imgTemp.AutoPan = false;
            this.imgTemp.AutoScroll = false;
            this.imgTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgTemp.GridDisplayMode = Cyotek.Windows.Forms.ImageBoxGridDisplayMode.None;
            this.imgTemp.Location = new System.Drawing.Point(10, 10);
            this.imgTemp.Margin = new System.Windows.Forms.Padding(10);
            this.imgTemp.Name = "imgTemp";
            this.imgTemp.PanMode = Cyotek.Windows.Forms.ImageBoxPanMode.Middle;
            this.imgTemp.Size = new System.Drawing.Size(426, 180);
            this.imgTemp.TabIndex = 0;
            this.imgTemp.Zoom = 150;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(6, 388);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(446, 32);
            this.label5.TabIndex = 93;
            this.label5.Text = "Mode Check";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AdjOffSetArea
            // 
            this.AdjOffSetArea.AutoRepeatAccelDeltaMs = -5;
            this.AdjOffSetArea.AutoRepeatAccelerate = true;
            this.AdjOffSetArea.AutoRepeatEnabled = true;
            this.AdjOffSetArea.AutoRepeatInitialDelay = 400;
            this.AdjOffSetArea.AutoRepeatInterval = 60;
            this.AdjOffSetArea.AutoRepeatMinInterval = 20;
            this.AdjOffSetArea.AutoShowTextbox = true;
            this.AdjOffSetArea.AutoSizeTextbox = true;
            this.AdjOffSetArea.BackColor = System.Drawing.Color.White;
            this.AdjOffSetArea.BarLeftGap = 20;
            this.AdjOffSetArea.BarRightGap = 6;
            this.AdjOffSetArea.ChromeGap = 1;
            this.AdjOffSetArea.ChromeWidthRatio = 0.14F;
            this.AdjOffSetArea.ColorBorder = System.Drawing.Color.LightGray;
            this.AdjOffSetArea.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.AdjOffSetArea.ColorScale = System.Drawing.Color.LightGray;
            this.AdjOffSetArea.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjOffSetArea.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjOffSetArea.ColorTrack = System.Drawing.Color.LightGray;
            this.AdjOffSetArea.Decimals = 0;
            this.AdjOffSetArea.DisabledDesaturateMix = 0.3F;
            this.AdjOffSetArea.DisabledDimFactor = 0.9F;
            this.AdjOffSetArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdjOffSetArea.EdgePadding = 2;
            this.AdjOffSetArea.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.AdjOffSetArea.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.AdjOffSetArea.KeyboardStep = 1F;
            this.AdjOffSetArea.Location = new System.Drawing.Point(6, 560);
            this.AdjOffSetArea.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.AdjOffSetArea.MatchTextboxFontToThumb = true;
            this.AdjOffSetArea.Max = 100F;
            this.AdjOffSetArea.MaxTextboxWidth = 0;
            this.AdjOffSetArea.MaxThumb = 1000;
            this.AdjOffSetArea.MaxTrackHeight = 1000;
            this.AdjOffSetArea.Min = 1F;
            this.AdjOffSetArea.MinChromeWidth = 64;
            this.AdjOffSetArea.MinimumSize = new System.Drawing.Size(140, 36);
            this.AdjOffSetArea.MinTextboxWidth = 32;
            this.AdjOffSetArea.MinThumb = 30;
            this.AdjOffSetArea.MinTrackHeight = 8;
            this.AdjOffSetArea.Name = "AdjOffSetArea";
            this.AdjOffSetArea.Radius = 8;
            this.AdjOffSetArea.ShowValueOnThumb = true;
            this.AdjOffSetArea.Size = new System.Drawing.Size(446, 53);
            this.AdjOffSetArea.SnapToStep = true;
            this.AdjOffSetArea.StartWithTextboxHidden = true;
            this.AdjOffSetArea.Step = 1F;
            this.AdjOffSetArea.TabIndex = 91;
            this.AdjOffSetArea.TextboxFontSize = 22F;
            this.AdjOffSetArea.TextboxSidePadding = 10;
            this.AdjOffSetArea.TextboxWidth = 600;
            this.AdjOffSetArea.ThumbDiameterRatio = 2F;
            this.AdjOffSetArea.ThumbValueBold = true;
            this.AdjOffSetArea.ThumbValueFontScale = 1.5F;
            this.AdjOffSetArea.ThumbValuePadding = 0;
            this.AdjOffSetArea.TightEdges = true;
            this.AdjOffSetArea.TrackHeightRatio = 0.45F;
            this.AdjOffSetArea.TrackWidthRatio = 1F;
            this.AdjOffSetArea.UnitText = "";
            this.AdjOffSetArea.Value = 1F;
            this.AdjOffSetArea.WheelStep = 1F;
            this.AdjOffSetArea.ValueChanged += new System.Action<float>(this.AdjOffSetArea_ValueChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(6, 528);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(446, 32);
            this.label2.TabIndex = 90;
            this.label2.Text = "Offset Area Check (%)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackScore
            // 
            this.trackScore.AutoRepeatAccelDeltaMs = -5;
            this.trackScore.AutoRepeatAccelerate = true;
            this.trackScore.AutoRepeatEnabled = true;
            this.trackScore.AutoRepeatInitialDelay = 400;
            this.trackScore.AutoRepeatInterval = 60;
            this.trackScore.AutoRepeatMinInterval = 20;
            this.trackScore.AutoShowTextbox = true;
            this.trackScore.AutoSizeTextbox = true;
            this.trackScore.BackColor = System.Drawing.Color.White;
            this.trackScore.BarLeftGap = 20;
            this.trackScore.BarRightGap = 6;
            this.trackScore.ChromeGap = 1;
            this.trackScore.ChromeWidthRatio = 0.14F;
            this.trackScore.ColorBorder = System.Drawing.Color.LightGray;
            this.trackScore.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.trackScore.ColorScale = System.Drawing.Color.LightGray;
            this.trackScore.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackScore.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.trackScore.ColorTrack = System.Drawing.Color.LightGray;
            this.trackScore.Decimals = 0;
            this.trackScore.DisabledDesaturateMix = 0.3F;
            this.trackScore.DisabledDimFactor = 0.9F;
            this.trackScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackScore.EdgePadding = 2;
            this.trackScore.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackScore.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackScore.KeyboardStep = 1F;
            this.trackScore.Location = new System.Drawing.Point(6, 655);
            this.trackScore.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
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
            this.trackScore.Size = new System.Drawing.Size(446, 53);
            this.trackScore.SnapToStep = true;
            this.trackScore.StartWithTextboxHidden = true;
            this.trackScore.Step = 1F;
            this.trackScore.TabIndex = 80;
            this.trackScore.TextboxFontSize = 22F;
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
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(6, 623);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(446, 32);
            this.label8.TabIndex = 77;
            this.label8.Text = "Tolerance";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel7.ColumnCount = 4;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel7.Controls.Add(this.btnChoose, 3, 0);
            this.tableLayoutPanel7.Controls.Add(this.btnScan, 2, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(6, 723);
            this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(5, 15, 5, 0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(446, 60);
            this.tableLayoutPanel7.TabIndex = 92;
            // 
            // btnChoose
            // 
            this.btnChoose.AutoFont = false;
            this.btnChoose.AutoFontHeightRatio = 0.75F;
            this.btnChoose.AutoFontMax = 100F;
            this.btnChoose.AutoFontMin = 6F;
            this.btnChoose.AutoFontWidthRatio = 0.92F;
            this.btnChoose.AutoImage = true;
            this.btnChoose.AutoImageMaxRatio = 0.75F;
            this.btnChoose.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnChoose.AutoImageTint = true;
            this.btnChoose.BackColor = System.Drawing.SystemColors.Control;
            this.btnChoose.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnChoose.BorderColor = System.Drawing.SystemColors.Control;
            this.btnChoose.BorderRadius = 10;
            this.btnChoose.BorderSize = 1;
            this.btnChoose.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnChoose.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnChoose.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnChoose.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnChoose.Corner = BeeGlobal.Corner.Both;
            this.btnChoose.DebounceResizeMs = 16;
            this.btnChoose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnChoose.FlatAppearance.BorderSize = 0;
            this.btnChoose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChoose.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChoose.ForeColor = System.Drawing.Color.Black;
            this.btnChoose.Image = null;
            this.btnChoose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChoose.ImageDisabled = null;
            this.btnChoose.ImageHover = null;
            this.btnChoose.ImageNormal = null;
            this.btnChoose.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnChoose.ImagePressed = null;
            this.btnChoose.ImageTextSpacing = 6;
            this.btnChoose.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnChoose.ImageTintHover = System.Drawing.Color.Empty;
            this.btnChoose.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnChoose.ImageTintOpacity = 0.5F;
            this.btnChoose.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnChoose.IsCLick = false;
            this.btnChoose.IsNotChange = false;
            this.btnChoose.IsRect = false;
            this.btnChoose.IsTouch = false;
            this.btnChoose.IsUnGroup = true;
            this.btnChoose.Location = new System.Drawing.Point(233, 5);
            this.btnChoose.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnChoose.Multiline = false;
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(208, 50);
            this.btnChoose.TabIndex = 88;
            this.btnChoose.Text = "Choose";
            this.btnChoose.TextColor = System.Drawing.Color.Black;
            this.btnChoose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnChoose.UseVisualStyleBackColor = false;
            this.btnChoose.Click += new System.EventHandler(this.rjButton2_Click);
            // 
            // btnScan
            // 
            this.btnScan.AutoFont = false;
            this.btnScan.AutoFontHeightRatio = 0.75F;
            this.btnScan.AutoFontMax = 100F;
            this.btnScan.AutoFontMin = 6F;
            this.btnScan.AutoFontWidthRatio = 0.92F;
            this.btnScan.AutoImage = true;
            this.btnScan.AutoImageMaxRatio = 0.75F;
            this.btnScan.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnScan.AutoImageTint = true;
            this.btnScan.BackColor = System.Drawing.SystemColors.Control;
            this.btnScan.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnScan.BorderColor = System.Drawing.SystemColors.Control;
            this.btnScan.BorderRadius = 10;
            this.btnScan.BorderSize = 1;
            this.btnScan.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnScan.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnScan.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnScan.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnScan.Corner = BeeGlobal.Corner.Both;
            this.btnScan.DebounceResizeMs = 16;
            this.btnScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnScan.FlatAppearance.BorderSize = 0;
            this.btnScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScan.ForeColor = System.Drawing.Color.Black;
            this.btnScan.Image = null;
            this.btnScan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnScan.ImageDisabled = null;
            this.btnScan.ImageHover = null;
            this.btnScan.ImageNormal = null;
            this.btnScan.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnScan.ImagePressed = null;
            this.btnScan.ImageTextSpacing = 6;
            this.btnScan.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnScan.ImageTintHover = System.Drawing.Color.Empty;
            this.btnScan.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnScan.ImageTintOpacity = 0.5F;
            this.btnScan.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnScan.IsCLick = false;
            this.btnScan.IsNotChange = true;
            this.btnScan.IsRect = false;
            this.btnScan.IsTouch = false;
            this.btnScan.IsUnGroup = true;
            this.btnScan.Location = new System.Drawing.Point(5, 5);
            this.btnScan.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnScan.Multiline = false;
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(208, 50);
            this.btnScan.TabIndex = 87;
            this.btnScan.Text = "Scan";
            this.btnScan.TextColor = System.Drawing.Color.Black;
            this.btnScan.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnScan.UseVisualStyleBackColor = false;
            this.btnScan.Click += new System.EventHandler(this.btnScanOCR_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.tableLayoutPanel10);
            this.tabPage1.Location = new System.Drawing.Point(4, 38);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(481, 735);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "PLC";
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.AutoScroll = true;
            this.tableLayoutPanel10.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel10.ColumnCount = 1;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel12, 0, 9);
            this.tableLayoutPanel10.Controls.Add(this.label12, 0, 8);
            this.tableLayoutPanel10.Controls.Add(this.label18, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel18, 0, 1);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.tableLayoutPanel10.RowCount = 20;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(475, 729);
            this.tableLayoutPanel10.TabIndex = 2;
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel12.ColumnCount = 2;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel12.Controls.Add(this.rjButton2, 0, 0);
            this.tableLayoutPanel12.Controls.Add(this.txtAdd, 1, 0);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(6, 133);
            this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(463, 50);
            this.tableLayoutPanel12.TabIndex = 94;
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
            this.rjButton2.BackColor = System.Drawing.Color.White;
            this.rjButton2.BackgroundColor = System.Drawing.Color.White;
            this.rjButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton2.BorderColor = System.Drawing.Color.White;
            this.rjButton2.BorderRadius = 10;
            this.rjButton2.BorderSize = 1;
            this.rjButton2.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.rjButton2.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.rjButton2.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.rjButton2.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton2.Corner = BeeGlobal.Corner.Left;
            this.rjButton2.DebounceResizeMs = 16;
            this.rjButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton2.FlatAppearance.BorderSize = 0;
            this.rjButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
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
            this.rjButton2.IsTouch = false;
            this.rjButton2.IsUnGroup = false;
            this.rjButton2.Location = new System.Drawing.Point(8, 5);
            this.rjButton2.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.rjButton2.Multiline = false;
            this.rjButton2.Name = "rjButton2";
            this.rjButton2.Size = new System.Drawing.Size(223, 40);
            this.rjButton2.TabIndex = 2;
            this.rjButton2.Text = "Address";
            this.rjButton2.TextColor = System.Drawing.Color.Black;
            this.rjButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton2.UseVisualStyleBackColor = false;
            // 
            // txtAdd
            // 
            this.txtAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAdd.Location = new System.Drawing.Point(234, 8);
            this.txtAdd.Name = "txtAdd";
            this.txtAdd.Size = new System.Drawing.Size(221, 38);
            this.txtAdd.TabIndex = 3;
            this.txtAdd.TextChanged += new System.EventHandler(this.txtAdd_TextChanged);
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Transparent;
            this.label12.Location = new System.Drawing.Point(6, 101);
            this.label12.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(463, 32);
            this.label12.TabIndex = 93;
            this.label12.Text = "Address Write ";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Transparent;
            this.label18.Location = new System.Drawing.Point(6, 5);
            this.label18.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(463, 32);
            this.label18.TabIndex = 71;
            this.label18.Text = "Send Result";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel18
            // 
            this.tableLayoutPanel18.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel18.ColumnCount = 2;
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel18.Controls.Add(this.btnOffSendResult, 1, 0);
            this.tableLayoutPanel18.Controls.Add(this.btnOnSendResult, 0, 0);
            this.tableLayoutPanel18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel18.Location = new System.Drawing.Point(6, 37);
            this.tableLayoutPanel18.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel18.Name = "tableLayoutPanel18";
            this.tableLayoutPanel18.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel18.RowCount = 1;
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel18.Size = new System.Drawing.Size(463, 54);
            this.tableLayoutPanel18.TabIndex = 39;
            // 
            // btnOffSendResult
            // 
            this.btnOffSendResult.AutoFont = true;
            this.btnOffSendResult.AutoFontHeightRatio = 0.75F;
            this.btnOffSendResult.AutoFontMax = 100F;
            this.btnOffSendResult.AutoFontMin = 6F;
            this.btnOffSendResult.AutoFontWidthRatio = 0.92F;
            this.btnOffSendResult.AutoImage = true;
            this.btnOffSendResult.AutoImageMaxRatio = 0.75F;
            this.btnOffSendResult.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnOffSendResult.AutoImageTint = true;
            this.btnOffSendResult.BackColor = System.Drawing.Color.White;
            this.btnOffSendResult.BackgroundColor = System.Drawing.Color.White;
            this.btnOffSendResult.BorderColor = System.Drawing.Color.White;
            this.btnOffSendResult.BorderRadius = 10;
            this.btnOffSendResult.BorderSize = 1;
            this.btnOffSendResult.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnOffSendResult.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnOffSendResult.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnOffSendResult.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnOffSendResult.Corner = BeeGlobal.Corner.Right;
            this.btnOffSendResult.DebounceResizeMs = 16;
            this.btnOffSendResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOffSendResult.FlatAppearance.BorderSize = 0;
            this.btnOffSendResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOffSendResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnOffSendResult.ForeColor = System.Drawing.Color.Black;
            this.btnOffSendResult.Image = null;
            this.btnOffSendResult.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOffSendResult.ImageDisabled = null;
            this.btnOffSendResult.ImageHover = null;
            this.btnOffSendResult.ImageNormal = null;
            this.btnOffSendResult.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnOffSendResult.ImagePressed = null;
            this.btnOffSendResult.ImageTextSpacing = 6;
            this.btnOffSendResult.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnOffSendResult.ImageTintHover = System.Drawing.Color.Empty;
            this.btnOffSendResult.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnOffSendResult.ImageTintOpacity = 0.5F;
            this.btnOffSendResult.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnOffSendResult.IsCLick = false;
            this.btnOffSendResult.IsNotChange = false;
            this.btnOffSendResult.IsRect = false;
            this.btnOffSendResult.IsTouch = false;
            this.btnOffSendResult.IsUnGroup = false;
            this.btnOffSendResult.Location = new System.Drawing.Point(231, 5);
            this.btnOffSendResult.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnOffSendResult.Multiline = false;
            this.btnOffSendResult.Name = "btnOffSendResult";
            this.btnOffSendResult.Size = new System.Drawing.Size(224, 44);
            this.btnOffSendResult.TabIndex = 3;
            this.btnOffSendResult.Text = "OFF";
            this.btnOffSendResult.TextColor = System.Drawing.Color.Black;
            this.btnOffSendResult.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOffSendResult.UseVisualStyleBackColor = false;
            this.btnOffSendResult.Click += new System.EventHandler(this.btnOffSendResult_Click);
            // 
            // btnOnSendResult
            // 
            this.btnOnSendResult.AutoFont = true;
            this.btnOnSendResult.AutoFontHeightRatio = 0.75F;
            this.btnOnSendResult.AutoFontMax = 100F;
            this.btnOnSendResult.AutoFontMin = 6F;
            this.btnOnSendResult.AutoFontWidthRatio = 0.92F;
            this.btnOnSendResult.AutoImage = true;
            this.btnOnSendResult.AutoImageMaxRatio = 0.75F;
            this.btnOnSendResult.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnOnSendResult.AutoImageTint = true;
            this.btnOnSendResult.BackColor = System.Drawing.Color.White;
            this.btnOnSendResult.BackgroundColor = System.Drawing.Color.White;
            this.btnOnSendResult.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOnSendResult.BorderColor = System.Drawing.Color.White;
            this.btnOnSendResult.BorderRadius = 10;
            this.btnOnSendResult.BorderSize = 1;
            this.btnOnSendResult.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnOnSendResult.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnOnSendResult.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnOnSendResult.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnOnSendResult.Corner = BeeGlobal.Corner.Left;
            this.btnOnSendResult.DebounceResizeMs = 16;
            this.btnOnSendResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOnSendResult.FlatAppearance.BorderSize = 0;
            this.btnOnSendResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOnSendResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnOnSendResult.ForeColor = System.Drawing.Color.Black;
            this.btnOnSendResult.Image = null;
            this.btnOnSendResult.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOnSendResult.ImageDisabled = null;
            this.btnOnSendResult.ImageHover = null;
            this.btnOnSendResult.ImageNormal = null;
            this.btnOnSendResult.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnOnSendResult.ImagePressed = null;
            this.btnOnSendResult.ImageTextSpacing = 6;
            this.btnOnSendResult.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnOnSendResult.ImageTintHover = System.Drawing.Color.Empty;
            this.btnOnSendResult.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnOnSendResult.ImageTintOpacity = 0.5F;
            this.btnOnSendResult.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnOnSendResult.IsCLick = true;
            this.btnOnSendResult.IsNotChange = false;
            this.btnOnSendResult.IsRect = false;
            this.btnOnSendResult.IsTouch = false;
            this.btnOnSendResult.IsUnGroup = false;
            this.btnOnSendResult.Location = new System.Drawing.Point(8, 5);
            this.btnOnSendResult.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnOnSendResult.Multiline = false;
            this.btnOnSendResult.Name = "btnOnSendResult";
            this.btnOnSendResult.Size = new System.Drawing.Size(223, 44);
            this.btnOnSendResult.TabIndex = 2;
            this.btnOnSendResult.Text = "ON";
            this.btnOnSendResult.TextColor = System.Drawing.Color.Black;
            this.btnOnSendResult.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOnSendResult.UseVisualStyleBackColor = false;
            this.btnOnSendResult.Click += new System.EventHandler(this.btnOnSendResult_Click);
            // 
            // pInspect
            // 
            this.pInspect.Controls.Add(this.btnTest);
            this.pInspect.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pInspect.Location = new System.Drawing.Point(0, 777);
            this.pInspect.Name = "pInspect";
            this.pInspect.Padding = new System.Windows.Forms.Padding(10);
            this.pInspect.Size = new System.Drawing.Size(489, 100);
            this.pInspect.TabIndex = 82;
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
            this.btnTest.BackColor = System.Drawing.SystemColors.Control;
            this.btnTest.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnTest.BorderColor = System.Drawing.SystemColors.Control;
            this.btnTest.BorderRadius = 10;
            this.btnTest.BorderSize = 1;
            this.btnTest.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnTest.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnTest.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
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
            this.btnTest.IsTouch = false;
            this.btnTest.IsUnGroup = true;
            this.btnTest.Location = new System.Drawing.Point(10, 10);
            this.btnTest.Margin = new System.Windows.Forms.Padding(10);
            this.btnTest.Multiline = false;
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(469, 80);
            this.btnTest.TabIndex = 81;
            this.btnTest.Text = "Inspect";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // oK_Cancel1
            // 
            this.oK_Cancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oK_Cancel1.Location = new System.Drawing.Point(0, 877);
            this.oK_Cancel1.Name = "oK_Cancel1";
            this.oK_Cancel1.Size = new System.Drawing.Size(489, 62);
            this.oK_Cancel1.TabIndex = 18;
            // 
            // ToolBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.pInspect);
            this.Controls.Add(this.oK_Cancel1);
            this.DoubleBuffered = true;
            this.Name = "ToolBarcode";
            this.Size = new System.Drawing.Size(489, 939);
            this.tabControl2.ResumeLayout(false);
            this.tabP1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.layTemp.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel12.PerformLayout();
            this.tableLayoutPanel18.ResumeLayout(false);
            this.pInspect.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabP1;
        private GroupControl.OK_Cancel oK_Cancel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label8;
        private AdjustBarEx trackScore;
        private System.Windows.Forms.Panel pInspect;
        private RJButton btnTest;
        private RJButton btnChoose;
        private RJButton btnScan;
        private System.Windows.Forms.TableLayoutPanel layTemp;
        private AdjustBarEx AdjOffSetArea;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private RJButton btnModeSingle;
        private RJButton btnModeMulti;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.Label label6;
        private AdjustBarEx AdjIndexProgChoose;
        private Cyotek.Windows.Forms.ImageBox imgTemp;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
        private RJButton rjButton2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel18;
        private RJButton btnOffSendResult;
        private RJButton btnOnSendResult;
        private System.Windows.Forms.TextBox txtAdd;
        private System.Windows.Forms.Label label3;
        private Group.EditRectRot EditRectRot1;
    }
}
