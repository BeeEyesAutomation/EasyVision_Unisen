using BeeCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeUi.Data
{
    public  class LoadData
    {
        public static List<BeeCore.PropetyTool> Project(String Project)
        {
            List < BeeCore.PropetyTool > listPropetyTool= new List <BeeCore.PropetyTool>();
            if (File.Exists("Program\\" + Project + "\\" + Project + ".prog"))
                listPropetyTool = Access.LoadProg("Program\\" + Project + "\\" + Project + ".prog");
            else
                listPropetyTool = new List<BeeCore.PropetyTool>();
            return listPropetyTool;
        }
        public static ParaCam Para(String Project)
        {
            ParaCam ParaCam = new ParaCam();
            if (File.Exists("Program\\" + Project + "\\" + Project + ".para"))
                ParaCam = Access.LoadParaCam("Program\\" + Project + "\\" + Project + ".para");
            else
                ParaCam=new ParaCam();
            return ParaCam;
        }
    }
}
