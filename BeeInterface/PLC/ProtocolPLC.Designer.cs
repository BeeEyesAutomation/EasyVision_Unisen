using System.Windows.Forms;
namespace BeeInterface
{
    partial class ProtocolPLC
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
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.laySetting = new System.Windows.Forms.TableLayoutPanel();
            this.pConnect = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.StatusIObtn = new BeeInterface.RJButton();
            this.btnConectIO = new BeeInterface.RJButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnBypass = new BeeInterface.RJButton();
            this.btn4S = new BeeInterface.RJButton();
            this.btn3S = new BeeInterface.RJButton();
            this.btn2S = new BeeInterface.RJButton();
            this.btn1S = new BeeInterface.RJButton();
            this.layTypeHard = new System.Windows.Forms.TableLayoutPanel();
            this.btnPCICard = new BeeInterface.RJButton();
            this.btnIO = new BeeInterface.RJButton();
            this.btnIsPLC = new BeeInterface.RJButton();
            this.layTypeComunication = new System.Windows.Forms.TableLayoutPanel();
            this.btnTCP = new BeeInterface.RJButton();
            this.btnSerial = new BeeInterface.RJButton();
            this.layBrand = new System.Windows.Forms.TableLayoutPanel();
            this.btnDelta = new BeeInterface.RJButton();
            this.rjButton2 = new BeeInterface.RJButton();
            this.btnMitsu2 = new BeeInterface.RJButton();
            this.btnMitsu = new BeeInterface.RJButton();
            this.rjButton7 = new BeeInterface.RJButton();
            this.btnMisu3 = new BeeInterface.RJButton();
            this.btnKeyence = new BeeInterface.RJButton();
            this.btnRtu = new BeeInterface.RJButton();
            this.btnModbusAscii = new BeeInterface.RJButton();
            this.laySettingCom = new System.Windows.Forms.TableLayoutPanel();
            this.layCom = new System.Windows.Forms.TableLayoutPanel();
            this.numSlaveID = new BeeInterface.CustomNumericEx();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRtsEnable = new BeeInterface.RJButton();
            this.btnDtrEnable = new BeeInterface.RJButton();
            this.cbStopBits = new System.Windows.Forms.ComboBox();
            this.cbParity = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbBaurate = new System.Windows.Forms.ComboBox();
            this.lbRTU2 = new System.Windows.Forms.Label();
            this.cbCom = new System.Windows.Forms.ComboBox();
            this.lbRTU1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cbDataBits = new System.Windows.Forms.ComboBox();
            this.layIP = new System.Windows.Forms.TableLayoutPanel();
            this.btnReScan = new BeeInterface.RJButton();
            this.lbTCP1 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.lbTCP2 = new System.Windows.Forms.Label();
            this.layParameterBits = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.label26 = new System.Windows.Forms.Label();
            this.rjButton9 = new BeeInterface.RJButton();
            this.customNumericEx2 = new BeeInterface.CustomNumericEx();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.rjButton10 = new BeeInterface.RJButton();
            this.label22 = new System.Windows.Forms.Label();
            this.rjButton3 = new BeeInterface.RJButton();
            this.customNumericEx1 = new BeeInterface.CustomNumericEx();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.label24 = new System.Windows.Forms.Label();
            this.rjButton8 = new BeeInterface.RJButton();
            this.tmOut = new BeeInterface.CustomNumericEx();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label28 = new System.Windows.Forms.Label();
            this.rjButton5 = new BeeInterface.RJButton();
            this.timerRead = new BeeInterface.CustomNumericEx();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.txtAddWrite = new System.Windows.Forms.TextBox();
            this.rjButton4 = new BeeInterface.RJButton();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.txtAddRead = new System.Windows.Forms.TextBox();
            this.rjButton6 = new BeeInterface.RJButton();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.layInput = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.btnEnableEditInput = new BeeInterface.RJButton();
            this.label20 = new System.Windows.Forms.Label();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.layOutput = new System.Windows.Forms.TableLayoutPanel();
            this.layOut = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnEnableEditType = new BeeInterface.RJButton();
            this.label19 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnReadProgress = new BeeInterface.RJButton();
            this.txtValueProgress = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.txtAddProgress = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnReadQty = new BeeInterface.RJButton();
            this.txtValueQty = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.txtAddQty = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnReadCountProg = new BeeInterface.RJButton();
            this.txtValueCountProg = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtAddCountProg = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnReadPO = new BeeInterface.RJButton();
            this.txtValuePO = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtAddPO = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnReadProg = new BeeInterface.RJButton();
            this.txtProg = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtAddProg = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tmConnect = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.laySetting.SuspendLayout();
            this.pConnect.SuspendLayout();
            this.layTypeHard.SuspendLayout();
            this.layTypeComunication.SuspendLayout();
            this.layBrand.SuspendLayout();
            this.laySettingCom.SuspendLayout();
            this.layCom.SuspendLayout();
            this.layIP.SuspendLayout();
            this.layParameterBits.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.layOut.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(575, 840);
            this.tabControl1.TabIndex = 55;
            this.tabControl1.VisibleChanged += new System.EventHandler(this.tabControl1_VisibleChanged);
            // 
            // tabPage7
            // 
            this.tabPage7.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage7.Controls.Add(this.laySetting);
            this.tabPage7.Location = new System.Drawing.Point(4, 33);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.tabPage7.Size = new System.Drawing.Size(567, 803);
            this.tabPage7.TabIndex = 2;
            this.tabPage7.Text = "Setting";
            // 
            // laySetting
            // 
            this.laySetting.AutoScroll = true;
            this.laySetting.ColumnCount = 1;
            this.laySetting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.laySetting.Controls.Add(this.pConnect, 0, 7);
            this.laySetting.Controls.Add(this.btn4S, 0, 8);
            this.laySetting.Controls.Add(this.btn3S, 0, 4);
            this.laySetting.Controls.Add(this.btn2S, 0, 2);
            this.laySetting.Controls.Add(this.btn1S, 0, 0);
            this.laySetting.Controls.Add(this.layTypeHard, 0, 1);
            this.laySetting.Controls.Add(this.layTypeComunication, 0, 5);
            this.laySetting.Controls.Add(this.layBrand, 0, 3);
            this.laySetting.Controls.Add(this.laySettingCom, 0, 6);
            this.laySetting.Controls.Add(this.layParameterBits, 0, 10);
            this.laySetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.laySetting.Location = new System.Drawing.Point(3, 10);
            this.laySetting.Name = "laySetting";
            this.laySetting.Padding = new System.Windows.Forms.Padding(1);
            this.laySetting.RowCount = 13;
            this.laySetting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.laySetting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.laySetting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.laySetting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.laySetting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.laySetting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.laySetting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.laySetting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.laySetting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.laySetting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.laySetting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.laySetting.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.laySetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.laySetting.Size = new System.Drawing.Size(561, 790);
            this.laySetting.TabIndex = 80;
            // 
            // pConnect
            // 
            this.pConnect.Controls.Add(this.panel5);
            this.pConnect.Controls.Add(this.StatusIObtn);
            this.pConnect.Controls.Add(this.btnConectIO);
            this.pConnect.Controls.Add(this.panel2);
            this.pConnect.Controls.Add(this.btnBypass);
            this.pConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pConnect.Location = new System.Drawing.Point(4, 679);
            this.pConnect.Name = "pConnect";
            this.pConnect.Size = new System.Drawing.Size(543, 45);
            this.pConnect.TabIndex = 109;
            this.pConnect.Visible = false;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(269, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(10, 45);
            this.panel5.TabIndex = 74;
            // 
            // StatusIObtn
            // 
            this.StatusIObtn.AutoFont = true;
            this.StatusIObtn.AutoFontHeightRatio = 0.75F;
            this.StatusIObtn.AutoFontMax = 100F;
            this.StatusIObtn.AutoFontMin = 6F;
            this.StatusIObtn.AutoFontWidthRatio = 0.92F;
            this.StatusIObtn.AutoImage = true;
            this.StatusIObtn.AutoImageMaxRatio = 0.75F;
            this.StatusIObtn.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.StatusIObtn.AutoImageTint = true;
            this.StatusIObtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.StatusIObtn.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.StatusIObtn.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.StatusIObtn.BorderRadius = 5;
            this.StatusIObtn.BorderSize = 1;
            this.StatusIObtn.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.StatusIObtn.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.StatusIObtn.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.StatusIObtn.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.StatusIObtn.Corner = BeeGlobal.Corner.Both;
            this.StatusIObtn.DebounceResizeMs = 16;
            this.StatusIObtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StatusIObtn.FlatAppearance.BorderSize = 0;
            this.StatusIObtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StatusIObtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.StatusIObtn.ForeColor = System.Drawing.Color.Black;
            this.StatusIObtn.Image = null;
            this.StatusIObtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.StatusIObtn.ImageDisabled = null;
            this.StatusIObtn.ImageHover = null;
            this.StatusIObtn.ImageNormal = null;
            this.StatusIObtn.ImagePadding = new System.Windows.Forms.Padding(1);
            this.StatusIObtn.ImagePressed = null;
            this.StatusIObtn.ImageTextSpacing = 6;
            this.StatusIObtn.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.StatusIObtn.ImageTintHover = System.Drawing.Color.Empty;
            this.StatusIObtn.ImageTintNormal = System.Drawing.Color.Empty;
            this.StatusIObtn.ImageTintOpacity = 0.5F;
            this.StatusIObtn.ImageTintPressed = System.Drawing.Color.Empty;
            this.StatusIObtn.IsCLick = false;
            this.StatusIObtn.IsNotChange = false;
            this.StatusIObtn.IsRect = false;
            this.StatusIObtn.IsTouch = false;
            this.StatusIObtn.IsUnGroup = false;
            this.StatusIObtn.Location = new System.Drawing.Point(0, 0);
            this.StatusIObtn.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.StatusIObtn.Multiline = false;
            this.StatusIObtn.Name = "StatusIObtn";
            this.StatusIObtn.Size = new System.Drawing.Size(279, 45);
            this.StatusIObtn.TabIndex = 73;
            this.StatusIObtn.Text = "----";
            this.StatusIObtn.TextColor = System.Drawing.Color.Black;
            this.StatusIObtn.UseVisualStyleBackColor = false;
            // 
            // btnConectIO
            // 
            this.btnConectIO.AutoFont = true;
            this.btnConectIO.AutoFontHeightRatio = 0.75F;
            this.btnConectIO.AutoFontMax = 100F;
            this.btnConectIO.AutoFontMin = 6F;
            this.btnConectIO.AutoFontWidthRatio = 0.92F;
            this.btnConectIO.AutoImage = true;
            this.btnConectIO.AutoImageMaxRatio = 0.75F;
            this.btnConectIO.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnConectIO.AutoImageTint = true;
            this.btnConectIO.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnConectIO.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnConectIO.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnConectIO.BorderRadius = 5;
            this.btnConectIO.BorderSize = 1;
            this.btnConectIO.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnConectIO.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnConectIO.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnConectIO.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnConectIO.Corner = BeeGlobal.Corner.Both;
            this.btnConectIO.DebounceResizeMs = 16;
            this.btnConectIO.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnConectIO.FlatAppearance.BorderSize = 0;
            this.btnConectIO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConectIO.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnConectIO.ForeColor = System.Drawing.Color.Black;
            this.btnConectIO.Image = null;
            this.btnConectIO.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConectIO.ImageDisabled = null;
            this.btnConectIO.ImageHover = null;
            this.btnConectIO.ImageNormal = null;
            this.btnConectIO.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnConectIO.ImagePressed = null;
            this.btnConectIO.ImageTextSpacing = 6;
            this.btnConectIO.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnConectIO.ImageTintHover = System.Drawing.Color.Empty;
            this.btnConectIO.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnConectIO.ImageTintOpacity = 0.5F;
            this.btnConectIO.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnConectIO.IsCLick = false;
            this.btnConectIO.IsNotChange = false;
            this.btnConectIO.IsRect = false;
            this.btnConectIO.IsTouch = false;
            this.btnConectIO.IsUnGroup = false;
            this.btnConectIO.Location = new System.Drawing.Point(279, 0);
            this.btnConectIO.Multiline = false;
            this.btnConectIO.Name = "btnConectIO";
            this.btnConectIO.Size = new System.Drawing.Size(123, 45);
            this.btnConectIO.TabIndex = 68;
            this.btnConectIO.Text = "Connect";
            this.btnConectIO.TextColor = System.Drawing.Color.Black;
            this.btnConectIO.UseVisualStyleBackColor = false;
            this.btnConectIO.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(402, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 45);
            this.panel2.TabIndex = 70;
            // 
            // btnBypass
            // 
            this.btnBypass.AutoFont = true;
            this.btnBypass.AutoFontHeightRatio = 0.75F;
            this.btnBypass.AutoFontMax = 100F;
            this.btnBypass.AutoFontMin = 6F;
            this.btnBypass.AutoFontWidthRatio = 0.92F;
            this.btnBypass.AutoImage = true;
            this.btnBypass.AutoImageMaxRatio = 0.75F;
            this.btnBypass.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnBypass.AutoImageTint = true;
            this.btnBypass.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnBypass.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnBypass.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnBypass.BorderRadius = 5;
            this.btnBypass.BorderSize = 1;
            this.btnBypass.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnBypass.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnBypass.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnBypass.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnBypass.Corner = BeeGlobal.Corner.Both;
            this.btnBypass.DebounceResizeMs = 16;
            this.btnBypass.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnBypass.FlatAppearance.BorderSize = 0;
            this.btnBypass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBypass.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.34375F);
            this.btnBypass.ForeColor = System.Drawing.Color.Black;
            this.btnBypass.Image = null;
            this.btnBypass.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBypass.ImageDisabled = null;
            this.btnBypass.ImageHover = null;
            this.btnBypass.ImageNormal = null;
            this.btnBypass.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnBypass.ImagePressed = null;
            this.btnBypass.ImageTextSpacing = 6;
            this.btnBypass.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnBypass.ImageTintHover = System.Drawing.Color.Empty;
            this.btnBypass.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnBypass.ImageTintOpacity = 0.5F;
            this.btnBypass.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnBypass.IsCLick = false;
            this.btnBypass.IsNotChange = false;
            this.btnBypass.IsRect = false;
            this.btnBypass.IsTouch = false;
            this.btnBypass.IsUnGroup = false;
            this.btnBypass.Location = new System.Drawing.Point(412, 0);
            this.btnBypass.Multiline = false;
            this.btnBypass.Name = "btnBypass";
            this.btnBypass.Size = new System.Drawing.Size(131, 45);
            this.btnBypass.TabIndex = 69;
            this.btnBypass.Text = "DisConnect";
            this.btnBypass.TextColor = System.Drawing.Color.Black;
            this.btnBypass.UseVisualStyleBackColor = false;
            this.btnBypass.Click += new System.EventHandler(this.btnBypass_Click);
            // 
            // btn4S
            // 
            this.btn4S.AutoFont = true;
            this.btn4S.AutoFontHeightRatio = 0.75F;
            this.btn4S.AutoFontMax = 100F;
            this.btn4S.AutoFontMin = 14F;
            this.btn4S.AutoFontWidthRatio = 0.92F;
            this.btn4S.AutoImage = true;
            this.btn4S.AutoImageMaxRatio = 0.75F;
            this.btn4S.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btn4S.AutoImageTint = true;
            this.btn4S.BackColor = System.Drawing.Color.White;
            this.btn4S.BackgroundColor = System.Drawing.Color.White;
            this.btn4S.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn4S.BorderColor = System.Drawing.Color.White;
            this.btn4S.BorderRadius = 10;
            this.btn4S.BorderSize = 1;
            this.btn4S.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn4S.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn4S.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn4S.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btn4S.Corner = BeeGlobal.Corner.None;
            this.btn4S.DebounceResizeMs = 6;
            this.btn4S.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn4S.FlatAppearance.BorderSize = 0;
            this.btn4S.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn4S.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn4S.ForeColor = System.Drawing.Color.White;
            this.btn4S.Image = null;
            this.btn4S.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn4S.ImageDisabled = null;
            this.btn4S.ImageHover = null;
            this.btn4S.ImageNormal = null;
            this.btn4S.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btn4S.ImagePressed = null;
            this.btn4S.ImageTextSpacing = 6;
            this.btn4S.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btn4S.ImageTintHover = System.Drawing.Color.Empty;
            this.btn4S.ImageTintNormal = System.Drawing.Color.Empty;
            this.btn4S.ImageTintOpacity = 0.5F;
            this.btn4S.ImageTintPressed = System.Drawing.Color.Empty;
            this.btn4S.IsCLick = true;
            this.btn4S.IsNotChange = false;
            this.btn4S.IsRect = false;
            this.btn4S.IsTouch = true;
            this.btn4S.IsUnGroup = true;
            this.btn4S.Location = new System.Drawing.Point(6, 737);
            this.btn4S.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.btn4S.Multiline = false;
            this.btn4S.Name = "btn4S";
            this.btn4S.Size = new System.Drawing.Size(539, 35);
            this.btn4S.TabIndex = 107;
            this.btn4S.Text = "4.Config Parameters";
            this.btn4S.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn4S.TextColor = System.Drawing.Color.White;
            this.btn4S.UseVisualStyleBackColor = false;
            this.btn4S.Click += new System.EventHandler(this.btn4S_Click);
            // 
            // btn3S
            // 
            this.btn3S.AutoFont = true;
            this.btn3S.AutoFontHeightRatio = 0.75F;
            this.btn3S.AutoFontMax = 100F;
            this.btn3S.AutoFontMin = 14F;
            this.btn3S.AutoFontWidthRatio = 0.92F;
            this.btn3S.AutoImage = true;
            this.btn3S.AutoImageMaxRatio = 0.75F;
            this.btn3S.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btn3S.AutoImageTint = true;
            this.btn3S.BackColor = System.Drawing.Color.White;
            this.btn3S.BackgroundColor = System.Drawing.Color.White;
            this.btn3S.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn3S.BorderColor = System.Drawing.Color.White;
            this.btn3S.BorderRadius = 10;
            this.btn3S.BorderSize = 1;
            this.btn3S.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn3S.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn3S.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn3S.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btn3S.Corner = BeeGlobal.Corner.None;
            this.btn3S.DebounceResizeMs = 6;
            this.btn3S.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn3S.FlatAppearance.BorderSize = 0;
            this.btn3S.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn3S.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn3S.ForeColor = System.Drawing.Color.White;
            this.btn3S.Image = null;
            this.btn3S.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn3S.ImageDisabled = null;
            this.btn3S.ImageHover = null;
            this.btn3S.ImageNormal = null;
            this.btn3S.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btn3S.ImagePressed = null;
            this.btn3S.ImageTextSpacing = 6;
            this.btn3S.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btn3S.ImageTintHover = System.Drawing.Color.Empty;
            this.btn3S.ImageTintNormal = System.Drawing.Color.Empty;
            this.btn3S.ImageTintOpacity = 0.5F;
            this.btn3S.ImageTintPressed = System.Drawing.Color.Empty;
            this.btn3S.IsCLick = true;
            this.btn3S.IsNotChange = false;
            this.btn3S.IsRect = false;
            this.btn3S.IsTouch = true;
            this.btn3S.IsUnGroup = true;
            this.btn3S.Location = new System.Drawing.Point(6, 331);
            this.btn3S.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.btn3S.Multiline = false;
            this.btn3S.Name = "btn3S";
            this.btn3S.Size = new System.Drawing.Size(539, 35);
            this.btn3S.TabIndex = 106;
            this.btn3S.Text = "3.Config for Connect";
            this.btn3S.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn3S.TextColor = System.Drawing.Color.White;
            this.btn3S.UseVisualStyleBackColor = false;
            this.btn3S.Click += new System.EventHandler(this.btn3S_Click);
            // 
            // btn2S
            // 
            this.btn2S.AutoFont = true;
            this.btn2S.AutoFontHeightRatio = 0.75F;
            this.btn2S.AutoFontMax = 100F;
            this.btn2S.AutoFontMin = 14F;
            this.btn2S.AutoFontWidthRatio = 0.92F;
            this.btn2S.AutoImage = true;
            this.btn2S.AutoImageMaxRatio = 0.75F;
            this.btn2S.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btn2S.AutoImageTint = true;
            this.btn2S.BackColor = System.Drawing.Color.White;
            this.btn2S.BackgroundColor = System.Drawing.Color.White;
            this.btn2S.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn2S.BorderColor = System.Drawing.Color.White;
            this.btn2S.BorderRadius = 10;
            this.btn2S.BorderSize = 1;
            this.btn2S.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn2S.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn2S.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn2S.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btn2S.Corner = BeeGlobal.Corner.None;
            this.btn2S.DebounceResizeMs = 6;
            this.btn2S.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn2S.FlatAppearance.BorderSize = 0;
            this.btn2S.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn2S.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn2S.ForeColor = System.Drawing.Color.White;
            this.btn2S.Image = null;
            this.btn2S.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn2S.ImageDisabled = null;
            this.btn2S.ImageHover = null;
            this.btn2S.ImageNormal = null;
            this.btn2S.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btn2S.ImagePressed = null;
            this.btn2S.ImageTextSpacing = 6;
            this.btn2S.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btn2S.ImageTintHover = System.Drawing.Color.Empty;
            this.btn2S.ImageTintNormal = System.Drawing.Color.Empty;
            this.btn2S.ImageTintOpacity = 0.5F;
            this.btn2S.ImageTintPressed = System.Drawing.Color.Empty;
            this.btn2S.IsCLick = true;
            this.btn2S.IsNotChange = false;
            this.btn2S.IsRect = false;
            this.btn2S.IsTouch = true;
            this.btn2S.IsUnGroup = true;
            this.btn2S.Location = new System.Drawing.Point(6, 106);
            this.btn2S.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.btn2S.Multiline = false;
            this.btn2S.Name = "btn2S";
            this.btn2S.Size = new System.Drawing.Size(539, 35);
            this.btn2S.TabIndex = 105;
            this.btn2S.Text = "2. Brand";
            this.btn2S.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn2S.TextColor = System.Drawing.Color.White;
            this.btn2S.UseVisualStyleBackColor = false;
            this.btn2S.Click += new System.EventHandler(this.btn2S_Click);
            // 
            // btn1S
            // 
            this.btn1S.AutoFont = true;
            this.btn1S.AutoFontHeightRatio = 0.75F;
            this.btn1S.AutoFontMax = 100F;
            this.btn1S.AutoFontMin = 14F;
            this.btn1S.AutoFontWidthRatio = 0.92F;
            this.btn1S.AutoImage = true;
            this.btn1S.AutoImageMaxRatio = 0.75F;
            this.btn1S.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btn1S.AutoImageTint = true;
            this.btn1S.BackColor = System.Drawing.Color.White;
            this.btn1S.BackgroundColor = System.Drawing.Color.White;
            this.btn1S.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn1S.BorderColor = System.Drawing.Color.White;
            this.btn1S.BorderRadius = 10;
            this.btn1S.BorderSize = 1;
            this.btn1S.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn1S.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn1S.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btn1S.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btn1S.Corner = BeeGlobal.Corner.None;
            this.btn1S.DebounceResizeMs = 6;
            this.btn1S.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn1S.FlatAppearance.BorderSize = 0;
            this.btn1S.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn1S.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn1S.ForeColor = System.Drawing.Color.White;
            this.btn1S.Image = null;
            this.btn1S.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn1S.ImageDisabled = null;
            this.btn1S.ImageHover = null;
            this.btn1S.ImageNormal = null;
            this.btn1S.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btn1S.ImagePressed = null;
            this.btn1S.ImageTextSpacing = 6;
            this.btn1S.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btn1S.ImageTintHover = System.Drawing.Color.Empty;
            this.btn1S.ImageTintNormal = System.Drawing.Color.Empty;
            this.btn1S.ImageTintOpacity = 0.5F;
            this.btn1S.ImageTintPressed = System.Drawing.Color.Empty;
            this.btn1S.IsCLick = true;
            this.btn1S.IsNotChange = false;
            this.btn1S.IsRect = false;
            this.btn1S.IsTouch = true;
            this.btn1S.IsUnGroup = true;
            this.btn1S.Location = new System.Drawing.Point(6, 11);
            this.btn1S.Margin = new System.Windows.Forms.Padding(5, 10, 5, 0);
            this.btn1S.Multiline = false;
            this.btn1S.Name = "btn1S";
            this.btn1S.Size = new System.Drawing.Size(539, 35);
            this.btn1S.TabIndex = 104;
            this.btn1S.Text = "1.Type ";
            this.btn1S.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn1S.TextColor = System.Drawing.Color.White;
            this.btn1S.UseVisualStyleBackColor = false;
            this.btn1S.Click += new System.EventHandler(this.btn1S_Click);
            // 
            // layTypeHard
            // 
            this.layTypeHard.BackColor = System.Drawing.Color.White;
            this.layTypeHard.ColumnCount = 3;
            this.layTypeHard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layTypeHard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layTypeHard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layTypeHard.Controls.Add(this.btnPCICard, 2, 0);
            this.layTypeHard.Controls.Add(this.btnIO, 1, 0);
            this.layTypeHard.Controls.Add(this.btnIsPLC, 0, 0);
            this.layTypeHard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layTypeHard.Location = new System.Drawing.Point(6, 46);
            this.layTypeHard.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.layTypeHard.Name = "layTypeHard";
            this.layTypeHard.Padding = new System.Windows.Forms.Padding(5);
            this.layTypeHard.RowCount = 1;
            this.layTypeHard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layTypeHard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layTypeHard.Size = new System.Drawing.Size(539, 50);
            this.layTypeHard.TabIndex = 86;
            this.layTypeHard.Visible = false;
            // 
            // btnPCICard
            // 
            this.btnPCICard.AutoFont = true;
            this.btnPCICard.AutoFontHeightRatio = 0.75F;
            this.btnPCICard.AutoFontMax = 100F;
            this.btnPCICard.AutoFontMin = 6F;
            this.btnPCICard.AutoFontWidthRatio = 0.92F;
            this.btnPCICard.AutoImage = true;
            this.btnPCICard.AutoImageMaxRatio = 0.75F;
            this.btnPCICard.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnPCICard.AutoImageTint = true;
            this.btnPCICard.BackColor = System.Drawing.Color.White;
            this.btnPCICard.BackgroundColor = System.Drawing.Color.White;
            this.btnPCICard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPCICard.BorderColor = System.Drawing.Color.White;
            this.btnPCICard.BorderRadius = 10;
            this.btnPCICard.BorderSize = 1;
            this.btnPCICard.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnPCICard.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnPCICard.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnPCICard.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnPCICard.Corner = BeeGlobal.Corner.Right;
            this.btnPCICard.DebounceResizeMs = 16;
            this.btnPCICard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPCICard.FlatAppearance.BorderSize = 0;
            this.btnPCICard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPCICard.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnPCICard.ForeColor = System.Drawing.Color.Black;
            this.btnPCICard.Image = null;
            this.btnPCICard.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPCICard.ImageDisabled = null;
            this.btnPCICard.ImageHover = null;
            this.btnPCICard.ImageNormal = null;
            this.btnPCICard.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnPCICard.ImagePressed = null;
            this.btnPCICard.ImageTextSpacing = 6;
            this.btnPCICard.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnPCICard.ImageTintHover = System.Drawing.Color.Empty;
            this.btnPCICard.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnPCICard.ImageTintOpacity = 0.5F;
            this.btnPCICard.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnPCICard.IsCLick = false;
            this.btnPCICard.IsNotChange = false;
            this.btnPCICard.IsRect = false;
            this.btnPCICard.IsTouch = false;
            this.btnPCICard.IsUnGroup = false;
            this.btnPCICard.Location = new System.Drawing.Point(357, 5);
            this.btnPCICard.Margin = new System.Windows.Forms.Padding(0);
            this.btnPCICard.Multiline = false;
            this.btnPCICard.Name = "btnPCICard";
            this.btnPCICard.Size = new System.Drawing.Size(177, 40);
            this.btnPCICard.TabIndex = 5;
            this.btnPCICard.Text = "PCI CARD";
            this.btnPCICard.TextColor = System.Drawing.Color.Black;
            this.btnPCICard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPCICard.UseVisualStyleBackColor = false;
            this.btnPCICard.Click += new System.EventHandler(this.btnPCICard_Click);
            // 
            // btnIO
            // 
            this.btnIO.AutoFont = true;
            this.btnIO.AutoFontHeightRatio = 0.75F;
            this.btnIO.AutoFontMax = 100F;
            this.btnIO.AutoFontMin = 6F;
            this.btnIO.AutoFontWidthRatio = 0.92F;
            this.btnIO.AutoImage = true;
            this.btnIO.AutoImageMaxRatio = 0.75F;
            this.btnIO.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnIO.AutoImageTint = true;
            this.btnIO.BackColor = System.Drawing.Color.White;
            this.btnIO.BackgroundColor = System.Drawing.Color.White;
            this.btnIO.BorderColor = System.Drawing.Color.White;
            this.btnIO.BorderRadius = 10;
            this.btnIO.BorderSize = 1;
            this.btnIO.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnIO.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnIO.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnIO.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnIO.Corner = BeeGlobal.Corner.None;
            this.btnIO.DebounceResizeMs = 16;
            this.btnIO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnIO.FlatAppearance.BorderSize = 0;
            this.btnIO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIO.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnIO.ForeColor = System.Drawing.Color.Black;
            this.btnIO.Image = null;
            this.btnIO.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIO.ImageDisabled = null;
            this.btnIO.ImageHover = null;
            this.btnIO.ImageNormal = null;
            this.btnIO.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnIO.ImagePressed = null;
            this.btnIO.ImageTextSpacing = 6;
            this.btnIO.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnIO.ImageTintHover = System.Drawing.Color.Empty;
            this.btnIO.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnIO.ImageTintOpacity = 0.5F;
            this.btnIO.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnIO.IsCLick = false;
            this.btnIO.IsNotChange = false;
            this.btnIO.IsRect = false;
            this.btnIO.IsTouch = false;
            this.btnIO.IsUnGroup = false;
            this.btnIO.Location = new System.Drawing.Point(181, 5);
            this.btnIO.Margin = new System.Windows.Forms.Padding(0);
            this.btnIO.Multiline = false;
            this.btnIO.Name = "btnIO";
            this.btnIO.Size = new System.Drawing.Size(176, 40);
            this.btnIO.TabIndex = 3;
            this.btnIO.Text = "I/O";
            this.btnIO.TextColor = System.Drawing.Color.Black;
            this.btnIO.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnIO.UseVisualStyleBackColor = false;
            this.btnIO.Click += new System.EventHandler(this.btnIO_Click);
            // 
            // btnIsPLC
            // 
            this.btnIsPLC.AutoFont = true;
            this.btnIsPLC.AutoFontHeightRatio = 0.75F;
            this.btnIsPLC.AutoFontMax = 100F;
            this.btnIsPLC.AutoFontMin = 6F;
            this.btnIsPLC.AutoFontWidthRatio = 0.92F;
            this.btnIsPLC.AutoImage = true;
            this.btnIsPLC.AutoImageMaxRatio = 0.75F;
            this.btnIsPLC.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnIsPLC.AutoImageTint = true;
            this.btnIsPLC.BackColor = System.Drawing.Color.White;
            this.btnIsPLC.BackgroundColor = System.Drawing.Color.White;
            this.btnIsPLC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnIsPLC.BorderColor = System.Drawing.Color.White;
            this.btnIsPLC.BorderRadius = 10;
            this.btnIsPLC.BorderSize = 1;
            this.btnIsPLC.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnIsPLC.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnIsPLC.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnIsPLC.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnIsPLC.Corner = BeeGlobal.Corner.Left;
            this.btnIsPLC.DebounceResizeMs = 16;
            this.btnIsPLC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnIsPLC.FlatAppearance.BorderSize = 0;
            this.btnIsPLC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIsPLC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
            this.btnIsPLC.ForeColor = System.Drawing.Color.Black;
            this.btnIsPLC.Image = null;
            this.btnIsPLC.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIsPLC.ImageDisabled = null;
            this.btnIsPLC.ImageHover = null;
            this.btnIsPLC.ImageNormal = null;
            this.btnIsPLC.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnIsPLC.ImagePressed = null;
            this.btnIsPLC.ImageTextSpacing = 6;
            this.btnIsPLC.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnIsPLC.ImageTintHover = System.Drawing.Color.Empty;
            this.btnIsPLC.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnIsPLC.ImageTintOpacity = 0.5F;
            this.btnIsPLC.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnIsPLC.IsCLick = true;
            this.btnIsPLC.IsNotChange = false;
            this.btnIsPLC.IsRect = false;
            this.btnIsPLC.IsTouch = false;
            this.btnIsPLC.IsUnGroup = false;
            this.btnIsPLC.Location = new System.Drawing.Point(8, 5);
            this.btnIsPLC.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnIsPLC.Multiline = false;
            this.btnIsPLC.Name = "btnIsPLC";
            this.btnIsPLC.Size = new System.Drawing.Size(173, 40);
            this.btnIsPLC.TabIndex = 2;
            this.btnIsPLC.Text = "PLC";
            this.btnIsPLC.TextColor = System.Drawing.Color.Black;
            this.btnIsPLC.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnIsPLC.UseVisualStyleBackColor = false;
            this.btnIsPLC.Click += new System.EventHandler(this.btnIsPLC_Click);
            // 
            // layTypeComunication
            // 
            this.layTypeComunication.BackColor = System.Drawing.Color.White;
            this.layTypeComunication.ColumnCount = 2;
            this.layTypeComunication.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.layTypeComunication.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.layTypeComunication.Controls.Add(this.btnTCP, 0, 0);
            this.layTypeComunication.Controls.Add(this.btnSerial, 1, 0);
            this.layTypeComunication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layTypeComunication.Location = new System.Drawing.Point(6, 366);
            this.layTypeComunication.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.layTypeComunication.Name = "layTypeComunication";
            this.layTypeComunication.RowCount = 1;
            this.layTypeComunication.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layTypeComunication.Size = new System.Drawing.Size(539, 56);
            this.layTypeComunication.TabIndex = 3;
            this.layTypeComunication.Visible = false;
            // 
            // btnTCP
            // 
            this.btnTCP.AutoFont = true;
            this.btnTCP.AutoFontHeightRatio = 0.75F;
            this.btnTCP.AutoFontMax = 100F;
            this.btnTCP.AutoFontMin = 6F;
            this.btnTCP.AutoFontWidthRatio = 0.92F;
            this.btnTCP.AutoImage = true;
            this.btnTCP.AutoImageMaxRatio = 0.75F;
            this.btnTCP.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnTCP.AutoImageTint = true;
            this.btnTCP.BackColor = System.Drawing.Color.White;
            this.btnTCP.BackgroundColor = System.Drawing.Color.White;
            this.btnTCP.BorderColor = System.Drawing.Color.White;
            this.btnTCP.BorderRadius = 5;
            this.btnTCP.BorderSize = 1;
            this.btnTCP.ClickBotColor = System.Drawing.Color.White;
            this.btnTCP.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnTCP.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnTCP.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnTCP.Corner = BeeGlobal.Corner.None;
            this.btnTCP.DebounceResizeMs = 16;
            this.btnTCP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTCP.FlatAppearance.BorderSize = 0;
            this.btnTCP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTCP.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.48438F);
            this.btnTCP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnTCP.Image = null;
            this.btnTCP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTCP.ImageDisabled = null;
            this.btnTCP.ImageHover = null;
            this.btnTCP.ImageNormal = null;
            this.btnTCP.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnTCP.ImagePressed = null;
            this.btnTCP.ImageTextSpacing = 6;
            this.btnTCP.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnTCP.ImageTintHover = System.Drawing.Color.Empty;
            this.btnTCP.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnTCP.ImageTintOpacity = 0.5F;
            this.btnTCP.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnTCP.IsCLick = false;
            this.btnTCP.IsNotChange = false;
            this.btnTCP.IsRect = false;
            this.btnTCP.IsTouch = false;
            this.btnTCP.IsUnGroup = false;
            this.btnTCP.Location = new System.Drawing.Point(3, 5);
            this.btnTCP.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.btnTCP.Multiline = false;
            this.btnTCP.Name = "btnTCP";
            this.btnTCP.Size = new System.Drawing.Size(209, 51);
            this.btnTCP.TabIndex = 95;
            this.btnTCP.Text = "TCP/IP";
            this.btnTCP.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnTCP.UseVisualStyleBackColor = false;
            this.btnTCP.Click += new System.EventHandler(this.btnTCP_Click_1);
            // 
            // btnSerial
            // 
            this.btnSerial.AutoFont = true;
            this.btnSerial.AutoFontHeightRatio = 0.75F;
            this.btnSerial.AutoFontMax = 100F;
            this.btnSerial.AutoFontMin = 6F;
            this.btnSerial.AutoFontWidthRatio = 0.92F;
            this.btnSerial.AutoImage = true;
            this.btnSerial.AutoImageMaxRatio = 0.75F;
            this.btnSerial.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnSerial.AutoImageTint = true;
            this.btnSerial.BackColor = System.Drawing.Color.White;
            this.btnSerial.BackgroundColor = System.Drawing.Color.White;
            this.btnSerial.BorderColor = System.Drawing.Color.White;
            this.btnSerial.BorderRadius = 5;
            this.btnSerial.BorderSize = 1;
            this.btnSerial.ClickBotColor = System.Drawing.Color.White;
            this.btnSerial.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnSerial.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnSerial.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnSerial.Corner = BeeGlobal.Corner.None;
            this.btnSerial.DebounceResizeMs = 16;
            this.btnSerial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSerial.FlatAppearance.BorderSize = 0;
            this.btnSerial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSerial.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.48438F);
            this.btnSerial.ForeColor = System.Drawing.Color.Black;
            this.btnSerial.Image = null;
            this.btnSerial.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSerial.ImageDisabled = null;
            this.btnSerial.ImageHover = null;
            this.btnSerial.ImageNormal = null;
            this.btnSerial.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnSerial.ImagePressed = null;
            this.btnSerial.ImageTextSpacing = 6;
            this.btnSerial.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnSerial.ImageTintHover = System.Drawing.Color.Empty;
            this.btnSerial.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnSerial.ImageTintOpacity = 0.5F;
            this.btnSerial.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnSerial.IsCLick = true;
            this.btnSerial.IsNotChange = false;
            this.btnSerial.IsRect = false;
            this.btnSerial.IsTouch = false;
            this.btnSerial.IsUnGroup = false;
            this.btnSerial.Location = new System.Drawing.Point(217, 5);
            this.btnSerial.Margin = new System.Windows.Forms.Padding(2, 5, 3, 0);
            this.btnSerial.Multiline = false;
            this.btnSerial.Name = "btnSerial";
            this.btnSerial.Size = new System.Drawing.Size(319, 51);
            this.btnSerial.TabIndex = 96;
            this.btnSerial.Text = "Serial";
            this.btnSerial.TextColor = System.Drawing.Color.Black;
            this.btnSerial.UseVisualStyleBackColor = false;
            this.btnSerial.Click += new System.EventHandler(this.btnSerial_Click);
            // 
            // layBrand
            // 
            this.layBrand.BackColor = System.Drawing.Color.White;
            this.layBrand.ColumnCount = 3;
            this.layBrand.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layBrand.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.layBrand.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.layBrand.Controls.Add(this.btnDelta, 2, 2);
            this.layBrand.Controls.Add(this.rjButton2, 2, 1);
            this.layBrand.Controls.Add(this.btnMitsu2, 2, 0);
            this.layBrand.Controls.Add(this.btnMitsu, 1, 0);
            this.layBrand.Controls.Add(this.rjButton7, 1, 1);
            this.layBrand.Controls.Add(this.btnMisu3, 0, 1);
            this.layBrand.Controls.Add(this.btnKeyence, 0, 0);
            this.layBrand.Controls.Add(this.btnRtu, 0, 2);
            this.layBrand.Controls.Add(this.btnModbusAscii, 1, 2);
            this.layBrand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layBrand.Location = new System.Drawing.Point(6, 141);
            this.layBrand.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.layBrand.Name = "layBrand";
            this.layBrand.Padding = new System.Windows.Forms.Padding(1);
            this.layBrand.RowCount = 3;
            this.layBrand.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layBrand.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layBrand.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layBrand.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layBrand.Size = new System.Drawing.Size(544, 180);
            this.layBrand.TabIndex = 0;
            this.layBrand.Visible = false;
            this.layBrand.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel3_Paint);
            // 
            // btnDelta
            // 
            this.btnDelta.AutoFont = true;
            this.btnDelta.AutoFontHeightRatio = 0.75F;
            this.btnDelta.AutoFontMax = 100F;
            this.btnDelta.AutoFontMin = 6F;
            this.btnDelta.AutoFontWidthRatio = 0.92F;
            this.btnDelta.AutoImage = true;
            this.btnDelta.AutoImageMaxRatio = 0.75F;
            this.btnDelta.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnDelta.AutoImageTint = true;
            this.btnDelta.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnDelta.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnDelta.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnDelta.BorderRadius = 5;
            this.btnDelta.BorderSize = 1;
            this.btnDelta.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnDelta.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnDelta.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnDelta.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnDelta.Corner = BeeGlobal.Corner.Both;
            this.btnDelta.DebounceResizeMs = 16;
            this.btnDelta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelta.FlatAppearance.BorderSize = 0;
            this.btnDelta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelta.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.btnDelta.ForeColor = System.Drawing.Color.Teal;
            this.btnDelta.Image = null;
            this.btnDelta.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelta.ImageDisabled = null;
            this.btnDelta.ImageHover = null;
            this.btnDelta.ImageNormal = null;
            this.btnDelta.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnDelta.ImagePressed = null;
            this.btnDelta.ImageTextSpacing = 6;
            this.btnDelta.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnDelta.ImageTintHover = System.Drawing.Color.Empty;
            this.btnDelta.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnDelta.ImageTintOpacity = 0.5F;
            this.btnDelta.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnDelta.IsCLick = false;
            this.btnDelta.IsNotChange = false;
            this.btnDelta.IsRect = false;
            this.btnDelta.IsTouch = false;
            this.btnDelta.IsUnGroup = false;
            this.btnDelta.Location = new System.Drawing.Point(366, 126);
            this.btnDelta.Margin = new System.Windows.Forms.Padding(5);
            this.btnDelta.Multiline = false;
            this.btnDelta.Name = "btnDelta";
            this.btnDelta.Size = new System.Drawing.Size(172, 50);
            this.btnDelta.TabIndex = 112;
            this.btnDelta.Text = "Delta";
            this.btnDelta.TextColor = System.Drawing.Color.Teal;
            this.btnDelta.UseVisualStyleBackColor = false;
            this.btnDelta.Click += new System.EventHandler(this.btnDelta_Click);
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
            this.rjButton2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton2.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton2.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton2.BorderRadius = 5;
            this.rjButton2.BorderSize = 1;
            this.rjButton2.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.rjButton2.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.rjButton2.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.rjButton2.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton2.Corner = BeeGlobal.Corner.Both;
            this.rjButton2.DebounceResizeMs = 16;
            this.rjButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton2.Enabled = false;
            this.rjButton2.FlatAppearance.BorderSize = 0;
            this.rjButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.rjButton2.ForeColor = System.Drawing.Color.Teal;
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
            this.rjButton2.Location = new System.Drawing.Point(366, 66);
            this.rjButton2.Margin = new System.Windows.Forms.Padding(5);
            this.rjButton2.Multiline = false;
            this.rjButton2.Name = "rjButton2";
            this.rjButton2.Size = new System.Drawing.Size(172, 50);
            this.rjButton2.TabIndex = 111;
            this.rjButton2.Text = "Panasonic";
            this.rjButton2.TextColor = System.Drawing.Color.Teal;
            this.rjButton2.UseVisualStyleBackColor = false;
            // 
            // btnMitsu2
            // 
            this.btnMitsu2.AutoFont = true;
            this.btnMitsu2.AutoFontHeightRatio = 0.75F;
            this.btnMitsu2.AutoFontMax = 100F;
            this.btnMitsu2.AutoFontMin = 6F;
            this.btnMitsu2.AutoFontWidthRatio = 0.92F;
            this.btnMitsu2.AutoImage = true;
            this.btnMitsu2.AutoImageMaxRatio = 0.75F;
            this.btnMitsu2.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMitsu2.AutoImageTint = true;
            this.btnMitsu2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnMitsu2.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnMitsu2.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnMitsu2.BorderRadius = 5;
            this.btnMitsu2.BorderSize = 1;
            this.btnMitsu2.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnMitsu2.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnMitsu2.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnMitsu2.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMitsu2.Corner = BeeGlobal.Corner.Both;
            this.btnMitsu2.DebounceResizeMs = 16;
            this.btnMitsu2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMitsu2.FlatAppearance.BorderSize = 0;
            this.btnMitsu2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMitsu2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.btnMitsu2.ForeColor = System.Drawing.Color.Teal;
            this.btnMitsu2.Image = null;
            this.btnMitsu2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMitsu2.ImageDisabled = null;
            this.btnMitsu2.ImageHover = null;
            this.btnMitsu2.ImageNormal = null;
            this.btnMitsu2.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnMitsu2.ImagePressed = null;
            this.btnMitsu2.ImageTextSpacing = 6;
            this.btnMitsu2.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnMitsu2.ImageTintHover = System.Drawing.Color.Empty;
            this.btnMitsu2.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnMitsu2.ImageTintOpacity = 0.5F;
            this.btnMitsu2.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnMitsu2.IsCLick = false;
            this.btnMitsu2.IsNotChange = false;
            this.btnMitsu2.IsRect = false;
            this.btnMitsu2.IsTouch = false;
            this.btnMitsu2.IsUnGroup = false;
            this.btnMitsu2.Location = new System.Drawing.Point(366, 6);
            this.btnMitsu2.Margin = new System.Windows.Forms.Padding(5);
            this.btnMitsu2.Multiline = false;
            this.btnMitsu2.Name = "btnMitsu2";
            this.btnMitsu2.Size = new System.Drawing.Size(172, 50);
            this.btnMitsu2.TabIndex = 110;
            this.btnMitsu2.Text = "Mitsubishi3U";
            this.btnMitsu2.TextColor = System.Drawing.Color.Teal;
            this.btnMitsu2.UseVisualStyleBackColor = false;
            this.btnMitsu2.Click += new System.EventHandler(this.btnMitsu2_Click);
            // 
            // btnMitsu
            // 
            this.btnMitsu.AutoFont = true;
            this.btnMitsu.AutoFontHeightRatio = 0.75F;
            this.btnMitsu.AutoFontMax = 100F;
            this.btnMitsu.AutoFontMin = 6F;
            this.btnMitsu.AutoFontWidthRatio = 0.92F;
            this.btnMitsu.AutoImage = true;
            this.btnMitsu.AutoImageMaxRatio = 0.75F;
            this.btnMitsu.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMitsu.AutoImageTint = true;
            this.btnMitsu.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnMitsu.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnMitsu.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnMitsu.BorderRadius = 5;
            this.btnMitsu.BorderSize = 1;
            this.btnMitsu.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnMitsu.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnMitsu.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnMitsu.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMitsu.Corner = BeeGlobal.Corner.Both;
            this.btnMitsu.DebounceResizeMs = 16;
            this.btnMitsu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMitsu.FlatAppearance.BorderSize = 0;
            this.btnMitsu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMitsu.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.btnMitsu.ForeColor = System.Drawing.Color.Teal;
            this.btnMitsu.Image = null;
            this.btnMitsu.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMitsu.ImageDisabled = null;
            this.btnMitsu.ImageHover = null;
            this.btnMitsu.ImageNormal = null;
            this.btnMitsu.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnMitsu.ImagePressed = null;
            this.btnMitsu.ImageTextSpacing = 6;
            this.btnMitsu.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnMitsu.ImageTintHover = System.Drawing.Color.Empty;
            this.btnMitsu.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnMitsu.ImageTintOpacity = 0.5F;
            this.btnMitsu.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnMitsu.IsCLick = true;
            this.btnMitsu.IsNotChange = false;
            this.btnMitsu.IsRect = false;
            this.btnMitsu.IsTouch = false;
            this.btnMitsu.IsUnGroup = false;
            this.btnMitsu.Location = new System.Drawing.Point(186, 6);
            this.btnMitsu.Margin = new System.Windows.Forms.Padding(5);
            this.btnMitsu.Multiline = false;
            this.btnMitsu.Name = "btnMitsu";
            this.btnMitsu.Size = new System.Drawing.Size(170, 50);
            this.btnMitsu.TabIndex = 109;
            this.btnMitsu.Text = "Mitsubishi5U";
            this.btnMitsu.TextColor = System.Drawing.Color.Teal;
            this.btnMitsu.UseVisualStyleBackColor = false;
            // 
            // rjButton7
            // 
            this.rjButton7.AutoFont = true;
            this.rjButton7.AutoFontHeightRatio = 0.75F;
            this.rjButton7.AutoFontMax = 100F;
            this.rjButton7.AutoFontMin = 6F;
            this.rjButton7.AutoFontWidthRatio = 0.92F;
            this.rjButton7.AutoImage = true;
            this.rjButton7.AutoImageMaxRatio = 0.75F;
            this.rjButton7.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton7.AutoImageTint = true;
            this.rjButton7.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton7.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton7.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.rjButton7.BorderRadius = 5;
            this.rjButton7.BorderSize = 1;
            this.rjButton7.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.rjButton7.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.rjButton7.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.rjButton7.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton7.Corner = BeeGlobal.Corner.Both;
            this.rjButton7.DebounceResizeMs = 16;
            this.rjButton7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton7.Enabled = false;
            this.rjButton7.FlatAppearance.BorderSize = 0;
            this.rjButton7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.rjButton7.ForeColor = System.Drawing.Color.Teal;
            this.rjButton7.Image = null;
            this.rjButton7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton7.ImageDisabled = null;
            this.rjButton7.ImageHover = null;
            this.rjButton7.ImageNormal = null;
            this.rjButton7.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton7.ImagePressed = null;
            this.rjButton7.ImageTextSpacing = 6;
            this.rjButton7.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton7.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton7.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton7.ImageTintOpacity = 0.5F;
            this.rjButton7.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton7.IsCLick = false;
            this.rjButton7.IsNotChange = false;
            this.rjButton7.IsRect = false;
            this.rjButton7.IsTouch = false;
            this.rjButton7.IsUnGroup = false;
            this.rjButton7.Location = new System.Drawing.Point(186, 66);
            this.rjButton7.Margin = new System.Windows.Forms.Padding(5);
            this.rjButton7.Multiline = false;
            this.rjButton7.Name = "rjButton7";
            this.rjButton7.Size = new System.Drawing.Size(170, 50);
            this.rjButton7.TabIndex = 103;
            this.rjButton7.Text = "Seimen";
            this.rjButton7.TextColor = System.Drawing.Color.Teal;
            this.rjButton7.UseVisualStyleBackColor = false;
            // 
            // btnMisu3
            // 
            this.btnMisu3.AutoFont = true;
            this.btnMisu3.AutoFontHeightRatio = 0.75F;
            this.btnMisu3.AutoFontMax = 100F;
            this.btnMisu3.AutoFontMin = 6F;
            this.btnMisu3.AutoFontWidthRatio = 0.92F;
            this.btnMisu3.AutoImage = true;
            this.btnMisu3.AutoImageMaxRatio = 0.75F;
            this.btnMisu3.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnMisu3.AutoImageTint = true;
            this.btnMisu3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnMisu3.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnMisu3.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnMisu3.BorderRadius = 5;
            this.btnMisu3.BorderSize = 1;
            this.btnMisu3.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnMisu3.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnMisu3.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnMisu3.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnMisu3.Corner = BeeGlobal.Corner.Both;
            this.btnMisu3.DebounceResizeMs = 16;
            this.btnMisu3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMisu3.FlatAppearance.BorderSize = 0;
            this.btnMisu3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMisu3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.btnMisu3.ForeColor = System.Drawing.Color.Teal;
            this.btnMisu3.Image = null;
            this.btnMisu3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMisu3.ImageDisabled = null;
            this.btnMisu3.ImageHover = null;
            this.btnMisu3.ImageNormal = null;
            this.btnMisu3.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnMisu3.ImagePressed = null;
            this.btnMisu3.ImageTextSpacing = 6;
            this.btnMisu3.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnMisu3.ImageTintHover = System.Drawing.Color.Empty;
            this.btnMisu3.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnMisu3.ImageTintOpacity = 0.5F;
            this.btnMisu3.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnMisu3.IsCLick = false;
            this.btnMisu3.IsNotChange = false;
            this.btnMisu3.IsRect = false;
            this.btnMisu3.IsTouch = false;
            this.btnMisu3.IsUnGroup = false;
            this.btnMisu3.Location = new System.Drawing.Point(6, 66);
            this.btnMisu3.Margin = new System.Windows.Forms.Padding(5);
            this.btnMisu3.Multiline = false;
            this.btnMisu3.Name = "btnMisu3";
            this.btnMisu3.Size = new System.Drawing.Size(170, 50);
            this.btnMisu3.TabIndex = 102;
            this.btnMisu3.Text = "Mitsubish Serial";
            this.btnMisu3.TextColor = System.Drawing.Color.Teal;
            this.btnMisu3.UseVisualStyleBackColor = false;
            this.btnMisu3.Click += new System.EventHandler(this.btnMisu3_Click);
            // 
            // btnKeyence
            // 
            this.btnKeyence.AutoFont = true;
            this.btnKeyence.AutoFontHeightRatio = 0.75F;
            this.btnKeyence.AutoFontMax = 100F;
            this.btnKeyence.AutoFontMin = 6F;
            this.btnKeyence.AutoFontWidthRatio = 0.92F;
            this.btnKeyence.AutoImage = true;
            this.btnKeyence.AutoImageMaxRatio = 0.75F;
            this.btnKeyence.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnKeyence.AutoImageTint = true;
            this.btnKeyence.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnKeyence.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnKeyence.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnKeyence.BorderRadius = 5;
            this.btnKeyence.BorderSize = 1;
            this.btnKeyence.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnKeyence.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnKeyence.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnKeyence.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnKeyence.Corner = BeeGlobal.Corner.Both;
            this.btnKeyence.DebounceResizeMs = 16;
            this.btnKeyence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnKeyence.FlatAppearance.BorderSize = 0;
            this.btnKeyence.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyence.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.btnKeyence.ForeColor = System.Drawing.Color.Teal;
            this.btnKeyence.Image = null;
            this.btnKeyence.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnKeyence.ImageDisabled = null;
            this.btnKeyence.ImageHover = null;
            this.btnKeyence.ImageNormal = null;
            this.btnKeyence.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnKeyence.ImagePressed = null;
            this.btnKeyence.ImageTextSpacing = 6;
            this.btnKeyence.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnKeyence.ImageTintHover = System.Drawing.Color.Empty;
            this.btnKeyence.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnKeyence.ImageTintOpacity = 0.5F;
            this.btnKeyence.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnKeyence.IsCLick = false;
            this.btnKeyence.IsNotChange = false;
            this.btnKeyence.IsRect = false;
            this.btnKeyence.IsTouch = false;
            this.btnKeyence.IsUnGroup = false;
            this.btnKeyence.Location = new System.Drawing.Point(6, 6);
            this.btnKeyence.Margin = new System.Windows.Forms.Padding(5);
            this.btnKeyence.Multiline = false;
            this.btnKeyence.Name = "btnKeyence";
            this.btnKeyence.Size = new System.Drawing.Size(170, 50);
            this.btnKeyence.TabIndex = 99;
            this.btnKeyence.Text = "Keyence";
            this.btnKeyence.TextColor = System.Drawing.Color.Teal;
            this.btnKeyence.UseVisualStyleBackColor = false;
            this.btnKeyence.Click += new System.EventHandler(this.btnKeyence_Click);
            // 
            // btnRtu
            // 
            this.btnRtu.AutoFont = true;
            this.btnRtu.AutoFontHeightRatio = 0.75F;
            this.btnRtu.AutoFontMax = 100F;
            this.btnRtu.AutoFontMin = 6F;
            this.btnRtu.AutoFontWidthRatio = 0.92F;
            this.btnRtu.AutoImage = true;
            this.btnRtu.AutoImageMaxRatio = 0.75F;
            this.btnRtu.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnRtu.AutoImageTint = true;
            this.btnRtu.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnRtu.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnRtu.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnRtu.BorderRadius = 5;
            this.btnRtu.BorderSize = 1;
            this.btnRtu.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnRtu.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnRtu.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnRtu.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnRtu.Corner = BeeGlobal.Corner.Both;
            this.btnRtu.DebounceResizeMs = 16;
            this.btnRtu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRtu.FlatAppearance.BorderSize = 0;
            this.btnRtu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRtu.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.btnRtu.ForeColor = System.Drawing.Color.Blue;
            this.btnRtu.Image = null;
            this.btnRtu.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRtu.ImageDisabled = null;
            this.btnRtu.ImageHover = null;
            this.btnRtu.ImageNormal = null;
            this.btnRtu.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnRtu.ImagePressed = null;
            this.btnRtu.ImageTextSpacing = 6;
            this.btnRtu.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnRtu.ImageTintHover = System.Drawing.Color.Empty;
            this.btnRtu.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnRtu.ImageTintOpacity = 0.5F;
            this.btnRtu.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnRtu.IsCLick = false;
            this.btnRtu.IsNotChange = false;
            this.btnRtu.IsRect = false;
            this.btnRtu.IsTouch = false;
            this.btnRtu.IsUnGroup = false;
            this.btnRtu.Location = new System.Drawing.Point(6, 126);
            this.btnRtu.Margin = new System.Windows.Forms.Padding(5);
            this.btnRtu.Multiline = false;
            this.btnRtu.Name = "btnRtu";
            this.btnRtu.Size = new System.Drawing.Size(170, 50);
            this.btnRtu.TabIndex = 104;
            this.btnRtu.Text = "Modbus-RTU";
            this.btnRtu.TextColor = System.Drawing.Color.Blue;
            this.btnRtu.UseVisualStyleBackColor = false;
            this.btnRtu.Click += new System.EventHandler(this.btnRS485_Click);
            // 
            // btnModbusAscii
            // 
            this.btnModbusAscii.AutoFont = true;
            this.btnModbusAscii.AutoFontHeightRatio = 0.75F;
            this.btnModbusAscii.AutoFontMax = 100F;
            this.btnModbusAscii.AutoFontMin = 6F;
            this.btnModbusAscii.AutoFontWidthRatio = 0.92F;
            this.btnModbusAscii.AutoImage = true;
            this.btnModbusAscii.AutoImageMaxRatio = 0.75F;
            this.btnModbusAscii.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnModbusAscii.AutoImageTint = true;
            this.btnModbusAscii.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnModbusAscii.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnModbusAscii.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnModbusAscii.BorderRadius = 5;
            this.btnModbusAscii.BorderSize = 1;
            this.btnModbusAscii.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnModbusAscii.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnModbusAscii.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnModbusAscii.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnModbusAscii.Corner = BeeGlobal.Corner.Both;
            this.btnModbusAscii.DebounceResizeMs = 16;
            this.btnModbusAscii.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnModbusAscii.FlatAppearance.BorderSize = 0;
            this.btnModbusAscii.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModbusAscii.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.28125F);
            this.btnModbusAscii.ForeColor = System.Drawing.Color.Blue;
            this.btnModbusAscii.Image = null;
            this.btnModbusAscii.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModbusAscii.ImageDisabled = null;
            this.btnModbusAscii.ImageHover = null;
            this.btnModbusAscii.ImageNormal = null;
            this.btnModbusAscii.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnModbusAscii.ImagePressed = null;
            this.btnModbusAscii.ImageTextSpacing = 6;
            this.btnModbusAscii.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnModbusAscii.ImageTintHover = System.Drawing.Color.Empty;
            this.btnModbusAscii.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnModbusAscii.ImageTintOpacity = 0.5F;
            this.btnModbusAscii.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnModbusAscii.IsCLick = false;
            this.btnModbusAscii.IsNotChange = false;
            this.btnModbusAscii.IsRect = false;
            this.btnModbusAscii.IsTouch = false;
            this.btnModbusAscii.IsUnGroup = false;
            this.btnModbusAscii.Location = new System.Drawing.Point(186, 126);
            this.btnModbusAscii.Margin = new System.Windows.Forms.Padding(5);
            this.btnModbusAscii.Multiline = false;
            this.btnModbusAscii.Name = "btnModbusAscii";
            this.btnModbusAscii.Size = new System.Drawing.Size(170, 50);
            this.btnModbusAscii.TabIndex = 105;
            this.btnModbusAscii.Text = "Modbus-ASII";
            this.btnModbusAscii.TextColor = System.Drawing.Color.Blue;
            this.btnModbusAscii.UseVisualStyleBackColor = false;
            this.btnModbusAscii.Click += new System.EventHandler(this.btnModbusASII_Click);
            // 
            // laySettingCom
            // 
            this.laySettingCom.BackColor = System.Drawing.Color.White;
            this.laySettingCom.ColumnCount = 2;
            this.laySettingCom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.laySettingCom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.laySettingCom.Controls.Add(this.layCom, 1, 0);
            this.laySettingCom.Controls.Add(this.layIP, 0, 0);
            this.laySettingCom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.laySettingCom.Location = new System.Drawing.Point(6, 422);
            this.laySettingCom.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.laySettingCom.Name = "laySettingCom";
            this.laySettingCom.RowCount = 1;
            this.laySettingCom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.laySettingCom.Size = new System.Drawing.Size(539, 254);
            this.laySettingCom.TabIndex = 4;
            this.laySettingCom.Visible = false;
            // 
            // layCom
            // 
            this.layCom.BackColor = System.Drawing.Color.White;
            this.layCom.ColumnCount = 2;
            this.layCom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layCom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layCom.Controls.Add(this.numSlaveID, 1, 6);
            this.layCom.Controls.Add(this.label4, 0, 6);
            this.layCom.Controls.Add(this.btnRtsEnable, 1, 5);
            this.layCom.Controls.Add(this.btnDtrEnable, 1, 4);
            this.layCom.Controls.Add(this.cbStopBits, 1, 3);
            this.layCom.Controls.Add(this.cbParity, 0, 3);
            this.layCom.Controls.Add(this.label8, 0, 2);
            this.layCom.Controls.Add(this.cbBaurate, 1, 1);
            this.layCom.Controls.Add(this.lbRTU2, 1, 0);
            this.layCom.Controls.Add(this.cbCom, 0, 1);
            this.layCom.Controls.Add(this.lbRTU1, 0, 0);
            this.layCom.Controls.Add(this.label7, 1, 2);
            this.layCom.Controls.Add(this.label9, 0, 4);
            this.layCom.Controls.Add(this.cbDataBits, 0, 5);
            this.layCom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layCom.Location = new System.Drawing.Point(217, 0);
            this.layCom.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.layCom.Name = "layCom";
            this.layCom.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.layCom.RowCount = 7;
            this.layCom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layCom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layCom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layCom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layCom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layCom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layCom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layCom.Size = new System.Drawing.Size(322, 254);
            this.layCom.TabIndex = 1;
            this.layCom.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel7_Paint);
            // 
            // numSlaveID
            // 
            this.numSlaveID.AutoShowTextbox = false;
            this.numSlaveID.AutoSizeTextbox = true;
            this.numSlaveID.BackColor = System.Drawing.Color.White;
            this.numSlaveID.BorderColor = System.Drawing.Color.Transparent;
            this.numSlaveID.BorderRadius = 20;
            this.numSlaveID.ButtonMaxSize = 64;
            this.numSlaveID.ButtonMinSize = 24;
            this.numSlaveID.Decimals = 0;
            this.numSlaveID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numSlaveID.ElementGap = 3;
            this.numSlaveID.FillTextboxToAvailable = true;
            this.numSlaveID.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numSlaveID.InnerPadding = new System.Windows.Forms.Padding(6);
            this.numSlaveID.KeyboardStep = 1F;
            this.numSlaveID.Location = new System.Drawing.Point(164, 199);
            this.numSlaveID.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.numSlaveID.Max = 254F;
            this.numSlaveID.MaxTextboxWidth = 0;
            this.numSlaveID.Min = 1F;
            this.numSlaveID.MinimumSize = new System.Drawing.Size(120, 32);
            this.numSlaveID.MinTextboxWidth = 30;
            this.numSlaveID.Name = "numSlaveID";
            this.numSlaveID.Size = new System.Drawing.Size(153, 55);
            this.numSlaveID.SnapToStep = true;
            this.numSlaveID.StartWithTextboxHidden = false;
            this.numSlaveID.Step = 1F;
            this.numSlaveID.TabIndex = 129;
            this.numSlaveID.TextboxFontSize = 16F;
            this.numSlaveID.TextboxSidePadding = 12;
            this.numSlaveID.TextboxWidth = 56;
            this.numSlaveID.UnitText = "";
            this.numSlaveID.Value = 1F;
            this.numSlaveID.WheelStep = 1F;
            this.numSlaveID.ValueChanged += new System.Action<float>(this.numSlaveID_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(8, 199);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(153, 55);
            this.label4.TabIndex = 128;
            this.label4.Text = "Slave ID";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRtsEnable
            // 
            this.btnRtsEnable.AutoFont = true;
            this.btnRtsEnable.AutoFontHeightRatio = 0.75F;
            this.btnRtsEnable.AutoFontMax = 100F;
            this.btnRtsEnable.AutoFontMin = 6F;
            this.btnRtsEnable.AutoFontWidthRatio = 0.92F;
            this.btnRtsEnable.AutoImage = true;
            this.btnRtsEnable.AutoImageMaxRatio = 0.75F;
            this.btnRtsEnable.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnRtsEnable.AutoImageTint = true;
            this.btnRtsEnable.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnRtsEnable.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnRtsEnable.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnRtsEnable.BorderRadius = 5;
            this.btnRtsEnable.BorderSize = 1;
            this.btnRtsEnable.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnRtsEnable.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnRtsEnable.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnRtsEnable.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnRtsEnable.Corner = BeeGlobal.Corner.Both;
            this.btnRtsEnable.DebounceResizeMs = 16;
            this.btnRtsEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRtsEnable.FlatAppearance.BorderSize = 0;
            this.btnRtsEnable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRtsEnable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.203125F, System.Drawing.FontStyle.Bold);
            this.btnRtsEnable.ForeColor = System.Drawing.Color.DimGray;
            this.btnRtsEnable.Image = null;
            this.btnRtsEnable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRtsEnable.ImageDisabled = null;
            this.btnRtsEnable.ImageHover = null;
            this.btnRtsEnable.ImageNormal = null;
            this.btnRtsEnable.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnRtsEnable.ImagePressed = null;
            this.btnRtsEnable.ImageTextSpacing = 6;
            this.btnRtsEnable.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnRtsEnable.ImageTintHover = System.Drawing.Color.Empty;
            this.btnRtsEnable.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnRtsEnable.ImageTintOpacity = 0.5F;
            this.btnRtsEnable.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnRtsEnable.IsCLick = true;
            this.btnRtsEnable.IsNotChange = false;
            this.btnRtsEnable.IsRect = false;
            this.btnRtsEnable.IsTouch = false;
            this.btnRtsEnable.IsUnGroup = true;
            this.btnRtsEnable.Location = new System.Drawing.Point(166, 163);
            this.btnRtsEnable.Margin = new System.Windows.Forms.Padding(5);
            this.btnRtsEnable.Multiline = false;
            this.btnRtsEnable.Name = "btnRtsEnable";
            this.btnRtsEnable.Size = new System.Drawing.Size(149, 31);
            this.btnRtsEnable.TabIndex = 127;
            this.btnRtsEnable.Text = "RtsEnable";
            this.btnRtsEnable.TextColor = System.Drawing.Color.DimGray;
            this.btnRtsEnable.UseVisualStyleBackColor = false;
            this.btnRtsEnable.Click += new System.EventHandler(this.btnRtsEnable_Click);
            // 
            // btnDtrEnable
            // 
            this.btnDtrEnable.AutoFont = true;
            this.btnDtrEnable.AutoFontHeightRatio = 0.75F;
            this.btnDtrEnable.AutoFontMax = 100F;
            this.btnDtrEnable.AutoFontMin = 6F;
            this.btnDtrEnable.AutoFontWidthRatio = 0.92F;
            this.btnDtrEnable.AutoImage = true;
            this.btnDtrEnable.AutoImageMaxRatio = 0.75F;
            this.btnDtrEnable.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnDtrEnable.AutoImageTint = true;
            this.btnDtrEnable.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnDtrEnable.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnDtrEnable.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnDtrEnable.BorderRadius = 5;
            this.btnDtrEnable.BorderSize = 1;
            this.btnDtrEnable.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnDtrEnable.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnDtrEnable.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnDtrEnable.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnDtrEnable.Corner = BeeGlobal.Corner.Both;
            this.btnDtrEnable.DebounceResizeMs = 16;
            this.btnDtrEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDtrEnable.FlatAppearance.BorderSize = 0;
            this.btnDtrEnable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDtrEnable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.203125F, System.Drawing.FontStyle.Bold);
            this.btnDtrEnable.ForeColor = System.Drawing.Color.DimGray;
            this.btnDtrEnable.Image = null;
            this.btnDtrEnable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDtrEnable.ImageDisabled = null;
            this.btnDtrEnable.ImageHover = null;
            this.btnDtrEnable.ImageNormal = null;
            this.btnDtrEnable.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnDtrEnable.ImagePressed = null;
            this.btnDtrEnable.ImageTextSpacing = 6;
            this.btnDtrEnable.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnDtrEnable.ImageTintHover = System.Drawing.Color.Empty;
            this.btnDtrEnable.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnDtrEnable.ImageTintOpacity = 0.5F;
            this.btnDtrEnable.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnDtrEnable.IsCLick = true;
            this.btnDtrEnable.IsNotChange = false;
            this.btnDtrEnable.IsRect = false;
            this.btnDtrEnable.IsTouch = false;
            this.btnDtrEnable.IsUnGroup = true;
            this.btnDtrEnable.Location = new System.Drawing.Point(166, 122);
            this.btnDtrEnable.Margin = new System.Windows.Forms.Padding(5);
            this.btnDtrEnable.Multiline = false;
            this.btnDtrEnable.Name = "btnDtrEnable";
            this.btnDtrEnable.Size = new System.Drawing.Size(149, 31);
            this.btnDtrEnable.TabIndex = 126;
            this.btnDtrEnable.Text = "DtrEnable";
            this.btnDtrEnable.TextColor = System.Drawing.Color.DimGray;
            this.btnDtrEnable.UseVisualStyleBackColor = false;
            this.btnDtrEnable.Click += new System.EventHandler(this.btnDtrEnable_Click);
            // 
            // cbStopBits
            // 
            this.cbStopBits.BackColor = System.Drawing.Color.Wheat;
            this.cbStopBits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbStopBits.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbStopBits.FormattingEnabled = true;
            this.cbStopBits.Items.AddRange(new object[] {
            "Bee"});
            this.cbStopBits.Location = new System.Drawing.Point(164, 85);
            this.cbStopBits.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.cbStopBits.Name = "cbStopBits";
            this.cbStopBits.Size = new System.Drawing.Size(153, 32);
            this.cbStopBits.TabIndex = 124;
            this.cbStopBits.SelectionChangeCommitted += new System.EventHandler(this.cbStopBits_SelectionChangeCommitted);
            // 
            // cbParity
            // 
            this.cbParity.BackColor = System.Drawing.Color.Wheat;
            this.cbParity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbParity.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbParity.FormattingEnabled = true;
            this.cbParity.Items.AddRange(new object[] {
            "Bee"});
            this.cbParity.Location = new System.Drawing.Point(4, 85);
            this.cbParity.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cbParity.Name = "cbParity";
            this.cbParity.Size = new System.Drawing.Size(155, 32);
            this.cbParity.TabIndex = 123;
            this.cbParity.SelectionChangeCommitted += new System.EventHandler(this.cbParity_SelectionChangeCommitted);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(8, 61);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(153, 24);
            this.label8.TabIndex = 121;
            this.label8.Text = "Parity";
            this.label8.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cbBaurate
            // 
            this.cbBaurate.BackColor = System.Drawing.Color.Wheat;
            this.cbBaurate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbBaurate.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBaurate.FormattingEnabled = true;
            this.cbBaurate.Items.AddRange(new object[] {
            "4800",
            "9600",
            "115200",
            "230400",
            "256000"});
            this.cbBaurate.Location = new System.Drawing.Point(164, 24);
            this.cbBaurate.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.cbBaurate.Name = "cbBaurate";
            this.cbBaurate.Size = new System.Drawing.Size(153, 37);
            this.cbBaurate.TabIndex = 120;
            this.cbBaurate.SelectedIndexChanged += new System.EventHandler(this.cbBaurate_SelectedIndexChanged);
            // 
            // lbRTU2
            // 
            this.lbRTU2.AutoSize = true;
            this.lbRTU2.BackColor = System.Drawing.Color.Transparent;
            this.lbRTU2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbRTU2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRTU2.ForeColor = System.Drawing.Color.Black;
            this.lbRTU2.Location = new System.Drawing.Point(161, 0);
            this.lbRTU2.Margin = new System.Windows.Forms.Padding(0);
            this.lbRTU2.Name = "lbRTU2";
            this.lbRTU2.Size = new System.Drawing.Size(159, 24);
            this.lbRTU2.TabIndex = 119;
            this.lbRTU2.Text = "Baurate";
            this.lbRTU2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cbCom
            // 
            this.cbCom.BackColor = System.Drawing.Color.Wheat;
            this.cbCom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCom.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCom.FormattingEnabled = true;
            this.cbCom.Items.AddRange(new object[] {
            "Bee"});
            this.cbCom.Location = new System.Drawing.Point(4, 24);
            this.cbCom.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cbCom.Name = "cbCom";
            this.cbCom.Size = new System.Drawing.Size(155, 32);
            this.cbCom.TabIndex = 115;
            this.cbCom.SelectionChangeCommitted += new System.EventHandler(this.comIO_SelectionChangeCommitted);
            // 
            // lbRTU1
            // 
            this.lbRTU1.AutoSize = true;
            this.lbRTU1.BackColor = System.Drawing.Color.Transparent;
            this.lbRTU1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbRTU1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRTU1.ForeColor = System.Drawing.Color.Black;
            this.lbRTU1.Location = new System.Drawing.Point(2, 0);
            this.lbRTU1.Margin = new System.Windows.Forms.Padding(0);
            this.lbRTU1.Name = "lbRTU1";
            this.lbRTU1.Size = new System.Drawing.Size(159, 24);
            this.lbRTU1.TabIndex = 114;
            this.lbRTU1.Text = "COM";
            this.lbRTU1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(161, 61);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(159, 24);
            this.label7.TabIndex = 118;
            this.label7.Text = "Stopbits";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(8, 117);
            this.label9.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(153, 41);
            this.label9.TabIndex = 122;
            this.label9.Text = "DataBits";
            this.label9.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cbDataBits
            // 
            this.cbDataBits.BackColor = System.Drawing.Color.Wheat;
            this.cbDataBits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbDataBits.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDataBits.FormattingEnabled = true;
            this.cbDataBits.Location = new System.Drawing.Point(4, 158);
            this.cbDataBits.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.cbDataBits.Name = "cbDataBits";
            this.cbDataBits.Size = new System.Drawing.Size(155, 32);
            this.cbDataBits.TabIndex = 125;
            this.cbDataBits.SelectionChangeCommitted += new System.EventHandler(this.cbDataBits_SelectionChangeCommitted);
            // 
            // layIP
            // 
            this.layIP.BackColor = System.Drawing.Color.White;
            this.layIP.ColumnCount = 1;
            this.layIP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layIP.Controls.Add(this.btnReScan, 0, 6);
            this.layIP.Controls.Add(this.lbTCP1, 0, 0);
            this.layIP.Controls.Add(this.txtPort, 0, 3);
            this.layIP.Controls.Add(this.txtIP, 0, 1);
            this.layIP.Controls.Add(this.lbTCP2, 0, 2);
            this.layIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layIP.Location = new System.Drawing.Point(0, 0);
            this.layIP.Margin = new System.Windows.Forms.Padding(0);
            this.layIP.Name = "layIP";
            this.layIP.Padding = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.layIP.RowCount = 7;
            this.layIP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layIP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layIP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layIP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layIP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layIP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layIP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layIP.Size = new System.Drawing.Size(215, 254);
            this.layIP.TabIndex = 0;
            // 
            // btnReScan
            // 
            this.btnReScan.AutoFont = true;
            this.btnReScan.AutoFontHeightRatio = 0.75F;
            this.btnReScan.AutoFontMax = 100F;
            this.btnReScan.AutoFontMin = 6F;
            this.btnReScan.AutoFontWidthRatio = 0.92F;
            this.btnReScan.AutoImage = true;
            this.btnReScan.AutoImageMaxRatio = 0.75F;
            this.btnReScan.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnReScan.AutoImageTint = true;
            this.btnReScan.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnReScan.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnReScan.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnReScan.BorderRadius = 5;
            this.btnReScan.BorderSize = 1;
            this.btnReScan.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnReScan.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnReScan.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnReScan.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnReScan.Corner = BeeGlobal.Corner.Both;
            this.btnReScan.DebounceResizeMs = 16;
            this.btnReScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReScan.FlatAppearance.BorderSize = 0;
            this.btnReScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.39844F);
            this.btnReScan.ForeColor = System.Drawing.Color.Black;
            this.btnReScan.Image = null;
            this.btnReScan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReScan.ImageDisabled = null;
            this.btnReScan.ImageHover = null;
            this.btnReScan.ImageNormal = null;
            this.btnReScan.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnReScan.ImagePressed = null;
            this.btnReScan.ImageTextSpacing = 6;
            this.btnReScan.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnReScan.ImageTintHover = System.Drawing.Color.Empty;
            this.btnReScan.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnReScan.ImageTintOpacity = 0.5F;
            this.btnReScan.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnReScan.IsCLick = false;
            this.btnReScan.IsNotChange = true;
            this.btnReScan.IsRect = false;
            this.btnReScan.IsTouch = false;
            this.btnReScan.IsUnGroup = false;
            this.btnReScan.Location = new System.Drawing.Point(8, 132);
            this.btnReScan.Margin = new System.Windows.Forms.Padding(5);
            this.btnReScan.Multiline = false;
            this.btnReScan.Name = "btnReScan";
            this.btnReScan.Size = new System.Drawing.Size(199, 117);
            this.btnReScan.TabIndex = 118;
            this.btnReScan.Text = "Re-Scan";
            this.btnReScan.TextColor = System.Drawing.Color.Black;
            this.btnReScan.UseVisualStyleBackColor = false;
            this.btnReScan.Click += new System.EventHandler(this.btnReScan_Click);
            // 
            // lbTCP1
            // 
            this.lbTCP1.AutoSize = true;
            this.lbTCP1.BackColor = System.Drawing.Color.LightGray;
            this.lbTCP1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTCP1.Enabled = false;
            this.lbTCP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTCP1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lbTCP1.Location = new System.Drawing.Point(3, 5);
            this.lbTCP1.Margin = new System.Windows.Forms.Padding(0);
            this.lbTCP1.Name = "lbTCP1";
            this.lbTCP1.Size = new System.Drawing.Size(209, 24);
            this.lbTCP1.TabIndex = 117;
            this.lbTCP1.Text = "IP";
            this.lbTCP1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPort
            // 
            this.txtPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPort.Enabled = false;
            this.txtPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPort.Location = new System.Drawing.Point(3, 92);
            this.txtPort.Margin = new System.Windows.Forms.Padding(0);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(209, 35);
            this.txtPort.TabIndex = 112;
            this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
            this.txtPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPort_KeyPress);
            // 
            // txtIP
            // 
            this.txtIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIP.Enabled = false;
            this.txtIP.Location = new System.Drawing.Point(3, 29);
            this.txtIP.Margin = new System.Windows.Forms.Padding(0);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(209, 29);
            this.txtIP.TabIndex = 111;
            this.txtIP.Text = "192.168.1.1";
            this.txtIP.TextChanged += new System.EventHandler(this.txtIP_TextChanged);
            // 
            // lbTCP2
            // 
            this.lbTCP2.AutoSize = true;
            this.lbTCP2.BackColor = System.Drawing.Color.LightGray;
            this.lbTCP2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTCP2.Enabled = false;
            this.lbTCP2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTCP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lbTCP2.Location = new System.Drawing.Point(3, 68);
            this.lbTCP2.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lbTCP2.Name = "lbTCP2";
            this.lbTCP2.Size = new System.Drawing.Size(209, 24);
            this.lbTCP2.TabIndex = 65;
            this.lbTCP2.Text = "Port";
            this.lbTCP2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbTCP2.Click += new System.EventHandler(this.label50_Click);
            // 
            // layParameterBits
            // 
            this.layParameterBits.ColumnCount = 1;
            this.layParameterBits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layParameterBits.Controls.Add(this.tableLayoutPanel9, 0, 3);
            this.layParameterBits.Controls.Add(this.tableLayoutPanel8, 0, 5);
            this.layParameterBits.Controls.Add(this.tableLayoutPanel6, 0, 4);
            this.layParameterBits.Controls.Add(this.tableLayoutPanel5, 0, 2);
            this.layParameterBits.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.layParameterBits.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.layParameterBits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layParameterBits.Location = new System.Drawing.Point(4, 775);
            this.layParameterBits.Name = "layParameterBits";
            this.layParameterBits.RowCount = 7;
            this.layParameterBits.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layParameterBits.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layParameterBits.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layParameterBits.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layParameterBits.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layParameterBits.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layParameterBits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layParameterBits.Size = new System.Drawing.Size(543, 350);
            this.layParameterBits.TabIndex = 108;
            this.layParameterBits.Visible = false;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel9.ColumnCount = 3;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 229F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.Controls.Add(this.label26, 2, 0);
            this.tableLayoutPanel9.Controls.Add(this.rjButton9, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.customNumericEx2, 1, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(5, 150);
            this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(533, 50);
            this.tableLayoutPanel9.TabIndex = 92;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.BackColor = System.Drawing.Color.Transparent;
            this.label26.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.Black;
            this.label26.Location = new System.Drawing.Point(498, 3);
            this.label26.Margin = new System.Windows.Forms.Padding(0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(35, 44);
            this.label26.TabIndex = 117;
            this.label26.Text = "ms";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rjButton9
            // 
            this.rjButton9.AutoFont = true;
            this.rjButton9.AutoFontHeightRatio = 0.75F;
            this.rjButton9.AutoFontMax = 100F;
            this.rjButton9.AutoFontMin = 6F;
            this.rjButton9.AutoFontWidthRatio = 0.92F;
            this.rjButton9.AutoImage = true;
            this.rjButton9.AutoImageMaxRatio = 0.75F;
            this.rjButton9.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton9.AutoImageTint = true;
            this.rjButton9.BackColor = System.Drawing.Color.White;
            this.rjButton9.BackgroundColor = System.Drawing.Color.White;
            this.rjButton9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton9.BorderColor = System.Drawing.Color.White;
            this.rjButton9.BorderRadius = 10;
            this.rjButton9.BorderSize = 1;
            this.rjButton9.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.rjButton9.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.rjButton9.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.rjButton9.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton9.Corner = BeeGlobal.Corner.Left;
            this.rjButton9.DebounceResizeMs = 16;
            this.rjButton9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton9.FlatAppearance.BorderSize = 0;
            this.rjButton9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.rjButton9.ForeColor = System.Drawing.Color.Black;
            this.rjButton9.Image = null;
            this.rjButton9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton9.ImageDisabled = null;
            this.rjButton9.ImageHover = null;
            this.rjButton9.ImageNormal = null;
            this.rjButton9.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton9.ImagePressed = null;
            this.rjButton9.ImageTextSpacing = 6;
            this.rjButton9.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton9.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton9.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton9.ImageTintOpacity = 0.5F;
            this.rjButton9.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton9.IsCLick = false;
            this.rjButton9.IsNotChange = true;
            this.rjButton9.IsRect = false;
            this.rjButton9.IsTouch = false;
            this.rjButton9.IsUnGroup = true;
            this.rjButton9.Location = new System.Drawing.Point(3, 3);
            this.rjButton9.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.rjButton9.Multiline = false;
            this.rjButton9.Name = "rjButton9";
            this.rjButton9.Size = new System.Drawing.Size(226, 44);
            this.rjButton9.TabIndex = 2;
            this.rjButton9.Text = "Time Reset Bits";
            this.rjButton9.TextColor = System.Drawing.Color.Black;
            this.rjButton9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton9.UseVisualStyleBackColor = false;
            // 
            // customNumericEx2
            // 
            this.customNumericEx2.AutoShowTextbox = false;
            this.customNumericEx2.AutoSizeTextbox = true;
            this.customNumericEx2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.customNumericEx2.BorderColor = System.Drawing.Color.Transparent;
            this.customNumericEx2.BorderRadius = 20;
            this.customNumericEx2.ButtonMaxSize = 64;
            this.customNumericEx2.ButtonMinSize = 24;
            this.customNumericEx2.Decimals = 0;
            this.customNumericEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customNumericEx2.ElementGap = 6;
            this.customNumericEx2.FillTextboxToAvailable = true;
            this.customNumericEx2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.customNumericEx2.InnerPadding = new System.Windows.Forms.Padding(6);
            this.customNumericEx2.KeyboardStep = 1F;
            this.customNumericEx2.Location = new System.Drawing.Point(232, 3);
            this.customNumericEx2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.customNumericEx2.Max = 1000F;
            this.customNumericEx2.MaxTextboxWidth = 0;
            this.customNumericEx2.Min = 1F;
            this.customNumericEx2.MinimumSize = new System.Drawing.Size(120, 32);
            this.customNumericEx2.MinTextboxWidth = 16;
            this.customNumericEx2.Name = "customNumericEx2";
            this.customNumericEx2.Size = new System.Drawing.Size(263, 44);
            this.customNumericEx2.SnapToStep = true;
            this.customNumericEx2.StartWithTextboxHidden = false;
            this.customNumericEx2.Step = 1F;
            this.customNumericEx2.TabIndex = 80;
            this.customNumericEx2.TextboxFontSize = 16F;
            this.customNumericEx2.TextboxSidePadding = 12;
            this.customNumericEx2.TextboxWidth = 56;
            this.customNumericEx2.UnitText = "";
            this.customNumericEx2.Value = 1F;
            this.customNumericEx2.WheelStep = 1F;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel8.ColumnCount = 4;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 153F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel8.Controls.Add(this.rjButton10, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.label22, 3, 0);
            this.tableLayoutPanel8.Controls.Add(this.rjButton3, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.customNumericEx1, 2, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(5, 250);
            this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(533, 50);
            this.tableLayoutPanel8.TabIndex = 91;
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
            this.rjButton10.BackColor = System.Drawing.Color.White;
            this.rjButton10.BackgroundColor = System.Drawing.Color.White;
            this.rjButton10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton10.BorderColor = System.Drawing.Color.White;
            this.rjButton10.BorderRadius = 10;
            this.rjButton10.BorderSize = 1;
            this.rjButton10.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.rjButton10.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.rjButton10.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.rjButton10.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton10.Corner = BeeGlobal.Corner.None;
            this.rjButton10.DebounceResizeMs = 16;
            this.rjButton10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton10.Enabled = false;
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
            this.rjButton10.IsNotChange = true;
            this.rjButton10.IsRect = false;
            this.rjButton10.IsTouch = false;
            this.rjButton10.IsUnGroup = true;
            this.rjButton10.Location = new System.Drawing.Point(76, 3);
            this.rjButton10.Margin = new System.Windows.Forms.Padding(0);
            this.rjButton10.Multiline = false;
            this.rjButton10.Name = "rjButton10";
            this.rjButton10.Size = new System.Drawing.Size(153, 44);
            this.rjButton10.TabIndex = 117;
            this.rjButton10.Text = "Blink Bit Alive";
            this.rjButton10.TextColor = System.Drawing.Color.Black;
            this.rjButton10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton10.UseVisualStyleBackColor = false;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.Black;
            this.label22.Location = new System.Drawing.Point(498, 3);
            this.label22.Margin = new System.Windows.Forms.Padding(0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(35, 44);
            this.label22.TabIndex = 116;
            this.label22.Text = "ms";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.rjButton3.BackColor = System.Drawing.Color.White;
            this.rjButton3.BackgroundColor = System.Drawing.Color.White;
            this.rjButton3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton3.BorderColor = System.Drawing.Color.White;
            this.rjButton3.BorderRadius = 10;
            this.rjButton3.BorderSize = 2;
            this.rjButton3.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.rjButton3.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.rjButton3.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
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
            this.rjButton3.IsNotChange = true;
            this.rjButton3.IsRect = false;
            this.rjButton3.IsTouch = false;
            this.rjButton3.IsUnGroup = true;
            this.rjButton3.Location = new System.Drawing.Point(3, 3);
            this.rjButton3.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.rjButton3.Multiline = false;
            this.rjButton3.Name = "rjButton3";
            this.rjButton3.Size = new System.Drawing.Size(73, 44);
            this.rjButton3.TabIndex = 2;
            this.rjButton3.Text = "OFF";
            this.rjButton3.TextColor = System.Drawing.Color.Black;
            this.rjButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton3.UseVisualStyleBackColor = false;
            // 
            // customNumericEx1
            // 
            this.customNumericEx1.AutoShowTextbox = false;
            this.customNumericEx1.AutoSizeTextbox = true;
            this.customNumericEx1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.customNumericEx1.BorderColor = System.Drawing.Color.Transparent;
            this.customNumericEx1.BorderRadius = 20;
            this.customNumericEx1.ButtonMaxSize = 64;
            this.customNumericEx1.ButtonMinSize = 24;
            this.customNumericEx1.Decimals = 0;
            this.customNumericEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customNumericEx1.ElementGap = 6;
            this.customNumericEx1.Enabled = false;
            this.customNumericEx1.FillTextboxToAvailable = true;
            this.customNumericEx1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.customNumericEx1.InnerPadding = new System.Windows.Forms.Padding(6);
            this.customNumericEx1.KeyboardStep = 1F;
            this.customNumericEx1.Location = new System.Drawing.Point(232, 3);
            this.customNumericEx1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.customNumericEx1.Max = 1000F;
            this.customNumericEx1.MaxTextboxWidth = 0;
            this.customNumericEx1.Min = 1F;
            this.customNumericEx1.MinimumSize = new System.Drawing.Size(120, 32);
            this.customNumericEx1.MinTextboxWidth = 16;
            this.customNumericEx1.Name = "customNumericEx1";
            this.customNumericEx1.Size = new System.Drawing.Size(263, 44);
            this.customNumericEx1.SnapToStep = true;
            this.customNumericEx1.StartWithTextboxHidden = false;
            this.customNumericEx1.Step = 1F;
            this.customNumericEx1.TabIndex = 80;
            this.customNumericEx1.TextboxFontSize = 16F;
            this.customNumericEx1.TextboxSidePadding = 12;
            this.customNumericEx1.TextboxWidth = 56;
            this.customNumericEx1.UnitText = "";
            this.customNumericEx1.Value = 1F;
            this.customNumericEx1.WheelStep = 1F;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 229F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.Controls.Add(this.label24, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.rjButton8, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.tmOut, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(5, 200);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(533, 50);
            this.tableLayoutPanel6.TabIndex = 90;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.Transparent;
            this.label24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.Black;
            this.label24.Location = new System.Drawing.Point(498, 3);
            this.label24.Margin = new System.Windows.Forms.Padding(0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(35, 44);
            this.label24.TabIndex = 121;
            this.label24.Text = "ms";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rjButton8
            // 
            this.rjButton8.AutoFont = true;
            this.rjButton8.AutoFontHeightRatio = 0.75F;
            this.rjButton8.AutoFontMax = 100F;
            this.rjButton8.AutoFontMin = 6F;
            this.rjButton8.AutoFontWidthRatio = 0.92F;
            this.rjButton8.AutoImage = true;
            this.rjButton8.AutoImageMaxRatio = 0.75F;
            this.rjButton8.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton8.AutoImageTint = true;
            this.rjButton8.BackColor = System.Drawing.Color.White;
            this.rjButton8.BackgroundColor = System.Drawing.Color.White;
            this.rjButton8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton8.BorderColor = System.Drawing.Color.White;
            this.rjButton8.BorderRadius = 10;
            this.rjButton8.BorderSize = 1;
            this.rjButton8.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.rjButton8.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.rjButton8.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.rjButton8.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton8.Corner = BeeGlobal.Corner.Left;
            this.rjButton8.DebounceResizeMs = 16;
            this.rjButton8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton8.FlatAppearance.BorderSize = 0;
            this.rjButton8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.rjButton8.ForeColor = System.Drawing.Color.Black;
            this.rjButton8.Image = null;
            this.rjButton8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton8.ImageDisabled = null;
            this.rjButton8.ImageHover = null;
            this.rjButton8.ImageNormal = null;
            this.rjButton8.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton8.ImagePressed = null;
            this.rjButton8.ImageTextSpacing = 6;
            this.rjButton8.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton8.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton8.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton8.ImageTintOpacity = 0.5F;
            this.rjButton8.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton8.IsCLick = false;
            this.rjButton8.IsNotChange = true;
            this.rjButton8.IsRect = false;
            this.rjButton8.IsTouch = false;
            this.rjButton8.IsUnGroup = true;
            this.rjButton8.Location = new System.Drawing.Point(3, 3);
            this.rjButton8.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.rjButton8.Multiline = false;
            this.rjButton8.Name = "rjButton8";
            this.rjButton8.Size = new System.Drawing.Size(226, 44);
            this.rjButton8.TabIndex = 2;
            this.rjButton8.Text = "TimeOut (ms)";
            this.rjButton8.TextColor = System.Drawing.Color.Black;
            this.rjButton8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton8.UseVisualStyleBackColor = false;
            // 
            // tmOut
            // 
            this.tmOut.AutoShowTextbox = false;
            this.tmOut.AutoSizeTextbox = true;
            this.tmOut.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tmOut.BorderColor = System.Drawing.Color.Transparent;
            this.tmOut.BorderRadius = 20;
            this.tmOut.ButtonMaxSize = 64;
            this.tmOut.ButtonMinSize = 24;
            this.tmOut.Decimals = 0;
            this.tmOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tmOut.ElementGap = 6;
            this.tmOut.FillTextboxToAvailable = true;
            this.tmOut.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tmOut.InnerPadding = new System.Windows.Forms.Padding(6);
            this.tmOut.KeyboardStep = 1F;
            this.tmOut.Location = new System.Drawing.Point(232, 3);
            this.tmOut.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.tmOut.Max = 5000F;
            this.tmOut.MaxTextboxWidth = 0;
            this.tmOut.Min = 1F;
            this.tmOut.MinimumSize = new System.Drawing.Size(120, 32);
            this.tmOut.MinTextboxWidth = 16;
            this.tmOut.Name = "tmOut";
            this.tmOut.Size = new System.Drawing.Size(263, 44);
            this.tmOut.SnapToStep = true;
            this.tmOut.StartWithTextboxHidden = false;
            this.tmOut.Step = 1F;
            this.tmOut.TabIndex = 120;
            this.tmOut.TextboxFontSize = 16F;
            this.tmOut.TextboxSidePadding = 12;
            this.tmOut.TextboxWidth = 56;
            this.tmOut.UnitText = "";
            this.tmOut.Value = 1F;
            this.tmOut.WheelStep = 1F;
            this.tmOut.ValueChanged += new System.Action<float>(this.tmOut_ValueChanged);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 228F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.Controls.Add(this.label28, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.rjButton5, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.timerRead, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(5, 100);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(533, 50);
            this.tableLayoutPanel5.TabIndex = 89;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.BackColor = System.Drawing.Color.Transparent;
            this.label28.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.ForeColor = System.Drawing.Color.Black;
            this.label28.Location = new System.Drawing.Point(498, 3);
            this.label28.Margin = new System.Windows.Forms.Padding(0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(35, 44);
            this.label28.TabIndex = 117;
            this.label28.Text = "ms";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rjButton5
            // 
            this.rjButton5.AutoFont = true;
            this.rjButton5.AutoFontHeightRatio = 0.75F;
            this.rjButton5.AutoFontMax = 100F;
            this.rjButton5.AutoFontMin = 6F;
            this.rjButton5.AutoFontWidthRatio = 0.92F;
            this.rjButton5.AutoImage = true;
            this.rjButton5.AutoImageMaxRatio = 0.75F;
            this.rjButton5.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton5.AutoImageTint = true;
            this.rjButton5.BackColor = System.Drawing.Color.White;
            this.rjButton5.BackgroundColor = System.Drawing.Color.White;
            this.rjButton5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton5.BorderColor = System.Drawing.Color.White;
            this.rjButton5.BorderRadius = 10;
            this.rjButton5.BorderSize = 1;
            this.rjButton5.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.rjButton5.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.rjButton5.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.rjButton5.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton5.Corner = BeeGlobal.Corner.Left;
            this.rjButton5.DebounceResizeMs = 16;
            this.rjButton5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton5.FlatAppearance.BorderSize = 0;
            this.rjButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.rjButton5.ForeColor = System.Drawing.Color.Black;
            this.rjButton5.Image = null;
            this.rjButton5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton5.ImageDisabled = null;
            this.rjButton5.ImageHover = null;
            this.rjButton5.ImageNormal = null;
            this.rjButton5.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton5.ImagePressed = null;
            this.rjButton5.ImageTextSpacing = 6;
            this.rjButton5.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton5.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton5.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton5.ImageTintOpacity = 0.5F;
            this.rjButton5.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton5.IsCLick = false;
            this.rjButton5.IsNotChange = true;
            this.rjButton5.IsRect = false;
            this.rjButton5.IsTouch = false;
            this.rjButton5.IsUnGroup = true;
            this.rjButton5.Location = new System.Drawing.Point(3, 3);
            this.rjButton5.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.rjButton5.Multiline = false;
            this.rjButton5.Name = "rjButton5";
            this.rjButton5.Size = new System.Drawing.Size(225, 44);
            this.rjButton5.TabIndex = 2;
            this.rjButton5.Text = "Time Read (ms)";
            this.rjButton5.TextColor = System.Drawing.Color.Black;
            this.rjButton5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton5.UseVisualStyleBackColor = false;
            // 
            // timerRead
            // 
            this.timerRead.AutoShowTextbox = false;
            this.timerRead.AutoSizeTextbox = true;
            this.timerRead.BackColor = System.Drawing.Color.WhiteSmoke;
            this.timerRead.BorderColor = System.Drawing.Color.Transparent;
            this.timerRead.BorderRadius = 20;
            this.timerRead.ButtonMaxSize = 64;
            this.timerRead.ButtonMinSize = 24;
            this.timerRead.Decimals = 0;
            this.timerRead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timerRead.ElementGap = 6;
            this.timerRead.FillTextboxToAvailable = true;
            this.timerRead.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.timerRead.InnerPadding = new System.Windows.Forms.Padding(6);
            this.timerRead.KeyboardStep = 1F;
            this.timerRead.Location = new System.Drawing.Point(231, 3);
            this.timerRead.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.timerRead.Max = 1000F;
            this.timerRead.MaxTextboxWidth = 0;
            this.timerRead.Min = 1F;
            this.timerRead.MinimumSize = new System.Drawing.Size(120, 32);
            this.timerRead.MinTextboxWidth = 16;
            this.timerRead.Name = "timerRead";
            this.timerRead.Size = new System.Drawing.Size(264, 44);
            this.timerRead.SnapToStep = true;
            this.timerRead.StartWithTextboxHidden = false;
            this.timerRead.Step = 1F;
            this.timerRead.TabIndex = 80;
            this.timerRead.TextboxFontSize = 16F;
            this.timerRead.TextboxSidePadding = 12;
            this.timerRead.TextboxWidth = 56;
            this.timerRead.UnitText = "";
            this.timerRead.Value = 1F;
            this.timerRead.WheelStep = 1F;
            this.timerRead.ValueChanged += new System.Action<float>(this.timerRead_ValueChanged);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 227F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.txtAddWrite, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.rjButton4, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(5, 50);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(533, 50);
            this.tableLayoutPanel4.TabIndex = 88;
            // 
            // txtAddWrite
            // 
            this.txtAddWrite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAddWrite.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddWrite.Location = new System.Drawing.Point(230, 3);
            this.txtAddWrite.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.txtAddWrite.Multiline = true;
            this.txtAddWrite.Name = "txtAddWrite";
            this.txtAddWrite.Size = new System.Drawing.Size(300, 41);
            this.txtAddWrite.TabIndex = 126;
            this.txtAddWrite.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAddWrite.TextChanged += new System.EventHandler(this.txtAddWrite_TextChanged);
            // 
            // rjButton4
            // 
            this.rjButton4.AutoFont = true;
            this.rjButton4.AutoFontHeightRatio = 0.75F;
            this.rjButton4.AutoFontMax = 100F;
            this.rjButton4.AutoFontMin = 6F;
            this.rjButton4.AutoFontWidthRatio = 0.92F;
            this.rjButton4.AutoImage = true;
            this.rjButton4.AutoImageMaxRatio = 0.75F;
            this.rjButton4.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton4.AutoImageTint = true;
            this.rjButton4.BackColor = System.Drawing.Color.White;
            this.rjButton4.BackgroundColor = System.Drawing.Color.White;
            this.rjButton4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton4.BorderColor = System.Drawing.Color.White;
            this.rjButton4.BorderRadius = 10;
            this.rjButton4.BorderSize = 1;
            this.rjButton4.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.rjButton4.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.rjButton4.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.rjButton4.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton4.Corner = BeeGlobal.Corner.Left;
            this.rjButton4.DebounceResizeMs = 16;
            this.rjButton4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton4.FlatAppearance.BorderSize = 0;
            this.rjButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.rjButton4.ForeColor = System.Drawing.Color.Black;
            this.rjButton4.Image = null;
            this.rjButton4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton4.ImageDisabled = null;
            this.rjButton4.ImageHover = null;
            this.rjButton4.ImageNormal = null;
            this.rjButton4.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton4.ImagePressed = null;
            this.rjButton4.ImageTextSpacing = 6;
            this.rjButton4.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton4.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton4.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton4.ImageTintOpacity = 0.5F;
            this.rjButton4.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton4.IsCLick = false;
            this.rjButton4.IsNotChange = true;
            this.rjButton4.IsRect = false;
            this.rjButton4.IsTouch = false;
            this.rjButton4.IsUnGroup = true;
            this.rjButton4.Location = new System.Drawing.Point(3, 3);
            this.rjButton4.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.rjButton4.Multiline = false;
            this.rjButton4.Name = "rjButton4";
            this.rjButton4.Size = new System.Drawing.Size(224, 44);
            this.rjButton4.TabIndex = 2;
            this.rjButton4.Text = "Address Write";
            this.rjButton4.TextColor = System.Drawing.Color.Black;
            this.rjButton4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton4.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 229F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.txtAddRead, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.rjButton6, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(533, 50);
            this.tableLayoutPanel3.TabIndex = 87;
            // 
            // txtAddRead
            // 
            this.txtAddRead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAddRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddRead.Location = new System.Drawing.Point(229, 3);
            this.txtAddRead.Margin = new System.Windows.Forms.Padding(0);
            this.txtAddRead.Multiline = true;
            this.txtAddRead.Name = "txtAddRead";
            this.txtAddRead.Size = new System.Drawing.Size(304, 44);
            this.txtAddRead.TabIndex = 125;
            this.txtAddRead.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAddRead.TextChanged += new System.EventHandler(this.txtAddRead_TextChanged);
            // 
            // rjButton6
            // 
            this.rjButton6.AutoFont = true;
            this.rjButton6.AutoFontHeightRatio = 0.75F;
            this.rjButton6.AutoFontMax = 100F;
            this.rjButton6.AutoFontMin = 6F;
            this.rjButton6.AutoFontWidthRatio = 0.92F;
            this.rjButton6.AutoImage = true;
            this.rjButton6.AutoImageMaxRatio = 0.75F;
            this.rjButton6.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.rjButton6.AutoImageTint = true;
            this.rjButton6.BackColor = System.Drawing.Color.White;
            this.rjButton6.BackgroundColor = System.Drawing.Color.White;
            this.rjButton6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rjButton6.BorderColor = System.Drawing.Color.White;
            this.rjButton6.BorderRadius = 10;
            this.rjButton6.BorderSize = 1;
            this.rjButton6.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.rjButton6.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.rjButton6.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.rjButton6.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.rjButton6.Corner = BeeGlobal.Corner.Left;
            this.rjButton6.DebounceResizeMs = 16;
            this.rjButton6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rjButton6.FlatAppearance.BorderSize = 0;
            this.rjButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rjButton6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.07813F);
            this.rjButton6.ForeColor = System.Drawing.Color.Black;
            this.rjButton6.Image = null;
            this.rjButton6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rjButton6.ImageDisabled = null;
            this.rjButton6.ImageHover = null;
            this.rjButton6.ImageNormal = null;
            this.rjButton6.ImagePadding = new System.Windows.Forms.Padding(1);
            this.rjButton6.ImagePressed = null;
            this.rjButton6.ImageTextSpacing = 6;
            this.rjButton6.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.rjButton6.ImageTintHover = System.Drawing.Color.Empty;
            this.rjButton6.ImageTintNormal = System.Drawing.Color.Empty;
            this.rjButton6.ImageTintOpacity = 0.5F;
            this.rjButton6.ImageTintPressed = System.Drawing.Color.Empty;
            this.rjButton6.IsCLick = false;
            this.rjButton6.IsNotChange = true;
            this.rjButton6.IsRect = false;
            this.rjButton6.IsTouch = false;
            this.rjButton6.IsUnGroup = true;
            this.rjButton6.Location = new System.Drawing.Point(3, 3);
            this.rjButton6.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.rjButton6.Multiline = false;
            this.rjButton6.Name = "rjButton6";
            this.rjButton6.Size = new System.Drawing.Size(226, 44);
            this.rjButton6.TabIndex = 2;
            this.rjButton6.Text = "Address Read";
            this.rjButton6.TextColor = System.Drawing.Color.Black;
            this.rjButton6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.rjButton6.UseVisualStyleBackColor = false;
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage5.Controls.Add(this.layInput);
            this.tabPage5.Controls.Add(this.tableLayoutPanel1);
            this.tabPage5.Controls.Add(this.tableLayoutPanel7);
            this.tabPage5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage5.Location = new System.Drawing.Point(4, 33);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(567, 803);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "Input";
            // 
            // layInput
            // 
            this.layInput.AutoScroll = true;
            this.layInput.ColumnCount = 1;
            this.layInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layInput.Location = new System.Drawing.Point(3, 75);
            this.layInput.Name = "layInput";
            this.layInput.RowCount = 1;
            this.layInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layInput.Size = new System.Drawing.Size(561, 725);
            this.layInput.TabIndex = 61;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.28326F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.67265F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.0441F));
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label10, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label14, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label18, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 49);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(561, 26);
            this.tableLayoutPanel1.TabIndex = 60;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(259, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(191, 26);
            this.label3.TabIndex = 2;
            this.label3.Text = "Value";
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(78, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(175, 26);
            this.label10.TabIndex = 1;
            this.label10.Text = "Type";
            // 
            // label14
            // 
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Location = new System.Drawing.Point(3, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(69, 26);
            this.label14.TabIndex = 0;
            this.label14.Text = "Bit";
            // 
            // label18
            // 
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Location = new System.Drawing.Point(456, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(102, 26);
            this.label18.TabIndex = 3;
            this.label18.Text = "Blink";
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.29084F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.70916F));
            this.tableLayoutPanel7.Controls.Add(this.btnEnableEditInput, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.label20, 0, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(561, 46);
            this.tableLayoutPanel7.TabIndex = 62;
            // 
            // btnEnableEditInput
            // 
            this.btnEnableEditInput.AutoFont = true;
            this.btnEnableEditInput.AutoFontHeightRatio = 0.75F;
            this.btnEnableEditInput.AutoFontMax = 100F;
            this.btnEnableEditInput.AutoFontMin = 6F;
            this.btnEnableEditInput.AutoFontWidthRatio = 0.92F;
            this.btnEnableEditInput.AutoImage = true;
            this.btnEnableEditInput.AutoImageMaxRatio = 0.75F;
            this.btnEnableEditInput.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnEnableEditInput.AutoImageTint = true;
            this.btnEnableEditInput.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnEnableEditInput.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnEnableEditInput.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnEnableEditInput.BorderRadius = 5;
            this.btnEnableEditInput.BorderSize = 1;
            this.btnEnableEditInput.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnEnableEditInput.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnEnableEditInput.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnEnableEditInput.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnEnableEditInput.Corner = BeeGlobal.Corner.Both;
            this.btnEnableEditInput.DebounceResizeMs = 16;
            this.btnEnableEditInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnableEditInput.FlatAppearance.BorderSize = 0;
            this.btnEnableEditInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnableEditInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.14063F);
            this.btnEnableEditInput.ForeColor = System.Drawing.Color.Black;
            this.btnEnableEditInput.Image = null;
            this.btnEnableEditInput.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnableEditInput.ImageDisabled = null;
            this.btnEnableEditInput.ImageHover = null;
            this.btnEnableEditInput.ImageNormal = null;
            this.btnEnableEditInput.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnEnableEditInput.ImagePressed = null;
            this.btnEnableEditInput.ImageTextSpacing = 6;
            this.btnEnableEditInput.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnEnableEditInput.ImageTintHover = System.Drawing.Color.Empty;
            this.btnEnableEditInput.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnEnableEditInput.ImageTintOpacity = 0.5F;
            this.btnEnableEditInput.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnEnableEditInput.IsCLick = false;
            this.btnEnableEditInput.IsNotChange = false;
            this.btnEnableEditInput.IsRect = false;
            this.btnEnableEditInput.IsTouch = false;
            this.btnEnableEditInput.IsUnGroup = true;
            this.btnEnableEditInput.Location = new System.Drawing.Point(197, 5);
            this.btnEnableEditInput.Margin = new System.Windows.Forms.Padding(5);
            this.btnEnableEditInput.Multiline = false;
            this.btnEnableEditInput.Name = "btnEnableEditInput";
            this.btnEnableEditInput.Size = new System.Drawing.Size(359, 36);
            this.btnEnableEditInput.TabIndex = 110;
            this.btnEnableEditInput.Text = "OFF";
            this.btnEnableEditInput.TextColor = System.Drawing.Color.Black;
            this.btnEnableEditInput.UseVisualStyleBackColor = false;
            this.btnEnableEditInput.Click += new System.EventHandler(this.btnEnableEditInput_Click);
            // 
            // label20
            // 
            this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(3, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(186, 46);
            this.label20.TabIndex = 1;
            this.label20.Text = "Enable Edit";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage6.Controls.Add(this.layOutput);
            this.tabPage6.Controls.Add(this.layOut);
            this.tabPage6.Controls.Add(this.tableLayoutPanel2);
            this.tabPage6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage6.Location = new System.Drawing.Point(4, 33);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(5);
            this.tabPage6.Size = new System.Drawing.Size(567, 803);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "Output";
            // 
            // layOutput
            // 
            this.layOutput.AutoScroll = true;
            this.layOutput.ColumnCount = 1;
            this.layOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layOutput.Location = new System.Drawing.Point(5, 77);
            this.layOutput.Name = "layOutput";
            this.layOutput.RowCount = 1;
            this.layOutput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layOutput.Size = new System.Drawing.Size(557, 721);
            this.layOutput.TabIndex = 57;
            this.layOutput.Paint += new System.Windows.Forms.PaintEventHandler(this.layOutput_Paint);
            // 
            // layOut
            // 
            this.layOut.BackColor = System.Drawing.SystemColors.Control;
            this.layOut.ColumnCount = 4;
            this.layOut.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.layOut.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.28326F));
            this.layOut.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.67265F));
            this.layOut.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.0441F));
            this.layOut.Controls.Add(this.label5, 2, 0);
            this.layOut.Controls.Add(this.label2, 1, 0);
            this.layOut.Controls.Add(this.label1, 0, 0);
            this.layOut.Controls.Add(this.label6, 3, 0);
            this.layOut.Dock = System.Windows.Forms.DockStyle.Top;
            this.layOut.Location = new System.Drawing.Point(5, 51);
            this.layOut.Name = "layOut";
            this.layOut.RowCount = 1;
            this.layOut.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.layOut.Size = new System.Drawing.Size(557, 26);
            this.layOut.TabIndex = 59;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(257, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(190, 26);
            this.label5.TabIndex = 2;
            this.label5.Text = "Value";
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(78, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Type";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bit";
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(453, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 26);
            this.label6.TabIndex = 3;
            this.label6.Text = "Blink";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.29084F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.70916F));
            this.tableLayoutPanel2.Controls.Add(this.btnEnableEditType, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label19, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(557, 46);
            this.tableLayoutPanel2.TabIndex = 60;
            // 
            // btnEnableEditType
            // 
            this.btnEnableEditType.AutoFont = true;
            this.btnEnableEditType.AutoFontHeightRatio = 0.75F;
            this.btnEnableEditType.AutoFontMax = 100F;
            this.btnEnableEditType.AutoFontMin = 6F;
            this.btnEnableEditType.AutoFontWidthRatio = 0.92F;
            this.btnEnableEditType.AutoImage = true;
            this.btnEnableEditType.AutoImageMaxRatio = 0.75F;
            this.btnEnableEditType.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnEnableEditType.AutoImageTint = true;
            this.btnEnableEditType.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnEnableEditType.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.btnEnableEditType.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnEnableEditType.BorderRadius = 5;
            this.btnEnableEditType.BorderSize = 1;
            this.btnEnableEditType.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnEnableEditType.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnEnableEditType.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnEnableEditType.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnEnableEditType.Corner = BeeGlobal.Corner.Both;
            this.btnEnableEditType.DebounceResizeMs = 16;
            this.btnEnableEditType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEnableEditType.FlatAppearance.BorderSize = 0;
            this.btnEnableEditType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnableEditType.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.14063F);
            this.btnEnableEditType.ForeColor = System.Drawing.Color.Black;
            this.btnEnableEditType.Image = null;
            this.btnEnableEditType.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnableEditType.ImageDisabled = null;
            this.btnEnableEditType.ImageHover = null;
            this.btnEnableEditType.ImageNormal = null;
            this.btnEnableEditType.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnEnableEditType.ImagePressed = null;
            this.btnEnableEditType.ImageTextSpacing = 6;
            this.btnEnableEditType.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnEnableEditType.ImageTintHover = System.Drawing.Color.Empty;
            this.btnEnableEditType.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnEnableEditType.ImageTintOpacity = 0.5F;
            this.btnEnableEditType.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnEnableEditType.IsCLick = false;
            this.btnEnableEditType.IsNotChange = false;
            this.btnEnableEditType.IsRect = false;
            this.btnEnableEditType.IsTouch = false;
            this.btnEnableEditType.IsUnGroup = true;
            this.btnEnableEditType.Location = new System.Drawing.Point(195, 5);
            this.btnEnableEditType.Margin = new System.Windows.Forms.Padding(5);
            this.btnEnableEditType.Multiline = false;
            this.btnEnableEditType.Name = "btnEnableEditType";
            this.btnEnableEditType.Size = new System.Drawing.Size(357, 36);
            this.btnEnableEditType.TabIndex = 110;
            this.btnEnableEditType.Text = "OFF";
            this.btnEnableEditType.TextColor = System.Drawing.Color.Black;
            this.btnEnableEditType.UseVisualStyleBackColor = false;
            this.btnEnableEditType.Click += new System.EventHandler(this.btnEnableEditType_Click);
            // 
            // label19
            // 
            this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label19.Location = new System.Drawing.Point(3, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(184, 46);
            this.label19.TabIndex = 1;
            this.label19.Text = "Enable Edit";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.panel8);
            this.tabPage1.Controls.Add(this.panel7);
            this.tabPage1.Controls.Add(this.panel6);
            this.tabPage1.Controls.Add(this.panel4);
            this.tabPage1.Controls.Add(this.panel3);
            this.tabPage1.Location = new System.Drawing.Point(4, 33);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(567, 803);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "Value";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.btnReadProgress);
            this.panel8.Controls.Add(this.txtValueProgress);
            this.panel8.Controls.Add(this.label27);
            this.panel8.Controls.Add(this.txtAddProgress);
            this.panel8.Controls.Add(this.label29);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(3, 275);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(561, 68);
            this.panel8.TabIndex = 28;
            // 
            // btnReadProgress
            // 
            this.btnReadProgress.AutoFont = true;
            this.btnReadProgress.AutoFontHeightRatio = 0.75F;
            this.btnReadProgress.AutoFontMax = 100F;
            this.btnReadProgress.AutoFontMin = 6F;
            this.btnReadProgress.AutoFontWidthRatio = 0.92F;
            this.btnReadProgress.AutoImage = true;
            this.btnReadProgress.AutoImageMaxRatio = 0.75F;
            this.btnReadProgress.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnReadProgress.AutoImageTint = true;
            this.btnReadProgress.BackColor = System.Drawing.SystemColors.Control;
            this.btnReadProgress.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnReadProgress.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnReadProgress.BorderRadius = 0;
            this.btnReadProgress.BorderSize = 0;
            this.btnReadProgress.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnReadProgress.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnReadProgress.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnReadProgress.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnReadProgress.Corner = BeeGlobal.Corner.Both;
            this.btnReadProgress.DebounceResizeMs = 16;
            this.btnReadProgress.FlatAppearance.BorderSize = 0;
            this.btnReadProgress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.203125F);
            this.btnReadProgress.ForeColor = System.Drawing.Color.Black;
            this.btnReadProgress.Image = null;
            this.btnReadProgress.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReadProgress.ImageDisabled = null;
            this.btnReadProgress.ImageHover = null;
            this.btnReadProgress.ImageNormal = null;
            this.btnReadProgress.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnReadProgress.ImagePressed = null;
            this.btnReadProgress.ImageTextSpacing = 6;
            this.btnReadProgress.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnReadProgress.ImageTintHover = System.Drawing.Color.Empty;
            this.btnReadProgress.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnReadProgress.ImageTintOpacity = 0.5F;
            this.btnReadProgress.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnReadProgress.IsCLick = false;
            this.btnReadProgress.IsNotChange = true;
            this.btnReadProgress.IsRect = false;
            this.btnReadProgress.IsTouch = false;
            this.btnReadProgress.IsUnGroup = true;
            this.btnReadProgress.Location = new System.Drawing.Point(344, 26);
            this.btnReadProgress.Multiline = false;
            this.btnReadProgress.Name = "btnReadProgress";
            this.btnReadProgress.Size = new System.Drawing.Size(55, 33);
            this.btnReadProgress.TabIndex = 29;
            this.btnReadProgress.Text = "Read";
            this.btnReadProgress.TextColor = System.Drawing.Color.Black;
            this.btnReadProgress.UseVisualStyleBackColor = false;
            this.btnReadProgress.Click += new System.EventHandler(this.btnReadProgress_Click);
            // 
            // txtValueProgress
            // 
            this.txtValueProgress.Location = new System.Drawing.Point(161, 33);
            this.txtValueProgress.Name = "txtValueProgress";
            this.txtValueProgress.Size = new System.Drawing.Size(168, 29);
            this.txtValueProgress.TabIndex = 3;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(157, 6);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(139, 24);
            this.label27.TabIndex = 2;
            this.label27.Text = "Value Progress";
            // 
            // txtAddProgress
            // 
            this.txtAddProgress.Location = new System.Drawing.Point(7, 30);
            this.txtAddProgress.Name = "txtAddProgress";
            this.txtAddProgress.Size = new System.Drawing.Size(118, 29);
            this.txtAddProgress.TabIndex = 1;
            this.txtAddProgress.TextChanged += new System.EventHandler(this.txtAddProgress_TextChanged);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(17, 3);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(125, 24);
            this.label29.TabIndex = 0;
            this.label29.Text = "Add Progress";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnReadQty);
            this.panel7.Controls.Add(this.txtValueQty);
            this.panel7.Controls.Add(this.label23);
            this.panel7.Controls.Add(this.txtAddQty);
            this.panel7.Controls.Add(this.label25);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(3, 207);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(561, 68);
            this.panel7.TabIndex = 27;
            // 
            // btnReadQty
            // 
            this.btnReadQty.AutoFont = true;
            this.btnReadQty.AutoFontHeightRatio = 0.75F;
            this.btnReadQty.AutoFontMax = 100F;
            this.btnReadQty.AutoFontMin = 6F;
            this.btnReadQty.AutoFontWidthRatio = 0.92F;
            this.btnReadQty.AutoImage = true;
            this.btnReadQty.AutoImageMaxRatio = 0.75F;
            this.btnReadQty.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnReadQty.AutoImageTint = true;
            this.btnReadQty.BackColor = System.Drawing.SystemColors.Control;
            this.btnReadQty.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnReadQty.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnReadQty.BorderRadius = 0;
            this.btnReadQty.BorderSize = 0;
            this.btnReadQty.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnReadQty.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnReadQty.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnReadQty.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnReadQty.Corner = BeeGlobal.Corner.Both;
            this.btnReadQty.DebounceResizeMs = 16;
            this.btnReadQty.FlatAppearance.BorderSize = 0;
            this.btnReadQty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.203125F);
            this.btnReadQty.ForeColor = System.Drawing.Color.Black;
            this.btnReadQty.Image = null;
            this.btnReadQty.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReadQty.ImageDisabled = null;
            this.btnReadQty.ImageHover = null;
            this.btnReadQty.ImageNormal = null;
            this.btnReadQty.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnReadQty.ImagePressed = null;
            this.btnReadQty.ImageTextSpacing = 6;
            this.btnReadQty.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnReadQty.ImageTintHover = System.Drawing.Color.Empty;
            this.btnReadQty.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnReadQty.ImageTintOpacity = 0.5F;
            this.btnReadQty.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnReadQty.IsCLick = false;
            this.btnReadQty.IsNotChange = true;
            this.btnReadQty.IsRect = false;
            this.btnReadQty.IsTouch = false;
            this.btnReadQty.IsUnGroup = true;
            this.btnReadQty.Location = new System.Drawing.Point(344, 26);
            this.btnReadQty.Multiline = false;
            this.btnReadQty.Name = "btnReadQty";
            this.btnReadQty.Size = new System.Drawing.Size(55, 33);
            this.btnReadQty.TabIndex = 29;
            this.btnReadQty.Text = "Read";
            this.btnReadQty.TextColor = System.Drawing.Color.Black;
            this.btnReadQty.UseVisualStyleBackColor = false;
            this.btnReadQty.Click += new System.EventHandler(this.btnReadQty_Click);
            // 
            // txtValueQty
            // 
            this.txtValueQty.Location = new System.Drawing.Point(161, 33);
            this.txtValueQty.Name = "txtValueQty";
            this.txtValueQty.Size = new System.Drawing.Size(168, 29);
            this.txtValueQty.TabIndex = 3;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(157, 6);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(92, 24);
            this.label23.TabIndex = 2;
            this.label23.Text = "Value Qty";
            // 
            // txtAddQty
            // 
            this.txtAddQty.Location = new System.Drawing.Point(7, 30);
            this.txtAddQty.Name = "txtAddQty";
            this.txtAddQty.Size = new System.Drawing.Size(118, 29);
            this.txtAddQty.TabIndex = 1;
            this.txtAddQty.TextChanged += new System.EventHandler(this.txtAddQty_TextChanged);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(17, 3);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(78, 24);
            this.label25.TabIndex = 0;
            this.label25.Text = "Add Qty";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnReadCountProg);
            this.panel6.Controls.Add(this.txtValueCountProg);
            this.panel6.Controls.Add(this.label16);
            this.panel6.Controls.Add(this.txtAddCountProg);
            this.panel6.Controls.Add(this.label17);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(3, 139);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(561, 68);
            this.panel6.TabIndex = 26;
            // 
            // btnReadCountProg
            // 
            this.btnReadCountProg.AutoFont = true;
            this.btnReadCountProg.AutoFontHeightRatio = 0.75F;
            this.btnReadCountProg.AutoFontMax = 100F;
            this.btnReadCountProg.AutoFontMin = 6F;
            this.btnReadCountProg.AutoFontWidthRatio = 0.92F;
            this.btnReadCountProg.AutoImage = true;
            this.btnReadCountProg.AutoImageMaxRatio = 0.75F;
            this.btnReadCountProg.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnReadCountProg.AutoImageTint = true;
            this.btnReadCountProg.BackColor = System.Drawing.SystemColors.Control;
            this.btnReadCountProg.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnReadCountProg.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnReadCountProg.BorderRadius = 0;
            this.btnReadCountProg.BorderSize = 0;
            this.btnReadCountProg.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnReadCountProg.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnReadCountProg.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnReadCountProg.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnReadCountProg.Corner = BeeGlobal.Corner.Both;
            this.btnReadCountProg.DebounceResizeMs = 16;
            this.btnReadCountProg.FlatAppearance.BorderSize = 0;
            this.btnReadCountProg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadCountProg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.203125F);
            this.btnReadCountProg.ForeColor = System.Drawing.Color.Black;
            this.btnReadCountProg.Image = null;
            this.btnReadCountProg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReadCountProg.ImageDisabled = null;
            this.btnReadCountProg.ImageHover = null;
            this.btnReadCountProg.ImageNormal = null;
            this.btnReadCountProg.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnReadCountProg.ImagePressed = null;
            this.btnReadCountProg.ImageTextSpacing = 6;
            this.btnReadCountProg.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnReadCountProg.ImageTintHover = System.Drawing.Color.Empty;
            this.btnReadCountProg.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnReadCountProg.ImageTintOpacity = 0.5F;
            this.btnReadCountProg.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnReadCountProg.IsCLick = false;
            this.btnReadCountProg.IsNotChange = true;
            this.btnReadCountProg.IsRect = false;
            this.btnReadCountProg.IsTouch = false;
            this.btnReadCountProg.IsUnGroup = true;
            this.btnReadCountProg.Location = new System.Drawing.Point(344, 26);
            this.btnReadCountProg.Multiline = false;
            this.btnReadCountProg.Name = "btnReadCountProg";
            this.btnReadCountProg.Size = new System.Drawing.Size(55, 33);
            this.btnReadCountProg.TabIndex = 29;
            this.btnReadCountProg.Text = "Read";
            this.btnReadCountProg.TextColor = System.Drawing.Color.Black;
            this.btnReadCountProg.UseVisualStyleBackColor = false;
            this.btnReadCountProg.Click += new System.EventHandler(this.btnReadCountProg_Click);
            // 
            // txtValueCountProg
            // 
            this.txtValueCountProg.Location = new System.Drawing.Point(161, 33);
            this.txtValueCountProg.Name = "txtValueCountProg";
            this.txtValueCountProg.Size = new System.Drawing.Size(168, 29);
            this.txtValueCountProg.TabIndex = 3;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(157, 6);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(159, 24);
            this.label16.TabIndex = 2;
            this.label16.Text = "Value Count Prog";
            // 
            // txtAddCountProg
            // 
            this.txtAddCountProg.Location = new System.Drawing.Point(7, 30);
            this.txtAddCountProg.Name = "txtAddCountProg";
            this.txtAddCountProg.Size = new System.Drawing.Size(118, 29);
            this.txtAddCountProg.TabIndex = 1;
            this.txtAddCountProg.TextChanged += new System.EventHandler(this.txtAddCountProg_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(17, 3);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(108, 24);
            this.label17.TabIndex = 0;
            this.label17.Text = "Add C.Prog";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnReadPO);
            this.panel4.Controls.Add(this.txtValuePO);
            this.panel4.Controls.Add(this.label13);
            this.panel4.Controls.Add(this.txtAddPO);
            this.panel4.Controls.Add(this.label15);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 71);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(561, 68);
            this.panel4.TabIndex = 25;
            // 
            // btnReadPO
            // 
            this.btnReadPO.AutoFont = true;
            this.btnReadPO.AutoFontHeightRatio = 0.75F;
            this.btnReadPO.AutoFontMax = 100F;
            this.btnReadPO.AutoFontMin = 6F;
            this.btnReadPO.AutoFontWidthRatio = 0.92F;
            this.btnReadPO.AutoImage = true;
            this.btnReadPO.AutoImageMaxRatio = 0.75F;
            this.btnReadPO.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnReadPO.AutoImageTint = true;
            this.btnReadPO.BackColor = System.Drawing.SystemColors.Control;
            this.btnReadPO.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnReadPO.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnReadPO.BorderRadius = 0;
            this.btnReadPO.BorderSize = 0;
            this.btnReadPO.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnReadPO.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnReadPO.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnReadPO.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnReadPO.Corner = BeeGlobal.Corner.Both;
            this.btnReadPO.DebounceResizeMs = 16;
            this.btnReadPO.FlatAppearance.BorderSize = 0;
            this.btnReadPO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadPO.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.203125F);
            this.btnReadPO.ForeColor = System.Drawing.Color.Black;
            this.btnReadPO.Image = null;
            this.btnReadPO.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReadPO.ImageDisabled = null;
            this.btnReadPO.ImageHover = null;
            this.btnReadPO.ImageNormal = null;
            this.btnReadPO.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnReadPO.ImagePressed = null;
            this.btnReadPO.ImageTextSpacing = 6;
            this.btnReadPO.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnReadPO.ImageTintHover = System.Drawing.Color.Empty;
            this.btnReadPO.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnReadPO.ImageTintOpacity = 0.5F;
            this.btnReadPO.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnReadPO.IsCLick = false;
            this.btnReadPO.IsNotChange = true;
            this.btnReadPO.IsRect = false;
            this.btnReadPO.IsTouch = false;
            this.btnReadPO.IsUnGroup = true;
            this.btnReadPO.Location = new System.Drawing.Point(344, 26);
            this.btnReadPO.Multiline = false;
            this.btnReadPO.Name = "btnReadPO";
            this.btnReadPO.Size = new System.Drawing.Size(55, 33);
            this.btnReadPO.TabIndex = 28;
            this.btnReadPO.Text = "Read";
            this.btnReadPO.TextColor = System.Drawing.Color.Black;
            this.btnReadPO.UseVisualStyleBackColor = false;
            this.btnReadPO.Click += new System.EventHandler(this.btnReadPO_Click);
            // 
            // txtValuePO
            // 
            this.txtValuePO.Location = new System.Drawing.Point(161, 33);
            this.txtValuePO.Name = "txtValuePO";
            this.txtValuePO.Size = new System.Drawing.Size(168, 29);
            this.txtValuePO.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(157, 6);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(91, 24);
            this.label13.TabIndex = 2;
            this.label13.Text = "Value PO";
            // 
            // txtAddPO
            // 
            this.txtAddPO.Location = new System.Drawing.Point(7, 30);
            this.txtAddPO.Name = "txtAddPO";
            this.txtAddPO.Size = new System.Drawing.Size(118, 29);
            this.txtAddPO.TabIndex = 1;
            this.txtAddPO.TextChanged += new System.EventHandler(this.txtPO_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(19, 3);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 24);
            this.label15.TabIndex = 0;
            this.label15.Text = "Add PO";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnReadProg);
            this.panel3.Controls.Add(this.txtProg);
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.txtAddProg);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(561, 68);
            this.panel3.TabIndex = 24;
            // 
            // btnReadProg
            // 
            this.btnReadProg.AutoFont = true;
            this.btnReadProg.AutoFontHeightRatio = 0.75F;
            this.btnReadProg.AutoFontMax = 100F;
            this.btnReadProg.AutoFontMin = 6F;
            this.btnReadProg.AutoFontWidthRatio = 0.92F;
            this.btnReadProg.AutoImage = true;
            this.btnReadProg.AutoImageMaxRatio = 0.75F;
            this.btnReadProg.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
            this.btnReadProg.AutoImageTint = true;
            this.btnReadProg.BackColor = System.Drawing.SystemColors.Control;
            this.btnReadProg.BackgroundColor = System.Drawing.SystemColors.Control;
            this.btnReadProg.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnReadProg.BorderRadius = 0;
            this.btnReadProg.BorderSize = 0;
            this.btnReadProg.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
            this.btnReadProg.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
            this.btnReadProg.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
            this.btnReadProg.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.btnReadProg.Corner = BeeGlobal.Corner.Both;
            this.btnReadProg.DebounceResizeMs = 16;
            this.btnReadProg.FlatAppearance.BorderSize = 0;
            this.btnReadProg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadProg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.203125F);
            this.btnReadProg.ForeColor = System.Drawing.Color.Black;
            this.btnReadProg.Image = null;
            this.btnReadProg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReadProg.ImageDisabled = null;
            this.btnReadProg.ImageHover = null;
            this.btnReadProg.ImageNormal = null;
            this.btnReadProg.ImagePadding = new System.Windows.Forms.Padding(1);
            this.btnReadProg.ImagePressed = null;
            this.btnReadProg.ImageTextSpacing = 6;
            this.btnReadProg.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.btnReadProg.ImageTintHover = System.Drawing.Color.Empty;
            this.btnReadProg.ImageTintNormal = System.Drawing.Color.Empty;
            this.btnReadProg.ImageTintOpacity = 0.5F;
            this.btnReadProg.ImageTintPressed = System.Drawing.Color.Empty;
            this.btnReadProg.IsCLick = false;
            this.btnReadProg.IsNotChange = true;
            this.btnReadProg.IsRect = false;
            this.btnReadProg.IsTouch = false;
            this.btnReadProg.IsUnGroup = true;
            this.btnReadProg.Location = new System.Drawing.Point(344, 27);
            this.btnReadProg.Multiline = false;
            this.btnReadProg.Name = "btnReadProg";
            this.btnReadProg.Size = new System.Drawing.Size(55, 33);
            this.btnReadProg.TabIndex = 27;
            this.btnReadProg.Text = "Read";
            this.btnReadProg.TextColor = System.Drawing.Color.Black;
            this.btnReadProg.UseVisualStyleBackColor = false;
            this.btnReadProg.Click += new System.EventHandler(this.btnReadProg_Click);
            // 
            // txtProg
            // 
            this.txtProg.Location = new System.Drawing.Point(161, 33);
            this.txtProg.Name = "txtProg";
            this.txtProg.Size = new System.Drawing.Size(168, 29);
            this.txtProg.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(157, 6);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(104, 24);
            this.label12.TabIndex = 2;
            this.label12.Text = "Value Prog";
            // 
            // txtAddProg
            // 
            this.txtAddProg.Location = new System.Drawing.Point(7, 30);
            this.txtAddProg.Name = "txtAddProg";
            this.txtAddProg.Size = new System.Drawing.Size(118, 29);
            this.txtAddProg.TabIndex = 1;
            this.txtAddProg.TextChanged += new System.EventHandler(this.txtAddProg_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(19, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(90, 24);
            this.label11.TabIndex = 0;
            this.label11.Text = "Add Prog";
            // 
            // tmConnect
            // 
            this.tmConnect.Interval = 3000;
            this.tmConnect.Tick += new System.EventHandler(this.tmConnect_Tick);
            // 
            // ProtocolPLC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.Name = "ProtocolPLC";
            this.Size = new System.Drawing.Size(575, 840);
            this.Load += new System.EventHandler(this.SettingPLC_Load);
            this.VisibleChanged += new System.EventHandler(this.SettingPLC_VisibleChanged);
            this.tabControl1.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.laySetting.ResumeLayout(false);
            this.pConnect.ResumeLayout(false);
            this.layTypeHard.ResumeLayout(false);
            this.layTypeComunication.ResumeLayout(false);
            this.layBrand.ResumeLayout(false);
            this.laySettingCom.ResumeLayout(false);
            this.layCom.ResumeLayout(false);
            this.layCom.PerformLayout();
            this.layIP.ResumeLayout(false);
            this.layIP.PerformLayout();
            this.layParameterBits.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel8.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.layOut.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.Timer tmConnect;
        private TabPage tabPage1;
        private Panel panel3;
        private TextBox txtAddProg;
        private Label label11;
        private Label label12;
        public TextBox txtProg;
        private Panel panel4;
        public TextBox txtValuePO;
        private Label label13;
        private TextBox txtAddPO;
        private Label label15;
        private Panel panel6;
        public TextBox txtValueCountProg;
        private Label label16;
        private TextBox txtAddCountProg;
        private Label label17;
        private RJButton btnReadPO;
        private RJButton btnReadProg;
        private RJButton btnReadCountProg;
        private Panel panel7;
        private RJButton btnReadQty;
        public TextBox txtValueQty;
        private Label label23;
        private TextBox txtAddQty;
        private Label label25;
        private Panel panel8;
        private RJButton btnReadProgress;
        public TextBox txtValueProgress;
        private Label label27;
        private TextBox txtAddProgress;
        private Label label29;
        private TableLayoutPanel laySetting;
        private RJButton btn1S;
        private TableLayoutPanel layTypeHard;
        private RJButton btnPCICard;
        private RJButton btnIO;
        private RJButton btnIsPLC;
        private TableLayoutPanel layTypeComunication;
        private RJButton btnTCP;
        private RJButton btnSerial;
        private CustomNumericEx timerRead;
        private CustomNumericEx tmOut;
        private TableLayoutPanel layBrand;
        private RJButton btnDelta;
        private RJButton rjButton2;
        private RJButton btnMitsu2;
        private RJButton btnMitsu;
        private RJButton rjButton7;
        private RJButton btnMisu3;
        private RJButton btnKeyence;
        private RJButton btnRtu;
        private RJButton btnModbusAscii;
        private TableLayoutPanel laySettingCom;
        private TableLayoutPanel layCom;
        private CustomNumericEx numSlaveID;
        private Label label4;
        private RJButton btnRtsEnable;
        private RJButton btnDtrEnable;
        public ComboBox cbStopBits;
        public ComboBox cbParity;
        private Label label8;
        public ComboBox cbBaurate;
        private Label lbRTU2;
        public ComboBox cbCom;
        private Label lbRTU1;
        private Label label7;
        private Label label9;
        public ComboBox cbDataBits;
        private TableLayoutPanel layIP;
        private RJButton btnReScan;
        private Label lbTCP1;
        private TextBox txtPort;
        private TextBox txtIP;
        private Label lbTCP2;
        private RJButton btn3S;
        private RJButton btn2S;
        private RJButton btn4S;
        private TableLayoutPanel layParameterBits;
        private TableLayoutPanel tableLayoutPanel4;
        private RJButton rjButton4;
        private TableLayoutPanel tableLayoutPanel3;
        private TextBox txtAddRead;
        private RJButton rjButton6;
        private TextBox txtAddWrite;
        private TableLayoutPanel tableLayoutPanel6;
        private RJButton rjButton8;
        private TableLayoutPanel tableLayoutPanel5;
        private RJButton rjButton5;
        private TableLayoutPanel layOutput;
        private TableLayoutPanel layOut;
        private Label label5;
        private Label label2;
        private Label label1;
        private Label label6;
        private TableLayoutPanel layInput;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label3;
        private Label label10;
        private Label label14;
        private Label label18;
        private TableLayoutPanel tableLayoutPanel2;
        private RJButton btnEnableEditType;
        private Label label19;
        private TableLayoutPanel tableLayoutPanel7;
        private RJButton btnEnableEditInput;
        private Label label20;
        private Panel pConnect;
        private Panel panel5;
        private RJButton StatusIObtn;
        private RJButton btnConectIO;
        private Panel panel2;
        private RJButton btnBypass;
        private TableLayoutPanel tableLayoutPanel8;
        private Label label22;
        private RJButton rjButton3;
        private CustomNumericEx customNumericEx1;
        private TableLayoutPanel tableLayoutPanel9;
        private Label label26;
        private RJButton rjButton9;
        private CustomNumericEx customNumericEx2;
        private Label label24;
        private Label label28;
        private RJButton rjButton10;
    }
}
