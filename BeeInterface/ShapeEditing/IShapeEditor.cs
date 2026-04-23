using System;
using BeeGlobal;

namespace BeeInterface.ShapeEditing
{
    /// <summary>
    /// Hợp đồng của UserControl cấp cao (ShapeEditorControl).
    /// Compose IImageCanvas + thêm logic edit shape (hit-test, drag, create…).
    ///
    /// UC2 implement interface này. View.cs (host) sẽ gọi qua interface,
    /// KHÔNG đụng vào field nội bộ của UC2 và KHÔNG phụ thuộc class concrete.
    /// </summary>
    public interface IShapeEditor
    {
        // ----- State injection -----

        /// <summary>State đang edit. Set vào để bắt đầu phiên edit, set null để cancel.</summary>
        ShapeEditState State { get; set; }

        /// <summary>Tuỳ chọn render/handle. Có thể đổi runtime, editor sẽ tự refresh.</summary>
        ShapeEditOptions Options { get; set; }

        /// <summary>Repository cấp dữ liệu (PropetyTool, Camera, Mat raw…).</summary>
        IShapeRepository Repository { get; set; }

        /// <summary>Canvas mà editor đang làm việc lên.</summary>
        IImageCanvas Canvas { get; }

        // ----- Commands -----

        /// <summary>Bắt đầu phiên edit cho 1 RectRotate cụ thể (Area/Mask/Crop…).</summary>
        void BeginEdit(RectRotate target, TypeCrop cropType);

        /// <summary>Huỷ phiên edit (revert preview, không emit Created/Moved).</summary>
        void Cancel();

        /// <summary>Commit phiên edit hiện tại (emit ShapeChanged kind tương ứng).</summary>
        void Commit();

        /// <summary>Xoá shape đang được chọn.</summary>
        void DeleteCurrent();

        // ----- Events -----

        /// <summary>Khi shape thay đổi (đủ kind, kể cả Preview).</summary>
        event EventHandler<ShapeChangedArgs> ShapeChanged;

        /// <summary>Khi InteractionMode đổi (Idle ↔ Dragging…). Host dùng để bật/tắt UI khác.</summary>
        event EventHandler<InteractionMode> InteractionChanged;

        /// <summary>Khi pick xong 1 màu (chỉ phát khi ColorPickerOverlay đang active).</summary>
        event EventHandler<ColorPickedArgs> ColorPicked;
    }

    /// <summary>Event args khi ColorPicker pick xong 1 màu.</summary>
    public class ColorPickedArgs : EventArgs
    {
        /// <summary>Màu pick được.</summary>
        public System.Drawing.Color Color { get; }

        /// <summary>Không gian màu được dùng (HSV / RGB).</summary>
        public ColorGp Mode { get; }

        /// <summary>Toạ độ pixel trong image gốc.</summary>
        public System.Drawing.Point ImagePoint { get; }

        public ColorPickedArgs(System.Drawing.Color color, ColorGp mode, System.Drawing.Point imagePoint)
        {
            Color = color;
            Mode = mode;
            ImagePoint = imagePoint;
        }
    }
}
