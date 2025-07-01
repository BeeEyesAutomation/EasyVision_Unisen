using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeCore;

namespace BeeUi.Data
{
    public class SaveData
    {
         public static void ParaPJ( String Project,ParaCommon ParaCam)
        {
            String path = "Program\\" + Project;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
                Access.SaveParaComon(path + "\\" + Project + ".para", ParaCam);
        }
        public static void Config(Config config)
        {
            Access.SaveConfig("Default.config", config);
        }
        public static void Camera(String Project, List<ParaCamera> ParaCamera)
        {
            String path = "Program\\" + Project;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            Access.SaveParaCamera(path + "\\" + G.Project + ".cam", ParaCamera);
           
        }
        public static void Project(String Project)
        {
            String path = "Program\\" + Project;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            Access.SaveProg(path + "\\" + G.Project + ".prog", G.PropetyTools);
            Access.SaveConfig("Default.config", G.Config);
            Access.SaveParaComon(path + "\\" + G.Project + ".para", BeeCore.G.ParaCam);
            Access.SaveParaCamera(path + "\\" + G.Project + ".cam", BeeCore.Common.listParaCamera);
        }
    }
}
