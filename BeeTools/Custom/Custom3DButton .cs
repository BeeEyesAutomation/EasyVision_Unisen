using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace BeeInterface
{
    public class Gradient3DButton : Button
    {
        private bool isHovered = false;
        private bool isPressed = false;

        public Gradient3DButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = Color.Red;
            this.ForeColor = Color.Black;

            this.MouseEnter += (s, e) => { isHovered = true; this.Invalidate(); };
            this.MouseLeave += (s, e) => { isHovered = false; isPressed = false; this.Invalidate(); };
            this.MouseDown += (s, e) => { isPressed = true; this.Invalidate(); };
            this.MouseUp += (s, e) => { isPressed = false; this.Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Vùng vẽ nút
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            // Gradient màu sắc (3 màu)
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.White, Color.Gray, LinearGradientMode.Vertical))
            {
                ColorBlend colorBlend = new ColorBlend();
                colorBlend.Colors = new Color[]
                {
                isPressed ? Color.DarkGray : Color.White,  // Màu trên
                isPressed ? Color.Gray : Color.LightGray,  // Màu giữa
                isPressed ? Color.Black : Color.DarkGray   // Màu dưới
                };

                colorBlend.Positions = new float[] { 0.0f, 0.5f, 1.0f }; // Vị trí màu trong gradient (0% - 50% - 100%)
                brush.InterpolationColors = colorBlend;

                g.FillRectangle(brush, rect);
            }


            // Vẽ viền mềm mại
           
            // Bo tròn góc nút
            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = 8; // Độ bo tròn
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();

                this.Region = new Region(path);
            }
            using (Pen pen = new Pen(Color.Red, 1))
            {
                g.DrawRectangle(pen, rect);
            }

            // Vẽ chữ
            TextRenderer.DrawText(g, this.Text, this.Font, rect, this.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
