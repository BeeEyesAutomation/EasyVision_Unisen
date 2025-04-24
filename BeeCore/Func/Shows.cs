using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore.Funtion
{
  
        public class Shows
        {
            public static Bitmap bmShow;
            public static void RefreshImg(Cyotek.Windows.Forms.ImageBox image, TypeImg typeImg = TypeImg.Raw)
            {
                image.Invoke((Action)(() =>
                {
                    Bitmap bmShow = null;

                    if (image.Image != null)
                    {
                        image.Image.Dispose(); // Giải phóng ảnh cũ
                    }
                    try
                    {
                        switch (typeImg)
                        {
                            case TypeImg.Raw:
                                //if (G.Config.TypeCamera == TypeCamera.TinyIV)
                                //    bmShow = BeeCore.Common.bmRaw;
                                //else
                                bmShow = BeeCore.Common.matRaw.Clone().ToBitmap();
                                break;
                                //case TypeImg.Result:
                                //    bmShow = BeeCore.Common.mat.ToBitmap();
                                //    break;
                                //case TypeImg.Crop:
                                //    GetCrop(ref rows, ref cols, ref Type);
                                //    break;
                        }
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bmShow.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            ms.Seek(0, SeekOrigin.Begin); // Đặt lại vị trí đầu stream

                            // Load hình ảnh từ MemoryStream vào PictureBox
                            image.Image = Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("GDI"))
                        {
                            image = new Cyotek.Windows.Forms.ImageBox();

                            image.Size = new System.Drawing.Size(image.Parent.Width, image.Parent.Height);

                            image.Parent = image.Parent;

                        }

                    }
                    finally
                    {

                    }

                }));
            }
        public static Cyotek.Windows.Forms.ImageBox imgTemp;
            public static void Full(Cyotek.Windows.Forms.ImageBox image)
            {
            if (image == null) return;
            imgTemp= image;
                int wRaw = BeeCore.G.ParaCam.SizeCCD.Width;
                int hRaw = BeeCore.G.ParaCam.SizeCCD.Height;
                int withBox = image.Width;
                int heightBox = image.Height;
                float ScaleW = (float)(withBox * 1.0 / wRaw);
                float ScaleH = (float)(heightBox * 1.0 / hRaw);
                float Scale = Math.Min(ScaleW, ScaleH);
            
                image.Zoom = (int)(Scale * 100.0);

                image.Invalidate();
            }
        }
    }

