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


            if (Global.ToolSettings == null)
            {
                Global.ToolSettings = new ToolSettings();

            }
            Global.pShowTool.Y = 10;  Global.pShowTool.X = 5;
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
                            dynamic control = DataTool.CreateControls(PropTool, i, indexThread, new Point( Global.pShowTool.X,  Global.pShowTool.Y));
                            ItemTool Itemtool = DataTool.CreateItemTool(PropTool, i, indexThread, new Point( Global.pShowTool.X,  Global.pShowTool.Y));

                             Global.pShowTool.Y += Itemtool.Height + 10;
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
       }
}
