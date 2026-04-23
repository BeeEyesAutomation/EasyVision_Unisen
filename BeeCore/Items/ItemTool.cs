using BeeCore;
using BeeCore.Func;
using BeeGlobal;
using CvPlus;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using Image = System.Drawing.Image;
using TextBox = System.Windows.Forms.TextBox;

namespace BeeCore
{ // ADD: n?u mu?n dùng chu?n .NET
    public interface IDeepCloneable<T>
    {
        T Clone(bool copyRuntime = true);
    }
    public partial class ItemTool : UserControl, IDeepCloneable<ItemTool>, ICloneable
    {  // ADD: Clone công khai
        /// <summary>
        /// Clone ItemTool b?ng cách t?o instance m?i và sao chép thu?c tính.
        /// copyRuntime=false: ch? copy c?u hình; true: copy c? tr?ng thái runtime (Score/Status/CT...).
        /// </summary>
        public ItemTool Clone(bool copyRuntime = true)
        {
            var clone = new ItemTool(this.TypeTool, this.Name,this.TriggerNum);

            // --- C?u hình/thi?t k? co b?n ---
            clone.Size = this.Size;
            clone.MinimumSize = this.MinimumSize;
            clone.MaximumSize = this.MaximumSize;
            clone.Margin = this.Margin;
            clone.Padding = this.Padding;
            clone.Enabled = this.Enabled;
            clone.Visible = this.Visible;
            clone.Font = (Font)this.Font?.Clone();
            clone.ForeColor = this.ForeColor;
            clone.BackColor = this.BackColor;
            clone.TriggerNum = this.TriggerNum;
            clone.IconTool = SafeCloneImage(this.IconTool); // tránh share cùng Bitmap
            clone.ColorTrack = this.ColorTrack;
            clone.Score=this.Score;
            clone.Step = this.Step;
            clone.Min = this.Min;     // dùng field/backing d? không b?n Invalidate quá s?m
            clone.Max = this.Max;
            clone.NotChange = this.NotChange;
            clone.IsEdit = this.IsEdit;

            clone.IndexThread = this.IndexThread;
            clone.IndexTool = this.IndexTool;
            clone.TriggerNum = this.TriggerNum;

            // --- Thu?c tính hi?n th?/tr?ng thái ---
            if (copyRuntime)
            {
                clone.ClStatus = this.ClStatus;
                clone.ClScore = this.ClScore;
                clone.Status = this.Status;
                clone.Score = this.Score;
                clone.CT = this.CT;

                // Set Value/ValueScore cu?i cùng d? c?p nh?t pTick + layout
                clone.ValueScore = this.ValueScore;
            }
            else
            {
                // Reset runtime: nhu lúc chua ch?y
                clone.ClStatus = Global.ColorNone;
                clone.ClScore = Global.ColorNone;
                clone.Status = "---";
                clone.Score = "---";
                clone.CT = 0;
                clone.ValueScore = 0;
                // N?u mu?n gi? Score c?u hình thì l?y t? Common.PropetyTools (n?u có)
                //try
                //{
                //    clone.Step = Common.TryGetTool(clone.IndexThread, clone.IndexTool).StepValue;
                //    clone.Min = Common.TryGetTool(clone.IndexThread, clone.IndexTool).MinValue;
                //    clone.Max = Common.TryGetTool(clone.IndexThread, clone.IndexTool).MaxValue;
                //    clone.Value = BeeCore.Common.TryGetTool(clone.IndexThread, clone.IndexTool).Score;
                //}
                //catch { /* an toàn n?u chua có Common.PropetyTools */ }
            }

            // Ð?ng b? n?i b? hình h?c
            clone.UpdateLayout();

            // LUU Ý: KHÔNG sao chép event subscriber bên ngoài (ValueChanged, ...).
            // N?u c?n, ? noi s? d?ng hãy dang ký l?i:
            // clone.ValueChanged += ...;

            return clone;
        }

        // ADD: h? tr? ICloneable (m?c d?nh copyRuntime=true)
        object ICloneable.Clone() => this.Clone(true);

        // ADD: ti?n ích clone Image an toàn
        private static Image SafeCloneImage(Image src)
        {
            if (src == null) return null;
            try
            {
                // N?u là Bitmap, clone pixel d? không share handle/stream
                if (src is Bitmap bmp)
                {
                    // Clone theo toàn b? rect và format
                    return bmp.Clone(new Rectangle(0, 0, bmp.Width, bmp.Height), bmp.PixelFormat);
                }
                // Fallback: dùng MemoryStream
                using (var ms = new System.IO.MemoryStream())
                {
                    src.Save(ms, src.RawFormat);
                    ms.Position = 0;
                    return Image.FromStream(ms);
                }
            }
            catch
            {
                // N?u không clone du?c, ch?p nh?n tr? v? ref (ít g?p)
                return src;
            }
        }
        public PointF pTick;
        public Color colorTrack = Color.Gray;
        private Image imgTick = Properties.Resources.Enable;
        public TypeTool TypeTool;
        public Image IconTool;
        public float valueScore;
        public bool NotChange = false;
        [Category("Min")]
        private float min;
        private float max = 100;
        private float value;
        private float step = 1;
        bool IsDown;
        
       // Bitmap bmpBase;
      //  Graphics gcBase;
        Bitmap bmBase;
        bool IsLoadBase = false;
       

       

      
        public float Min { get => min; set { min = value;
                if (TriggerNum != TriggerNum.Trigger1)
                    return;
                if (Value < Min) Value = Min;
                this.Refresh();
            } }
        public float Max { get => max; set
            {
                max = value;
                if (TriggerNum != TriggerNum.Trigger1)
                    return;
                if (Value >Max) Value = Max;
                this.Refresh();
            }
        }
        public float Value
        {
            get => value;
            set
            {
                if (Max == Min) Max++;
                if (value > Max) value = Max;
                if (value < Min) value = Min;
                if (!float.IsNaN(value))
                {if (NotChange) return;
                    if (TriggerNum != TriggerNum.Trigger1) 
                        return;
                    this.value = (float)Math.Round(value, 1);
                    pTick = new Point(pTrack.X + (int)((value * 1.0 / (Max - Min)) * (this.szTrack.Width - imgTick.Width)), pTrack.Y);
                    BeeCore.Common.TryGetTool(IndexThread, IndexTool).Score = Value;
                    this.Invalidate();
                }    
              

            }
        }
        public event Action<float> ValueChanged;

        public float Step { get => step; set => step = value; }
        public float ValueScore
        {
            get => valueScore; set
            {

                if (value != valueScore)
                {

                    valueScore = value - value % step;
                    //if (valueScore > Max)
                    //    ValueScore = Max;
                    //if (valueScore <Min) 
                    //    ValueScore = Min;
                    this.Invalidate();
                }
            }

        }

        public Color ColorTrack
        {
            get => colorTrack; set
            {
                if (value != colorTrack)
                {
                    // bmpBase = new Bitmap(pT.Width, pT.Height);
                    colorTrack = value;
                    this.Invalidate();
                    //gcBase = Graphics.FromImage(bmpBase);
                    //Rectangle rect = new Rectangle(0, 0, pT.Width - 2, (int)(pT.Height / 3.5));
                    //gcBase.FillRectangle(new SolidBrrush(colorTrack), rect);
                    //gcBase.DrawImage(Properties.Resources.BID_SLIDER_SCALE_8PIX_W303, new Rectangle(0, (int)(pT.Height / 3.5), pT.Width - 2, pT.Height / 3));


                    //pT.Image = bmpBase;
                    //if(bmBase!=null)
                    //bmBase.Dispose();
                }
            }
        }

       

        private void imgTrack_MouseClick(object sender, MouseEventArgs e)
        {

            if ( e.Location.X - pTick.X>5&&e.Location.Y>pTrack.Y&& e.Location.Y < pTrack.Y+szTrack.Height)
                Value += Step;
            else if ( pTick.X- e.Location.X > 5 && e.Location.Y > pTrack.Y && e.Location.Y < pTrack.Y + szTrack.Height)
                Value -= Step;
        }

        private void pTrack_MouseClick(object sender, MouseEventArgs e)
        {
            if (NotChange) return;
            if (e.Location.X -pTick.X >5)
                Value += Step;
            else if(pTick.X - e.Location.X > 5)
                Value -= Step;
        }

        public ItemTool(TypeTool typeTool,string names,TriggerNum triggerNum)
        {
          
            InitializeComponent();
            this.Name = names;
            this.TypeTool = typeTool;
            this.MouseMove += ItemTool_MouseMove;
            this.MouseLeave += ItemTool_MouseLeave;
            this.MouseUp += ItemTool_MouseUp;
            this.MouseDown += ItemTool_MouseDown;
            this.Click += ItemTool_Click;
            this.VisibleChanged += ItemTool_VisibleChanged;
            this.EnabledChanged += ItemTool_EnabledChanged;
          TriggerNum = triggerNum;
            if (TriggerNum != TriggerNum.Trigger1)
                NotChange = true;
            //if(TypeTool==TypeTool.Measure)
            //{
            //    Score.Max = 10;
            //}




        }

        private void ItemTool_EnabledChanged(object sender, EventArgs e)
        {
            isHovered = !this.Enabled;
            this.Invalidate();
        }

        private void ItemTool_VisibleChanged(object sender, EventArgs e)
        {if (IndexTool >= Common.EnsureToolList(IndexThread).Count)
                return;
            Common.TryGetTool(IndexThread, IndexTool).StatusToolChanged -= ItemTool_StatusToolChanged;
            Common.TryGetTool(IndexThread, IndexTool).StatusToolChanged += ItemTool_StatusToolChanged;
            //  Value = BeeCore.Common.TryGetTool(IndexThread, IndexTool).Score;
        }

        private void ItemTool_MouseUp(object sender, MouseEventArgs e)
        {
          
            if (IsDown)
            {
                if (ValueChanged != null) ValueChanged(Value);
            }

            IsDown = false;
        }

        private void ItemTool_MouseDown(object sender, MouseEventArgs e)
        {
            IsDown = true;
        }

   

       
        private void Parent_VisibleChanged1(object sender, EventArgs e)
        {
            //if (Parent != null && Global.indexToolSelected != -1 && Global.indexToolSelected < G.listAlltool[IndexThread].Count)
            //{
            //    if (this.Parent.Visible)
            //    {
            //        G.IsEdit = false;
            //        if (G.listAlltool[IndexThread].FindIndex(a => a.ItemTool == this) != G.indexToolSelected) return;
            //        if (G.PropetyOld != null && G.IsCancel)
            //        {
            //            G.IsCancel = false;
            //            G.listAlltool[IndexThread][G.indexToolSelected].tool.Propety = G.PropetyOld.Clone();
            //            BeeCore.Common.TryGetTool(IndexThread, G.indexToolSelected).Propety = G.listAlltool[IndexThread][G.indexToolSelected].tool.Propety;

            //            G.EditTool.View.imgView.Invalidate();
            //        }
            //        Score.Value = BeeCore.Common.TryGetTool(IndexThread, G.indexToolSelected).Propety.Score;
            //    }
            //}
            
        }

        private void Parent_VisibleChanged(object sender, EventArgs e)
        {
            
        }

        private void ItemTool_Click(object sender, EventArgs e)
        {
            if (Global.IsRun) return;
           // this.BackgroundImage = imgChoose;
            this.IsCLick = true;
            Global.StatusDraw = StatusDraw.None;
            Global.IndexToolSelected = IndexTool;
            foreach (Control c in this.Parent.Controls)
            {

                if (c is ItemTool && c != this)
                {
                    ItemTool btn = c as ItemTool;
                    btn.IsCLick = false;
                   
                   // c.BackgroundImage = imgUnChoose;
                }
            }
        }
        bool isCLick;
        bool isHovered = false;
        public String Name = "";
        public Font Font = new Font("Arial", 14);
        public Font FontStaus = new Font("Arial", 22, FontStyle.Bold);
        public TriggerNum TriggerNum = TriggerNum.Trigger1;
        public bool IsCLick
        {
            get { return this.isCLick; }
            set
            {
                this.isCLick = value;
                //if (value)
                //{
                //    this.BackgroundImage = imgChoose;
                //}
                //else
                //    this.BackgroundImage = imgUnChoose;
                this.Invalidate();

            }
        }
            private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }
        private Size szStatus = new Size(80, 42);
        private Size szCT = new Size(60, 24);
        private Size szNameTool= new Size(60, 24);
        private Size szTrack = new Size(60, 15);
        private Point pTrack = new Point(10, 10);
        protected override void OnPaint(PaintEventArgs pevent)
        {
            try
            {
                base.OnPaint(pevent);
                if (this.ClientRectangle == null) return;
                if(this.Parent == null) return; 
                pEnd = new PointF(this.Width - 5, 5);
                Rectangle rectSurface = this.ClientRectangle;
                Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
                // Xác d?nh màu n?n d?a trên tr?ng thái
                Color topColor, middleColor, bottomColor;

                if (isCLick)
                {
                    // Màu khi b?m xu?ng
                    topColor = Color.FromArgb(244, 192, 89);
                    middleColor = Color.FromArgb(246, 204, 120);
                    bottomColor = Color.FromArgb(247, 211, 139);//247, 211, 139
                }
                else if (isHovered)
                {
                    // Màu khi hover
                    topColor = Color.FromArgb(208, 211, 213);
                    middleColor = Color.FromArgb(193, 197, 199);
                    bottomColor = Color.FromArgb(179, 182, 185);
                }
                else
                {
                    // Màu m?c d?nh
                    topColor = Color.FromArgb(243, 247, 250);
                    middleColor = Color.FromArgb(218, 221, 224);
                    bottomColor = Color.FromArgb(199, 203, 206);
                }

                // Gradient 3 màu
                using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.White, Color.Gray, LinearGradientMode.Vertical))
                {
                    ColorBlend colorBlend = new ColorBlend();
                    colorBlend.Colors = new Color[] { topColor, middleColor, bottomColor };
                    colorBlend.Positions = new float[] { 0.0f, 0.5f, 1.0f }; // 3 di?m màu
                    brush.InterpolationColors = colorBlend;

                    pevent.Graphics.FillRectangle(brush, rect);
                }
                // V? hình ?nh n?u có

                // V? hình ?nh n?u có
                int imgSize = Math.Min(this.Height - 10, 24); // Gi?i h?n kích thu?c ?nh
                Rectangle imgRect = Rectangle.Empty;
                Rectangle textRect = rect;
                int spacing = 5; // Kho?ng cách gi?a ?nh và ch?


                // V? ch? trên button
                textRect = new Rectangle(0, 0, this.Width, this.Height);
                TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

                TextRenderer.DrawText(pevent.Graphics, this.Text, this.Font, textRect, this.ForeColor, flags);
                int smoothSize = 2;

                Rectangle rectBorder = Rectangle.Inflate(rectSurface, -1, -1);

                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, 10))

                using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(Color.Transparent, 1))
                {
                    pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    //Button surface
                    this.Region = new Region(pathSurface);
                    //Draw surface border for HD result
                    pevent.Graphics.DrawPath(penSurface, pathSurface);

                    ////Button border                    
                    //if (borderSize >= 1)
                    //    //Draw control border
                    //    pevent.Graphics.DrawPath(penBorder, pathBorder);
                    //else
                    //    pevent.Graphics.DrawPath(new Pen(this.Parent.BackColor,1), pathBorder);
                }
                int num = IndexTool + 1;


                pevent.Graphics.DrawImage(IconTool, pFist);
                pevent.Graphics.DrawString(num + "." + Name, Font, Brushes.Black, new PointF(pFist.X + IconTool.Width + 5, pFist.Y));
                Size sz = Cal.GetSizeText(Status, new Font("Arial", 20, FontStyle.Bold));
                szNameTool = Cal.GetSizeText(num + "." + Name, Font);
                if (Status == null)
                    Status = String.Empty;

                pevent.Graphics.FillRectangle(new SolidBrush(ClStatus), new RectangleF(pEnd.X - szStatus.Width, pEnd.Y, szStatus.Width, szStatus.Height));
                pevent.Graphics.DrawString(Status, new Font("Arial", 20, FontStyle.Bold), Brushes.White, new PointF(pEnd.X - szStatus.Width / 2 - sz.Width / 2, pEnd.Y + szStatus.Height / 2 - sz.Height / 2));
                sz = Cal.GetSizeText(CT + "ms", new Font("Arial", 10, FontStyle.Bold));
                int space1 = 5;
                pevent.Graphics.FillRectangle(new SolidBrush(Color.Gray), new RectangleF(pEnd.X - szStatus.Width - space1 - szCT.Width, pEnd.Y, szCT.Width, szCT.Height));
                pevent.Graphics.DrawString(CT + "ms", new Font("Arial", 10, FontStyle.Bold), Brushes.White, new PointF(pEnd.X - szStatus.Width - space1 - szCT.Width / 2 - sz.Width / 2, pEnd.Y + szCT.Height / 2 - sz.Height / 2));
                space1 = 3;
                sz = Cal.GetSizeText(Score, new Font("Arial", 18, FontStyle.Bold));
                pevent.Graphics.DrawString(Score, new Font("Arial", 18, FontStyle.Bold), Brushes.Gray, new PointF(pEnd.X - szStatus.Width / 2 - sz.Width / 2, pEnd.Y + szStatus.Height + space1));


                if (max == min) max++;
                int LocalValue = (int)((valueScore / ((max - min) * 1.0)) * (szTrack.Width - 2));
                // int w = imgTick.Width;
                //  Image imgBar = Properties.Resources.BID_SLIDER_SCALE_8PIX_W303;
                rect = new Rectangle(pTrack.X, pTrack.Y, szTrack.Width, szTrack.Height);
                pevent.Graphics.FillRectangle(new SolidBrush(colorTrack), rect);

                //  pevent.Graphics.DrawImage(imgBar, new Rectangle(pTrack.X,pTrack.Y, szTrack.Width , imgBar.Height));

                pevent.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(50, 255, 255, 255)), new RectangleF(pTrack.X, pTrack.Y, LocalValue, szTrack.Height));
                if (!NotChange)
                {
                    pevent.Graphics.DrawImage(imgTick, pTick);
                    sz = Cal.GetSizeText(value + "", new Font("Arial", 11));

                    pevent.Graphics.DrawString(value + "", new Font("Arial", 11), Brushes.Black, new PointF(pTick.X + imgTick.Width / 2 - (int)sz.Width / 2, pTick.Y + imgTick.Height));
                }
            }
            catch(Exception ex)
            {

            }
        }
        public Color ClStatus = Color.Green;
        public String Status = "---";
        public String Score = "---";
        public Color ClScore = Color.Green;
        public double CT = 0;


           protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (this.Parent == null)
                return;
            this.Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
        }

        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void ItemTool_MouseLeave(object sender, EventArgs e)
        {
            isHovered = false;
            this.Invalidate();
            if (_IsEdit)
                imgTick = Properties.Resources.Enable;
            else
                imgTick = Properties.Resources.Disnable;
            this.Refresh();
         
            //if (!IsCLick)
            //    this.BackgroundImage = imgUnChoose;
            //else
            //    this.BackgroundImage = imgChoose;

        }
   
        private bool _IsEdit = false;

      
        public bool IsEdit
        {
            get => _IsEdit;
            set
            {
                if (_IsEdit != value)
                {
                    _IsEdit = value;
                    if(_IsEdit)
                        imgTick = Properties.Resources.Enable;
                    else
                        imgTick = Properties.Resources.Disnable;
                    this.Refresh();
                }
            }
        }
        private void ItemTool_MouseMove(object sender, MouseEventArgs e)
        {
            isHovered = true;
            this.Invalidate();
            if (!NotChange)
            {
                if (IsDown && IsEdit)
                {
                    imgTick = Properties.Resources.Choose;
                    Point pointPanel = e.Location;
                    if (pointPanel.X >= imgTick.Width / 2 && pointPanel.X <= pTrack.X + szTrack.Width - imgTick.Width / 2)
                    {
                        Value = (float)Math.Round((float)((pointPanel.X - imgTick.Width / 2 - pTrack.X) / ((szTrack.Width - imgTick.Width) * 1.0) * (Max - Min)), 1);
                        Value = Value - Value % Step;
                    }

                }
                else
                {
                    if (_IsEdit)
                        imgTick = Properties.Resources.Enable;
                    else
                        imgTick = Properties.Resources.Disnable;
                    this.Refresh();

                }
            }
               
          //  imgTick = Properties.Resources.Enable;
            //if (!IsCLick)
            //    this.BackgroundImage = imgSelect;
            //else
            //    this.BackgroundImage = imgChoose;

        }

        //Image imgChoose = Properties.Resources.btnChoose1;
        //Image imgSelect = Properties.Resources.btnSelect;
        //Image imgUnChoose = Properties.Resources.btnUnChoose;
        public int IndexThread = 0;
        private void ItemTool_Load(object sender, EventArgs e)
        {
            UpdateLayout();
            this.DoubleClick += ItemTool_DoubleClick;
            Step = Common.TryGetTool(IndexThread, IndexTool).StepValue;
            Min = Common.TryGetTool(IndexThread, IndexTool).MinValue;
            Max = Common.TryGetTool(IndexThread, IndexTool).MaxValue;
            Value = BeeCore.Common.TryGetTool(IndexThread, IndexTool).Score;
            Common.TryGetTool(IndexThread, IndexTool).StatusToolChanged -= ItemTool_StatusToolChanged;
            Common.TryGetTool(IndexThread, IndexTool).StatusToolChanged += ItemTool_StatusToolChanged;
            Common.TryGetTool(IndexThread, IndexTool).ScoreChanged -= ItemTool_ScoreChanged;
            Common.TryGetTool(IndexThread, IndexTool).ScoreChanged += ItemTool_ScoreChanged;
            this.Parent.VisibleChanged += Parent_VisibleChanged1;
            imgTick = Properties.Resources.Disnable;
            this.Resize += ItemTool_Resize;
            this.Refresh();
        }

        private void ItemTool_Resize(object sender, EventArgs e)
        {
            UpdateLayout();
            this.Invalidate();
        }

        private void UpdateLayout()
        {
            var r = this.ClientRectangle;
            int margin = 10;

            // 1) V? trí và size thanh track:
            szTrack = new Size(r.Width - szStatus.Width - margin * 2, 15);
            pTrack = new Point(margin, r.Height / 2 - szTrack.Height / 2+5);

            // 2) V? trí tick theo value:
            float ratio = (Value - Min) / (Max - Min);
            pTick = new PointF(
                pTrack.X + ratio * (szTrack.Width - imgTick.Width),
                 pTrack.Y
            );

            // 3) V? trí icon/label trên d?u:
            pFist = new Point(margin, margin);
            pEnd = new PointF(r.Right - margin, margin);

           
        }
        private void ItemTool_ScoreChanged(float obj)
        {
            Value = obj;
            this.Refresh();
        }

        private void ItemTool_StatusToolChanged(PropetyTool tool, StatusTool obj)
        {
            if (TriggerNum != Global.TriggerNum&&!Global.Config.IsMultiProg)
                return;
            switch(obj)
            {
               
                case StatusTool.NotInitial:
                    break;

                case StatusTool.WaitCheck:
                    valueScore = 0;
                    Score = "---";
                    Status = "---";
                    CT = 0;
                    ClStatus = Global.ColorNone;
                    ClScore = Global.ColorNone;
                    colorTrack = Global.ColorNone;
                    break;
                case StatusTool.Processing:
                    valueScore = 0;
                    Score = "---" ;
                    Status = "WAIT";
                    CT = 0; 
                    ClStatus = Global.ColorNone;
                    ClScore = Global.ColorNone;
                    colorTrack = Global.ColorNone;
                    break;
                case StatusTool.Done:
                  
                    if(Common.TryGetTool(IndexThread, IndexTool).Results==Results.OK)
                    {   
                        if (Common.TryGetTool(IndexThread, IndexTool).Location != null&&Common.TryGetTool(IndexThread, IndexTool).Location != "")
                            Score = Common.TryGetTool(IndexThread, IndexTool).Location;
                        else 
                        {
                            valueScore = Common.TryGetTool(IndexThread, IndexTool).ScoreResult;
                            Score = valueScore + "";
                        }    
                         
                        Status = Common.TryGetTool(IndexThread, IndexTool).Results.ToString();
                        CT = Common.TryGetTool(IndexThread, IndexTool).CycleTime;
                        colorTrack =  Global.ParaShow.ColorOK;
                        ClStatus =  Global.ParaShow.ColorOK;
                        ClScore=  Global.ParaShow.ColorOK;
                    }
                    else if (Common.TryGetTool(IndexThread, IndexTool).Results == Results.NG)
                    {
                        if (Common.TryGetTool(IndexThread, IndexTool).Location != "")
                            Score = Common.TryGetTool(IndexThread, IndexTool).Location;
                        else
                        {
                            valueScore = Common.TryGetTool(IndexThread, IndexTool).ScoreResult;
                            Score = valueScore + "";
                        }
                        Status = Common.TryGetTool(IndexThread, IndexTool).Results.ToString();
                        CT = Common.TryGetTool(IndexThread, IndexTool).CycleTime;
                        colorTrack = Global.ParaShow.ColorNG;
                        ClStatus = Global.ParaShow.ColorNG;
                        ClScore = Global.ParaShow.ColorNG;
                    }
                    else if (Common.TryGetTool(IndexThread, IndexTool).Results == Results.None)
                    {
                        Score = "---";
                        Status = "NC";
                        colorTrack = Global.ColorNone;
                        ClStatus = Global.ColorNone;
                        ClScore = Global.ColorNone;
                    }
                   
                        break;
              

            }
           
            this.Invalidate();
        }

        Control control=new Control();
        public int IndexTool = -1;
        public PropetyTool PropetyOld = null;
       private void ItemTool_DoubleClick(object sender, EventArgs e)
        {
            //if (Global.ParaCommon.matRegister != null)
            //    BeeCore.Common.listCamera[IndexCCD].matRaw = OpenCvSharp.Extensions.BitmapConverter.ToMat(Global.ParaCommon.matRegister);
            //else if (G.IsCCD)
            //    BeeCore.Common.listCamera[IndexCCD].matRaw = null;// BeeCore.Common.GetImageRaw();
            //if (BeeCore.Common.listCamera[IndexCCD].matRaw == null)
            //{
            //    MessageBox.Show("Vui long dang ky Anh");
            //    return;
            //}
         
            Global.IndexToolSelected = -1;
            if (Global.IsRun) return;
            //this.Parent.Visible = false;
            txtEdit.Visible = false;
            Global.StatusDraw = StatusDraw.Edit;
            Global.IsEditTool = true;
            Global.IndexToolSelected = IndexTool;
          
            // G.listAlltool[IndexThread].FindIndex(a => a.ItemTool == this);

        }

      
        public TextBox txtEdit = new TextBox();
        Point pFist = new Point(5, 20);
        PointF pEnd = new PointF(5,100);
        public void EditName()
        {
           
            txtEdit.Visible = true;
            txtEdit.KeyDown -= TxtEdit_KeyDown;
            txtEdit.Parent = this;
            // txtEdit.Font = name.Font;
            txtEdit.Location = new Point(pFist.X + IconTool.Width + 5, pFist.Y-5);
            txtEdit.Font = new Font("Arial", 14);
            txtEdit.Size = new Size(szNameTool.Width+20, 30);
            txtEdit.BringToFront();
            txtEdit.Text = Name;
            txtEdit.Focus();
            txtEdit.KeyDown += TxtEdit_KeyDown;
        }
        public void VisbleEditname()
        {
            txtEdit.Visible = false;
        }
        private void name_DoubleClick(object sender, EventArgs e)
        {
            //G.indexToolSelected = G.listAlltool[IndexThread].FindIndex(a => a.ItemTool == this);

          
        }

        private void TxtEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                Global.IndexToolSelected = IndexTool;
                BeeCore.Common.TryGetTool(IndexThread, Global.IndexToolSelected).Name = txtEdit.Text.Trim();
                BeeCore.Common.TryGetTool(IndexThread, Global.IndexToolSelected).Propety2.SetModel();
              Name= txtEdit.Text.Trim();
                txtEdit.Visible = false;
                this.Invalidate();
              Global.ToolSettings.  btnRename.IsCLick = false;
            }    
        }

        private void lbStatus_Click(object sender, EventArgs e)
        {

        }

        private void Score_Load(object sender, EventArgs e)
        {

        }

        private void ItemTool_SizeChanged(object sender, EventArgs e)
        {
            //szTrack = new Size(this.Width - szStatus.Width + 10, 50);
            //bmpBase = new Bitmap(szTrack.Width, szTrack.Height);
            //gcBase = Graphics.FromImage(bmpBase);
            //pTrack = new Point(this.Height / 2 - szTrack.Height / 2, szStatus.Height + 10);
        }
    }
}
