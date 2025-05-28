// ImageStore.cpp
#include "ImageStore.h"

void ImageStore::AddImage(const std::string& name, const cv::Mat& image) {
    images[name] = image.clone();  // clone để đảm bảo ảnh không bị hỏng dữ liệu
}

cv::Mat ImageStore::GetImage(const std::string& name) {
    return images.at(name);  // sẽ throw nếu không tồn tại
}

std::vector<std::string> ImageStore::GetAllNames() {
    std::vector<std::string> names;
    for (const auto& kv : images) {
        names.push_back(kv.first);
    }
    return names;
}
