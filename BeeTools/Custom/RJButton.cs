using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeeGlobal;

namespace BeeInterface
{
  
    [Serializable()]
    public class RJButton : Button
    {
        //Fields
        private int borderSize = 0;
        private int borderRadius = 0;
        private Corner _Corner = 0;
        private Color borderColor = Color.PaleVioletRed;

        //Properties
        [Category("RJ Code Advance")]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                borderSize = value;
                this.Invalidate();
            }
        }
        //Properties
        [Category("_Corner")]
        public Corner Corner
        {
            get { return _Corner; }
            set
            {
                _Corner = value;
                this.Invalidate();
            }
        }
        [Category("RJ Code Advance")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                borderRadius = value;
                this.Invalidate();
            }
        }

        [Category("RJ Code Advance")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }

        [Category("RJ Code Advance")]
        public Color BackgroundColor
        {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        }

        [Category("RJ Code Advance")]
        public Color TextColor
        {
            get { return this.ForeColor; }
            set { this.ForeColor = value; }
        }
        bool _IsRect=false;
        //Image imgChoose = Properties.Resources.btnChoose1;
        //Image imgSelect = Properties.Resources.btnSelect;
        //Image imgUnChoose = Properties.Resources.btnUnChoose;
        [Category("Bool Button Rect")]
        public Boolean IsRect
        {
            get { return this._IsRect; }
            set { this._IsRect = value;
                //if(_IsRect)
                //{
                //    imgChoose = Properties.Resources.btnChoose2;
                //    imgUnChoose = Properties.Resources.btnUnChoose2;
                //    imgSelect = Properties.Resources.btnSelect2;
                //}
                //if (IsCLick)
                //    this.BackgroundImage = imgChoose;
                //else
                //    this.BackgroundImage = imgUnChoose;
                this.Invalidate();

            }
        }
       
        public bool IsCLick {
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
                if(value==true&&!this.IsUnGroup&&this.Parent!=null)
                foreach (Control c in this.Parent.Controls)
                {

                    if (c is RJButton && c != this)
                    {
                        RJButton btn = c as RJButton;
                        btn.IsCLick = false;
                      //  this.borderColor = Color.Silver;
                      //  c.BackgroundImage = imgUnChoose;
                    }
                }
                this.Invalidate();

            }
        }
        private bool isNotChange;
        [Category("Bool Button Rect")]
        public bool IsNotChange { get => isNotChange; set
            {
                isNotChange = value; this.Invalidate();
            }
        }

        public bool IsUnGroup { get => isUnGroup; set { isUnGroup = value; this.Invalidate(); } }

        //Constructor
        public RJButton()
        {
           
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(150, 40);
            this.BackgroundColor = Color.Transparent;
            this.ForeColor = Color.White;
            this.Resize += new EventHandler(Button_Resize);
            this.SizeChanged += RJButton_SizeChanged;
            this.MarginChanged += RJButton_MarginChanged;
           // this.MouseMove += (s, e) => { isHovered = true; this.Invalidate(); };
           // this.MouseLeave += (s, e) => { isHovered = false; isPressed = false; this.Invalidate(); };
           // this.Click += (s, e) => { isPressed = true; this.Invalidate(); };
           //// this.MouseUp += (s, e) => { isPressed = false; this.Invalidate(); };
            this.MouseMove += RJButton_MouseMove;
            this.MouseLeave += RJButton_MouseLeave;
            this.Click += RJButton_Click;
           
            //if (IsRect)
            //{
            //    imgChoose = Properties.Resources.btnChoose2;
            //    imgUnChoose = Properties.Resources.btnUnChoose2;
            //    imgSelect = Properties.Resources.btnSelect2;
            //}else
            //{
            //    imgChoose = Properties.Resources.btnChoose1;
            //    imgUnChoose = Properties.Resources.btnUnChoose;
            //    imgSelect = Properties.Resources.btnSelect;

            //}
            //if(IsCLick)
            //this.BackgroundImage = imgChoose;
            //else
            //    this.BackgroundImage = imgUnChoose;
        }

        private void RJButton_MarginChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void RJButton_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private bool isCLick = false;
        private bool isUnGroup;

        private Image imgOld;
        private void RJButton_Click(object sender, EventArgs e)
        {if (isNotChange) return;
           
            if (IsUnGroup)
            {
                IsCLick = !IsCLick;
                return;
            }
            else
                isPressed = true;
            this.IsCLick = true;
            
            foreach (Control c in this.Parent.Controls)
            {

                if(c is RJButton&&c!=this)
                {
                    RJButton btn = c as RJButton;
                    btn.IsCLick = false;
                  //  this.borderColor = Color.Silver;
                  ///  c.BackgroundImage= imgUnChoose;
                }
            }

        }

        private void RJButton_MouseLeave(object sender, EventArgs e)
        {
            isPressed = false; isHovered = false;
            //if (!IsCLick)
            //        this.BackgroundImage = imgUnChoose;
            //    else
            //        this.BackgroundImage = imgChoose;
               // this.borderColor = Color.Silver;
            
        }

        private void RJButton_MouseMove(object sender, MouseEventArgs e)
        {
            isHovered = true;
            isPressed = false;
            //if (!IsCLick)
            //    this.BackgroundImage = imgSelect;
            //  this.borderColor = Color.Silver;
        }
        private bool isHovered = false;
        private bool isPressed = false;
        //Methods
        private  GraphicsPath GetFigurePath(Rectangle rect, int curveSize)
        {
            GraphicsPath path = new GraphicsPath();

            switch (Corner)
            {
                case Corner.Both:
                    // Bo cả 4 góc
                    path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90); // Top-left
                    path.AddLine(rect.X + curveSize, rect.Y, rect.Right - curveSize, rect.Y);
                    path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90); // Top-right
                    path.AddLine(rect.Right, rect.Y + curveSize, rect.Right, rect.Bottom - curveSize);
                    path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90); // Bottom-right
                    path.AddLine(rect.Right - curveSize, rect.Bottom, rect.X + curveSize, rect.Bottom);
                    path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90); // Bottom-left
                    path.AddLine(rect.X, rect.Bottom - curveSize, rect.X, rect.Y + curveSize);
                    break;

                case Corner.Left:
                    // Bo góc trái (trên + dưới)
                    path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90); // Top-left
                    path.AddLine(rect.X + curveSize, rect.Y, rect.Right, rect.Y); // Top
                    path.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom); // Right
                    path.AddLine(rect.Right, rect.Bottom, rect.X + curveSize, rect.Bottom); // Bottom
                    path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90); // Bottom-left
                    path.AddLine(rect.X, rect.Bottom - curveSize, rect.X, rect.Y + curveSize); // Left
                    break;

                case Corner.Right:
                    // Bo góc phải (trên + dưới)
                    path.AddLine(rect.X, rect.Y, rect.Right - curveSize, rect.Y); // Top
                    path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90); // Top-right
                    path.AddLine(rect.Right, rect.Y + curveSize, rect.Right, rect.Bottom - curveSize); // Right
                    path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90); // Bottom-right
                    path.AddLine(rect.Right - curveSize, rect.Bottom, rect.X, rect.Bottom); // Bottom
                    path.AddLine(rect.X, rect.Bottom, rect.X, rect.Y); // Left
                    break;
                case Corner.None:
                
                    path.AddRectangle( rect); // Top

                    break;
            }

            path.CloseFigure();
            return path;
        }
        public Image ButtonImage { get; set; } // Ảnh hiển thị trên nút
        public ContentAlignment ImageAlign { get; set; } = ContentAlignment.MiddleLeft; // Vị trí ảnh
        public static TextFormatFlags ConvertAlignment(ContentAlignment align)
        {
            TextFormatFlags flags = TextFormatFlags.GlyphOverhangPadding;

            switch (align)
            {
                case ContentAlignment.TopLeft:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                    break;
                case ContentAlignment.TopCenter:
                    flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.TopRight:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                    break;
                case ContentAlignment.MiddleLeft:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case ContentAlignment.MiddleCenter:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.MiddleRight:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;
                case ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                    break;
                case ContentAlignment.BottomCenter:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                    break;
            }

            return flags;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);


            Rectangle rectSurface = this.ClientRectangle;
            Rectangle rectBorder = Rectangle.Inflate(rectSurface, -borderSize, -borderSize);
            Rectangle rect = new Rectangle(0, 0, this.Width , this.Height );
            // Xác định màu nền dựa trên trạng thái
            Color topColor, middleColor, bottomColor;
            if (!this.Enabled)
            {
                // Màu khi bấm xuống
                topColor = this.BackColor;// Color.FromArgb(160, 160, 160);
                middleColor = this.BackColor;// Color.FromArgb(180, 180, 180);
                bottomColor = this.BackColor;// Color.FromArgb(160, 160, 160);//247, 211, 139
            }
           else if (isCLick)
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
                topColor = Color.FromArgb(245, 248, 251);//243, 247, 250
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
      
            Rectangle imgRect = Rectangle.Empty;
        Rectangle textRect = rect;
        int spacing = 5; // Khoảng cách giữa ảnh và chữ
            if (this.Enabled)
            {
                if (Image != null)
                {
                    Size imgSize = Image.Size; // Giới hạn kích thước ảnh

                    switch (TextImageRelation)
                    {
                        case TextImageRelation.ImageBeforeText:
                            imgRect = new Rectangle(10, (this.Height - imgSize.Height) / 2, imgSize.Width, imgSize.Height);
                            textRect = new Rectangle(imgSize.Width + 15, 0, this.Width - imgSize.Width - 20, this.Height);
                            break;
                        case TextImageRelation.TextBeforeImage:
                            imgRect = new Rectangle(this.Width - imgSize.Width - 10, (this.Height - imgSize.Height) / 2, imgSize.Width, imgSize.Height);
                            textRect = new Rectangle(10, 0, this.Width - imgSize.Width - 20, this.Height);
                            break;
                        case TextImageRelation.ImageAboveText:
                            imgRect = new Rectangle((this.Width - imgSize.Width) / 2, 5, imgSize.Width, imgSize.Height);
                            textRect = new Rectangle(0, imgSize.Height + spacing, this.Width, this.Height - imgSize.Height - spacing);
                            break;
                        case TextImageRelation.TextAboveImage:
                            textRect = new Rectangle(0, 0, this.Width, this.Height - imgSize.Height - spacing);
                            imgRect = new Rectangle((this.Width - imgSize.Width) / 2, this.Height - imgSize.Height - 5, imgSize.Width, imgSize.Height);
                            break;
                        case TextImageRelation.Overlay:
                            imgRect = new Rectangle((this.Width - imgSize.Width) / 2, (this.Height - imgSize.Height) / 2, imgSize.Width, imgSize.Height);
                            break;
                    }
                    pevent.Graphics.DrawImage(Image, imgRect);
                    TextFormatFlags flags = ConvertAlignment(this.TextAlign);

                    
                    TextRenderer.DrawText(pevent.Graphics, this.Text, this.Font, textRect, this.ForeColor, flags);



                    // Vẽ chữ trên button

                }
                else
                {

                    // Vẽ chữ trên button
                    textRect = new Rectangle(0, 0, this.Width, this.Height);
                    TextFormatFlags flags = ConvertAlignment(this.TextAlign);

                    //  TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

                    //if (Image != null && ImageAlign == ContentAlignment.MiddleLeft)
                    //    textRect = new Rectangle(imgSize.Width + 15, 0, this.Width - imgSize.Width - 15, this.Height);
                    //else if (ButtonImage != null && ImageAlign == ContentAlignment.MiddleRight)
                    //    textRect = new Rectangle(0, 0, this.Width - imgSize.Width - 15, this.Height);
                    // Font cho số "1."
                 
                    TextRenderer.DrawText(pevent.Graphics, this.Text, this.Font, textRect, this.ForeColor, flags);
                }
            }
            int smoothSize = 1;

            if (borderRadius == 0) return;
            if (borderSize >=1) //Rounded button
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius))
                using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    //Button surface
                    this.Region = new Region(pathSurface);
                    //Draw surface border for HD result
                    if (this.Enabled)
                        pevent.Graphics.DrawPath(penSurface, pathSurface);

                    ////Button border                    
                    //if (borderSize >= 1)
                    //    //Draw control border
                    //    pevent.Graphics.DrawPath(penBorder, pathBorder);
                    //else
                    //    pevent.Graphics.DrawPath(new Pen(this.Parent.BackColor,1), pathBorder);


                }
            }
            else //Normal button
            {
                pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                //Button surface
                this.Region = new Region(pathSurface);
                //pevent.Graphics.SmoothingMode = SmoothingMode.None;
                ////Button surface
                //this.Region = new Region(rectSurface);
                ////Button border
                //if (borderSize >= 1)
                //{
                //    using (Pen penBorder = new Pen(borderColor, borderSize))
                //    {
                //        penBorder.Alignment = PenAlignment.Inset;
                //        pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
                //    }
                //}
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
        private void Button_Resize(object sender, EventArgs e)
        {
            if (borderRadius > this.Height)
                borderRadius = this.Height;
        }
    }
}   

