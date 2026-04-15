#pragma once
#include <opencv2/imgproc/types_c.h>
#include <opencv2/core/utility.hpp>
#include <opencv2/core.hpp>
#include <opencv2/imgproc.hpp>
#include <opencv2/imgcodecs.hpp>
#include <vector>
#include <cmath>
#include <limits>


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
	public value struct PatternMatchOptions
	{
		bool EnableScoreStability;
		// master switch
		bool EnableValidate;
		// soft anti-text (entropy penalty)
		bool EnableSoftAntiText;
		double EntropyWeight;   // 0..1+ (0 disables effect)
		// min-edge validation (estimated strong edges)
		bool EnableMinEdge;
		double MinEdgeRatio;    // 0..1 (e.g. 0.6)
		// multi-scale consistency (compare coarse vs final score)
		bool EnableMultiScaleCheck;
		double MaxScaleScoreDiff; // in score unit 0..1 (e.g. 0.12)

		PatternMatchOptions(bool init)
		{
			EnableScoreStability = true;
			EnableValidate = true;
			EnableSoftAntiText = false;
			EntropyWeight = 0.5;
			EnableMinEdge = false;
			MinEdgeRatio = 0.6;
			EnableMultiScaleCheck = false;
			MaxScaleScoreDiff = 0.12;
		}
	};
	// ===== Struct native =====
	// ===== Struct native =====
	public struct s_TemplData
	{
		vector<Mat> vecPyramid;
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
		void clear()
		{
			vector<Mat>().swap(vecPyramid);
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
		//cv::Mat EnhanceForPatternStrong(const cv::Mat& src);

		void ClearStableScaleBank()
		{
			std::vector<s_StableScaleTemplate>().swap(stableScaleBank);
		}

		// pipeline
		void BuildTemplatePyramid();
	public:int GetTopLayer(Mat* matTempl, int iMinDstLength);
		  void MatchTemplate(cv::Mat& matSrc, s_TemplData* pTemplData, cv::Mat& matResult, int iLayer, bool bUseSIMD, bool m_ckSIMD);
		  void GetRotatedROI(Mat& matSrc, cv::Size size, Point2f ptLT, double dAngle, Mat& matROI);
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

			RelaxedRawScore = 0.18;
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
