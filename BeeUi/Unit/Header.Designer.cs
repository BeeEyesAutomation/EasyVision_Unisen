﻿using BeeCore;
using BeeGlobal;
using BeeInterface;

namespace BeeUi.Common
{
    partial class Header
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Header));
            this.workConnect = new System.ComponentModel.BackgroundWorker();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.workLoadProgram = new System.ComponentModel.BackgroundWorker();
            this.tmQrCode = new System.Windows.Forms.Timer(this.components);
            this.workSaveProject = new System.ComponentModel.BackgroundWorker();
            this.pModel = new System.Windows.Forms.TableLayoutPanel();
            this.txtQrCode = new BeeInterface.TextBoxAuto();
            this.btnEnQrCode = new BeeInterface.RJButton();
            this.btnShowList = new BeeInterface.RJButton();
            this.tmShow = new System.Windows.Forms.Timer(this.components);
            this.tmIninitial = new System.Windows.Forms.Timer(this.components);
            this.split1 = new System.Windows.Forms.Splitter();
            this.split2 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnMode = new BeeInterface.RJButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnTraining = new BeeInterface.RJButton();
            this.pEdit = new BeeUi.Unit.EditProg();
            this.pModel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // workConnect
            // 
            this.workConnect.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workConnect_DoWork);
            this.workConnect.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workConnect_RunWorkerCompleted);
            // 
            // saveFile
            // 
            this.saveFile.DefaultExt = "*.prog";
            this.saveFile.Filter = "Program | *.prog";
            this.saveFile.InitialDirectory = "Program";
            this.saveFile.Title = "Save As";
            // 
            // workLoadProgram
            // 
            this.workLoadProgram.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workLoadProgram_DoWork);
            this.workLoadProgram.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workLoadProgram_RunWorkerCompleted);
            // 
            // tmQrCode
            // 
            this.tmQrCode.Interval = 10000;
            this.tmQrCode.Tick += new System.EventHandler(this.tmQrCode_Tick);
            // 
            // workSaveProject
            // 
            this.workSaveProject.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workSaveProject_DoWork);
            this.workSaveProject.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workSaveProject_RunWorkerCompleted);
            // 
            // pModel
            // 
            this.pModel.BackColor = System.Drawing.Color.Transparent;
            this.pModel.ColumnCount = 4;
            this.pModel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pModel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pModel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pModel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pModel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pModel.Controls.Add(this.txtQrCode, 2, 0);
            this.pModel.Controls.Add(this.btnEnQrCode, 1, 0);
            this.pModel.Controls.Add(this.btnShowList, 3, 0);
            this.pModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pModel.Location = new System.Drawing.Point(315, 0);
            this.pModel.Name = "pModel";
            this.pModel.RowCount = 1;
            this.pModel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pModel.Size = new System.Drawing.Size(1320, 81);
            this.pModel.TabIndex = 29;
            this.pModel.SizeChanged += new System.EventHandler(this.pModel_SizeChanged);
            // 
            // txtQrCode
            // 
            this.txtQrCode.AutoFont = true;
            this.txtQrCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQrCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 46.24609F);
            this.txtQrCode.Location = new System.Drawing.Point(73, 3);
            this.txtQrCode.Name = "txtQrCode";
            this.txtQrCode.Size = new System.Drawing.Size(1163, 75);
            this.txtQrCode.TabIndex = 28;
            this.txtQrCode.Text = "Prog no";
            this.txtQrCode.TextChanged += new System.EventHandler(this.txtQrCode_TextChanged);
            this.txtQrCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQrCode_KeyDown);
            this.txtQrCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQrCode_KeyPress);
            // 
            // btnEnQrCode
            // 
            this.btnEnQrCode.AutoFont = true;
            this.btnEnQrCode.AutoFontHeightRatio = 0.6F;
            this.btnEnQrCode.AutoFontMax = 100F;
            this.btnEnQrCode.AutoFontMin = 6F;
            this.btnEnQrCode.AutoFontWidthRatio = 0.92F;
            this.btnEnQrCode.AutoImage = true;
            this.btnEnQrCode.AutoImageMaxRatio = 0.75F;
            this.btnEnQrCode.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnEnQrCode.AutoImageTint = true;
            this.btnEnQrCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnEnQrCode.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnEnQrCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnEnQrCode.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnEnQrCode.BorderRadius = 10;
            this.btnEnQrCode.BorderSize = 1;
            this.btnEnQrCode.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnEnQrCode.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnEnQrCode.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnEnQrCode.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnEnQrCode.Corner = BeeGlobal.Corner.Both;
            this.btnEnQrCode.DebounceResizeMs = 16;
            this.btnEnQrCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnQrCode.FlatAppearance.BorderSize = 0;
            this.btnEnQrCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnQrCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnQrCode.ForeColor = System.Drawing.Color.Black;
            this.btnEnQrCode.Image = ((System.Drawing.Image)(resources.GetObject("btnEnQrCode.Image")));
            this.btnEnQrCode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnQrCode.ImageDisabled = null;
            this.btnEnQrCode.ImageHover = null;
            this.btnEnQrCode.ImageNormal = null;
            this.btnEnQrCode.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnEnQrCode.ImagePressed = null;
            this.btnEnQrCode.ImageTextSpacing = 6;
            this.btnEnQrCode.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnEnQrCode.ImageTintHover = System.Drawing.Color.Empty;
            this.btnEnQrCode.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnEnQrCode.ImageTintOpacity = 0.5F;
            this.btnEnQrCode.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnEnQrCode.IsCLick = false;
            this.btnEnQrCode.IsNotChange = false;
            this.btnEnQrCode.IsRect = false;
            this.btnEnQrCode.IsUnGroup = true;
            this.btnEnQrCode.Location = new System.Drawing.Point(10, 8);
            this.btnEnQrCode.Margin = new System.Windows.Forms.Padding(10, 8, 0, 8);
            this.btnEnQrCode.Multiline = false;
            this.btnEnQrCode.Name = "btnEnQrCode";
            this.btnEnQrCode.Size = new System.Drawing.Size(60, 65);
            this.btnEnQrCode.TabIndex = 23;
            this.btnEnQrCode.TextColor = System.Drawing.Color.Black;
            this.btnEnQrCode.UseVisualStyleBackColor = false;
            this.btnEnQrCode.Visible = false;
            this.btnEnQrCode.Click += new System.EventHandler(this.btnEnQrCode_Click);
            // 
            // btnShowList
            // 
            this.btnShowList.AutoFont = true;
            this.btnShowList.AutoFontHeightRatio = 0.6F;
            this.btnShowList.AutoFontMax = 100F;
            this.btnShowList.AutoFontMin = 6F;
            this.btnShowList.AutoFontWidthRatio = 0.92F;
            this.btnShowList.AutoImage = true;
            this.btnShowList.AutoImageMaxRatio = 0.75F;
            this.btnShowList.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnShowList.AutoImageTint = true;
            this.btnShowList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnShowList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnShowList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnShowList.BorderRadius = 10;
            this.btnShowList.BorderSize = 1;
            this.btnShowList.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnShowList.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnShowList.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnShowList.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnShowList.Corner = BeeGlobal.Corner.Both;
            this.btnShowList.DebounceResizeMs = 16;
            this.btnShowList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowList.FlatAppearance.BorderSize = 0;
            this.btnShowList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowList.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.24219F);
            this.btnShowList.ForeColor = System.Drawing.Color.White;
            this.btnShowList.Image = global::BeeUi.Properties.Resources.Down_Button;
            this.btnShowList.ImageDisabled = null;
            this.btnShowList.ImageHover = null;
            this.btnShowList.ImageNormal = null;
            this.btnShowList.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnShowList.ImagePressed = null;
            this.btnShowList.ImageTextSpacing = 6;
            this.btnShowList.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnShowList.ImageTintHover = System.Drawing.Color.Empty;
            this.btnShowList.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnShowList.ImageTintOpacity = 0.5F;
            this.btnShowList.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnShowList.IsCLick = false;
            this.btnShowList.IsNotChange = true;
            this.btnShowList.IsRect = false;
            this.btnShowList.IsUnGroup = false;
            this.btnShowList.Location = new System.Drawing.Point(1242, 3);
            this.btnShowList.Multiline = false;
            this.btnShowList.Name = "btnShowList";
            this.btnShowList.Size = new System.Drawing.Size(75, 75);
            this.btnShowList.TabIndex = 27;
            this.btnShowList.TextColor = System.Drawing.Color.White;
            this.btnShowList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnShowList.UseVisualStyleBackColor = false;
            this.btnShowList.Click += new System.EventHandler(this.btnShowList_Click);
            // 
            // tmShow
            // 
            this.tmShow.Interval = 500;
            this.tmShow.Tick += new System.EventHandler(this.tmShow_Tick);
            // 
            // tmIninitial
            // 
            this.tmIninitial.Interval = 2000;
            this.tmIninitial.Tick += new System.EventHandler(this.tmIninitial_Tick);
            // 
            // split1
            // 
            this.split1.Location = new System.Drawing.Point(200, 0);
            this.split1.MaximumSize = new System.Drawing.Size(20, 81);
            this.split1.Name = "split1";
            this.split1.Size = new System.Drawing.Size(15, 81);
            this.split1.TabIndex = 31;
            this.split1.TabStop = false;
            // 
            // split2
            // 
            this.split2.Dock = System.Windows.Forms.DockStyle.Right;
            this.split2.Location = new System.Drawing.Point(1630, 0);
            this.split2.MaximumSize = new System.Drawing.Size(5, 81);
            this.split2.Name = "split2";
            this.split2.Size = new System.Drawing.Size(5, 81);
            this.split2.TabIndex = 32;
            this.split2.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnMode);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.panel1.Size = new System.Drawing.Size(200, 81);
            this.panel1.TabIndex = 33;
            // 
            // btnMode
            // 
            this.btnMode.AutoFont = true;
            this.btnMode.AutoFontHeightRatio = 0.8F;
            this.btnMode.AutoFontMax = 100F;
            this.btnMode.AutoFontMin = 6F;
            this.btnMode.AutoFontWidthRatio = 0.92F;
            this.btnMode.AutoImage = true;
            this.btnMode.AutoImageMaxRatio = 0.75F;
            this.btnMode.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMode.AutoImageTint = true;
            this.btnMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.btnMode.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.btnMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMode.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.btnMode.BorderRadius = 15;
            this.btnMode.BorderSize = 1;
            this.btnMode.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnMode.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnMode.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnMode.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMode.Corner = BeeGlobal.Corner.Both;
            this.btnMode.DebounceResizeMs = 16;
            this.btnMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMode.Enabled = false;
            this.btnMode.FlatAppearance.BorderSize = 0;
            this.btnMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 31.33594F);
            this.btnMode.ForeColor = System.Drawing.Color.Black;
            this.btnMode.Image = null;
            this.btnMode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMode.ImageDisabled = null;
            this.btnMode.ImageHover = null;
            this.btnMode.ImageNormal = null;
            this.btnMode.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnMode.ImagePressed = null;
            this.btnMode.ImageTextSpacing = 6;
            this.btnMode.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnMode.ImageTintHover = System.Drawing.Color.Empty;
            this.btnMode.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnMode.ImageTintOpacity = 0.5F;
            this.btnMode.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnMode.IsCLick = false;
            this.btnMode.IsNotChange = false;
            this.btnMode.IsRect = false;
            this.btnMode.IsUnGroup = true;
            this.btnMode.Location = new System.Drawing.Point(2, 3);
            this.btnMode.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.btnMode.Multiline = false;
            this.btnMode.Name = "btnMode";
            this.btnMode.Size = new System.Drawing.Size(198, 75);
            this.btnMode.TabIndex = 27;
            this.btnMode.Text = "RUN";
            this.btnMode.TextColor = System.Drawing.Color.Black;
            this.btnMode.UseVisualStyleBackColor = false;
            this.btnMode.Click += new System.EventHandler(this.btnMode_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnTraining);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(215, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(2, 3, 0, 3);
            this.panel2.Size = new System.Drawing.Size(100, 81);
            this.panel2.TabIndex = 37;
            this.panel2.Visible = false;
            // 
            // btnTraining
            // 
            this.btnTraining.AutoFont = true;
            this.btnTraining.AutoFontHeightRatio = 0.9F;
            this.btnTraining.AutoFontMax = 100F;
            this.btnTraining.AutoFontMin = 8F;
            this.btnTraining.AutoFontWidthRatio = 0.92F;
            this.btnTraining.AutoImage = true;
            this.btnTraining.AutoImageMaxRatio = 0.5F;
            this.btnTraining.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnTraining.AutoImageTint = true;
            this.btnTraining.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.btnTraining.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.btnTraining.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnTraining.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.btnTraining.BorderRadius = 15;
            this.btnTraining.BorderSize = 1;
            this.btnTraining.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnTraining.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnTraining.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnTraining.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnTraining.Corner = BeeGlobal.Corner.Both;
            this.btnTraining.DebounceResizeMs = 16;
            this.btnTraining.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTraining.FlatAppearance.BorderSize = 0;
            this.btnTraining.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTraining.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnTraining.ForeColor = System.Drawing.Color.IndianRed;
            this.btnTraining.Image = global::BeeUi.Properties.Resources.Change_1;
            this.btnTraining.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTraining.ImageDisabled = null;
            this.btnTraining.ImageHover = null;
            this.btnTraining.ImageNormal = null;
            this.btnTraining.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnTraining.ImagePressed = null;
            this.btnTraining.ImageTextSpacing = 6;
            this.btnTraining.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnTraining.ImageTintHover = System.Drawing.Color.Empty;
            this.btnTraining.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnTraining.ImageTintOpacity = 0.5F;
            this.btnTraining.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnTraining.IsCLick = false;
            this.btnTraining.IsNotChange = false;
            this.btnTraining.IsRect = false;
            this.btnTraining.IsUnGroup = true;
            this.btnTraining.Location = new System.Drawing.Point(2, 3);
            this.btnTraining.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.btnTraining.Multiline = false;
            this.btnTraining.Name = "btnTraining";
            this.btnTraining.Size = new System.Drawing.Size(98, 75);
            this.btnTraining.TabIndex = 27;
            this.btnTraining.Text = "Training";
            this.btnTraining.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnTraining.TextColor = System.Drawing.Color.IndianRed;
            this.btnTraining.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnTraining.UseVisualStyleBackColor = false;
            this.btnTraining.Click += new System.EventHandler(this.btnTraining_Click);
            // 
            // pEdit
            // 
            this.pEdit.BackColor = System.Drawing.Color.Transparent;
            this.pEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.pEdit.Location = new System.Drawing.Point(1635, 0);
            this.pEdit.Name = "pEdit";
            this.pEdit.Size = new System.Drawing.Size(500, 81);
            this.pEdit.TabIndex = 36;
            // 
            // Header
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(195)))), ((int)(((byte)(195)))));
            this.Controls.Add(this.split2);
            this.Controls.Add(this.pModel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pEdit);
            this.Controls.Add(this.split1);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(3000, 90);
            this.Name = "Header";
            this.Size = new System.Drawing.Size(2135, 81);
            this.Load += new System.EventHandler(this.Header_Load);
            this.SizeChanged += new System.EventHandler(this.Header_SizeChanged);
            this.pModel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker workConnect;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.Timer tmQrCode;
        public System.ComponentModel.BackgroundWorker workLoadProgram;
        public System.ComponentModel.BackgroundWorker workSaveProject;
        public RJButton btnEnQrCode;
        public RJButton btnMode;
        public  System.Windows.Forms.TableLayoutPanel pModel;
        private System.Windows.Forms.Timer tmIninitial;
        public System.Windows.Forms.Timer tmShow;
        private RJButton btnShowList;
        private System.Windows.Forms.Panel panel1;
        public TextBoxAuto txtQrCode;
        public System.Windows.Forms.Splitter split1;
        public System.Windows.Forms.Splitter split2;
        public Unit.EditProg pEdit;
        private System.Windows.Forms.Panel panel2;
        public RJButton btnTraining;
    }
}
