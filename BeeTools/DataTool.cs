using BeeCore;
using BeeGlobal;
using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;

namespace BeeInterface
{
    public class DataTool
    {
        public static dynamic New(TypeTool typeTool)
        {
            dynamic control = null;
            switch (typeTool)
            {
                case TypeTool.OutLine:
                    control = new ToolOutLine();
                    break;
                case TypeTool.Color_Area:
                    control = new ToolColorArea();
                    break;
                case TypeTool.Pattern:
                    control = new ToolPattern();
                    break;
                case TypeTool.Position_Adjustment:
                    control = new ToolPosition_Adjustment();
                    break;
                case TypeTool.Edge_Pixels:
                    control = new ToolEdgePixels();
                    break;
                case TypeTool.MatchingShape:
                    control = new ToolMatchingShape();
                    break;
                case TypeTool.Learning:
                    control = new ToolYolo();
                    break;
                case TypeTool.OCR:
                    control = new ToolOCR();
                    break;
                case TypeTool.BarCode:
                    control = new ToolBarcode();
                    break;
                case TypeTool.Positions:
                    control = new ToolPositions();
                    break;
                case TypeTool.Measure:
                    control = new ToolMeasure();
                    break;
                case TypeTool.Circle:
                    control = new ToolCircle();
                    break;
                case TypeTool.Width:
                    control = new ToolWidth();
                    break;
                default:
                    return null;
                    break;
            }
            return control;
        }
        public static RectRotate NewRotRect(TypeCrop TypeCrop)
        {
            int with = 50, height = 50;
            System.Drawing.Size szImg = Global.ParaCommon.SizeCCD;
            switch (TypeCrop)
            {
                case TypeCrop.Crop:
                 return  new RectRotate(new RectangleF(-with / 2, -height / 2, with, height), new PointF(szImg.Width / 2, szImg.Height / 2), 0, AnchorPoint.None, false);
                    break;
                case TypeCrop.Area:
                 return   new RectRotate(new RectangleF(-szImg.Width / 2 + szImg.Width / 10, -szImg.Height / 2 + szImg.Width / 10, szImg.Width - szImg.Width / 5, szImg.Height - szImg.Width / 5), new PointF(szImg.Width / 2, szImg.Height / 2), 0, AnchorPoint.None, false);
                    break;
                case TypeCrop.Mask:
                    return new RectRotate(new RectangleF(-with / 2, -height / 2, with, height), new PointF(szImg.Width / 2, szImg.Height / 2), 0, AnchorPoint.None, false);
                    break;

            }
            return new RectRotate();
        }
        public static dynamic CreateItemTool(BeeCore.PropetyTool PropetyTool, int Index, int IndexThread, Point pDraw)
        {


            ItemTool itemTool = null;
            TypeTool TypeTool = PropetyTool.TypeTool;
            try
            {
                itemTool = new ItemTool(TypeTool, TypeTool.ToString() + Convert.ToString(Index - 1));
                itemTool.Location = pDraw;
                itemTool.CT = 0;
                itemTool.Score = "---";
                itemTool.Status = "---";
                // itemTool.Score.Value = Convert.ToInt32((double)control.Propety.Score);
                itemTool.ClScore = Color.Gray;
                itemTool.ClStatus = Color.Gray;
                itemTool.IndexTool = Index;
                itemTool.IconTool = (Image)Properties.Resources.ResourceManager.GetObject(TypeTool.ToString());

               
                if (PropetyTool.Name == null) PropetyTool.Name = "";
                if (PropetyTool.Name.Trim() == "")
                    itemTool.Name = TypeTool.ToString() + " " + Index;
                else
                    itemTool.Name = PropetyTool.Name.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                BeeCore.Common.PropetyTools[Global.IndexChoose].Remove(PropetyTool);
                return null;
            }
            return itemTool;
        }
        public static dynamic NewControl(TypeTool TypeTool, int Index, int IndexThread,String Nametool, Point pDraw)
        {



         
            dynamic control = New(TypeTool);

            try
            {
                if (control == null) return null;
                int with = 50, height = 50;
               
                control.Propety.Index = Index;
                System.Drawing.Size szImg = Global.ParaCommon.SizeCCD;
                if (control.Propety.rotCrop == null)
                    if (TypeTool != TypeTool.Edge_Pixels &&
                       TypeTool != TypeTool.BarCode &&
                      TypeTool != TypeTool.Learning &&
                      TypeTool != TypeTool.OCR &&
                      TypeTool != TypeTool.Circle &&
                      TypeTool != TypeTool.Width &&
                      TypeTool != TypeTool.Color_Area && TypeTool != TypeTool.MatchingShape && TypeTool != TypeTool.Measure)
                        control.Propety.rotCrop = new RectRotate(new RectangleF(-with / 2, -height / 2, with, height), new PointF(szImg.Width / 2, szImg.Height / 2), 0, AnchorPoint.None, false);
                    else
                        control.Propety.rotCrop = null;
                if (control.Propety.rotArea == null)
                    control.Propety.rotArea = new RectRotate(new RectangleF(-szImg.Width / 2 + szImg.Width / 10, -szImg.Height / 2 + szImg.Width / 10, szImg.Width - szImg.Width / 5, szImg.Height - szImg.Width / 5), new PointF(szImg.Width / 2, szImg.Height / 2), 0, AnchorPoint.None, false);
                if (control.Propety.rotMask == null)
                    control.Propety.rotMask = new RectRotate(new RectangleF(-with / 2, -height / 2, with, height), new PointF(szImg.Width / 2, szImg.Height / 2), 0, AnchorPoint.None, false);

                control.Propety.SetModel();


                control.Name =Nametool;
             
                //tools = new Tools(itemTool, control);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
              
                return null;
            }
            return control;
        }

        public static  dynamic CreateControls(BeeCore.PropetyTool PropetyTool,int Index,int IndexThread,Point pDraw)
        {
           
            
                
                TypeTool TypeTool = PropetyTool.TypeTool;
                dynamic control = New(TypeTool);
               
            try
            {
                if (control == null) return null;
                int with = 50, height = 50;
                control.Propety = PropetyTool.Propety;
                control.Propety.Index = Index;
                System.Drawing.Size szImg = Global.ParaCommon.SizeCCD;
                if(PropetyTool.Propety.rotCrop==null)
                if (TypeTool != TypeTool.Edge_Pixels &&
                   TypeTool != TypeTool.BarCode &&
                  TypeTool != TypeTool.Learning &&
                  TypeTool != TypeTool.OCR &&
                  TypeTool != TypeTool.Circle &&
                  TypeTool != TypeTool.Width &&
                  TypeTool != TypeTool.Color_Area && TypeTool != TypeTool.MatchingShape && TypeTool != TypeTool.Measure)
                    control.Propety.rotCrop = new RectRotate(new RectangleF(-with / 2, -height / 2, with, height), new PointF(szImg.Width / 2, szImg.Height / 2), 0, AnchorPoint.None,false);
                else
                    control.Propety.rotCrop = null;
                if (PropetyTool.Propety.rotArea == null)
                    control.Propety.rotArea = new RectRotate(new RectangleF(-szImg.Width / 2 + szImg.Width / 10, -szImg.Height / 2 + szImg.Width / 10, szImg.Width - szImg.Width / 5, szImg.Height - szImg.Width / 5), new PointF(szImg.Width / 2, szImg.Height / 2), 0, AnchorPoint.None, false);
                if (PropetyTool.Propety.rotMask== null)
                    control.Propety.rotMask = new RectRotate(new RectangleF(-with / 2, -height / 2, with, height), new PointF(szImg.Width / 2, szImg.Height / 2), 0, AnchorPoint.None, false);
              
               // BeeCore.Common.CreateTemp(TypeTool, IndexThread);
                if (PropetyTool.Name == null) PropetyTool.Name = "";
                control.Name = PropetyTool.Name;
                PropetyTool.worker = new System.ComponentModel.BackgroundWorker();
                PropetyTool.timer = new System.Diagnostics.Stopwatch();
                PropetyTool.worker.DoWork += (sender, e) =>
                {
                    PropetyTool.StatusTool = StatusTool.Processing;
                    PropetyTool.timer.Restart();
                    if (!Global.IsRun)
                        PropetyTool. Propety.rotAreaAdjustment = PropetyTool.Propety.rotArea;
                    PropetyTool.Propety.DoWork(PropetyTool.Propety.rotAreaAdjustment);
                };
                PropetyTool.worker.RunWorkerCompleted += (sender, e) =>
                {
                    PropetyTool.Propety.Complete();
                    if (!Global.IsRun)
                        Global.StatusDraw = StatusDraw.Check;
                    PropetyTool.StatusTool = StatusTool.Done;
                    PropetyTool.timer.Stop();
                    PropetyTool.CycleTime = (int)PropetyTool.timer.Elapsed.TotalMilliseconds;
                };
                //tools = new Tools(itemTool, control);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
               BeeCore.Common.PropetyTools[Global.IndexChoose].Remove(PropetyTool);
                return null;
            }
            return control;
        }
        public static bool LoadModel(dynamic control)
        {
            try
            {
                if (!control.workLoadModel.IsBusy)
                {
                    control.workLoadModel.RunWorkerAsync();
                    
                }
            }
            catch { }
            {
                return false;
            }
            return true;
        }
        public static bool LoadPropety(dynamic control)
        {
            try
            {
               
                    control.LoadPara();
              
            }
            catch(Exception ex) 
            {
               
                String exs = ex.Message;
                return false;
            }
            return true;
        }
    }
}
