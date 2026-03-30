using BeeGlobal;
using System.Windows.Forms;

namespace BeeInterface
{
    partial class ToolOCR
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabP1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.EditRectRot1 = new BeeInterface.Group.EditRectRot();
            this.lay8 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnValuePLC = new BeeInterface.RJButton();
            this.txtAddressPLC = new System.Windows.Forms.TextBox();
            this.btn8 = new BeeInterface.RJButton();
            this.AdjLimitArea = new BeeInterface.AdjustBarEx();
            this.btn6 = new BeeInterface.RJButton();
            this.btn5 = new BeeInterface.RJButton();
            this.lay4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSet = new BeeInterface.RJButton();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.btn4 = new BeeInterface.RJButton();
            this.btn2 = new BeeInterface.RJButton();
            this.btn3 = new BeeInterface.RJButton();
            this.trackScore = new BeeInterface.AdjustBarEx();
            this.lay3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnApply = new BeeInterface.RJButton();
            this.txtAllow = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.numBlur = new BeeInterface.CustomNumericEx();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.numUnsharp = new BeeInterface.CustomNumericEx();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.numCLAHE = new BeeInterface.CustomNumericEx();
            this.label1 = new System.Windows.Forms.Label();
            this.tmCheckFist = new System.Windows.Forms.Timer(this.components);
            this.workLoadModel = new System.ComponentModel.BackgroundWorker();
            this.pInspect = new System.Windows.Forms.Panel();
            this.btnTest = new BeeInterface.RJButton();
            this.tmEnble = new System.Windows.Forms.Timer(this.components);
            this.oK_Cancel1 = new BeeInterface.GroupControl.OK_Cancel();
            this.tabControl1.SuspendLayout();
            this.tabP1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.lay8.SuspendLayout();
            this.lay4.SuspendLayout();
            this.lay3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.pInspect.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabP1);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(450, 1166);
            this.tabControl1.TabIndex = 18;
            // 
            // tabP1
            // 
            this.tabP1.AutoScroll = true;
            this.tabP1.BackColor = System.Drawing.SystemColors.Control;
            this.tabP1.Controls.Add(this.tableLayoutPanel1);
            this.tabP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabP1.Location = new System.Drawing.Point(4, 34);
            this.tabP1.Name = "tabP1";
            this.tabP1.Padding = new System.Windows.Forms.Padding(3);
            this.tabP1.Size = new System.Drawing.Size(442, 1128);
            this.tabP1.TabIndex = 0;
            this.tabP1.Text = "Basic";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.EditRectRot1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lay8, 0, 15);
            this.tableLayoutPanel1.Controls.Add(this.btn8, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.AdjLimitArea, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.btn6, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.btn5, 0, 16);
            this.tableLayoutPanel1.Controls.Add(this.lay4, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.btn4, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.btn2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btn3, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.trackScore, 0, 17);
            this.tableLayoutPanel1.Controls.Add(this.lay3, 0, 9);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 19;
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(436, 1122);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // EditRectRot1
            // 
            this.EditRectRot1._rotCurrent = null;
            this.EditRectRot1.BackColor = System.Drawing.SystemColors.Control;
            this.EditRectRot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditRectRot1.IsHide = false;
            this.EditRectRot1.Location = new System.Drawing.Point(3, 48);
            this.EditRectRot1.Name = "EditRectRot1";
            this.EditRectRot1.rotCurrent = null;
            this.EditRectRot1.Size = new System.Drawing.Size(430, 330);
            this.EditRectRot1.TabIndex = 131;
            this.EditRectRot1.Visible = false;
            // 
            // lay8
            // 
            this.lay8.ColumnCount = 2;
            this.lay8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.lay8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.lay8.Controls.Add(this.label4, 1, 0);
            this.lay8.Controls.Add(this.label3, 0, 0);
            this.lay8.Controls.Add(this.btnValuePLC, 1, 1);
            this.lay8.Controls.Add(this.txtAddressPLC, 0, 1);
            this.lay8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lay8.Location = new System.Drawing.Point(3, 784);
            this.lay8.Name = "lay8";
            this.lay8.RowCount = 2;
            this.lay8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.lay8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.lay8.Size = new System.Drawing.Size(430, 80);
            this.lay8.TabIndex = 114;
            this.lay8.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(220, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(205, 26);
            this.label4.TabIndex = 105;
            this.label4.Text = "Value";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(205, 26);
            this.label3.TabIndex = 104;
            this.label3.Text = "AddPLC";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Visible = false;
            // 
            // btnValuePLC
            // 
            this.btnValuePLC.AutoFont = false;
            this.btnValuePLC.AutoFontHeightRatio = 0.75F;
            this.btnValuePLC.AutoFontMax = 100F;
            this.btnValuePLC.AutoFontMin = 6F;
            this.btnValuePLC.AutoFontWidthRatio = 0.92F;
            this.btnValuePLC.AutoImage = true;
            this.btnValuePLC.AutoImageMaxRatio = 0.75F;
            this.btnValuePLC.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnValuePLC.AutoImageTint = true;
            this.btnValuePLC.BackColor = System.Drawing.SystemColors.Control;
            this.btnValuePLC.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnValuePLC.BorderColor = System.Drawing.SystemColors.Control;
            this.btnValuePLC.BorderRadius = 10;
            this.btnValuePLC.BorderSize = 1;
            this.btnValuePLC.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnValuePLC.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnValuePLC.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnValuePLC.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnValuePLC.Corner = BeeGlobal.Corner.Both;
            this.btnValuePLC.DebounceResizeMs = 16;
            this.btnValuePLC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnValuePLC.FlatAppearance.BorderSize = 0;
            this.btnValuePLC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValuePLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValuePLC.ForeColor = System.Drawing.Color.Black;
            this.btnValuePLC.Image = null;
            this.btnValuePLC.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnValuePLC.ImageDisabled = null;
            this.btnValuePLC.ImageHover = null;
            this.btnValuePLC.ImageNormal = null;
            this.btnValuePLC.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnValuePLC.ImagePressed = null;
            this.btnValuePLC.ImageTextSpacing = 6;
            this.btnValuePLC.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnValuePLC.ImageTintHover = System.Drawing.Color.Empty;
            this.btnValuePLC.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnValuePLC.ImageTintOpacity = 0.5F;
            this.btnValuePLC.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnValuePLC.IsCLick = false;
            this.btnValuePLC.IsNotChange = true;
            this.btnValuePLC.IsRect = false;
            this.btnValuePLC.IsTouch = false;
            this.btnValuePLC.IsUnGroup = true;
            this.btnValuePLC.Location = new System.Drawing.Point(218, 29);
            this.btnValuePLC.Multiline = false;
            this.btnValuePLC.Name = "btnValuePLC";
            this.btnValuePLC.Size = new System.Drawing.Size(209, 48);
            this.btnValuePLC.TabIndex = 82;
            this.btnValuePLC.Text = "----";
            this.btnValuePLC.TextColor = System.Drawing.Color.Black;
            this.btnValuePLC.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnValuePLC.UseVisualStyleBackColor = false;
            this.btnValuePLC.Click += new System.EventHandler(this.btnValuePLC_Click);
            // 
            // txtAddressPLC
            // 
            this.txtAddressPLC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAddressPLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddressPLC.Location = new System.Drawing.Point(10, 29);
            this.txtAddressPLC.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.txtAddressPLC.Name = "txtAddressPLC";
            this.txtAddressPLC.Size = new System.Drawing.Size(202, 44);
            this.txtAddressPLC.TabIndex = 47;
            this.txtAddressPLC.TextChanged += new System.EventHandler(this.AddressPLC_TextChanged);
            // 
            // btn8
            // 
            this.btn8.AutoFont = true;
            this.btn8.AutoFontHeightRatio = 0.75F;
            this.btn8.AutoFontMax = 100F;
            this.btn8.AutoFontMin = 14F;
            this.btn8.AutoFontWidthRatio = 0.92F;
            this.btn8.AutoImage = true;
            this.btn8.AutoImageMaxRatio = 0.75F;
            this.btn8.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btn8.AutoImageTint = true;
            this.btn8.BackColor = System.Drawing.Color.White;
            this.btn8.BackgroundColor = System.Drawing.Color.White;
            this.btn8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn8.BorderColor = System.Drawing.Color.White;
            this.btn8.BorderRadius = 10;
            this.btn8.BorderSize = 1;
            this.btn8.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn8.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn8.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn8.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btn8.Corner = BeeGlobal.Corner.None;
            this.btn8.DebounceResizeMs = 6;
            this.btn8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn8.FlatAppearance.BorderSize = 0;
            this.btn8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn8.ForeColor = System.Drawing.Color.White;
            this.btn8.Image = null;
            this.btn8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn8.ImageDisabled = null;
            this.btn8.ImageHover = null;
            this.btn8.ImageNormal = null;
            this.btn8.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btn8.ImagePressed = null;
            this.btn8.ImageTextSpacing = 6;
            this.btn8.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btn8.ImageTintHover = System.Drawing.Color.Empty;
            this.btn8.ImageTintNormal = System.Drawing.Color.Empty;
            this.btn8.ImageTintOpacity = 0.5F;
            this.btn8.ImageTintPressed = System.Drawing.Color.Empty;
            this.btn8.IsCLick = true;
            this.btn8.IsNotChange = false;
            this.btn8.IsRect = false;
            this.btn8.IsTouch = true;
            this.btn8.IsUnGroup = true;
            this.btn8.Location = new System.Drawing.Point(5, 746);
            this.btn8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btn8.Multiline = false;
            this.btn8.Name = "btn8";
            this.btn8.Size = new System.Drawing.Size(426, 35);
            this.btn8.TabIndex = 113;
            this.btn8.Text = "5.Compare With Data From PLC";
            this.btn8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn8.TextColor = System.Drawing.Color.White;
            this.btn8.UseVisualStyleBackColor = false;
            this.btn8.Click += new System.EventHandler(this.btn8_Click);
            // 
            // AdjLimitArea
            // 
            this.AdjLimitArea.AutoRepeatAccelDeltaMs = -5;
            this.AdjLimitArea.AutoRepeatAccelerate = true;
            this.AdjLimitArea.AutoRepeatEnabled = true;
            this.AdjLimitArea.AutoRepeatInitialDelay = 400;
            this.AdjLimitArea.AutoRepeatInterval = 60;
            this.AdjLimitArea.AutoRepeatMinInterval = 20;
            this.AdjLimitArea.AutoShowTextbox = true;
            this.AdjLimitArea.AutoSizeTextbox = true;
            this.AdjLimitArea.BackColor = System.Drawing.SystemColors.Control;
            this.AdjLimitArea.BarLeftGap = 20;
            this.AdjLimitArea.BarRightGap = 6;
            this.AdjLimitArea.ChromeGap = 8;
            this.AdjLimitArea.ChromeWidthRatio = 0.14F;
            this.AdjLimitArea.ColorBorder = System.Drawing.Color.DarkGray;
            this.AdjLimitArea.ColorFill = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(213)))), ((int)(((byte)(143)))));
            this.AdjLimitArea.ColorScale = System.Drawing.Color.DarkGray;
            this.AdjLimitArea.ColorThumb = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjLimitArea.ColorThumbBorder = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(201)))), ((int)(((byte)(110)))));
            this.AdjLimitArea.ColorTrack = System.Drawing.Color.DarkGray;
            this.AdjLimitArea.Decimals = 0;
            this.AdjLimitArea.DisabledDesaturateMix = 0.3F;
            this.AdjLimitArea.DisabledDimFactor = 0.9F;
            this.AdjLimitArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AdjLimitArea.EdgePadding = 2;
            this.AdjLimitArea.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.AdjLimitArea.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.AdjLimitArea.KeyboardStep = 1F;
            this.AdjLimitArea.Location = new System.Drawing.Point(3, 594);
            this.AdjLimitArea.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.AdjLimitArea.MatchTextboxFontToThumb = true;
            this.AdjLimitArea.Max = 100F;
            this.AdjLimitArea.MaxTextboxWidth = 0;
            this.AdjLimitArea.MaxThumb = 1000;
            this.AdjLimitArea.MaxTrackHeight = 1000;
            this.AdjLimitArea.Min = 0F;
            this.AdjLimitArea.MinChromeWidth = 64;
            this.AdjLimitArea.MinimumSize = new System.Drawing.Size(140, 36);
            this.AdjLimitArea.MinTextboxWidth = 32;
            this.AdjLimitArea.MinThumb = 30;
            this.AdjLimitArea.MinTrackHeight = 8;
            this.AdjLimitArea.Name = "AdjLimitArea";
            this.AdjLimitArea.Radius = 8;
            this.AdjLimitArea.ShowValueOnThumb = true;
            this.AdjLimitArea.Size = new System.Drawing.Size(430, 51);
            this.AdjLimitArea.SnapToStep = true;
            this.AdjLimitArea.StartWithTextboxHidden = true;
            this.AdjLimitArea.Step = 1F;
            this.AdjLimitArea.TabIndex = 112;
            this.AdjLimitArea.TextboxFontSize = 20F;
            this.AdjLimitArea.TextboxSidePadding = 10;
            this.AdjLimitArea.TextboxWidth = 600;
            this.AdjLimitArea.ThumbDiameterRatio = 2F;
            this.AdjLimitArea.ThumbValueBold = true;
            this.AdjLimitArea.ThumbValueFontScale = 1.5F;
            this.AdjLimitArea.ThumbValuePadding = 0;
            this.AdjLimitArea.TightEdges = true;
            this.AdjLimitArea.TrackHeightRatio = 0.45F;
            this.AdjLimitArea.TrackWidthRatio = 1F;
            this.AdjLimitArea.UnitText = "";
            this.AdjLimitArea.Value = 0F;
            this.AdjLimitArea.WheelStep = 1F;
            this.AdjLimitArea.ValueChanged += new System.Action<float>(this.AdjLimitArea_ValueChanged);
            // 
            // btn6
            // 
            this.btn6.AutoFont = true;
            this.btn6.AutoFontHeightRatio = 0.75F;
            this.btn6.AutoFontMax = 100F;
            this.btn6.AutoFontMin = 14F;
            this.btn6.AutoFontWidthRatio = 0.92F;
            this.btn6.AutoImage = true;
            this.btn6.AutoImageMaxRatio = 0.75F;
            this.btn6.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btn6.AutoImageTint = true;
            this.btn6.BackColor = System.Drawing.Color.White;
            this.btn6.BackgroundColor = System.Drawing.Color.White;
            this.btn6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn6.BorderColor = System.Drawing.Color.White;
            this.btn6.BorderRadius = 10;
            this.btn6.BorderSize = 1;
            this.btn6.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn6.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn6.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn6.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btn6.Corner = BeeGlobal.Corner.None;
            this.btn6.DebounceResizeMs = 6;
            this.btn6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn6.FlatAppearance.BorderSize = 0;
            this.btn6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn6.ForeColor = System.Drawing.Color.White;
            this.btn6.Image = null;
            this.btn6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn6.ImageDisabled = null;
            this.btn6.ImageHover = null;
            this.btn6.ImageNormal = null;
            this.btn6.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btn6.ImagePressed = null;
            this.btn6.ImageTextSpacing = 6;
            this.btn6.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btn6.ImageTintHover = System.Drawing.Color.Empty;
            this.btn6.ImageTintNormal = System.Drawing.Color.Empty;
            this.btn6.ImageTintOpacity = 0.5F;
            this.btn6.ImageTintPressed = System.Drawing.Color.Empty;
            this.btn6.IsCLick = true;
            this.btn6.IsNotChange = false;
            this.btn6.IsRect = false;
            this.btn6.IsTouch = true;
            this.btn6.IsUnGroup = true;
            this.btn6.Location = new System.Drawing.Point(5, 556);
            this.btn6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btn6.Multiline = false;
            this.btn6.Name = "btn6";
            this.btn6.Size = new System.Drawing.Size(426, 35);
            this.btn6.TabIndex = 111;
            this.btn6.Text = "3.Limit Area (pixel)";
            this.btn6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn6.TextColor = System.Drawing.Color.White;
            this.btn6.UseVisualStyleBackColor = false;
            this.btn6.Click += new System.EventHandler(this.btn6_Click);
            // 
            // btn5
            // 
            this.btn5.AutoFont = true;
            this.btn5.AutoFontHeightRatio = 0.75F;
            this.btn5.AutoFontMax = 100F;
            this.btn5.AutoFontMin = 14F;
            this.btn5.AutoFontWidthRatio = 0.92F;
            this.btn5.AutoImage = true;
            this.btn5.AutoImageMaxRatio = 0.75F;
            this.btn5.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btn5.AutoImageTint = true;
            this.btn5.BackColor = System.Drawing.Color.White;
            this.btn5.BackgroundColor = System.Drawing.Color.White;
            this.btn5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn5.BorderColor = System.Drawing.Color.White;
            this.btn5.BorderRadius = 10;
            this.btn5.BorderSize = 1;
            this.btn5.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn5.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn5.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn5.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btn5.Corner = BeeGlobal.Corner.None;
            this.btn5.DebounceResizeMs = 6;
            this.btn5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn5.FlatAppearance.BorderSize = 0;
            this.btn5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn5.ForeColor = System.Drawing.Color.White;
            this.btn5.Image = null;
            this.btn5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn5.ImageDisabled = null;
            this.btn5.ImageHover = null;
            this.btn5.ImageNormal = null;
            this.btn5.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btn5.ImagePressed = null;
            this.btn5.ImageTextSpacing = 6;
            this.btn5.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btn5.ImageTintHover = System.Drawing.Color.Empty;
            this.btn5.ImageTintNormal = System.Drawing.Color.Empty;
            this.btn5.ImageTintOpacity = 0.5F;
            this.btn5.ImageTintPressed = System.Drawing.Color.Empty;
            this.btn5.IsCLick = false;
            this.btn5.IsNotChange = false;
            this.btn5.IsRect = false;
            this.btn5.IsTouch = true;
            this.btn5.IsUnGroup = true;
            this.btn5.Location = new System.Drawing.Point(5, 867);
            this.btn5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btn5.Multiline = false;
            this.btn5.Name = "btn5";
            this.btn5.Size = new System.Drawing.Size(426, 35);
            this.btn5.TabIndex = 110;
            this.btn5.Text = "6.Score";
            this.btn5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn5.TextColor = System.Drawing.Color.White;
            this.btn5.UseVisualStyleBackColor = false;
            // 
            // lay4
            // 
            this.lay4.ColumnCount = 2;
            this.lay4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.lay4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.lay4.Controls.Add(this.btnSet, 1, 0);
            this.lay4.Controls.Add(this.txtContent, 0, 0);
            this.lay4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lay4.Location = new System.Drawing.Point(3, 693);
            this.lay4.Name = "lay4";
            this.lay4.RowCount = 1;
            this.lay4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.lay4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.lay4.Size = new System.Drawing.Size(430, 50);
            this.lay4.TabIndex = 108;
            // 
            // btnSet
            // 
            this.btnSet.AutoFont = true;
            this.btnSet.AutoFontHeightRatio = 0.75F;
            this.btnSet.AutoFontMax = 100F;
            this.btnSet.AutoFontMin = 6F;
            this.btnSet.AutoFontWidthRatio = 0.92F;
            this.btnSet.AutoImage = true;
            this.btnSet.AutoImageMaxRatio = 0.75F;
            this.btnSet.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSet.AutoImageTint = true;
            this.btnSet.BackColor = System.Drawing.SystemColors.Control;
            this.btnSet.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnSet.BorderColor = System.Drawing.Color.Silver;
            this.btnSet.BorderRadius = 10;
            this.btnSet.BorderSize = 1;
            this.btnSet.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnSet.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnSet.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnSet.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSet.Corner = BeeGlobal.Corner.Right;
            this.btnSet.DebounceResizeMs = 16;
            this.btnSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSet.FlatAppearance.BorderSize = 0;
            this.btnSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.203125F);
            this.btnSet.ForeColor = System.Drawing.Color.Black;
            this.btnSet.Image = null;
            this.btnSet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSet.ImageDisabled = null;
            this.btnSet.ImageHover = null;
            this.btnSet.ImageNormal = null;
            this.btnSet.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSet.ImagePressed = null;
            this.btnSet.ImageTextSpacing = 6;
            this.btnSet.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSet.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSet.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSet.ImageTintOpacity = 0.5F;
            this.btnSet.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSet.IsCLick = false;
            this.btnSet.IsNotChange = true;
            this.btnSet.IsRect = false;
            this.btnSet.IsTouch = false;
            this.btnSet.IsUnGroup = false;
            this.btnSet.Location = new System.Drawing.Point(330, 0);
            this.btnSet.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnSet.Multiline = false;
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(97, 50);
            this.btnSet.TabIndex = 48;
            this.btnSet.Text = "Set Maching";
            this.btnSet.TextColor = System.Drawing.Color.Black;
            this.btnSet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSet.UseVisualStyleBackColor = false;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // txtContent
            // 
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContent.Location = new System.Drawing.Point(10, 3);
            this.txtContent.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(317, 44);
            this.txtContent.TabIndex = 47;
            // 
            // btn4
            // 
            this.btn4.AutoFont = true;
            this.btn4.AutoFontHeightRatio = 0.75F;
            this.btn4.AutoFontMax = 100F;
            this.btn4.AutoFontMin = 14F;
            this.btn4.AutoFontWidthRatio = 0.92F;
            this.btn4.AutoImage = true;
            this.btn4.AutoImageMaxRatio = 0.75F;
            this.btn4.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btn4.AutoImageTint = true;
            this.btn4.BackColor = System.Drawing.Color.White;
            this.btn4.BackgroundColor = System.Drawing.Color.White;
            this.btn4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn4.BorderColor = System.Drawing.Color.White;
            this.btn4.BorderRadius = 10;
            this.btn4.BorderSize = 1;
            this.btn4.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn4.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn4.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn4.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btn4.Corner = BeeGlobal.Corner.None;
            this.btn4.DebounceResizeMs = 6;
            this.btn4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn4.FlatAppearance.BorderSize = 0;
            this.btn4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn4.ForeColor = System.Drawing.Color.White;
            this.btn4.Image = null;
            this.btn4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn4.ImageDisabled = null;
            this.btn4.ImageHover = null;
            this.btn4.ImageNormal = null;
            this.btn4.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btn4.ImagePressed = null;
            this.btn4.ImageTextSpacing = 6;
            this.btn4.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btn4.ImageTintHover = System.Drawing.Color.Empty;
            this.btn4.ImageTintNormal = System.Drawing.Color.Empty;
            this.btn4.ImageTintOpacity = 0.5F;
            this.btn4.ImageTintPressed = System.Drawing.Color.Empty;
            this.btn4.IsCLick = false;
            this.btn4.IsNotChange = false;
            this.btn4.IsRect = false;
            this.btn4.IsTouch = true;
            this.btn4.IsUnGroup = true;
            this.btn4.Location = new System.Drawing.Point(5, 655);
            this.btn4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btn4.Multiline = false;
            this.btn4.Name = "btn4";
            this.btn4.Size = new System.Drawing.Size(426, 35);
            this.btn4.TabIndex = 107;
            this.btn4.Text = "4.Compare With Data Fixed";
            this.btn4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn4.TextColor = System.Drawing.Color.White;
            this.btn4.UseVisualStyleBackColor = false;
            this.btn4.Click += new System.EventHandler(this.btn4_Click);
            // 
            // btn2
            // 
            this.btn2.AutoFont = true;
            this.btn2.AutoFontHeightRatio = 0.75F;
            this.btn2.AutoFontMax = 100F;
            this.btn2.AutoFontMin = 14F;
            this.btn2.AutoFontWidthRatio = 0.92F;
            this.btn2.AutoImage = true;
            this.btn2.AutoImageMaxRatio = 0.75F;
            this.btn2.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btn2.AutoImageTint = true;
            this.btn2.BackColor = System.Drawing.Color.White;
            this.btn2.BackgroundColor = System.Drawing.Color.White;
            this.btn2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn2.BorderColor = System.Drawing.Color.White;
            this.btn2.BorderRadius = 10;
            this.btn2.BorderSize = 1;
            this.btn2.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn2.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn2.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn2.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btn2.Corner = BeeGlobal.Corner.None;
            this.btn2.DebounceResizeMs = 6;
            this.btn2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn2.FlatAppearance.BorderSize = 0;
            this.btn2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn2.ForeColor = System.Drawing.Color.White;
            this.btn2.Image = null;
            this.btn2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn2.ImageDisabled = null;
            this.btn2.ImageHover = null;
            this.btn2.ImageNormal = null;
            this.btn2.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btn2.ImagePressed = null;
            this.btn2.ImageTextSpacing = 6;
            this.btn2.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btn2.ImageTintHover = System.Drawing.Color.Empty;
            this.btn2.ImageTintNormal = System.Drawing.Color.Empty;
            this.btn2.ImageTintOpacity = 0.5F;
            this.btn2.ImageTintPressed = System.Drawing.Color.Empty;
            this.btn2.IsCLick = true;
            this.btn2.IsNotChange = false;
            this.btn2.IsRect = false;
            this.btn2.IsTouch = true;
            this.btn2.IsUnGroup = true;
            this.btn2.Location = new System.Drawing.Point(5, 10);
            this.btn2.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.btn2.Multiline = false;
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(426, 35);
            this.btn2.TabIndex = 101;
            this.btn2.Text = "1.Choose Area";
            this.btn2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn2.TextColor = System.Drawing.Color.White;
            this.btn2.UseVisualStyleBackColor = false;
            this.btn2.Click += new System.EventHandler(this.btn2_Click);
            // 
            // btn3
            // 
            this.btn3.AutoFont = true;
            this.btn3.AutoFontHeightRatio = 0.75F;
            this.btn3.AutoFontMax = 100F;
            this.btn3.AutoFontMin = 14F;
            this.btn3.AutoFontWidthRatio = 0.92F;
            this.btn3.AutoImage = true;
            this.btn3.AutoImageMaxRatio = 0.75F;
            this.btn3.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btn3.AutoImageTint = true;
            this.btn3.BackColor = System.Drawing.Color.White;
            this.btn3.BackgroundColor = System.Drawing.Color.White;
            this.btn3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn3.BorderColor = System.Drawing.Color.White;
            this.btn3.BorderRadius = 10;
            this.btn3.BorderSize = 1;
            this.btn3.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn3.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn3.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn3.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btn3.Corner = BeeGlobal.Corner.None;
            this.btn3.DebounceResizeMs = 6;
            this.btn3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn3.FlatAppearance.BorderSize = 0;
            this.btn3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn3.ForeColor = System.Drawing.Color.White;
            this.btn3.Image = null;
            this.btn3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn3.ImageDisabled = null;
            this.btn3.ImageHover = null;
            this.btn3.ImageNormal = null;
            this.btn3.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btn3.ImagePressed = null;
            this.btn3.ImageTextSpacing = 6;
            this.btn3.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btn3.ImageTintHover = System.Drawing.Color.Empty;
            this.btn3.ImageTintNormal = System.Drawing.Color.Empty;
            this.btn3.ImageTintOpacity = 0.5F;
            this.btn3.ImageTintPressed = System.Drawing.Color.Empty;
            this.btn3.IsCLick = true;
            this.btn3.IsNotChange = false;
            this.btn3.IsRect = false;
            this.btn3.IsTouch = true;
            this.btn3.IsUnGroup = true;
            this.btn3.Location = new System.Drawing.Point(5, 381);
            this.btn3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btn3.Multiline = false;
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(426, 35);
            this.btn3.TabIndex = 100;
            this.btn3.Text = "2.Chars Allow";
            this.btn3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn3.TextColor = System.Drawing.Color.White;
            this.btn3.UseVisualStyleBackColor = false;
            this.btn3.Click += new System.EventHandler(this.btn3_Click);
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
            this.trackScore.DisabledDesaturateMix = 0.3F;
            this.trackScore.DisabledDimFactor = 0.9F;
            this.trackScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackScore.EdgePadding = 2;
            this.trackScore.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.trackScore.InnerPadding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.trackScore.KeyboardStep = 1F;
            this.trackScore.Location = new System.Drawing.Point(3, 905);
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
            this.trackScore.Size = new System.Drawing.Size(430, 51);
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
            // lay3
            // 
            this.lay3.BackColor = System.Drawing.SystemColors.Control;
            this.lay3.ColumnCount = 2;
            this.lay3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.lay3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.lay3.Controls.Add(this.btnApply, 1, 0);
            this.lay3.Controls.Add(this.txtAllow, 0, 0);
            this.lay3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lay3.Location = new System.Drawing.Point(3, 419);
            this.lay3.Name = "lay3";
            this.lay3.RowCount = 1;
            this.lay3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.lay3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 134F));
            this.lay3.Size = new System.Drawing.Size(430, 134);
            this.lay3.TabIndex = 53;
            this.lay3.Visible = false;
            // 
            // btnApply
            // 
            this.btnApply.AutoFont = true;
            this.btnApply.AutoFontHeightRatio = 0.75F;
            this.btnApply.AutoFontMax = 100F;
            this.btnApply.AutoFontMin = 6F;
            this.btnApply.AutoFontWidthRatio = 0.92F;
            this.btnApply.AutoImage = true;
            this.btnApply.AutoImageMaxRatio = 0.75F;
            this.btnApply.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnApply.AutoImageTint = true;
            this.btnApply.BackColor = System.Drawing.SystemColors.Control;
            this.btnApply.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnApply.BorderColor = System.Drawing.Color.Silver;
            this.btnApply.BorderRadius = 10;
            this.btnApply.BorderSize = 1;
            this.btnApply.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnApply.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnApply.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnApply.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnApply.Corner = BeeGlobal.Corner.Right;
            this.btnApply.DebounceResizeMs = 16;
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnApply.FlatAppearance.BorderSize = 0;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnApply.ForeColor = System.Drawing.Color.Black;
            this.btnApply.Image = null;
            this.btnApply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApply.ImageDisabled = null;
            this.btnApply.ImageHover = null;
            this.btnApply.ImageNormal = null;
            this.btnApply.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnApply.ImagePressed = null;
            this.btnApply.ImageTextSpacing = 6;
            this.btnApply.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnApply.ImageTintHover = System.Drawing.Color.Empty;
            this.btnApply.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnApply.ImageTintOpacity = 0.5F;
            this.btnApply.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnApply.IsCLick = false;
            this.btnApply.IsNotChange = true;
            this.btnApply.IsRect = false;
            this.btnApply.IsTouch = false;
            this.btnApply.IsUnGroup = false;
            this.btnApply.Location = new System.Drawing.Point(340, 10);
            this.btnApply.Margin = new System.Windows.Forms.Padding(10);
            this.btnApply.Multiline = false;
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(80, 114);
            this.btnApply.TabIndex = 48;
            this.btnApply.Text = "Apply";
            this.btnApply.TextColor = System.Drawing.Color.Black;
            this.btnApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // txtAllow
            // 
            this.txtAllow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAllow.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAllow.Location = new System.Drawing.Point(10, 3);
            this.txtAllow.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.txtAllow.Multiline = true;
            this.txtAllow.Name = "txtAllow";
            this.txtAllow.Size = new System.Drawing.Size(317, 128);
            this.txtAllow.TabIndex = 47;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.tableLayoutPanel8);
            this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(442, 1128);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Extension";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel8.ColumnCount = 1;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel10, 0, 3);
            this.tableLayoutPanel8.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel4, 0, 2);
            this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel9, 0, 1);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 9;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(436, 1122);
            this.tableLayoutPanel8.TabIndex = 0;
            this.tableLayoutPanel8.Visible = false;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel10.ColumnCount = 2;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel10.Controls.Add(this.numBlur, 1, 0);
            this.tableLayoutPanel10.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(10, 145);
            this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 1;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(421, 50);
            this.tableLayoutPanel10.TabIndex = 49;
            // 
            // numBlur
            // 
            this.numBlur.AutoShowTextbox = false;
            this.numBlur.AutoSizeTextbox = true;
            this.numBlur.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numBlur.BackColor = System.Drawing.SystemColors.Control;
            this.numBlur.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.numBlur.BorderRadius = 6;
            this.numBlur.ButtonMaxSize = 64;
            this.numBlur.ButtonMinSize = 24;
            this.numBlur.Decimals = 0;
            this.numBlur.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numBlur.ElementGap = 6;
            this.numBlur.FillTextboxToAvailable = true;
            this.numBlur.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numBlur.InnerPadding = new System.Windows.Forms.Padding(6);
            this.numBlur.KeyboardStep = 1F;
            this.numBlur.Location = new System.Drawing.Point(210, 0);
            this.numBlur.Margin = new System.Windows.Forms.Padding(0);
            this.numBlur.Max = 100F;
            this.numBlur.MaxTextboxWidth = 0;
            this.numBlur.Min = 0F;
            this.numBlur.MinimumSize = new System.Drawing.Size(120, 32);
            this.numBlur.MinTextboxWidth = 16;
            this.numBlur.Name = "numBlur";
            this.numBlur.Size = new System.Drawing.Size(211, 50);
            this.numBlur.SnapToStep = true;
            this.numBlur.StartWithTextboxHidden = false;
            this.numBlur.Step = 1F;
            this.numBlur.TabIndex = 45;
            this.numBlur.TextboxFontSize = 24F;
            this.numBlur.TextboxSidePadding = 12;
            this.numBlur.TextboxWidth = 56;
            this.numBlur.UnitText = "";
            this.numBlur.Value = 3F;
            this.numBlur.WheelStep = 1F;
            this.numBlur.ValueChanged += new System.Action<float>(this.numBlur_ValueChanged);
            this.numBlur.Load += new System.EventHandler(this.numBlur_Load);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(210, 50);
            this.label7.TabIndex = 44;
            this.label7.Text = "Noise (0 for Unuser)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label7.Click += new System.EventHandler(this.label7_Click_1);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(436, 38);
            this.label6.TabIndex = 48;
            this.label6.Text = "Bộ Lọc Ảnh";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.numUnsharp, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(10, 97);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(421, 38);
            this.tableLayoutPanel4.TabIndex = 47;
            // 
            // numUnsharp
            // 
            this.numUnsharp.AutoShowTextbox = false;
            this.numUnsharp.AutoSizeTextbox = true;
            this.numUnsharp.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numUnsharp.BackColor = System.Drawing.SystemColors.Control;
            this.numUnsharp.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.numUnsharp.BorderRadius = 6;
            this.numUnsharp.ButtonMaxSize = 64;
            this.numUnsharp.ButtonMinSize = 24;
            this.numUnsharp.Decimals = 0;
            this.numUnsharp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numUnsharp.ElementGap = 6;
            this.numUnsharp.FillTextboxToAvailable = true;
            this.numUnsharp.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numUnsharp.InnerPadding = new System.Windows.Forms.Padding(6);
            this.numUnsharp.KeyboardStep = 1F;
            this.numUnsharp.Location = new System.Drawing.Point(210, 0);
            this.numUnsharp.Margin = new System.Windows.Forms.Padding(0);
            this.numUnsharp.Max = 100F;
            this.numUnsharp.MaxTextboxWidth = 0;
            this.numUnsharp.Min = 0F;
            this.numUnsharp.MinimumSize = new System.Drawing.Size(120, 32);
            this.numUnsharp.MinTextboxWidth = 16;
            this.numUnsharp.Name = "numUnsharp";
            this.numUnsharp.Size = new System.Drawing.Size(211, 38);
            this.numUnsharp.SnapToStep = true;
            this.numUnsharp.StartWithTextboxHidden = false;
            this.numUnsharp.Step = 1F;
            this.numUnsharp.TabIndex = 45;
            this.numUnsharp.TextboxFontSize = 24F;
            this.numUnsharp.TextboxSidePadding = 12;
            this.numUnsharp.TextboxWidth = 56;
            this.numUnsharp.UnitText = "";
            this.numUnsharp.Value = 3F;
            this.numUnsharp.WheelStep = 1F;
            this.numUnsharp.ValueChanged += new System.Action<float>(this.numUnsharp_ValueChanged);
            this.numUnsharp.Load += new System.EventHandler(this.numUnsharp_Load);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(210, 38);
            this.label2.TabIndex = 44;
            this.label2.Text = "Sharp (0 for Unuser)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel9.Controls.Add(this.numCLAHE, 1, 0);
            this.tableLayoutPanel9.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(10, 41);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(423, 48);
            this.tableLayoutPanel9.TabIndex = 45;
            // 
            // numCLAHE
            // 
            this.numCLAHE.AutoShowTextbox = false;
            this.numCLAHE.AutoSizeTextbox = true;
            this.numCLAHE.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.numCLAHE.BackColor = System.Drawing.SystemColors.Control;
            this.numCLAHE.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.numCLAHE.BorderRadius = 6;
            this.numCLAHE.ButtonMaxSize = 64;
            this.numCLAHE.ButtonMinSize = 24;
            this.numCLAHE.Decimals = 0;
            this.numCLAHE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numCLAHE.ElementGap = 6;
            this.numCLAHE.FillTextboxToAvailable = true;
            this.numCLAHE.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numCLAHE.InnerPadding = new System.Windows.Forms.Padding(6);
            this.numCLAHE.KeyboardStep = 1F;
            this.numCLAHE.Location = new System.Drawing.Point(211, 0);
            this.numCLAHE.Margin = new System.Windows.Forms.Padding(0);
            this.numCLAHE.Max = 100F;
            this.numCLAHE.MaxTextboxWidth = 0;
            this.numCLAHE.Min = 0F;
            this.numCLAHE.MinimumSize = new System.Drawing.Size(120, 32);
            this.numCLAHE.MinTextboxWidth = 16;
            this.numCLAHE.Name = "numCLAHE";
            this.numCLAHE.Size = new System.Drawing.Size(212, 48);
            this.numCLAHE.SnapToStep = true;
            this.numCLAHE.StartWithTextboxHidden = false;
            this.numCLAHE.Step = 1F;
            this.numCLAHE.TabIndex = 45;
            this.numCLAHE.TextboxFontSize = 24F;
            this.numCLAHE.TextboxSidePadding = 12;
            this.numCLAHE.TextboxWidth = 56;
            this.numCLAHE.UnitText = "";
            this.numCLAHE.Value = 2F;
            this.numCLAHE.WheelStep = 1F;
            this.numCLAHE.ValueChanged += new System.Action<float>(this.numCLAHE_ValueChanged);
            this.numCLAHE.Load += new System.EventHandler(this.numCLAHE_Load);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(211, 48);
            this.label1.TabIndex = 44;
            this.label1.Text = "Contrast (0 for Unuser)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tmCheckFist
            // 
            this.tmCheckFist.Tick += new System.EventHandler(this.tmCheckFist_Tick);
            // 
            // workLoadModel
            // 
            this.workLoadModel.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workLoadModel_DoWork);
            this.workLoadModel.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workLoadModel_RunWorkerCompleted);
            // 
            // pInspect
            // 
            this.pInspect.Controls.Add(this.btnTest);
            this.pInspect.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pInspect.Location = new System.Drawing.Point(0, 1009);
            this.pInspect.Name = "pInspect";
            this.pInspect.Padding = new System.Windows.Forms.Padding(10);
            this.pInspect.Size = new System.Drawing.Size(450, 100);
            this.pInspect.TabIndex = 85;
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
            this.btnTest.Size = new System.Drawing.Size(430, 80);
            this.btnTest.TabIndex = 81;
            this.btnTest.Text = "Inspect";
            this.btnTest.TextColor = System.Drawing.Color.Black;
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click_1);
            // 
            // oK_Cancel1
            // 
            this.oK_Cancel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.oK_Cancel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.oK_Cancel1.Location = new System.Drawing.Point(0, 1109);
            this.oK_Cancel1.Name = "oK_Cancel1";
            this.oK_Cancel1.Size = new System.Drawing.Size(450, 57);
            this.oK_Cancel1.TabIndex = 20;
            // 
            // ToolOCR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pInspect);
            this.Controls.Add(this.oK_Cancel1);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Name = "ToolOCR";
            this.Size = new System.Drawing.Size(450, 1166);
            this.tabControl1.ResumeLayout(false);
            this.tabP1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.lay8.ResumeLayout(false);
            this.lay8.PerformLayout();
            this.lay4.ResumeLayout(false);
            this.lay4.PerformLayout();
            this.lay3.ResumeLayout(false);
            this.lay3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.pInspect.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabP1;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabPage tabPage4;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
        private System.Windows.Forms.Timer tmCheckFist;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.ComponentModel.BackgroundWorker workLoadModel;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private CustomNumericEx numUnsharp;
        private System.Windows.Forms.Label label2;
        private CustomNumericEx numCLAHE;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private CustomNumericEx numBlur;
        private System.Windows.Forms.Label label7;
        private  System.Windows.Forms.TableLayoutPanel lay3;
        private RJButton btnApply;
        private System.Windows.Forms.TextBox txtAllow;
        private AdjustBarEx trackScore;
        private GroupControl.OK_Cancel oK_Cancel1;
        private Panel pInspect;
        private RJButton btnTest;
        private RJButton btn3;
        private RJButton btn2;
        private TableLayoutPanel lay4;
        private RJButton btnSet;
        private TextBox txtContent;
        private RJButton btn4;
        private RJButton btn6;
        private AdjustBarEx AdjLimitArea;
        private RJButton btn5;
        private RJButton btn8;
        private TableLayoutPanel lay8;
        private TextBox txtAddressPLC;
        private RJButton btnValuePLC;
        private Timer tmEnble;
        private Label label4;
        private Label label3;
        private Group.EditRectRot EditRectRot1;
    }
}
