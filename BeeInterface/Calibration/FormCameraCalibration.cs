using BeeCore;
using BeeCppCli;
using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface
{
    public class FormCameraCalibration : Form
    {
        private sealed class CaptureEntry
        {
            public CalibrationFrameCli Frame;
            public Bitmap PreviewBitmap;
            public string Status;
        }

        private readonly CameraCalibrationService _calibrationService = new CameraCalibrationService();
        private readonly CameraCalibrationProfileStore _profileStore = new CameraCalibrationProfileStore();
        private readonly CameraPreviewCorrection _previewCorrection = new CameraPreviewCorrection();
        private readonly CameraScaleCalibrationService _scaleService = new CameraScaleCalibrationService();
        private readonly List<CaptureEntry> _captures = new List<CaptureEntry>();
        private readonly Timer _previewTimer = new Timer();
        private readonly Timer _guidanceTimer = new Timer();

        private ComboBox _cbCamera;
        private ComboBox _cbProfiles;
        private ComboBox _cbPatternType;
        private NumericUpDown _numRows;
        private NumericUpDown _numColumns;
        private NumericUpDown _numSpacing;
        private NumericUpDown _numScaleSample;
        private CheckBox _chkApplyPreview;
        private Label _lbProfileStatus;
        private Label _lbSolveStatus;
        private Label _lbScaleStatus;
        private PictureBox _picRaw;
        private PictureBox _picCorrected;
        private ListView _lvCaptures;
        private RJButton _btnCapture;
        private RJButton _btnSolve;
        private RJButton _btnDetectScale;
        private RJButton _btnApplyScale;
        private RJButton _btnSaveProfile;
        private RJButton _btnRefresh;
        private CheckBox _chkGuidance;
        private Label _lbGuidance;

        private Bitmap _rawBitmap;
        private Bitmap _correctedBitmap;
        private CameraCalibrationProfile _workingProfile;
        private ScaleCalibrationCli _lastScaleResult;
        private bool _isRefreshingFrame;
        private bool _isGuidanceBusy;

        public FormCameraCalibration()
        {
            Text = "Camera Calibration";
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new System.Drawing.Size(1180, 720);
            BackColor = Color.FromArgb(245, 245, 245);
            Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);

            BuildUi();
            _previewTimer.Interval = 350;
            _previewTimer.Tick += async (sender, args) => await RefreshPreviewAsync(false);
            _guidanceTimer.Interval = 700;
            _guidanceTimer.Tick += async (sender, args) => await UpdateGuidanceAsync();

            Load += FormCameraCalibration_Load;
            FormClosed += FormCameraCalibration_FormClosed;
        }

        private void BuildUi()
        {
            TableLayoutPanel root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(12)
            };
            root.RowStyles.Add(new RowStyle());
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle());
            Controls.Add(root);

            root.Controls.Add(BuildHeaderPanel(), 0, 0);
            root.Controls.Add(BuildBodyPanel(), 0, 1);
            root.Controls.Add(BuildFooterPanel(), 0, 2);
        }

        private Control BuildHeaderPanel()
        {
            TableLayoutPanel panel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 8,
                Margin = new Padding(0, 0, 0, 12)
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));

            _cbCamera = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            _cbCamera.SelectedIndexChanged += async (sender, args) =>
            {
                LoadSelectedCameraProfile();
                await RefreshPreviewAsync(true);
            };

            _cbProfiles = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            _cbProfiles.SelectedIndexChanged += (sender, args) => LoadSelectedProfile();

            _chkApplyPreview = new CheckBox
            {
                Dock = DockStyle.Fill,
                Text = "Apply Preview",
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };

            _btnRefresh = CreateActionButton("Refresh Frame", async (sender, args) => await RefreshPreviewAsync(true), Color.WhiteSmoke);
            RJButton btnClear = CreateActionButton("Clear Captures", btnClear_Click, Color.WhiteSmoke);
            RJButton btnClose = CreateActionButton("Close", (sender, args) => Close(), Color.WhiteSmoke);

            panel.Controls.Add(CreateHeaderLabel("Camera"), 0, 0);
            panel.Controls.Add(_cbCamera, 1, 0);
            panel.Controls.Add(CreateHeaderLabel("Profile"), 2, 0);
            panel.Controls.Add(_cbProfiles, 3, 0);
            panel.Controls.Add(_chkApplyPreview, 4, 0);
            panel.Controls.Add(_btnRefresh, 5, 0);
            panel.Controls.Add(btnClear, 6, 0);
            panel.Controls.Add(btnClose, 7, 0);
            return panel;
        }

        private Control BuildBodyPanel()
        {
            TableLayoutPanel body = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 370F));
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            body.Controls.Add(BuildControlPanel(), 0, 0);
            body.Controls.Add(BuildPreviewPanel(), 1, 0);
            return body;
        }

        private Control BuildControlPanel()
        {
            TableLayoutPanel panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6,
                Margin = new Padding(0, 0, 12, 0)
            };
            panel.RowStyles.Add(new RowStyle());
            panel.RowStyles.Add(new RowStyle());
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            panel.RowStyles.Add(new RowStyle());
            panel.RowStyles.Add(new RowStyle());
            panel.RowStyles.Add(new RowStyle());
            panel.RowStyles.Add(new RowStyle());

            panel.Controls.Add(BuildPatternPanel(), 0, 0);
            panel.Controls.Add(BuildCaptureButtons(), 0, 1);
            panel.Controls.Add(BuildCaptureList(), 0, 2);
            panel.Controls.Add(BuildSolveStatusPanel(), 0, 3);
            panel.Controls.Add(BuildScalePanel(), 0, 4);
            panel.Controls.Add(BuildProfileButtons(), 0, 5);
            panel.Controls.Add(BuildGuidancePanel(), 0, 6);
            return panel;
        }

        private Control BuildPatternPanel()
        {
            GroupBox group = new GroupBox
            {
                Text = "Pattern",
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10)
            };

            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            _cbPatternType = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            _cbPatternType.Items.AddRange(new object[] { "Chessboard", "Symmetric Circle Grid", "Asymmetric Circle Grid" });
            _cbPatternType.SelectedIndex = 0;

            _numRows = CreateNumber(2, 40, 7, 0);
            _numColumns = CreateNumber(2, 40, 10, 0);
            _numSpacing = CreateNumber(0.01M, 1000M, 1.00M, 2);

            table.Controls.Add(CreateFieldLabel("Type"), 0, 0);
            table.Controls.Add(_cbPatternType, 1, 0);
            table.Controls.Add(CreateFieldLabel("Rows"), 0, 1);
            table.Controls.Add(_numRows, 1, 1);
            table.Controls.Add(CreateFieldLabel("Columns"), 0, 2);
            table.Controls.Add(_numColumns, 1, 2);
            table.Controls.Add(CreateFieldLabel("Spacing (mm)"), 0, 3);
            table.Controls.Add(_numSpacing, 1, 3);

            group.Controls.Add(table);
            return group;
        }

        private Control BuildCaptureButtons()
        {
            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 8, 0, 8)
            };

            _btnCapture = CreateActionButton("Capture Frame", async (sender, args) => await CaptureFrameAsync(), Color.White);
            _btnSolve = CreateActionButton("Solve Profile", async (sender, args) => await SolveAsync(), Color.White);

            panel.Controls.Add(_btnCapture);
            panel.Controls.Add(_btnSolve);
            return panel;
        }

        private Control BuildCaptureList()
        {
            GroupBox group = new GroupBox
            {
                Text = "Captured Frames",
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            _lvCaptures = new ListView
            {
                Dock = DockStyle.Fill,
                View = System.Windows.Forms.View.Details,
                FullRowSelect = true,
                MultiSelect = false,
                HideSelection = false
            };
            _lvCaptures.Columns.Add("#", 40);
            _lvCaptures.Columns.Add("Status", 80);
            _lvCaptures.Columns.Add("Score", 70);
            _lvCaptures.Columns.Add("Message", 150);
            _lvCaptures.SelectedIndexChanged += _lvCaptures_SelectedIndexChanged;

            group.Controls.Add(_lvCaptures);
            return group;
        }

        private Control BuildSolveStatusPanel()
        {
            GroupBox group = new GroupBox
            {
                Text = "Solve Summary",
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10)
            };

            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1
            };

            _lbProfileStatus = CreateStatusLabel("No profile selected.");
            _lbSolveStatus = CreateStatusLabel("Capture 3 or more accepted frames to solve.");
            table.Controls.Add(_lbProfileStatus, 0, 0);
            table.Controls.Add(_lbSolveStatus, 0, 1);
            group.Controls.Add(table);
            return group;
        }

        private Control BuildScalePanel()
        {
            GroupBox group = new GroupBox
            {
                Text = "Scale Calibration",
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10)
            };

            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            _numScaleSample = CreateNumber(0.01M, 10000M, 10.00M, 2);
            _lbScaleStatus = CreateStatusLabel("Run after solving or loading a profile.");
            _btnDetectScale = CreateActionButton("Detect Scale", async (sender, args) => await DetectScaleAsync(), Color.White);
            _btnApplyScale = CreateActionButton("Apply Scale", btnApplyScale_Click, Color.WhiteSmoke);

            FlowLayoutPanel actions = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight
            };
            actions.Controls.Add(_btnDetectScale);
            actions.Controls.Add(_btnApplyScale);

            table.Controls.Add(CreateFieldLabel("Sample size (mm)"), 0, 0);
            table.Controls.Add(_numScaleSample, 1, 0);
            table.Controls.Add(actions, 0, 1);
            table.SetColumnSpan(actions, 2);
            table.Controls.Add(_lbScaleStatus, 0, 2);
            table.SetColumnSpan(_lbScaleStatus, 2);
            group.Controls.Add(table);
            return group;
        }

        private Control BuildProfileButtons()
        {
            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 8, 0, 0)
            };

            _btnSaveProfile = CreateActionButton("Save Profile", btnSaveProfile_Click, Color.White);
            panel.Controls.Add(_btnSaveProfile);
            return panel;
        }

        private Control BuildGuidancePanel()
        {
            GroupBox group = new GroupBox
            {
                Text = "Live Guidance",
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10)
            };

            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1
            };

            _chkGuidance = new CheckBox
            {
                Text = "Enable guidance",
                AutoSize = true
            };
            _chkGuidance.CheckedChanged += _chkGuidance_CheckedChanged;

            _lbGuidance = CreateStatusLabel("Guidance disabled.");
            _lbGuidance.ForeColor = Color.DimGray;

            table.Controls.Add(_chkGuidance, 0, 0);
            table.Controls.Add(_lbGuidance, 0, 1);
            group.Controls.Add(table);
            return group;
        }

        private Control BuildPreviewPanel()
        {
            TableLayoutPanel panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            _picRaw = CreatePreviewBox();
            _picCorrected = CreatePreviewBox();

            panel.Controls.Add(CreatePreviewGroup("Raw / Debug Preview", _picRaw), 0, 0);
            panel.Controls.Add(CreatePreviewGroup("Corrected Preview", _picCorrected), 0, 1);
            return panel;
        }

        private Control BuildFooterPanel()
        {
            Label label = new Label
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                ForeColor = Color.DimGray,
                Padding = new Padding(2, 10, 2, 0),
                Text = "Preview uses the existing camera path only. Inspection input remains raw until later integration."
            };
            return label;
        }

        private async void FormCameraCalibration_Load(object sender, EventArgs e)
        {
            _profileStore.EnsureLoaded();
            LoadCameraList();
            LoadProfileList();
            _chkApplyPreview.Checked = Global.CameraCalibration != null && Global.CameraCalibration.ApplyToPreview;
            _previewTimer.Start();
            await RefreshPreviewAsync(true);
        }

        private void FormCameraCalibration_FormClosed(object sender, FormClosedEventArgs e)
        {
            _previewTimer.Stop();
            _guidanceTimer.Stop();
            ReplaceBitmap(ref _rawBitmap, null);
            ReplaceBitmap(ref _correctedBitmap, null);

            foreach (CaptureEntry capture in _captures)
            {
                if (capture.PreviewBitmap != null)
                    capture.PreviewBitmap.Dispose();
            }
            _captures.Clear();
        }

        private async Task RefreshPreviewAsync(bool updateStatus)
        {
            if (_isRefreshingFrame)
                return;

            Camera camera = GetSelectedCamera();
            if (camera == null)
            {
                if (updateStatus)
                    _lbProfileStatus.Text = "No camera configured for calibration.";
                return;
            }

            _isRefreshingFrame = true;
            try
            {
                using (Mat rawFrame = await AcquireFrameAsync(camera))
                {
                    if (rawFrame == null || rawFrame.Empty())
                    {
                        if (updateStatus)
                            _lbProfileStatus.Text = "Waiting for camera frame.";
                        return;
                    }

                    ReplaceBitmap(ref _rawBitmap, rawFrame.ToBitmap());
                    _picRaw.Image = _rawBitmap;
                    UpdateCorrectedPreview(rawFrame);
                    if (updateStatus)
                        UpdateProfileStatus(rawFrame.Width, rawFrame.Height);
                }
            }
            catch (Exception ex)
            {
                _lbProfileStatus.Text = "Preview error: " + ex.Message;
            }
            finally
            {
                _isRefreshingFrame = false;
            }
        }

        private async Task CaptureFrameAsync()
        {
            Camera camera = GetSelectedCamera();
            if (camera == null)
            {
                _lbSolveStatus.Text = "Select a camera before capture.";
                return;
            }

            using (Mat frame = await AcquireFrameAsync(camera))
            {
                if (frame == null || frame.Empty())
                {
                    _lbSolveStatus.Text = "No frame available for capture.";
                    return;
                }

                CalibrationFrameCli detection = _calibrationService.DetectGrid(
                    frame,
                    _cbPatternType.SelectedIndex,
                    (int)_numRows.Value,
                    (int)_numColumns.Value,
                    (double)_numSpacing.Value);

                Bitmap preview = ExtractDebugBitmap(detection.DebugPtr, detection.DebugW, detection.DebugH, detection.DebugStride, detection.DebugChannels);
                if (preview == null)
                    preview = frame.ToBitmap();

                CaptureEntry entry = new CaptureEntry
                {
                    Frame = detection,
                    PreviewBitmap = preview,
                    Status = detection.Found ? "Accepted" : "Rejected"
                };
                _captures.Add(entry);

                ListViewItem item = new ListViewItem((_captures.Count).ToString());
                item.SubItems.Add(entry.Status);
                item.SubItems.Add(detection.QualityScore.ToString("0.000"));
                item.SubItems.Add(detection.Message ?? string.Empty);
                item.Tag = entry;
                _lvCaptures.Items.Add(item);
                item.Selected = true;

                _lbSolveStatus.Text = string.Format(
                    "{0} frame {1}. Accepted frames: {2}.",
                    entry.Status,
                    _captures.Count,
                    _captures.Count(c => c.Frame != null && c.Frame.Found));
            }
        }

        private async Task SolveAsync()
        {
            int accepted = _captures.Count(c => c.Frame != null && c.Frame.Found);
            if (accepted < 3)
            {
                _lbSolveStatus.Text = "Need at least 3 accepted frames before solve.";
                return;
            }

            Camera camera = GetSelectedCamera();
            if (camera == null)
            {
                _lbSolveStatus.Text = "Camera is not available.";
                return;
            }

            using (Mat frame = await AcquireFrameAsync(camera))
            {
                string cameraId = GetSelectedCameraId();
                _workingProfile = _calibrationService.Solve(
                    _captures.Select(c => c.Frame),
                    cameraId,
                    cameraId,
                    _cbPatternType.SelectedIndex,
                    (int)_numRows.Value,
                    (int)_numColumns.Value,
                    (double)_numSpacing.Value);

                if (_workingProfile == null || _workingProfile.CameraMatrix == null || _workingProfile.CameraMatrix.Length != 9)
                {
                    _lbSolveStatus.Text = "Solve failed.";
                    return;
                }

                _lbSolveStatus.Text = string.Format(
                    "Solved: reprojection error {0:0.0000}, resolution {1}x{2}.",
                    _workingProfile.MeanReprojectionError,
                    _workingProfile.ImageWidth,
                    _workingProfile.ImageHeight);

                if (frame != null && !frame.Empty())
                    UpdateCorrectedPreview(frame);
                UpdateProfileStatus(_workingProfile.ImageWidth, _workingProfile.ImageHeight);
            }
        }

        private async Task DetectScaleAsync()
        {
            if (_workingProfile == null)
            {
                _lbScaleStatus.Text = "Solve or select a profile before scale detection.";
                return;
            }

            Camera camera = GetSelectedCamera();
            if (camera == null)
            {
                _lbScaleStatus.Text = "Camera is not available.";
                return;
            }

            using (Mat rawFrame = await AcquireFrameAsync(camera))
            {
                if (rawFrame == null || rawFrame.Empty())
                {
                    _lbScaleStatus.Text = "No frame available for scale detection.";
                    return;
                }

                Mat sourceForScale = rawFrame;
                Mat corrected = null;
                if (_previewCorrection.TryCorrectPreview(rawFrame, _workingProfile, out corrected))
                    sourceForScale = corrected;

                try
                {
                    _lastScaleResult = _scaleService.DetectScaleSample(sourceForScale, (double)_numScaleSample.Value);
                    Bitmap overlay = ExtractDebugBitmap(
                        _lastScaleResult.DebugPtr,
                        _lastScaleResult.DebugW,
                        _lastScaleResult.DebugH,
                        _lastScaleResult.DebugStride,
                        _lastScaleResult.DebugChannels);

                    if (overlay != null)
                    {
                        ReplaceBitmap(ref _correctedBitmap, overlay);
                        _picCorrected.Image = _correctedBitmap;
                    }

                    if (_lastScaleResult.Found)
                    {
                        _lbScaleStatus.Text = string.Format(
                            "Scale detected: {0:0.000000} mm/px, confidence {1:0.000}.",
                            _lastScaleResult.MmPerPixel,
                            _lastScaleResult.Confidence);
                    }
                    else
                    {
                        _lbScaleStatus.Text = string.IsNullOrWhiteSpace(_lastScaleResult.Message)
                            ? "Scale sample not found."
                            : _lastScaleResult.Message;
                    }
                }
                finally
                {
                    if (corrected != null)
                        corrected.Dispose();
                }
            }
        }

        private void btnApplyScale_Click(object sender, EventArgs e)
        {
            if (_workingProfile == null)
            {
                _lbScaleStatus.Text = "No working profile to update.";
                return;
            }
            if (!_scaleService.ApplyScaleToProfile(_workingProfile, _lastScaleResult))
            {
                _lbScaleStatus.Text = "Run a successful scale detection first.";
                return;
            }
            if (_scaleService.ApplyScaleToGlobalConfig(_workingProfile))
            {
                SaveData.Config(Global.Config);
                _lbScaleStatus.Text = string.Format(
                    "Scale applied to profile and shared config: {0:0.000000} mm/px.",
                    _workingProfile.ScaleMmPerPixel);
            }
            else
            {
                _lbScaleStatus.Text = "Scale stored in profile, but shared config is not ready.";
            }
        }

        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            if (_workingProfile == null)
            {
                _lbProfileStatus.Text = "Nothing to save yet.";
                return;
            }

            _workingProfile.IsValidatedForPreview = true;
            CameraCalibrationProfile saved = _profileStore.Upsert(_workingProfile);
            _profileStore.SetActive(saved.ProfileId);
            Global.CameraCalibration.ApplyToPreview = _chkApplyPreview.Checked;
            Global.CameraCalibration.ApplyToInspectionInput = false;
            _profileStore.Save();
            LoadProfileList();
            SelectProfile(saved.ProfileId);
            _lbProfileStatus.Text = "Profile saved to Common\\CameraCalibration.config.";
        }

        private void _chkGuidance_CheckedChanged(object sender, EventArgs e)
        {
            if (_chkGuidance.Checked)
            {
                _guidanceTimer.Start();
                _lbGuidance.Text = "Guidance enabled. Waiting for frame.";
            }
            else
            {
                _guidanceTimer.Stop();
                _lbGuidance.Text = "Guidance disabled.";
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            foreach (CaptureEntry capture in _captures)
            {
                if (capture.PreviewBitmap != null)
                    capture.PreviewBitmap.Dispose();
            }

            _captures.Clear();
            _lvCaptures.Items.Clear();
            _workingProfile = GetSelectedStoredProfileClone();
            _lastScaleResult = null;
            _lbSolveStatus.Text = "Capture 3 or more accepted frames to solve.";
            _lbScaleStatus.Text = "Run after solving or loading a profile.";
        }

        private void _lvCaptures_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_lvCaptures.SelectedItems.Count == 0)
                return;

            CaptureEntry entry = _lvCaptures.SelectedItems[0].Tag as CaptureEntry;
            if (entry == null || entry.PreviewBitmap == null)
                return;

            ReplaceBitmap(ref _rawBitmap, new Bitmap(entry.PreviewBitmap));
            _picRaw.Image = _rawBitmap;
        }

        private void LoadCameraList()
        {
            _cbCamera.Items.Clear();
            for (int index = 0; index < BeeCore.Common.listCamera.Count; ++index)
            {
                Camera camera = BeeCore.Common.listCamera[index];
                ParaCamera para = Global.listParaCamera != null && index < Global.listParaCamera.Count ? Global.listParaCamera[index] : null;
                string name = para != null && !string.IsNullOrWhiteSpace(para.Name)
                    ? para.Name
                    : "Camera " + (index + 1);
                _cbCamera.Items.Add(new CameraItem(index, name, camera));
            }

            if (_cbCamera.Items.Count > 0)
            {
                int preferred = Math.Max(0, Math.Min(Global.IndexCCCD, _cbCamera.Items.Count - 1));
                _cbCamera.SelectedIndex = preferred;
            }
        }

        private void LoadProfileList()
        {
            CameraCalibrationConfig config = _profileStore.EnsureLoaded();
            _cbProfiles.Items.Clear();
            foreach (CameraCalibrationProfile profile in config.Profiles.Where(p => p != null))
                _cbProfiles.Items.Add(new ProfileItem(profile));

            if (_cbProfiles.Items.Count > 0)
            {
                string activeProfileId = config.ActiveProfileId;
                SelectProfile(activeProfileId);
            }
        }

        private void LoadSelectedCameraProfile()
        {
            CameraCalibrationProfile profile = _profileStore.FindProfile(GetSelectedCameraId(), CurrentFrameWidthHint(), CurrentFrameHeightHint());
            if (profile != null)
            {
                _workingProfile = CloneProfile(profile);
                SelectProfile(profile.ProfileId);
            }
            else
            {
                _workingProfile = null;
                UpdateProfileStatus(CurrentFrameWidthHint(), CurrentFrameHeightHint());
            }
        }

        private void LoadSelectedProfile()
        {
            ProfileItem selected = _cbProfiles.SelectedItem as ProfileItem;
            _workingProfile = selected == null ? null : CloneProfile(selected.Profile);
            UpdateProfileStatus(CurrentFrameWidthHint(), CurrentFrameHeightHint());
        }

        private void UpdateProfileStatus(int width, int height)
        {
            CameraCalibrationProfile profile = _workingProfile ?? GetSelectedStoredProfileClone();
            if (profile == null)
            {
                _lbProfileStatus.Text = "No saved profile for this camera yet.";
                return;
            }

            string cameraId = GetSelectedCameraId();
            bool valid = _calibrationService.ValidateProfileForFrame(profile, width, height, cameraId);
            _lbProfileStatus.Text = valid
                ? string.Format("Profile ready for preview: {0}x{1}, error {2:0.0000}.", profile.ImageWidth, profile.ImageHeight, profile.MeanReprojectionError)
                : string.Format("Profile loaded but resolution/camera mismatch. Expected {0}x{1}.", profile.ImageWidth, profile.ImageHeight);
        }

        private void UpdateCorrectedPreview(Mat rawFrame)
        {
            CameraCalibrationProfile profile = _workingProfile ?? GetSelectedStoredProfileClone();
            if (profile == null)
            {
                ReplaceBitmap(ref _correctedBitmap, null);
                _picCorrected.Image = null;
                return;
            }

            Mat corrected;
            if (_previewCorrection.TryCorrectPreview(rawFrame, profile, out corrected))
            {
                using (corrected)
                {
                    ReplaceBitmap(ref _correctedBitmap, corrected.ToBitmap());
                    _picCorrected.Image = _correctedBitmap;
                }
            }
            else
            {
                ReplaceBitmap(ref _correctedBitmap, null);
                _picCorrected.Image = null;
            }
        }

        private async Task<Mat> AcquireFrameAsync(Camera camera)
        {
            if (camera == null)
                return null;

            try
            {
                if (camera.matRaw != null && !camera.matRaw.IsDisposed && !camera.matRaw.Empty())
                    return camera.matRaw.Clone();
            }
            catch
            {
            }

            await Task.Run(() => camera.Read());
            try
            {
                if (camera.matRaw != null && !camera.matRaw.IsDisposed && !camera.matRaw.Empty())
                    return camera.matRaw.Clone();
            }
            catch
            {
            }

            return null;
        }

        private Camera GetSelectedCamera()
        {
            CameraItem item = _cbCamera.SelectedItem as CameraItem;
            return item == null ? null : item.Camera;
        }

        private string GetSelectedCameraId()
        {
            CameraItem item = _cbCamera.SelectedItem as CameraItem;
            return item == null ? string.Empty : item.Name;
        }

        private int CurrentFrameWidthHint()
        {
            Camera camera = GetSelectedCamera();
            if (camera == null || camera.matRaw == null || camera.matRaw.IsDisposed || camera.matRaw.Empty())
                return 0;
            return camera.matRaw.Width;
        }

        private int CurrentFrameHeightHint()
        {
            Camera camera = GetSelectedCamera();
            if (camera == null || camera.matRaw == null || camera.matRaw.IsDisposed || camera.matRaw.Empty())
                return 0;
            return camera.matRaw.Height;
        }

        private CameraCalibrationProfile GetSelectedStoredProfileClone()
        {
            ProfileItem item = _cbProfiles.SelectedItem as ProfileItem;
            return item == null ? null : CloneProfile(item.Profile);
        }

        private async Task UpdateGuidanceAsync()
        {
            if (_isGuidanceBusy || !_chkGuidance.Checked)
                return;

            Camera camera = GetSelectedCamera();
            if (camera == null)
            {
                _lbGuidance.Text = "Guidance unavailable: no camera.";
                return;
            }

            _isGuidanceBusy = true;
            try
            {
                using (Mat frame = await AcquireFrameAsync(camera))
                {
                    if (frame == null || frame.Empty())
                    {
                        _lbGuidance.Text = "Guidance waiting for frame.";
                        return;
                    }

                    LiveGuidanceCli guidance = await Task.Run(() => _calibrationService.AnalyzeLiveGuidance(
                        frame,
                        _workingProfile,
                        _cbPatternType.SelectedIndex,
                        (int)_numRows.Value,
                        (int)_numColumns.Value,
                        (double)_numSpacing.Value));

                    _lbGuidance.Text = string.IsNullOrWhiteSpace(guidance.Message)
                        ? "Guidance: target not found."
                        : "Guidance: " + guidance.Message;

                    Bitmap overlay = ExtractDebugBitmap(
                        guidance.DebugPtr,
                        guidance.DebugW,
                        guidance.DebugH,
                        guidance.DebugStride,
                        guidance.DebugChannels);
                    if (overlay != null)
                    {
                        ReplaceBitmap(ref _correctedBitmap, overlay);
                        _picCorrected.Image = _correctedBitmap;
                    }
                }
            }
            catch (Exception ex)
            {
                _lbGuidance.Text = "Guidance error: " + ex.Message;
            }
            finally
            {
                _isGuidanceBusy = false;
            }
        }

        private void SelectProfile(string profileId)
        {
            if (string.IsNullOrWhiteSpace(profileId))
                return;

            for (int index = 0; index < _cbProfiles.Items.Count; ++index)
            {
                ProfileItem item = _cbProfiles.Items[index] as ProfileItem;
                if (item != null && item.Profile != null && item.Profile.ProfileId == profileId)
                {
                    _cbProfiles.SelectedIndex = index;
                    break;
                }
            }
        }

        private static CameraCalibrationProfile CloneProfile(CameraCalibrationProfile profile)
        {
            if (profile == null)
                return null;

            return new CameraCalibrationProfile
            {
                ProfileId = profile.ProfileId,
                CameraId = profile.CameraId,
                CameraName = profile.CameraName,
                ImageWidth = profile.ImageWidth,
                ImageHeight = profile.ImageHeight,
                LensName = profile.LensName,
                WorkingDistanceMm = profile.WorkingDistanceMm,
                PatternType = profile.PatternType,
                PatternRows = profile.PatternRows,
                PatternColumns = profile.PatternColumns,
                PatternSpacingMm = profile.PatternSpacingMm,
                CameraMatrix = profile.CameraMatrix == null ? new double[0] : (double[])profile.CameraMatrix.Clone(),
                DistortionCoefficients = profile.DistortionCoefficients == null ? new double[0] : (double[])profile.DistortionCoefficients.Clone(),
                RectificationHomography = profile.RectificationHomography == null ? new double[0] : (double[])profile.RectificationHomography.Clone(),
                MeanReprojectionError = profile.MeanReprojectionError,
                PerFrameErrors = profile.PerFrameErrors == null ? new double[0] : (double[])profile.PerFrameErrors.Clone(),
                ScaleMmPerPixel = profile.ScaleMmPerPixel,
                ScaleSampleRealSizeMm = profile.ScaleSampleRealSizeMm,
                ScaleSamplePixelSize = profile.ScaleSamplePixelSize,
                CreatedAt = profile.CreatedAt,
                UpdatedAt = profile.UpdatedAt,
                IsValidatedForPreview = profile.IsValidatedForPreview,
                IsValidatedForInspection = profile.IsValidatedForInspection
            };
        }

        private static Bitmap ExtractDebugBitmap(IntPtr pointer, int width, int height, int stride, int channels)
        {
            if (pointer == IntPtr.Zero)
                return null;
            if (width <= 0 || height <= 0 || stride <= 0 || channels <= 0)
            {
                CameraCalibration.FreeBuffer(pointer);
                return null;
            }

            try
            {
                MatType type = channels == 1 ? MatType.CV_8UC1 : (channels == 4 ? MatType.CV_8UC4 : MatType.CV_8UC3);
                using (Mat view = Mat.FromPixelData(height, width, type, pointer, stride))
                using (Mat cloned = view.Clone())
                {
                    return cloned.ToBitmap();
                }
            }
            finally
            {
                CameraCalibration.FreeBuffer(pointer);
            }
        }

        private static void ReplaceBitmap(ref Bitmap target, Bitmap next)
        {
            if (target != null)
                target.Dispose();
            target = next;
        }

        private static GroupBox CreatePreviewGroup(string title, PictureBox pictureBox)
        {
            GroupBox group = new GroupBox
            {
                Text = title,
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            group.Controls.Add(pictureBox);
            return group;
        }

        private static PictureBox CreatePreviewBox()
        {
            return new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private static NumericUpDown CreateNumber(decimal min, decimal max, decimal value, int decimals)
        {
            NumericUpDown numeric = new NumericUpDown
            {
                Dock = DockStyle.Fill,
                Minimum = min,
                Maximum = max,
                DecimalPlaces = decimals,
                Value = value
            };
            if (decimals > 0)
                numeric.Increment = 0.1M;
            return numeric;
        }

        private static Label CreateHeaderLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
                Padding = new Padding(0, 8, 0, 0)
            };
        }

        private static Label CreateFieldLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
                Padding = new Padding(0, 6, 0, 0)
            };
        }

        private static Label CreateStatusLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                AutoSize = true,
                ForeColor = Color.DimGray,
                Padding = new Padding(0, 2, 0, 2)
            };
        }

        private static RJButton CreateActionButton(string text, EventHandler onClick, Color backColor)
        {
            RJButton button = new RJButton
            {
                AutoFont = true,
                AutoImage = false,
                AutoSize = false,
                Width = 140,
                Height = 38,
                Text = text,
                IsNotChange = true,
                IsUnGroup = true,
                BackColor = backColor,
                BackgroundColor = backColor,
                BorderColor = Color.Gainsboro,
                BorderRadius = 6,
                BorderSize = 1,
                TextColor = Color.Black,
                Margin = new Padding(0, 0, 8, 8)
            };
            button.Click += onClick;
            return button;
        }

        private sealed class CameraItem
        {
            public CameraItem(int index, string name, Camera camera)
            {
                Index = index;
                Name = name;
                Camera = camera;
            }

            public int Index { get; private set; }
            public string Name { get; private set; }
            public Camera Camera { get; private set; }

            public override string ToString()
            {
                return string.Format("{0}: {1}", Index + 1, Name);
            }
        }

        private sealed class ProfileItem
        {
            public ProfileItem(CameraCalibrationProfile profile)
            {
                Profile = profile;
            }

            public CameraCalibrationProfile Profile { get; private set; }

            public override string ToString()
            {
                return string.Format(
                    "{0} ({1}x{2})",
                    string.IsNullOrWhiteSpace(Profile.CameraName) ? Profile.ProfileId : Profile.CameraName,
                    Profile.ImageWidth,
                    Profile.ImageHeight);
            }
        }
    }
}
