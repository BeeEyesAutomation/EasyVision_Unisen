using BeeCore;
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
            System.Windows.Forms.Timer tmOutAlive;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Header));
            this.workConnect = new System.ComponentModel.BackgroundWorker();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.pPO = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel13 = new System.Windows.Forms.Panel();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.SerialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.workLoadProgram = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tmQrCode = new System.Windows.Forms.Timer(this.components);
            this.workSaveProject = new System.ComponentModel.BackgroundWorker();
            this.Layout = new System.Windows.Forms.TableLayoutPanel();
            this.btnMode = new BeeInterface.RJButton();
            this.pModel = new System.Windows.Forms.TableLayoutPanel();
            this.btnEnQrCode = new BeeInterface.RJButton();
            this.btnShowList = new BeeInterface.RJButton();
            this.txtQrCode = new BeeInterface.TextBoxAuto();
            this.editProg1 = new BeeUi.Unit.EditProg();
            this.tmShow = new System.Windows.Forms.Timer(this.components);
            this.tmIninitial = new System.Windows.Forms.Timer(this.components);
            tmOutAlive = new System.Windows.Forms.Timer(this.components);
            this.pPO.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel13.SuspendLayout();
            this.Layout.SuspendLayout();
            this.pModel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmOutAlive
            // 
            tmOutAlive.Interval = 500;
            tmOutAlive.Tick += new System.EventHandler(this.tmOutAlive_Tick);
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
            // pPO
            // 
            this.pPO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(114)))), ((int)(((byte)(114)))));
            this.pPO.Controls.Add(this.panel4);
            this.pPO.Controls.Add(this.panel13);
            this.pPO.Controls.Add(this.panel8);
            this.pPO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pPO.Location = new System.Drawing.Point(192, 2);
            this.pPO.Margin = new System.Windows.Forms.Padding(2);
            this.pPO.Name = "pPO";
            this.pPO.Padding = new System.Windows.Forms.Padding(5);
            this.pPO.Size = new System.Drawing.Size(396, 73);
            this.pPO.TabIndex = 28;
            this.pPO.Visible = false;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.Controls.Add(this.textBox4);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.textBox5);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(5, 37);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(384, 31);
            this.panel4.TabIndex = 36;
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.textBox4.Location = new System.Drawing.Point(268, 0);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(116, 31);
            this.textBox4.TabIndex = 33;
            this.textBox4.Text = "123456789";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(181, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 31);
            this.label4.TabIndex = 17;
            this.label4.Text = "LOT";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox5.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox5.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.textBox5.Location = new System.Drawing.Point(71, 0);
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(110, 31);
            this.textBox5.TabIndex = 32;
            this.textBox5.Text = "VN123";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 31);
            this.label5.TabIndex = 16;
            this.label5.Text = "CUS";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel13
            // 
            this.panel13.BackColor = System.Drawing.Color.Transparent;
            this.panel13.Controls.Add(this.textBox3);
            this.panel13.Controls.Add(this.label3);
            this.panel13.Controls.Add(this.textBox2);
            this.panel13.Controls.Add(this.label2);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel13.Location = new System.Drawing.Point(5, 5);
            this.panel13.Name = "panel13";
            this.panel13.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.panel13.Size = new System.Drawing.Size(384, 32);
            this.panel13.TabIndex = 35;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.textBox3.Location = new System.Drawing.Point(268, 0);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(116, 30);
            this.textBox3.TabIndex = 33;
            this.textBox3.Text = "123456789";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(181, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 30);
            this.label3.TabIndex = 17;
            this.label3.Text = "PO";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.textBox2.Location = new System.Drawing.Point(71, 0);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(110, 30);
            this.textBox2.TabIndex = 32;
            this.textBox2.Text = "N.V.A";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 30);
            this.label2.TabIndex = 16;
            this.label2.Text = "NV";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.Transparent;
            this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(389, 5);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(2, 63);
            this.panel8.TabIndex = 29;
            // 
            // SerialPort1
            // 
            this.SerialPort1.BaudRate = 115200;
            this.SerialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.SerialPort_DataReceived);
            // 
            // workLoadProgram
            // 
            this.workLoadProgram.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workLoadProgram_DoWork);
            this.workLoadProgram.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workLoadProgram_RunWorkerCompleted);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
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
            // Layout
            // 
            this.Layout.BackColor = System.Drawing.Color.Transparent;
            this.Layout.ColumnCount = 4;
            this.Layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Layout.Controls.Add(this.btnMode, 0, 0);
            this.Layout.Controls.Add(this.pPO, 1, 0);
            this.Layout.Controls.Add(this.pModel, 2, 0);
            this.Layout.Controls.Add(this.editProg1, 3, 0);
            this.Layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Layout.Location = new System.Drawing.Point(0, 0);
            this.Layout.Margin = new System.Windows.Forms.Padding(0);
            this.Layout.Name = "Layout";
            this.Layout.RowCount = 1;
            this.Layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Layout.Size = new System.Drawing.Size(2135, 77);
            this.Layout.TabIndex = 31;
            // 
            // btnMode
            // 
            this.btnMode.BackColor = System.Drawing.Color.Transparent;
            this.btnMode.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnMode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMode.BackgroundImage")));
            this.btnMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMode.BorderColor = System.Drawing.Color.Transparent;
            this.btnMode.BorderRadius = 5;
            this.btnMode.BorderSize = 1;
            this.btnMode.Corner = BeeGlobal.Corner.Both;
            this.btnMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMode.FlatAppearance.BorderSize = 0;
            this.btnMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 23.42969F);
            this.btnMode.ForeColor = System.Drawing.Color.Black;
            this.btnMode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMode.IsCLick = false;
            this.btnMode.IsNotChange = false;
            this.btnMode.IsRect = false;
            this.btnMode.IsUnGroup = true;
            this.btnMode.Location = new System.Drawing.Point(2, 5);
            this.btnMode.Margin = new System.Windows.Forms.Padding(2, 5, 8, 5);
            this.btnMode.Name = "btnMode";
            this.btnMode.Size = new System.Drawing.Size(180, 67);
            this.btnMode.TabIndex = 27;
            this.btnMode.Text = "RUN";
            this.btnMode.TextColor = System.Drawing.Color.Black;
            this.btnMode.UseVisualStyleBackColor = false;
            this.btnMode.Click += new System.EventHandler(this.btnMode_Click);
            // 
            // pModel
            // 
            this.pModel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pModel.ColumnCount = 5;
            this.pModel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pModel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pModel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pModel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.pModel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.pModel.Controls.Add(this.btnEnQrCode, 1, 0);
            this.pModel.Controls.Add(this.btnShowList, 3, 0);
            this.pModel.Controls.Add(this.txtQrCode, 2, 0);
            this.pModel.Location = new System.Drawing.Point(593, 3);
            this.pModel.Name = "pModel";
            this.pModel.RowCount = 1;
            this.pModel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pModel.Size = new System.Drawing.Size(1504, 71);
            this.pModel.TabIndex = 29;
            this.pModel.SizeChanged += new System.EventHandler(this.pModel_SizeChanged);
            // 
            // btnEnQrCode
            // 
            this.btnEnQrCode.BackColor = System.Drawing.Color.Transparent;
            this.btnEnQrCode.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnEnQrCode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEnQrCode.BackgroundImage")));
            this.btnEnQrCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnEnQrCode.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEnQrCode.BorderRadius = 10;
            this.btnEnQrCode.BorderSize = 1;
            this.btnEnQrCode.Corner = BeeGlobal.Corner.Both;
            this.btnEnQrCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnQrCode.FlatAppearance.BorderSize = 0;
            this.btnEnQrCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnQrCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnQrCode.ForeColor = System.Drawing.Color.Black;
            this.btnEnQrCode.Image = ((System.Drawing.Image)(resources.GetObject("btnEnQrCode.Image")));
            this.btnEnQrCode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnQrCode.IsCLick = false;
            this.btnEnQrCode.IsNotChange = false;
            this.btnEnQrCode.IsRect = false;
            this.btnEnQrCode.IsUnGroup = true;
            this.btnEnQrCode.Location = new System.Drawing.Point(10, 8);
            this.btnEnQrCode.Margin = new System.Windows.Forms.Padding(10, 8, 0, 8);
            this.btnEnQrCode.Name = "btnEnQrCode";
            this.btnEnQrCode.Size = new System.Drawing.Size(60, 55);
            this.btnEnQrCode.TabIndex = 23;
            this.btnEnQrCode.TextColor = System.Drawing.Color.Black;
            this.btnEnQrCode.UseVisualStyleBackColor = false;
            this.btnEnQrCode.Visible = false;
            this.btnEnQrCode.Click += new System.EventHandler(this.btnEnQrCode_Click);
            // 
            // btnShowList
            // 
            this.btnShowList.BackColor = System.Drawing.Color.Transparent;
            this.btnShowList.BackgroundColor = System.Drawing.Color.Transparent;
            this.btnShowList.BorderColor = System.Drawing.Color.Transparent;
            this.btnShowList.BorderRadius = 5;
            this.btnShowList.BorderSize = 1;
            this.btnShowList.Corner = BeeGlobal.Corner.None;
            this.btnShowList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnShowList.FlatAppearance.BorderSize = 0;
            this.btnShowList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowList.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.24219F);
            this.btnShowList.ForeColor = System.Drawing.Color.White;
            this.btnShowList.Image = global::BeeUi.Properties.Resources.Down_Button;
            this.btnShowList.IsCLick = false;
            this.btnShowList.IsNotChange = false;
            this.btnShowList.IsRect = false;
            this.btnShowList.IsUnGroup = false;
            this.btnShowList.Location = new System.Drawing.Point(1438, 3);
            this.btnShowList.Name = "btnShowList";
            this.btnShowList.Size = new System.Drawing.Size(55, 65);
            this.btnShowList.TabIndex = 27;
            this.btnShowList.TextColor = System.Drawing.Color.White;
            this.btnShowList.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnShowList.UseVisualStyleBackColor = false;
            this.btnShowList.Click += new System.EventHandler(this.btnShowList_Click);
            // 
            // txtQrCode
            // 
            this.txtQrCode.AutoFont = true;
            this.txtQrCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQrCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 39.67188F);
            this.txtQrCode.Location = new System.Drawing.Point(73, 3);
            this.txtQrCode.Name = "txtQrCode";
            this.txtQrCode.Size = new System.Drawing.Size(1359, 65);
            this.txtQrCode.TabIndex = 28;
            this.txtQrCode.Text = "Prog no";
            this.txtQrCode.TextChanged += new System.EventHandler(this.txtQrCode_TextChanged);
            this.txtQrCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQrCode_KeyDown);
            this.txtQrCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQrCode_KeyPress);
            // 
            // editProg1
            // 
            this.editProg1.BackColor = System.Drawing.Color.Transparent;
            this.editProg1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editProg1.Location = new System.Drawing.Point(2103, 3);
            this.editProg1.Name = "editProg1";
            this.editProg1.Size = new System.Drawing.Size(29, 71);
            this.editProg1.TabIndex = 30;
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
            // Header
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Controls.Add(this.Layout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Header";
            this.Size = new System.Drawing.Size(2135, 77);
            this.Load += new System.EventHandler(this.Header_Load);
            this.SizeChanged += new System.EventHandler(this.Header_SizeChanged);
            this.pPO.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.Layout.ResumeLayout(false);
            this.pModel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker workConnect;
        private System.Windows.Forms.SaveFileDialog saveFile;
        public System.IO.Ports.SerialPort SerialPort1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Timer tmQrCode;
        public System.ComponentModel.BackgroundWorker workLoadProgram;
        public System.ComponentModel.BackgroundWorker workSaveProject;
        public RJButton btnEnQrCode;
        public RJButton btnMode;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        public System.Windows.Forms.Panel pPO;
        private System.Windows.Forms.Panel panel4;
        public System.Windows.Forms.TableLayoutPanel pModel;
      
        public System.Windows.Forms.TableLayoutPanel Layout;
        private System.Windows.Forms.Timer tmIninitial;
        public System.Windows.Forms.Timer tmShow;
        private Unit.EditProg editProg1;
        private RJButton btnShowList;
        private TextBoxAuto txtQrCode;
    }
}
