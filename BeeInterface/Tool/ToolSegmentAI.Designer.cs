namespace BeeInterface
{
    partial class ToolSegmentAI
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabTrainingData;
        private System.Windows.Forms.TabPage tabTrain;
        private System.Windows.Forms.TabPage tabInference;
        private RJButton btnHeaderGeneral;
        private System.Windows.Forms.Panel panelGeneralParams;
        private System.Windows.Forms.TextBox txtModelPath;
        private System.Windows.Forms.TextBox txtSamplesPath;
        private System.Windows.Forms.Label lblModelPath;
        private System.Windows.Forms.Label lblSamplesPath;
        private System.Windows.Forms.Label lblModelState;
        private RJButton btnHeaderTrainData;
        private System.Windows.Forms.Panel panelTrainingData;
        private System.Windows.Forms.ListBox listSamples;
        private System.Windows.Forms.Button btnAddSample;
        private System.Windows.Forms.Button btnRemoveSample;
        private System.Windows.Forms.Button btnClearSamples;
        private System.Windows.Forms.Label lblSampleCount;
        private System.Windows.Forms.Label lblSampleInfo;
        private System.Windows.Forms.PictureBox picSamplePreview;
        private RJButton btnHeaderTrainParams;
        private System.Windows.Forms.Panel panelTrainParams;
        private System.Windows.Forms.NumericUpDown numTrees;
        private System.Windows.Forms.NumericUpDown numMaxDepth;
        private System.Windows.Forms.NumericUpDown numMinSample;
        private System.Windows.Forms.Button btnTrainStart;
        private System.Windows.Forms.Button btnTrainCancel;
        private System.Windows.Forms.ProgressBar progressTrain;
        private System.Windows.Forms.Label lblTrainStatus;
        private System.Windows.Forms.TextBox txtTrainLog;
        private System.Windows.Forms.Label lblTrees;
        private System.Windows.Forms.Label lblMaxDepth;
        private System.Windows.Forms.Label lblMinSample;
        private RJButton btnHeaderInferParams;
        private System.Windows.Forms.Panel panelInferParams;
        private System.Windows.Forms.NumericUpDown numThreshold;
        private System.Windows.Forms.NumericUpDown numMinArea;
        private System.Windows.Forms.CheckBox chkGpu;
        private System.Windows.Forms.Button btnBrowseTest;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TextBox txtTestImage;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.Label lblMinArea;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label lblDefectPixels;

        private void InitializeComponent()
        {
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.panelGeneralParams = new System.Windows.Forms.Panel();
            this.lblModelState = new System.Windows.Forms.Label();
            this.txtSamplesPath = new System.Windows.Forms.TextBox();
            this.txtModelPath = new System.Windows.Forms.TextBox();
            this.lblSamplesPath = new System.Windows.Forms.Label();
            this.lblModelPath = new System.Windows.Forms.Label();
            this.btnHeaderGeneral = new BeeInterface.RJButton();
            this.tabTrainingData = new System.Windows.Forms.TabPage();
            this.panelTrainingData = new System.Windows.Forms.Panel();
            this.picSamplePreview = new System.Windows.Forms.PictureBox();
            this.lblSampleInfo = new System.Windows.Forms.Label();
            this.lblSampleCount = new System.Windows.Forms.Label();
            this.btnClearSamples = new System.Windows.Forms.Button();
            this.btnRemoveSample = new System.Windows.Forms.Button();
            this.btnAddSample = new System.Windows.Forms.Button();
            this.listSamples = new System.Windows.Forms.ListBox();
            this.btnHeaderTrainData = new BeeInterface.RJButton();
            this.tabTrain = new System.Windows.Forms.TabPage();
            this.txtTrainLog = new System.Windows.Forms.TextBox();
            this.panelTrainParams = new System.Windows.Forms.Panel();
            this.lblMinSample = new System.Windows.Forms.Label();
            this.lblMaxDepth = new System.Windows.Forms.Label();
            this.lblTrees = new System.Windows.Forms.Label();
            this.lblTrainStatus = new System.Windows.Forms.Label();
            this.progressTrain = new System.Windows.Forms.ProgressBar();
            this.btnTrainCancel = new System.Windows.Forms.Button();
            this.btnTrainStart = new System.Windows.Forms.Button();
            this.numMinSample = new System.Windows.Forms.NumericUpDown();
            this.numMaxDepth = new System.Windows.Forms.NumericUpDown();
            this.numTrees = new System.Windows.Forms.NumericUpDown();
            this.btnHeaderTrainParams = new BeeInterface.RJButton();
            this.tabInference = new System.Windows.Forms.TabPage();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.panelInferParams = new System.Windows.Forms.Panel();
            this.lblDefectPixels = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            this.lblMinArea = new System.Windows.Forms.Label();
            this.lblThreshold = new System.Windows.Forms.Label();
            this.txtTestImage = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnBrowseTest = new System.Windows.Forms.Button();
            this.chkGpu = new System.Windows.Forms.CheckBox();
            this.numMinArea = new System.Windows.Forms.NumericUpDown();
            this.numThreshold = new System.Windows.Forms.NumericUpDown();
            this.btnHeaderInferParams = new BeeInterface.RJButton();
            this.tabMain.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.panelGeneralParams.SuspendLayout();
            this.tabTrainingData.SuspendLayout();
            this.panelTrainingData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSamplePreview)).BeginInit();
            this.tabTrain.SuspendLayout();
            this.panelTrainParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinSample)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTrees)).BeginInit();
            this.tabInference.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.panelInferParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).BeginInit();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabGeneral);
            this.tabMain.Controls.Add(this.tabTrainingData);
            this.tabMain.Controls.Add(this.tabTrain);
            this.tabMain.Controls.Add(this.tabInference);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(860, 620);
            this.tabMain.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.panelGeneralParams);
            this.tabGeneral.Controls.Add(this.btnHeaderGeneral);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(8);
            this.tabGeneral.Size = new System.Drawing.Size(852, 594);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // panelGeneralParams
            // 
            this.panelGeneralParams.Controls.Add(this.lblModelState);
            this.panelGeneralParams.Controls.Add(this.txtSamplesPath);
            this.panelGeneralParams.Controls.Add(this.txtModelPath);
            this.panelGeneralParams.Controls.Add(this.lblSamplesPath);
            this.panelGeneralParams.Controls.Add(this.lblModelPath);
            this.panelGeneralParams.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelGeneralParams.Location = new System.Drawing.Point(8, 48);
            this.panelGeneralParams.Name = "panelGeneralParams";
            this.panelGeneralParams.Padding = new System.Windows.Forms.Padding(10);
            this.panelGeneralParams.Size = new System.Drawing.Size(836, 154);
            this.panelGeneralParams.TabIndex = 1;
            // 
            // lblModelState
            // 
            this.lblModelState.AutoSize = true;
            this.lblModelState.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblModelState.Location = new System.Drawing.Point(13, 112);
            this.lblModelState.Name = "lblModelState";
            this.lblModelState.Size = new System.Drawing.Size(125, 19);
            this.lblModelState.TabIndex = 4;
            this.lblModelState.Text = "Model not trained";
            // 
            // txtSamplesPath
            // 
            this.txtSamplesPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSamplesPath.Location = new System.Drawing.Point(112, 58);
            this.txtSamplesPath.Name = "txtSamplesPath";
            this.txtSamplesPath.ReadOnly = true;
            this.txtSamplesPath.Size = new System.Drawing.Size(711, 20);
            this.txtSamplesPath.TabIndex = 3;
            // 
            // txtModelPath
            // 
            this.txtModelPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModelPath.Location = new System.Drawing.Point(112, 22);
            this.txtModelPath.Name = "txtModelPath";
            this.txtModelPath.ReadOnly = true;
            this.txtModelPath.Size = new System.Drawing.Size(711, 20);
            this.txtModelPath.TabIndex = 2;
            // 
            // lblSamplesPath
            // 
            this.lblSamplesPath.AutoSize = true;
            this.lblSamplesPath.Location = new System.Drawing.Point(14, 61);
            this.lblSamplesPath.Name = "lblSamplesPath";
            this.lblSamplesPath.Size = new System.Drawing.Size(77, 13);
            this.lblSamplesPath.TabIndex = 1;
            this.lblSamplesPath.Text = "Samples folder";
            // 
            // lblModelPath
            // 
            this.lblModelPath.AutoSize = true;
            this.lblModelPath.Location = new System.Drawing.Point(14, 25);
            this.lblModelPath.Name = "lblModelPath";
            this.lblModelPath.Size = new System.Drawing.Size(62, 13);
            this.lblModelPath.TabIndex = 0;
            this.lblModelPath.Text = "Model path";
            // 
            // btnHeaderGeneral
            // 
            this.btnHeaderGeneral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(66)))), ((int)(((byte)(84)))));
            this.btnHeaderGeneral.BorderRadius = 10;
            this.btnHeaderGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHeaderGeneral.ForeColor = System.Drawing.Color.White;
            this.btnHeaderGeneral.Location = new System.Drawing.Point(8, 8);
            this.btnHeaderGeneral.Name = "btnHeaderGeneral";
            this.btnHeaderGeneral.Size = new System.Drawing.Size(836, 40);
            this.btnHeaderGeneral.TabIndex = 0;
            this.btnHeaderGeneral.Text = "General storage";
            this.btnHeaderGeneral.UseVisualStyleBackColor = false;
            // 
            // tabTrainingData
            // 
            this.tabTrainingData.Controls.Add(this.panelTrainingData);
            this.tabTrainingData.Controls.Add(this.btnHeaderTrainData);
            this.tabTrainingData.Location = new System.Drawing.Point(4, 22);
            this.tabTrainingData.Name = "tabTrainingData";
            this.tabTrainingData.Padding = new System.Windows.Forms.Padding(8);
            this.tabTrainingData.Size = new System.Drawing.Size(852, 594);
            this.tabTrainingData.TabIndex = 1;
            this.tabTrainingData.Text = "Training Data";
            this.tabTrainingData.UseVisualStyleBackColor = true;
            // 
            // panelTrainingData
            // 
            this.panelTrainingData.Controls.Add(this.picSamplePreview);
            this.panelTrainingData.Controls.Add(this.lblSampleInfo);
            this.panelTrainingData.Controls.Add(this.lblSampleCount);
            this.panelTrainingData.Controls.Add(this.btnClearSamples);
            this.panelTrainingData.Controls.Add(this.btnRemoveSample);
            this.panelTrainingData.Controls.Add(this.btnAddSample);
            this.panelTrainingData.Controls.Add(this.listSamples);
            this.panelTrainingData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTrainingData.Location = new System.Drawing.Point(8, 48);
            this.panelTrainingData.Name = "panelTrainingData";
            this.panelTrainingData.Padding = new System.Windows.Forms.Padding(8);
            this.panelTrainingData.Size = new System.Drawing.Size(836, 538);
            this.panelTrainingData.TabIndex = 1;
            // 
            // picSamplePreview
            // 
            this.picSamplePreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picSamplePreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(36)))), ((int)(((byte)(40)))));
            this.picSamplePreview.Location = new System.Drawing.Point(260, 44);
            this.picSamplePreview.Name = "picSamplePreview";
            this.picSamplePreview.Size = new System.Drawing.Size(565, 483);
            this.picSamplePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSamplePreview.TabIndex = 6;
            this.picSamplePreview.TabStop = false;
            // 
            // lblSampleInfo
            // 
            this.lblSampleInfo.AutoSize = true;
            this.lblSampleInfo.Location = new System.Drawing.Point(257, 16);
            this.lblSampleInfo.Name = "lblSampleInfo";
            this.lblSampleInfo.Size = new System.Drawing.Size(117, 13);
            this.lblSampleInfo.TabIndex = 5;
            this.lblSampleInfo.Text = "Select sample to preview";
            // 
            // lblSampleCount
            // 
            this.lblSampleCount.AutoSize = true;
            this.lblSampleCount.Location = new System.Drawing.Point(11, 16);
            this.lblSampleCount.Name = "lblSampleCount";
            this.lblSampleCount.Size = new System.Drawing.Size(54, 13);
            this.lblSampleCount.TabIndex = 4;
            this.lblSampleCount.Text = "0 samples";
            // 
            // btnClearSamples
            // 
            this.btnClearSamples.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearSamples.Location = new System.Drawing.Point(173, 504);
            this.btnClearSamples.Name = "btnClearSamples";
            this.btnClearSamples.Size = new System.Drawing.Size(75, 23);
            this.btnClearSamples.TabIndex = 3;
            this.btnClearSamples.Text = "Clear";
            this.btnClearSamples.UseVisualStyleBackColor = true;
            // 
            // btnRemoveSample
            // 
            this.btnRemoveSample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveSample.Location = new System.Drawing.Point(92, 504);
            this.btnRemoveSample.Name = "btnRemoveSample";
            this.btnRemoveSample.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveSample.TabIndex = 2;
            this.btnRemoveSample.Text = "Remove";
            this.btnRemoveSample.UseVisualStyleBackColor = true;
            // 
            // btnAddSample
            // 
            this.btnAddSample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddSample.Location = new System.Drawing.Point(11, 504);
            this.btnAddSample.Name = "btnAddSample";
            this.btnAddSample.Size = new System.Drawing.Size(75, 23);
            this.btnAddSample.TabIndex = 1;
            this.btnAddSample.Text = "Add";
            this.btnAddSample.UseVisualStyleBackColor = true;
            // 
            // listSamples
            // 
            this.listSamples.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listSamples.FormattingEnabled = true;
            this.listSamples.Location = new System.Drawing.Point(11, 44);
            this.listSamples.Name = "listSamples";
            this.listSamples.Size = new System.Drawing.Size(237, 446);
            this.listSamples.TabIndex = 0;
            // 
            // btnHeaderTrainData
            // 
            this.btnHeaderTrainData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(66)))), ((int)(((byte)(84)))));
            this.btnHeaderTrainData.BorderRadius = 10;
            this.btnHeaderTrainData.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHeaderTrainData.ForeColor = System.Drawing.Color.White;
            this.btnHeaderTrainData.Location = new System.Drawing.Point(8, 8);
            this.btnHeaderTrainData.Name = "btnHeaderTrainData";
            this.btnHeaderTrainData.Size = new System.Drawing.Size(836, 40);
            this.btnHeaderTrainData.TabIndex = 0;
            this.btnHeaderTrainData.Text = "Samples and annotation";
            this.btnHeaderTrainData.UseVisualStyleBackColor = false;
            // 
            // tabTrain
            // 
            this.tabTrain.Controls.Add(this.txtTrainLog);
            this.tabTrain.Controls.Add(this.panelTrainParams);
            this.tabTrain.Controls.Add(this.btnHeaderTrainParams);
            this.tabTrain.Location = new System.Drawing.Point(4, 22);
            this.tabTrain.Name = "tabTrain";
            this.tabTrain.Padding = new System.Windows.Forms.Padding(8);
            this.tabTrain.Size = new System.Drawing.Size(852, 594);
            this.tabTrain.TabIndex = 2;
            this.tabTrain.Text = "Train";
            this.tabTrain.UseVisualStyleBackColor = true;
            // 
            // txtTrainLog
            // 
            this.txtTrainLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTrainLog.Location = new System.Drawing.Point(8, 188);
            this.txtTrainLog.Multiline = true;
            this.txtTrainLog.Name = "txtTrainLog";
            this.txtTrainLog.ReadOnly = true;
            this.txtTrainLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTrainLog.Size = new System.Drawing.Size(836, 398);
            this.txtTrainLog.TabIndex = 2;
            // 
            // panelTrainParams
            // 
            this.panelTrainParams.Controls.Add(this.lblMinSample);
            this.panelTrainParams.Controls.Add(this.lblMaxDepth);
            this.panelTrainParams.Controls.Add(this.lblTrees);
            this.panelTrainParams.Controls.Add(this.lblTrainStatus);
            this.panelTrainParams.Controls.Add(this.progressTrain);
            this.panelTrainParams.Controls.Add(this.btnTrainCancel);
            this.panelTrainParams.Controls.Add(this.btnTrainStart);
            this.panelTrainParams.Controls.Add(this.numMinSample);
            this.panelTrainParams.Controls.Add(this.numMaxDepth);
            this.panelTrainParams.Controls.Add(this.numTrees);
            this.panelTrainParams.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTrainParams.Location = new System.Drawing.Point(8, 48);
            this.panelTrainParams.Name = "panelTrainParams";
            this.panelTrainParams.Padding = new System.Windows.Forms.Padding(10);
            this.panelTrainParams.Size = new System.Drawing.Size(836, 140);
            this.panelTrainParams.TabIndex = 1;
            // 
            // lblMinSample
            // 
            this.lblMinSample.AutoSize = true;
            this.lblMinSample.Location = new System.Drawing.Point(247, 21);
            this.lblMinSample.Name = "lblMinSample";
            this.lblMinSample.Size = new System.Drawing.Size(59, 13);
            this.lblMinSample.TabIndex = 9;
            this.lblMinSample.Text = "Min sample";
            // 
            // lblMaxDepth
            // 
            this.lblMaxDepth.AutoSize = true;
            this.lblMaxDepth.Location = new System.Drawing.Point(130, 21);
            this.lblMaxDepth.Name = "lblMaxDepth";
            this.lblMaxDepth.Size = new System.Drawing.Size(58, 13);
            this.lblMaxDepth.TabIndex = 8;
            this.lblMaxDepth.Text = "Max depth";
            // 
            // lblTrees
            // 
            this.lblTrees.AutoSize = true;
            this.lblTrees.Location = new System.Drawing.Point(13, 21);
            this.lblTrees.Name = "lblTrees";
            this.lblTrees.Size = new System.Drawing.Size(34, 13);
            this.lblTrees.TabIndex = 7;
            this.lblTrees.Text = "Trees";
            // 
            // lblTrainStatus
            // 
            this.lblTrainStatus.AutoSize = true;
            this.lblTrainStatus.Location = new System.Drawing.Point(13, 99);
            this.lblTrainStatus.Name = "lblTrainStatus";
            this.lblTrainStatus.Size = new System.Drawing.Size(21, 13);
            this.lblTrainStatus.TabIndex = 6;
            this.lblTrainStatus.Text = "0%";
            // 
            // progressTrain
            // 
            this.progressTrain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressTrain.Location = new System.Drawing.Point(80, 94);
            this.progressTrain.Name = "progressTrain";
            this.progressTrain.Size = new System.Drawing.Size(743, 23);
            this.progressTrain.TabIndex = 5;
            // 
            // btnTrainCancel
            // 
            this.btnTrainCancel.Enabled = false;
            this.btnTrainCancel.Location = new System.Drawing.Point(474, 36);
            this.btnTrainCancel.Name = "btnTrainCancel";
            this.btnTrainCancel.Size = new System.Drawing.Size(92, 32);
            this.btnTrainCancel.TabIndex = 4;
            this.btnTrainCancel.Text = "Cancel";
            this.btnTrainCancel.UseVisualStyleBackColor = true;
            // 
            // btnTrainStart
            // 
            this.btnTrainStart.Location = new System.Drawing.Point(376, 36);
            this.btnTrainStart.Name = "btnTrainStart";
            this.btnTrainStart.Size = new System.Drawing.Size(92, 32);
            this.btnTrainStart.TabIndex = 3;
            this.btnTrainStart.Text = "Start Train";
            this.btnTrainStart.UseVisualStyleBackColor = true;
            // 
            // numMinSample
            // 
            this.numMinSample.Location = new System.Drawing.Point(250, 40);
            this.numMinSample.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMinSample.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMinSample.Name = "numMinSample";
            this.numMinSample.Size = new System.Drawing.Size(92, 20);
            this.numMinSample.TabIndex = 2;
            this.numMinSample.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numMaxDepth
            // 
            this.numMaxDepth.Location = new System.Drawing.Point(133, 40);
            this.numMaxDepth.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.numMaxDepth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxDepth.Name = "numMaxDepth";
            this.numMaxDepth.Size = new System.Drawing.Size(92, 20);
            this.numMaxDepth.TabIndex = 1;
            this.numMaxDepth.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // numTrees
            // 
            this.numTrees.Location = new System.Drawing.Point(16, 40);
            this.numTrees.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numTrees.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTrees.Name = "numTrees";
            this.numTrees.Size = new System.Drawing.Size(92, 20);
            this.numTrees.TabIndex = 0;
            this.numTrees.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // btnHeaderTrainParams
            // 
            this.btnHeaderTrainParams.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(66)))), ((int)(((byte)(84)))));
            this.btnHeaderTrainParams.BorderRadius = 10;
            this.btnHeaderTrainParams.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHeaderTrainParams.ForeColor = System.Drawing.Color.White;
            this.btnHeaderTrainParams.Location = new System.Drawing.Point(8, 8);
            this.btnHeaderTrainParams.Name = "btnHeaderTrainParams";
            this.btnHeaderTrainParams.Size = new System.Drawing.Size(836, 40);
            this.btnHeaderTrainParams.TabIndex = 0;
            this.btnHeaderTrainParams.Text = "Training parameters";
            this.btnHeaderTrainParams.UseVisualStyleBackColor = false;
            // 
            // tabInference
            // 
            this.tabInference.Controls.Add(this.picPreview);
            this.tabInference.Controls.Add(this.panelInferParams);
            this.tabInference.Controls.Add(this.btnHeaderInferParams);
            this.tabInference.Location = new System.Drawing.Point(4, 22);
            this.tabInference.Name = "tabInference";
            this.tabInference.Padding = new System.Windows.Forms.Padding(8);
            this.tabInference.Size = new System.Drawing.Size(852, 594);
            this.tabInference.TabIndex = 3;
            this.tabInference.Text = "Inference";
            this.tabInference.UseVisualStyleBackColor = true;
            // 
            // picPreview
            // 
            this.picPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(36)))), ((int)(((byte)(40)))));
            this.picPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPreview.Location = new System.Drawing.Point(8, 188);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(836, 398);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 2;
            this.picPreview.TabStop = false;
            // 
            // panelInferParams
            // 
            this.panelInferParams.Controls.Add(this.lblDefectPixels);
            this.panelInferParams.Controls.Add(this.lblResult);
            this.panelInferParams.Controls.Add(this.lblScore);
            this.panelInferParams.Controls.Add(this.lblMinArea);
            this.panelInferParams.Controls.Add(this.lblThreshold);
            this.panelInferParams.Controls.Add(this.txtTestImage);
            this.panelInferParams.Controls.Add(this.btnTest);
            this.panelInferParams.Controls.Add(this.btnBrowseTest);
            this.panelInferParams.Controls.Add(this.chkGpu);
            this.panelInferParams.Controls.Add(this.numMinArea);
            this.panelInferParams.Controls.Add(this.numThreshold);
            this.panelInferParams.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInferParams.Location = new System.Drawing.Point(8, 48);
            this.panelInferParams.Name = "panelInferParams";
            this.panelInferParams.Padding = new System.Windows.Forms.Padding(10);
            this.panelInferParams.Size = new System.Drawing.Size(836, 140);
            this.panelInferParams.TabIndex = 1;
            // 
            // lblDefectPixels
            // 
            this.lblDefectPixels.AutoSize = true;
            this.lblDefectPixels.Location = new System.Drawing.Point(412, 86);
            this.lblDefectPixels.Name = "lblDefectPixels";
            this.lblDefectPixels.Size = new System.Drawing.Size(78, 13);
            this.lblDefectPixels.TabIndex = 10;
            this.lblDefectPixels.Text = "Defect pixels: 0";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(293, 86);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(62, 13);
            this.lblResult.TabIndex = 9;
            this.lblResult.Text = "Result: N/A";
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Location = new System.Drawing.Point(174, 86);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(68, 13);
            this.lblScore.TabIndex = 8;
            this.lblScore.Text = "Score: 0.000";
            // 
            // lblMinArea
            // 
            this.lblMinArea.AutoSize = true;
            this.lblMinArea.Location = new System.Drawing.Point(93, 22);
            this.lblMinArea.Name = "lblMinArea";
            this.lblMinArea.Size = new System.Drawing.Size(47, 13);
            this.lblMinArea.TabIndex = 7;
            this.lblMinArea.Text = "Min area";
            // 
            // lblThreshold
            // 
            this.lblThreshold.AutoSize = true;
            this.lblThreshold.Location = new System.Drawing.Point(13, 22);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new System.Drawing.Size(54, 13);
            this.lblThreshold.TabIndex = 6;
            this.lblThreshold.Text = "Threshold";
            // 
            // txtTestImage
            // 
            this.txtTestImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTestImage.Location = new System.Drawing.Point(177, 40);
            this.txtTestImage.Name = "txtTestImage";
            this.txtTestImage.ReadOnly = true;
            this.txtTestImage.Size = new System.Drawing.Size(504, 20);
            this.txtTestImage.TabIndex = 5;
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(747, 36);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(76, 28);
            this.btnTest.TabIndex = 4;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            // 
            // btnBrowseTest
            // 
            this.btnBrowseTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseTest.Location = new System.Drawing.Point(687, 36);
            this.btnBrowseTest.Name = "btnBrowseTest";
            this.btnBrowseTest.Size = new System.Drawing.Size(54, 28);
            this.btnBrowseTest.TabIndex = 3;
            this.btnBrowseTest.Text = "...";
            this.btnBrowseTest.UseVisualStyleBackColor = true;
            // 
            // chkGpu
            // 
            this.chkGpu.AutoSize = true;
            this.chkGpu.Location = new System.Drawing.Point(16, 85);
            this.chkGpu.Name = "chkGpu";
            this.chkGpu.Size = new System.Drawing.Size(83, 17);
            this.chkGpu.TabIndex = 2;
            this.chkGpu.Text = "Enable GPU";
            this.chkGpu.UseVisualStyleBackColor = true;
            // 
            // numMinArea
            // 
            this.numMinArea.Location = new System.Drawing.Point(96, 40);
            this.numMinArea.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numMinArea.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMinArea.Name = "numMinArea";
            this.numMinArea.Size = new System.Drawing.Size(64, 20);
            this.numMinArea.TabIndex = 1;
            this.numMinArea.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // numThreshold
            // 
            this.numThreshold.DecimalPlaces = 2;
            this.numThreshold.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numThreshold.Location = new System.Drawing.Point(16, 40);
            this.numThreshold.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numThreshold.Name = "numThreshold";
            this.numThreshold.Size = new System.Drawing.Size(64, 20);
            this.numThreshold.TabIndex = 0;
            this.numThreshold.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // btnHeaderInferParams
            // 
            this.btnHeaderInferParams.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(66)))), ((int)(((byte)(84)))));
            this.btnHeaderInferParams.BorderRadius = 10;
            this.btnHeaderInferParams.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHeaderInferParams.ForeColor = System.Drawing.Color.White;
            this.btnHeaderInferParams.Location = new System.Drawing.Point(8, 8);
            this.btnHeaderInferParams.Name = "btnHeaderInferParams";
            this.btnHeaderInferParams.Size = new System.Drawing.Size(836, 40);
            this.btnHeaderInferParams.TabIndex = 0;
            this.btnHeaderInferParams.Text = "Inference parameters";
            this.btnHeaderInferParams.UseVisualStyleBackColor = false;
            // 
            // ToolSegmentAI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabMain);
            this.Name = "ToolSegmentAI";
            this.Size = new System.Drawing.Size(860, 620);
            this.tabMain.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.panelGeneralParams.ResumeLayout(false);
            this.panelGeneralParams.PerformLayout();
            this.tabTrainingData.ResumeLayout(false);
            this.panelTrainingData.ResumeLayout(false);
            this.panelTrainingData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSamplePreview)).EndInit();
            this.tabTrain.ResumeLayout(false);
            this.tabTrain.PerformLayout();
            this.panelTrainParams.ResumeLayout(false);
            this.panelTrainParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinSample)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTrees)).EndInit();
            this.tabInference.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.panelInferParams.ResumeLayout(false);
            this.panelInferParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numThreshold)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
