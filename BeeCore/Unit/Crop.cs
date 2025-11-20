
using BeeCore.Func;
using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Point = System.Drawing.Point;

namespace BeeCore
{
    [Serializable()]
    public class Crop
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public int IndexThread;
        public void SetModel()
        {
            rotMask = null;
            rotCrop = null;
           
            Common.PropetyTools[IndexThread][Index].StepValue = 0.1f;
            Common.PropetyTools[IndexThread][Index].MinValue = 0;
            Common.PropetyTools[IndexThread][Index].MaxValue = 45;
            Common.PropetyTools[IndexThread][Index].StatusTool = StatusTool.WaitCheck;
          
        }
        public float Scale = 1;
      
      
        public Crop() { }
      
        public String nameTool = "";
        public int Index = 0;
        public String PathSaveImage = "";
        
       
      
   
        public RectRotate rotArea, rotCrop, rotMask;
        public RectRotate rotAreaTemp = new RectRotate();
        public RectRotate rotAreaAdjustment;
        public RectRotate rotPositionAdjustment;
       
        public TypeCrop TypeCrop;
      [NonSerialized]
      private bool IsDone1=false,  IsDone2 = false,  IsDone3 = false,  IsDone4 = false;
        public void DoWork(RectRotate rectRotate)
        {
            if (PathSaveImage != "")
            {

                try
                {
                    if (matProcess!=null)
                        if (!matProcess.IsDisposed)
                        if (!matProcess.IsDisposed)
                        if (!matProcess.Empty())
                            matProcess.Release();
                    if (rectRotate != null)
                    {
                        matProcess = Cropper.CropRotatedRect(BeeCore.Common.listCamera[IndexThread].matRaw, rectRotate, null);
                        String path = PathSaveImage + "\\" + Global.Project + "\\" + Common.PropetyTools[Global.IndexChoose][Index].Name + "_" + DateTime.Now.ToString("yyyyMMdd_HH_mm_ss") + ".png";
                        string dir = PathSaveImage + "\\" + Global.Project;
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                        Cv2.ImWrite(path, matProcess);
                        Common.PropetyTools[Global.IndexChoose][Index].Results = Results.OK;
                    }
                    else
                    {
                        Common.PropetyTools[Global.IndexChoose][Index].Results = Results.NG;
                    }    
                }
                catch(Exception e)
                {
                    String ex = e.Message;
                }
            }
          
        
        }
        [NonSerialized]
        private   PointF pCenter1, pCenter2, pCenter3, pCenter4, pIntersection;
        public async Task SendResult()
        {
           
        }
        public void Complete()
        {
           
           
        }
        [NonSerialized]
        public Mat matProcess = new Mat();

        public LineOrientation LineOrientation = LineOrientation.Vertical;
        [NonSerialized]
        public GapResult GapResult = new GapResult();
        public Graphics DrawResult(Graphics gc)
        {

            if (rotAreaAdjustment == null && Global.IsRun) return gc;

            gc.ResetTransform();
            RectRotate rotA = rotArea;
            if (Global.IsRun) rotA = rotAreaAdjustment;

            var mat = new Matrix();
            if (!Global.IsRun)
            {
                mat.Translate(Global.pScroll.X, Global.pScroll.Y);
                mat.Scale(Global.ScaleZoom, Global.ScaleZoom);
            }
            mat.Translate(rotA._PosCenter.X, rotA._PosCenter.Y);
            mat.Rotate(rotA._rectRotation);
            gc.Transform = mat;
            Brush brushText = Brushes.White;
            Color cl = Color.LimeGreen;
            switch (Common.PropetyTools[Global.IndexChoose][Index].Results)
            {
                case Results.OK:
                    cl =  Global.Config.ColorOK;
                    break;
                case Results.NG:
                    cl = Global.Config.ColorNG;
                    break;
            }
            String nameTool = (int)(Index + 1) + "." + Common.PropetyTools[Global.IndexChoose][Index].Name;
            Font font = new Font("Arial", Global.Config.FontSize, FontStyle.Bold);
            if (Global.Config.IsShowBox)
                Draws.Box1Label(gc, rotA, nameTool, font, brushText, cl,  Global.Config.ThicknessLine);







            return gc;
        }

    }
}
