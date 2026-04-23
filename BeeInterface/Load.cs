using BeeCore;
using BeeGlobal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeInterface
{
    public class Load
    { public static void NewTool()
        {
            Global.itemNews = new List<ItemNew>();
            Global.itemNews.Add(new ItemNew(TypeTool.Position_Adjustment, GroupTool.Basic_Tool, Properties.Resources.Position_Adjustment, Properties.Resources.contentPosition_Adjustment, "?? Can ch?nh v? trí thông minh\r\nT? d?ng phát hi?n và hi?u ch?nh sai l?ch xoay, tru?t ho?c l?ch góc c?a s?n ph?m.\r\n?? Ð?m b?o hình ?nh luôn du?c chu?n hóa tru?c khi ki?m tra, phù h?p v?i dây chuy?n t?c d? cao."));
            Global.itemNews.Add(new ItemNew(TypeTool.Pattern, GroupTool.Basic_Tool, Properties.Resources.Pattern, Properties.Resources.contentPattern, "?? Nh?n d?ng m?u chu?n xác\r\nSo kh?p v?i m?u tham chi?u d? xác d?nh v? trí, hu?ng và d? chính xác c?a chi ti?t.\r\n?? ?ng d?ng trong d?nh v? linh ki?n, g?p d?t robot ho?c ki?m tra l?p ráp."));
            Global.itemNews.Add(new ItemNew(TypeTool.CheckMissing, GroupTool.Basic_Tool, Properties.Resources.Pattern, Properties.Resources.contentPattern, "?? Nh?n d?ng m?u chu?n xác\r\nSo kh?p v?i m?u tham chi?u d? xác d?nh v? trí, hu?ng và d? chính xác c?a chi ti?t.\r\n?? ?ng d?ng trong d?nh v? linh ki?n, g?p d?t robot ho?c ki?m tra l?p ráp."));

            Global.itemNews.Add(new ItemNew(TypeTool.MultiPattern, GroupTool.Basic_Tool, Properties.Resources.Pattern, Properties.Resources.contentPattern, "?? Nh?n d?ng m?u chu?n xác\r\nSo kh?p v?i m?u tham chi?u d? xác d?nh v? trí, hu?ng và d? chính xác c?a chi ti?t.\r\n?? ?ng d?ng trong d?nh v? linh ki?n, g?p d?t robot ho?c ki?m tra l?p ráp."));

            Global.itemNews.Add(new ItemNew(TypeTool.OKNG, GroupTool.Basic_Tool, Properties.Resources.OKNG, Properties.Resources.contentOKNG, "?? Phân lo?i t?c thì\r\nÐua ra k?t qu? OK (d?t) ho?c NG (không d?t) theo tiêu chí cài d?t.\r\n?? Tích h?p tr?c ti?p v?i PLC/I/O, ph?n h?i ngay trên dây chuy?n s?n xu?t."));
            Global.itemNews.Add(new ItemNew(TypeTool.Color_Area, GroupTool.Basic_Tool, Properties.Resources.Color_Area, Properties.Resources.contentColor, "?? Ki?m tra vùng màu công nghi?p\r\nÐo lu?ng chính xác màu s?c và di?n tích vùng c?n ki?m tra.\r\n?? ?ng d?ng trong phân lo?i s?n ph?m, in ?n bao bì, nh?n di?n logo ho?c nhãn mác."));
            Global.itemNews.Add(new ItemNew(TypeTool.VisualMatch, GroupTool.Basic_Tool, Properties.Resources.VisualMatch, Properties.Resources.contentVisualMatch, "So kh?p m?u (Visual Matching): So sánh hình ?nh s?n ph?m th?c t? v?i ?nh m?u chu?n (master).\r\n\r\nPhân lo?i OK/NG: K?t qu? t? d?ng phân lo?i s?n ph?m d?t chu?n (OK) ho?c l?i (NG).\r\n\r\nNh?n di?n l?i ph? bi?n:\r\n\r\nSai màu (Color Mismatch)\r\n\r\nXu?c, v?t do (Scratch, Stain)\r\n\r\nSai biên d?ng/hình d?ng (Shape Mismatch)\r\n\r\nThi?u/du chi ti?t (Missing/Extra Region)"));
            Global.itemNews.Add(new ItemNew(TypeTool.EdgePixel, GroupTool.Basic_Tool, Properties.Resources.EdgePixel, Properties.Resources.contentEdgePixel, "Content.."));
            //Global.itemNews.Add(new ItemNew(TypeTool.MatchingShape, GroupTool.Basic_Tool, Properties.Resources.MatchingShape, Properties.Resources.contentMatchingShape, "?? Ð?i sánh hình d?ng\r\nNh?n di?n và phân tích hình dáng c?a v?t th? dù b? xoay, phóng to/thu nh? ho?c nhi?u n?n.\r\n?? Gi?i pháp m?nh cho ki?m tra ngo?i quan, phát hi?n sai hình d?ng, thi?u chi ti?t."));

            Global.itemNews.Add(new ItemNew(TypeTool.Pitch, GroupTool.Extra_Tool_1, Properties.Resources.Pitch, Properties.Resources.contentPitch, "Content.."));
            Global.itemNews.Add(new ItemNew(TypeTool.Width, GroupTool.Extra_Tool_1, Properties.Resources.Width, Properties.Resources.ContentWidth, "Content.."));
            Global.itemNews.Add(new ItemNew(TypeTool.Edge, GroupTool.Extra_Tool_1, Properties.Resources.Edge, Properties.Resources.contentEdge, "Content.."));
            Global.itemNews.Add(new ItemNew(TypeTool.Intersect, GroupTool.Extra_Tool_1, Properties.Resources.Intersect, Properties.Resources.contentInsert, "Content.."));

            Global.itemNews.Add(new ItemNew(TypeTool.Circle, GroupTool.Extra_Tool_1, Properties.Resources.Circle, Properties.Resources.contentCircle, "Content.."));
            Global.itemNews.Add(new ItemNew(TypeTool.Corner, GroupTool.Extra_Tool_1, Properties.Resources.Corner, Properties.Resources.Corner, "Content.."));
            Global.itemNews.Add(new ItemNew(TypeTool.Measure, GroupTool.Extra_Tool_1, Properties.Resources.Measure, Properties.Resources.Measure, "Content.."));
       
            Global.itemNews.Add(new ItemNew(TypeTool.Learning, GroupTool.Extra_Tool_2, Properties.Resources.Learning, Properties.Resources.Learning, "Content..",Global.IsLearning));
            Global.itemNews.Add(new ItemNew(TypeTool.MultiLearning, GroupTool.Extra_Tool_2, Properties.Resources.Learning, Properties.Resources.Learning, "Content.."));

            Global.itemNews.Add(new ItemNew(TypeTool.CraftOCR, GroupTool.Extra_Tool_2, Properties.Resources.OCR, Properties.Resources.contentOCR, "Content..",Global.IsOCR));

            Global.itemNews.Add(new ItemNew(TypeTool.OCR, GroupTool.Extra_Tool_2, Properties.Resources.OCR, Properties.Resources.contentOCR, "Content.."));
            bool IsQRCode = false;
            if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "\\ZXing.dll"))
                IsQRCode = true;
            Global.itemNews.Add(new ItemNew(TypeTool.BarCode, GroupTool.Extra_Tool_2, Properties.Resources.Barcode, Properties.Resources.Barcode, "Content..", IsQRCode));
            Global.itemNews.Add(new ItemNew(TypeTool.Crop, GroupTool.Extra_Tool_2, Properties.Resources.Crop, Properties.Resources.contentCrop, "?? C?t vùng quan sát\r\nL?a ch?n và c?t chính xác khu v?c c?n x? lý d? tang t?c d? tính toán và gi?m nhi?u.\r\n?? T?i uu hi?u nang và t?p trung vào di?m ki?m tra quan tr?ng."));
            Global.itemNews.Add(new ItemNew(TypeTool.AutoTrig, GroupTool.Extra_Tool_2, Properties.Resources.Crop, Properties.Resources.contentCrop, "Auto Trigger\r\n T?u tìm ?nh T?t nh?t"));

        }
        public static void ArrangeLogic()
        {
            Global.ParaCommon.indexLogic1 = new List<int>();
            Global.ParaCommon.indexLogic2 = new List<int>();
            Global.ParaCommon.indexLogic3 = new List<int>();
            Global.ParaCommon.indexLogic4 = new List<int>();
            Global.ParaCommon.indexLogic5 = new List<int>();
            Global.ParaCommon.indexLogic6 = new List<int>();
            foreach (PropetyTool propetyTool in BeeCore.Common.EnsureToolList(Global.IndexProgChoose))
            {
                if (propetyTool.IndexLogics[0] == true)
                    Global.ParaCommon.indexLogic1.Add(propetyTool.Propety2.Index);
                if (propetyTool.IndexLogics[1] == true)
                    Global.ParaCommon.indexLogic2.Add(propetyTool.Propety2.Index);
                if (propetyTool.IndexLogics[2] == true)
                    Global.ParaCommon.indexLogic3.Add(propetyTool.Propety2.Index);
                if (propetyTool.IndexLogics[3] == true)
                    Global.ParaCommon.indexLogic4.Add(propetyTool.Propety2.Index);
                if (propetyTool.IndexLogics[4] == true)
                    Global.ParaCommon.indexLogic5.Add(propetyTool.Propety2.Index);
                if (propetyTool.IndexLogics[5] == true)
                    Global.ParaCommon.indexLogic6.Add(propetyTool.Propety2.Index);

            }
        }
    }
}
