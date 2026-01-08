using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
    [Serializable()]
    public class ParaShow
    {
        public int FontSize = 16;
        public int Opacity = 100;
        public int RadEdit = 40;
        public Color ColorOK;
        public Color ColorNG;
        public Color ColorNone;
        public Color ColorChoose;
        public Color ColorInfor;
        public Color TextColor;

        public int ThicknessLine = 4;
        public bool IsShowDetail = true;
        public bool IsShowBox = true;
        public bool IsShowScore = false;
        public bool IsShowLabel = true;
        public bool IsShowPostion = true;
        public bool IsShowResult = true;
        public bool IsShowNotMatching = true;
        public bool IsShowMatProcess = false;
        public ParaShow() { }
    }
}
