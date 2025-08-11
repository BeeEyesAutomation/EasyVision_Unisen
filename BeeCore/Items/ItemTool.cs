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
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using Image = System.Drawing.Image;
using TextBox = System.Windows.Forms.TextBox;

namespace BeeCore
{ [Serializable()]
    public partial class ItemTool : UserControl
    {
        public PointF pTick;
        private Color colorTrack = Color.Gray;
        private Image imgTick = Properties.Resources.Enable;
        public TypeTool TypeTool;
        public Image IconTool;
        private float valueScore;
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
                if (Value < Min) Value = Min;
                this.Refresh();
            } }
        public float Max { get => max; set
            {
                max = value;
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
                {
                    this.value = (float)Math.Round(value, 1);
                    pTick = new Point(pTrack.X + (int)((value * 1.0 / (Max - Min)) * (this.szTrack.Width - imgTick.Width)), pTrack.Y);
                    BeeCore.Common.PropetyTools[Global.IndexChoose][IndexTool].Score = Value;
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
                    //gcBase.FillRectangle(new SolidBrush(colorTrack), rect);
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
            if (e.Location.X -pTick.X >5)
                Value += Step;
            else if(pTick.X - e.Location.X > 5)
                Value -= Step;
        }

        public ItemTool(TypeTool typeTool,string names)
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
        
            //if(TypeTool==TypeTool.Measure)
            //{
            //    Score.Max = 10;
            //}




        }

        private void ItemTool_VisibleChanged(object sender, EventArgs e)
        {
          //  Value = BeeCore.Common.PropetyTools[Global.IndexChoose][IndexTool].Score;
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
            //if (Parent != null && Global.indexToolSelected != -1 && Global.indexToolSelected < G.listAlltool[Global.IndexChoose].Count)
            //{
            //    if (this.Parent.Visible)
            //    {
            //        G.IsEdit = false;
            //        if (G.listAlltool[Global.IndexChoose].FindIndex(a => a.ItemTool == this) != G.indexToolSelected) return;
            //        if (G.PropetyOld != null && G.IsCancel)
            //        {
            //            G.IsCancel = false;
            //            G.listAlltool[Global.IndexChoose][G.indexToolSelected].tool.Propety = G.PropetyOld.Clone();
            //            BeeCore.Common.PropetyTools[Global.IndexChoose][G.indexToolSelected].Propety = G.listAlltool[Global.IndexChoose][G.indexToolSelected].tool.Propety;

            //            G.EditTool.View.imgView.Invalidate();
            //        }
            //        Score.Value = BeeCore.Common.PropetyTools[Global.IndexChoose][G.indexToolSelected].Propety.Score;
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
        private Size szTrack = new Size(60, 15);
        private Point pTrack = new Point(10, 10);
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            pEnd = new PointF(this.Width - 5, 5);
            Rectangle rectSurface = this.ClientRectangle;
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            // Xác định màu nền dựa trên trạng thái
            Color topColor, middleColor, bottomColor;

            if (isCLick)
            {
                // Màu khi bấm xuống
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
                // Màu mặc định
                topColor = Color.FromArgb(243, 247, 250);
                middleColor = Color.FromArgb(218, 221, 224);
                bottomColor = Color.FromArgb(199, 203, 206);
            }

            // Gradient 3 màu
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.White, Color.Gray, LinearGradientMode.Vertical))
            {
                ColorBlend colorBlend = new ColorBlend();
                colorBlend.Colors = new Color[] { topColor, middleColor, bottomColor };
                colorBlend.Positions = new float[] { 0.0f, 0.5f, 1.0f }; // 3 điểm màu
                brush.InterpolationColors = colorBlend;

                pevent.Graphics.FillRectangle(brush, rect);
            }
            // Vẽ hình ảnh nếu có

            // Vẽ hình ảnh nếu có
            int imgSize = Math.Min(this.Height - 10, 24); // Giới hạn kích thước ảnh
            Rectangle imgRect = Rectangle.Empty;
            Rectangle textRect = rect;
            int spacing = 5; // Khoảng cách giữa ảnh và chữ

         
                // Vẽ chữ trên button
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
            pevent.Graphics.DrawString(num+"."+Name, Font,Brushes.Black, new PointF(pFist.X+IconTool.Width+5, pFist.Y));
            Size sz = Cal.GetSizeText(Status, new Font("Arial", 20, FontStyle.Bold));
            if (Status == null)
               Status = String.Empty;

            pevent.Graphics.FillRectangle(new SolidBrush(ClStatus), new RectangleF(pEnd.X- szStatus.Width, pEnd.Y, szStatus.Width, szStatus.Height));
            pevent.Graphics.DrawString(Status, new Font("Arial",20,FontStyle.Bold), Brushes.White, new PointF(pEnd.X - szStatus .Width/ 2- sz.Width/2, pEnd.Y+ szStatus .Height/ 2-sz.Height/2));
             sz = Cal.GetSizeText(CT + "ms", new Font("Arial", 10, FontStyle.Bold));
            int space1 = 5;
            pevent.Graphics.FillRectangle(new SolidBrush(Color.Gray), new RectangleF(pEnd.X - szStatus.Width- space1 - szCT.Width, pEnd.Y , szCT.Width, szCT.Height));
            pevent.Graphics.DrawString(CT+"ms", new Font("Arial", 10, FontStyle.Bold), Brushes.White, new PointF(pEnd.X - szStatus.Width - space1 - szCT.Width/2 - sz.Width / 2  , pEnd.Y +szCT.Height/2- sz.Height / 2));
             space1 =3;
            sz = Cal.GetSizeText(Score, new Font("Arial", 18, FontStyle.Bold));
            pevent.Graphics.DrawString(Score, new Font("Arial", 18, FontStyle.Bold), Brushes.Gray, new PointF(pEnd.X - szStatus.Width/2  - sz.Width / 2, pEnd.Y + szStatus.Height + space1 ));
           
            
            if (max == min) max++;
            int LocalValue = (int)((valueScore / ((max - min) * 1.0)) * (szTrack.Width - 2));
            // int w = imgTick.Width;
          //  Image imgBar = Properties.Resources.BID_SLIDER_SCALE_8PIX_W303;
             rect = new Rectangle(pTrack.X,pTrack.Y, szTrack.Width, szTrack.Height );
            pevent.Graphics.FillRectangle(new SolidBrush(colorTrack), rect);

          //  pevent.Graphics.DrawImage(imgBar, new Rectangle(pTrack.X,pTrack.Y, szTrack.Width , imgBar.Height));

            pevent.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(50, 255, 255, 255)), new RectangleF(pTrack.X,pTrack.Y, LocalValue, szTrack.Height));

            pevent.Graphics.DrawImage(imgTick, pTick);
            sz = Cal.GetSizeText(value + "", new Font("Arial", 11));

            pevent.Graphics.DrawString(value + "", new Font("Arial", 11), Brushes.Black, new PointF(pTick.X + imgTick.Width / 2 - (int)sz.Width / 2, pTick.Y + imgTick.Height));

        }
        public Color ClStatus = Color.Green;
        public String Status = "---";
        public String Score = "---";
        public Color ClScore = Color.Green;
        public double CT = 0;


           protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
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
            if (IsDown && IsEdit)
            {
                imgTick = Properties.Resources.Choose;
                Point pointPanel = e.Location;
                if (pointPanel.X >= imgTick.Width / 2  && pointPanel.X <= pTrack.X+ szTrack.Width - imgTick.Width / 2 )
                {
                    Value = (float)Math.Round((float)((pointPanel.X - imgTick.Width / 2 -pTrack.X) / ((szTrack.Width - imgTick.Width ) * 1.0) * (Max - Min)), 1);
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
               
          //  imgTick = Properties.Resources.Enable;
            //if (!IsCLick)
            //    this.BackgroundImage = imgSelect;
            //else
            //    this.BackgroundImage = imgChoose;

        }

        //Image imgChoose = Properties.Resources.btnChoose1;
        //Image imgSelect = Properties.Resources.btnSelect;
        //Image imgUnChoose = Properties.Resources.btnUnChoose;
        private void ItemTool_Load(object sender, EventArgs e)
        {
            UpdateLayout();
            this.DoubleClick += ItemTool_DoubleClick;
            Step = Common.PropetyTools[Global.IndexChoose][IndexTool].StepValue;
            Min = Common.PropetyTools[Global.IndexChoose][IndexTool].MinValue;
            Max = Common.PropetyTools[Global.IndexChoose][IndexTool].MaxValue;
          
            Value = BeeCore.Common.PropetyTools[Global.IndexChoose][IndexTool].Score;
            Common.PropetyTools[Global.IndexChoose][IndexTool].StatusToolChanged += ItemTool_StatusToolChanged;
            Common.PropetyTools[Global.IndexChoose][IndexTool].ScoreChanged += ItemTool_ScoreChanged;
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

            // 1) Vị trí và size thanh track:
            szTrack = new Size(r.Width - szStatus.Width - margin * 2, 15);
            pTrack = new Point(margin, r.Height / 2 - szTrack.Height / 2+5);

            // 2) Vị trí tick theo value:
            float ratio = (Value - Min) / (Max - Min);
            pTick = new PointF(
                pTrack.X + ratio * (szTrack.Width - imgTick.Width),
                pTrack.Y - (imgTick.Height - szTrack.Height) / 2
            );

            // 3) Vị trí icon/label trên đầu:
            pFist = new Point(margin, margin);
            pEnd = new PointF(r.Right - margin, margin);

           
        }
        private void ItemTool_ScoreChanged(float obj)
        {
            Value = obj;
            this.Refresh();
        }

        private void ItemTool_StatusToolChanged(StatusTool obj)
        {
            switch(obj)
            {
               
                case StatusTool.NotInitial:
                    break;
                case StatusTool.Processing:
                    valueScore = 0;
                    Score = "---" ;
                    Status = "---";
                    CT = 0; 
                    ClStatus = Global.ColorNone;
                    ClScore = Global.ColorNone;
                    colorTrack = Global.ColorNone;
                    break;
                case StatusTool.Done:
                  
                    if(Common.PropetyTools[Global.IndexChoose][IndexTool].Results==Results.OK)
                    {
                        valueScore = Common.PropetyTools[Global.IndexChoose][IndexTool].ScoreResult;
                        Score = valueScore + "";
                        Status = Common.PropetyTools[Global.IndexChoose][IndexTool].Results.ToString();
                        CT = Common.PropetyTools[Global.IndexChoose][IndexTool].CycleTime;
                        colorTrack = Global.ColorOK;
                        ClStatus = Global.ColorOK;
                        ClScore= Global.ColorOK;
                    }
                    else if (Common.PropetyTools[Global.IndexChoose][IndexTool].Results == Results.None)
                    {
                        Score = "---";
                        Status = "NC";
                        colorTrack = Global.ColorNone;
                        ClStatus = Global.ColorNone;
                        ClScore = Global.ColorNone;
                    }
                    else
                    {
                        valueScore = Common.PropetyTools[Global.IndexChoose][IndexTool].ScoreResult;
                        Score = valueScore + "";
                        Status = Common.PropetyTools[Global.IndexChoose][IndexTool].Results.ToString();
                        CT = Common.PropetyTools[Global.IndexChoose][IndexTool].CycleTime;
                        colorTrack = Global.ColorNG;
                        ClStatus = Global.ColorNG;
                        ClScore = Global.ColorNG;
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
            //    BeeCore.Common.listCamera[Global.IndexChoose].matRaw = OpenCvSharp.Extensions.BitmapConverter.ToMat(Global.ParaCommon.matRegister);
            //else if (G.IsCCD)
            //    BeeCore.Common.listCamera[Global.IndexChoose].matRaw = null;// BeeCore.Common.GetImageRaw();
            //if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw == null)
            //{
            //    MessageBox.Show("Vui long dang ky Anh");
            //    return;
            //}
            Global.IndexToolSelected = -1;
            if (Global.IsRun) return;
            //this.Parent.Visible = false;
            txtEdit.Visible = false;
            Global.StatusDraw = StatusDraw.Edit;
            Global.IndexToolSelected = IndexTool;
            Global.IsEditTool = true;
            // G.listAlltool[Global.IndexChoose].FindIndex(a => a.ItemTool == this);

        }

        private void Score_ValueChanged(float obj)
        {
            Global.IndexToolSelected = IndexTool;
          //  BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.Score = (float)Score.Value;
           
        }
        TextBox txtEdit = new TextBox();
        Point pFist = new Point(5, 5);
        PointF pEnd = new PointF(5,100);
        private void name_DoubleClick(object sender, EventArgs e)
        {
            //G.indexToolSelected = G.listAlltool[Global.IndexChoose].FindIndex(a => a.ItemTool == this);

            if (Global.IsRun) return;
            txtEdit.Visible = true;
            txtEdit.KeyDown -= TxtEdit_KeyDown;
            txtEdit.Parent = this;
           // txtEdit.Font = name.Font;
            txtEdit.Location = new Point(pFist.X, pFist.Y - 2);

            txtEdit.Size = new Size(100,30);
            txtEdit.BringToFront();
            txtEdit.Text = Name;
            txtEdit.Focus();
            txtEdit.KeyDown += TxtEdit_KeyDown;
        }

        private void TxtEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                Global.IndexToolSelected = IndexTool;
                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Name = txtEdit.Text.Trim();
                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.SetModel();
              Name= txtEdit.Text.Trim();
                txtEdit.Visible = false;
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
