using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    [Serializable()]
    public class Vision
    {
        public String NameModel;
        public String PathPythonTest;
        public String PathYolo;
        public String PathPythonTrain;
        public float Score;
        public float Counter;
        public String Ex;
        public bool IsOK;
        public int MaxObject;
        public IntPtr ptr;
        public float Cycle;
        public Vision()
        {

        }
    }
}
