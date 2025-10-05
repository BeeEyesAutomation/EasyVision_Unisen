using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using Control = System.Windows.Forms.Control;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace BeeCore.Funtion
{
  
        public class ShowTool
    {
       
        public static void ShowAllChart(TableLayoutPanel control)
        {
            control.SuspendLayout();
            int row = 0;
            control.RowStyles.Clear(); control.Controls.Clear();

            if (Global.ParaCommon.IsMultiCamera)
            {
                int i = 0;
                foreach (List<PropetyTool> ListPropety in BeeCore.Common.PropetyTools)
                {
                    if (ListPropety.Count() == 0)
                        continue;
                   // Global.ToolSettings.Labels[i].Font = new Font("Arial", 12, FontStyle.Regular);
                    if (BeeCore.Common.listCamera[0] == null)
                        Global.ToolSettings.Labels[i].lbGroup.Text = "Follow Chart " + i + " (No Camera)";
                    else
                    {
                        int Len = BeeCore.Common.listCamera[i].Para.Name.Length;
                        if (Len > 20)
                            Global.ToolSettings.Labels[i].lbGroup.Text = "Follow Chart " + i + " " + BeeCore.Common.listCamera[i].Para.Name.Substring(0, 20) + "...";
                        else
                            Global.ToolSettings.Labels[i].lbGroup.Text = "Follow Chart " + i + " " + BeeCore.Common.listCamera[i].Para.Name;
                    }
                    

                    Global.ToolSettings.Labels[i].Height = 40;

                    Global.ToolSettings.Labels[i].Dock = DockStyle.Fill;
                    Global.ToolSettings.Labels[i].Margin = new Padding(5, 15, 5,0);
                    control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                    control.RowCount = row + 1;
                    //  Global.ToolSettings.Labels[i].Margin = new Padding(2, 15, 2, 0);
                    control.Controls.Add(Global.ToolSettings.Labels[i], 0, row);
                    row++;
                    foreach (PropetyTool tool in ListPropety)
                    {
                        control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50));
                        control.RowCount = row + 1;
                        tool.ItemTool.Dock = DockStyle.Fill;
                        tool.ItemTool.Margin = new Padding(5, 0, 10, 5);
                        control.Controls.Add(tool.ItemTool, 0, row);
                        row++;
                    }
                    i++;
                  
                }
                control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50));
                control.RowCount = row + 1;
            }
            else
            {
               
                for (int i = 0; i < Global.ParaCommon.NumTrig; i++)
                {
                    foreach (PropetyTool tool in BeeCore.Common.PropetyTools[0])
                    {
                        switch (i)
                        {
                            case 1:
                              
                              
                                tool.ItemTool2 = tool.ItemTool.Clone();
                              
                                tool.ItemTool2.TriggerNum = TriggerNum.Trigger2;
                                tool.ItemTool2.NotChange = true;
                                break;
                            case 2:
                              
                                tool.ItemTool3 = tool.ItemTool.Clone();
                                tool.ItemTool3.TriggerNum = TriggerNum.Trigger3;
                                tool.ItemTool3.NotChange = true;
                                break;
                            case 3:
                               
                                tool.ItemTool4 = tool.ItemTool.Clone();
                                tool.ItemTool4.TriggerNum = TriggerNum.Trigger4;
                                tool.ItemTool4.NotChange = true;
                                break;
                           
                        }
                    }
                }
             
                for (int i = 0; i < Global.ParaCommon.NumTrig; i++)
                {

                    if (Global.ParaCommon.NumTrig > 1 || Global.ParaCommon.IsMultiCamera)
                    {


                        if (BeeCore.Common.listCamera[0] == null)
                            Global.ToolSettings.Labels[i].Text = "Follow Chart " + i + " (No Camera)";
                        else
                        {
                            int Len = BeeCore.Common.listCamera[0].Para.Name.Length;
                            if (Len > 20)
                                Global.ToolSettings.Labels[i].lbGroup.Text = "Follow Chart " + i + " " + BeeCore.Common.listCamera[0].Para.Name.Substring(0, 20) + "...";
                            else
                                Global.ToolSettings.Labels[i].lbGroup.Text = "Follow Chart " + i + " " + BeeCore.Common.listCamera[0].Para.Name;
                        }


                        Global.ToolSettings.Labels[i].Height = 40;

                        Global.ToolSettings.Labels[i].Dock = DockStyle.Fill;
                        Global.ToolSettings.Labels[i].Margin = new Padding(5, 15, 5, 0);
                        control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                        control.RowCount = row + 1;
                        //  Global.ToolSettings.Labels[i].Margin = new Padding(2, 15, 2, 0);
                        control.Controls.Add(Global.ToolSettings.Labels[i], 0, row);
                        row++;
                    }
                    foreach (PropetyTool tool in BeeCore.Common.PropetyTools[0])
                    {
                        switch (i)
                        {
                            case 0:
                               
                    
                                control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                                control.RowCount = row + 1;
                                tool.ItemTool.Dock=DockStyle.Fill;
                                tool.ItemTool.Margin = new Padding(5, 0, 10, 5);
                                control.Controls.Add(tool.ItemTool,0, row);
                                break;
                            case 1:
                                control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                                control.RowCount = row + 1;
                                tool.ItemTool2.Dock = DockStyle.Fill;
                                tool.ItemTool2.Margin = new Padding(5, 0, 10, 5);
                                control.Controls.Add(tool.ItemTool2, 0, row);
                               
                                break;
                            case 2:
                                control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                                control.RowCount = row + 1;
                                tool.ItemTool3.Dock = DockStyle.Fill;
                                tool.ItemTool3.Margin = new Padding(5, 0, 10, 5);
                                control.Controls.Add(tool.ItemTool3, 0, row);
                                break;
                            case 3:

                                control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                                control.RowCount = row + 1;
                                tool.ItemTool4.Dock = DockStyle.Fill;
                                tool.ItemTool4.Margin = new Padding(5, 0, 5, 5);
                                control.Controls.Add(tool.ItemTool4, 0, row);
                                break;

                        }
                        row++;
                      }
                      
                  }

                control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute,50));
                control.RowCount = row + 1;

            }    

                control.ResumeLayout(true);
        }
        public static  void ShowChart(TableLayoutPanel control, List< PropetyTool> propetyTool)
        {
            int row = 0;
            control.RowStyles.Clear();
            control.Controls.Clear();

            foreach ( PropetyTool tool in propetyTool )
            {
                control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                control.RowCount = row + 1;
                tool.ItemTool.Dock = DockStyle.Fill;
                tool.ItemTool.Margin = new Padding(5, 0, 10, 5);
                control.Controls.Add(tool.ItemTool, 0, row);
                row++;
            }
            control.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50));
            control.RowCount = row + 1;
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
                //case TypeImg.Result:
                //    bmShow = BeeCore.Common.bmResult;
                //    break;
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
            Global.ZoomMinimum = (int)(Scale * 100.0);
            image.Zoom =(int) Global.ZoomMinimum;

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
            Global.ZoomMinimum = (int)(Scale * 100.0);
            image.Zoom = (int)Global.ZoomMinimum;
            image.Invalidate();
        }
    }
    }

