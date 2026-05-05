using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BeeInterface
{
    [DefaultEvent(nameof(RoiActionClicked))]
    public partial class RoiToolbar : UserControl
    {
        private bool _showMaskGroup = true;
        private bool _showPolygon = true;
        private bool _showSampling = true;

        public RoiToolbar()
        {
            InitializeComponent();
            WireButtonActions();
            ApplyVisibility();
        }

        public event EventHandler<RoiToolEventArgs> RoiActionClicked;

        [Category("Behavior")]
        [DefaultValue(true)]
        public bool ShowMaskGroup
        {
            get { return _showMaskGroup; }
            set
            {
                if (_showMaskGroup == value) return;
                _showMaskGroup = value;
                ApplyVisibility();
            }
        }

        [Category("Behavior")]
        [DefaultValue(true)]
        public bool ShowPolygon
        {
            get { return _showPolygon; }
            set
            {
                if (_showPolygon == value) return;
                _showPolygon = value;
                ApplyVisibility();
            }
        }

        [Category("Behavior")]
        [DefaultValue(true)]
        public bool ShowSampling
        {
            get { return _showSampling; }
            set
            {
                if (_showSampling == value) return;
                _showSampling = value;
                ApplyVisibility();
            }
        }

        private void WireButtonActions()
        {
            Wire(btnRect, RoiAction.Rect);
            Wire(btnEllipse, RoiAction.Ellipse);
            Wire(btnPolygon, RoiAction.Polygon);
            Wire(btnHexagon, RoiAction.Hexagon);
            Wire(btnNewShape, RoiAction.NewShape);
            Wire(btnArea, RoiAction.Area);
            Wire(btnCrop, RoiAction.Crop);
            Wire(btnMask, RoiAction.Mask);
            Wire(btnInsideOut, RoiAction.InsideOut);
            Wire(btnOutsideIn, RoiAction.OutsideIn);
            Wire(btnCropFull, RoiAction.CropFull);
            Wire(btnCropHalt, RoiAction.CropHalt);
            Wire(btnBlack, RoiAction.Black);
            Wire(btnWhite, RoiAction.White);
        }

        private void Wire(Button button, RoiAction action)
        {
            button.Tag = action;
            button.Click -= OnActionButtonClick;
            button.Click += OnActionButtonClick;
        }

        private void OnActionButtonClick(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null || !(button.Tag is RoiAction))
                return;

            var handler = RoiActionClicked;
            if (handler != null)
                handler(this, new RoiToolEventArgs((RoiAction)button.Tag));
        }

        private void ApplyVisibility()
        {
            if (btnMask == null)
                return;

            btnPolygon.Visible = _showPolygon;
            btnHexagon.Visible = _showPolygon;
            btnNewShape.Visible = _showPolygon;

            btnMask.Visible = _showMaskGroup;
            btnInsideOut.Visible = _showMaskGroup;
            btnOutsideIn.Visible = _showMaskGroup;

            btnBlack.Visible = _showSampling;
            btnWhite.Visible = _showSampling;
        }
    }

    public enum RoiAction
    {
        Rect,
        Ellipse,
        Polygon,
        Hexagon,
        NewShape,
        Area,
        Crop,
        Mask,
        InsideOut,
        OutsideIn,
        CropFull,
        CropHalt,
        Black,
        White
    }

    public sealed class RoiToolEventArgs : EventArgs
    {
        public RoiToolEventArgs(RoiAction action)
        {
            Action = action;
        }

        public RoiAction Action { get; private set; }
    }
}
