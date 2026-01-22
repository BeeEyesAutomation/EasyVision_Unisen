#include "Pattern2.h"

#include <opencv2/imgproc/imgproc_c.h>

using namespace cv;
using namespace BeeCpp;
using namespace std; 

using namespace System;
// ===== Validation helpers (optional, used when enabled via PatternMatchOptions) =====
static float CalcEntropyGray32(const cv::Mat& gray8)
{
	CV_Assert(gray8.type() == CV_8UC1);
	int histSize = 32;
	float range[] = { 0, 256 };
	const float* ranges = range;
	cv::Mat hist;
	cv::calcHist(&gray8, 1, 0, cv::Mat(), hist, 1, &histSize, &ranges);
	hist /= (float)gray8.total();
	float e = 0.f;
	for (int i = 0; i < histSize; ++i)
	{
		float p = hist.at<float>(i);
		if (p > 1e-6f) e -= p * log2f(p);
	}
	return e;
}

// Fast proxy for "edge amount" (avoid heavy Canny). Uses Sobel magnitude threshold.
static int CountStrongEdges(const cv::Mat& gray8)
{
	CV_Assert(gray8.type() == CV_8UC1);
	cv::Mat gx, gy;
	cv::Sobel(gray8, gx, CV_16S, 1, 0, 3);
	cv::Sobel(gray8, gy, CV_16S, 0, 1, 3);
	cv::Mat agx, agy;
	cv::convertScaleAbs(gx, agx);
	cv::convertScaleAbs(gy, agy);
	cv::Mat mag;
	cv::addWeighted(agx, 0.5, agy, 0.5, 0.0, mag);
	// Threshold chosen to be robust across lighting; this is only a relative indicator.
	return cv::countNonZero(mag > 40);
}
static bool ValidateAndAdjustScore(
	double& ioScore,
	const cv::Mat& graySrcLevel0,
	const cv::Point2d& ptLTLevel0,
	int templW,
	int templH,
	const s_TemplData& templData,
	const PatternMatchOptions% opt,
	double coarseScore)
{
	if (!opt.EnableValidate) return true;

	// Build axis-aligned ROI (clamped) for quick content stats.
	int x = (int)std::floor(ptLTLevel0.x);
	int y = (int)std::floor(ptLTLevel0.y);
	cv::Rect roi(x, y, templW, templH);
	roi &= cv::Rect(0, 0, graySrcLevel0.cols, graySrcLevel0.rows);
	if (roi.width < 8 || roi.height < 8) return false; // too close to border / invalid

	cv::Mat patch = graySrcLevel0(roi);

	// --- Min-edge validation (optional) ---
	if (opt.EnableMinEdge && templData.modelEdgeCount > 0)
	{
		int edgeCount = CountStrongEdges(patch);
		if (edgeCount < (int)std::floor((double)templData.modelEdgeCount * opt.MinEdgeRatio))
			return false;
	}

	// --- Soft Anti-Text via entropy penalty (optional) ---
	if (opt.EnableSoftAntiText && opt.EntropyWeight > 0.0)
	{
		float e = CalcEntropyGray32(patch);
		// Use delta vs template entropy when available (safer for complex patterns)
		double delta = (double)e - templData.modelEntropy;
		if (delta > 0.0)
		{
			double penalty = exp(-delta * opt.EntropyWeight);
			ioScore *= penalty;
		}
	}

	// --- Multi-scale consistency (optional) ---
	if (opt.EnableMultiScaleCheck)
	{
		if (coarseScore >= 0.0 && fabs(ioScore - coarseScore) > opt.MaxScaleScoreDiff)
			return false;
	}

	return true;
}




// ===== Helpers (không dùng lambda) =====
// anonymous namespace

// ===== Pattern wrapper =====
Pattern2::Pattern2() {
	img = new Img();
	com = new CommonPlus();
	// default options (backward compatible)
	m_opt = PatternMatchOptions(true);
}
Pattern2::~Pattern2() { this->!Pattern2(); }
Pattern2::!Pattern2() { if (img) { delete img; img = nullptr; } }

void Pattern2::SetMatchOptions(PatternMatchOptions opt)
{
	m_opt = opt;
}

PatternMatchOptions Pattern2::GetMatchOptions()
{
	return m_opt;
}
void Pattern2::SetImgeSampleNoCrop(IntPtr data, int w, int h, int stride, int ch)
{
	img->matSample = Mat(h, w, CV_8UC1, data.ToPointer(), stride);

}
// === PUBLIC API ===
System::IntPtr Pattern2::SetImgeSample(System::IntPtr tplData, int tplW, int tplH, int tplStride, int tplChannels, RectRotateCli rr, Nullable<RectRotateCli> rrMask,bool NoCrop,
	[System::Runtime::InteropServices::Out] int% outW,
	[System::Runtime::InteropServices::Out] int% outH,
	[System::Runtime::InteropServices::Out] int% outStride,
	[System::Runtime::InteropServices::Out] int% outChannels)
{
   
	if (NoCrop)img->matSample = Mat(tplH, tplW, CV_8UC1, tplData.ToPointer(), tplStride);
	else
	{// img->matSample là cv::Mat
		Nullable<RectRotateCli> mask =
			rrMask.HasValue ? Nullable<RectRotateCli>(rrMask.Value)
			: Nullable<RectRotateCli>();

		com->CropRotToMat(
			tplData, tplW, tplH, tplStride, tplChannels,
			rr, mask, /*returnMaskOnly*/ false,
			System::IntPtr(&img->matSample)
		);
		/*Mat raw(tplH, tplW, CV_8UC1, tplData.ToPointer(), tplStride);
		cv::Mat result;

		if (rrMask.HasValue) { RectRotateCli m = rrMask.Value; com->RunCrop(raw, rr, &m, false, result); }
		else { com->RunCrop(raw, rr, nullptr, false, result); }
		img->matSample = result.clone();
		*/

	}
	if (!img->matSample.isContinuous()) img->matSample = img->matSample.clone();
	const int W = img->matSample.cols, H = img->matSample.rows, C = img->matSample.channels();
	const int S = (int)img->matSample.step;
	const size_t bytes = (size_t)S * H;

	IntPtr mem = System::Runtime::InteropServices::Marshal::AllocHGlobal((IntPtr)(long long)bytes);
	if (mem == IntPtr::Zero) return IntPtr::Zero;
	std::memcpy(mem.ToPointer(), img->matSample.data, bytes);
	outW = W; outH = H; outStride = S; outChannels = C;
	return mem;
  }
void Pattern2::SetImgeRaw(System::IntPtr tplData, int tplW, int tplH, int tplStride, int tplChannels, RectRotateCli rr, Nullable<RectRotateCli> rrMask)
{
	Nullable<RectRotateCli> mask =
		rrMask.HasValue ? Nullable<RectRotateCli>(rrMask.Value)
		: Nullable<RectRotateCli>();

	com->CropRotToMat(
		tplData, tplW, tplH, tplStride, tplChannels,
		rr, mask, /*returnMaskOnly*/ false,
		System::IntPtr(&img->matRaw)
	);
   
	//cv::imwrite("Pos.png", img->matRaw);
}
void Pattern2::SetRawNoCrop(IntPtr data, int w, int h, int stride, int ch)
{
	if (data == IntPtr::Zero || w <= 0 || h <= 0 || stride <= 0)
	{
		// reset an toàn
		this->img->matRaw.release();
		return;
	}

	// chọn kiểu Mat đúng kênh
	int cvType;
	switch (ch)
	{
	case 1: cvType = CV_8UC1; break;
	case 3: cvType = CV_8UC3; break;
	case 4: cvType = CV_8UC4; break;
	default: cvType = CV_8UC1; break; // fallback
	}

	// wrap IntPtr thành Mat (không copy dữ liệu)
	cv::Mat wrapped(h, w, cvType, data.ToPointer(), stride);

	// clone để đảm bảo Mat trong C++ sở hữu dữ liệu, tránh phụ thuộc vào vùng nhớ bên C#
	this->img->matRaw = wrapped.clone();
	//img->matRaw = Mat (h, w, CV_8UC1, data.ToPointer(), stride);
	
}
void Pattern2::LearnPattern()
{
	Mat raw = img->matSample.clone();

	// Normalize template to gray8 for optional validation stats
	cv::Mat templGray;
	if (raw.channels() == 3) cv::cvtColor(raw, templGray, cv::COLOR_BGR2GRAY);
	else if (raw.channels() == 1) templGray = raw;
	else {
		cv::Mat tmp; cv::convertScaleAbs(raw, tmp);
		if (tmp.channels() == 3) cv::cvtColor(tmp, templGray, cv::COLOR_BGR2GRAY);
		else templGray = tmp;
	}
	if (templGray.type() != CV_8UC1) templGray.convertTo(templGray, CV_8UC1);
	
	int iTopLayer = img->GetTopLayer(&raw, (int)sqrt((double)img->m_iMinReduceArea));
	buildPyramid(raw, img->m_TemplData.vecPyramid, iTopLayer);
	s_TemplData* templData = &img->m_TemplData;
	templData->iBorderColor = mean(raw).val[0] < 128 ? 255 : 0;
	int iSize = templData->vecPyramid.size();
	templData->resize(iSize);

	for (int i = 0; i < iSize; i++)
	{
		double invArea = 1. / ((double)templData->vecPyramid[i].rows * templData->vecPyramid[i].cols);
		Scalar templMean, templSdv;
		double templNorm = 0, templSum2 = 0;

		meanStdDev(templData->vecPyramid[i], templMean, templSdv);
		templNorm = templSdv[0] * templSdv[0] + templSdv[1] * templSdv[1] + templSdv[2] * templSdv[2] + templSdv[3] * templSdv[3];

		if (templNorm < DBL_EPSILON)
		{
			templData->vecResultEqual1[i] = true;
		}
		templSum2 = templNorm + templMean[0] * templMean[0] + templMean[1] * templMean[1] + templMean[2] * templMean[2] + templMean[3] * templMean[3];


		templSum2 /= invArea;
		templNorm = std::sqrt(templNorm);
		templNorm /= std::sqrt(invArea); // care of accuracy here


		templData->vecInvArea[i] = invArea;
		templData->vecTemplMean[i] = templMean;
		templData->vecTemplNorm[i] = templNorm;
	}
	templData->bIsPatternLearned = true;

	// ---- Optional template stats (do not affect legacy matching) ----
	// These are used only when validation options are enabled.
	try {
		templData->modelEntropy = (double)CalcEntropyGray32(templGray);
		templData->modelEdgeCount = CountStrongEdges(templGray);
	}
	catch (...) {
		templData->modelEntropy = 0;
		templData->modelEdgeCount = 0;
	}
}
void Pattern2::FreeBuffer(System::IntPtr p)
{
	if (p != System::IntPtr::Zero)
		System::Runtime::InteropServices::Marshal::FreeHGlobal(p);
}

List<Rotaterectangle>^ Pattern2::Match(
    bool   m_bStopLayer1,
    double m_dToleranceAngle,
    double m_dTolerance1,
    double m_dTolerance2,
    double m_dScore,
    bool   m_ckSIMD,
    bool   m_ckBitwiseNot,
    bool   m_bSubPixel,
    int    m_iMaxPos,
    double m_dMaxOverlap,
    bool   useMultiThread,
    int    numThreads
)
{
    auto results = gcnew List<Rotaterectangle>(Math::Max(m_iMaxPos, 0));

    if (img == nullptr) return results;
    if (img->matSample.empty()) return results;
    if (img->matRaw.empty())    return results;
    if (!img->m_TemplData.bIsPatternLearned) return results;

    if (m_iMaxPos <= 0) m_iMaxPos = 1;
    if (m_iMaxPos > 256) m_iMaxPos = 256;

    const int    MAX_ANGLES = 361;
    const double epsTiny = 1e-6;
    const double MIN_ANGLE_STEP = 0.05;

    // 1) Gray 8U
    cv::Mat srcForPyr;
    {
        cv::Mat src = img->matRaw;

        if (src.channels() == 3) cv::cvtColor(src, srcForPyr, cv::COLOR_BGR2GRAY);
        else if (src.channels() == 1) srcForPyr = src;
        else {
            cv::Mat tmp; cv::convertScaleAbs(src, tmp);
            if (tmp.channels() == 3) cv::cvtColor(tmp, srcForPyr, cv::COLOR_BGR2GRAY);
            else if (tmp.channels() == 1) srcForPyr = tmp;
            else cv::cvtColor(tmp, srcForPyr, cv::COLOR_BGR2GRAY);
        }

        if (srcForPyr.depth() != CV_8U) cv::convertScaleAbs(srcForPyr, srcForPyr);
        if (m_ckBitwiseNot) cv::bitwise_not(srcForPyr, srcForPyr);
    }

    // 2) Pyramid
    int iTopLayer = img->GetTopLayer(&img->matSample, (int)std::sqrt((double)img->m_iMinReduceArea));
    if (iTopLayer < 0) iTopLayer = 0;

    std::vector<cv::Mat> vecMatSrcPyr;
    vecMatSrcPyr.reserve((size_t)iTopLayer + 1);
    cv::buildPyramid(srcForPyr, vecMatSrcPyr, iTopLayer);

    s_TemplData* pTemplData = &img->m_TemplData;
    if ((int)pTemplData->vecPyramid.size() <= iTopLayer) return results;

    // 3) Angle step
    double dAngleStepAuto = std::atan(2.0 / std::max(
        pTemplData->vecPyramid[(size_t)iTopLayer].cols,
        pTemplData->vecPyramid[(size_t)iTopLayer].rows)) * R2D;
    if (dAngleStepAuto < epsTiny) dAngleStepAuto = 0.5;

    const double userStep = std::fabs(m_dToleranceAngle);
    const bool   useFixedStep = (userStep > epsTiny);
    double dAngleStep = useFixedStep ? std::max(userStep, MIN_ANGLE_STEP) : dAngleStepAuto;

    if (m_dTolerance2 < m_dTolerance1) std::swap(m_dTolerance1, m_dTolerance2);

    std::vector<double> vecAngles;
    vecAngles.reserve(64);
    for (double a = m_dTolerance1; a <= m_dTolerance2 + 0.5 * dAngleStep; a += dAngleStep)
        vecAngles.push_back(a);
    if (vecAngles.empty()) vecAngles.push_back(0.0);

    if ((int)vecAngles.size() > MAX_ANGLES) {
        std::vector<double> trimmed; trimmed.reserve(MAX_ANGLES);
        double stepA = (vecAngles.back() - vecAngles.front()) / (MAX_ANGLES - 1);
        for (int i = 0; i < MAX_ANGLES; ++i) trimmed.push_back(vecAngles.front() + i * stepA);
        vecAngles.swap(trimmed);
    }

    // 4) Coarse match @ top layer
    std::vector<s_MatchParameter> vecMatchParameter;
    vecMatchParameter.reserve(vecAngles.size() * (size_t)m_iMaxPos);

    const int iTopSrcW = vecMatSrcPyr[(size_t)iTopLayer].cols;
    const int iTopSrcH = vecMatSrcPyr[(size_t)iTopLayer].rows;
    cv::Point2f ptCenter((iTopSrcW - 1) * 0.5f, (iTopSrcH - 1) * 0.5f);

    const cv::Size sizePatTop = pTemplData->vecPyramid[(size_t)iTopLayer].size();

    for (size_t ai = 0; ai < vecAngles.size(); ++ai)
    {
        try {
            const double angDeg = vecAngles[ai];

            // ===== GIỐNG HÀM CŨ: sizeBest + fTx/fTy =====
            cv::Mat matR = cv::getRotationMatrix2D(ptCenter, angDeg, 1.0);
            const cv::Size sizeBest = img->GetBestRotationSize(
                vecMatSrcPyr[(size_t)iTopLayer].size(),
                pTemplData->vecPyramid[(size_t)iTopLayer].size(),
                angDeg);

            const float fTx = (sizeBest.width - 1) * 0.5f - ptCenter.x;
            const float fTy = (sizeBest.height - 1) * 0.5f - ptCenter.y;
            matR.at<double>(0, 2) += fTx;
            matR.at<double>(1, 2) += fTy;

            cv::Mat matRotatedSrc;
            cv::warpAffine(
                vecMatSrcPyr[(size_t)iTopLayer],
                matRotatedSrc,
                matR,
                sizeBest,
                cv::INTER_LINEAR,
                cv::BORDER_CONSTANT,
                cv::Scalar(pTemplData->iBorderColor));

            cv::Mat matResult;
            img->MatchTemplate(matRotatedSrc, pTemplData, matResult, iTopLayer, false, m_ckSIMD);
            if (matResult.empty()) continue;

            if (matResult.type() != CV_32F) matResult.convertTo(matResult, CV_32F);

            double dMaxVal = -1.0;
            cv::Point ptMaxLoc;
            cv::minMaxLoc(matResult, nullptr, &dMaxVal, nullptr, &ptMaxLoc);

            if (dMaxVal < m_dScore) continue;

            // ===== mp.pt phải trừ fTx/fTy như hàm cũ =====
            s_MatchParameter mp(cv::Point2f((float)ptMaxLoc.x - fTx, (float)ptMaxLoc.y - fTy), dMaxVal, angDeg);
            mp.nPyrLevel = iTopLayer;

            // ✅ ValidateScoreStability: 1 lần / candidate
            if (m_opt.EnableValidate && m_opt.EnableScoreStability)
            {
                if (!ValidateScoreStability(img, mp, m_ckSIMD, m_ckBitwiseNot))
                    continue;
            }

            vecMatchParameter.push_back(mp);
        }
        catch (const std::exception&) {}
    }

    if (vecMatchParameter.empty()) return results;

    std::sort(vecMatchParameter.begin(), vecMatchParameter.end(), compareScoreBig2Small);

    // ======= RULE TRẢ KẾT QUẢ GIỐNG HÀM CŨ =======
    // Coarse-only: đưa mp.pt về hệ gốc (un-rotate) rồi build rectR trước khi FilterWithRotatedRect
    const int  iStopLayer = m_bStopLayer1 ? 1 : 0;
    const int  iDstW = pTemplData->vecPyramid[(size_t)iStopLayer].cols * (iStopLayer == 0 ? 1 : 2);
    const int  iDstH = pTemplData->vecPyramid[(size_t)iStopLayer].rows * (iStopLayer == 0 ? 1 : 2);

    for (size_t i = 0; i < vecMatchParameter.size(); ++i)
    {
        // đưa điểm về hệ gốc của layer top
        const double dRAngle = -vecMatchParameter[i].dMatchAngle * D2R;
        cv::Point2f ptLT = img->ptRotatePt2f(vecMatchParameter[i].pt, ptCenter, dRAngle);

        // scale theo rule code cũ (topLayer==0 -> 1, else -> 2)
        const float sc = (iTopLayer == 0 ? 1.f : 2.f);
        ptLT *= sc;

        vecMatchParameter[i].pt = ptLT;

        // build rectR (bắt buộc để rectR.center không = 0,0)
        const double angDeg = vecMatchParameter[i].dMatchAngle;
        const double angRad = -angDeg * D2R;
        const double c = std::cos(angRad);
        const double s = std::sin(angRad);

        const cv::Point2f vW((float)(iDstW * c), (float)(-iDstW * s));
        const cv::Point2f vH((float)(iDstH * s), (float)(iDstH * c));

        const cv::Point2f center(
            ptLT.x + 0.5f * (vW.x + vH.x),
            ptLT.y + 0.5f * (vW.y + vH.y)
        );

        vecMatchParameter[i].rectR = cv::RotatedRect(
            center,
            cv::Size2f((float)iDstW, (float)iDstH),
            (float)angDeg
        );
    }

    // Overlap filter dùng rectR đã build
    img->FilterWithRotatedRect(&vecMatchParameter, CV_TM_CCOEFF_NORMED, m_dMaxOverlap);
    std::sort(vecMatchParameter.begin(), vecMatchParameter.end(), compareScoreBig2Small);
    if (vecMatchParameter.empty()) return results;

    const int iW = pTemplData->vecPyramid[0].cols;
    const int iH = pTemplData->vecPyramid[0].rows;

    const size_t takeN = std::min((size_t)m_iMaxPos, vecMatchParameter.size());
    for (size_t i = 0; i < takeN; ++i)
    {
        const double ang = vecMatchParameter[i].dMatchAngle;
        const cv::Point2f c = vecMatchParameter[i].rectR.center; // ✅ giống hàm cũ

        Rotaterectangle rr;
        rr.Cx = c.x; rr.Cy = c.y;
        rr.AngleDeg = ang;
        rr.Width = (double)iW;
        rr.Height = (double)iH;
        rr.Score = vecMatchParameter[i].dMatchScore * 100.0;
        results->Add(rr);
    }

    return results;
}





// ===============================
// CÁCH 1 – SCORE STABILITY TEST
// ===============================
bool BeeCpp::Pattern2::ValidateScoreStability(
    Img* img,
    const s_MatchParameter& mp,
    bool m_ckSIMD,
    bool m_ckBitwiseNot
)
{
    // Get ROI by current best pose
    cv::Mat roi;
    img->GetRotatedROI(
        img->matRaw,
        img->matSample.size(),
        cv::Point2f((float)mp.pt.x, (float)mp.pt.y),
        mp.dMatchAngle,
        roi
    );

    if (roi.empty())
        return false;

    // Light blur
    cv::Mat roiBlur;
    cv::GaussianBlur(roi, roiBlur, cv::Size(3,3), 1.0);

    // Re-match on blurred ROI (score only)
    cv::Mat result;
    img->MatchTemplate(
        roiBlur,
        &img->m_TemplData,
        result,
        0,
        m_ckSIMD,
        m_ckBitwiseNot
    );

    double minVal, maxVal;
    cv::minMaxLoc(result, &minVal, &maxVal);

    double diff  = std::fabs(mp.dMatchScore - maxVal);
    double ratio = diff / std::max(mp.dMatchScore, 1e-6);

    return ratio <= 0.15; // fixed threshold
}
