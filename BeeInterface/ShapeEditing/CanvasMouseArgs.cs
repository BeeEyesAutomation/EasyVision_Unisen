using System;
using System.Drawing;
using System.Windows.Forms;

namespace BeeInterface.ShapeEditing
{
    /// <summary>
    /// Event args cho các sự kiện chuột trên ImageCanvasControl.
    /// Khác với MouseEventArgs thông thường, args này đã pre-compute sẵn
    /// toạ độ trong image space để consumer khỏi tự convert.
    /// </summary>
    public class CanvasMouseArgs : EventArgs
    {
        /// <summary>Toạ độ chuột trong control (screen / client space của ImageBox).</summary>
        public Point Screen { get; }

        /// <summary>Toạ độ chuột đã quy đổi về image space (px ảnh gốc).</summary>
        public PointF Image { get; }

        /// <summary>Nút chuột vừa kích hoạt sự kiện.</summary>
        public MouseButtons Button { get; }

        /// <summary>Số click (1 = single, 2 = double).</summary>
        public int Clicks { get; }

        /// <summary>Delta wheel (chỉ có giá trị với MouseWheel).</summary>
        public int Delta { get; }

        /// <summary>Cho consumer chặn xử lý mặc định của ImageBox (vd: pan/zoom).</summary>
        public bool Handled { get; set; }

        public CanvasMouseArgs(Point screen, PointF image, MouseButtons button, int clicks, int delta)
        {
            Screen = screen;
            Image = image;
            Button = button;
            Clicks = clicks;
            Delta = delta;
        }
    }
}
