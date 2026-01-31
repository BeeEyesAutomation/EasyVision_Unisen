#include "MonoSegCore.h"
using namespace cv;
using namespace std;

// ==================== AUTO THRESHOLD ====================
int MonoSegCore::AutoThresholdHistogramValley(const Mat& score8U)
{
    int histSize = 256;
    float range[] = { 0,256 };
    const float* ranges[] = { range };
    Mat hist;
    calcHist(&score8U, 1, 0, Mat(), hist, 1, &histSize, ranges);
    GaussianBlur(hist, hist, Size(1, 9), 0);

    int p1 = 0, p2 = 0;
    for (int i = 0; i < 256; i++) if (hist.at<float>(i) > hist.at<float>(p1)) p1 = i;
    for (int i = 0; i < 256; i++) if (i != p1 && hist.at<float>(i) > hist.at<float>(p2)) p2 = i;
    if (p1 > p2) swap(p1, p2);

    int valley = p1;
    float minv = hist.at<float>(p1);
    for (int i = p1; i <= p2; i++) {
        float v = hist.at<float>(i);
        if (v < minv) { minv = v; valley = i; }
    }
    return valley;
}
float MonoSegCore::MedianInplace(std::vector<float>& v)
{
    if (v.empty()) return 0.f;
    size_t m = v.size() / 2;
    nth_element(v.begin(), v.begin() + m, v.end());
    return v[m];
}
RotatedRect MonoSegCore::MinAreaRect_GlobalFromROI(const Mat& roiMask, const Rect& roiBox)
{
    vector<vector<Point>> cnts;
    findContours(roiMask, cnts, RETR_EXTERNAL, CHAIN_APPROX_SIMPLE);
    if (cnts.empty()) return RotatedRect();

    RotatedRect rr = minAreaRect(cnts[0]);
    // rr đang ở tọa độ ROI -> offset về global
    rr.center.x += (float)roiBox.x;
    rr.center.y += (float)roiBox.y;

    // normalize: height >= width để angle ổn định
    if (rr.size.width > rr.size.height)
    {
        swap(rr.size.width, rr.size.height);
        rr.angle += 90.f;
    }
    return rr;
}

void MonoSegCore::DrawBoxesToColorImage(
    const Mat& base8U,
    const vector<Rect>& boxes,
    Mat& outBGR,
    Scalar boxColor,
    int thickness
)
{
    CV_Assert(base8U.type() == CV_8UC1);

    cvtColor(base8U, outBGR, COLOR_GRAY2BGR);

    for (const auto& b : boxes)
        rectangle(outBGR, b, boxColor, thickness);
}

void MonoSegCore::DrawRectRotatesToColorImage(
    const Mat& base8U,
    const vector<RotatedRect>& rects,
    const vector<uint8_t>& isPaper,
    Mat& outBGR,
    int thickness
)
{
    CV_Assert(base8U.type() == CV_8UC1);
    cvtColor(base8U, outBGR, COLOR_GRAY2BGR);

    size_t n = rects.size();
    for (size_t i = 0; i < n; i++)
    {
        Scalar col = Scalar(0, 255, 0); // chip xanh
        if (i < isPaper.size() && isPaper[i]) col = Scalar(0, 0, 255); // paper đỏ

        Point2f pts[4];
        rects[i].points(pts);
        for (int k = 0; k < 4; k++)
            line(outBGR, pts[k], pts[(k + 1) & 3], col, thickness);
    }
}

// ==================== SEGMENT MONO ====================
int MonoSegCore::SegmentLowContrastMono(
    const Mat& gray8U,
    Mat& outMask8U,
    const MonoSegParams& pin,
    Mat* outScore
)
{
    CV_Assert(gray8U.type() == CV_8UC1);

    MonoSegParams p = pin;
    p.bgBlurK = MakeOdd(max(3, p.bgBlurK));
    p.blackHatK = MakeOdd(max(3, p.blackHatK));

    Mat score;

    if (p.useBlackHat)
    {
        Mat k = getStructuringElement(MORPH_RECT, Size(p.blackHatK, p.blackHatK));
        morphologyEx(gray8U, score, MORPH_BLACKHAT, k);
    }
    else
    {
        Mat bg;
        blur(gray8U, bg, Size(p.bgBlurK, p.bgBlurK));
        if (p.mode == 0) subtract(bg, gray8U, score);
        else             subtract(gray8U, bg, score);
    }

    normalize(score, score, 0, 255, NORM_MINMAX);
    score.convertTo(score, CV_8U);

    int thr = AutoThresholdHistogramValley(score);
    threshold(score, outMask8U, thr, 255, THRESH_BINARY);

    morphologyEx(outMask8U, outMask8U, MORPH_OPEN,
        getStructuringElement(MORPH_RECT, Size(p.openK, p.openK)));
    morphologyEx(outMask8U, outMask8U, MORPH_CLOSE,
        getStructuringElement(MORPH_RECT, Size(p.closeK, p.closeK)));

    if (outScore) *outScore = score;
    return countNonZero(outMask8U);
}
static void SplitVerticalBlobs(
    const Mat& inMask,
    Mat& outMask,
    int minGap
)
{
    outMask = Mat::zeros(inMask.size(), CV_8UC1);

    Mat labels, stats, cents;
    int n = connectedComponentsWithStats(
        inMask, labels, stats, cents
    );

    for (int i = 1; i < n; i++)
    {
        int x = stats.at<int>(i, CC_STAT_LEFT);
        int y = stats.at<int>(i, CC_STAT_TOP);
        int w = stats.at<int>(i, CC_STAT_WIDTH);
        int h = stats.at<int>(i, CC_STAT_HEIGHT);

        Mat roi = (labels == i)(Rect(x, y, w, h));

        // projection theo Y
        Mat proj;
        reduce(roi, proj, 1, REDUCE_SUM, CV_32S);

        int start = -1;
        for (int r = 0; r < proj.rows; r++)
        {
            int v = proj.at<int>(r, 0);
            if (v > 0 && start < 0) start = r;
            if (v == 0 && start >= 0)
            {
                if (r - start > minGap)
                {
                    Rect part(x, y + start, w, r - start);
                    outMask(part).setTo(255);
                }
                start = -1;
            }
        }

        if (start >= 0)
        {
            Rect part(x, y + start, w, proj.rows - start);
            outMask(part).setTo(255);
        }
    }
}

int MonoSegCore::SegmentLowContrastMonoHardNoise(
    const Mat& gray8U,
    Mat& outMask8U,
    const MonoSegParams& pin,
    Mat* outScore
)
{
    CV_Assert(gray8U.type() == CV_8UC1);

    MonoSegParams p = pin;
    p.bgBlurK = MakeOdd(max(3, p.bgBlurK));
    p.blackHatK = MakeOdd(max(3, p.blackHatK));

    Mat score;

    // ===== 1️⃣ BACKGROUND SUPPRESSION =====
    if (p.useBlackHat)
    {
        Mat k = getStructuringElement(
            MORPH_RECT,
            Size(p.blackHatK, p.blackHatK)
        );
        morphologyEx(gray8U, score, MORPH_BLACKHAT, k);
    }
    else
    {
        Mat bg;
        blur(gray8U, bg, Size(p.bgBlurK, p.bgBlurK));
        if (p.mode == 0) subtract(bg, gray8U, score);
        else             subtract(gray8U, bg, score);
    }

    normalize(score, score, 0, 255, NORM_MINMAX);
    score.convertTo(score, CV_8U);

    // ===== 2️⃣ THRESHOLD =====
    int thr = AutoThresholdHistogramValley(score);
    threshold(score, outMask8U, thr, 255, THRESH_BINARY);

    // đảm bảo object là foreground
    if (countNonZero(outMask8U) < (outMask8U.total() / 2))
        bitwise_not(outMask8U, outMask8U);

    // ===== 3️⃣ OPEN DỌC (DIỆT NOISE) =====
    if (p.openK > 0)
    {
        Mat kOpen = getStructuringElement(
            MORPH_RECT,
            Size(p.openK, p.openK * 5)
        );
        morphologyEx(outMask8U, outMask8U, MORPH_OPEN, kOpen);
    }

    // ===== 4️⃣ CLOSE DỌC (LIỀN KHỐI) =====
    if (p.closeK > 0)
    {
        Mat kClose = getStructuringElement(
            MORPH_RECT,
            Size(p.closeK, p.closeK * 2)
        );
        morphologyEx(outMask8U, outMask8U, MORPH_CLOSE, kClose);
    }

    // ===== 5️⃣ CONNECTED COMPONENT FILTER =====
    Mat labels, stats, cents;
    int n = connectedComponentsWithStats(
        outMask8U, labels, stats, cents
    );

    Mat clean = Mat::zeros(outMask8U.size(), CV_8UC1);
    int totalArea = 0;

    for (int i = 1; i < n; i++)
    {
        int area = stats.at<int>(i, CC_STAT_AREA);
        int w = stats.at<int>(i, CC_STAT_WIDTH);
        int h = stats.at<int>(i, CC_STAT_HEIGHT);

        if (area < 800) continue;   // loại noise
        if (h < w * 2)  continue;   // chỉ giữ object đứng

        clean.setTo(255, labels == i);
        totalArea += area;
    }

    // ===== 6️⃣ FILL HOLE (CỰC KỲ QUAN TRỌNG) =====
    Mat filled = clean.clone();
    floodFill(filled, Point(0, 0), Scalar(255));
    bitwise_not(filled, filled);
    clean |= filled;

    // ===== 7️⃣ SPLIT OBJECT DÍNH THEO TRỤC Y =====
    Mat split;
    SplitVerticalBlobs(clean, split, 15);

    // ===== 8️⃣ OUTPUT CUỐI =====
    outMask8U = split;

    if (outScore) *outScore = score;
    return totalArea;
}

static inline void NormalizeRectAngle(cv::RotatedRect& rr)
{
    // OpenCV: angle ∈ (-90, 0]
    if (rr.size.width < rr.size.height)
    {
        rr.angle += 90.f;
        std::swap(rr.size.width, rr.size.height);
    }

    // ép về [0, 90)
    if (rr.angle < 0) rr.angle += 90.f;
}
// ==================== NEW: PAPER + CHIP ROTATED RECTS ====================
int MonoSegCore::ExtractPaperAndChipRectRotatesFromMask(
    const cv::Mat& mask8U,
    std::vector<cv::RotatedRect>& outRR,
    std::vector<uint8_t>& outIsPaper,
    const ChipExtractParams& p
)
{
    CV_Assert(mask8U.type() == CV_8UC1);
    outRR.clear();
    outIsPaper.clear();

    const int W = mask8U.cols;
    const int H = mask8U.rows;
    const double imgArea = (double)W * H;


    // =========================================================
    // B) CHIP DETECT – CLEAN MASK + CC
    // =========================================================
    cv::Mat clean = mask8U.clone();
    cv::morphologyEx(clean, clean, cv::MORPH_CLOSE,
        cv::getStructuringElement(cv::MORPH_RECT,
            cv::Size(p.vertKernelW, p.vertKernelH)));
    cv::morphologyEx(clean, clean, cv::MORPH_OPEN,
        cv::getStructuringElement(cv::MORPH_RECT,
            cv::Size(p.openK, p.openK)));

    cv::Mat labels, stats, cents;
    int n = cv::connectedComponentsWithStats(clean, labels, stats, cents, 8);
    if (n <= 1) return (int)outRR.size();

    // ---- collect candidates ----
    std::vector<cv::Rect> cand;
    std::vector<float> Ws, Hs;

    for (int i = 1; i < n; i++)
    {
        int area = stats.at<int>(i, cv::CC_STAT_AREA);
        int x = stats.at<int>(i, cv::CC_STAT_LEFT);
        int y = stats.at<int>(i, cv::CC_STAT_TOP);
        int w = stats.at<int>(i, cv::CC_STAT_WIDTH);
        int h = stats.at<int>(i, cv::CC_STAT_HEIGHT);

        if (area < p.minArea) continue;
        if (w < p.minW || h < p.minH) continue;
        if (h <= w) continue;
        if ((float)h / (float)w < p.minAspect) continue;

        cand.emplace_back(x, y, w, h);
        Ws.push_back((float)w);
        Hs.push_back((float)h);
    }

    if ((int)cand.size() < 1)
        return (int)outRR.size();

    // ---- auto-learn size ----
    auto median = [](std::vector<float>& v)
        {
            size_t m = v.size() / 2;
            std::nth_element(v.begin(), v.begin() + m, v.end());
            return v[m];
        };

    float medW = median(Ws);
    float medH = median(Hs);

    float minW = medW * (1.f - p.sizeTol);
    float maxW = medW * (1.f + p.sizeTol);
    float minH = medH * (1.f - p.sizeTol);
    float maxH = medH * (1.f + p.sizeTol);
    //float medW = median(Ws);
    //float medH = median(Hs);

    //float tol = p.sizeTol;   // ví dụ 0.25f
    //float minW = medW * (1.f - tol);
    //float maxW = medW * (1.f + tol);
    //float minH = medH * (1.f - tol);
    //float maxH = medH * (1.f + tol);


    // ---- final chip filter ----
    for (auto& box : cand)
    {
        if (box.width < minW || box.width > maxW) continue;
        if (box.height < minH || box.height > maxH) continue;
        float ar = (float)box.height / (float)box.width;
        if (ar < 2.0f) continue;   // loại box lùn / méo

        cv::Mat roi = clean(box);
        double fill = (double)cv::countNonZero(roi) / (double)box.area();
        if (fill < p.minFillRatio) continue;
        /*  cv::Mat roi = clean(box);
          double fill = (double)cv::countNonZero(roi) / (double)box.area();
          if (fill < p.minFillRatio) continue;*/

        std::vector<std::vector<cv::Point>> cnts;
        cv::findContours(roi, cnts, cv::RETR_EXTERNAL, cv::CHAIN_APPROX_SIMPLE);
        if (cnts.empty()) continue;
        cv::RotatedRect rr = cv::minAreaRect(cnts[0]);
        rr.center.x += box.x;
        rr.center.y += box.y;

        NormalizeRectAngle(rr);
        if (rr.size.width > rr.size.height)
        {
            std::swap(rr.size.width, rr.size.height);
            rr.angle += 90.f;
        }
        /*cv::RotatedRect rr = cv::minAreaRect(cnts[0]);
        rr.center.x += box.x;
        rr.center.y += box.y;

        if (rr.size.width > rr.size.height)
        {
            std::swap(rr.size.width, rr.size.height);
            rr.angle += 90.f;
        }*/

        outRR.push_back(rr);
        outIsPaper.push_back(0);
    }

    return (int)outRR.size();
}


// ==================== BACKWARD: CHIP BOXES (axis aligned) ====================
int MonoSegCore::ExtractChipBoxesFromMask(
    const Mat& mask8U,
    vector<Rect>& outBoxes,
    const ChipExtractParams& p
)
{
    // dùng luôn hàm rotated rect, rồi convert sang boundingRect
    vector<RotatedRect> rr;
    vector<uint8_t> isPaper;
    ExtractPaperAndChipRectRotatesFromMask(mask8U, rr, isPaper, p);

    outBoxes.clear();
    for (size_t i = 0; i < rr.size(); i++)
    {
        if (i < isPaper.size() && isPaper[i]) continue; // bỏ paper
        outBoxes.push_back(rr[i].boundingRect());
    }
    return (int)outBoxes.size();
}
