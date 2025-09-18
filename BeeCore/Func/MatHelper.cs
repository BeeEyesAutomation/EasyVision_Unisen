using System;
using OpenCvSharp;

public static class MatHelper
{
    /// <summary>
    /// Copy dữ liệu từ buffer unmanaged (uchar*) vào Mat đích an toàn.
    /// </summary>
    /// <param name="src">con trỏ buffer gốc (uchar*)</param>
    /// <param name="dst">Mat đích (đã có kích thước và type đúng)</param>
    /// <param name="rows">số dòng</param>
    /// <param name="cols">số cột</param>
    /// <param name="channels">số kênh (1 = mono, 3 = BGR)</param>
    public static unsafe void CopyToMat(IntPtr src, Mat dst, int rows, int cols, int channels)
    {
        if (src == IntPtr.Zero)
        {
            dst = new Mat();
            return;
        }
        if (dst == null || dst.Empty())
        {
            dst = new Mat();
            return;
        }

        int elemSize = dst.ElemSize();               // bytes per pixel
        long bytesPerRow = (long)cols * elemSize;
        long dstStep = (long)dst.Step();             // stride của Mat (có thể >= bytesPerRow)

        byte* pSrc = (byte*)src.ToPointer();
        byte* pDst = (byte*)dst.Data;

        if (dstStep == bytesPerRow)
        {
            // Trường hợp không có padding → copy một lần
            Buffer.MemoryCopy(pSrc, pDst, dstStep * rows, bytesPerRow * rows);
        }
        else
        {
            // Có padding → copy từng dòng
            for (int r = 0; r < rows; r++)
            {
                Buffer.MemoryCopy(pSrc + r * bytesPerRow,
                                  pDst + r * dstStep,
                                  dstStep,
                                  bytesPerRow);
            }
        }
    }
}
