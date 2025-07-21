using BeeCore;
using BeeGlobal;
using BeeInterface;
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

namespace BeeUi.Data
{
    public class ClassProject
    {
        public static void Load(String NameProject)
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
                {if (paraCamera != null)
                        BeeCore.Common.listCamera.Add(new Camera(paraCamera, indexCCD));
                    else
                        BeeCore.Common.listCamera.Add(null);
                    indexCCD++;
                }
                if (BeeCore.Common.listCamera.Count() > Global.IndexChoose)
                    if (BeeCore.Common.listCamera[Global.IndexChoose] != null)
                        Global.ParaCommon.SizeCCD = BeeCore.Common.listCamera[Global.IndexChoose].GetSzCCD();
            }
            //Global.ParaCommon.SizeCCD = Camera.GetSzCCD();
            BeeCore.Common.PropetyTools = LoadData.Project(NameProject);


            if(BeeCore.Common.PropetyTools.Count==0)
            {
                BeeCore.Common.PropetyTools = new List<List<PropetyTool>> { new List<PropetyTool>(), new List<PropetyTool>(),  new List<PropetyTool>(), new List<PropetyTool>() };
              //  G.listAlltool = new List<List<Tools>> { new List<Tools>(), new List<Tools>(), new List<Tools>(), new List<Tools>() };

            }
            //else
            //    G.listAlltool = new List<List<Tools>>();


            if (G.ToolSettings == null)
            {
                G.ToolSettings = new ToolSettings();

            }
            G.ToolSettings.Y = 10; G.ToolSettings.X = 5;
            int indexThread = 0;

            foreach (List< BeeCore.PropetyTool> ListTool in BeeCore.Common.PropetyTools)
            {
                if (ListTool != null)
                {
                    int i = 0;
                    foreach(PropetyTool PropTool in ListTool)
                    {
                        try
                        {
                            dynamic control = DataTool.CreateControls(PropTool, i, indexThread, new Point(G.ToolSettings.X, G.ToolSettings.Y));
                            ItemTool Itemtool = DataTool.CreateItemTool(PropTool, i, indexThread, new Point(G.ToolSettings.X, G.ToolSettings.Y));

                            G.ToolSettings.Y += Itemtool.Height + 10;
                            PropTool.ItemTool = Itemtool;
                            PropTool.Control = control;
                            DataTool.LoadPropety(PropTool.Control);
                        }
                        catch(Exception ex)
                        {
                            String s = ex.Message;
                        }
                      
                        i++;
                    }

                }
             


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
            }
          
            G.IsLoad = true;
          
        }
        int indexToolPosition = 0;
        public StatusProcessing ProcessingAll(StatusProcessing StatusProcessing, int indexThread = 0)
        {
            if (!G.Initial)
                return StatusProcessing;
            if (!Global.Comunication.IO.IsConnected && !Global.Comunication.IO.IsBypass)
                return StatusProcessing;
            if (BeeCore.Common.PropetyTools[indexThread].Count == 0)
                return StatusProcessing.Done;
            switch (StatusProcessing)
            {
                case StatusProcessing.None:
                    // timer.Restart();
                //int    indexToolPosition = BeeCore.Common.PropetyTools[indexThread].FindIndex(a => a.TypeTool == TypeTool.Position_Adjustment);
                    if (indexToolPosition == -1)
                    {
                        StatusProcessing = StatusProcessing.Processing;
                        return StatusProcessing;
                    }
                    if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].TypeTool == TypeTool.Position_Adjustment)
                    {

                        if (!BeeCore.Common.PropetyTools[indexThread][indexToolPosition].worker.IsBusy)
                            BeeCore.Common.PropetyTools[indexThread][indexToolPosition].worker.RunWorkerAsync();

                        StatusProcessing = StatusProcessing.Adjusting;
                    }
                    else
                    {
                        foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[indexThread])
                        {

                            dynamic Propety = propetyTool.Propety;
                            Propety.rotAreaAdjustment = Propety.rotArea;


                        }
                        StatusProcessing = StatusProcessing.Processing;
                    }

                    break;
                case StatusProcessing.Adjusting:
                    if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].StatusTool == StatusTool.Done)
                    {
                        dynamic Propety = BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Propety;

                        StatusProcessing = StatusProcessing.Processing;

                        if (BeeCore.Common.PropetyTools[indexThread][indexToolPosition].Results==Results.OK)
                        {
                            if (Global.rotOriginAdj == null) return StatusProcessing;
                            Global.X_Adjustment = Propety.rotArea._PosCenter.X - Propety.rotArea._rect.Width / 2 + Propety.rectRotates[0]._PosCenter.X -Global.rotOriginAdj ._PosCenter.X;
                            Global.Y_Adjustment = Propety.rotArea._PosCenter.Y - Propety.rotArea._rect.Height / 2 + Propety.rectRotates[0]._PosCenter.Y -Global.rotOriginAdj ._PosCenter.Y;
                            Global.angle_Adjustment = Propety.rotArea._rectRotation + Propety.rectRotates[0]._rectRotation -Global.rotOriginAdj ._rectRotation;
                        }
                        foreach (PropetyTool propetyTool in BeeCore.Common.PropetyTools[indexThread])
                        {
                            if (propetyTool.TypeTool == TypeTool.Position_Adjustment)
                                continue;
                            if (Global.rotOriginAdj  != null)
                            {
                                propetyTool.Propety.rotAreaAdjustment = BeeCore.Common.GetPositionAdjustment(propetyTool.Propety.rotArea,Global.rotOriginAdj );
                                if (propetyTool.TypeTool == TypeTool.Positions)
                                {
                                    propetyTool.Propety.pOrigin = new System.Drawing.Point(Global.pOrigin.X, Global.pOrigin.Y);
                                    propetyTool.Propety.AngleOrigin = Global.AngleOrigin;
                                }


                            }
                            else
                                propetyTool.Propety.rotAreaAdjustment = propetyTool.Propety.rotArea;


                        }
                    }
                    break;
                case StatusProcessing.Processing:
                    G.StatusDashboard.StatusText= "---";
                     G.StatusDashboard.StatusBlockBackColor= Color.Gray;
                    foreach (PropetyTool PropetyTool in BeeCore.Common.PropetyTools[indexThread])
                    {
                        PropetyTool.ItemTool.Status = "---";
                        // Tools.ItemTool.Score.ColorTrack = Color.Gray;
                        PropetyTool.ItemTool.ClScore = Color.Gray;
                        PropetyTool.ItemTool.ClStatus = Color.Gray;
                        PropetyTool.ItemTool.Refresh();
                        if (PropetyTool.TypeTool == TypeTool.Position_Adjustment) continue;
                        PropetyTool.StatusTool = StatusTool.WaitCheck;
                        if (!PropetyTool.worker.IsBusy)
                            PropetyTool.worker.RunWorkerAsync();
                    }
                    StatusProcessing = StatusProcessing.WaitingDone;
                    break;
                case StatusProcessing.WaitingDone:
                    StatusProcessing = StatusProcessing.Done;
                    Parallel.For(0, BeeCore.Common.PropetyTools[indexThread].Count, i =>
                    {
                        PropetyTool PropetyTool = BeeCore.Common.PropetyTools[indexThread][i];


                       
                        if (PropetyTool.StatusTool != StatusTool.Done)
                        {
                            StatusProcessing = StatusProcessing.WaitingDone;

                        }
                        else
                        {
                            //if (Tools.tool.Propety.IsOK)
                            //{

                            //    if (G.Config.ConditionOK == ConditionOK.Logic)
                            //    {
                            //        if (propetyTool.UsedTool == UsedTool.Used)
                            //        {
                            //            Tools.ItemTool.lbStatus.Text = "OK";
                            //            Tools.ItemTool.Score.ColorTrack = Color.FromArgb(0, 172, 73);
                            //            Tools.ItemTool.lbScore.ForeColor = Color.FromArgb(0, 172, 73);
                            //            Tools.ItemTool.lbStatus.BackColor = Color.FromArgb(0, 172, 73);
                            //        }
                            //        else
                            //        {
                            //            Tools.ItemTool.Score.ColorTrack = Color.DarkRed;
                            //            Tools.ItemTool.lbStatus.Text = "NG";
                            //            Tools.ItemTool.lbScore.ForeColor = Color.DarkRed;
                            //            Tools.ItemTool.lbStatus.BackColor = Color.DarkRed;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        Tools.ItemTool.lbStatus.Text = "OK";
                            //        Tools.ItemTool.Score.ColorTrack = Color.FromArgb(0, 172, 73);
                            //        Tools.ItemTool.lbScore.ForeColor = Color.FromArgb(0, 172, 73);
                            //        Tools.ItemTool.lbStatus.BackColor = Color.FromArgb(0, 172, 73);
                            //    }
                            //}

                            //else
                            //{

                            //    if (G.Config.ConditionOK == ConditionOK.Logic)
                            //    {
                            //        if (propetyTool.UsedTool != UsedTool.Used)
                            //        {
                            //            Tools.ItemTool.lbStatus.Text = "OK";
                            //            Tools.ItemTool.Score.ColorTrack = Color.FromArgb(0, 172, 73);
                            //            Tools.ItemTool.lbScore.ForeColor = Color.FromArgb(0, 172, 73);
                            //            Tools.ItemTool.lbStatus.BackColor = Color.FromArgb(0, 172, 73);
                            //        }
                            //        else
                            //        {
                            //            Tools.ItemTool.Score.ColorTrack = Color.DarkRed;
                            //            Tools.ItemTool.lbStatus.Text = "NG";
                            //            Tools.ItemTool.lbScore.ForeColor = Color.DarkRed;
                            //            Tools.ItemTool.lbStatus.BackColor = Color.DarkRed;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        Tools.ItemTool.Score.ColorTrack = Color.DarkRed;
                            //        Tools.ItemTool.lbStatus.Text = "NG";
                            //        Tools.ItemTool.lbScore.ForeColor = Color.DarkRed;
                            //        Tools.ItemTool.lbStatus.BackColor = Color.DarkRed;
                            //    }

                            //}
                            //Tools.ItemTool.Refresh();
                        }

                    }
                    );

                    break;
            }
            return StatusProcessing;


        }
    }
}
