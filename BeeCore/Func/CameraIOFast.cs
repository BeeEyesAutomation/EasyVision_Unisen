using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore.Func
{
    using BeeGlobal;
    using OpenCvSharp;
    using System;

    public static class CameraIOFast
    {
        /// <summary>
        /// Copy trực tiếp từ buffer native vào matRaw (không tạo Mat trung gian).
        /// Dùng khi bạn CHẮC CHẮN buffer liên tục (no padding/stride).
        /// </summary>
        public static unsafe bool TryGrabFast_NoStride(ref Mat matRaw)
        {
            
          

            IntPtr intPtr = IntPtr.Zero;
            int rows = 0, cols = 0;
            int matType = MatType.CV_8UC1;

            try
            {
                intPtr = Native.GetRaw(ref rows, ref cols, ref matType);
                if (intPtr == IntPtr.Zero || rows <= 0 || cols <= 0)
                    return false;

                // Allocate/reuse destination Mat
                
                if (matRaw == null || matRaw.Rows != rows || matRaw.Cols != cols || matRaw.Type() != matType)
                {
                    matRaw?.Dispose();
                    matRaw = new Mat(rows, cols, matType);
                }

                byte* src = (byte*)intPtr;
                byte* dst = (byte*)matRaw.Data;

                int elem = (int)matRaw.ElemSize();
                long bytesPerRow = (long)cols * elem;
                long dstStep = (long)matRaw.Step();      // có thể >= bytesPerRow do alignment

                // Copy từng dòng để an toàn với step của đích
                long copyBytes = Math.Min(bytesPerRow, dstStep);
                for (int r = 0; r < rows; r++)
                {
                    Buffer.MemoryCopy(src + r * bytesPerRow,
                                      dst + r * dstStep,
                                      dstStep,
                                      copyBytes);
                }

               
                return true;
            }
            catch(Exception ex)
            {
                Global.Ex ="CAMERAIOFAST_" +ex.Message;
                return false;
            }
            finally
            {
                if (intPtr != IntPtr.Zero)
                    Native.FreeBuffer(intPtr);
            }
        }
        public static unsafe bool TryGrabFast_WithStride(ref Mat matRaw)
        {
            

            IntPtr intPtr = IntPtr.Zero;
            int rows = 0, cols = 0, strideBytes = 0;
            int matType = MatType.CV_8UC1;

            try
            {
               // intPtr = Native.GetRawWithStride(ref rows, ref cols, ref matType, ref strideBytes);
                if (intPtr == IntPtr.Zero || rows <= 0 || cols <= 0 || strideBytes <= 0)
                    return false;

                if (matRaw == null || matRaw.Rows != rows || matRaw.Cols != cols || matRaw.Type() != matType)
                {
                    matRaw?.Dispose();
                    matRaw = new Mat(rows, cols, matType);
                }

                byte* src = (byte*)intPtr;
                byte* dst = (byte*)matRaw.Data;

                long dstStep = (long)matRaw.Step();
                int elem = (int)matRaw.ElemSize();
                long bytesPerRowNoPad = (long)cols * elem;

                // số byte thực sự cần copy mỗi dòng là min(nguồn, đích, bytesPerRowNoPad)
                long copyBytes = Math.Min(bytesPerRowNoPad, Math.Min(strideBytes, dstStep));

                for (int r = 0; r < rows; r++)
                {
                    Buffer.MemoryCopy(src + (long)r * strideBytes,
                                      dst + (long)r * dstStep,
                                      dstStep,
                                      copyBytes);
                }

                
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (intPtr != IntPtr.Zero)
                    Native.FreeBuffer(intPtr);
            }
        }

    }

}
