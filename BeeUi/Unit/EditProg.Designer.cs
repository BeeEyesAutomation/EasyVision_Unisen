using BeeInterface;
using System.Windows.Forms;

namespace BeeUi.Unit
{
    partial class EditProg
    {

        #region Component Designer generated code
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditProg));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnMenu = new BeeInterface.RJButton();
            this.pMenu = new System.Windows.Forms.TableLayoutPanel();
            this.btnRename = new BeeInterface.RJButton();
            this.btnDelect = new BeeInterface.RJButton();
            this.btnSave = new BeeInterface.RJButton();
            this.btnSaveAs = new BeeInterface.RJButton();
            this.btnAdd = new BeeInterface.RJButton();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.pMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.btnMenu, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pMenu, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(500, 56);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnMenu
            // 
            this.btnMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMenu.AutoFont = false;
            this.btnMenu.AutoFontHeightRatio = 0.75F;
            this.btnMenu.AutoFontMax = 100F;
            this.btnMenu.AutoFontMin = 6F;
            this.btnMenu.AutoFontWidthRatio = 0.92F;
            this.btnMenu.AutoImage = true;
            this.btnMenu.AutoImageMaxRatio = 0.75F;
            this.btnMenu.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMenu.AutoImageTint = true;
            this.btnMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnMenu.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnMenu.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnMenu.BorderRadius = 8;
            this.btnMenu.BorderSize = 1;
            this.btnMenu.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnMenu.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnMenu.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnMenu.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMenu.Corner = BeeGlobal.Corner.Right;
            this.btnMenu.DebounceResizeMs = 16;
            this.btnMenu.FlatAppearance.BorderSize = 0;
            this.btnMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu.ForeColor = System.Drawing.Color.Black;
            this.btnMenu.Image = ((System.Drawing.Image)(resources.GetObject("btnMenu.Image")));
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
            this.btnMenu.Location = new System.Drawing.Point(470, 0);
            this.btnMenu.Margin = new System.Windows.Forms.Padding(0);
            this.btnMenu.Multiline = false;
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(30, 56);
            this.btnMenu.TabIndex = 26;
            this.btnMenu.TextColor = System.Drawing.Color.Black;
            this.btnMenu.UseVisualStyleBackColor = false;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // pMenu
            // 
            this.pMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.pMenu.ColumnCount = 5;
            this.pMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.pMenu.Controls.Add(this.btnRename, 2, 0);
            this.pMenu.Controls.Add(this.btnDelect, 4, 0);
            this.pMenu.Controls.Add(this.btnSave, 1, 0);
            this.pMenu.Controls.Add(this.btnSaveAs, 3, 0);
            this.pMenu.Controls.Add(this.btnAdd, 0, 0);
            this.pMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMenu.Location = new System.Drawing.Point(0, 0);
            this.pMenu.Margin = new System.Windows.Forms.Padding(0);
            this.pMenu.Name = "pMenu";
            this.pMenu.Padding = new System.Windows.Forms.Padding(3);
            this.pMenu.RowCount = 1;
            this.pMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pMenu.Size = new System.Drawing.Size(470, 56);
            this.pMenu.TabIndex = 25;
            this.pMenu.SizeChanged += new System.EventHandler(this.pMenu_SizeChanged);
            // 
            // btnRename
            // 
            this.btnRename.AutoFont = false;
            this.btnRename.AutoFontHeightRatio = 0.75F;
            this.btnRename.AutoFontMax = 100F;
            this.btnRename.AutoFontMin = 5F;
            this.btnRename.AutoFontWidthRatio = 0.92F;
            this.btnRename.AutoImage = true;
            this.btnRename.AutoImageMaxRatio = 0.5F;
            this.btnRename.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnRename.AutoImageTint = true;
            this.btnRename.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRename.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRename.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRename.BorderRadius = 4;
            this.btnRename.BorderSize = 1;
            this.btnRename.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnRename.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnRename.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnRename.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnRename.Corner = BeeGlobal.Corner.Both;
            this.btnRename.DebounceResizeMs = 16;
            this.btnRename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRename.FlatAppearance.BorderSize = 0;
            this.btnRename.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRename.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRename.ForeColor = System.Drawing.Color.Black;
            this.btnRename.Image = global::BeeUi.Properties.Resources.Rename;
            this.btnRename.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRename.ImageDisabled = null;
            this.btnRename.ImageHover = null;
            this.btnRename.ImageNormal = null;
            this.btnRename.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnRename.ImagePressed = null;
            this.btnRename.ImageTextSpacing = 2;
            this.btnRename.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnRename.ImageTintHover = System.Drawing.Color.Empty;
            this.btnRename.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnRename.ImageTintOpacity = 0.5F;
            this.btnRename.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnRename.IsCLick = false;
            this.btnRename.IsNotChange = true;
            this.btnRename.IsRect = false;
            this.btnRename.IsUnGroup = true;
            this.btnRename.Location = new System.Drawing.Point(189, 4);
            this.btnRename.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.btnRename.Multiline = false;
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(88, 48);
            this.btnRename.TabIndex = 16;
            this.btnRename.Text = "Rename";
            this.btnRename.TextColor = System.Drawing.Color.Black;
            this.btnRename.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRename.UseVisualStyleBackColor = false;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // btnDelect
            // 
            this.btnDelect.AutoFont = false;
            this.btnDelect.AutoFontHeightRatio = 0.75F;
            this.btnDelect.AutoFontMax = 100F;
            this.btnDelect.AutoFontMin = 5F;
            this.btnDelect.AutoFontWidthRatio = 0.92F;
            this.btnDelect.AutoImage = true;
            this.btnDelect.AutoImageMaxRatio = 0.5F;
            this.btnDelect.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnDelect.AutoImageTint = true;
            this.btnDelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDelect.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDelect.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDelect.BorderRadius = 4;
            this.btnDelect.BorderSize = 1;
            this.btnDelect.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnDelect.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnDelect.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnDelect.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnDelect.Corner = BeeGlobal.Corner.Both;
            this.btnDelect.DebounceResizeMs = 16;
            this.btnDelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelect.FlatAppearance.BorderSize = 0;
            this.btnDelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelect.ForeColor = System.Drawing.Color.Black;
            this.btnDelect.Image = ((System.Drawing.Image)(resources.GetObject("btnDelect.Image")));
            this.btnDelect.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDelect.ImageDisabled = null;
            this.btnDelect.ImageHover = null;
            this.btnDelect.ImageNormal = null;
            this.btnDelect.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnDelect.ImagePressed = null;
            this.btnDelect.ImageTextSpacing = 2;
            this.btnDelect.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnDelect.ImageTintHover = System.Drawing.Color.Empty;
            this.btnDelect.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnDelect.ImageTintOpacity = 0.5F;
            this.btnDelect.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnDelect.IsCLick = false;
            this.btnDelect.IsNotChange = true;
            this.btnDelect.IsRect = false;
            this.btnDelect.IsUnGroup = true;
            this.btnDelect.Location = new System.Drawing.Point(373, 4);
            this.btnDelect.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.btnDelect.Multiline = false;
            this.btnDelect.Name = "btnDelect";
            this.btnDelect.Size = new System.Drawing.Size(92, 48);
            this.btnDelect.TabIndex = 13;
            this.btnDelect.Text = "Delete ";
            this.btnDelect.TextColor = System.Drawing.Color.Black;
            this.btnDelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDelect.UseVisualStyleBackColor = false;
            this.btnDelect.Click += new System.EventHandler(this.btnDelect_Click);
            // 
            // btnSave
            // 
            this.btnSave.AutoFont = false;
            this.btnSave.AutoFontHeightRatio = 0.75F;
            this.btnSave.AutoFontMax = 100F;
            this.btnSave.AutoFontMin = 5F;
            this.btnSave.AutoFontWidthRatio = 0.92F;
            this.btnSave.AutoImage = true;
            this.btnSave.AutoImageMaxRatio = 0.5F;
            this.btnSave.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSave.AutoImageTint = true;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSave.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSave.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSave.BorderRadius = 4;
            this.btnSave.BorderSize = 1;
            this.btnSave.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnSave.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnSave.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnSave.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSave.Corner = BeeGlobal.Corner.Both;
            this.btnSave.DebounceResizeMs = 16;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.ImageDisabled = null;
            this.btnSave.ImageHover = null;
            this.btnSave.ImageNormal = null;
            this.btnSave.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSave.ImagePressed = null;
            this.btnSave.ImageTextSpacing = 2;
            this.btnSave.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSave.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSave.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSave.ImageTintOpacity = 0.5F;
            this.btnSave.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSave.IsCLick = false;
            this.btnSave.IsNotChange = true;
            this.btnSave.IsRect = false;
            this.btnSave.IsUnGroup = true;
            this.btnSave.Location = new System.Drawing.Point(97, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.btnSave.Multiline = false;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 48);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Save ";
            this.btnSave.TextColor = System.Drawing.Color.Black;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.AutoFont = false;
            this.btnSaveAs.AutoFontHeightRatio = 0.75F;
            this.btnSaveAs.AutoFontMax = 100F;
            this.btnSaveAs.AutoFontMin = 5F;
            this.btnSaveAs.AutoFontWidthRatio = 0.92F;
            this.btnSaveAs.AutoImage = true;
            this.btnSaveAs.AutoImageMaxRatio = 0.5F;
            this.btnSaveAs.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSaveAs.AutoImageTint = true;
            this.btnSaveAs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSaveAs.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSaveAs.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveAs.BackgroundImage")));
            this.btnSaveAs.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSaveAs.BorderRadius = 4;
            this.btnSaveAs.BorderSize = 1;
            this.btnSaveAs.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnSaveAs.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnSaveAs.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnSaveAs.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSaveAs.Corner = BeeGlobal.Corner.Both;
            this.btnSaveAs.DebounceResizeMs = 16;
            this.btnSaveAs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveAs.FlatAppearance.BorderSize = 0;
            this.btnSaveAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveAs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveAs.ForeColor = System.Drawing.Color.Black;
            this.btnSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAs.Image")));
            this.btnSaveAs.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSaveAs.ImageDisabled = null;
            this.btnSaveAs.ImageHover = null;
            this.btnSaveAs.ImageNormal = null;
            this.btnSaveAs.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSaveAs.ImagePressed = null;
            this.btnSaveAs.ImageTextSpacing = 2;
            this.btnSaveAs.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSaveAs.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSaveAs.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSaveAs.ImageTintOpacity = 0.5F;
            this.btnSaveAs.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSaveAs.IsCLick = false;
            this.btnSaveAs.IsNotChange = true;
            this.btnSaveAs.IsRect = false;
            this.btnSaveAs.IsUnGroup = true;
            this.btnSaveAs.Location = new System.Drawing.Point(281, 4);
            this.btnSaveAs.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.btnSaveAs.Multiline = false;
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(88, 48);
            this.btnSaveAs.TabIndex = 15;
            this.btnSaveAs.Text = "Copy";
            this.btnSaveAs.TextColor = System.Drawing.Color.Black;
            this.btnSaveAs.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSaveAs.UseVisualStyleBackColor = false;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.AutoFont = false;
            this.btnAdd.AutoFontHeightRatio = 0.75F;
            this.btnAdd.AutoFontMax = 100F;
            this.btnAdd.AutoFontMin = 5F;
            this.btnAdd.AutoFontWidthRatio = 0.92F;
            this.btnAdd.AutoImage = true;
            this.btnAdd.AutoImageMaxRatio = 0.5F;
            this.btnAdd.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnAdd.AutoImageTint = true;
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAdd.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAdd.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAdd.BorderRadius = 4;
            this.btnAdd.BorderSize = 1;
            this.btnAdd.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnAdd.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnAdd.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnAdd.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnAdd.Corner = BeeGlobal.Corner.Both;
            this.btnAdd.DebounceResizeMs = 16;
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.Black;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAdd.ImageDisabled = null;
            this.btnAdd.ImageHover = null;
            this.btnAdd.ImageNormal = null;
            this.btnAdd.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnAdd.ImagePressed = null;
            this.btnAdd.ImageTextSpacing = 2;
            this.btnAdd.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnAdd.ImageTintHover = System.Drawing.Color.Empty;
            this.btnAdd.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnAdd.ImageTintOpacity = 0.5F;
            this.btnAdd.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnAdd.IsCLick = false;
            this.btnAdd.IsNotChange = true;
            this.btnAdd.IsRect = false;
            this.btnAdd.IsUnGroup = true;
            this.btnAdd.Location = new System.Drawing.Point(5, 4);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.btnAdd.Multiline = false;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(88, 48);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "New";
            this.btnAdd.TextColor = System.Drawing.Color.Black;
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // saveFile
            // 
            this.saveFile.DefaultExt = "*.prog";
            this.saveFile.Filter = "Program | *.prog";
            this.saveFile.InitialDirectory = "Program";
            this.saveFile.Title = "Save As";
            // 
            // EditProg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "EditProg";
            this.Size = new System.Drawing.Size(500, 56);
            this.Load += new System.EventHandler(this.EditProg_Load);
            this.SizeChanged += new System.EventHandler(this.EditProg_SizeChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
        private  System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public  System.Windows.Forms.TableLayoutPanel pMenu;
        private System.Windows.Forms.SaveFileDialog saveFile;
        public RJButton btnSave;
        public RJButton btnDelect;
        public RJButton btnSaveAs;
        public RJButton btnAdd;
        public RJButton btnRename;
        public RJButton btnMenu;
    }
}
