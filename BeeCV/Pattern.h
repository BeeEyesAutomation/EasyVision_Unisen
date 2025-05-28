//#include <windows.h>
//#pragma once

// MatchToolDlg.h: 標頭檔
//

#include "G.h"
namespace CvPlus {
	public struct s_TemplData
	{
		vector<Mat> vecPyramid;
		vector<Scalar> vecTemplMean;
		vector<double> vecTemplNorm;
		vector<double> vecInvArea;
		vector<BOOL> vecResultEqual1;
		BOOL bIsPatternLearned;
		int iBorderColor;
		void clear()
		{
			vector<Mat>().swap(vecPyramid);
			vector<double>().swap(vecTemplNorm);
			vector<double>().swap(vecInvArea);
			vector<Scalar>().swap(vecTemplMean);
			vector<BOOL>().swap(vecResultEqual1);
		}
		void resize(int iSize)
		{
			vecTemplMean.resize(iSize);
			vecTemplNorm.resize(iSize, 0);
			vecInvArea.resize(iSize, 1);
			vecResultEqual1.resize(iSize, FALSE);
		}
		s_TemplData()
		{
			bIsPatternLearned = FALSE;
		}
	};
	public struct s_MatchParameter
	{
		Point2d pt;
		double dMatchScore;
		double dMatchAngle;
		//Mat matRotatedSrc;
		Rect rectRoi;
		double dAngleStart;
		double dAngleEnd;
		RotatedRect rectR;
		Rect rectBounding;
		BOOL bDelete;

		double vecResult[3][3];//for subpixel
		int iMaxScoreIndex;//for subpixel
		BOOL bPosOnBorder;
		Point2d ptSubPixel;
		double dNewAngle;

		s_MatchParameter(Point2f ptMinMax, double dScore, double dAngle)//, Mat matRotatedSrc = Mat ())
		{
			pt = ptMinMax;
			dMatchScore = dScore;
			dMatchAngle = dAngle;

			bDelete = FALSE;
			dNewAngle = 0.0;

			bPosOnBorder = FALSE;
		}
		s_MatchParameter()
		{
			double dMatchScore = 0;
			double dMatchAngle = 0;
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
			{}
			Block(Rect rect_, double dMax_, cv::Point ptMaxLoc_)
			{
				rect = rect_;
				dMax = dMax_;
				ptMaxLoc = ptMaxLoc_;
			}
		};
		s_BlockMax()
		{}
		vector<Block> vecBlock;
		Mat matSrc;
		s_BlockMax(Mat matSrc_, cv::Size sizeTemplate)
		{
			matSrc = matSrc_;
			//將matSrc 拆成數個block，分別計算最大值
			int iBlockW = sizeTemplate.width * 2;
			int iBlockH = sizeTemplate.height * 2;

			int iCol = matSrc.cols / iBlockW;
			BOOL bHResidue = matSrc.cols % iBlockW != 0;

			int iRow = matSrc.rows / iBlockH;
			BOOL bVResidue = matSrc.rows % iBlockH != 0;

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
	
		
	
		
	
	
		double m_dSrcScale;
		double m_dDstScale;
		CPoint m_ptMouseMove;
		vector< s_TemplData> m_TemplData;
	

		
			vector<s_SingleTargetMatch> m_vecSingleTargetData;
			CString m_strExecureTime;
		
		BOOL m_bShowResult;
	

		CListCtrl m_listMsg;
		BOOL m_bDebugMode;
	
	
		//縮放
		int m_iScaleTimes;
		double m_dNewScale=1;

		BOOL bInitial;
		int m_iDlgOrgWidth;
		int m_iDlgOrgHeight;
		int m_iScreenWidth;
		int m_iScreenHeight;
		float m_fMaxScaleX;
		float m_fMaxScaleY;
		CList<CRect, CRect> m_listRect;
	//	afx_msg void OnSize(UINT nType, int cx, int cy);
		CStatic m_staticMaxPos;
		CFont m_font;
		//afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
		BOOL m_ckOutputROI;
		
		
		
		CComboBox m_cbLanSelect;
		CString m_strLanIndex;
		CString m_strLanScore;
		CString m_strLanAngle;
		CString m_strLanPosX;
		CString m_strLanPosY;
		CString m_strLanExecutionTime;
		CString m_strLanSourceImageSize;
		CString m_strLanDstImageSize;
		CString m_strLanPixelPos;
		CString m_strTotalNum;
		public ref  class  Pattern
	{
	
	public: float cycleOutLine = 0;
	public:int GetTopLayer(Mat* matTempl, int iMinDstLength);
	public:void LearnPattern( int m_iMinReduceArea,int ixTemp);

	
	public:	 bool IsProcess;


	public:	double m_dToleranceAngle ;
	public:int m_iMinReduceArea =256;
	public:	 int m_iMessageCount=1 ;
	public:System::Collections::Generic::List<System::String^>^ listMatch = gcnew System::Collections::Generic::List<System::String^>();
	public: void CreateTemp();
	public: int ScoreRS=0;
	public : bool Match(
		 int ixTemp,
		bool m_bStopLayer1,
		double m_dTolerance1 ,
		double m_dTolerance2 ,
		double m_dScore,
		int threshMin ,
		int threshMax,
		bool m_ckSIMD ,
		bool m_ckBitwiseNot,
		bool m_bSubPixel ,
		int m_iMaxPos ,
		double m_dMaxOverlap 

	);
	
		
		
		  void MatchTemplate(cv::Mat& matSrc, s_TemplData* pTemplData, cv::Mat& matResult, int iLayer, BOOL bUseSIMD,  BOOL m_ckSIMD);
		  void GetRotatedROI(Mat& matSrc, cv::Size size, Point2f ptLT, double dAngle, Mat& matROI);
		  void CCOEFF_Denominator(cv::Mat& matSrc, s_TemplData* pTemplData, cv::Mat& matResult, int iLayer);
		  cv::Size  GetBestRotationSize(cv::Size sizeSrc, cv::Size sizeDst, double dRAngle);
		  Point2f ptRotatePt2f(Point2f ptInput, Point2f ptOrg, double dAngle);
		  void FilterWithScore(vector<s_MatchParameter>* vec, double dScore);
	    void FilterWithRotatedRect(vector<s_MatchParameter>* vec, int iMethod , double dMaxOverLap );//= CV_TM_CCOEFF_NORMED,= 0
		  cv::Point GetNextMaxLoc(Mat& matResult, cv::Point ptMaxLoc, cv::Size sizeTemplate, double& dMaxValue, double dMaxOverlap);
		  cv::Point GetNextMaxLoc(Mat& matResult, cv::Point ptMaxLoc, cv::Size sizeTemplate, double& dMaxValue, double dMaxOverlap, s_BlockMax& blockMax);
		  void SortPtWithCenter(vector<Point2f>& vecSort);
		  BOOL SubPixEsimation(vector<s_MatchParameter>* vec, double* dX, double* dY, double* dAngle, double dAngleStep, int iMaxScoreIndex);
		  void DrawDashLine(Mat& matDraw, cv::Point ptStart, cv::Point ptEnd, Scalar color1 , Scalar color2);//= Scalar(0, 0, 255)  = Scalar::all(255)
		  void DrawMarkCross(Mat& matDraw, int iX, int iY, int iLength, Scalar color, int iThickness);
		  LRESULT OnShowMSG(WPARAM wMSGPointer, LPARAM lIsShowTime,int m_iMessageCount);

		 
		///  Mat RefreshSrcView(Mat matDraw, Mat raw, cv::Rect myROI,int ixTemp);
	};
	
}

