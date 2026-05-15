using BeeCore;
using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CvPoint = OpenCvSharp.Point;
using CvRect = OpenCvSharp.Rect;
using DrawingImage = System.Drawing.Image;
using DrawingSize = System.Drawing.Size;

namespace BeeInterface
{
    public partial class ToolSegmentAI : UserControl
    {
        private PropetyTool ownerTool;
        private CancellationTokenSource trainCancellation;
        private string currentPreviewImagePath = "";

        public ToolSegmentAI()
        {
            InitializeComponent();
            if (Propety == null)
                Propety = new SegmentAI();
            WireEvents();
        }

        public SegmentAI Propety { get; set; }

        private PropetyTool OwnerTool
        {
            get
            {
                if (ownerTool == null && Propety != null)
                    ownerTool = Common.TryGetTool(Global.IndexProgChoose, Propety.Index);
                return ownerTool;
            }
        }

        public void LoadPara()
        {
            if (Propety == null)
                Propety = new SegmentAI();

            int index = Propety.Index >= 0 ? Propety.Index : Global.IndexToolSelected;
            Propety.EnsureStoragePaths(Math.Max(0, index));
            Propety.SetModel();
            BindPropetyToUi();
            AttachPropetyEvents();
            RefreshSamplesList();
        }

        private void WireEvents()
        {
            btnHeaderGeneral.Click -= TogglePanel_Click;
            btnHeaderGeneral.Click += TogglePanel_Click;
            btnHeaderTrainData.Click -= TogglePanel_Click;
            btnHeaderTrainData.Click += TogglePanel_Click;
            btnHeaderTrainParams.Click -= TogglePanel_Click;
            btnHeaderTrainParams.Click += TogglePanel_Click;
            btnHeaderInferParams.Click -= TogglePanel_Click;
            btnHeaderInferParams.Click += TogglePanel_Click;

            btnAddSample.Click -= BtnAddSample_Click;
            btnAddSample.Click += BtnAddSample_Click;
            btnRemoveSample.Click -= BtnRemoveSample_Click;
            btnRemoveSample.Click += BtnRemoveSample_Click;
            btnClearSamples.Click -= BtnClearSamples_Click;
            btnClearSamples.Click += BtnClearSamples_Click;
            listSamples.SelectedIndexChanged -= ListSamples_SelectedIndexChanged;
            listSamples.SelectedIndexChanged += ListSamples_SelectedIndexChanged;

            btnTrainStart.Click -= BtnTrainStart_Click;
            btnTrainStart.Click += BtnTrainStart_Click;
            btnTrainCancel.Click -= BtnTrainCancel_Click;
            btnTrainCancel.Click += BtnTrainCancel_Click;

            btnBrowseTest.Click -= BtnBrowseTest_Click;
            btnBrowseTest.Click += BtnBrowseTest_Click;
            btnTest.Click -= BtnTest_Click;
            btnTest.Click += BtnTest_Click;

            numTrees.ValueChanged -= TrainParam_ValueChanged;
            numTrees.ValueChanged += TrainParam_ValueChanged;
            numMaxDepth.ValueChanged -= TrainParam_ValueChanged;
            numMaxDepth.ValueChanged += TrainParam_ValueChanged;
            numMinSample.ValueChanged -= TrainParam_ValueChanged;
            numMinSample.ValueChanged += TrainParam_ValueChanged;
            numThreshold.ValueChanged -= InferParam_ValueChanged;
            numThreshold.ValueChanged += InferParam_ValueChanged;
            numMinArea.ValueChanged -= InferParam_ValueChanged;
            numMinArea.ValueChanged += InferParam_ValueChanged;
            chkGpu.CheckedChanged -= ChkGpu_CheckedChanged;
            chkGpu.CheckedChanged += ChkGpu_CheckedChanged;
        }

        private void AttachPropetyEvents()
        {
            Propety.PercentChange -= Propety_PercentChange;
            Propety.PercentChange += Propety_PercentChange;
            Propety.ScoreChanged -= Propety_ScoreChanged;
            Propety.ScoreChanged += Propety_ScoreChanged;
        }

        private void BindPropetyToUi()
        {
            txtModelPath.Text = Propety.pathModel;
            txtSamplesPath.Text = Propety.pathSamplesFolder;
            numTrees.Value = Clamp(Propety.numTrees, numTrees.Minimum, numTrees.Maximum);
            numMaxDepth.Value = Clamp(Propety.maxDepth, numMaxDepth.Minimum, numMaxDepth.Maximum);
            numMinSample.Value = Clamp(Propety.minSampleCount, numMinSample.Minimum, numMinSample.Maximum);
            numThreshold.Value = Clamp((decimal)Propety.defectThreshold, numThreshold.Minimum, numThreshold.Maximum);
            numMinArea.Value = Clamp(Propety.minDefectArea, numMinArea.Minimum, numMinArea.Maximum);
            chkGpu.Checked = Propety.enableGpu;
            lblModelState.Text = Propety.IsIni ? "Model loaded" : "Model not trained";
        }

        private static decimal Clamp(decimal value, decimal min, decimal max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private void TogglePanel_Click(object sender, EventArgs e)
        {
            if (sender == btnHeaderGeneral)
                panelGeneralParams.Visible = !panelGeneralParams.Visible;
            else if (sender == btnHeaderTrainData)
                panelTrainingData.Visible = !panelTrainingData.Visible;
            else if (sender == btnHeaderTrainParams)
                panelTrainParams.Visible = !panelTrainParams.Visible;
            else if (sender == btnHeaderInferParams)
                panelInferParams.Visible = !panelInferParams.Visible;
        }

        private void TrainParam_ValueChanged(object sender, EventArgs e)
        {
            if (Propety == null)
                return;
            Propety.numTrees = (int)numTrees.Value;
            Propety.maxDepth = (int)numMaxDepth.Value;
            Propety.minSampleCount = (int)numMinSample.Value;
        }

        private void InferParam_ValueChanged(object sender, EventArgs e)
        {
            if (Propety == null)
                return;
            Propety.defectThreshold = (float)numThreshold.Value;
            Propety.minDefectArea = (int)numMinArea.Value;
        }

        private void ChkGpu_CheckedChanged(object sender, EventArgs e)
        {
            if (Propety == null)
                return;
            Propety.enableGpu = chkGpu.Checked;
            if (Propety.inferer != null)
                Propety.inferer.SetGpu(Propety.enableGpu);
        }

        private void BtnAddSample_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp;*.tif;*.tiff|All files|*.*";
                dialog.Multiselect = true;
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                foreach (string imagePath in dialog.FileNames)
                    AddSampleFromImage(imagePath);
            }

            RefreshSamplesList();
        }

        private void AddSampleFromImage(string imagePath)
        {
            if (!File.Exists(imagePath))
                return;

            Propety.EnsureStoragePaths(Math.Max(0, Propety.Index));
            using (var painter = new MaskPainter())
            using (var form = CreateMaskEditorForm(painter))
            {
                painter.LoadBackground(imagePath);
                if (form.ShowDialog(this) != DialogResult.OK)
                    return;

                byte[] labels = painter.BuildMaskBytes();
                if (!HasPaintedLabels(labels))
                {
                    MessageBox.Show(this, "Please paint at least one NG or OK brush stroke before saving the sample.", "Segment AI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string stem = "sample_" + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                string sampleImagePath = Path.Combine(Propety.pathSamplesFolder, stem + Path.GetExtension(imagePath));
                string sampleMaskPath = Path.Combine(Propety.pathSamplesFolder, stem + "_mask.png");
                File.Copy(imagePath, sampleImagePath, true);
                SaveMask(labels, painter.MaskWidth, painter.MaskHeight, sampleMaskPath);

                Propety.samples.Add(new SegSample
                {
                    ImagePath = sampleImagePath,
                    MaskPath = sampleMaskPath,
                    Created = DateTime.Now
                });
            }
        }

        private Form CreateMaskEditorForm(MaskPainter painter)
        {
            var form = new Form
            {
                Text = "Segment AI Sample Painter",
                StartPosition = FormStartPosition.CenterParent,
                Size = new DrawingSize(980, 720),
                MinimizeBox = false,
                MaximizeBox = true
            };

            var footer = new Panel { Dock = DockStyle.Bottom, Height = 48, Padding = new Padding(8) };
            var btnOk = new Button { Text = "Save Sample", DialogResult = DialogResult.OK, Dock = DockStyle.Right, Width = 120 };
            var btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Dock = DockStyle.Right, Width = 100 };
            btnOk.Click -= SampleDialogOk_Click;
            btnOk.Click += SampleDialogOk_Click;
            btnCancel.Click -= SampleDialogCancel_Click;
            btnCancel.Click += SampleDialogCancel_Click;
            footer.Controls.Add(btnOk);
            footer.Controls.Add(btnCancel);
            painter.Dock = DockStyle.Fill;
            form.Controls.Add(painter);
            form.Controls.Add(footer);
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;
            return form;
        }

        private void SampleDialogOk_Click(object sender, EventArgs e)
        {
        }

        private void SampleDialogCancel_Click(object sender, EventArgs e)
        {
        }

        private void BtnRemoveSample_Click(object sender, EventArgs e)
        {
            int index = listSamples.SelectedIndex;
            if (Propety.samples == null || index < 0 || index >= Propety.samples.Count)
                return;

            Propety.samples.RemoveAt(index);
            RefreshSamplesList();
        }

        private void BtnClearSamples_Click(object sender, EventArgs e)
        {
            if (Propety.samples == null || Propety.samples.Count == 0)
                return;
            if (MessageBox.Show(this, "Clear all Segment AI samples?", "Segment AI", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            Propety.samples.Clear();
            RefreshSamplesList();
        }

        private void ListSamples_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listSamples.SelectedIndex;
            if (Propety.samples == null || index < 0 || index >= Propety.samples.Count)
                return;

            var sample = Propety.samples[index];
            lblSampleInfo.Text = Path.GetFileName(sample.ImagePath) + " | " + CountMaskLabels(sample.MaskPath);
            ShowSamplePreview(sample);
        }

        private async void BtnTrainStart_Click(object sender, EventArgs e)
        {
            if (Propety.samples == null || Propety.samples.Count == 0)
            {
                MessageBox.Show(this, "Add and paint at least one training sample first.", "Segment AI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TrainParam_ValueChanged(sender, e);
            InferParam_ValueChanged(sender, e);
            SetTrainingUi(true);
            trainCancellation = new CancellationTokenSource();
            progressTrain.Value = 0;
            AppendLog("Training started.");

            bool ok = false;
            try
            {
                CancellationToken token = trainCancellation.Token;
                ok = await Task.Run(() => Propety.Train(ReportTrainProgress, token));
            }
            catch (Exception ex)
            {
                AppendLog("Training failed: " + ex.Message);
            }
            finally
            {
                SetTrainingUi(false);
                trainCancellation.Dispose();
                trainCancellation = null;
            }

            lblModelState.Text = ok ? "Model loaded" : "Training failed";
            AppendLog(ok ? "Model saved: " + Propety.pathModel : "Training did not complete.");
        }

        private void BtnTrainCancel_Click(object sender, EventArgs e)
        {
            if (trainCancellation != null)
                trainCancellation.Cancel();
        }

        private void SetTrainingUi(bool isTraining)
        {
            btnTrainStart.Enabled = !isTraining;
            btnTrainCancel.Enabled = isTraining;
            btnAddSample.Enabled = !isTraining;
            btnRemoveSample.Enabled = !isTraining;
            btnClearSamples.Enabled = !isTraining;
        }

        private void ReportTrainProgress(int value)
        {
            if (IsDisposed)
                return;
            if (InvokeRequired)
            {
                BeginInvoke(new Action<int>(ReportTrainProgress), value);
                return;
            }

            Propety_PercentChange(value);
        }

        private void Propety_PercentChange(int value)
        {
            int percent = Math.Max(progressTrain.Minimum, Math.Min(progressTrain.Maximum, value));
            progressTrain.Value = percent;
            lblTrainStatus.Text = percent + "%";
        }

        private void Propety_ScoreChanged()
        {
            if (IsDisposed)
                return;
            if (InvokeRequired)
            {
                BeginInvoke(new Action(Propety_ScoreChanged));
                return;
            }

            UpdateInferenceLabels();
        }

        private void BtnBrowseTest_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp;*.tif;*.tiff|All files|*.*";
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                currentPreviewImagePath = dialog.FileName;
                txtTestImage.Text = currentPreviewImagePath;
                ShowImage(picPreview, currentPreviewImagePath);
            }
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentPreviewImagePath) || !File.Exists(currentPreviewImagePath))
            {
                BtnBrowseTest_Click(sender, e);
                if (string.IsNullOrEmpty(currentPreviewImagePath) || !File.Exists(currentPreviewImagePath))
                    return;
            }

            try
            {
                InferParam_ValueChanged(sender, e);
                using (Mat image = Cv2.ImRead(currentPreviewImagePath, ImreadModes.Color))
                {
                    Propety.DoWork(image, new CvRect(0, 0, image.Width, image.Height));
                    UpdateInferenceLabels();
                    ShowInferenceOverlay(image);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Segment AI Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshSamplesList()
        {
            listSamples.Items.Clear();
            if (Propety.samples == null)
                return;

            for (int i = 0; i < Propety.samples.Count; i++)
            {
                var sample = Propety.samples[i];
                listSamples.Items.Add((i + 1).ToString("D2") + " - " + Path.GetFileName(sample.ImagePath));
            }

            lblSampleCount.Text = Propety.samples.Count + " samples";
        }

        private void ShowSamplePreview(SegSample sample)
        {
            if (sample == null || !File.Exists(sample.ImagePath))
                return;

            using (Mat image = Cv2.ImRead(sample.ImagePath, ImreadModes.Color))
            using (Mat display = image.Clone())
            {
                if (File.Exists(sample.MaskPath))
                    BlendMask(display, sample.MaskPath);
                ReplacePicture(picSamplePreview, BitmapConverter.ToBitmap(display));
            }
        }

        private void ShowInferenceOverlay(Mat image)
        {
            using (Mat display = image.Clone())
            {
                BlendMask(display, Propety.lastMask, Propety.lastMaskW, Propety.lastMaskH);
                ReplacePicture(picPreview, BitmapConverter.ToBitmap(display));
            }
        }

        private void BlendMask(Mat display, string maskPath)
        {
            using (Mat mask = Cv2.ImRead(maskPath, ImreadModes.Grayscale))
            {
                if (mask.Empty() || mask.Width != display.Width || mask.Height != display.Height)
                    return;
                byte[] labels = new byte[mask.Width * mask.Height];
                Marshal.Copy(mask.Data, labels, 0, labels.Length);
                BlendMask(display, labels, mask.Width, mask.Height);
            }
        }

        private void BlendMask(Mat display, byte[] labels, int width, int height)
        {
            if (labels == null || labels.Length == 0 || width != display.Width || height != display.Height)
                return;

            for (int y = 0; y < height; y++)
            {
                int row = y * width;
                for (int x = 0; x < width; x++)
                {
                    byte label = labels[row + x];
                    if (label == 0)
                        continue;

                    Vec3b color = display.At<Vec3b>(y, x);
                    if (label == 1 || label >= 200)
                    {
                        color.Item0 = (byte)((color.Item0 * 3 + 40) / 4);
                        color.Item1 = (byte)((color.Item1 * 3 + 40) / 4);
                        color.Item2 = (byte)((color.Item2 * 3 + 255) / 4);
                    }
                    else
                    {
                        color.Item0 = (byte)((color.Item0 * 3 + 40) / 4);
                        color.Item1 = (byte)((color.Item1 * 3 + 220) / 4);
                        color.Item2 = (byte)((color.Item2 * 3 + 80) / 4);
                    }
                    display.Set(y, x, color);
                }
            }
        }

        private void SaveMask(byte[] labels, int width, int height, string path)
        {
            byte[] visible = new byte[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                if (labels[i] == 1)
                    visible[i] = 255;
                else if (labels[i] == 2)
                    visible[i] = 128;
            }

            using (var mask = new Mat(height, width, MatType.CV_8UC1))
            {
                Marshal.Copy(visible, 0, mask.Data, visible.Length);
                Cv2.ImWrite(path, mask);
            }
        }

        private bool HasPaintedLabels(byte[] labels)
        {
            if (labels == null)
                return false;
            for (int i = 0; i < labels.Length; i++)
            {
                if (labels[i] == 1 || labels[i] == 2)
                    return true;
            }
            return false;
        }

        private string CountMaskLabels(string maskPath)
        {
            if (string.IsNullOrEmpty(maskPath) || !File.Exists(maskPath))
                return "no mask";

            using (Mat mask = Cv2.ImRead(maskPath, ImreadModes.Grayscale))
            {
                int defect = 0;
                int normal = 0;
                for (int y = 0; y < mask.Rows; y++)
                {
                    for (int x = 0; x < mask.Cols; x++)
                    {
                        byte value = mask.At<byte>(y, x);
                        if (value >= 200)
                            defect++;
                        else if (value >= 100)
                            normal++;
                    }
                }
                return "NG px " + defect + ", OK px " + normal;
            }
        }

        private void UpdateInferenceLabels()
        {
            lblScore.Text = "Score: " + Propety.lastScore.ToString("0.000");
            lblResult.Text = Propety.IsOK ? "Result: OK" : "Result: NG";
            lblDefectPixels.Text = "Defect pixels: " + Propety.Counter;
        }

        private void AppendLog(string message)
        {
            if (txtTrainLog.InvokeRequired)
            {
                txtTrainLog.BeginInvoke(new Action<string>(AppendLog), message);
                return;
            }
            txtTrainLog.AppendText(DateTime.Now.ToString("HH:mm:ss") + "  " + message + Environment.NewLine);
        }

        private void ShowImage(PictureBox pictureBox, string imagePath)
        {
            using (DrawingImage image = DrawingImage.FromFile(imagePath))
                ReplacePicture(pictureBox, new Bitmap(image));
        }

        private void ReplacePicture(PictureBox pictureBox, Bitmap bitmap)
        {
            var old = pictureBox.Image;
            pictureBox.Image = bitmap;
            if (old != null)
                old.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (trainCancellation != null)
                {
                    trainCancellation.Cancel();
                    trainCancellation.Dispose();
                    trainCancellation = null;
                }
                components?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
