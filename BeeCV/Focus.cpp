#include "Focus.h"
using namespace CvPlus;
   // Helper function for simple interpolation
   std::vector<double> makeInterpSpline(const std::vector<int>& x_values,
   const std::vector<double>& y_values,
    const std::vector<double>& xnew) {
        std::vector<double> result(xnew.size(), 0.0);
        // Simple linear interpolation
        for (size_t i = 0; i < xnew.size(); i++) {
            double x_target = xnew[i];
            // Find the two nearest points
            size_t left_idx = 0;
            while (left_idx < x_values.size() - 1 && x_values[left_idx + 1] < x_target) {
                left_idx++;
            }
            if (left_idx == x_values.size() - 1) {
                // Extrapolation at the right end
                result[i] = y_values[left_idx];
            }
            else if (x_target <= x_values[0]) {
                // Extrapolation at the left end
                result[i] = y_values[0];
            }
            else {
                // Linear interpolation between two points
                double x_left = x_values[left_idx];
                double x_right = x_values[left_idx + 1];
                double y_left = y_values[left_idx];
                double y_right = y_values[left_idx + 1];

                double t = (x_target - x_left) / (x_right - x_left);
                result[i] = y_left + t * (y_right - y_left);
            }
        }

        return result;
    }
    // Calculate the sharpness score for a single image
    double Def:: calculateSharpness(const cv::Mat& img) {
        cv::resize(img, img, cv::Size(), 0.5, 0.5);
        // Apply Sobel operators
        cv::Mat sobelx, sobely, gradient_magnitude;
        cv::Sobel(img, sobelx, CV_64F, 1, 0);
        cv::Sobel(img, sobely, CV_64F, 0, 1);
        // Calculate gradient magnitude
        cv::magnitude(sobelx, sobely, gradient_magnitude);
        // Convert to vector for easier processing
        std::vector<double> gmag;
        for (int r = 0; r < gradient_magnitude.rows; r++) {
            for (int c = 0; c < gradient_magnitude.cols; c++) {
                double val = gradient_magnitude.at<double>(r, c);
                if (val != 0) {
                    gmag.push_back(val);
                }
            }
        }
        // Calculate median
        size_t size = gmag.size();
        if (size == 0) {
            std::cerr << "Warning: No non-zero gradient values in image" << std::endl;
            return 0.0;
        }
        std::sort(gmag.begin(), gmag.end());
        double median;
        if (size % 2 == 0) {
            median = (gmag[size / 2 - 1] + gmag[size / 2]) / 2.0;
        }
        else {
            median = gmag[size / 2];
        }
        // Filter values below threshold
        double threshold = median * x;
        std::vector<double> filtered_gmag;
        for (double val : gmag) {
            if (val > threshold) {
                filtered_gmag.push_back(val);
            }
        }

        // Calculate sum
        double sum = 0.0;
        for (double val : filtered_gmag) {
            sum += val;
        }

        return sum;
    }


    // Export results to CSV
    void exportResults(const std::string& filepath,
        const std::vector<std::string>& imagePaths,
        const std::vector<double>& scores) {
        std::ofstream outfile(filepath);
        outfile << "ImagePath,SharpnessScore\n";
        for (size_t i = 0; i < imagePaths.size(); i++) {
            outfile << imagePaths[i] << "," << scores[i] << "\n";
        }
        outfile.close();
        std::cout << "Results exported to " << filepath << std::endl;
    }

    double Focus::Get() {
    try {
        // Create AutoFocus instance
        Def focuser;

        //// Define paths to test images
        //std::vector<std::string> imagePaths;
        //imagePaths.push_back("C:\\Users\\TEAM\\Desktop\\3.bmp");
        //imagePaths.push_back("C:\\Users\\TEAM\\Desktop\\OK.jpg");
        //imagePaths.push_back("C:\\Users\\TEAM\\Desktop\\NG.jpg");
        //imagePaths.push_back("C:\\Users\\TEAM\\Desktop\\1.bmp");
        //imagePaths.push_back("C:\\Users\\TEAM\\Desktop\\2.bmp");
        //imagePaths.push_back("C:\\Users\\TEAM\\Desktop\\3.bmp");

        //std::cout << "Starting image sharpness analysis..." << std::endl;

        // Bắt đầu đo thời gian
        auto start = std::chrono::high_resolution_clock::now();

        //// Find the sharpest image
        //std::string sharpestImage = focuser.findSharpestImage();

        //// Kết thúc đo thời gian
        //auto end = std::chrono::high_resolution_clock::now();
        //std::chrono::duration<double> elapsed = end - start;

        //std::cout << "Time taken to find the sharpest image: " << elapsed.count() << " seconds" << std::endl;

        // Get all scores for export
    //  double scores = focuser.calculateSharpness(matRaw);
      return focuser.calculateSharpness(matRaw);
        // Export results
       // focuser.exportResults("C:\\sharpness_results.csv", imagePaths, scores);

    }
    catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << std::endl;
        return 1;
    }
    catch (...) {
        std::cerr << "Unknown error occurred" << std::endl;
        return 1;
    }

    return 0;
}