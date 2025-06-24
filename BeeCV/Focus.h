#pragma once
#include "G.h"
#include <iostream>
#include <vector>
#include <cmath>
#include <algorithm>
#include <fstream>
#include <string>
#include <opencv2/opencv.hpp>
#include <opencv2/imgproc.hpp>
#include <chrono>
#include <omp.h>  // Thêm thư viện OpenMP để hỗ trợ đa luồng
namespace CvPlus
{
    class Def
    {
    protected:
        int k;
        std::vector<int> height_list;
        std::vector<double> height_listmag_list;
        int x;
    public:

       Def(): k(700), x(10) {}
       void findSharpestImage();
       double calculateSharpness(const cv::Mat& img);
    };
    ref class Focus
    {
  //  public: double SetTemp();
    public: double Get();
    };
}
