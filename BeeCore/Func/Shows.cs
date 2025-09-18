using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using static CvPlus.s_BlockMax;
using Control = System.Windows.Forms.Control;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace BeeCore.Funtion
{
  
        public class Shows
    {
       
        public static void ShowAllChart(Control control)
        {
            control.Controls.Clear();
            int y = 5;
            int index = 0;
            foreach (List<PropetyTool> ListPropety in BeeCore.Common.PropetyTools)
            {
                if (ListPropety.Count() == 0) 
                    continue;
                Label label = new Label();
                label.Font=new Font("Arial",12,FontStyle.Bold);
                if (BeeCore.Common.listCamera[index] == null)
                    label.Text = "Follow Chart(Null)";
                else
                    label.Text = "Follow Chart(" + BeeCore.Common.listCamera[index].Para.Name.Split('(')[0] + ")";
                label.Location = new Point(3, y);
                label.AutoSize = false;
                label.Width= Global.ToolSettings.Width - 10;
                control.Controls.Add(label);
                y += 25;
                foreach (PropetyTool tool in ListPropety)
                {
                    tool.ItemTool.Location = new Point(10, y);
                    y += tool.ItemTool.Height + 5;
                    tool.ItemTool.Width = Global.ToolSettings.Width - 10;
                    control.Controls.Add(tool.ItemTool);
                }
                index++;
                if (Global.ParaCommon.IsMultiCamera == false)
                    break;
            }
            control.ResumeLayout(true);
        }
        public static  void ShowChart(  Control control,List< PropetyTool> propetyTool)
        {
            control.Controls.Clear();
            int y = 10;
            foreach ( PropetyTool tool in propetyTool )
            {
                tool.ItemTool.Location = new Point(10,y);
                y += tool.ItemTool.Height+5;
                tool.ItemTool.Width = Global.ToolSettings.Width - 10;
                control. Controls.Add(tool.ItemTool);

            }
            control.ResumeLayout(true);
        }
        public static Bitmap bmShow;
         public static void RefreshImg(Cyotek.Windows.Forms.ImageBox image, Mat matRaw, TypeImg typeImg = TypeImg.Raw)
{
    image.Invoke((Action)(() =>
    {
        Bitmap bmShow = null;
        
        try
        {
            // Giải phóng ảnh cũ nếu có
            if (image.Image != null)
            {
                image.Image.Dispose();
                image.Image = null;
            }

            // Chọn ảnh để hiển thị
            switch (typeImg)
            {
                case TypeImg.Raw:
                    bmShow =matRaw.ToBitmap();
                    break;
                case TypeImg.Result:
                    bmShow = BeeCore.Common.bmResult;
                    break;
            }
            using (Bitmap bm = new Bitmap(bmShow))
            {
                image.Image = new Bitmap(bm);

            }
                // Tạo bản sao của ảnh thông qua MemoryStream an toàn
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    bmShow.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //    ms.Seek(0, SeekOrigin.Begin);

                //    using (var tempImage = Image.FromStream(ms))
                //    {
                //        // Clone để ảnh không giữ tham chiếu tới stream
                //        bmpFinal = new Bitmap(tempImage);
                //    }
                //}

             
           
             // GC định kỳ sau mỗi 60 frame (nếu fps cao)
             frameCounter++;
            if (frameCounter % 2== 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi: " + ex.Message);

            if (ex.Message.Contains("GDI"))
            {
                var newImageBox = new Cyotek.Windows.Forms.ImageBox
                {
                    Size = new System.Drawing.Size(image.Parent.Width, image.Parent.Height),
                    Parent = image.Parent
                };

                image = newImageBox;
            }


        }
        finally
        {
            // Giải phóng bmShow nếu là ảnh tạm
            if (bmShow != null )
            {
                bmShow.Dispose();
            }

            // Nếu bmpFinal không được gán vào image → giải phóng
          
        }
    }));        }
        private static int frameCounter=0;
        public static Cyotek.Windows.Forms.ImageBox imgTemp;
        public static void Full(Cyotek.Windows.Forms.ImageBox image,Size szImg)
            {
            if (image == null) return;
            imgTemp= image;
               
                int withBox = image.Width;
                int heightBox = image.Height;
                float ScaleW = (float)(withBox * 1.0 / szImg.Width);
                float ScaleH = (float)(heightBox * 1.0 / szImg.Height);
                float Scale = Math.Min(ScaleW, ScaleH);
            
                image.Zoom = (int)(Scale * 100.0);

                image.Invalidate();
            }
        public static void Full(Cyotek.Windows.Forms.ImageBox image, OpenCvSharp.Size szImg)
        {
            if (image == null) return;
            imgTemp = image;

            int withBox = image.Width;
            int heightBox = image.Height;
            float ScaleW = (float)(withBox * 1.0 / szImg.Width);
            float ScaleH = (float)(heightBox * 1.0 / szImg.Height);
            float Scale = Math.Min(ScaleW, ScaleH);

            image.Zoom = (int)(Scale * 100.0);

            image.Invalidate();
        }
    }
    }

