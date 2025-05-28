#pragma once
// ImageStore.h
#pragma once
#include <opencv2/opencv.hpp>
#include <map>
#include <string>

class ImageStore {
private:
    std::map<std::string, cv::Mat> images;
public:
    void AddImage(const std::string& name, const cv::Mat& image);
    cv::Mat GetImage(const std::string& name);
    std::vector<std::string> GetAllNames();
};

