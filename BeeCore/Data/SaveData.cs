using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BeeGlobal;
namespace BeeCore
{
    public class SaveData
    {
         public static void ParaPJ( String Project,ParaCommon ParaCam)
        {
            if(Global.Config.ModeSaveProg==ModeSaveProg.Single)
            {
                String path = "Common";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Access.SaveParaComon(path + "\\Common.para", ParaCam);

            }
            else
            {
                String path = "Program\\" + Project;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Access.SaveParaComon(path + "\\" + Project + ".para", ParaCam);

            }    
            
        }
        public static void Config(Config config)
        {
            if (!Directory.Exists("Common"))
                Directory.CreateDirectory("Common");
            Access.SaveConfig("Common\\Default.config", config);
        }
        public static void Camera(String Project, List<ParaCamera> ParaCamera)
        {
            if (Global.Config.ModeSaveProg == ModeSaveProg.Single)
            {
                String path = "Common";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Access.SaveParaCamera(path + "\\Common.cam", ParaCamera);
            }
            else
            {
                String path = "Program\\" + Project;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Access.SaveParaCamera(path + "\\" + Global.Project + ".cam", ParaCamera);
            }    
            
           
        }
        public static void Program(String Project, List<List<PropetyTool>> Prog)
        {
            //if (Global.Config.ModeSaveProg == ModeSaveProg.Single)
            //{
            //    String path = "Common";
            //    if (!Directory.Exists(path))
            //        Directory.CreateDirectory(path);
            //    Access.SaveProg(path + "\\Common.prog", Prog);
            //}
            //else
            //{
                String path = "Program\\" + Project;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Access.SaveProg(path + "\\" + Global.Project + ".prog", Prog);
            //}


        }
        public static void Project(String Project)
        {
            try
            {
                Config(Global.Config);
                   String path = "Program\\" + Project;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Program(Global.Project, BeeCore.Common.PropetyTools);
               // Access.SaveProg(path + "\\" + Global.Project + ".prog", BeeCore.Common.PropetyTools);
               // Access.SaveConfig("Default.config", Global.Config);
                ParaPJ(Global.Project, Global.ParaCommon);
                Camera(Global.Project, Global.listParaCamera);
               // Access.SaveParaComon(path + "\\" + Global.Project + ".para", Global.ParaCommon);
                //Access.SaveParaCamera(path + "\\" + Global.Project + ".cam", Global.listParaCamera);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
