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
         
            
            if (Global.Config.IsSaveCommon)
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
        public static void ProgNo(List<ProgNo> ProgNo)
        {
            if (!Directory.Exists("Common"))
                Directory.CreateDirectory("Common");
            Access.SaveProgNo("Common\\ProgNo.no", ProgNo);
        }
        public static void Camera(String Project, List<ParaCamera> ParaCamera)
        {
            if (Global.Config.IsSaveParaCam)
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
        public static void Comunication(String Project, Comunication Comunication)
        {
            if (Global.Config.IsSaveCommunication)
            {
                String path = "Common";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Access.SaveComunication(path + "\\Common.com", Comunication);
            }
            else
            {
                String path = "Program\\" + Project;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Access.SaveComunication(path + "\\" + Global.Project + ".com", Comunication);
            }


        }
        public static void ParaShow(String Project, ParaShow ParaShow)
        {
            if (Global.Config.IsSaveParaShow)
            {
                String path = "Common";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Access.SaveParaShow(path + "\\Common.gc", ParaShow);
            }
            else
            {
                String path = "Program\\" + Project;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Access.SaveParaShow(path + "\\" + Global.Project + ".gc", ParaShow);
            }

        }
        public static void ListImgRegister(String Project, List<ItemRegsImg> listImg)
        {
            if (Global.Config.IsSaveListRegister)
            {
                String path = "Common";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Access.SavelistImg(path + "\\Common.reg", listImg);
            }
            else
            {
                String path = "Program\\" + Project;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Access.SavelistImg(path + "\\" + Global.Project + ".reg", listImg);
            }

        }
        public static void ListImgSim(String Project, List<ItemRegsImg> listImg)
        {
            if (Global.Config.IsSaveListSim)
            {
                String path = "Common";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Access.SavelistImg(path + "\\Common.sim", listImg);
            }
            else
            {
                String path = "Program\\" + Project;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Access.SavelistImg(path + "\\" + Global.Project + ".sim", listImg);
            }

        }
        public static void Program(String Project, List<List<PropetyTool>> Prog)
        {
            String path = "Program\\" + Project;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            Access.SaveProg(path + "\\" + Global.Project + ".prog", Prog);

        }
        public static void Project(String Project)
        {
            try
            {
                Config(Global.Config);
                   String path = "Program\\" + Project;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                Program(Project, BeeCore.Common.PropetyTools);
               // Access.SaveProg(path + "\\" + Global.Project + ".prog", BeeCore.Common.PropetyTools);
               // Access.SaveConfig("Default.config", Global.Config);
                ParaPJ(Project, Global.ParaCommon);
                Camera(Project, Global.listParaCamera);
                Comunication(Project, Global.Comunication);
                ParaShow(Project, Global.ParaShow);
                ListImgRegister(Project, Global.listRegsImg);
                ListImgSim(Project, Global.listSimImg);
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
