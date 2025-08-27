using BeeGlobal;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
  
    public class Native
    {
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        unsafe public static extern IntPtr GetRaw(ref int rows, ref int cols, ref int Type);
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        unsafe public static extern IntPtr GetCrop(ref int rows, ref int cols, ref int Type);
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        unsafe public static extern IntPtr GetResult(ref int rows, ref int cols, ref int Type);
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        unsafe public static extern void SetRaw(IntPtr data, int image_rows, int image_cols, MatType matType);
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        unsafe public static extern void SetImgTemp(IntPtr data, int image_rows, int image_cols, MatType matType);
        [DllImport(@".\BeeCV.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        unsafe public static extern void FreeBuffer(IntPtr ptr);
        public  IntPtr intPtr;
        public  Mat GetImg( TypeImg typeImg=TypeImg.Raw)
        {

            int rows = 0, cols = 0, Type = 0;
            IntPtr intPtr = new IntPtr();
            Mat raw  = new Mat();
            try
            {

               
                    switch (typeImg)
                    {
                        case TypeImg.Raw:
                            intPtr= GetRaw(ref rows, ref cols, ref Type);
                            break;
                        case TypeImg.Result:
                            intPtr= GetResult(ref rows, ref cols, ref Type);
                            break;
                        case TypeImg.Crop:
                            intPtr= GetCrop(ref rows, ref cols, ref Type);
                            break;
                    }
                     raw = new Mat(rows, cols, Type, intPtr);
                    return raw.Clone();
                
            }
         
            finally
            {
                raw.Release();
                // Giải phóng bộ nhớ sau khi sử dụng
                //Marshal.FreeHGlobal(intPtr);
            }
        }
        public  bool SetImg(Mat mat, TypeImg typeImg = TypeImg.Raw)
        {if(mat == null)
                return false;
         if(mat.Empty()) return false;
            Mat raw = mat;

            try
            {
                
                unsafe
                {
                    switch (typeImg)
                    {
                        case TypeImg.Raw:

                            SetRaw(raw.Data, raw.Rows, raw.Cols, raw.Type());
                            break;
                        case TypeImg.Crop:
                            SetImgTemp(raw.Data, raw.Rows, raw.Cols, raw.Type());
                            break;
                    }                  
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                // Giải phóng bộ nhớ sau khi sử dụng
                Marshal.FreeHGlobal(intPtr);
            }
        }


    }
}
