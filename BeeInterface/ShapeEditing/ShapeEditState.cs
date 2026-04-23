using System.Drawing;
using BeeGlobal;

namespace BeeInterface.ShapeEditing
{
    /// <summary>
    /// State POCO của ShapeEditor — gom toàn bộ thứ đang nằm trong Global.* mà
    /// các handler MouseDown/MouseMove/MouseUp/Paint trong View.cs đang đụng đến.
    /// Class này KHÔNG phụ thuộc vào WinForms control nào và KHÔNG đụng Global.*.
    /// Bước 1: chỉ là khung dữ liệu. Logic sẽ được di chuyển vào ở Bước 5.
    /// </summary>
    public class ShapeEditState
    {
        // ----- Selection -----
        /// <summary>Tool đang được chọn (tương ứng Global.IndexToolSelected).</summary>
        public int IndexTool { get; set; } = -1;

        /// <summary>Program đang được chọn (tương ứng Global.IndexProgChoose).</summary>
        public int IndexProg { get; set; } = 0;

        /// <summary>Camera/CCD đang xem (tương ứng Global.IndexCCCD).</summary>
        public int IndexCamera { get; set; } = 0;

        // ----- Mode flags -----
        /// <summary>Cờ máy đang chạy chu trình check (tương ứng Global.IsRun).
        /// Khi true thì editor không cho thay đổi shape.</summary>
        public bool IsRun { get; set; } = false;

        /// <summary>Trạng thái vẽ chính (Edit / Scan / Check / Color…).</summary>
        public StatusDraw Status { get; set; } = StatusDraw.None;

        /// <summary>Trạng thái tương tác chuột hiện tại của editor.</summary>
        public InteractionMode Interaction { get; set; } = InteractionMode.Idle;

        // ----- Current shape being edited -----
        /// <summary>RectRotate đang được edit (tương ứng Global.rotCurrent).</summary>
        public RectRotate Current { get; set; }

        /// <summary>Loại crop của shape đang edit (Area / Mask / Crop / Limit).</summary>
        public TypeCrop CropType { get; set; } = TypeCrop.Area;

        // ----- Color picker -----
        /// <summary>Bật chế độ pick màu (tương ứng Global.IsGetColor).</summary>
        public bool IsGetColor { get; set; } = false;

        /// <summary>Đã pick xong màu (tương ứng Global.IsSetColor).</summary>
        public bool IsSetColor { get; set; } = false;

        /// <summary>Loại không gian màu đang pick (HSV / RGB).</summary>
        public ColorGp ColorMode { get; set; } = ColorGp.RGB;

        /// <summary>Màu đã pick được (tương ứng Global.ColorSample).</summary>
        public Color ColorSample { get; set; } = Color.Empty;
    }
}
