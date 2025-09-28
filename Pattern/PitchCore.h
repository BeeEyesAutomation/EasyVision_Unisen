#pragma once
#include <vector>
#include <string>
#include <opencv2/opencv.hpp>

namespace BeeCpp {

    enum class ScanAxis { X = 0, Y = 1 };

    struct Peak {
        int x = 0, y = 0;     // toạ độ pixel (ảnh)
        double v = 0.0;       // giá trị tín hiệu 1D tại peak (debug)
    };

    struct PitchResultCore {
        int status = 0;
        std::string message;

        // ===== Peaks (pixel) =====
        std::vector<Peak> crests;
        std::vector<Peak> roots;

        // ===== Pitch / Height theo PX (nội bộ dùng; không expose ra C#) =====
        std::vector<double> pitches;        // crest pitch (px)
        std::vector<double> pitches_root;   // root pitch  (px)

        std::vector<int>    crest_cl_x;     // X pixel tại trung trục đúng X/Y peak
        std::vector<double> crest_cl_y;     // Y trung trục (ScanAxis::X) hoặc y=peak (ScanAxis::Y)
        std::vector<int>    root_cl_x;
        std::vector<double> root_cl_y;

        std::vector<double> crest_heights;  // (px) dọc hoặc ngang tùy trục
        std::vector<double> root_heights;   // (px)

        // ===== Tỉ lệ đơn vị =====
        double scale_mm_per_px = 1.0;

        // ===== Pitch / Height theo MM (để expose) =====
        std::vector<double> pitches_mm;
        std::vector<double> pitches_root_mm;
        std::vector<double> crest_heights_mm;
        std::vector<double> root_heights_mm;

        // ===== Statistics theo MM (để expose) =====
        double pitch_mean_mm = 0, pitch_median_mm = 0, pitch_min_mm = 0, pitch_max_mm = 0, pitch_std_mm = 0;
        double pitch_root_mean_mm = 0, pitch_root_median_mm = 0, pitch_root_min_mm = 0, pitch_root_max_mm = 0, pitch_root_std_mm = 0;

        double crest_h_mean_mm = 0, crest_h_med_mm = 0, crest_h_min_mm = 0, crest_h_max_mm = 0, crest_h_std_mm = 0;
        double root_h_mean_mm = 0, root_h_med_mm = 0, root_h_min_mm = 0, root_h_max_mm = 0, root_h_std_mm = 0;

        // (debug)
        std::vector<Peak> rejected_crests;

        int usedMinPeakDist = 0;
        double usedMinProm = 0;

        cv::Mat debugBGR;   // ảnh debug (BGR8)
    };

    class PitchCore {
    public:
        // Ảnh input là ảnh cạnh (CV_8UC1)
        void setImage(const cv::Mat& edgeGray);

        // Bỏ biên theo X/Y (pixel)
        void setMargins(int marginX, int marginY);

        // Promote crest bị reject nếu xa đỉnh hợp lệ gần nhất >= minNeighborDist
        void setRejectedPolicy(bool enablePromote, int minNeighborDist);

        // Chọn trục quét
        void setScanAxis(ScanAxis axis) { _axis = axis; }

        // Đặt sigma Gaussian: signalSigma (cho tín hiệu), centerlineFallbackSigma (cho trung trục khi thiếu vỏ)
        void setGaussianSigma(double signalSigma, double centerlineFallbackSigma = -1.0);

        // Đặt tỉ lệ đổi đơn vị mm/px
        void setScaleMmPerPx(double mmPerPx);

        // Thực thi đo
        PitchResultCore measure();

    private:
        cv::Mat _edgeGray;         // CV_8UC1
        int _marginX = 0;
        int _marginY = 0;
        bool _promoteRejected = false;
        int _rejMinNeighborDist = 0;
        ScanAxis _axis = ScanAxis::X;

        double _sigmaSignal = 5.0;
        double _sigmaCenterFallback = 25.0;

        double _mmPerPx = 1.0;

        // ===== Helpers =====
        static std::vector<double> gaussianSmooth(const std::vector<double>& s, double sigma);
        static void robustStats(const std::vector<double>& v, double& mean, double& med, double& mn, double& mx, double& stdev);
        static void autoTune(const std::vector<double>& s, int& mpd, double& prom);
        static std::vector<int> findPeaks1D(const std::vector<double>& s, int minDist, double minProm, std::vector<int>& outRejected);
        static void linearInterpolateInplace(std::vector<double>& arr);
        static std::vector<double> buildCenterline(const std::vector<double>& ys,
            const std::vector<int>& crestIdx,
            const std::vector<int>& rootIdx,
            double fallbackSigma);
    };

} // namespace BeeCpp
