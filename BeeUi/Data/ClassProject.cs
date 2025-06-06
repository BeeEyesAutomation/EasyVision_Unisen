using BeeCore;
using BeeUi.Commons;
using BeeUi.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeUi.Data
{
    public class ClassProject
    {
        public static void Load(String NameProject)
        {
            BeeCore.G.ParaCam = LoadData.Para(NameProject);


            NameProject = NameProject.Replace(".prog", "");
            BeeCore.G.ParaCam.SizeCCD = Camera.GetSzCCD();
            G.PropetyTools = LoadData.Project(NameProject);



            G.listAlltool = new List<Tools>();


            if (G.ToolSettings == null)
            {
                G.ToolSettings = new ToolSettings();

            }
            G.ToolSettings.Y = 10; G.ToolSettings.X = 5;
            int index = 0;
            foreach (BeeCore.PropetyTool propety in G.PropetyTools)
            {
                Tools tool = DataTool.SetPropety(propety, index);
                if (tool != null)
                    G.listAlltool.Add(tool);
                
                index++;

            }
            foreach (Tools tool in G.listAlltool)
            {
                DataTool.LoadPropety(tool.tool);
               X: if(tool.tool.Propety.StatusTool != StatusTool.Initialed)
                {
                    Task.Delay(10);
                    goto X;
                }
               else
                {
                    continue;
                }
            }


            G.IsLoad = true;
           // IsLoaded = true;

            BeeCore.G.ParaCam.Exposure = BeeCore.G.ParaCam._Exposure;
            BeeCore.G.ParaCam.TypeLight = BeeCore.G.ParaCam._TypeLight;
            BeeCore.G.ParaCam.TypeResolution = BeeCore.G.ParaCam._TypeResolution;
        }
    }
}
