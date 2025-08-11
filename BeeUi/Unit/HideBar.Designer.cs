using BeeGlobal;
using BeeInterface;

namespace BeeUi.Unit
{
    partial class HideBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HideBar));
            this.tableLayoutPanel1 = new  System.Windows.Forms.TableLayoutPanel();
            this.btnExit = new BeeInterface.RJButton();
            this.btnShuttdown = new BeeInterface.RJButton();
            this.rjButton1 = new BeeInterface.RJButton();
            this.btnShowToolBar = new BeeInterface.RJButton();
            this.btnMenu = new BeeInterface.RJButton();
            this.btnShowDashBoard = new BeeInterface.RJButton();
            this.btnShowTop = new BeeInterface.RJButton();
            this.btnfull = new BeeInterface.RJButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.Controls.Add(this.btnExit, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnShuttdown, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.rjButton1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnShowToolBar, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnMenu, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnShowDashBoard, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnShowTop, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnfull, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(577, 46);
            this.tableLayoutPanel1.TabIndex = 36;
            // 
            // btnExit
            // 
            this.btnExit.AutoFont = true;
            this.btnExit.AutoFontHeightRatio = 0.85F;
            this.btnExit.AutoFontMax = 100F;
            this.btnExit.AutoFontMin = 7F;
            this.btnExit.AutoFontWidthRatio = 1F;
            this.btnExit.AutoImage = true;
            this.btnExit.AutoImageMaxRatio = 1F;
            this.btnExit.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnExit.AutoImageTint = true;
            this.btnExit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnExit.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnExit.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnExit.BorderRadius = 5;
            this.btnExit.BorderSize = 1;
            this.btnExit.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnExit.Corner = BeeGlobal.Corner.Both;
            this.btnExit.DebounceResizeMs = 16;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Image = global::BeeUi.Properties.Resources.exit;
            this.btnExit.ImageDisabled = null;
            this.btnExit.ImageHover = null;
            this.btnExit.ImageNormal = null;
            this.btnExit.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnExit.ImagePressed = null;
            this.btnExit.ImageTextSpacing = 2;
            this.btnExit.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnExit.ImageTintHover = System.Drawing.Color.Empty;
            this.btnExit.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnExit.ImageTintOpacity = 0.5F;
            this.btnExit.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnExit.IsCLick = false;
            this.btnExit.IsNotChange = true;
            this.btnExit.IsRect = false;
            this.btnExit.IsUnGroup = true;
            this.btnExit.Location = new System.Drawing.Point(507, 2);
            this.btnExit.Margin = new System.Windows.Forms.Padding(2);
            this.btnExit.Multiline = false;
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(68, 42);
            this.btnExit.TabIndex = 48;
            this.btnExit.Text = "Exit";
            this.btnExit.TextColor = System.Drawing.Color.Black;
            this.btnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnShuttdown
            // 
            this.btnShuttdown.AutoFont = true;
            this.btnShuttdown.AutoFontHeightRatio = 0.85F;
            this.btnShuttdown.AutoFontMax = 100F;
            this.btnShuttdown.AutoFontMin = 7F;
            this.btnShuttdown.AutoFontWidthRatio = 1F;
            this.btnShuttdown.AutoImage = true;
            this.btnShuttdown.AutoImageMaxRatio = 1F;
            this.btnShuttdown.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnShuttdown.AutoImageTint = true;
            this.btnShuttdown.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnShuttdown.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnShuttdown.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnShuttdown.BackgroundImage")));
            this.btnShuttdown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnShuttdown.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnShuttdown.BorderRadius = 5;
            this.btnShuttdown.BorderSize = 1;
            this.btnShuttdown.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnShuttdown.Corner = BeeGlobal.Corner.Both;
            this.btnShuttdown.DebounceResizeMs = 16;
            this.btnShuttdown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShuttdown.FlatAppearance.BorderSize = 0;
            this.btnShuttdown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShuttdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.btnShuttdown.ForeColor = System.Drawing.Color.Black;
            this.btnShuttdown.Image = global::BeeUi.Properties.Resources.Shutdown;
            this.btnShuttdown.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnShuttdown.ImageDisabled = null;
            this.btnShuttdown.ImageHover = null;
            this.btnShuttdown.ImageNormal = null;
            this.btnShuttdown.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnShuttdown.ImagePressed = null;
            this.btnShuttdown.ImageTextSpacing = 2;
            this.btnShuttdown.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnShuttdown.ImageTintHover = System.Drawing.Color.Empty;
            this.btnShuttdown.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnShuttdown.ImageTintOpacity = 0.5F;
            this.btnShuttdown.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnShuttdown.IsCLick = false;
            this.btnShuttdown.IsNotChange = true;
            this.btnShuttdown.IsRect = true;
            this.btnShuttdown.IsUnGroup = true;
            this.btnShuttdown.Location = new System.Drawing.Point(405, 2);
            this.btnShuttdown.Margin = new System.Windows.Forms.Padding(10, 2, 2, 2);
            this.btnShuttdown.Multiline = false;
            this.btnShuttdown.Name = "btnShuttdown";
            this.btnShuttdown.Size = new System.Drawing.Size(98, 42);
            this.btnShuttdown.TabIndex = 47;
            this.btnShuttdown.Text = "Shuttdown";
            this.btnShuttdown.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShuttdown.TextColor = System.Drawing.Color.Black;
            this.btnShuttdown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnShuttdown.UseVisualStyleBackColor = false;
            this.btnShuttdown.Click += new System.EventHandler(this.btnShuttdown_Click);
            // 
            // rjButton1
            // 
            this.rjButton1.AutoFont = true;
            this.rjButton1.AutoFontHeightRatio = 1F;
            this.rjButton1.AutoFontMax = 100F;
            this.rjButton1.AutoFontMin = 8F;
            this.rjButton1.AutoFontWidthRatio = 0.92F;
            this.rjButton1.AutoImage = true;
            this.rjButton1.AutoImageMaxRatio = 1F;
            this.rjButton1.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton1.AutoImageTint = true;
            this.rjButton1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton1.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rjButton1.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton1.BorderRadius = 5;
            this.rjButton1.BorderSize = 1;
            this.rjButton1.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton1.Corner = BeeGlobal.Corner.Both;
            this.rjButton1.DebounceResizeMs = 16;
            this.rjButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton1.Enabled = false;
            this.rjButton1.FlatAppearance.BorderSize = 0;
            this.rjButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.rjButton1.ForeColor = System.Drawing.Color.Black;
            this.rjButton1.Image = global::BeeUi.Properties.Resources.Show;
            this.rjButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton1.ImageDisabled = null;
            this.rjButton1.ImageHover = null;
            this.rjButton1.ImageNormal = null;
            this.rjButton1.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton1.ImagePressed = null;
            this.rjButton1.ImageTextSpacing = 0;
            this.rjButton1.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton1.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton1.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton1.ImageTintOpacity = 0.5F;
            this.rjButton1.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton1.IsCLick = false;
            this.rjButton1.IsNotChange = false;
            this.rjButton1.IsRect = false;
            this.rjButton1.IsUnGroup = true;
            this.rjButton1.Location = new System.Drawing.Point(2, 2);
            this.rjButton1.Margin = new System.Windows.Forms.Padding(2);
            this.rjButton1.Multiline = false;
            this.rjButton1.Name = "rjButton1";
            this.rjButton1.Size = new System.Drawing.Size(76, 42);
            this.rjButton1.TabIndex = 46;
            this.rjButton1.Text = "HideBar";
            this.rjButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton1.TextColor = System.Drawing.Color.Black;
            this.rjButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton1.UseVisualStyleBackColor = false;
            // 
            // btnShowToolBar
            // 
            this.btnShowToolBar.AutoFont = true;
            this.btnShowToolBar.AutoFontHeightRatio = 1F;
            this.btnShowToolBar.AutoFontMax = 100F;
            this.btnShowToolBar.AutoFontMin = 8F;
            this.btnShowToolBar.AutoFontWidthRatio = 0.92F;
            this.btnShowToolBar.AutoImage = true;
            this.btnShowToolBar.AutoImageMaxRatio = 0.75F;
            this.btnShowToolBar.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnShowToolBar.AutoImageTint = true;
            this.btnShowToolBar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnShowToolBar.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnShowToolBar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnShowToolBar.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnShowToolBar.BorderRadius = 5;
            this.btnShowToolBar.BorderSize = 1;
            this.btnShowToolBar.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnShowToolBar.Corner = BeeGlobal.Corner.Both;
            this.btnShowToolBar.DebounceResizeMs = 16;
            this.btnShowToolBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowToolBar.FlatAppearance.BorderSize = 0;
            this.btnShowToolBar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowToolBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnShowToolBar.ForeColor = System.Drawing.Color.Black;
            this.btnShowToolBar.Image = null;
            this.btnShowToolBar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShowToolBar.ImageDisabled = null;
            this.btnShowToolBar.ImageHover = null;
            this.btnShowToolBar.ImageNormal = null;
            this.btnShowToolBar.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnShowToolBar.ImagePressed = null;
            this.btnShowToolBar.ImageTextSpacing = 6;
            this.btnShowToolBar.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnShowToolBar.ImageTintHover = System.Drawing.Color.Empty;
            this.btnShowToolBar.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnShowToolBar.ImageTintOpacity = 0.5F;
            this.btnShowToolBar.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnShowToolBar.IsCLick = false;
            this.btnShowToolBar.IsNotChange = false;
            this.btnShowToolBar.IsRect = false;
            this.btnShowToolBar.IsUnGroup = true;
            this.btnShowToolBar.Location = new System.Drawing.Point(334, 2);
            this.btnShowToolBar.Margin = new System.Windows.Forms.Padding(2);
            this.btnShowToolBar.Multiline = false;
            this.btnShowToolBar.Name = "btnShowToolBar";
            this.btnShowToolBar.Size = new System.Drawing.Size(59, 42);
            this.btnShowToolBar.TabIndex = 44;
            this.btnShowToolBar.Text = "Tool Bar";
            this.btnShowToolBar.TextColor = System.Drawing.Color.Black;
            this.btnShowToolBar.UseVisualStyleBackColor = false;
            this.btnShowToolBar.Click += new System.EventHandler(this.btnShowToolBar_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.AutoFont = true;
            this.btnMenu.AutoFontHeightRatio = 1F;
            this.btnMenu.AutoFontMax = 100F;
            this.btnMenu.AutoFontMin = 8F;
            this.btnMenu.AutoFontWidthRatio = 0.92F;
            this.btnMenu.AutoImage = true;
            this.btnMenu.AutoImageMaxRatio = 0.75F;
            this.btnMenu.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMenu.AutoImageTint = true;
            this.btnMenu.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnMenu.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMenu.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnMenu.BorderRadius = 5;
            this.btnMenu.BorderSize = 1;
            this.btnMenu.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMenu.Corner = BeeGlobal.Corner.Both;
            this.btnMenu.DebounceResizeMs = 16;
            this.btnMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMenu.FlatAppearance.BorderSize = 0;
            this.btnMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnMenu.ForeColor = System.Drawing.Color.Black;
            this.btnMenu.Image = null;
            this.btnMenu.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenu.ImageDisabled = null;
            this.btnMenu.ImageHover = null;
            this.btnMenu.ImageNormal = null;
            this.btnMenu.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnMenu.ImagePressed = null;
            this.btnMenu.ImageTextSpacing = 6;
            this.btnMenu.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnMenu.ImageTintHover = System.Drawing.Color.Empty;
            this.btnMenu.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnMenu.ImageTintOpacity = 0.5F;
            this.btnMenu.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnMenu.IsCLick = false;
            this.btnMenu.IsNotChange = false;
            this.btnMenu.IsRect = false;
            this.btnMenu.IsUnGroup = true;
            this.btnMenu.Location = new System.Drawing.Point(271, 2);
            this.btnMenu.Margin = new System.Windows.Forms.Padding(2);
            this.btnMenu.Multiline = false;
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(59, 42);
            this.btnMenu.TabIndex = 43;
            this.btnMenu.Text = "Menu Bar";
            this.btnMenu.TextColor = System.Drawing.Color.Black;
            this.btnMenu.UseVisualStyleBackColor = false;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // btnShowDashBoard
            // 
            this.btnShowDashBoard.AutoFont = true;
            this.btnShowDashBoard.AutoFontHeightRatio = 1F;
            this.btnShowDashBoard.AutoFontMax = 100F;
            this.btnShowDashBoard.AutoFontMin = 8F;
            this.btnShowDashBoard.AutoFontWidthRatio = 0.92F;
            this.btnShowDashBoard.AutoImage = true;
            this.btnShowDashBoard.AutoImageMaxRatio = 0.75F;
            this.btnShowDashBoard.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnShowDashBoard.AutoImageTint = true;
            this.btnShowDashBoard.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnShowDashBoard.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnShowDashBoard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnShowDashBoard.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnShowDashBoard.BorderRadius = 5;
            this.btnShowDashBoard.BorderSize = 1;
            this.btnShowDashBoard.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnShowDashBoard.Corner = BeeGlobal.Corner.Both;
            this.btnShowDashBoard.DebounceResizeMs = 16;
            this.btnShowDashBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowDashBoard.FlatAppearance.BorderSize = 0;
            this.btnShowDashBoard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowDashBoard.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnShowDashBoard.ForeColor = System.Drawing.Color.Black;
            this.btnShowDashBoard.Image = null;
            this.btnShowDashBoard.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShowDashBoard.ImageDisabled = null;
            this.btnShowDashBoard.ImageHover = null;
            this.btnShowDashBoard.ImageNormal = null;
            this.btnShowDashBoard.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnShowDashBoard.ImagePressed = null;
            this.btnShowDashBoard.ImageTextSpacing = 6;
            this.btnShowDashBoard.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnShowDashBoard.ImageTintHover = System.Drawing.Color.Empty;
            this.btnShowDashBoard.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnShowDashBoard.ImageTintOpacity = 0.5F;
            this.btnShowDashBoard.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnShowDashBoard.IsCLick = false;
            this.btnShowDashBoard.IsNotChange = false;
            this.btnShowDashBoard.IsRect = false;
            this.btnShowDashBoard.IsUnGroup = true;
            this.btnShowDashBoard.Location = new System.Drawing.Point(208, 2);
            this.btnShowDashBoard.Margin = new System.Windows.Forms.Padding(2);
            this.btnShowDashBoard.Multiline = false;
            this.btnShowDashBoard.Name = "btnShowDashBoard";
            this.btnShowDashBoard.Size = new System.Drawing.Size(59, 42);
            this.btnShowDashBoard.TabIndex = 42;
            this.btnShowDashBoard.Text = "DashBoard";
            this.btnShowDashBoard.TextColor = System.Drawing.Color.Black;
            this.btnShowDashBoard.UseVisualStyleBackColor = false;
            this.btnShowDashBoard.Click += new System.EventHandler(this.btnShowDashBoard_Click);
            // 
            // btnShowTop
            // 
            this.btnShowTop.AutoFont = true;
            this.btnShowTop.AutoFontHeightRatio = 1F;
            this.btnShowTop.AutoFontMax = 100F;
            this.btnShowTop.AutoFontMin = 8F;
            this.btnShowTop.AutoFontWidthRatio = 0.92F;
            this.btnShowTop.AutoImage = true;
            this.btnShowTop.AutoImageMaxRatio = 0.75F;
            this.btnShowTop.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnShowTop.AutoImageTint = true;
            this.btnShowTop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnShowTop.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnShowTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnShowTop.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnShowTop.BorderRadius = 5;
            this.btnShowTop.BorderSize = 1;
            this.btnShowTop.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnShowTop.Corner = BeeGlobal.Corner.Both;
            this.btnShowTop.DebounceResizeMs = 16;
            this.btnShowTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowTop.FlatAppearance.BorderSize = 0;
            this.btnShowTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnShowTop.ForeColor = System.Drawing.Color.Black;
            this.btnShowTop.Image = null;
            this.btnShowTop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShowTop.ImageDisabled = null;
            this.btnShowTop.ImageHover = null;
            this.btnShowTop.ImageNormal = null;
            this.btnShowTop.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnShowTop.ImagePressed = null;
            this.btnShowTop.ImageTextSpacing = 6;
            this.btnShowTop.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnShowTop.ImageTintHover = System.Drawing.Color.Empty;
            this.btnShowTop.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnShowTop.ImageTintOpacity = 0.5F;
            this.btnShowTop.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnShowTop.IsCLick = false;
            this.btnShowTop.IsNotChange = false;
            this.btnShowTop.IsRect = false;
            this.btnShowTop.IsUnGroup = true;
            this.btnShowTop.Location = new System.Drawing.Point(145, 2);
            this.btnShowTop.Margin = new System.Windows.Forms.Padding(2);
            this.btnShowTop.Multiline = false;
            this.btnShowTop.Name = "btnShowTop";
            this.btnShowTop.Size = new System.Drawing.Size(59, 42);
            this.btnShowTop.TabIndex = 41;
            this.btnShowTop.Text = "Top Bar";
            this.btnShowTop.TextColor = System.Drawing.Color.Black;
            this.btnShowTop.UseVisualStyleBackColor = false;
            this.btnShowTop.Click += new System.EventHandler(this.btnShowTop_Click);
            // 
            // btnfull
            // 
            this.btnfull.AutoFont = true;
            this.btnfull.AutoFontHeightRatio = 1F;
            this.btnfull.AutoFontMax = 100F;
            this.btnfull.AutoFontMin = 8F;
            this.btnfull.AutoFontWidthRatio = 0.92F;
            this.btnfull.AutoImage = true;
            this.btnfull.AutoImageMaxRatio = 0.75F;
            this.btnfull.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnfull.AutoImageTint = true;
            this.btnfull.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnfull.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnfull.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnfull.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnfull.BorderRadius = 5;
            this.btnfull.BorderSize = 1;
            this.btnfull.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnfull.Corner = BeeGlobal.Corner.Both;
            this.btnfull.DebounceResizeMs = 16;
            this.btnfull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnfull.FlatAppearance.BorderSize = 0;
            this.btnfull.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnfull.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.39063F);
            this.btnfull.ForeColor = System.Drawing.Color.Black;
            this.btnfull.Image = null;
            this.btnfull.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnfull.ImageDisabled = null;
            this.btnfull.ImageHover = null;
            this.btnfull.ImageNormal = null;
            this.btnfull.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnfull.ImagePressed = null;
            this.btnfull.ImageTextSpacing = 6;
            this.btnfull.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnfull.ImageTintHover = System.Drawing.Color.Empty;
            this.btnfull.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnfull.ImageTintOpacity = 0.5F;
            this.btnfull.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnfull.IsCLick = false;
            this.btnfull.IsNotChange = false;
            this.btnfull.IsRect = false;
            this.btnfull.IsUnGroup = true;
            this.btnfull.Location = new System.Drawing.Point(82, 2);
            this.btnfull.Margin = new System.Windows.Forms.Padding(2);
            this.btnfull.Multiline = false;
            this.btnfull.Name = "btnfull";
            this.btnfull.Size = new System.Drawing.Size(59, 42);
            this.btnfull.TabIndex = 40;
            this.btnfull.Text = "Full";
            this.btnfull.TextColor = System.Drawing.Color.Black;
            this.btnfull.UseVisualStyleBackColor = false;
            this.btnfull.Click += new System.EventHandler(this.btnfull_Click);
            // 
            // HideBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "HideBar";
            this.Size = new System.Drawing.Size(577, 46);
            this.Load += new System.EventHandler(this.HideBar_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public RJButton btnShowDashBoard;
        public RJButton btnShowTop;
        public RJButton btnfull;
        public RJButton btnMenu;
        public RJButton btnShowToolBar;
        public RJButton rjButton1;
        public RJButton btnShuttdown;
        public RJButton btnExit;
    }
}
