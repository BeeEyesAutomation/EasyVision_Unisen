using System.Collections.Generic;
using OpenCvSharp;

namespace BeeCore.ShapeEditing
{
    public interface IShapeRepository
    {
        IList<PropetyTool> GetPropetyTools(int idxProg);
        PropetyTool GetTool(int idxProg, int idxTool);
        Camera GetCamera(int idx);
        Mat GetRawMat(int idxCamera);
    }
}
