using BeeCore;
using BeeGlobal;
using BeeInterface.PLC;
using BeeInterface.Properties;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface.Group
{
    public partial class EditRectRot : UserControl
    {
       public  List<RectRotate> Rot = new List<RectRotate>();
      
        public EditRectRot( )
        {
            InitializeComponent();
          
        }
        public bool _IsHide = false;
       
        public bool IsHide
        {
            get => _IsHide;
            set
            {
                if (_IsHide != value)
                {
                    _IsHide = value;
                  if(_IsHide)
                    {
                        Global.RotateCurentChanged -= Global_RotateCurentChanged;
                    }    
                }
            }
        }
        int index = 0;
        public  RectRotate _rotCurrent { get; set; }
      
        public  event Action<RectRotate> RotateCurentChanged;
        public  RectRotate rotCurrent
        {
            get => _rotCurrent;
            set
            {
                if (_rotCurrent != value)
                {
                    _rotCurrent = value;
                    RotateCurentChanged?.Invoke(_rotCurrent); // Gọi event
                }
            }
        }
        private void Btn_Click(object sender, EventArgs e)
        {
            RJButton btn = sender as RJButton;
            index = Convert.ToInt32( btn.Name);
            rotCurrent = Rot[index];
            if (rotCurrent == null)
                rotCurrent = new RectRotate();
            lay2Mask.Enabled = _rotCurrent.TypeCrop == TypeCrop.Mask ? true : false;
            layRange.Enabled = _rotCurrent.TypeCrop == TypeCrop.Area ? true : false;
            layLimit.Enabled = (_rotCurrent.TypeCrop == TypeCrop.Limit || _rotCurrent.TypeCrop == TypeCrop.Mask);
            btnElip.IsCLick = _rotCurrent.Shape == ShapeType.Ellipse ? true : false;
            btnRect.IsCLick = _rotCurrent.Shape == ShapeType.Rectangle ? true : false;
            btnHexagon.IsCLick = _rotCurrent.Shape == ShapeType.Hexagon ? true : false;
            btnPolygon.IsCLick = _rotCurrent.Shape == ShapeType.Polygon ? true : false;
            btnWhite.IsCLick = _rotCurrent.IsWhite;
            btnBlack.IsCLick = !_rotCurrent.IsWhite;

            
            if (btn.IsCLick)
            {
               
                Global.rotCurrent = rotCurrent.Clone();
               
                Global.StatusDraw = StatusDraw.None;
                Global.StatusDraw = StatusDraw.Edit;
                
            }
            else
            {
                
                
            }
        }

        private void Global_RotateCurentChanged(RectRotate obj)
        {
          
            if(obj._rect.Width!=0)
            {
                rotCurrent = obj.Clone();
                Rot[index] = rotCurrent;
            }
            else
            {
                Console.WriteLine("Err");
            }    
          
        }

        ShapeType ShapeType = ShapeType.Rectangle;
        private void SetShapeFor( ShapeType shape)
        {

            RectRotate rr = Rot[index];

            rr.Shape = shape;
            if (shape == ShapeType.Polygon)
            {
                if (rr.PolyLocalPoints == null || rr.PolyLocalPoints.Count() == 0)
                    NewShape(shape);
                else
                {
                    rr._rectRotation = 0;
                    rr.UpdateFromPolygon(false);

                }
            }
            if (shape == ShapeType.Hexagon)
            {
                if (rr.HexVertexOffsets == null || rr.HexVertexOffsets.Count() == 0)
                    NewShape(shape);
            }

            _rotCurrent = rr;
            Rot[index] = rr;
            Global._rotCurrent = rr.Clone();
            // Global.TypeCrop = which;
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;



        }
        private void NewShape(ShapeType newShape, int W = 0, int H = 0)
        {
            // 1) Chốt shape hiện tại
         
          
            //if (Global.TypeCrop == TypeCrop.Area) rr = prop?.rotArea;
            //else if (Global.TypeCrop == TypeCrop.Mask) rr = prop?.rotMask;
            //else rr = prop?.rotCrop;

            if (rotCurrent != null)
            {
                // Nếu đang drag: chấm dứt
                rotCurrent._dragAnchor = AnchorPoint.None;
                rotCurrent.ActiveVertexIndex = -1;

                // Nếu là polygon đang dựng dở
                if (rotCurrent.Shape == ShapeType.Polygon && rotCurrent.IsPolygonClosed == false)
                {
                    // CHỌN 1 TRONG 3 CHÍNH SÁCH:

                    // (A) Giữ tạm nguyên trạng (không chuẩn hoá, không xoá điểm)
                    // -> Không làm gì thêm

                    // (B) Tự đóng & chuẩn hoá (nếu muốn)
                    // nếu có >=3 điểm thì tự đóng:
                    // if (rotCurrent.PolyLocalPoints != null && rotCurrent.PolyLocalPoints.Count >= 3) {
                    //     var p0 = rotCurrent.PolyLocalPoints[0];
                    //     rotCurrent.PolyLocalPoints.Add(p0);
                    //     rotCurrent.IsPolygonClosed = true;
                    //     rotCurrent.UpdateFromPolygon(updateAngle: rotCurrent.AutoOrientPolygon);
                    // }

                    // (C) Huỷ polygon đang dựng
                    // rotCurrent.PolygonClear();
                }
            }
            else
            {
              rotCurrent = new RectRotate();
            }



            rotCurrent.Shape = newShape;

            switch (newShape)
            {
                case ShapeType.Polygon:
                    // Local sạch, xoá điểm cũ: chờ click điểm đầu tiên
                    rotCurrent.ResetFrameForNewPolygonHard();
                    rotCurrent.AutoOrientPolygon = false; // thường tắt lúc dựng, bạn có thể để true nếu quen
                    break;

                case ShapeType.Rectangle:
                case ShapeType.Ellipse:
                case ShapeType.Hexagon:
                    // Không cần xoá toàn bộ; chỉ đảm bảo không kéo theo trạng thái cũ
                    rotCurrent._dragAnchor = AnchorPoint.None;
                    rotCurrent.ActiveVertexIndex = -1;

                    // Option: reset rotation cho phiên mới (tuỳ UX)
                    // rotCurrent._rectRotation = 0f;

                    // Để trống _rect: user kéo trái→phải để tạo mới theo logic MouseDown/Move của bạn
                    rotCurrent._rect = RectangleF.Empty;

                    // Hexagon: offsets về 0
                    if (newShape == ShapeType.Hexagon)
                    {
                        if (rotCurrent.HexVertexOffsets == null || rotCurrent.HexVertexOffsets.Length != 6)
                            rotCurrent.HexVertexOffsets = new PointF[6];
                        for (int i = 0; i < 6; i++) rotCurrent.HexVertexOffsets[i] = PointF.Empty;
                    }

                    break;
            }
            if (W != 0 && H != 0)
            {
                rotCurrent._PosCenter = new PointF(Global.Config.SizeCCD.Width / 2, Global.Config.SizeCCD.Height / 2);
                rotCurrent._rect = new RectangleF(-W / 2, -H / 2, W, H);
            }

            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;

        }

        private void btnNewShape_Click(object sender, EventArgs e)
        {
            TypeCrop typeCrop = rotCurrent.TypeCrop;
            String OldName = rotCurrent.Name;
            NewShape(ShapeType, (int)numW.Value, (int)numH.Value);
            rotCurrent.TypeCrop = typeCrop;
            rotCurrent.Name = OldName;
            Rot[index] = rotCurrent;
            Global.rotCurrent = rotCurrent.Clone();
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;
        }
        private void btnRect_Click(object sender, EventArgs e)
        {
            ShapeType = ShapeType.Rectangle;
            SetShapeFor( ShapeType);
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;
        }

        private void btnElip_Click(object sender, EventArgs e)
        {
            ShapeType = ShapeType.Ellipse;
            SetShapeFor(ShapeType);
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;
        }

        private void btnHexagon_Click(object sender, EventArgs e)
        {
            ShapeType = ShapeType.Hexagon;
            SetShapeFor( ShapeType);
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;
        }

        private void btnPolygon_Click(object sender, EventArgs e)
        {
            ShapeType = ShapeType.Polygon;

            SetShapeFor( ShapeType);
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;
        }
        private void btnNone_Click(object sender, EventArgs e)
        {TypeCrop typeCrop=rotCurrent.TypeCrop;
            String Name = rotCurrent.Name;
            rotCurrent = new RectRotate();
            rotCurrent.TypeCrop = typeCrop;
            rotCurrent.Name = Name;
            Rot[index] = rotCurrent;
            Global.rotCurrent = rotCurrent;
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;
            
        }
        private void btnWhite_Click(object sender, EventArgs e)
        {
            rotCurrent.IsWhite = btnWhite.IsCLick;
          
        }

        private void btnBlack_Click(object sender, EventArgs e)
        {
            rotCurrent.IsWhite = !btnBlack.IsCLick;
          
        }
        public void Refresh(bool IsIni=false)
        {
            if (Rot == null)
                return;
            int CountType = Rot.Count;
            if(Rot[index] == null)
                Rot[index]=new RectRotate();
            this.rotCurrent = Rot[index];
            Global.rotCurrent = this.rotCurrent.Clone();
            Global.RotateCurentChanged -= Global_RotateCurentChanged;
            Global.RotateCurentChanged += Global_RotateCurentChanged;
         
            lay2Mask.Enabled = _rotCurrent.TypeCrop == TypeCrop.Mask ? true : false;
            layRange.Enabled = _rotCurrent.TypeCrop == TypeCrop.Area ? true : false;
            layLimit.Enabled = _rotCurrent.TypeCrop == TypeCrop.Limit ? true : false;
            btnElip.IsCLick = _rotCurrent.Shape == ShapeType.Ellipse ? true : false;
            btnRect.IsCLick = _rotCurrent.Shape == ShapeType.Rectangle ? true : false;
            btnHexagon.IsCLick = _rotCurrent.Shape == ShapeType.Hexagon ? true : false;
            btnPolygon.IsCLick = _rotCurrent.Shape == ShapeType.Polygon ? true : false;
            if (this.layType.Controls.Count < CountType|| IsIni)
            {
            


             
               
                this.layType.SuspendLayout();
                this.layType.ColumnStyles.Clear();
              
              

                for (int i = this.layType.ColumnStyles.Count - 1; i >= 0; i--)
                {
                    this.layType.ColumnStyles.RemoveAt(i);
                }
                if(this.Controls.Count > 0) 
                this.layType.Controls.Clear();
               
                this.layType.ColumnCount = 0;

                this.layType.ResumeLayout();
                for (int i = 0; i < CountType; i++)
                {
                    this.layType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100 / CountType));
                }
             
                for (int i = 0; i < CountType; i++)
                {
                   
                    RJButton btn = new RJButton();
                    btn.AutoFont = true;
                    btn.AutoFontHeightRatio = 0.75F;
                    btn.AutoFontMax = 100F;
                    btn.AutoFontMin = 6F;
                    btn.AutoFontWidthRatio = 0.92F;
                    btn.AutoImage = true;
                    btn.AutoImageMaxRatio = 0.75F;
                    btn.AutoImageMode = BeeInterface.RJButton.ImageFitMode.Contain;
                    btn.AutoImageTint = true;
                    btn.BackColor = System.Drawing.Color.White;
                    btn.BackgroundColor = System.Drawing.Color.White;
                    btn.BorderColor = System.Drawing.Color.White;
                    btn.BorderRadius = 5;
                    btn.BorderSize = 1;
                    btn.ClickBotColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(211)))), ((int)(((byte)(139)))));
                    btn.ClickMidColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(204)))), ((int)(((byte)(120)))));
                    btn.ClickTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(192)))), ((int)(((byte)(89)))));
                    btn.ContentPadding = new System.Windows.Forms.Padding(8, 6, 8, 6);
                    btn.DebounceResizeMs = 6;
                    btn.Dock = System.Windows.Forms.DockStyle.Fill;
                    btn.FlatAppearance.BorderSize = 0;
                    // btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.60938F);
                    btn.ForeColor = System.Drawing.Color.Black;
                    btn.Enabled =! Rot[i].IsVisible;
                    btn.ImageTintDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
                    btn.ImageTintHover = System.Drawing.Color.Empty;
                    btn.ImageTintNormal = System.Drawing.Color.Empty;
                    btn.ImageTintOpacity = 0.5F;
                    btn.ImageTintPressed = System.Drawing.Color.Empty;

                    btn.IsCLick = false;
                    btn.IsNotChange = false;
                    btn.IsRect = false;
                    btn.IsTouch = false;
                    btn.IsUnGroup = false;

                    btn.Margin = new System.Windows.Forms.Padding(0);
                    btn.Multiline = false;
                    btn.Dock = DockStyle.Fill;
                    btn.Size = new System.Drawing.Size(110, 45);
                    btn.TabIndex = 5;
                 
                    btn.Text = Rot[i].Name;
                    btn.TextColor = System.Drawing.Color.Black;
                    btn.Name = i.ToString();
                    if (i == 0)
                    {
                        btn.IsCLick = true;
                        btn.Corner = BeeGlobal.Corner.Left;
                    }
                    else if (i == CountType - 1)
                        btn.Corner = BeeGlobal.Corner.Right;
                    else
                        btn.Corner = BeeGlobal.Corner.None;
                    btn.Click += Btn_Click;


                    this.layType.Controls.Add(btn, i, 0);

                }
            } 
        }
        public event Action<int> ChooseScanChange;
  

        private void EditRectRot_Load(object sender, EventArgs e)
        {
          
        }

        private void EditRectRot_VisibleChanged(object sender, EventArgs e)
        {
        
        
           
           
             
        }
        public event Action<bool> ChooseEditBegin;
        public event Action<int> ChooseEditEnd;
        private void btnEdit_Click(object sender, EventArgs e)
        {
           
           // Propety.ModeCheck = ModeCheck.Single;
           
            //Propety.IsScan = btnEdit.IsCLick; ;

            if (btnEdit.IsCLick)
            {
                Global.IndexRotChoose = -1;
                btnEdit.Text = "OK"; Global.StatusDraw = StatusDraw.Scan;
                ChooseEditBegin?.Invoke(true); // Gọi event
                //RectRotate rot = Propety.ListRotMask[(int)numIndexArea.Value];
                //PointF pCenter = Propety.rotArea.LocalToWorld(rot._PosCenter);

                //rot._PosCenter = pCenter;
                //Propety.rotMask = rot;
            }
            else
            {
              
                btnEdit.Text = "Edit"; Global.StatusDraw = StatusDraw.Edit;
                ChooseEditEnd?.Invoke(Global.IndexRotChoose); // Gọi event
               
            }

        }
        public  event Action<RectRotate> AddRotEvent;
        public  event Action<bool> UnoRotEvent;
        public  event Action<bool> DeleteEvent;
        public  event Action<bool> ClearAllEvent;
        private void btnAddToList_Click(object sender, EventArgs e)
        {
            RectRotate rot = rotCurrent.Clone();
            RectRotate rotArea = null;

            foreach (RectRotate rot1 in Rot)
            {
                if (rot1.TypeCrop == TypeCrop.Area)
                {
                    rotArea = rot1.Clone();
                    break;
                }
            }

            if (rotArea != null)
            {
                // 1) đổi tâm từ world -> local của rotArea
                rot._PosCenter = rotArea.WorldToLocal(rot._PosCenter);

                // 2) đổi góc từ world -> local của rotArea
                rot._rectRotation = rot._rectRotation - rotArea._rectRotation;

                // 3) chuẩn hóa góc về [-180, 180] nếu muốn
                //while (rot._rectRotation > 180f) rot._rectRotation -= 360f;
                //while (rot._rectRotation < -180f) rot._rectRotation += 360f;
                AddRotEvent?.Invoke(rot.Clone()); // Gọi event
         

             
            }

        }

        private void btnUnoMask_Click(object sender, EventArgs e)
        {
            UnoRotEvent?.Invoke(true); // Gọi event
              Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;
        }

        bool IsFullSize= true;
        RectRotate rotTemp=new RectRotate();
        private void btnCropHalt_Click(object sender, EventArgs e)
        {
         
            rotCurrent = rotTemp.Clone();
            rotCurrent.TypeCrop = rotTemp.TypeCrop;
            rotCurrent.Name = rotTemp.Name;
            Rot[index] = rotCurrent;
            Global.rotCurrent = rotCurrent.Clone();
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;

        }

        private void btnCropFull_Click(object sender, EventArgs e)
        {
            IsFullSize = true;

            rotTemp = rotCurrent.Clone();
            rotCurrent = new RectRotate(new RectangleF(-Global.Config.SizeCCD.Width / 2, -Global.Config.SizeCCD.Height / 2, Global.Config.SizeCCD.Width, Global.Config.SizeCCD.Height), new PointF(Global.Config.SizeCCD.Width / 2, Global.Config.SizeCCD.Height / 2), 0, AnchorPoint.None);
            rotCurrent.TypeCrop = rotTemp.TypeCrop;
            rotCurrent.Name = rotTemp.Name;
            Rot[index] = rotCurrent;
            Global.rotCurrent = rotCurrent.Clone();
            Global.StatusDraw = StatusDraw.None;
            Global.StatusDraw = StatusDraw.Edit;
        }

        private void btnClearAllMask_Click(object sender, EventArgs e)
        {
            ClearAllEvent?.Invoke(true); // Gọi event
          
          
        }
    }
}
