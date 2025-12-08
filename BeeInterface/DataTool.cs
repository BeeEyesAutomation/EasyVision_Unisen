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
        public static void LoadProject(String NameProject)
        {
            NameProject = NameProject.Replace(".prog", "");
            Global.ParaCommon = LoadData.Para(NameProject);
            List<ParaCamera> paraCameras = LoadData.ParaCamera(NameProject);
            if (paraCameras.Count() > 0)
            {
                Global.listParaCamera = paraCameras;
                BeeCore.Common.listCamera = new List<Camera>();
                int indexCCD = 0;
                foreach (ParaCamera paraCamera in paraCameras)
                {
                    if (paraCamera != null)
                        BeeCore.Common.listCamera.Add(new Camera(paraCamera, indexCCD));
                    else
                        BeeCore.Common.listCamera.Add(null);
                  
                    indexCCD++;
                }
            X: if (BeeCore.Common.listCamera.Count() != 4)
                {
                    BeeCore.Common.listCamera.Add(null);
                    goto X;
                }
                if (BeeCore.Common.listCamera.Count() > 0)
                    if (BeeCore.Common.listCamera[0] != null)
                        Global.Config.SizeCCD = BeeCore.Common.listCamera[0].GetSzCCD();
            }
            else
            {
                BeeCore.Common.listCamera = new List<Camera>();
                Global.listParaCamera = new List<ParaCamera> { null,null,null,null};
                foreach (ParaCamera paraCamera in Global.listParaCamera)
                {
                    if (paraCamera== null)
                       
                        BeeCore.Common.listCamera.Add(null);
                    
                   
                }
            }
                //Global.Config.SizeCCD = Camera.GetSzCCD();
                BeeCore.Common.PropetyTools = LoadData.Project(NameProject);
           

            if (BeeCore.Common.PropetyTools.Count == 0)
            {
                BeeCore.Common.PropetyTools = new List<List<PropetyTool>> { new List<PropetyTool>(), new List<PropetyTool>(), new List<PropetyTool>(), new List<PropetyTool>() };
                //  G.listAlltool = new List<List<Tools>> { new List<Tools>(), new List<Tools>(), new List<Tools>(), new List<Tools>() };

            }

            for (int i = 0; i < BeeCore.Common.PropetyTools.Count; i++)
            {   if(BeeCore.Common.PropetyTools[i]!=null)
                    for (int j = 0; j< BeeCore.Common.PropetyTools[i].Count; j++)
                    {
                        TypeTool TypeTool= BeeCore.Common.PropetyTools[i][j].TypeTool;
                        int ix = Global.itemNews.FindIndex(a => a.TypeTool == TypeTool);
                        if (ix >= 0)
                            if (!Global.itemNews[ix].IsEn)
                            {
                                BeeCore.Common.PropetyTools[i].RemoveAt(j);
                                j--;
                            }
                          
                    } 
            }    
            //else
            //    G.listAlltool = new List<List<Tools>>();


            //if (Global.ToolSettings == null)
            //{
            //    Global.ToolSettings = new ToolSettings();

            //}
            Global.pShowTool.Y = 10; Global.pShowTool.X = 5;
            int indexThread = 0;

            foreach (List<BeeCore.PropetyTool> ListTool in BeeCore.Common.PropetyTools)
            {
                if (ListTool != null)
                {
                    int i = 0;
                    foreach (PropetyTool PropTool in ListTool)
                    {
                        try
                        {
                            dynamic control = DataTool.CreateControls(PropTool, i, indexThread, new Point(Global.pShowTool.X, Global.pShowTool.Y));
                            ItemTool Itemtool = DataTool.CreateItemTool(PropTool, i, indexThread, new Point(Global.pShowTool.X, Global.pShowTool.Y));

                            Global.pShowTool.Y += Itemtool.Height + 10;
                            PropTool.ItemTool = Itemtool;
                            PropTool.Propety.IndexThread = indexThread;
                            PropTool.Control = control;
                            DataTool.LoadPropety(PropTool.Control);
                        }
                        catch (Exception ex)
                        {
                            String s = ex.Message;
                        }

                        i++;
                    }

                }

                if (Global.Config.IsMultiCamera == false)
                    break;

                indexThread++;

            }
            //foreach (List<Tools> ListTool in G.listAlltool)
            //{
            //    if (ListTool == null) continue;
            //    foreach (Tools Tool in ListTool)
            //    DataTool.LoadPropety(Tool.tool);


            //}
            //foreach (Tools tool in G.listAlltool)
            //{
            //    DataTool.LoadPropety(tool.tool);
            //   //X: if(tool.tool.Propety.StatusTool != StatusTool.Initialed)
            //   // {
            //   //     Task.Delay(10);
            //   //     goto X;
            //   // }
            //   //else
            //   // {
            //   //     continue;
            //   // }
            //}
            foreach (List<PropetyTool> ListTool in BeeCore.Common.PropetyTools)
            {

                Parallel.ForEach(ListTool, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, propety =>
                {
                    if (propety != null)
                        if (propety.Propety != null)
                            propety.Propety.SetModel();
                });
                if (Global.Config.IsMultiCamera == false)
                    break;
            }

            Global.IsLoadProgFist = true;

        }
        public static dynamic New(TypeTool typeTool)
        {
            dynamic control = null;
            switch (typeTool)
            {
               
                case TypeTool.Color_Area:
                    control = new ToolColorArea();
                    break;
                case TypeTool.Pattern:
                    control = new ToolPattern();
                    break;
                case TypeTool.Position_Adjustment:
                    control = new ToolPosition_Adjustment();
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
               
                case TypeTool.Measure:
                    control = new ToolMeasure();
                    break;
                case TypeTool.Circle:
                    control = new ToolCircle();
                    break;
                case TypeTool.Width:
                    control = new ToolWidth();
                    break;
                case TypeTool.OKNG:
                    control = new ToolOKNG();
                    break;
                case TypeTool.Crop:
                    control = new ToolCrop();
                    break;
                case TypeTool.Corner:
                    control = new ToolCorner();
                    break;
                case TypeTool.VisualMatch:
                    control = new ToolVisualMatch();
                    break;
                case TypeTool.Pitch:
                    control = new ToolPitch();
                    break;
                case TypeTool.EdgePixel:
                    control = new ToolEdgePixel();
                    break;
                case TypeTool.Edge:
                    control = new ToolEdge();
                    break;
                case TypeTool.CraftOCR:
                    control = new ToolCraftOCR();
                    break;
                case TypeTool.Intersect:
                    control = new ToolIntersect();
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
            System.Drawing.Size szImg = Global.Config.SizeCCD;
            switch (TypeCrop)
            {
                case TypeCrop.Crop:
                 return  new RectRotate(new RectangleF(-with / 2, -height / 2, with, height), new PointF(szImg.Width / 2, szImg.Height / 2), 0, AnchorPoint.None);
                    break;
                case TypeCrop.Area:
                 return   new RectRotate(new RectangleF(-szImg.Width / 2 + szImg.Width / 10, -szImg.Height / 2 + szImg.Width / 10, szImg.Width - szImg.Width / 5, szImg.Height - szImg.Width / 5), new PointF(szImg.Width / 2, szImg.Height / 2), 0, AnchorPoint.None);
                    break;
                case TypeCrop.Mask:
                    return new RectRotate(new RectangleF(-with / 2, -height / 2, with, height), new PointF(szImg.Width / 2, szImg.Height / 2), 0, AnchorPoint.None);
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
                itemTool = new ItemTool(TypeTool, TypeTool.ToString() + Convert.ToString(Index - 1),TriggerNum.Trigger1);
                itemTool.Location = pDraw;
                itemTool.CT = 0;
                itemTool.Score = "---";
                itemTool.Status = "---";
                if (TypeTool == TypeTool.OKNG)
                    itemTool.NotChange = true;
                // itemTool.Score.Value = Convert.ToInt32((double)control.Propety.Score);
                itemTool.ClScore = Color.Gray;
                itemTool.ClStatus = Color.Gray;
                itemTool.IndexTool = Index;
                itemTool.IndexThread = IndexThread;
                PropetyTool.Propety.Index = Index;
                itemTool.IconTool = Global.itemNews[Global.itemNews.FindIndex(a => a.TypeTool == TypeTool)].Icon;// (Image)Properties.Resources.ResourceManager.GetObject(TypeTool.ToString());
                itemTool.Anchor= AnchorStyles.Top|AnchorStyles.Left|AnchorStyles.Right;
              
              
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
                System.Drawing.Size szImg = Global.Config.SizeCCD;
              
              


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
                System.Drawing.Size szImg = Global.Config.SizeCCD;
               
               // BeeCore.Common.CreateTemp(TypeTool, IndexThread);
                if (PropetyTool.Name == null) PropetyTool.Name = "";
                control.Name = PropetyTool.Name;
                PropetyTool.worker = new System.ComponentModel.BackgroundWorker();
                PropetyTool.timer = new System.Diagnostics.Stopwatch();
                PropetyTool.worker.DoWork += (sender, e) =>
                {
                    PropetyTool.DoWork();
                };
                PropetyTool.worker.RunWorkerCompleted += (sender, e) =>
                {
                    PropetyTool.Complete();
                };
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
