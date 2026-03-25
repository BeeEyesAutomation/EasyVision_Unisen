using BeeCore.Core;
using BeeCpp;
using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore.Algorithm
{
    public  class Colors
    {
        static BeeCpp.ColorArea ColorAreaPP = new BeeCpp.ColorArea();
        public static HSVCli GetHSV(Mat raw, int x, int y)
        {
            try
            {
                using (Mat mat = raw.Clone())
                {
                    if (mat.Empty())
                        return new HSVCli();
                    if (mat.Type() == MatType.CV_8UC1)
                    {
                        Cv2.CvtColor(mat, mat, ColorConversionCodes.GRAY2BGR);
                    }

                    ColorAreaPP.SetImgeRaw(mat.Data, mat.Width, mat.Height, (int)mat.Step(), mat.Channels());
                    return ColorAreaPP.GetHSV(x, y);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return new HSVCli();
        }
        public static RGBCli GetRGB(Mat raw, int x, int y)
        {
            using (Mat mat = raw.Clone())
            {
                if (mat.Empty()) return new RGBCli();
                if (mat.Type() == MatType.CV_8UC1)
                {
                    Cv2.CvtColor(mat, mat, ColorConversionCodes.GRAY2BGR);
                }
                BeeCpp.ColorArea ColorAreaPP = new BeeCpp.ColorArea();
                ColorAreaPP.SetImgeRaw(mat.Data, mat.Width, mat.Height, (int)mat.Step(), mat.Channels());
                return ColorAreaPP.GetRGB(x, y);
            }
        }
        public static Color GetColor(RGBCli rGB)
        {
         return   Color.FromArgb(rGB.R, rGB.G, rGB.B);
           
        }
        public static Color GetColor(HSVCli hSV)
        {
            if (hSV == null)
                return new Color();
            return HsvConvert.FromHsvOpenCv((byte)hSV.H, (byte)hSV.S, (byte)hSV.V);

        }
        //public static void SetTemp(HSVCli[] arrHSV,int Extraction)
        //{
        //    ColorAreaPP.SetTempHSV(arrHSV, Extraction);
        //}


        
    }
}
