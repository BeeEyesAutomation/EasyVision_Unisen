#include <opencv2/opencv.hpp>
#include <opencv2/imgproc/imgproc_c.h>
#using <System.Windows.Forms.dll> 
#using <System.Drawing.dll> 
using namespace System;

namespace BeeCpp
{
    class Common {
    public:
      static  cv::Mat RotateMat(const cv::Mat& raw, const cv::RotatedRect& rot);


          
    };
}