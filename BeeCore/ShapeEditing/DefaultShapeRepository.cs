using System.Collections.Generic;
using OpenCvSharp;

namespace BeeCore.ShapeEditing
{
    public sealed class DefaultShapeRepository : IShapeRepository
    {
        public static readonly DefaultShapeRepository Instance = new DefaultShapeRepository();

        private DefaultShapeRepository()
        {
        }

        public IList<PropetyTool> GetPropetyTools(int idxProg)
        {
            if (idxProg < 0 || idxProg >= BeeCore.Common.PropetyTools.Count)
                return null;

            return BeeCore.Common.EnsureToolList(idxProg);
        }

        public PropetyTool GetTool(int idxProg, int idxTool)
        {
            var tools = GetPropetyTools(idxProg);
            if (tools == null || idxTool < 0 || idxTool >= tools.Count)
                return null;

            return tools[idxTool];
        }

        public Camera GetCamera(int idx)
        {
            if (idx < 0 || idx >= BeeCore.Common.listCamera.Count)
                return null;

            return BeeCore.Common.listCamera[idx];
        }

        public Mat GetRawMat(int idxCamera)
        {
            var camera = GetCamera(idxCamera);
            return camera == null ? null : camera.matRaw;
        }
    }
}
