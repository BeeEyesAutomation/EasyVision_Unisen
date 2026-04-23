using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace BeeInterface.ShapeEditing
{
    /// <summary>
    /// Hợp đồng của UserControl cấp thấp (ImageCanvasControl).
    /// Trách nhiệm DUY NHẤT: hiển thị ảnh + zoom/pan + transform toạ độ.
    /// KHÔNG biết gì về RectRotate, TypeCrop, ColorPicker — đó là việc của
    /// IShapeEditor ở lớp trên (UC2).
    ///
    /// Quy tắc: UC1 KHÔNG được using BeeGlobal. UC1 KHÔNG được đăng ký event
    /// lên object bên ngoài — nó chỉ expose event và cho consumer đăng ký vào.
    /// </summary>
    public interface IImageCanvas
    {
        // ----- Image & zoom -----

        /// <summary>Gán ảnh hiển thị. Truyền null để clear.</summary>
        void SetImage(Mat image);

        /// <summary>Zoom hiện tại (0..1 = 0..100%). Giống Cyotek.ImageBox.Zoom/100.</summary>
        float Zoom { get; }

        /// <summary>Vị trí scroll hiện tại (pixel trong client space).</summary>
        Point ScrollPosition { get; }

        /// <summary>Kích thước vùng hiển thị (client area).</summary>
        Size ViewportSize { get; }

        // ----- Coordinate transform -----

        /// <summary>Quy đổi toạ độ client (chuột trong control) về image space.</summary>
        PointF ScreenToImage(Point clientPoint);

        /// <summary>Quy đổi toạ độ image space về client (dùng khi cần vẽ overlay ở screen space).</summary>
        Point ImageToScreen(PointF imagePoint);

        // ----- Overlay -----

        /// <summary>Đăng ký 1 painter. Các painter sẽ được gọi trong Paint event theo ZOrder tăng dần.</summary>
        void AddOverlay(IOverlayPainter painter);

        /// <summary>Gỡ painter đã đăng ký.</summary>
        void RemoveOverlay(IOverlayPainter painter);

        /// <summary>Yêu cầu vẽ lại (tương đương Invalidate()).</summary>
        void RequestRedraw();

        // ----- Cursor & interaction -----

        /// <summary>Set cursor của canvas. Dùng cho ColorPicker đổi cursor thành Dropper.</summary>
        void SetCursor(Cursor cursor);

        /// <summary>Bật/tắt Pan bằng chuột giữa của ImageBox. ShapeEditor cần tắt khi đang drag shape.</summary>
        void SetPanEnabled(bool enabled);

        // ----- Events (consumer đăng ký vào) -----

        event EventHandler<CanvasMouseArgs> CanvasMouseDown;
        event EventHandler<CanvasMouseArgs> CanvasMouseMove;
        event EventHandler<CanvasMouseArgs> CanvasMouseUp;
        event EventHandler<CanvasMouseArgs> CanvasMouseWheel;
        event EventHandler ZoomChanged;
    }
}
