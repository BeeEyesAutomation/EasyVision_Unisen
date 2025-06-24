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
         public static void ParaPJ( String Project,ParaCam ParaCam)
        {
            String path = "Program\\" + Project;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
                Access.SaveParaCam(path + "\\" + Project + ".para", ParaCam);
        }
        public static void Config(Config config)
        {
            Access.SaveConfig("Default.config", config);
        }
        public static void Project(String Project, ParaCam ParaCam)
        {
            String path = "Program\\" + Project;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            Access.SaveProg(path + "\\" + G.Project + ".prog", G.PropetyTools);
            Access.SaveConfig("Default.config", G.Config);
            Access.SaveParaCam(path + "\\" + G.Project + ".para", BeeCore.G.ParaCam);
        }
    }
}
