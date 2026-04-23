using System.Drawing;
using BeeGlobal;

namespace BeeInterface.ShapeEditing
{
    /// <summary>
    /// Tuỳ chọn hiển thị/render của ShapeEditor — thay cho việc đọc trực tiếp
    /// Global.ParaShow, Global.Config, Global.ScaleZoom, Global.pScroll trong Paint.
    /// Inject qua property thay vì lấy từ singleton.
    /// </summary>
    public class ShapeEditOptions
    {
        /// <summary>Tham số hiển thị (màu, font, độ dày nét…).</summary>
        public ParaShow ParaShow { get; set; }

        /// <summary>Config máy (SizeCCD, IsShowGrid…).</summary>
        public Config Config { get; set; }

        /// <summary>Scale zoom hiện tại (0..1), tương ứng Global.ScaleZoom.</summary>
        public float ScaleZoom { get; set; } = 1f;

        /// <summary>Vị trí scroll hiện tại, tương ứng Global.pScroll.</summary>
        public Point ScrollPosition { get; set; } = Point.Empty;

        /// <summary>Bán kính handle resize/rotate (px trong screen space).</summary>
        public float HandleRadius { get; set; } = 6f;

        /// <summary>Tolerance (px) để close polygon khi click gần điểm đầu.</summary>
        public float PolygonCloseTolerance { get; set; } = 8f;

        /// <summary>Ngưỡng (px) để chuyển từ MaybeCreate → CreatingNew.</summary>
        public float CreateDragThreshold { get; set; } = 3f;

        /// <summary>Snap góc khi xoay (degree). 0 = không snap.</summary>
        public float AngleSnapDegree { get; set; } = 0f;
    }
}
