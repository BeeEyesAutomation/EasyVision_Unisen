using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    [Serializable()]
    public class Comunication
    {
        public MethordComunication MethordComunication = MethordComunication.Params;
        public StatusComunication StatusComunication = StatusComunication.Disconnect;
        public ModeComunication ModeComunication = ModeComunication.Server;
        public TypeComunication TypeComunication = TypeComunication.MobusRS485;
        public String IP = "";
        public String Com = "COM3";
        public bool IsConnected =false ;
        public int Port = 5555;
        public int Baurate = 115200;
        public String[] nameInput = new String[8];
        public String[] nameOutput = new String[16];
        public int[] valueInput = new int[8];
        public int[] valueOutput = new int[16];
        public int[] AddInput;
        public int[] AddOutput;
        public byte SlaveID = 2;
    }
}
