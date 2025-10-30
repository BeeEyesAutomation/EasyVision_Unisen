
using BeeGlobal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore { 
    public  class LoadData
    {
        public static List<List<BeeCore.PropetyTool>> Project(String Project)
        {
            List<List < BeeCore.PropetyTool >> listPropetyTool= new List<List <BeeCore.PropetyTool>>();
            if (File.Exists("Program\\" + Project + "\\" + Project + ".prog"))
                listPropetyTool = Access.LoadProg("Program\\" + Project + "\\" + Project + ".prog");
            else
                listPropetyTool = new List<List<BeeCore.PropetyTool>>();
            return listPropetyTool;
        }
        public static ParaCommon Para(String Project)
        {
            ParaCommon ParaCam = new ParaCommon();
            if (File.Exists("Program\\" + Project + "\\" + Project + ".para"))
                ParaCam = Access.LoadParaComon("Program\\" + Project + "\\" + Project + ".para");
            else
                ParaCam=new ParaCommon();
            if(ParaCam.listRegsImg==null) ParaCam.listRegsImg = new List<ItemRegsImg>();
            return ParaCam;
        }
        public static List<ParaCamera> ParaCamera(String Project)
        {
           List<  ParaCamera> ParaCam = new List<ParaCamera>();
            if (File.Exists("Program\\" + Project + "\\" + Project + ".cam"))
                ParaCam = Access.LoadParaCamera("Program\\" + Project + "\\" + Project + ".cam");
            else
                ParaCam = new List<ParaCamera>();
            return ParaCam;
        }
    }
}
