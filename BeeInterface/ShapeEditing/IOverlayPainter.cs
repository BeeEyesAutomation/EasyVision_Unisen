using System.Drawing;

namespace BeeInterface.ShapeEditing
{
    /// <summary>
    /// Hợp đồng cho mọi component muốn vẽ overlay lên ImageCanvasControl.
    /// Canvas sẽ gọi Paint() trong Paint event, theo thứ tự đăng ký.
    ///
    /// Trách nhiệm: chỉ vẽ. KHÔNG được sửa state, KHÔNG được trigger Invalidate
    /// chéo (sẽ gây stack overflow). Nếu cần redraw, gọi canvas.RequestRedraw().
    /// </summary>
    public interface IOverlayPainter
    {
        /// <summary>
        /// Thứ tự vẽ. Số nhỏ vẽ trước (nằm dưới), số lớn vẽ sau (nằm trên).
        /// Gợi ý: Grid = 0, Shapes = 100, Handles = 200, ColorPicker cursor = 300.
        /// </summary>
        int ZOrder { get; }

        /// <summary>True = đang bật, false = canvas sẽ skip không gọi Paint.</summary>
        bool Visible { get; }

        /// <summary>
        /// Vẽ overlay. Graphics đã được Cyotek ImageBox transform sẵn theo
        /// zoom/pan, nên consumer nên vẽ trong image space.
        /// </summary>
        void Paint(Graphics g, IImageCanvas canvas);
    }
}
