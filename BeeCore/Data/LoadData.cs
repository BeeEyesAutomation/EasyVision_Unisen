
using BeeGlobal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore { 
    public  class LoadData
    {
        public static List<List<BeeCore.PropetyTool>> Project(String Project)
        {
            List<List<BeeCore.PropetyTool>> listPropetyTool = new List<List<BeeCore.PropetyTool>>();
            try
            {
              
                if (Global.Config.IsSaveProg)
                {
                    if (File.Exists("Common\\Common.prog"))
                        listPropetyTool = Access.LoadProg("Common\\Common.prog");
                    else
                        listPropetyTool = new List<List<BeeCore.PropetyTool>>();
                }
                else
                {
                    if (File.Exists("Program\\" + Project + "\\" + Project + ".prog"))
                        listPropetyTool = Access.LoadProg("Program\\" + Project + "\\" + Project + ".prog");
                    else
                        listPropetyTool = new List<List<BeeCore.PropetyTool>>();
                }
            }
            catch(Exception ex) {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Load Project", ex.Message.ToString()));
            }


            return listPropetyTool;
        }
        public static ParaCommon Para(String Project)
        {
            ParaCommon ParaCam = new ParaCommon();
            try
            {

                if (Global.Config.IsSaveCommon)
                {
                    if (File.Exists("Common\\Common.para"))
                        ParaCam = Access.LoadParaComon("Common\\Common.para");
                    else
                        ParaCam = new ParaCommon();
                }
                else
                {
                    if (File.Exists("Program\\" + Project + "\\" + Project + ".para"))
                        ParaCam = Access.LoadParaComon("Program\\" + Project + "\\" + Project + ".para");
                    else
                        ParaCam = new ParaCommon();
                }
            }
            catch (Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Load Common", ex.Message.ToString()));
            }
        //    if(ParaCam.listRegsImg==null) ParaCam.listRegsImg = new List<ItemRegsImg>();
            return ParaCam;
        }
        public static Comunication Comunication(String Project)
        {
            Comunication Comunication = new Comunication();
            try
            {
                if (Global.Config.IsSaveCommunication)
                {
                    if (File.Exists("Common\\Common.com"))
                        Comunication = Access.LoadComunication("Common\\Common.com");
                    else
                        Comunication = new Comunication();
                }
                else
                {
                    if (File.Exists("Program\\" + Project + "\\" + Project + ".com"))
                        Comunication = Access.LoadComunication("Program\\" + Project + "\\" + Project + ".com");
                    else
                        Comunication = new Comunication();
                }
            }
            catch (Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Load Communication", ex.Message.ToString()));
            }
            return Comunication;
        }
        public static ParaShow ParaShow(String Project)
        {
            bool IsNew = false;
            ParaShow ParaShow = new ParaShow();
            try
            {
                if (Global.Config.IsSaveParaShow)
                {
                    if (File.Exists("Common\\Common.gc"))
                        ParaShow = Access.LoadParaShow("Common\\Common.gc");
                    else
                    {
                        IsNew = true; ParaShow = new ParaShow();
                    }

                }
                else
                {
                    if (File.Exists("Program\\" + Project + "\\" + Project + ".gc"))
                        ParaShow = Access.LoadParaShow("Program\\" + Project + "\\" + Project + ".gc");
                    else
                    {
                        ParaShow = new ParaShow(); IsNew = true;
                    }

                }
                if (IsNew)
                {
                    ParaShow.ColorOK = Color.FromArgb(0, 172, 73);
                    ParaShow.ColorNG = Color.DarkRed;
                    ParaShow.ColorInfor = Color.Blue;
                    ParaShow.ColorNone = Color.LightGray;
                    ParaShow.ColorChoose = Color.FromArgb(246, 204, 120);
                    ParaShow.TextColor = Color.White;
                }
            }
            catch(Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Load Show", ex.Message.ToString()));
            }
            return ParaShow;
        }
        public static List<ItemRegsImg> listImgRegister(String Project)
        {
            List<ItemRegsImg> listImg = new List<ItemRegsImg>();
            try
            {


                if (Global.Config.IsSaveListRegister)
                {
                    if (File.Exists("Common\\Common.reg"))
                        listImg = Access.LoadlistImg("Common\\Common.reg");
                    else
                        listImg = new List<ItemRegsImg>();
                }
                else
                {
                    if (File.Exists("Program\\" + Project + "\\" + Project + ".reg"))
                        listImg = Access.LoadlistImg("Program\\" + Project + "\\" + Project + ".reg");
                    else
                        listImg = new List<ItemRegsImg>();
                }
            }
            catch(Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Load Project", ex.Message.ToString()));
            }
            return listImg;
        }
        public static List<ItemRegsImg> listImgSim(String Project)
        {
            List<ItemRegsImg> listImg = new List<ItemRegsImg>();
            try
            {
                if (Global.Config.IsSaveListSim)
                {
                    if (File.Exists("Common\\Common.sim"))
                        listImg = Access.LoadlistImg("Common\\Common.sim");
                    else
                        listImg = new List<ItemRegsImg>();
                }
                else
                {
                    if (File.Exists("Program\\" + Project + "\\" + Project + ".sim"))
                        listImg = Access.LoadlistImg("Program\\" + Project + "\\" + Project + ".sim");
                    else
                        listImg = new List<ItemRegsImg>();
                }
            }
            catch(Exception  ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Load ImgSim", ex.Message.ToString()));
            }
            return listImg;
        }
        public static Config Config( )
        {
            Config config = new Config();
            try
            {
                if (File.Exists("Common\\Default.config"))
                    config = Access.LoadConfig("Common\\Default.config");
                else
                    config = new Config();
                if (config.LimitExposure == 0)
                    config.LimitExposure = 100000;
                if (config.LimitDelayTrigger == 0)
                    config.LimitDelayTrigger = 10000;
                if (config.ListNameStep == null)
                {
                    config.ListNameStep = "Start \n Done";

                }

                config.IsExternal = true;
            }
            catch (Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Load Config", ex.Message.ToString()));
            }
            return config;

        }
        public static List<ProgNo> ProgNo()
        {
            List < ProgNo > ProgNo=new List<ProgNo>();
            if (File.Exists("Common\\ProgNo.no"))
                ProgNo = Access.LoadProgNo("Common\\ProgNo.no");
            return ProgNo;

        }
        public static List<ParaCamera> ParaCamera(String Project)
        {
            List<ParaCamera> ParaCam = new List<ParaCamera>();
            try
            {
                if (Global.Config.IsSaveParaCam)
                {
                    if (File.Exists("Common\\Common.cam"))
                        ParaCam = Access.LoadParaCamera("Common\\Common.cam");
                    else
                        ParaCam = new List<ParaCamera>();
                }
                else
                {
                    if (File.Exists("Program\\" + Project + "\\" + Project + ".cam"))
                        ParaCam = Access.LoadParaCamera("Program\\" + Project + "\\" + Project + ".cam");
                    else
                        ParaCam = new List<ParaCamera>();
                }
            }
            catch (Exception ex)
            {
                Global.LogsDashboard.AddLog(new LogEntry(DateTime.Now, LeveLLog.ERROR, "Load ParaCam", ex.Message.ToString()));
            }
            return ParaCam;
        }
    }
}
