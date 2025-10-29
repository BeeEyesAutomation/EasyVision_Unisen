﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
   
    [Serializable()]
    public class Config
    {
        //public Image icon;
        public Color ColorBG = Color.FromArgb(223, 223, 223);
        public Color ColorBar1 = Color.FromArgb(114, 114, 114);
        public Color ColorBar2 = Color.FromArgb(100, 114, 114, 114);
        public Color ColorEnd = Color.WhiteSmoke;
        public Color ColorRight = Color.WhiteSmoke;
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
        public bool IsShowPostion = true;
        public bool IsShowResult = true;
        public bool IsShowNotMatching = true;
        public bool IsShowMatProcess = false;

        public float TotalTime = 0, Percent = 0;
        public  int SumOK, SumNG, SumTime = 0;
        public  Color colorGui = Color.FromArgb(100, 114, 114, 114);
        public int WidthEditProg = 0;
        public double Scalefont = 1;
        public int AlphaMenu = 100;
        public int AlphaBar = 80;
        public int AlphaBackground = 40;
        public int AlphaText = 20;
        public int RoundRad = 10;
        public bool IsByPass;
        public int DelayOutput = 100;
        public  int delayTrigger = 100;
        public int imgOffSetX = 0, imgOffSetY = 0, imgZoom = 0;
        
        public int LimitDateSave = 15;
        public bool IsSaveOK = true, IsSaveNG = false;
        public bool IsSaveRaw = true, IsSaveRS = false;
        public int TypeSave = 1;
        public int numToolOK = 0;
       
        public LogicOK LogicOK =LogicOK.AND;
        public ConditionOK ConditionOK=ConditionOK.TotalOK;
        
        public String namePort="",IDPort="";
        public String IDCamera="";
       
        public Users Users = Users.User;
        public int CCD,ScoreCalib=1;
        public bool IsHist = false,IsShowGird=false,IsShowCenter=false,IsShowArea=false;
    }
}
