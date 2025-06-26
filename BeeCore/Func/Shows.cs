using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Size = System.Drawing.Size;

namespace BeeCore.Funtion
{
  
        public class Shows
        {
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

