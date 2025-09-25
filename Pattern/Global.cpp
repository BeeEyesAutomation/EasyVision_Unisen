#include "Global.h"
#include <vector>
#include <cstring>
#include <atomic>
using namespace BeeCpp;
using namespace cv;
cv::Mat Common::RotateMat(const cv::Mat& raw, const cv::RotatedRect& rot)
{
    Mat matRs, matR = getRotationMatrix2D(rot.center, rot.angle, 1);
    float fTranslationX = (rot.size.width - 1) / 2.0f - rot.center.x;
    float fTranslationY = (rot.size.height - 1) / 2.0f - rot.center.y;
    matR.at<double>(0, 2) += fTranslationX;
    matR.at<double>(1, 2) += fTranslationY;
    warpAffine(raw, matRs, matR, rot.size, INTER_LINEAR, BORDER_CONSTANT);
    return matRs;
}
