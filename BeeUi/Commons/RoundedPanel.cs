using System;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Management.Instrumentation;
using System.Windows.Forms;

namespace BeeUi.Commons
{
  
    public class RoundedPanel : Panel
    {
        public int BorderRadius { get; set; } = 20; // Bo góc
        public int BorderSize { get; set; } = 2;    // Độ dày viền
        public Color BorderColor { get; set; } = Color.Transparent;

        public RoundedPanel()
        {
            this.BackColor = Color.White;
            this.Size = new Size(200, 200);
            this.DoubleBuffered = true;
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
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Vẽ đường viền bo tròn
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            int radius = BorderRadius * 2;

            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);  // Trái trên
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90); // Phải trên
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90); // Phải dưới
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);  // Trái dưới
            path.CloseFigure();

            // Cắt vùng hiển thị theo hình bo góc
            this.Region = new Region(path);

            //// Vẽ viền
            //using (Pen pen = new Pen(BorderColor, BorderSize))
            //{
            //    g.DrawPath(pen, path);
            //}
            Rectangle rectSurface = this.ClientRectangle;
              int smoothSize = 4;


          
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, BorderRadius))
            
                using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
             
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    //Button surface
                   
                        g.DrawPath(penSurface, pathSurface);

                    ////Button border                    
                    //if (borderSize >= 1)
                    //    //Draw control border
                    //    pevent.Graphics.DrawPath(penBorder, pathBorder);
                    //else
                    //    pevent.Graphics.DrawPath(new Pen(this.Parent.BackColor,1), pathBorder);


                }
            
        }
    }
}
