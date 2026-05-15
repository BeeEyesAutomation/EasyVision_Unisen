using BeeCore;
using BeeCpp;
using BeeCore.Core;
using BeeCore.Funtion.Engines;
using BeeGlobal;
using Newtonsoft.Json.Linq;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShapeType = BeeGlobal.ShapeType;

namespace BeeInterface
{
    [Serializable()]
    public partial class ToolColorArea : UserControl
    {
     
        #region OwnerTool cache (Phase 2 refactor)
        private PropetyTool _ownerTool;
        private PropetyTool OwnerTool
        {
            get
            {
                if (_ownerTool == null)
                    _ownerTool = Common.TryGetTool(Global.IndexProgChoose, Propety.Index);
                return _ownerTool;
            }
        }
        private void InvalidateOwnerToolCache() => _ownerTool = null;
        #endregion
        public TypeTool TypeTool;
        public ToolColorArea( )
        {
            InitializeComponent();
            InitializeColorAreaMultiUi();
            if (Propety == null)
                Propety = new BeeCore.ColorArea();
        }
        public int indexTool;
        public BeeCore.ColorArea Propety { get; set; }
        public bool IsClear;
        public bool IsIni = false;
        View view = null;
        private bool _isSyncingColorAreaMultiUi;
        
        private ComboBox cboColorAreaScan;
        private ListBox lstColorAreaLists;
        private TextBox txtColorListName;
        private CheckBox chkColorListEnabled;
      //  private ComboBox cboColorListType;
        private Button btnColorListAdd;
        private Button btnColorListDuplicate;
        private Button btnColorListRemove;

        private void InitializeColorAreaMultiUi()
        {
            if (layMultiColor == null )
                return;
           
            layMultiColor.SuspendLayout();
            layMultiColor.Controls.Clear();
            layMultiColor.RowStyles.Clear();
            layMultiColor.AutoScroll = true;
            layMultiColor.RowCount = 6;
            for (int i = 0; i < layMultiColor.RowCount - 1; i++)
                layMultiColor.RowStyles.Add(new RowStyle());
            layMultiColor.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

         //    layMultiColor.Controls.Add(CreateLabeledControl("Mode", cboColorAreaMode), 0, 0);

            cboColorAreaScan = CreateCombo(new[] { "X", "Y" });
            cboColorAreaScan.SelectedIndexChanged += ColorAreaScanChanged;
            layMultiColor.Controls.Add(CreateLabeledControl("Scan Direction", cboColorAreaScan), 0, 1);

            lstColorAreaLists = new ListBox();
            lstColorAreaLists.Dock = DockStyle.Fill;
            lstColorAreaLists.Height = 120;
            lstColorAreaLists.Font = new Font("Segoe UI", 11F);
            lstColorAreaLists.SelectedIndexChanged += ColorAreaListSelectedChanged;
            layMultiColor.Controls.Add(lstColorAreaLists, 0, 2);

            TableLayoutPanel buttonRow = new TableLayoutPanel();
            buttonRow.ColumnCount = 3;
            buttonRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            buttonRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            buttonRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            buttonRow.Dock = DockStyle.Top;
            buttonRow.Height = 42;
            btnColorListAdd = CreateButton("Add");
            btnColorListDuplicate = CreateButton("Copy");
            btnColorListRemove = CreateButton("Remove");
            btnColorListAdd.Click += ColorListAddClicked;
            btnColorListDuplicate.Click += ColorListDuplicateClicked;
            btnColorListRemove.Click += ColorListRemoveClicked;
            buttonRow.Controls.Add(btnColorListAdd, 0, 0);
            buttonRow.Controls.Add(btnColorListDuplicate, 1, 0);
            buttonRow.Controls.Add(btnColorListRemove, 2, 0);
            layMultiColor.Controls.Add(buttonRow, 0, 3);

            txtColorListName = new TextBox();
            txtColorListName.Dock = DockStyle.Fill;
            txtColorListName.Font = new Font("Segoe UI", 11F);
            txtColorListName.TextChanged += ColorListNameChanged;
            layMultiColor.Controls.Add(CreateLabeledControl("Name", txtColorListName), 0, 4);

            chkColorListEnabled = new CheckBox();
            chkColorListEnabled.Text = "Enabled";
            chkColorListEnabled.Dock = DockStyle.Top;
            chkColorListEnabled.Font = new Font("Segoe UI", 11F);
            chkColorListEnabled.CheckedChanged += ColorListEnabledChanged;
            layMultiColor.Controls.Add(chkColorListEnabled, 0, 5);

           // cboColorListType = CreateCombo(new[] { "HSV", "RGB" });
            //  cboColorListType.SelectedIndexChanged += ColorListTypeChanged;
            //  layMultiColor.Controls.Add(CreateLabeledControl("Color", cboColorListType), 0, 6);

            layMultiColor.ResumeLayout();
        }

        private ComboBox CreateCombo(string[] items)
        {
            ComboBox combo = new ComboBox();
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.Dock = DockStyle.Fill;
            combo.Font = new Font("Segoe UI", 11F);
            combo.Items.AddRange(items);
            return combo;
        }

        private NumericUpDown CreateNumber(decimal min, decimal max)
        {
            NumericUpDown number = new NumericUpDown();
            number.Dock = DockStyle.Fill;
            number.Font = new Font("Segoe UI", 11F);
            number.Minimum = min;
            number.Maximum = max;
            number.DecimalPlaces = 0;
            number.Increment = 1;
            return number;
        }

        private Button CreateButton(string text)
        {
            Button button = new Button();
            button.Dock = DockStyle.Fill;
            button.Text = text;
            button.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            return button;
        }

        private Control CreateLabeledControl(string text, Control control)
        {
            TableLayoutPanel panel = new TableLayoutPanel();
            panel.ColumnCount = 2;
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panel.Dock = DockStyle.Top;
            panel.Height = 40;

            Label label = new Label();
            label.Text = text;
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            panel.Controls.Add(label, 0, 0);
            panel.Controls.Add(control, 1, 0);
            return panel;
        }

        public void LoadPara( )
        {
            Propety.EnsureMultiListModel();
            btnScanSingle.IsCLick = Propety.CheckMode == ColorAreaCheckMode.Single ? true : false;
            btnScanMulti.IsCLick = Propety.CheckMode == ColorAreaCheckMode.MultiList ? true : false;

            layMultiColor.Visible = Propety.CheckMode == ColorAreaCheckMode.MultiList ? true : false;

            EditRectRot1.Rot = new List<RectRotate> { Propety.rotArea, Propety.rotMask };
            EditRectRot1.Refresh();
            EditRectRot1.RotateCurentChanged -= EditRectRot1_RotateCurentChanged;
            EditRectRot1.RotateCurentChanged += EditRectRot1_RotateCurentChanged;
            EditRectRot1.IsHide = false;
           
            this.VisibleChanged -= ToolColorArea_VisibleChanged;
            this.VisibleChanged += ToolColorArea_VisibleChanged;
            Global.SetColorChange -= Global_SetColorChange;
            Global.SetColorChange += Global_SetColorChange;
            if (Propety.listCLShow==null)
                Propety.listCLShow = new List<Color>();
            var state = ColorAreaEngineRunner.ReadFromOwner(OwnerTool, Propety);
            trackScore.Min = state.ScoreMin;
            trackScore.Max = state.ScoreMax;
            trackScore.Step = state.ScoreStep;
            trackScore.Value = state.Score;
            _isSyncingColorAreaMultiUi = true;
            AdjValueTemp.Value = Propety.PxTemp;
            _isSyncingColorAreaMultiUi = false;
            btnNGLess.IsCLick = Propety.IsNGLess;
            btnNGMore.IsCLick = Propety.IsNGMore;
            AdjClose.Value = Propety.SizeClose;
            AdjOpen.Value = Propety.SizeOpen;
            AdjClearNoise.Value = Propety.SizeClearsmall;
            AdjClearBig.Value = Propety.SizeClearBig;
            btnClose.IsCLick = Propety.IsClose;
            btnOpen.IsCLick = Propety.IsOpen;
            btnIsClearSmall.IsCLick = Propety.IsClearNoiseSmall;
            btnIsClearBig.IsCLick = Propety.IsClearNoiseBig;
            AdjClearNoise.Enabled = Propety.IsClearNoiseSmall;
            AdjClearBig.Enabled = Propety.IsClearNoiseBig;
            AdjOpen.Enabled = Propety.IsOpen;
            AdjClose.Enabled = Propety.IsClose;
             if (OwnerTool != null)
             {
                 OwnerTool.StatusToolChanged -= ToolColorArea_StatusToolChanged;
                 OwnerTool.StatusToolChanged += ToolColorArea_StatusToolChanged;
             }
             if (OwnerTool != null)
             {
                 OwnerTool.ScoreChanged -= ToolColorArea_ScoreChanged;
                 OwnerTool.ScoreChanged += ToolColorArea_ScoreChanged;
             }
        
            _isSyncingColorAreaMultiUi = true;
            trackPixel.Value = (int)Propety.Extraction;
            _isSyncingColorAreaMultiUi = false;
            //ColorAreaColorList list = GetSelectedColorList();
            //if (list == null)
            //    return;
            //list.TypeColor = cboColorListType.SelectedIndex == 1 ? ColorGp.RGB : ColorGp.HSV;
            //Propety.TypeColor = list.TypeColor;
            switch (Propety.TypeColor)
            {
                case ColorGp.RGB:
                    btnRGB.IsCLick = true;
                    break;
                case ColorGp.HSV:
                    btnHSV.IsCLick = true;
                    break;
            }
          
              
            btnGetColor.IsCLick = Propety.IsGetColor;

            trackScore.Value = state.Score;
            UpdateColorAreaMultiUi();
         
            RefreshSelectedColorPalette();


        }

        private void UpdateColorAreaMultiUi()
        {
            if ( Propety == null)
                return;

            _isSyncingColorAreaMultiUi = true;
            try
            {
                Propety.EnsureMultiListModel();
              
                cboColorAreaScan.SelectedIndex = Propety.ScanDirection == ColorAreaScanDirection.Y ? 1 : 0;
                bool multi = Propety.CheckMode == ColorAreaCheckMode.MultiList;
                cboColorAreaScan.Enabled = multi;
                lstColorAreaLists.Enabled = multi;
                txtColorListName.Enabled = multi;
                chkColorListEnabled.Enabled = multi;
              //  cboColorListType.Enabled = multi;
                btnColorListAdd.Enabled = multi;
                btnColorListDuplicate.Enabled = multi;
                btnColorListRemove.Enabled = multi;
                RefreshColorListDisplay();
                LoadSelectedColorListToUi();
            }
            finally
            {
                _isSyncingColorAreaMultiUi = false;
            }
        }

        private void RefreshColorListDisplay()
        {
            if (lstColorAreaLists == null || Propety == null)
                return;

            int selected = lstColorAreaLists.SelectedIndex;
            lstColorAreaLists.BeginUpdate();
            try
            {
                lstColorAreaLists.Items.Clear();
                Propety.EnsureMultiListModel();
                for (int i = 0; i < Propety.ColorLists.Count; i++)
                {
                    ColorAreaColorList list = Propety.ColorLists[i];
                    string state = list.IsEnabled ? "" : " [Off]";
                    lstColorAreaLists.Items.Add((i + 1) + ". " + list.Name + state);
                }
                if (lstColorAreaLists.Items.Count > 0)
                {
                    if (selected < 0 || selected >= lstColorAreaLists.Items.Count)
                        selected = 0;
                    lstColorAreaLists.SelectedIndex = selected;
                }
            }
            finally
            {
                lstColorAreaLists.EndUpdate();
            }
        }

        private ColorAreaColorList GetSelectedColorList()
        {
            if (Propety == null)
                return null;

            Propety.EnsureMultiListModel();
            int index = lstColorAreaLists != null ? lstColorAreaLists.SelectedIndex : 0;
            if (index < 0 || index >= Propety.ColorLists.Count)
                index = 0;
            return Propety.ColorLists.Count > 0 ? Propety.ColorLists[index] : null;
        }

        private void LoadSelectedColorListToUi()
        {
            ColorAreaColorList list = GetSelectedColorList();
            if (list == null)
                return;

            txtColorListName.Text = list.Name;
            chkColorListEnabled.Checked = list.IsEnabled;
            btnRGB.IsCLick= list.TypeColor == ColorGp.RGB?true:false;
            btnHSV.IsCLick = list.TypeColor == ColorGp.HSV ? true : false;
            //cboColorListType.SelectedIndex = list.TypeColor == ColorGp.RGB ? 1 : 0;
            if (Propety.CheckMode == ColorAreaCheckMode.MultiList)
            {
                trackPixel.Value = list.Extraction;
                AdjValueTemp.Value = list.PixelTemplate;
                btnHSV.IsCLick = list.TypeColor == ColorGp.HSV;
                btnRGB.IsCLick = list.TypeColor == ColorGp.RGB;
                RefreshSelectedColorPalette();
            }
        }

        private decimal ClampDecimal(int value, decimal min, decimal max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private void RefreshSelectedColorPalette()
        {
            ColorAreaColorList list = GetSelectedColorList();
            if (list == null)
                return;

            Propety.listCLShow = new List<Color>();
            if (list.TypeColor == ColorGp.HSV)
            {
                AddHsvColorsToPalette(list.HSVs);
                AddHsvColorsToPalette(list.ExternHSVs);
            }
            else
            {
                AddRgbColorsToPalette(list.RGBs);
                AddRgbColorsToPalette(list.ExternRGBs);
            }
            picColor.Invalidate();
        }

        private void RefreshSinglePalette()
        {
            if (Propety == null) return;
            if (Propety.listCLShow == null)
                Propety.listCLShow = new List<Color>();
            else
                Propety.listCLShow.Clear();

            if (Propety.TypeColor == ColorGp.HSV)
                AddHsvColorsToPalette(Propety.HSVs);
            else
                AddRgbColorsToPalette(Propety.RGBs);

            picColor.Invalidate();
        }

        private void RefreshParamsForCurrentMode()
        {
            if (Propety == null) return;

            _isSyncingColorAreaMultiUi = true;
            try
            {
                if (Propety.CheckMode == ColorAreaCheckMode.MultiList)
                {
                    Propety.EnsureMultiListModel();
                    LoadSelectedColorListToUi();
                }
                else
                {
                    trackPixel.Value = Propety.Extraction;
                    AdjValueTemp.Value = Propety.PxTemp;
                    btnHSV.IsCLick = Propety.TypeColor == ColorGp.HSV;
                    btnRGB.IsCLick = Propety.TypeColor == ColorGp.RGB;
                    RefreshSinglePalette();
                }
            }
            finally
            {
                _isSyncingColorAreaMultiUi = false;
            }
        }

        private void AddHsvColorsToPalette(List<HSV> hsvs)
        {
            if (hsvs == null)
                return;
            foreach (HSV hsv in hsvs)
                Propety.listCLShow.Add(HsvConvert.FromHsvOpenCv((byte)hsv.H, (byte)hsv.S, (byte)hsv.V));
        }

        private void AddRgbColorsToPalette(List<RGB> rgbs)
        {
            if (rgbs == null)
                return;
            foreach (RGB rgb in rgbs)
                Propety.listCLShow.Add(Color.FromArgb(rgb.R, rgb.G, rgb.B));
        }

     
        private void ColorAreaScanChanged(object sender, EventArgs e)
        {
            if (_isSyncingColorAreaMultiUi || Propety == null)
                return;
            Propety.ScanDirection = cboColorAreaScan.SelectedIndex == 1 ? ColorAreaScanDirection.Y : ColorAreaScanDirection.X;
        }

        private void ColorAreaListSelectedChanged(object sender, EventArgs e)
        {
            if (_isSyncingColorAreaMultiUi)
                return;
            _isSyncingColorAreaMultiUi = true;
            try
            {
                LoadSelectedColorListToUi();
            }
            finally
            {
                _isSyncingColorAreaMultiUi = false;
            }
        }

        private void ColorListAddClicked(object sender, EventArgs e)
        {
            Propety.EnsureMultiListModel();
            int nextId = Propety.ColorLists.Count == 0 ? 1 : Propety.ColorLists.Max(x => x != null ? x.Id : 0) + 1;
            ColorAreaColorList list = new ColorAreaColorList();
            list.Id = nextId;
            list.Name = "Color List " + nextId;
            list.IsEnabled = true;
            list.TypeColor = Propety.TypeColor;
            list.Extraction = Propety.Extraction;
            list.PixelTemplate = Propety.PxTemp;
            Propety.ColorLists.Add(list);
            RefreshColorListDisplay();
            lstColorAreaLists.SelectedIndex = Propety.ColorLists.Count - 1;
        }

        private void ColorListDuplicateClicked(object sender, EventArgs e)
        {
            ColorAreaColorList source = GetSelectedColorList();
            if (source == null)
                return;

            int nextId = Propety.ColorLists.Max(x => x != null ? x.Id : 0) + 1;
            ColorAreaColorList copy = new ColorAreaColorList();
            copy.Id = nextId;
            copy.Name = source.Name + " Copy";
            copy.IsEnabled = source.IsEnabled;
            copy.TypeColor = source.TypeColor;
            copy.Extraction = source.Extraction;
            copy.PixelTemplate = source.PixelTemplate;
            copy.HSVs = CloneHsvList(source.HSVs);
            copy.RGBs = CloneRgbList(source.RGBs);
            copy.ExternHSVs = CloneHsvList(source.ExternHSVs);
            copy.ExternRGBs = CloneRgbList(source.ExternRGBs);
            Propety.ColorLists.Add(copy);
            RefreshColorListDisplay();
            lstColorAreaLists.SelectedIndex = Propety.ColorLists.Count - 1;
        }

        private void ColorListRemoveClicked(object sender, EventArgs e)
        {
            Propety.EnsureMultiListModel();
            int index = lstColorAreaLists.SelectedIndex;
            if (index < 0 || index >= Propety.ColorLists.Count)
                return;

            Propety.ColorLists.RemoveAt(index);
            if (Propety.ColorLists.Count == 0)
                Propety.ColorLists.Add(new ColorAreaColorList { Id = 1, Name = "Color List 1", TypeColor = Propety.TypeColor });

            RefreshColorListDisplay();
            lstColorAreaLists.SelectedIndex = Math.Min(index, Propety.ColorLists.Count - 1);
        }

        private void ColorListNameChanged(object sender, EventArgs e)
        {
            if (_isSyncingColorAreaMultiUi)
                return;
            ColorAreaColorList list = GetSelectedColorList();
            if (list == null)
                return;
            list.Name = txtColorListName.Text;
            RefreshColorListDisplay();
        }

        private void ColorListEnabledChanged(object sender, EventArgs e)
        {
            if (_isSyncingColorAreaMultiUi)
                return;
            ColorAreaColorList list = GetSelectedColorList();
            if (list == null)
                return;
            list.IsEnabled = chkColorListEnabled.Checked;
            RefreshColorListDisplay();
        }

        private void ColorListTypeChanged(object sender, EventArgs e)
        {
            if (_isSyncingColorAreaMultiUi)
                return;
         
        }

        private List<HSV> CloneHsvList(List<HSV> source)
        {
            List<HSV> clone = new List<HSV>();
            if (source == null)
                return clone;
            foreach (HSV hsv in source)
                clone.Add(new HSV(hsv.H, hsv.S, hsv.V));
            return clone;
        }

        private List<RGB> CloneRgbList(List<RGB> source)
        {
            List<RGB> clone = new List<RGB>();
            if (source == null)
                return clone;
            foreach (RGB rgb in source)
                clone.Add(new RGB(rgb.R, rgb.G, rgb.B));
            return clone;
        }

        private void ToolColorArea_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                Global.SetColorChange -= Global_SetColorChange;
                EditRectRot1.IsHide = true;
                EditRectRot1.RotateCurentChanged -= EditRectRot1_RotateCurentChanged;
            }
        }

        private void EditRectRot1_RotateCurentChanged(RectRotate obj)
        {
            switch (obj.TypeCrop)
            {
                case TypeCrop.Area:
                    Propety.rotArea = obj; break;
                case TypeCrop.Crop:
                    Propety.rotCrop = obj; break;
                case TypeCrop.Mask:
                    Propety.rotMask = obj; break;

            }
        }

        private void Global_SetColorChange(bool obj)
        {if (obj)
            {
                Propety.hSV = BeeCore.Common.HSVSample;
                Propety.rGB = BeeCore.Common.RGBSample;

                if (Propety.CheckMode == ColorAreaCheckMode.MultiList)
                {
                    AddColorToSelectedList();
                }
                else
                {
                    Propety.AddColor();
                    Propety.SetColor();
                }
                picColor.Invalidate();
            }
        }

        private void AddColorToSelectedList()
        {
            ColorAreaColorList list = GetSelectedColorList();
            if (list == null)
                return;

            switch (list.TypeColor)
            {
                case ColorGp.HSV:
                    if (list.HSVs == null)
                        list.HSVs = new List<HSV>();
                    if (Propety.hSV != null)
                        list.HSVs.Add(new HSV(Propety.hSV.H, Propety.hSV.S, Propety.hSV.V));
                    break;
                case ColorGp.RGB:
                    if (list.RGBs == null)
                        list.RGBs = new List<RGB>();
                    if (Propety.rGB != null)
                        list.RGBs.Add(new RGB(Propety.rGB.R, Propety.rGB.G, Propety.rGB.B));
                    break;
            }

            RefreshSelectedColorPalette();
        }

        private void RemoveLastColorFromSelectedList()
        {
            ColorAreaColorList list = GetSelectedColorList();
            if (list == null)
                return;

            if (list.TypeColor == ColorGp.HSV)
            {
                if (list.HSVs != null && list.HSVs.Count > 0)
                    list.HSVs.RemoveAt(list.HSVs.Count - 1);
            }
            else
            {
                if (list.RGBs != null && list.RGBs.Count > 0)
                    list.RGBs.RemoveAt(list.RGBs.Count - 1);
            }
        }

        private void ClearSelectedColorList()
        {
            ColorAreaColorList list = GetSelectedColorList();
            if (list == null)
                return;

            if (list.HSVs == null) list.HSVs = new List<HSV>();
            if (list.RGBs == null) list.RGBs = new List<RGB>();
            if (list.ExternHSVs == null) list.ExternHSVs = new List<HSV>();
            if (list.ExternRGBs == null) list.ExternRGBs = new List<RGB>();
            list.HSVs.Clear();
            list.RGBs.Clear();
            list.ExternHSVs.Clear();
            list.ExternRGBs.Clear();
        }

        private void SetSelectedListColorType(ColorGp typeColor)
        {
            ColorAreaColorList list = GetSelectedColorList();
            if (list == null)
                return;

            list.TypeColor = typeColor;
            Propety.TypeColor = typeColor;
            btnHSV.IsCLick = typeColor == ColorGp.HSV;
            btnRGB.IsCLick = typeColor == ColorGp.RGB;
            if (!_isSyncingColorAreaMultiUi)
            {btnHSV.IsCLick = typeColor == ColorGp.HSV ? true : false;
                btnRGB.IsCLick = typeColor == ColorGp.RGB ? true : false;
                //cboColorListType.SelectedIndex = typeColor == ColorGp.RGB ? 1 : 0;
            }

            RefreshSelectedColorPalette();
        }

      
   
        private void btnNone_Click(object sender, EventArgs e)
        {
            switch (Global.TypeCrop)
            {
                //case TypeCrop.Crop:
                //    Propety.rotCrop.Shape= btnElip.IsCLick==true ? ShapeType.Ellipse: ShapeType.Rectangle;
                //    break;
                //case TypeCrop.Area:
                //    Propety.rotArea.Shape= btnElip.IsCLick==true ? ShapeType.Ellipse: ShapeType.Rectangle;
                //    break;
                case TypeCrop.Mask:
                    Propety.rotMask = null;// = btnElip.IsCLick;
                    break;

            }
            //  G.EditTool.View.imgView.Invalidate();
        }

        private void SetShapeFor(TypeCrop which, ShapeType shape)
        {

            RectRotate rr = null;
            if (which == TypeCrop.Area) { if (Propety.rotArea == null) Propety.rotArea = new RectRotate(); rr = Propety.rotArea; }
            else if (which == TypeCrop.Mask) { if (Propety.rotMask == null) Propety.rotMask = new RectRotate(); rr = Propety.rotMask; }
            else { if (Propety.rotCrop == null) Propety.rotCrop = new RectRotate(); rr = Propety.rotCrop; }

            rr.Shape = shape;
            if (shape == ShapeType.Polygon)
            {
                if (rr.PolyLocalPoints == null || rr.PolyLocalPoints.Count() == 0)
                    NewShape(shape);
                else
                {
                    rr.UpdateFromPolygon(true);
                }
            }
            if (shape == ShapeType.Hexagon)
            {
                if (rr.HexVertexOffsets == null || rr.HexVertexOffsets.Count() == 0)
                    NewShape(shape);
            }


            Global.TypeCrop = which;
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;



        }
        private void NewShape(ShapeType newShape)
        {
            // 1) Chốt shape hiện tại
            var prop = Common.TryGetTool(Global.IndexToolSelected).Propety2;
            RectRotate rr = null;
            if (Global.TypeCrop == TypeCrop.Area) rr = prop?.rotArea;
            else if (Global.TypeCrop == TypeCrop.Mask) rr = prop?.rotMask;
            else rr = prop?.rotCrop;

            if (rr != null)
            {
                // Nếu đang drag: chấm dứt
                rr._dragAnchor = AnchorPoint.None;
                rr.ActiveVertexIndex = -1;

                // Nếu là polygon đang dựng dở
                if (rr.Shape == ShapeType.Polygon && rr.IsPolygonClosed == false)
                {
                    // CHỌN 1 TRONG 3 CHÍNH SÁCH:

                    // (A) Giữ tạm nguyên trạng (không chuẩn hoá, không xoá điểm)
                    // -> Không làm gì thêm

                    // (B) Tự đóng & chuẩn hoá (nếu muốn)
                    // nếu có >=3 điểm thì tự đóng:
                    // if (rr.PolyLocalPoints != null && rr.PolyLocalPoints.Count >= 3) {
                    //     var p0 = rr.PolyLocalPoints[0];
                    //     rr.PolyLocalPoints.Add(p0);
                    //     rr.IsPolygonClosed = true;
                    //     rr.UpdateFromPolygon(updateAngle: rr.AutoOrientPolygon);
                    // }

                    // (C) Huỷ polygon đang dựng
                    // rr.PolygonClear();
                }
            }



            // 3) Gán shape mới & chuẩn bị khung
            if (rr == null)
            {
                // tuỳ code lưu trữ của bạn mà tạo mới:
                rr = new RectRotate();
                if (Global.TypeCrop == TypeCrop.Area) prop.rotArea = rr;
                else if (Global.TypeCrop == TypeCrop.Mask) prop.rotMask = rr;
                else prop.rotCrop = rr;
            }

            rr.Shape = newShape;

            switch (newShape)
            {
                case ShapeType.Polygon:
                    // Local sạch, xoá điểm cũ: chờ click điểm đầu tiên
                    rr.ResetFrameForNewPolygonHard();
                    rr.AutoOrientPolygon = false; // thường tắt lúc dựng, bạn có thể để true nếu quen
                    break;

                case ShapeType.Rectangle:
                case ShapeType.Ellipse:
                case ShapeType.Hexagon:
                    // Không cần xoá toàn bộ; chỉ đảm bảo không kéo theo trạng thái cũ
                    rr._dragAnchor = AnchorPoint.None;
                    rr.ActiveVertexIndex = -1;

                    // Option: reset rotation cho phiên mới (tuỳ UX)
                    // rr._rectRotation = 0f;

                    // Để trống _rect: user kéo trái→phải để tạo mới theo logic MouseDown/Move của bạn
                    rr._rect = RectangleF.Empty;

                    // Hexagon: offsets về 0
                    if (newShape == ShapeType.Hexagon)
                    {
                        if (rr.HexVertexOffsets == null || rr.HexVertexOffsets.Length != 6)
                            rr.HexVertexOffsets = new PointF[6];
                        for (int i = 0; i < 6; i++) rr.HexVertexOffsets[i] = PointF.Empty;
                    }

                    break;
            }

            // Cập nhật về prop
            if (Global.TypeCrop == TypeCrop.Area) prop.rotArea = rr;
            else if (Global.TypeCrop == TypeCrop.Mask) prop.rotMask = rr;
            else prop.rotCrop = rr;


        }

        ShapeType ShapeType = ShapeType.Rectangle;
        private void btnHexagon_Click(object sender, EventArgs e)
        {
            ShapeType = ShapeType.Hexagon;
            SetShapeFor(Global.TypeCrop, ShapeType);
        }

        private void btnPolygon_Click(object sender, EventArgs e)
        {
            ShapeType = ShapeType.Polygon;

            SetShapeFor(Global.TypeCrop, ShapeType);
        }

        private void btnNewShape_Click(object sender, EventArgs e)
        {
            NewShape(ShapeType);
        }
        private void btnRect_Click(object sender, EventArgs e)
        {
            ShapeType = ShapeType.Rectangle;
            SetShapeFor(Global.TypeCrop, ShapeType);

        }

        private void btnElip_Click(object sender, EventArgs e)
        {
            ShapeType = ShapeType.Ellipse;
            SetShapeFor(Global.TypeCrop, ShapeType);

        }

    
    
        private void ToolColorArea_ScoreChanged(float obj)
        {
           trackScore.Value =(float)obj;
        }

        private void ToolColorArea_StatusToolChanged(PropetyTool tool, StatusTool obj)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => ToolColorArea_StatusToolChanged(tool, obj)));
                return;
            }

            if (Global.IsRun) return;
            if (obj == StatusTool.Done)
            {
                btnInspect.Enabled = true;

                if (Propety.IsCalib)
                {
                    btnCalib.IsCLick = false;
                    btnCalib.Enabled = true;
                    Propety.IsCalib = false;
                    if (Propety.CheckMode == ColorAreaCheckMode.MultiList)
                    {
                        RefreshColorListDisplay();
                        LoadSelectedColorListToUi();
                    }
                    else
                    {
                        Propety.PxTemp = Propety.pxRS;
                        AdjValueTemp.Value = Propety.PxTemp;
                    }
                }
            }


        }
        public Mat matCrop=new Mat();
        public void GetTemp()
        {
          
        
            
           // picColor.Invalidate();
            //Propety.SetColor();
           
        }
        

        private void rjButton3_Click(object sender, EventArgs e)
        {

          
          //  cv3.Pattern();
        }

        private void rjButton8_Click(object sender, EventArgs e)
        {
           
           
            btnGetColor.IsCLick = false;
            Propety.IsGetColor = btnGetColor.IsCLick;
            Global.StatusDraw = StatusDraw.Check;
        }
        
      
     
      
       
    
       

      

     

      
      
    
       
       

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnCropRect_Click_1(object sender, EventArgs e)
        {
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            IsFullSize = false;
            if (Propety.rotAreaTemp != null)
                Propety.rotArea = Propety.rotAreaTemp.Clone();
            Global.StatusDraw = StatusDraw.Edit;


        }

        private void rjButton1_Click(object sender, EventArgs e)
        {

        }

        bool IsFullSize;
        private void btnCropArea_Click_1(object sender, EventArgs e)
        {
            IsFullSize = true;
            Propety.rotAreaTemp = Propety.rotArea.Clone();
            Propety.rotArea = new RectRotate(new RectangleF(-BeeCore.Common.listCamera[Global. IndexProgChoose].matRaw.Width / 2, -BeeCore.Common.listCamera[Global. IndexProgChoose].matRaw.Height / 2, BeeCore.Common.listCamera[Global. IndexProgChoose].matRaw.Width, BeeCore.Common.listCamera[Global. IndexProgChoose].matRaw.Height), new PointF(BeeCore.Common.listCamera[Global. IndexProgChoose].matRaw.Width / 2, BeeCore.Common.listCamera[Global. IndexProgChoose].matRaw.Height / 2), 0, AnchorPoint.None);

           
           Global.TypeCrop= TypeCrop.Area;
            Propety.TypeCrop = Global.TypeCrop;
            Global.StatusDraw = StatusDraw.Edit;

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
          //  G.IsCancel = true;
           
          //  G.EditTool.RefreshGuiEdit(Step.Step3);
            btnGetColor.IsCLick = false;
            Propety.IsGetColor = btnGetColor.IsCLick;
        }

      
        private void btnClBlack_Click(object sender, EventArgs e)
        {
           
            btnDeleteAll.PerformClick();
        }

        private void trackScore_ValueChanged(float obj)
        {
            ColorAreaEngineRunner.ApplyScoreToOwner(OwnerTool, trackScore.Value);
            
          

        }
       
      

        private void picColor_Paint(object sender, PaintEventArgs e)
        {
            int x = 0;int h = picColor.Height;int w = h;
            foreach (Color cl in Propety.listCLShow)
            {

                e.Graphics.FillRectangle(new SolidBrush( cl), new RectangleF(x, 0, w, h));
                e.Graphics.DrawRectangle(new Pen(Color.Black,1), new Rectangle(x, 0, w, h));
                x += w ;
            }
        }

        private void trackPixel_Validating(object sender, CancelEventArgs e)
        {

        }

        private void trackPixel_ValueChanged(float obj)
        {
            if (_isSyncingColorAreaMultiUi)
                return;
            if (Propety.CheckMode == ColorAreaCheckMode.MultiList)
            {
                ColorAreaColorList list = GetSelectedColorList();
                if (list != null)
                {
                    list.Extraction = (int)trackPixel.Value;
                }
                return;
            }

            Propety.Extraction = (int)trackPixel.Value;

            if(!IsIni)
            {
                IsIni = true;
                return;
            }    
             Propety.SetColor();
           
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {if (Propety.listCLShow.Count == 0) return;
            if (Propety.CheckMode == ColorAreaCheckMode.MultiList)
            {
                RemoveLastColorFromSelectedList();
                RefreshSelectedColorPalette();
                return;
            }

            Propety.listCLShow.RemoveAt(Propety.listCLShow.Count - 1);
            Propety.Undo();
            picColor.Invalidate();
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {

            
           
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (Propety.CheckMode == ColorAreaCheckMode.MultiList)
            {
                ClearSelectedColorList();
                RefreshSelectedColorPalette();
                return;
            }

            Propety.listCLShow = new List<Color>();
            Propety.ClearTemp();
          //  G.EditTool.View.ClearTemp(Propety);
            picColor.Invalidate();
        }

        private void pMode_Paint(object sender, PaintEventArgs e)
        {

        }

       

      
  

       
        private void btnHSV_Click(object sender, EventArgs e)
        {
            if (Propety.CheckMode == ColorAreaCheckMode.MultiList)
            {
                SetSelectedListColorType(ColorGp.HSV);
                return;
            }

            Propety.TypeColor = ColorGp.HSV;
            btnHSV.IsCLick = true;
            btnRGB.IsCLick = false;
            RefreshSinglePalette();
        }

      

        private void btnInspect_Click(object sender, EventArgs e)
        {
            btnInspect.Enabled = false;
            if (!ColorAreaEngineRunner.TryRunSelectedTool())
            {
                btnInspect.Enabled = true;
                btnInspect.IsCLick = false;
            }
        }

        private void btnRGB_Click(object sender, EventArgs e)
        {
            if (Propety.CheckMode == ColorAreaCheckMode.MultiList)
            {
                SetSelectedListColorType(ColorGp.RGB);
                return;
            }

            Propety.TypeColor = ColorGp.RGB;
            btnRGB.IsCLick = true;
            btnHSV.IsCLick = false;
            RefreshSinglePalette();
        }
       

        private void btnGetColor_Click(object sender, EventArgs e)
        {
            
          
            Propety.IsGetColor = btnGetColor.IsCLick;
            Global.IsGetColor = btnGetColor.IsCLick;
            Global.ColorGp = Propety.TypeColor;
            if (Propety.IsGetColor)
            {
                
                
                Global.StatusDraw = StatusDraw.Color;
               
            }
            else
                Global.StatusDraw = StatusDraw.Edit;
        }

     

        private void AdjClearNoise_ValueChanged(float obj)
        {
            Propety.SizeClearsmall = (int)AdjClearNoise.Value;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Propety.IsClose = btnClose.IsCLick;
            AdjClose.Enabled = Propety.IsClose;
        }

        private void btnEnableNoise_Click(object sender, EventArgs e)
        {

            Propety.IsClearNoiseSmall = btnIsClearSmall.IsCLick;
            AdjClearNoise.Enabled = Propety.IsClearNoiseSmall;
        }

        private void AdjClose_ValueChanged(float obj)
        {

            Propety.SizeClose = (int)AdjClose.Value;
        }


        private void AdjOpen_ValueChanged(float obj)
        {
            Propety.SizeOpen = (int)AdjOpen.Value;
        }

        private void AdjClearBig_ValueChanged(float obj)
        {
            Propety.SizeClearBig = (int)AdjClearBig.Value;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Propety.IsOpen = btnOpen.IsCLick;
            AdjOpen.Enabled = Propety.IsOpen;
        }

        private void btnIsClearBig_Click(object sender, EventArgs e)
        {
            Propety.IsClearNoiseBig = btnIsClearBig.IsCLick;
            AdjClearBig.Enabled = Propety.IsClearNoiseBig;

        }

        private void btnCalib_Click(object sender, EventArgs e)
        {
            btnCalib.Enabled = false;
            ColorAreaEngineRunner.BeginCalibration(Propety);
         
            if (!ColorAreaEngineRunner.TryRunSelectedTool())
            {
                btnCalib.Enabled = true;
                btnCalib.IsCLick = false;
            }
        }

        private void AdjValueTemp_ValueChanged(float obj)
        {
            if (_isSyncingColorAreaMultiUi)
                return;
            if (Propety.CheckMode == ColorAreaCheckMode.MultiList)
            {
                ColorAreaColorList list = GetSelectedColorList();
                if (list != null)
                {
                    list.PixelTemplate = (int)AdjValueTemp.Value;
                }
                return;
            }

            Propety.PxTemp = (int)AdjValueTemp.Value;
        }

        private void label9_Click(object sender, EventArgs e)
        {
                    }

        private void rjButton2_Click(object sender, EventArgs e)
        {

        }

        private void btnNGLess_Click(object sender, EventArgs e)
        {
            Propety.IsNGLess=btnNGLess.IsCLick;
        }

        private void btnNGMore_Click(object sender, EventArgs e)
        {
            Propety.IsNGMore=btnNGMore.IsCLick;
        }

       

        private void btn2_Click(object sender, EventArgs e)
        {
            EditRectRot1.Visible = !btn2.IsCLick;
        }

        private void btnScanSingle_Click(object sender, EventArgs e)
        {
            Propety.CheckMode = ColorAreaCheckMode.Single;
            layMultiColor.Visible = false;
            Propety.ColorAreaPP = new BeeCpp.ColorArea();
            UpdateColorAreaMultiUi();
            RefreshParamsForCurrentMode();
            Propety.PrimeNativeColors();
        }

        private void btnScanMulti_Click(object sender, EventArgs e)
        {
            Propety.CheckMode = ColorAreaCheckMode.MultiList;
            layMultiColor.Visible = true;
            Propety.ColorAreaPP = new BeeCpp.ColorArea();
            UpdateColorAreaMultiUi();
            RefreshParamsForCurrentMode();
            Propety.PrimeNativeColors();
        }

        private void rjButton3_Click_1(object sender, EventArgs e)
        {
            
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            layLimitOKNG.Visible = !btn4.IsCLick;
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            trackScore.Visible = !btn5.IsCLick;
        }
    }
}
