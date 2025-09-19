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
            Access.SaveParaCamera(path + "\\" + Global.Project + ".cam", ParaCamera);
           
        }
        public static void Project(String Project)
        {
            try
            {
                if (Global.listParaCamera[0] != null)
                {
                    MessageBox.Show("OK Cam " + BeeCore.Common.listCamera[0].Para.Name);
                }
                else
                    MessageBox.Show("NULL1");
                if (BeeCore.Common.listCamera[0] != null)
                {
                    MessageBox.Show("OK Cam " + BeeCore.Common.listCamera[0].Para.Name);
                }
                else
                    MessageBox.Show("NULL2");
                String path = "Program\\" + Project;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Access.SaveProg(path + "\\" + Global.Project + ".prog", BeeCore.Common.PropetyTools);
                Access.SaveConfig("Default.config", Global.Config);
                Access.SaveParaComon(path + "\\" + Global.Project + ".para", Global.ParaCommon);
                Access.SaveParaCamera(path + "\\" + Global.Project + ".cam", Global.listParaCamera);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
