using BeeCore;
using BeeGlobal;
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

namespace BeeInterface
{ [Serializable()]
    public partial class ItemTool : UserControl
    {
        public TypeTool TypeTool;
        
        public ItemTool(TypeTool typeTool,string names)
        {
          
            InitializeComponent();

        
            this.Name = names;
            this.TypeTool = typeTool;
            this.MouseMove += ItemTool_MouseMove;
            this.MouseLeave += ItemTool_MouseLeave;
            this.Click += ItemTool_Click;
          
           
            if(TypeTool==TypeTool.Measure)
            {
                Score.Max = 10;
            }
         
          
         


        }

        private void Measure_ResultChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
            //Global.indexToolSelected = G.listAlltool[Global.IndexChoose].FindIndex(a => a.ItemTool == this);
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
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);


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


        }
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
            //if (!IsCLick)
            //    this.BackgroundImage = imgUnChoose;
            //else
            //    this.BackgroundImage = imgChoose;

        }

        private void ItemTool_MouseMove(object sender, MouseEventArgs e)
        {
            isHovered = true;
            this.Invalidate();
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
            lbNumber.MouseMove += ItemTool_MouseMove;
            lbNumber.MouseLeave += ItemTool_MouseLeave;
            lbNumber.Click += ItemTool_Click;
            name.MouseMove += ItemTool_MouseMove;
            name.MouseLeave += ItemTool_MouseLeave;
            name.Click += ItemTool_Click;
            icon.MouseMove += ItemTool_MouseMove;
            icon.MouseLeave += ItemTool_MouseLeave;
            icon.Click += ItemTool_Click;
            this.DoubleClick += ItemTool_DoubleClick;
            this.Parent.VisibleChanged += Parent_VisibleChanged1;
           
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
            if (Global.IsRun) return;
            //this.Parent.Visible = false;
            txtEdit.Visible = false;
            Global.StatusDraw = StatusDraw.Edit;
            Global.IndexToolSelected = IndexTool;
            // G.listAlltool[Global.IndexChoose].FindIndex(a => a.ItemTool == this);
           
        }

        private void Score_ValueChanged(float obj)
        {
            Global.IndexToolSelected = IndexTool;
            BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Propety.Score = (float)Score.Value;
           
        }
        TextBox txtEdit = new TextBox();
        private void name_DoubleClick(object sender, EventArgs e)
        {
            //G.indexToolSelected = G.listAlltool[Global.IndexChoose].FindIndex(a => a.ItemTool == this);

            if (Global.IsRun) return;
            txtEdit.Visible = true;
            txtEdit.KeyDown -= TxtEdit_KeyDown;
            txtEdit.Parent = this;
            txtEdit.Font = name.Font;
            txtEdit.Location = new Point(name.Location.X, name.Location.Y - 2);
            txtEdit.Size = name.Size;
            txtEdit.BringToFront();
            txtEdit.Text = name.Text;
            txtEdit.Focus();
            txtEdit.KeyDown += TxtEdit_KeyDown;
        }

        private void TxtEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                BeeCore.Common.PropetyTools[Global.IndexChoose][Global.IndexToolSelected].Name = txtEdit.Text.Trim();
                name.Text= txtEdit.Text.Trim();
                txtEdit.Visible = false;
            }    
        }

        private void lbStatus_Click(object sender, EventArgs e)
        {

        }

        private void Score_Load(object sender, EventArgs e)
        {

        }
    }
}
