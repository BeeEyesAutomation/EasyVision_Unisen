
using System;
using System.Collections.Generic;
using OpenCvSharp;
using OpenCvSharp.Extensions;   // nếu cần Bitmap <‑> Mat


namespace BeeCore.Algorithm
{
  
    // ❶ Định nghĩa delegate cho một filter: (src → dst)
    public delegate void ImageFilter(Mat src, Mat dst);

    public sealed class ImagePreprocessPipeline : IDisposable
    {
        

        private readonly List<ImageFilter> _filters = new List<ImageFilter>();

        /// <summary>Thêm filter vào pipeline (chainable).</summary>
        public ImagePreprocessPipeline Add(ImageFilter filter)
        {
            _filters.Add(filter);
            return this;
        }

        /// <summary>Chạy toàn bộ pipeline trên ảnh đầu vào.</summary>
        public Mat Apply(Mat input)
        {
            if (input == null || input.Empty())
                throw new ArgumentException("Input Mat không hợp lệ!");

            // clone để không làm thay đổi ảnh gốc
            Mat current = input.Clone();

            foreach (var f in _filters)
            {
                var next = new Mat();
                f(current, next);
                current.Dispose();   // giải phóng Mat cũ
                current = next;
            }
            return current;          // caller phải Dispose()
        }

        public void Dispose()
        {
            // không giữ tài nguyên riêng, nhưng giữ interface IDisposable
        }
    }

}
