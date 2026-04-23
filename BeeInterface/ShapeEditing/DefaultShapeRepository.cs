using System.Collections.Generic;
using BeeCore;
using OpenCvSharp;

namespace BeeInterface.ShapeEditing
{
    /// <summary>
    /// Default implementation của IShapeRepository. Wrap quanh BeeCore.Common.*
    /// để giữ tương thích ngược với code hiện tại, nhưng các UC mới (UC1/UC2)
    /// chỉ thấy interface — không đụng trực tiếp Common/Global.
    ///
    /// Khi nào muốn thay sang nguồn data khác (mock test, file, DB…) chỉ cần
    /// inject implementation khác, không đụng UC.
    /// </summary>
    public class DefaultShapeRepository : IShapeRepository
    {
        /// <summary>Singleton mặc định để host (View.cs) đang migrate có thể gán thẳng.</summary>
        public static readonly DefaultShapeRepository Instance = new DefaultShapeRepository();

        public IList<PropetyTool> GetPropetyTools(int indexProg)
        {
            if (indexProg < 0 || indexProg >= BeeCore.Common.PropetyTools.Count)
                return null;
            return BeeCore.Common.EnsureToolList(indexProg);
        }

        public PropetyTool GetTool(int indexProg, int indexTool)
        {
            var list = GetPropetyTools(indexProg);
            if (list == null) return null;
            if (indexTool < 0 || indexTool >= list.Count) return null;
            return list[indexTool];
        }

        public Camera GetCamera(int indexCamera)
        {
            if (indexCamera < 0 || indexCamera >= BeeCore.Common.listCamera.Count)
                return null;
            return BeeCore.Common.listCamera[indexCamera];
        }

        public Mat GetRawMat(int indexCamera)
        {
            var cam = GetCamera(indexCamera);
            return cam?.matRaw;
        }

        public bool GetCameraMouseDown(int indexCamera)
        {
            var cam = GetCamera(indexCamera);
            if (cam == null) return false;

            var field = cam.GetType().GetField("IsMouseDown");
            if (field != null && field.FieldType == typeof(bool))
                return (bool)field.GetValue(cam);

            var property = cam.GetType().GetProperty("IsMouseDown");
            if (property != null && property.PropertyType == typeof(bool))
                return (bool)property.GetValue(cam, null);

            return false;
        }

        public void SetCameraMouseDown(int indexCamera, bool value)
        {
            var cam = GetCamera(indexCamera);
            if (cam == null) return;

            var field = cam.GetType().GetField("IsMouseDown");
            if (field != null && field.FieldType == typeof(bool))
            {
                field.SetValue(cam, value);
                return;
            }

            var property = cam.GetType().GetProperty("IsMouseDown");
            if (property != null && property.PropertyType == typeof(bool) && property.CanWrite)
                property.SetValue(cam, value, null);
        }
    }
}
