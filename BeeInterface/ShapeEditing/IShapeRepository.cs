using System.Collections.Generic;
using BeeCore;
using BeeGlobal;
using OpenCvSharp;

namespace BeeInterface.ShapeEditing
{
    /// <summary>
    /// Interface để 2 UserControl mới (ImageCanvasControl, ShapeEditorControl)
    /// truy cập dữ liệu domain mà KHÔNG gọi trực tiếp BeeCore.Common.* hay Global.*.
    /// Implementation mặc định sẽ wrap quanh BeeCore.Common.* để giữ tương thích
    /// ngược, nhưng các UC chỉ thấy interface.
    /// </summary>
    public interface IShapeRepository
    {
        /// <summary>Lấy danh sách PropetyTool theo program index.</summary>
        IList<PropetyTool> GetPropetyTools(int indexProg);

        /// <summary>Lấy 1 PropetyTool theo program + tool index.</summary>
        PropetyTool GetTool(int indexProg, int indexTool);

        /// <summary>Lấy camera theo index.</summary>
        Camera GetCamera(int indexCamera);

        /// <summary>Lấy ảnh raw hiện tại của camera (không clone, gọi xong nên Clone() nếu cần dùng lâu).</summary>
        Mat GetRawMat(int indexCamera);

        /// <summary>Set/Get cờ MouseDown trên camera (đang được vài chỗ trong code đụng vào).</summary>
        bool GetCameraMouseDown(int indexCamera);
        void SetCameraMouseDown(int indexCamera, bool value);
    }
}
