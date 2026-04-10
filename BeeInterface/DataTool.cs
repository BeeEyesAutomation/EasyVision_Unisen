using BeeCore;
using BeeGlobal;
using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;

namespace BeeInterface
{
    public class DataTool
    {
        private static void SafeDispose(Control ctl)
        {
            if (ctl == null) return;

            try
            {
                if (ctl.InvokeRequired)
                {
                    ctl.BeginInvoke(new Action(() =>
                    {
                        if (ctl.Parent != null)
                            ctl.Parent.Controls.Remove(ctl);

                        if (!ctl.IsDisposed)
                            ctl.Dispose();
                    }));
                }
                else
                {
                    if (ctl.Parent != null)
                        ctl.Parent.Controls.Remove(ctl);

                    if (!ctl.IsDisposed)
                        ctl.Dispose();
                }
            }
            catch
            {
            }
        }
        public static void LoadProjectData(string NameProject)
        {
            try
            {if(Global.EditTool!=null)
                Global.EditTool.UnResgisTer();
                NameProject = NameProject.Replace(".prog", "");

                // ==== dispose tools cũ ====
                if (BeeCore.Common.PropetyTools != null)
                {
                    foreach (var list in BeeCore.Common.PropetyTools)
                        if (list != null)
                            foreach (var t in list)
                                t?.Dispose();
                }

                // ==== load config ====
                if (!Global.IsIntialProgram || !Global.Config.IsSaveProg)
                    Global.ParaCommon = LoadData.Para(NameProject);

                if (!Global.IsIntialProgram || !Global.Config.IsSaveCommunication)
                    Global.Comunication = LoadData.Comunication(NameProject);

                if (!Global.IsIntialProgram || !Global.Config.IsSaveParaShow)
                    Global.ParaShow = LoadData.ParaShow(NameProject);

                if (!Global.IsIntialProgram || !Global.Config.IsSaveListRegister)
                    Global.listRegsImg = LoadData.listImgRegister(NameProject);

                // ==== load camera ====
                if (!Global.IsIntialProgram || !Global.Config.IsSaveParaCam)
                {
                    try
                    {
                        var paraCameras = LoadData.ParaCamera(NameProject);
                        Global.listParaCamera = paraCameras;

                        BeeCore.Common.listCamera = new List<Camera>();
                        int index = 0;

                        foreach (var p in paraCameras)
                        {
                            if (p != null)
                                BeeCore.Common.listCamera.Add(new Camera(p, index));
                            else
                                BeeCore.Common.listCamera.Add(null);
                            index++;
                        }

                        while (BeeCore.Common.listCamera.Count < 4)
                            BeeCore.Common.listCamera.Add(null);

                        if (BeeCore.Common.listCamera[0] != null)
                            Global.Config.SizeCCD = BeeCore.Common.listCamera[0].GetSzCCD();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                // ==== load tools ====
                BeeCore.Common.PropetyTools = LoadData.Project(NameProject);

                if (BeeCore.Common.PropetyTools.Count == 0)
                {
                    BeeCore.Common.PropetyTools = new List<List<PropetyTool>>
            {
                new List<PropetyTool>(),
                new List<PropetyTool>(),
                new List<PropetyTool>(),
                new List<PropetyTool>()
            };
                }

                //if(BeeCore.Common.PropetyTools.Count >= 2)
                //{
                //    List<PropetyTool> newList = new List<PropetyTool>();
                //    foreach (PropetyTool List1 in BeeCore.Common.PropetyTools[1])
                //    {
                //        newList.Add((PropetyTool)List1.Clone());
                //    }
                //    BeeCore.Common.PropetyTools[2] = newList;


                //    List<PropetyTool> newList2 = new List<PropetyTool>();
                //    foreach (PropetyTool List2 in BeeCore.Common.PropetyTools[1])
                //    {
                //        newList2.Add((PropetyTool)List2.Clone());
                //    }
                //    BeeCore.Common.PropetyTools[3] = newList2;
                //}
                bool IsVerNew = false;
                foreach (List<PropetyTool> ListTool in BeeCore.Common.PropetyTools)
                {
                    if (ListTool == null) continue;

                    foreach (PropetyTool propety in ListTool)
                    {
                        if (propety.Propety2 == null && propety.Propety != null)
                        {
                            IsVerNew = true;

                        }
                    }
                }
                if (IsVerNew)
                {

                    String NameBK = Global.Project;

                    SaveData.Program(NameBK, BeeCore.Common.PropetyTools,true);
                }
                int indexThread = 0;
                foreach (List<PropetyTool> ListTool in BeeCore.Common.PropetyTools)
                {
                    if (ListTool == null)
                    {
                        indexThread ++;
                        continue;
                    }

                    foreach (PropetyTool propety in ListTool)
                    {
                        if(propety.Propety2 == null && propety.Propety!=null)
                        {
                            IsVerNew = true;
                        
                            propety.Propety2 = propety.Propety.Clone();
                            propety.Propety = null;
                        }


                        if (propety != null)
                            if (propety.Propety2 != null)
                            {
                                propety.Propety2.IndexThread = indexThread;
                                propety.Propety2.SetModel();
                            }    
                                

                    }
                    indexThread++;
                }
                if (IsVerNew)
                {
                 
                    SaveData.Program(Global.Project, BeeCore.Common.PropetyTools);
                    Global.IsLoadProgFist = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "LoadProg", ex.Message));
            }
        }
        public static void BuildProjectUI()
        {
            try { 
          
                Global.pShowTool.Y = 10;
                Global.pShowTool.X = 5;

                int indexThread = 0;

                for (int t = 0; t < BeeCore.Common.PropetyTools.Count; t++)
                {
                    //var list = BeeCore.Common.PropetyTools[t];
                  
                    if (BeeCore.Common.PropetyTools[t] == null) continue;

                   

                    for (int i = 0; i < BeeCore.Common.PropetyTools[t].Count; i++)
                    {
                        
                       
                        try
                        {
                        if (BeeCore.Common.PropetyTools[t][i] == null)
                        {
                          
                            continue;
                        }
                            BeeCore.Common.PropetyTools[t][i].ItemTool = CreateItemTool(BeeCore.Common.PropetyTools[t][i], i, indexThread);
                            if (BeeCore.Common.PropetyTools[t][i].TypeTool == TypeTool.Learning)
                            {
                                BeeCore.Common.PropetyTools[t][i].ItemTool.NotChange = true;
                            }
 
                            BeeCore.Common.PropetyTools[t][i].Control = CreateControls(BeeCore.Common.PropetyTools[t][i], i, indexThread);
                         
                        
                        }
                        catch(Exception ex) {
                            Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "BuildUI2", ex.Message));
                            Console.WriteLine(ex.Message);
                        }

                       
                    }

                    if (!Global.Config.IsMultiProg)
                        break;

                    indexThread++;
                }
            //try
            //{
            //    // ==== set model ====
            //    foreach (List<PropetyTool> ListTool in BeeCore.Common.PropetyTools)
            //    {
            //        if (ListTool == null) continue;

            //        foreach (PropetyTool propety in ListTool)
            //        {
                            
            //                try
            //            {
            //                if (propety != null)
            //                    if (propety.Propety2 != null)
            //                        propety.Propety2.SetModel();
            //            }
            //            catch(Exception ex)
            //            {
            //                Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "BuildUI", ex.Message));
            //                Console.WriteLine(ex.Message);
            //            }
            //        }
                 
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "BuildUI", ex.Message));
            //}
            }
            catch (Exception ex)
            {
                Global.LogsDashboard?.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "BuildUI 3", ex.Message));
            }
        }


   
        public static dynamic New(TypeTool typeTool,bool IsNew=false)
        {
            dynamic control = null;
            switch (typeTool)
            {
               
                case TypeTool.Color_Area:
                    control = new ToolColorArea();
                    control.Propety = new ColorArea();
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
                    control = new ToolOCR(IsNew);
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
                case TypeTool.MultiPattern:
                    control = new ToolMultiPattern();
                    break;
                case TypeTool.AutoTrig:
                    control = new ToolAutoTrig();
                    break;
                case TypeTool.MultiLearning:
                    control = new ToolMultiOnnx();
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
        public static dynamic CreateItemTool(BeeCore.PropetyTool PropetyTool, int Index, int IndexThread)
        {


            ItemTool itemTool = null;
            TypeTool TypeTool = PropetyTool.TypeTool;
            try
            {
                itemTool = new ItemTool(TypeTool, TypeTool.ToString() + Convert.ToString(Index - 1),TriggerNum.Trigger1);
                //itemTool.Location = pDraw;
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
                PropetyTool.Propety2.Index = Index;
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
                BeeCore.Common.PropetyTools[Global.IndexProgChoose].Remove(PropetyTool);
                return null;
            }
            return itemTool;
        }
        public static dynamic NewControl(TypeTool TypeTool, int Index, int IndexThread,String Nametool)
        {



         
            dynamic control = New(TypeTool,true);

            try
            {
                if (control == null) return null;
                int with = 50, height = 50;
               
                control.Propety.Index = Index;
                System.Drawing.Size szImg = Global.Config.SizeCCD;
                control.Propety.IndexThread = IndexThread;
              



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

        public static  dynamic CreateControls(BeeCore.PropetyTool PropetyTool,int Index,int IndexThread)
        {
           
            
                
                TypeTool TypeTool = PropetyTool.TypeTool;
                dynamic control = New(TypeTool,false);
               
            try
            {
                if (control == null) return null;
                int with = 50, height = 50;
                
                control.Propety= PropetyTool.Propety2;
             
                control.Propety.Index = Index;
                System.Drawing.Size szImg = Global.Config.SizeCCD;
               
               // BeeCore.Common.CreateTemp(TypeTool, IndexThread);
                if (PropetyTool.Name == null) PropetyTool.Name = "";
                control.Name = PropetyTool.Name;
               // PropetyTool.worker = new System.ComponentModel.BackgroundWorker();
                PropetyTool.timer = new System.Diagnostics.Stopwatch();
                //PropetyTool.worker.DoWork += (sender, e) =>
                //{
                //    var bw = (BackgroundWorker)sender;

                //    Exception exOut = null;
                //    bool finished = false;

                //    var t = System.Threading.Tasks.Task.Run(() =>
                //    {
                //        try
                //        {
                //            PropetyTool.DoWork();
                //            finished = true;
                //        }
                //        catch (Exception ex) { exOut = ex; }

                //    });

                //    if (!t.Wait(Global.Config.TimerOutChecking))
                //    {
                //        PropetyTool.Results = Results.NG;
                //        PropetyTool.StatusTool = StatusTool.Done;
                //        e.Cancel = true; // coi như timeout
                //        return;
                //    }

                //    if (exOut != null) throw exOut;
                //    if (!finished) e.Cancel = true;
                  
                //};
                //PropetyTool.worker.RunWorkerCompleted += (sender, e) =>
                //{
                //    PropetyTool.Complete();
                //};
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
               BeeCore.Common.PropetyTools[Global.IndexProgChoose].Remove(PropetyTool);
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
