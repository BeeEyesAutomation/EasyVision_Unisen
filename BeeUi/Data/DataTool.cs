using BeeCore;
using BeeUi.Commons;
using BeeUi.Tool;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;

namespace BeeUi.Data
{
    public class DataTool
    {
        public static dynamic New(BeeCore.TypeTool typeTool)
        {
            dynamic control = null;
            switch (typeTool)
            {
                case BeeCore.TypeTool.OutLine:
                    control = new ToolOutLine();
                    break;
                case BeeCore.TypeTool.Color_Area:
                    control = new ToolColorArea();
                    break;
                case BeeCore.TypeTool.Pattern:
                    control = new ToolPattern();
                    break;
                case BeeCore.TypeTool.Position_Adjustment:
                    control = new ToolPosition_Adjustment();
                    break;
                case BeeCore.TypeTool.Edge_Pixels:
                    control = new ToolEdgePixels();
                    break;
                case BeeCore.TypeTool.MatchingShape:
                    control = new ToolMatchingShape();
                    break;
                case BeeCore.TypeTool.Learning:
                    control = new ToolYolo();
                    break;
                case BeeCore.TypeTool.OCR:
                    control = new ToolOCR();
                    break;
                case BeeCore.TypeTool.BarCode:
                    control = new ToolBarcode();
                    break;
                case BeeCore.TypeTool.Positions:
                    control = new ToolPositions();
                    break;
                case BeeCore.TypeTool.Measure:
                    control = new ToolMeasure();
                    break;
                case BeeCore.TypeTool.Circle:
                    control = new ToolCircle();
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
            System.Drawing.Size szImg = BeeCore.G.ParaCam.SizeCCD;
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
        public static Tools SetPropety(BeeCore.PropetyTool PropetyTool,int Inndex,int IndexThread)
        {
            Tools tools = new Tools();
            try
            {
              
                Commons.ItemTool itemTool = null;
                BeeCore.TypeTool TypeTool = PropetyTool.TypeTool;
                dynamic control = New(TypeTool);
                if (control == null) return null;
                int with = 50, height = 50;
                control.Propety = PropetyTool.Propety;
                control.Propety.Index = Inndex;
                System.Drawing.Size szImg = BeeCore.G.ParaCam.SizeCCD;
                if(PropetyTool.Propety.rotCrop==null)
                if (TypeTool != TypeTool.Edge_Pixels &&
                   TypeTool != TypeTool.BarCode &&
                   TypeTool != TypeTool.Learning &&
                   TypeTool != TypeTool.OCR &&
                    TypeTool != TypeTool.Circle &&
                  TypeTool != TypeTool.Color_Area && TypeTool != TypeTool.MatchingShape && TypeTool != TypeTool.Measure)
                    control.Propety.rotCrop = new RectRotate(new RectangleF(-with / 2, -height / 2, with, height), new PointF(szImg.Width / 2, szImg.Height / 2), 0, AnchorPoint.None,false);
                else
                    control.Propety.rotCrop = null;
                if (PropetyTool.Propety.rotArea == null)
                    control.Propety.rotArea = new RectRotate(new RectangleF(-szImg.Width / 2 + szImg.Width / 10, -szImg.Height / 2 + szImg.Width / 10, szImg.Width - szImg.Width / 5, szImg.Height - szImg.Width / 5), new PointF(szImg.Width / 2, szImg.Height / 2), 0, AnchorPoint.None, false);
                if (PropetyTool.Propety.rotMask== null)
                    control.Propety.rotMask = new RectRotate(new RectangleF(-with / 2, -height / 2, with, height), new PointF(szImg.Width / 2, szImg.Height / 2), 0, AnchorPoint.None, false);
                itemTool = new Commons.ItemTool(TypeTool, TypeTool.ToString() + Convert.ToString(G.listAlltool[G.indexChoose].Count - 1));
                itemTool.Location = new Point(G.ToolSettings.X, G.ToolSettings.Y);
                itemTool.lbCycle.Text = "---";
                itemTool.lbScore.Text = "---";
                itemTool.lbStatus.Text = "---";
                itemTool.Score.Value = Convert.ToInt32((double)control.Propety.Score);
                itemTool.lbScore.ForeColor = Color.Gray;
                itemTool.lbStatus.BackColor = Color.Gray;
                G.ToolSettings.Y += itemTool.Height + 10;
                BeeCore.Common.CreateTemp(TypeTool, IndexThread);
                if (PropetyTool.Name == null) PropetyTool.Name = "";
                if (PropetyTool.Name.Trim() == "")
                    itemTool.name.Text = TypeTool.ToString() + " " + G.listAlltool[G.indexChoose].Count();
                else
                    itemTool.name.Text = PropetyTool.Name.Trim();
                control.Name = PropetyTool.Name;
                PropetyTool.Propety.nameTool = PropetyTool.Name;
                itemTool.lbNumber.Text = G.listAlltool[G.indexChoose].Count() + "";
                itemTool.icon.Image = (Image)Properties.Resources.ResourceManager.GetObject(TypeTool.ToString());
                tools = new Tools(itemTool, control);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                G.PropetyTools[G.indexChoose].Remove(PropetyTool);
                return null;
            }
            return tools;
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
