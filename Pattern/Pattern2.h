#pragma once
#include <opencv2/imgproc/types_c.h>
#include <opencv2/core/utility.hpp>
#include <opencv2/core.hpp>
#include <opencv2/imgproc.hpp>
#include <opencv2/imgcodecs.hpp>
#include <vector>
#include <cmath>
#include <limits>
#include <mutex>


#include <thread>
#include <limits>
#include <algorithm>
// C++/CLI
using namespace cv;
using namespace std;
using namespace System;
using namespace System::Collections::Generic;
#include "Global.h"
namespace BeeCpp
{

	// ===== Hằng số =====
#ifndef VISION_TOLERANCE
#define VISION_TOLERANCE 1e-6
#endif
#ifndef D2R
#define D2R (3.14159265358979323846/180.0)
#endif
#ifndef R2D
#define R2D (180.0/3.14159265358979323846)
#endif
#ifndef MATCH_CANDIDATE_NUM
#define MATCH_CANDIDATE_NUM 32
#endif
	// ===== Struct native =====
	// ===== Struct native =====
	public struct s_TemplData
	{
		vector<Mat> vecPyramid;
		vector<cv::UMat> vecPyramidGpu;
		vector<Scalar> vecTemplMean;
		vector<double> vecTemplNorm;
		vector<double> vecInvArea;
		vector<bool> vecResultEqual1;
		bool bIsPatternLearned;
		int iBorderColor;
		// Optional template statistics for validation (do NOT affect legacy flow)
		int    modelEdgeCount = 0; // strong-edge count estimated from template (level 0)
		double modelEntropy = 0; // entropy of template gray (level 0)
		double modelEdgeDensity = 0; // edgeCount / area
		double modelEntropyNorm = 0; // entropy / 8.0

		// Auto thresholds/weights derived from the template itself
		double autoStrongBaseScore = 0.90;
		double autoSoftEdgeIou = 0.08;
		double autoSoftEdgeRatio = 0.75;
		double autoHardEdgeIou = 0.10;
		double autoHardEdgeRatio = 0.80;
		double autoMinFused = 0.55;
		double autoWBase = 0.70;
		double autoWRaw = 0.10;
		double autoWGrad = 0.05;
		double autoWEdgeIou = 0.10;
		double autoWEdgeRatio = 0.05;

		// Preprocess snapshot learned with the template.
		bool   ppEnableBitwiseNot = false;
		bool   ppEnableIlluminationFix = false;
		int    ppIllumKernel = 0;
		bool   ppEnableCLAHE = false;
		double ppClaheClip = 2.0;
		int    ppClaheTile = 8;
		bool   ppEnableGamma = false;
		double ppGamma = 1.0;
		int    ppDenoiseMethod = 0;
		int    ppDenoiseKernel = 3;
		int    ppDomain = 0;
		int    ppEdgeMethod = 2;
		int    ppEdgeKernel = 3;
		double ppCannyLow = 0.0;
		double ppCannyHigh = 0.0;
		bool   ppEdgeKeepMagnitude = true;
		int    ppEdgeDilatePx = 1;
		bool   ppEnableEdgeLengthFilter = false;
		int    ppMinEdgeSegmentLen = 6;
		double ppFuseGrayWeight = 0.5;

		cv::Mat tplPreprocessedGray;
		cv::Mat tplEdgeMagnitude;
		cv::Mat tplEdgeBinary;
		void clear()
		{
			vector<Mat>().swap(vecPyramid);
			vector<cv::UMat>().swap(vecPyramidGpu);
			vector<double>().swap(vecTemplNorm);
			vector<double>().swap(vecInvArea);
			vector<Scalar>().swap(vecTemplMean);
			vector<bool>().swap(vecResultEqual1);
			modelEdgeCount = 0;
			modelEntropy = 0;
			modelEdgeDensity = 0;
			modelEntropyNorm = 0;
			autoStrongBaseScore = 0.90;
			autoSoftEdgeIou = 0.08;
			autoSoftEdgeRatio = 0.75;
			autoHardEdgeIou = 0.10;
			autoHardEdgeRatio = 0.80;
			autoMinFused = 0.55;
			autoWBase = 0.70;
			autoWRaw = 0.10;
			autoWGrad = 0.05;
			autoWEdgeIou = 0.10;
			autoWEdgeRatio = 0.05;
			ppEnableBitwiseNot = false;
			ppEnableIlluminationFix = false;
			ppIllumKernel = 0;
			ppEnableCLAHE = false;
			ppClaheClip = 2.0;
			ppClaheTile = 8;
			ppEnableGamma = false;
			ppGamma = 1.0;
			ppDenoiseMethod = 0;
			ppDenoiseKernel = 3;
			ppDomain = 0;
			ppEdgeMethod = 2;
			ppEdgeKernel = 3;
			ppCannyLow = 0.0;
			ppCannyHigh = 0.0;
			ppEdgeKeepMagnitude = true;
			ppEdgeDilatePx = 1;
			ppEnableEdgeLengthFilter = false;
			ppMinEdgeSegmentLen = 6;
			ppFuseGrayWeight = 0.5;
			tplPreprocessedGray.release();
			tplEdgeMagnitude.release();
			tplEdgeBinary.release();
		}
		void resize(int iSize)
		{
			vecTemplMean.resize(iSize);
			vecTemplNorm.resize(iSize, 0);
			vecInvArea.resize(iSize, 1);
			vecResultEqual1.resize(iSize, false);
		}
		s_TemplData()
		{
			bIsPatternLearned = false;
		}
	};

	public struct s_StableScaleTemplate
	{
		double scale = 1.0;

		cv::Mat tplGray;
		cv::Mat tplNorm;
		cv::Mat tplGrad;
		cv::Mat tplEdge;
		cv::Mat tplEdgeMagnitude;
		int tplEdgeCount = 0;

		s_TemplData templData;

		double thresholdFinal = 0.50;
		double strongBase = 0.88;
		double softEdgeIou = 0.06;
		double softEdgeRatio = 0.50;
		double hardEdgeIou = 0.10;
		double hardEdgeRatio = 0.65;

		double wBase = 0.72;
		double wRaw = 0.10;
		double wGrad = 0.04;
		double wEdgeIou = 0.10;
		double wEdgeRatio = 0.04;
	};
	public struct s_MatchParameter
	{
		int nPyrLevel = 0;
		Point2d pt;
		double dMatchScore;
		// score at coarse (top) layer for optional multi-scale consistency check
		double dCoarseScore;
		double dMatchAngle;
		//Mat matRotatedSrc;
		Rect rectRoi;
		double dAngleStart;
		double dAngleEnd;
		RotatedRect rectR;
		Rect rectBounding;
		bool bDelete;

		double vecResult[3][3];//for subpixel
		int iMaxScoreIndex;//for subpixel
		bool bPosOnBorder;
		Point2d ptSubPixel;
		double dNewAngle;

		s_MatchParameter(Point2f ptMinMax, double dScore, double dAngle)//, Mat matRotatedSrc = Mat ())
		{
			pt = ptMinMax;
			dMatchScore = dScore;
			dMatchAngle = dAngle;
			dCoarseScore = dScore;

			bDelete = false;
			dNewAngle = 0.0;

			bPosOnBorder = false;
		}
		s_MatchParameter()
		{
			dMatchScore = 0;
			dMatchAngle = 0;
			dCoarseScore = 0;
		}
		~s_MatchParameter()
		{

		}
	};
	public struct s_SingleTargetMatch
	{
		Point2d ptLT, ptRT, ptRB, ptLB, ptCenter;
		double dMatchedAngle;
		double dMatchScore;
	};
	public struct s_BlockMax
	{
		struct Block
		{
			Rect rect;
			double dMax;
			cv::Point ptMaxLoc;
			Block()
			{
			}
			Block(Rect rect_, double dMax_, cv::Point ptMaxLoc_)
			{
				rect = rect_;
				dMax = dMax_;
				ptMaxLoc = ptMaxLoc_;
			}
		};
		s_BlockMax()
		{
		}
		vector<Block> vecBlock;
		Mat matSrc;
		s_BlockMax(Mat matSrc_, cv::Size sizeTemplate)
		{
			matSrc = matSrc_;
			//將matSrc 拆成數個block，分別計算最大值
			int iBlockW = sizeTemplate.width * 2;
			int iBlockH = sizeTemplate.height * 2;

			int iCol = matSrc.cols / iBlockW;
			bool bHResidue = matSrc.cols % iBlockW != 0;

			int iRow = matSrc.rows / iBlockH;
			bool bVResidue = matSrc.rows % iBlockH != 0;

			if (iCol == 0 || iRow == 0)
			{
				vecBlock.clear();
				return;
			}

			vecBlock.resize(iCol * iRow);
			int iCount = 0;
			for (int y = 0; y < iRow; y++)
			{
				for (int x = 0; x < iCol; x++)
				{
					Rect rectBlock(x * iBlockW, y * iBlockH, iBlockW, iBlockH);
					vecBlock[iCount].rect = rectBlock;
					minMaxLoc(matSrc(rectBlock), 0, &vecBlock[iCount].dMax, 0, &vecBlock[iCount].ptMaxLoc);
					vecBlock[iCount].ptMaxLoc += rectBlock.tl();
					iCount++;
				}
			}
			if (bHResidue && bVResidue)
			{
				Rect rectRight(iCol * iBlockW, 0, matSrc.cols - iCol * iBlockW, matSrc.rows);
				Block blockRight;
				blockRight.rect = rectRight;
				minMaxLoc(matSrc(rectRight), 0, &blockRight.dMax, 0, &blockRight.ptMaxLoc);
				blockRight.ptMaxLoc += rectRight.tl();
				vecBlock.push_back(blockRight);

				Rect rectBottom(0, iRow * iBlockH, iCol * iBlockW, matSrc.rows - iRow * iBlockH);
				Block blockBottom;
				blockBottom.rect = rectBottom;
				minMaxLoc(matSrc(rectBottom), 0, &blockBottom.dMax, 0, &blockBottom.ptMaxLoc);
				blockBottom.ptMaxLoc += rectBottom.tl();
				vecBlock.push_back(blockBottom);
			}
			else if (bHResidue)
			{
				Rect rectRight(iCol * iBlockW, 0, matSrc.cols - iCol * iBlockW, matSrc.rows);
				Block blockRight;
				blockRight.rect = rectRight;
				minMaxLoc(matSrc(rectRight), 0, &blockRight.dMax, 0, &blockRight.ptMaxLoc);
				blockRight.ptMaxLoc += rectRight.tl();
				vecBlock.push_back(blockRight);
			}
			else
			{
				Rect rectBottom(0, iRow * iBlockH, matSrc.cols, matSrc.rows - iRow * iBlockH);
				Block blockBottom;
				blockBottom.rect = rectBottom;
				minMaxLoc(matSrc(rectBottom), 0, &blockBottom.dMax, 0, &blockBottom.ptMaxLoc);
				blockBottom.ptMaxLoc += rectBottom.tl();
				vecBlock.push_back(blockBottom);
			}
		}
		void UpdateMax(Rect rectIgnore)
		{
			if (vecBlock.size() == 0)
				return;
			//找出所有跟rectIgnore交集的block
			int iSize = vecBlock.size();
			for (int i = 0; i < iSize; i++)
			{
				Rect rectIntersec = rectIgnore & vecBlock[i].rect;
				//無交集
				if (rectIntersec.width == 0 && rectIntersec.height == 0)
					continue;
				//有交集，更新極值和極值位置
				minMaxLoc(matSrc(vecBlock[i].rect), 0, &vecBlock[i].dMax, 0, &vecBlock[i].ptMaxLoc);
				vecBlock[i].ptMaxLoc += vecBlock[i].rect.tl();
			}
		}
		void GetMaxValueLoc(double& dMax, cv::Point& ptMaxLoc)
		{
			int iSize = vecBlock.size();
			if (iSize == 0)
			{
				minMaxLoc(matSrc, 0, &dMax, 0, &ptMaxLoc);
				return;
			}
			//從block中找最大值
			int iIndex = 0;
			dMax = vecBlock[0].dMax;
			for (int i = 1; i < iSize; i++)
			{
				if (vecBlock[i].dMax >= dMax)
				{
					iIndex = i;
					dMax = vecBlock[i].dMax;
				}
			}
			ptMaxLoc = vecBlock[iIndex].ptMaxLoc;
		}
	};


	inline bool compareScoreBig2Small(const s_MatchParameter& a,
		const s_MatchParameter& b) {
		return a.dMatchScore > b.dMatchScore;
	}



	// ===== Class Img (native) =====
	class Img
	{
	public:
		cv::Mat     matRaw;
		cv::Mat     matSample;
		s_TemplData m_TemplData;
		std::vector<s_StableScaleTemplate> stableScaleBank;
		int         m_iMinReduceArea = 256;
		bool        m_EnableGpu = false;
		std::recursive_mutex stateMutex;
		//cv::Mat EnhanceForPatternStrong(const cv::Mat& src);

		void ClearStableScaleBank()
		{
			std::vector<s_StableScaleTemplate>().swap(stableScaleBank);
		}

		// pipeline
		void BuildTemplatePyramid();
	public:int GetTopLayer(Mat* matTempl, int iMinDstLength);
		  void MatchTemplate(cv::Mat& matSrc, s_TemplData* pTemplData, cv::Mat& matResult, int iLayer, bool bUseSIMD, bool m_ckSIMD);
		  void MatchTemplateGpu(cv::UMat& matSrc, s_TemplData* pTemplData, cv::UMat& matResult, int iLayer);
		  void GetRotatedROI(Mat& matSrc, cv::Size size, Point2f ptLT, double dAngle, Mat& matROI);
		  void GetRotatedROIGpu(cv::UMat& matSrc, cv::Size size, Point2f ptLT, double dAngle, cv::UMat& matROI);
		  void CCOEFF_Denominator(cv::Mat& matSrc, s_TemplData* pTemplData, cv::Mat& matResult, int iLayer);
		  cv::Size  GetBestRotationSize(cv::Size sizeSrc, cv::Size sizeDst, double dRAngle);
		  Point2f ptRotatePt2f(Point2f ptInput, Point2f ptOrg, double dAngle);
		  void FilterWithScore(vector<s_MatchParameter>* vec, double dScore);
		  void FilterWithRotatedRect(vector<s_MatchParameter>* vec, int iMethod, double dMaxOverLap);//= CV_TM_CCOEFF_NORMED,= 0
		  cv::Point GetNextMaxLoc(Mat& matResult, cv::Point ptMaxLoc, cv::Size sizeTemplate, double& dMaxValue, double dMaxOverlap);
		  cv::Point GetNextMaxLoc(Mat& matResult, cv::Point ptMaxLoc, cv::Size sizeTemplate, double& dMaxValue, double dMaxOverlap, s_BlockMax& blockMax);
		  void SortPtWithCenter(vector<Point2f>& vecSort);
		  bool SubPixEsimation(vector<s_MatchParameter>* vec, double* dX, double* dY, double* dAngle, double dAngleStep, int iMaxScoreIndex);

	};

	// ===== Kiểu trả cho C# =====
	public value struct Rotaterectangle {
		double Cx, Cy, AngleDeg, Width, Height, Score;
	};
	public enum class Pattern2DifficultyLevel
	{
		Normal = 0, // giữ hành vi hiện tại
		Easy = 1,   // dễ hơn
		Hard = 2    // khó hơn
	};

	public enum class Pattern2FeatureDomain
	{
		Gray = 0,
		Edge = 1,
		GrayPlusEdge = 2
	};

	public enum class Pattern2EdgeMethod
	{
		SobelMag = 0,
		ScharrMag = 1,
		Canny = 2,
		Laplacian = 3
	};

	public enum class Pattern2DenoiseMethod
	{
		None = 0,
		Gaussian = 1,
		Median = 2,
		Bilateral = 3
	};

	public value struct Pattern2PreprocessConfig
	{
		bool   EnableBitwiseNot;
		bool   EnableIlluminationFix;
		int    IllumKernel;
		bool   EnableCLAHE;
		double ClaheClip;
		int    ClaheTile;
		bool   EnableGammaCorrection;
		double Gamma;

		Pattern2DenoiseMethod DenoiseMethod;
		int    DenoiseKernel;

		Pattern2FeatureDomain Domain;

		Pattern2EdgeMethod EdgeMethod;
		int    EdgeKernel;
		double CannyLow;
		double CannyHigh;
		bool   EdgeKeepMagnitude;
		int    EdgeDilatePx;
		bool   EnableEdgeLengthFilter;
		int    MinEdgeSegmentLen;

		double FuseGrayWeight;
		bool   AutoPickDomain;

		Pattern2PreprocessConfig(bool init)
		{
			EnableBitwiseNot = false;
			EnableIlluminationFix = false;
			IllumKernel = 0;
			EnableCLAHE = false;
			ClaheClip = 2.0;
			ClaheTile = 8;
			EnableGammaCorrection = false;
			Gamma = 1.0;
			DenoiseMethod = Pattern2DenoiseMethod::None;
			DenoiseKernel = 3;
			Domain = Pattern2FeatureDomain::Gray;
			EdgeMethod = Pattern2EdgeMethod::Canny;
			EdgeKernel = 3;
			CannyLow = 0.0;
			CannyHigh = 0.0;
			EdgeKeepMagnitude = true;
			EdgeDilatePx = 1;
			EnableEdgeLengthFilter = false;
			MinEdgeSegmentLen = 6;
			FuseGrayWeight = 0.5;
			AutoPickDomain = false;
		}
	};

	public value struct Pattern2StableConfig
	{
		double AngleStartDeg;
		double AngleEndDeg;
		double AngleStepDeg;     // <= 0 => auto
		double MinAcceptScore;   // <= 0 => auto by template
		int    MaxPos;
		double MaxOverlap;       // 0..1
		bool   BitwiseNot;
		bool   SubPixel;

		// Logging
		bool   DebugLog;
		System::String^ DebugLogPath;

		// Feature toggles
		bool   EnableValidator;      // use raw/grad/edge validator to build final score
		bool   EnableKeepFilter;     // extra shape gate after score threshold
		bool   EnableNms;            // rotated-rect NMS at end
		bool   EnableAutoThreshold;  // thresholds/weights depend on template stats

		// Scale search around template size
		bool   EnableScaleSearch;
		double ScaleMin;             // e.g. 0.90
		double ScaleMax;             // e.g. 1.10
		double ScaleStep;            // e.g. 0.02, <=0 => auto/no search

		// Coarse stage score for calling Match()
		double RelaxedRawScore;      // 0..1
		Pattern2DifficultyLevel Difficulty;
		Pattern2PreprocessConfig Preprocess;
		bool EnableGpu;             // OpenCL/UMat acceleration for matchTemplate, CPU fallback if unavailable
		bool EnableCpuMultiThread;  // CPU-only angle scan parallelism; ignored when EnableGpu is active
		int CpuThreads;             // <=0 => auto hardware_concurrency
		Pattern2StableConfig(bool init)
		{
			AngleStartDeg = -10.0;
			AngleEndDeg = 10.0;
			AngleStepDeg = 0.0;
			MinAcceptScore = 0.0;      // auto
			MaxPos = 10;
			MaxOverlap = 0.20;
			BitwiseNot = false;
			SubPixel = true;

			DebugLog = false;
			DebugLogPath = "pattern2_debug.txt";

			EnableValidator = true;
			EnableKeepFilter = true;
			EnableNms = true;
			EnableAutoThreshold = true;

			EnableScaleSearch = false;
			ScaleMin = 0.95;
			ScaleMax = 1.05;
			ScaleStep = 0.02;
			Difficulty = Pattern2DifficultyLevel::Normal;
			RelaxedRawScore = 0.18;
			Preprocess = Pattern2PreprocessConfig(true);
			EnableGpu = false;
			EnableCpuMultiThread = false;
			CpuThreads = 0;
		}
	};


	// ===== Wrapper managed =====
	public ref class Pattern2
	{
	public:
		Pattern2();
		~Pattern2();
		!Pattern2();
		void SetRawNoCrop(IntPtr data, int w, int h, int stride, int ch);
		void SetImgeRaw(IntPtr data, int w, int h, int stride, int ch, RectRotateCli rr, Nullable<RectRotateCli> rrMask);
		void SetImgeSampleNoCrop(IntPtr data, int w, int h, int stride, int ch);
		System::IntPtr SetImgeSample(IntPtr data, int w, int h, int stride, int ch, RectRotateCli rr, Nullable<RectRotateCli> rrMask, bool NoCrop,
			[System::Runtime::InteropServices::Out] int% outW,
			[System::Runtime::InteropServices::Out] int% outH,
			[System::Runtime::InteropServices::Out] int% outStride,
			[System::Runtime::InteropServices::Out] int% outChannels);
		void LearnPattern();
		void LearnPatternStable();
		void LearnPatternStable(Pattern2StableConfig cfg);
		void FreeBuffer(System::IntPtr p);
		void SetGpuEnabled(bool enable);
		static bool IsGpuAvailable();
		System::IntPtr PreviewPreprocessed(
			IntPtr data, int w, int h, int stride, int ch,
			Pattern2PreprocessConfig cfg,
			int outputKind,
			[System::Runtime::InteropServices::Out] int% outW,
			[System::Runtime::InteropServices::Out] int% outH,
			[System::Runtime::InteropServices::Out] int% outStride,
			[System::Runtime::InteropServices::Out] int% outChannels);
		static Pattern2PreprocessConfig PresetGeneralGray();
		static Pattern2PreprocessConfig PresetUnevenLighting();
		static Pattern2PreprocessConfig PresetMetalShiny();
		static Pattern2PreprocessConfig PresetPCBOrText();
		static Pattern2PreprocessConfig PresetLowContrast();
		List<Rotaterectangle>^ Match(
			bool m_bStopLayer1,
			double m_dToleranceAngle,
			double m_dTolerance1,
			double m_dTolerance2,
			double m_dScore,
			bool m_ckSIMD,
			bool m_ckBitwiseNot,
			bool m_bSubPixel,
			int m_iMaxPos,
			double m_dMaxOverlap,
			bool useMultiThread,
			int numThreads
		);
		List<Rotaterectangle>^ MatchStable(Pattern2StableConfig cfg);

	private:
		CommonPlus* com;
		Img* img;
	};
}
