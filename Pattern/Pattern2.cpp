#include "Pattern2.h"

#include <opencv2/imgproc/imgproc_c.h>

using namespace cv;
using namespace BeeCpp;
using namespace std;

using namespace System;
using namespace System::IO;
using namespace System::Text;
using namespace System::Globalization;
inline int _mm_hsum_epi32(__m128i V)      // V3 V2 V1 V0
{
	// 實測這個速度要快些，_mm_extract_epi32最慢。
	__m128i T = _mm_add_epi32(V, _mm_srli_si128(V, 8));  // V3+V1   V2+V0  V1  V0  
	T = _mm_add_epi32(T, _mm_srli_si128(T, 4));    // V3+V1+V2+V0  V2+V0+V1 V1+V0 V0 
	return _mm_cvtsi128_si32(T);       // 提取低位 
}
inline int IM_Conv_SIMD(unsigned char* pCharKernel, unsigned char* pCharConv, int iLength)
{
	const int iBlockSize = 16, Block = iLength / iBlockSize;
	__m128i SumV = _mm_setzero_si128();
	__m128i Zero = _mm_setzero_si128();
	for (int Y = 0; Y < Block * iBlockSize; Y += iBlockSize)
	{
		__m128i SrcK = _mm_loadu_si128((__m128i*)(pCharKernel + Y));
		__m128i SrcC = _mm_loadu_si128((__m128i*)(pCharConv + Y));
		__m128i SrcK_L = _mm_unpacklo_epi8(SrcK, Zero);
		__m128i SrcK_H = _mm_unpackhi_epi8(SrcK, Zero);
		__m128i SrcC_L = _mm_unpacklo_epi8(SrcC, Zero);
		__m128i SrcC_H = _mm_unpackhi_epi8(SrcC, Zero);
		__m128i SumT = _mm_add_epi32(_mm_madd_epi16(SrcK_L, SrcC_L), _mm_madd_epi16(SrcK_H, SrcC_H));
		SumV = _mm_add_epi32(SumV, SumT);
	}
	int Sum = _mm_hsum_epi32(SumV);
	for (int Y = Block * iBlockSize; Y < iLength; Y++)
	{
		Sum += pCharKernel[Y] * pCharConv[Y];
	}
	return Sum;
}
bool compareScoreBig2Small(const s_MatchParameter& lhs, const s_MatchParameter& rhs) { return  lhs.dMatchScore > rhs.dMatchScore; }
bool comparePtWithAngle(const pair<Point2f, double> lhs, const pair<Point2f, double> rhs) { return lhs.second < rhs.second; }
bool compareMatchResultByPos(const s_SingleTargetMatch& lhs, const s_SingleTargetMatch& rhs)
{
	double dTol = 2;
	if (fabs(lhs.ptCenter.y - rhs.ptCenter.y) <= dTol)
		return lhs.ptCenter.x < rhs.ptCenter.x;
	else
		return lhs.ptCenter.y < rhs.ptCenter.y;

};
bool compareMatchResultByScore(const s_SingleTargetMatch& lhs, const s_SingleTargetMatch& rhs) { return lhs.dMatchScore > rhs.dMatchScore; }
bool compareMatchResultByPosX(const s_SingleTargetMatch& lhs, const s_SingleTargetMatch& rhs) { return lhs.ptCenter.x < rhs.ptCenter.x; }



// ===== Img implement =====
void Img::BuildTemplatePyramid()
{
	Mat raw = matSample.clone();

	int iTopLayer = GetTopLayer(&raw, (int)sqrt((double)m_iMinReduceArea));
	buildPyramid(raw, m_TemplData.vecPyramid, iTopLayer);
	s_TemplData* templData = &m_TemplData;
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
	m_TemplData.vecPyramid.clear();
	if (matSample.empty()) { m_TemplData.bIsPatternLearned = false; return; }

	cv::Mat gray = matSample;
	if (gray.type() == CV_8UC3) cv::cvtColor(gray, gray, cv::COLOR_BGR2GRAY);

	m_TemplData.vecPyramid.push_back(gray.clone()); // level 0

	cv::Mat cur = gray.clone();
	while (true) {
		cv::Mat next;
		cv::pyrDown(cur, next);
		if (next.cols < 16 || next.rows < 16) break;
		if (next.cols * next.rows < m_iMinReduceArea) break;
		m_TemplData.vecPyramid.push_back(next);
		cur = next;
	}

	if (m_TemplData.vecPyramid.empty())
		m_TemplData.vecPyramid.push_back(gray);

	m_TemplData.bIsPatternLearned = true;
	m_TemplData.iBorderColor = 0;
}

bool Img::SubPixEsimation(vector<s_MatchParameter>* vec, double* dNewX, double* dNewY, double* dNewAngle, double dAngleStep, int iMaxScoreIndex)
{
	//Az=S, (A.T)Az=(A.T)s, z = ((A.T)A).inv (A.T)s

	Mat matA(27, 10, CV_64F);
	Mat matZ(10, 1, CV_64F);
	Mat matS(27, 1, CV_64F);

	double dX_maxScore = (*vec)[iMaxScoreIndex].pt.x;
	double dY_maxScore = (*vec)[iMaxScoreIndex].pt.y;
	double dTheata_maxScore = (*vec)[iMaxScoreIndex].dMatchAngle;
	int iRow = 0;
	/*for (int x = -1; x <= 1; x++)
	{
		for (int y = -1; y <= 1; y++)
		{
			for (int theta = 0; theta <= 2; theta++)
			{*/
	for (int theta = 0; theta <= 2; theta++)
	{
		for (int y = -1; y <= 1; y++)
		{
			for (int x = -1; x <= 1; x++)
			{
				//xx yy tt xy xt yt x y t 1
				//0  1  2  3  4  5  6 7 8 9
				double dX = dX_maxScore + x;
				double dY = dY_maxScore + y;
				//double dT = (*vec)[theta].dMatchAngle + (theta - 1) * dAngleStep;
				double dT = (dTheata_maxScore + (theta - 1) * dAngleStep) * D2R;
				matA.at<double>(iRow, 0) = dX * dX;
				matA.at<double>(iRow, 1) = dY * dY;
				matA.at<double>(iRow, 2) = dT * dT;
				matA.at<double>(iRow, 3) = dX * dY;
				matA.at<double>(iRow, 4) = dX * dT;
				matA.at<double>(iRow, 5) = dY * dT;
				matA.at<double>(iRow, 6) = dX;
				matA.at<double>(iRow, 7) = dY;
				matA.at<double>(iRow, 8) = dT;
				matA.at<double>(iRow, 9) = 1.0;
				matS.at<double>(iRow, 0) = (*vec)[iMaxScoreIndex + (theta - 1)].vecResult[x + 1][y + 1];
				iRow++;
#ifdef _DEBUG
				/*string str = format ("%.6f, %.6f, %.6f, %.6f, %.6f, %.6f, %.6f, %.6f, %.6f, %.6f", dValueA[0], dValueA[1], dValueA[2], dValueA[3], dValueA[4], dValueA[5], dValueA[6], dValueA[7], dValueA[8], dValueA[9]);
				fileA <<  str << endl;
				str = format ("%.6f", dValueS[iRow]);
				fileS << str << endl;*/
#endif
			}
		}
	}
	//求解Z矩陣，得到k0~k9
	//[ x* ] = [ 2k0 k3 k4 ]-1 [ -k6 ]
	//| y* | = | k3 2k1 k5 |   | -k7 |
	//[ t* ] = [ k4 k5 2k2 ]   [ -k8 ]

	//solve (matA, matS, matZ, DECOMP_SVD);
	matZ = (matA.t() * matA).inv() * matA.t() * matS;
	Mat matZ_t;
	transpose(matZ, matZ_t);
	double* dZ = matZ_t.ptr<double>(0);
	Mat matK1 = (Mat_<double>(3, 3) <<
		(2 * dZ[0]), dZ[3], dZ[4],
		dZ[3], (2 * dZ[1]), dZ[5],
		dZ[4], dZ[5], (2 * dZ[2]));
	Mat matK2 = (Mat_<double>(3, 1) << -dZ[6], -dZ[7], -dZ[8]);
	Mat matDelta = matK1.inv() * matK2;

	*dNewX = matDelta.at<double>(0, 0);
	*dNewY = matDelta.at<double>(1, 0);
	*dNewAngle = matDelta.at<double>(2, 0) * R2D;
	return true;
}

int Img::GetTopLayer(Mat* matTempl, int iMinDstLength)
{
	int iTopLayer = 0;
	int iMinReduceArea = iMinDstLength * iMinDstLength;
	int iArea = matTempl->cols * matTempl->rows;
	while (iArea > iMinReduceArea)
	{
		iArea /= 4;
		iTopLayer++;
	}
	return iTopLayer;
}
void Img::MatchTemplate(cv::Mat& matSrc, s_TemplData* pTemplData, cv::Mat& matResult, int iLayer, bool bUseSIMD, bool m_ckSIMD)
{
	if (m_ckSIMD && bUseSIMD)
	{
		//From ImageShop
		matResult.create(matSrc.rows - pTemplData->vecPyramid[iLayer].rows + 1,
			matSrc.cols - pTemplData->vecPyramid[iLayer].cols + 1, CV_32FC1);
		matResult.setTo(0);
		cv::Mat& matTemplate = pTemplData->vecPyramid[iLayer];
		int  t_r_end = matTemplate.rows, t_r = 0;
		for (int r = 0; r < matResult.rows; r++)
		{
			float* r_matResult = matResult.ptr<float>(r);
			uchar* r_source = matSrc.ptr<uchar>(r);
			uchar* r_template, * r_sub_source;
			for (int c = 0; c < matResult.cols; ++c, ++r_matResult, ++r_source)
			{
				r_template = matTemplate.ptr<uchar>();
				r_sub_source = r_source;
				for (t_r = 0; t_r < t_r_end; ++t_r, r_sub_source += matSrc.cols, r_template += matTemplate.cols)
				{
					*r_matResult = *r_matResult + IM_Conv_SIMD(r_template, r_sub_source, matTemplate.cols);
				}
			}
		}
		//From ImageShop
	}
	else
		matchTemplate(matSrc, pTemplData->vecPyramid[iLayer], matResult, CV_TM_CCORR);

	/*Mat diff;
	absdiff(matResult, matResult, diff);
	double dMaxValue;
	minMaxLoc(diff, 0, &dMaxValue, 0,0);*/
	CCOEFF_Denominator(matSrc, pTemplData, matResult, iLayer);
}
void Img::GetRotatedROI(Mat& matSrc, cv::Size size, Point2f ptLT, double dAngle, Mat& matROI)
{
	double dAngle_radian = dAngle * D2R;
	Point2f ptC((matSrc.cols - 1) / 2.0f, (matSrc.rows - 1) / 2.0f);
	Point2f ptLT_rotate = ptRotatePt2f(ptLT, ptC, dAngle_radian);
	cv::Size sizePadding(size.width + 6, size.height + 6);


	Mat rMat = getRotationMatrix2D(ptC, dAngle, 1);
	rMat.at<double>(0, 2) -= ptLT_rotate.x - 3;
	rMat.at<double>(1, 2) -= ptLT_rotate.y - 3;
	//平移旋轉矩陣(0, 2) (1, 2)的減，為旋轉後的圖形偏移，-= ptLT_rotate.x - 3 代表旋轉後的圖形往-X方向移動ptLT_rotate.x - 3
	//Debug

	//Debug
	warpAffine(matSrc, matROI, rMat, sizePadding);
}
void Img::CCOEFF_Denominator(cv::Mat& matSrc, s_TemplData* pTemplData, cv::Mat& matResult, int iLayer)
{
	if (pTemplData->vecResultEqual1[iLayer])
	{
		matResult = Scalar::all(1);
		return;
	}
	double* q0 = 0, * q1 = 0, * q2 = 0, * q3 = 0;

	Mat sum, sqsum;
	integral(matSrc, sum, sqsum, CV_64F);

	q0 = (double*)sqsum.data;
	q1 = q0 + pTemplData->vecPyramid[iLayer].cols;
	q2 = (double*)(sqsum.data + pTemplData->vecPyramid[iLayer].rows * sqsum.step);
	q3 = q2 + pTemplData->vecPyramid[iLayer].cols;

	double* p0 = (double*)sum.data;
	double* p1 = p0 + pTemplData->vecPyramid[iLayer].cols;
	double* p2 = (double*)(sum.data + pTemplData->vecPyramid[iLayer].rows * sum.step);
	double* p3 = p2 + pTemplData->vecPyramid[iLayer].cols;

	int sumstep = sum.data ? (int)(sum.step / sizeof(double)) : 0;
	int sqstep = sqsum.data ? (int)(sqsum.step / sizeof(double)) : 0;

	//
	double dTemplMean0 = pTemplData->vecTemplMean[iLayer][0];
	double dTemplNorm = pTemplData->vecTemplNorm[iLayer];
	double dInvArea = pTemplData->vecInvArea[iLayer];
	//

	int i, j;
	for (i = 0; i < matResult.rows; i++)
	{
		float* rrow = matResult.ptr<float>(i);
		int idx = i * sumstep;
		int idx2 = i * sqstep;

		for (j = 0; j < matResult.cols; j += 1, idx += 1, idx2 += 1)
		{
			double num = rrow[j], t;
			double wndMean2 = 0, wndSum2 = 0;

			t = p0[idx] - p1[idx] - p2[idx] + p3[idx];
			wndMean2 += t * t;
			num -= t * dTemplMean0;
			wndMean2 *= dInvArea;


			t = q0[idx2] - q1[idx2] - q2[idx2] + q3[idx2];
			wndSum2 += t;


			//t = std::sqrt (MAX (wndSum2 - wndMean2, 0)) * dTemplNorm;

			double diff2 = MAX(wndSum2 - wndMean2, 0);
			if (diff2 <= std::min(0.5, 10 * FLT_EPSILON * wndSum2))
				t = 0; // avoid rounding errors
			else
				t = std::sqrt(diff2) * dTemplNorm;

			if (fabs(num) < t)
				num /= t;
			else if (fabs(num) < t * 1.125)
				num = num > 0 ? 1 : -1;
			else
				num = 0;

			rrow[j] = (float)num;
		}
	}
}
cv::Size Img::GetBestRotationSize(cv::Size sizeSrc, cv::Size sizeDst, double dRAngle)
{
	double dRAngle_radian = dRAngle * D2R;
	cv::Point ptLT(0, 0), ptLB(0, sizeSrc.height - 1), ptRB(sizeSrc.width - 1, sizeSrc.height - 1), ptRT(sizeSrc.width - 1, 0);
	Point2f ptCenter((sizeSrc.width - 1) / 2.0f, (sizeSrc.height - 1) / 2.0f);
	Point2f ptLT_R = ptRotatePt2f(Point2f(ptLT), ptCenter, dRAngle_radian);
	Point2f ptLB_R = ptRotatePt2f(Point2f(ptLB), ptCenter, dRAngle_radian);
	Point2f ptRB_R = ptRotatePt2f(Point2f(ptRB), ptCenter, dRAngle_radian);
	Point2f ptRT_R = ptRotatePt2f(Point2f(ptRT), ptCenter, dRAngle_radian);

	float fTopY = max(max(ptLT_R.y, ptLB_R.y), max(ptRB_R.y, ptRT_R.y));
	float fBottomY = min(min(ptLT_R.y, ptLB_R.y), min(ptRB_R.y, ptRT_R.y));
	float fRightX = max(max(ptLT_R.x, ptLB_R.x), max(ptRB_R.x, ptRT_R.x));
	float fLeftX = min(min(ptLT_R.x, ptLB_R.x), min(ptRB_R.x, ptRT_R.x));

	if (dRAngle > 360)
		dRAngle -= 360;
	else if (dRAngle < 0)
		dRAngle += 360;

	if (fabs(fabs(dRAngle) - 90) < VISION_TOLERANCE || fabs(fabs(dRAngle) - 270) < VISION_TOLERANCE)
	{
		return cv::Size(sizeSrc.height, sizeSrc.width);
	}
	else if (fabs(dRAngle) < VISION_TOLERANCE || fabs(fabs(dRAngle) - 180) < VISION_TOLERANCE)
	{
		return sizeSrc;
	}

	double dAngle = dRAngle;

	if (dAngle > 0 && dAngle < 90)
	{
		;
	}
	else if (dAngle > 90 && dAngle < 180)
	{
		dAngle -= 90;
	}
	else if (dAngle > 180 && dAngle < 270)
	{
		dAngle -= 180;
	}
	else if (dAngle > 270 && dAngle < 360)
	{
		dAngle -= 270;
	}
	else//Debug
	{

	}

	float fH1 = sizeDst.width * sin(dAngle * D2R) * cos(dAngle * D2R);
	float fH2 = sizeDst.height * sin(dAngle * D2R) * cos(dAngle * D2R);

	int iHalfHeight = (int)ceil(fTopY - ptCenter.y - fH1);
	int iHalfWidth = (int)ceil(fRightX - ptCenter.x - fH2);

	cv::Size sizeRet(iHalfWidth * 2, iHalfHeight * 2);

	bool bWrongSize = (sizeDst.width < sizeRet.width && sizeDst.height > sizeRet.height)
		|| (sizeDst.width > sizeRet.width && sizeDst.height < sizeRet.height
			|| sizeDst.area() > sizeRet.area());
	if (bWrongSize)
		sizeRet = cv::Size(int(fRightX - fLeftX + 0.5), int(fTopY - fBottomY + 0.5));

	return sizeRet;
}
Point2f Img::ptRotatePt2f(Point2f ptInput, Point2f ptOrg, double dAngle)
{
	double dWidth = ptOrg.x * 2;
	double dHeight = ptOrg.y * 2;
	double dY1 = dHeight - ptInput.y, dY2 = dHeight - ptOrg.y;

	double dX = (ptInput.x - ptOrg.x) * cos(dAngle) - (dY1 - ptOrg.y) * sin(dAngle) + ptOrg.x;
	double dY = (ptInput.x - ptOrg.x) * sin(dAngle) + (dY1 - ptOrg.y) * cos(dAngle) + dY2;

	dY = -dY + dHeight;
	return Point2f((float)dX, (float)dY);
}
void Img::FilterWithScore(vector<s_MatchParameter>* vec, double dScore)
{
	std::sort(vec->begin(), vec->end(), compareScoreBig2Small);
	int iSize = vec->size(), iIndexDelete = iSize + 1;
	if (iSize > 0)
	{
		double score = (*vec)[0].dMatchScore * 100;

	}


	for (int i = 0; i < iSize; i++)
	{
		if ((*vec)[i].dMatchScore < dScore)
		{
			iIndexDelete = i;
			break;
		}
	}
	if (iIndexDelete == iSize + 1)//沒有任何元素小於dScore
		return;
	vec->erase(vec->begin() + iIndexDelete, vec->end());
	return;
}
void Img::FilterWithRotatedRect(vector<s_MatchParameter>* vec, int iMethod, double dMaxOverLap)
{
	int iMatchSize = (int)vec->size();
	RotatedRect rect1, rect2;
	for (int i = 0; i < iMatchSize - 1; i++)
	{
		if (vec->at(i).bDelete)
			continue;
		for (int j = i + 1; j < iMatchSize; j++)
		{
			if (vec->at(j).bDelete)
				continue;
			rect1 = vec->at(i).rectR;
			rect2 = vec->at(j).rectR;
			vector<Point2f> vecInterSec;
			int iInterSecType = rotatedRectangleIntersection(rect1, rect2, vecInterSec);
			if (iInterSecType == INTERSECT_NONE)//無交集
				continue;
			else if (iInterSecType == INTERSECT_FULL) //一個矩形包覆另一個
			{
				int iDeleteIndex;
				if (iMethod == CV_TM_SQDIFF)
					iDeleteIndex = (vec->at(i).dMatchScore <= vec->at(j).dMatchScore) ? j : i;
				else
					iDeleteIndex = (vec->at(i).dMatchScore >= vec->at(j).dMatchScore) ? j : i;
				vec->at(iDeleteIndex).bDelete = true;
			}
			else//交點 > 0
			{
				if (vecInterSec.size() < 3)//一個或兩個交點
					continue;
				else
				{
					int iDeleteIndex;
					//求面積與交疊比例
					SortPtWithCenter(vecInterSec);
					double dArea = contourArea(vecInterSec);
					double dRatio = dArea / rect1.size.area();
					//若大於最大交疊比例，選分數高的
					if (dRatio > dMaxOverLap)
					{
						if (iMethod == CV_TM_SQDIFF)
							iDeleteIndex = (vec->at(i).dMatchScore <= vec->at(j).dMatchScore) ? j : i;
						else
							iDeleteIndex = (vec->at(i).dMatchScore >= vec->at(j).dMatchScore) ? j : i;
						vec->at(iDeleteIndex).bDelete = true;
					}
				}
			}
		}
	}
	vector<s_MatchParameter>::iterator it;
	for (it = vec->begin(); it != vec->end();)
	{
		if ((*it).bDelete)
			it = vec->erase(it);
		else
			++it;
	}
}
cv::Point Img::GetNextMaxLoc(Mat& matResult, cv::Point ptMaxLoc, cv::Size sizeTemplate, double& dMaxValue, double dMaxOverlap)
{
	//比對到的區域完全不重疊 : +-一個樣板寬高
	//int iStartX = ptMaxLoc.x - iTemplateW;
	//int iStartY = ptMaxLoc.y - iTemplateH;
	//int iEndX = ptMaxLoc.x + iTemplateW;

	//int iEndY = ptMaxLoc.y + iTemplateH;
	////塗黑
	//rectangle (matResult, Rect (iStartX, iStartY, 2 * iTemplateW * (1-dMaxOverlap * 2), 2 * iTemplateH * (1-dMaxOverlap * 2)), Scalar (dMinValue), CV_FILLED);
	////得到下一個最大值
	//cv::Point ptNewMaxLoc;
	//minMaxLoc (matResult, 0, &dMaxValue, 0, &ptNewMaxLoc);
	//return ptNewMaxLoc;

	//比對到的區域需考慮重疊比例
	int iStartX = ptMaxLoc.x - sizeTemplate.width * (1 - dMaxOverlap);
	int iStartY = ptMaxLoc.y - sizeTemplate.height * (1 - dMaxOverlap);
	//塗黑
	rectangle(matResult, Rect(iStartX, iStartY, 2 * sizeTemplate.width * (1 - dMaxOverlap), 2 * sizeTemplate.height * (1 - dMaxOverlap)), Scalar(-1), CV_FILLED);
	//得到下一個最大值
	cv::Point ptNewMaxLoc;
	minMaxLoc(matResult, 0, &dMaxValue, 0, &ptNewMaxLoc);
	return ptNewMaxLoc;
}
cv::Point Img::GetNextMaxLoc(Mat& matResult, cv::Point ptMaxLoc, cv::Size sizeTemplate, double& dMaxValue, double dMaxOverlap, s_BlockMax& blockMax)
{
	//比對到的區域需考慮重疊比例
	int iStartX = int(ptMaxLoc.x - sizeTemplate.width * (1 - dMaxOverlap));
	int iStartY = int(ptMaxLoc.y - sizeTemplate.height * (1 - dMaxOverlap));
	Rect rectIgnore(iStartX, iStartY, int(2 * sizeTemplate.width * (1 - dMaxOverlap))
		, int(2 * sizeTemplate.height * (1 - dMaxOverlap)));
	//塗黑
	rectangle(matResult, rectIgnore, Scalar(-1), CV_FILLED);
	blockMax.UpdateMax(rectIgnore);
	cv::Point ptReturn;
	blockMax.GetMaxValueLoc(dMaxValue, ptReturn);
	return ptReturn;
}
void Img::SortPtWithCenter(vector<Point2f>& vecSort)
{
	int iSize = (int)vecSort.size();
	Point2f ptCenter(0, 0);
	for (int i = 0; i < iSize; i++)
		ptCenter += vecSort[i];
	ptCenter /= iSize;

	Point2f vecX(1, 0);

	vector<pair<Point2f, double>> vecPtAngle(iSize);
	for (int i = 0; i < iSize; i++)
	{
		vecPtAngle[i].first = vecSort[i];//pt
		Point2f vec1(vecSort[i].x - ptCenter.x, vecSort[i].y - ptCenter.y);
		float fNormVec1 = std::sqrt(vec1.x * vec1.x + vec1.y * vec1.y);
		float fDot = vec1.x;

		if (vec1.y < 0)//若點在中心的上方
		{
			vecPtAngle[i].second = acos(fDot / fNormVec1) * R2D;
		}
		else if (vec1.y > 0)//下方
		{
			vecPtAngle[i].second = 360 - acos(fDot / fNormVec1) * R2D;
		}
		else//點與中心在相同Y
		{
			if (vec1.x - ptCenter.x > 0)
				vecPtAngle[i].second = 0;
			else
				vecPtAngle[i].second = 180;
		}

	}
	std::sort(vecPtAngle.begin(), vecPtAngle.end(), comparePtWithAngle);
	for (int i = 0; i < iSize; i++)
		vecSort[i] = vecPtAngle[i].first;
}

// ===== Helpers (không dùng lambda) =====
// anonymous namespace

// ===== Pattern wrapper =====
Pattern2::Pattern2() {
	img = new Img(); com = new CommonPlus();
}
Pattern2::~Pattern2() { this->!Pattern2(); }
Pattern2::!Pattern2() { if (img) { delete img; img = nullptr; } if (com) { delete com; com = nullptr; } }
void Pattern2::SetImgeSampleNoCrop(IntPtr data, int w, int h, int stride, int ch)
{
	if (data == IntPtr::Zero || w <= 0 || h <= 0 || stride <= 0)
	{
		img->matSample.release();
		return;
	}

	int type = 0;
	switch (ch)
	{
	case 1: type = CV_8UC1; break;
	case 3: type = CV_8UC3; break;
	case 4: type = CV_8UC4; break;
	default:
		img->matSample.release();
		return;
	}

	// stride phải là số byte mỗi dòng
	cv::Mat src(h, w, type, data.ToPointer(), (size_t)stride);

	// clone để giữ dữ liệu riêng, tránh buffer gốc bị đổi/mất
	cv::Mat tmp = src.clone();

	// nếu cần đưa về gray
	if (ch == 3)
		cv::cvtColor(tmp, tmp, cv::COLOR_BGR2GRAY);
	else if (ch == 4)
		cv::cvtColor(tmp, tmp, cv::COLOR_BGRA2GRAY);

	// tmp = img->EnhanceForPatternStrong(tmp);

	img->matSample = tmp;
	img->ClearStableScaleBank();
	img->m_TemplData.clear();
	img->m_TemplData.bIsPatternLearned = false;
}
// === PUBLIC API ===
System::IntPtr Pattern2::SetImgeSample(System::IntPtr tplData, int tplW, int tplH, int tplStride, int tplChannels, RectRotateCli rr, Nullable<RectRotateCli> rrMask, bool NoCrop,
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
			rr, mask, /*returnMaskOnly*/ false, 0,
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
	//img->matSample = img->EnhanceForPatternStrong(img->matSample);
	const int W = img->matSample.cols, H = img->matSample.rows, C = img->matSample.channels();
	const int S = (int)img->matSample.step;
	const size_t bytes = (size_t)S * H;

	IntPtr mem = System::Runtime::InteropServices::Marshal::AllocHGlobal((IntPtr)(long long)bytes);
	if (mem == IntPtr::Zero) return IntPtr::Zero;
	std::memcpy(mem.ToPointer(), img->matSample.data, bytes);
	outW = W; outH = H; outStride = S; outChannels = C;
	img->ClearStableScaleBank();
	img->m_TemplData.clear();
	img->m_TemplData.bIsPatternLearned = false;
	return mem;
}
void Pattern2::SetImgeRaw(System::IntPtr tplData, int tplW, int tplH, int tplStride, int tplChannels, RectRotateCli rr, Nullable<RectRotateCli> rrMask)
{
	Nullable<RectRotateCli> mask =
		rrMask.HasValue ? Nullable<RectRotateCli>(rrMask.Value)
		: Nullable<RectRotateCli>();

	com->CropRotToMat(
		tplData, tplW, tplH, tplStride, tplChannels,
		rr, mask, /*returnMaskOnly*/ false, 0,
		System::IntPtr(&img->matRaw)
	);
	//img->matRaw = img->EnhanceForPatternStrong(img->matRaw);
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
	//img->matRaw=	img->EnhanceForPatternStrong(img->matRaw);
	//img->matRaw = Mat (h, w, CV_8UC1, data.ToPointer(), stride);

}static inline int ClampI(int v, int lo, int hi)
{
	return (v < lo) ? lo : (v > hi ? hi : v);
}


namespace
{
	static inline double Clamp01(double v)
	{
		return (v < 0.0) ? 0.0 : (v > 1.0 ? 1.0 : v);
	}

	static cv::Mat ToGray8U(const cv::Mat& src)
	{
		if (src.empty()) return cv::Mat();

		cv::Mat gray;
		if (src.channels() == 1) gray = src.clone();
		else if (src.channels() == 3) cv::cvtColor(src, gray, cv::COLOR_BGR2GRAY);
		else if (src.channels() == 4) cv::cvtColor(src, gray, cv::COLOR_BGRA2GRAY);
		else
		{
			cv::Mat tmp;
			cv::convertScaleAbs(src, tmp);
			if (tmp.channels() == 1) gray = tmp;
			else if (tmp.channels() == 3) cv::cvtColor(tmp, gray, cv::COLOR_BGR2GRAY);
			else if (tmp.channels() == 4) cv::cvtColor(tmp, gray, cv::COLOR_BGRA2GRAY);
			else gray = tmp;
		}

		if (gray.depth() != CV_8U)
			cv::convertScaleAbs(gray, gray);

		return gray;
	}

	static cv::Mat NormalizeForCompare(const cv::Mat& gray8)
	{
		cv::Mat blur, out;
		cv::GaussianBlur(gray8, blur, cv::Size(0, 0), 2.2);
		cv::addWeighted(gray8, 1.35, blur, -0.35, 0.0, out);
		cv::equalizeHist(out, out);
		return out;
	}

	static cv::Mat GradientMag8(const cv::Mat& gray8)
	{
		cv::Mat gx, gy, mag, out;
		cv::Sobel(gray8, gx, CV_32F, 1, 0, 3);
		cv::Sobel(gray8, gy, CV_32F, 0, 1, 3);
		cv::magnitude(gx, gy, mag);

		double maxv = 0.0;
		cv::minMaxLoc(mag, nullptr, &maxv, nullptr, nullptr);
		if (maxv <= 1e-6)
			return cv::Mat::zeros(gray8.size(), CV_8UC1);

		mag.convertTo(out, CV_8UC1, 255.0 / maxv);
		return out;
	}

	static cv::Mat EdgeMaskFromGrad(const cv::Mat& grad8)
	{
		cv::Mat bin;
		cv::threshold(grad8, bin, 0, 255, cv::THRESH_BINARY | cv::THRESH_OTSU);
		cv::morphologyEx(
			bin, bin, cv::MORPH_CLOSE,
			cv::getStructuringElement(cv::MORPH_RECT, cv::Size(3, 3)));
		return bin;
	}

	static double ScoreSameSizeNCC(const cv::Mat& templ, const cv::Mat& roi)
	{
		if (templ.empty() || roi.empty()) return -1.0;
		if (templ.size() != roi.size()) return -1.0;
		if (templ.type() != CV_8UC1 || roi.type() != CV_8UC1) return -1.0;

		cv::Mat res;
		cv::matchTemplate(roi, templ, res, cv::TM_CCOEFF_NORMED);

		double maxv = -1.0;
		cv::minMaxLoc(res, nullptr, &maxv, nullptr, nullptr);
		return std::isfinite(maxv) ? maxv : -1.0;
	}

	static double EdgeIoU(const cv::Mat& a, const cv::Mat& b)
	{
		if (a.empty() || b.empty()) return 0.0;
		if (a.size() != b.size()) return 0.0;

		cv::Mat inters, uni;
		cv::bitwise_and(a, b, inters);
		cv::bitwise_or(a, b, uni);

		const int interCnt = cv::countNonZero(inters);
		const int unionCnt = cv::countNonZero(uni);
		if (unionCnt <= 0) return 0.0;

		return (double)interCnt / (double)unionCnt;
	}

	static double RatioSimilarity(int a, int b)
	{
		if (a <= 0 || b <= 0) return 0.0;
		return (double)std::min(a, b) / (double)std::max(a, b);
	}

	static double Entropy8U(const cv::Mat& gray8)
	{
		if (gray8.empty() || gray8.type() != CV_8UC1) return 0.0;

		int hist[256] = { 0 };
		for (int y = 0; y < gray8.rows; ++y)
		{
			const uchar* p = gray8.ptr<uchar>(y);
			for (int x = 0; x < gray8.cols; ++x) hist[p[x]]++;
		}

		const double invN = 1.0 / std::max<size_t>(1, gray8.total());
		double H = 0.0;
		for (int i = 0; i < 256; ++i)
		{
			if (hist[i] == 0) continue;
			const double p = hist[i] * invN;
			H -= p * (std::log(p) / std::log(2.0));
		}
		return H;
	}


	struct AutoGate
	{
		double edgeDensity;
		double entropyNorm;
		double strongBaseScore;
		double softEdgeIou;
		double softEdgeRatio;
		double hardEdgeIou;
		double hardEdgeRatio;
		double minFused;
		double wBase;
		double wRaw;
		double wGrad;
		double wEdgeIou;
		double wEdgeRatio;
	};

	static AutoGate BuildAutoGate(int modelEdgeCount, double modelEntropy, cv::Size tplSize, double manualMinAccept)
	{
		AutoGate g{};

		const double area = std::max(1.0, (double)tplSize.area());
		const double edgeDensity = (double)std::max(0, modelEdgeCount) / area;
		const double edgeNorm = Clamp01(edgeDensity / 0.25);
		const double entropyNorm = Clamp01(modelEntropy / 8.0);

		g.edgeDensity = edgeDensity;
		g.entropyNorm = entropyNorm;

		// Base score remains the main anchor. Keep edge gates softer than before.
		g.strongBaseScore = std::max(0.86, std::min(0.92, 0.86 + 0.03 * entropyNorm));
		g.softEdgeIou = std::max(0.05, std::min(0.12, 0.04 + 0.28 * edgeDensity));
		g.hardEdgeIou = std::max(0.08, std::min(0.16, g.softEdgeIou + 0.04));

		g.softEdgeRatio = std::max(0.45, std::min(0.72, 0.42 + 0.90 * edgeDensity));
		g.hardEdgeRatio = std::max(0.58, std::min(0.82, g.softEdgeRatio + 0.12));

		g.minFused = std::max(0.28, std::min(0.42, 0.28 + 0.10 * entropyNorm));
		if (manualMinAccept > 0.0)
			g.minFused = Clamp01(manualMinAccept);

		// Weights sum to 1. finalScore will be the only returned score.
		g.wBase = 0.72;
		g.wRaw = 0.10;
		g.wGrad = 0.04;
		g.wEdgeIou = 0.10;
		g.wEdgeRatio = 0.04;

		const double wsum = g.wBase + g.wRaw + g.wGrad + g.wEdgeIou + g.wEdgeRatio;
		if (wsum > 1e-9)
		{
			g.wBase /= wsum;
			g.wRaw /= wsum;
			g.wGrad /= wsum;
			g.wEdgeIou /= wsum;
			g.wEdgeRatio /= wsum;
		}

		return g;
	}

	static std::vector<double> BuildScaleList(bool enableScaleSearch, double smin, double smax, double sstep)
	{
		std::vector<double> scales;

		if (!enableScaleSearch)
		{
			scales.push_back(1.0);
			return scales;
		}

		if (smin <= 0.0) smin = 1.0;
		if (smax <= 0.0) smax = 1.0;
		if (smax < smin) std::swap(smin, smax);

		if (sstep <= 1e-9)
		{
			scales.push_back(1.0);
			return scales;
		}

		for (double s = smin; s <= smax + 1e-9; s += sstep)
			scales.push_back(s);

		bool has1 = false;
		for (double s : scales)
		{
			if (std::abs(s - 1.0) <= std::max(1e-9, sstep * 0.5))
			{
				has1 = true;
				break;
			}
		}
		if (!has1)
			scales.push_back(1.0);

		std::sort(scales.begin(), scales.end());
		scales.erase(std::unique(scales.begin(), scales.end(), [sstep](double a, double b)
			{
				return std::abs(a - b) <= std::max(1e-9, sstep * 0.25);
			}), scales.end());

		return scales;
	}

	static void BuildStableTemplDataFromGray(
		const cv::Mat& gray,
		int minReduceArea,
		s_TemplData& outData)
	{
		outData.clear();
		outData.bIsPatternLearned = false;

		if (gray.empty())
			return;

		int iTopLayer = 0;
		int area = gray.cols * gray.rows;
		while (area > std::max(1, minReduceArea))
		{
			area /= 4;
			++iTopLayer;
		}

		cv::buildPyramid(gray, outData.vecPyramid, iTopLayer);
		outData.resize((int)outData.vecPyramid.size());

		for (int i = 0; i < (int)outData.vecPyramid.size(); ++i)
		{
			const cv::Mat& t = outData.vecPyramid[i];
			const double invArea = 1.0 / (double)(t.rows * t.cols);

			cv::Scalar meanV, stdV;
			cv::meanStdDev(t, meanV, stdV);

			double templNorm = stdV[0] * stdV[0];
			if (templNorm < DBL_EPSILON)
				outData.vecResultEqual1[i] = true;

			templNorm = std::sqrt(templNorm);
			templNorm /= std::sqrt(invArea);

			outData.vecInvArea[i] = invArea;
			outData.vecTemplMean[i] = meanV;
			outData.vecTemplNorm[i] = templNorm;
		}

		cv::Mat tplNorm = NormalizeForCompare(gray);
		cv::Mat tplGrad = GradientMag8(tplNorm);
		cv::Mat tplEdge = EdgeMaskFromGrad(tplGrad);

		outData.modelEdgeCount = cv::countNonZero(tplEdge);
		outData.modelEntropy = Entropy8U(gray);
		outData.modelEdgeDensity = (double)outData.modelEdgeCount / std::max(1, gray.rows * gray.cols);
		outData.modelEntropyNorm = Clamp01(outData.modelEntropy / 8.0);

		const AutoGate g = BuildAutoGate(outData.modelEdgeCount, outData.modelEntropy, gray.size(), 0.0);
		outData.autoStrongBaseScore = g.strongBaseScore;
		outData.autoSoftEdgeIou = g.softEdgeIou;
		outData.autoSoftEdgeRatio = g.softEdgeRatio;
		outData.autoHardEdgeIou = g.hardEdgeIou;
		outData.autoHardEdgeRatio = g.hardEdgeRatio;
		outData.autoMinFused = g.minFused;
		outData.autoWBase = g.wBase;
		outData.autoWRaw = g.wRaw;
		outData.autoWGrad = g.wGrad;
		outData.autoWEdgeIou = g.wEdgeIou;
		outData.autoWEdgeRatio = g.wEdgeRatio;

		outData.iBorderColor = cv::mean(gray).val[0] < 128 ? 255 : 0;
		outData.bIsPatternLearned = true;
	}

}


//cv::Mat Img::EnhanceForPatternStrong(const cv::Mat& src)
//{
//	if (src.empty())
//		return cv::Mat();
//
//	Mat gray;
//	if (src.channels() == 3)
//		cvtColor(src, gray, COLOR_BGR2GRAY);
//	else
//		gray = src.clone();
//
//	// 1) blur nhẹ
//	GaussianBlur(gray, gray, Size(5, 5), 0);
//
//	// 2) gradient magnitude
//	Mat gx, gy;
//	Sobel(gray, gx, CV_32F, 1, 0, 3);
//	Sobel(gray, gy, CV_32F, 0, 1, 3);
//
//	Mat mag;
//	magnitude(gx, gy, mag);
//
//	// 3) normalize để tìm biên yếu
//	normalize(mag, mag, 0, 255, NORM_MINMAX);
//	mag.convertTo(mag, CV_8U);
//
//	// 4) tìm điểm biên
//	std::vector<Point> pts;
//	findNonZero(mag, pts);
//
//	Mat out = Mat::zeros(src.size(), CV_8UC1);
//
//	if (pts.empty())
//		return out;
//
//	// 5) bounding box
//	Rect box = boundingRect(pts);
//
//	// tránh trường hợp ăn full ảnh
//	if (box.width > src.cols * 0.9 || box.height > src.rows * 0.9)
//		return out;
//	// 8) vẽ box trắng trên nền đen
//	cv::rectangle(out, box, cv::Scalar(255), FILLED);
//
//	return out;
//}
//cv::Mat Img::EnhanceForPatternStrong(const cv::Mat& src)
//{
//	cv::Mat gray, bg, norm, edge;
//
//	//-------------------------------------
//	// 1. convert gray
//	//-------------------------------------
//	if (src.channels() == 3)
//		cv::cvtColor(src, gray, cv::COLOR_BGR2GRAY);
//	else
//		gray = src.clone();
//
//	//-------------------------------------
//	// 2. remove uneven lighting
//	//-------------------------------------
//	cv::blur(gray, bg, cv::Size(61, 61));
//	cv::subtract(gray, bg, norm);
//
//	//-------------------------------------
//	// 3. edge detection
//	//-------------------------------------
//	cv::Mat gx, gy;
//
//	cv::Sobel(norm, gx, CV_16S, 1, 0, 3);
//	cv::Sobel(norm, gy, CV_16S, 0, 1, 3);
//
//	cv::convertScaleAbs(gx, gx);
//	cv::convertScaleAbs(gy, gy);
//
//	cv::addWeighted(gx, 0.5, gy, 0.5, 0, edge);
//
//	//-------------------------------------
//	// 4. threshold edge
//	//-------------------------------------
//	cv::Mat edgeBin;
//	cv::threshold(edge, edgeBin, 10, 255, cv::THRESH_BINARY);
//
//	//-------------------------------------
//	// 5. close edge gap
//	//-------------------------------------
//	cv::morphologyEx(
//		edgeBin,
//		edgeBin,
//		cv::MORPH_CLOSE,
//		cv::getStructuringElement(cv::MORPH_RECT, cv::Size(5, 5))
//	);
//
//	//-------------------------------------
//	// 6. find contours
//	//-------------------------------------
//	std::vector<std::vector<cv::Point>> contours;
//
//	cv::findContours(
//		edgeBin,
//		contours,
//		cv::RETR_EXTERNAL,
//		cv::CHAIN_APPROX_SIMPLE
//	);
//
//	//-------------------------------------
//	// 7. find largest contour
//	//-------------------------------------
//	int bestIdx = -1;
//	double bestArea = 0;
//
//	for (int i = 0; i < contours.size(); i++)
//	{
//		double area = cv::contourArea(contours[i]);
//
//		if (area > bestArea)
//		{
//			bestArea = area;
//			bestIdx = i;
//		}
//	}
//
//	//-------------------------------------
//	// 8. create output mask
//	//-------------------------------------
//	cv::Mat mask = cv::Mat::zeros(src.size(), CV_8UC1);
//
//	if (bestIdx >= 0)
//	{
//		cv::Rect box = cv::boundingRect(contours[bestIdx]);
//
//		// draw box white
//		cv::rectangle(mask, box, cv::Scalar(255), 1);
//	}
//
//	return mask;
//	
//}
void Pattern2::LearnPattern()
{
	Mat raw = img->matSample.clone();

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
	img->ClearStableScaleBank();
}


void Pattern2::LearnPatternStable()
{
	Pattern2StableConfig cfg(true);
	LearnPatternStable(cfg);
}

void Pattern2::LearnPatternStable(Pattern2StableConfig cfg)
{
	if (img == nullptr) return;

	img->ClearStableScaleBank();
	img->m_TemplData.clear();
	img->m_TemplData.bIsPatternLearned = false;

	if (img->matSample.empty())
		return;

	const cv::Mat originalGray = ToGray8U(img->matSample);
	if (originalGray.empty())
		return;

	const std::vector<double> scales =
		BuildScaleList(cfg.EnableScaleSearch, cfg.ScaleMin, cfg.ScaleMax, cfg.ScaleStep);

	int bestBaseIdx = -1;
	double bestBaseDist = DBL_MAX;

	for (double scale : scales)
	{
		cv::Mat scaledGray;
		if (std::abs(scale - 1.0) <= 1e-9)
		{
			scaledGray = originalGray.clone();
		}
		else
		{
			const int sw = std::max(8, (int)std::round(originalGray.cols * scale));
			const int sh = std::max(8, (int)std::round(originalGray.rows * scale));
			cv::resize(originalGray, scaledGray, cv::Size(sw, sh), 0, 0,
				(scale >= 1.0) ? cv::INTER_LINEAR : cv::INTER_AREA);
		}

		if (scaledGray.cols < 8 || scaledGray.rows < 8)
			continue;

		s_StableScaleTemplate item;
		item.scale = scale;
		item.tplGray = scaledGray;
		item.tplNorm = NormalizeForCompare(item.tplGray);
		item.tplGrad = GradientMag8(item.tplNorm);
		item.tplEdge = EdgeMaskFromGrad(item.tplGrad);
		item.tplEdgeCount = std::max(1, cv::countNonZero(item.tplEdge));

		BuildStableTemplDataFromGray(item.tplGray, img->m_iMinReduceArea, item.templData);

		item.thresholdFinal =
			(cfg.MinAcceptScore > 0.0)
			? Clamp01(cfg.MinAcceptScore)
			: (cfg.EnableAutoThreshold ? item.templData.autoMinFused : 0.50);

		item.strongBase =
			cfg.EnableAutoThreshold ? item.templData.autoStrongBaseScore : 0.88;
		item.softEdgeIou =
			cfg.EnableAutoThreshold ? item.templData.autoSoftEdgeIou : 0.06;
		item.softEdgeRatio =
			cfg.EnableAutoThreshold ? item.templData.autoSoftEdgeRatio : 0.50;
		item.hardEdgeIou =
			cfg.EnableAutoThreshold ? item.templData.autoHardEdgeIou : 0.10;
		item.hardEdgeRatio =
			cfg.EnableAutoThreshold ? item.templData.autoHardEdgeRatio : 0.65;

		item.wBase =
			cfg.EnableAutoThreshold ? item.templData.autoWBase : 0.72;
		item.wRaw =
			cfg.EnableAutoThreshold ? item.templData.autoWRaw : 0.10;
		item.wGrad =
			cfg.EnableAutoThreshold ? item.templData.autoWGrad : 0.04;
		item.wEdgeIou =
			cfg.EnableAutoThreshold ? item.templData.autoWEdgeIou : 0.10;
		item.wEdgeRatio =
			cfg.EnableAutoThreshold ? item.templData.autoWEdgeRatio : 0.04;

		img->stableScaleBank.push_back(std::move(item));

		const double d = std::abs(scale - 1.0);
		if (d < bestBaseDist)
		{
			bestBaseDist = d;
			bestBaseIdx = (int)img->stableScaleBank.size() - 1;
		}
	}

	if (img->stableScaleBank.empty())
		return;

	if (bestBaseIdx < 0) bestBaseIdx = 0;

	img->m_TemplData = img->stableScaleBank[(size_t)bestBaseIdx].templData;
	img->m_TemplData.bIsPatternLearned = true;
	img->matSample = originalGray.clone();
}

void Pattern2::FreeBuffer(System::IntPtr p)
{
	if (p != System::IntPtr::Zero)
		System::Runtime::InteropServices::Marshal::FreeHGlobal(p);
}

List<Rotaterectangle>^ Pattern2::Match(
	bool   m_bStopLayer1,
	double m_dToleranceAngle,   // NEW: nếu > 0 dùng làm bước quét góc (độ)
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
	//  Kết quả .NET 
	auto results = gcnew List<Rotaterectangle>(Math::Max(m_iMaxPos, 0));

	//  Kiểm tra đầu vào nhanh 
	if (img == nullptr) return results;
	if (img->matSample.empty()) return results;
	if (img->matRaw.empty())    return results;
	if (!img->m_TemplData.bIsPatternLearned) return results;

	//  Kẹp tham số nguy hiểm 
	if (m_iMaxPos <= 0) m_iMaxPos = 1;
	if (m_iMaxPos > 256) m_iMaxPos = 256;
	const int    MAX_NEXT_MAX = 128;
	const int    MAX_ANGLES = 361;
	const double MAX_PIXELS = 80e6;
	const double epsTiny = 1e-6;
	const double MIN_ANGLE_STEP = 0.05; // độ; tránh bước quá nhỏ

	//  Chuẩn hoá nguồn về GRAY 8U 
	cv::Mat srcForPyr;
	{
		cv::Mat src = img->matRaw;

		if (src.channels() == 3) {
			cv::cvtColor(src, srcForPyr, cv::COLOR_BGR2GRAY);
		}
		else if (src.channels() == 1) {
			srcForPyr = src;
		}
		else {
			cv::Mat tmp; cv::convertScaleAbs(src, tmp);
			if (tmp.channels() == 3) cv::cvtColor(tmp, srcForPyr, cv::COLOR_BGR2GRAY);
			else if (tmp.channels() == 1) srcForPyr = tmp;
			else cv::cvtColor(tmp, srcForPyr, cv::COLOR_BGR2GRAY);
		}
		if (srcForPyr.depth() != CV_8U) cv::convertScaleAbs(srcForPyr, srcForPyr);
		if (m_ckBitwiseNot) cv::bitwise_not(srcForPyr, srcForPyr);
	}

	//  Xác định top layer với “ngân sách” 
	int iTopLayer = img->GetTopLayer(&img->matSample, (int)std::sqrt((double)img->m_iMinReduceArea));
	if (iTopLayer < 0) iTopLayer = 0;
	{
		// xấp xỉ 1 + 1/4 + 1/16 + 1/64
		double est = (double)srcForPyr.total() * (1.0 + 0.25 + 0.0625 + 0.015625);
		while (iTopLayer > 0 && est > MAX_PIXELS) {
			--iTopLayer;
			est = (double)srcForPyr.total() * (1.0 + 0.25 + 0.0625 + 0.015625);
		}
	}

	//  Pyramid nguồn 
	std::vector<cv::Mat> vecMatSrcPyr;
	vecMatSrcPyr.reserve((size_t)iTopLayer + 1);
	cv::buildPyramid(srcForPyr, vecMatSrcPyr, iTopLayer);

	s_TemplData* pTemplData = &img->m_TemplData;
	if ((int)pTemplData->vecPyramid.size() <= iTopLayer) return results;

	//  Góc quét ở top layer 
	double dAngleStepAuto = std::atan(2.0 / std::max(pTemplData->vecPyramid[(size_t)iTopLayer].cols,
		pTemplData->vecPyramid[(size_t)iTopLayer].rows)) * R2D;
	if (dAngleStepAuto < epsTiny) dAngleStepAuto = 0.5;

	const double userStep = std::fabs(m_dToleranceAngle);
	const bool   useFixedStep = (userStep > epsTiny);
	double dAngleStep = useFixedStep ? std::max(userStep, MIN_ANGLE_STEP) : dAngleStepAuto;

	//  Dải góc ở top layer 
	std::vector<double> vecAngles;
	vecAngles.reserve(64);
	{
		// Dải [m_dTolerance1, m_dTolerance2]
		if (m_dTolerance2 < m_dTolerance1) std::swap(m_dTolerance1, m_dTolerance2);
		for (double a = m_dTolerance1; a <= m_dTolerance2 + 0.5 * dAngleStep; a += dAngleStep)
			vecAngles.push_back(a);
		if (vecAngles.empty()) vecAngles.push_back(0.0);

		// Kẹp số góc
		if ((int)vecAngles.size() > MAX_ANGLES) {
			std::vector<double> trimmed; trimmed.reserve(MAX_ANGLES);
			double stepA = (vecAngles.back() - vecAngles.front()) / (MAX_ANGLES - 1);
			for (int i = 0; i < MAX_ANGLES; ++i)
				trimmed.push_back(vecAngles.front() + i * stepA);
			vecAngles.swap(trimmed);
		}
	}

	//  Ngưỡng theo tầng 
	std::vector<double> vecLayerScore((size_t)iTopLayer + 1, m_dScore);
	for (int l = 1; l <= iTopLayer; ++l)
		vecLayerScore[(size_t)l] = vecLayerScore[(size_t)l - 1] * 0.90;

	const int iTopSrcW = vecMatSrcPyr[(size_t)iTopLayer].cols;
	const int iTopSrcH = vecMatSrcPyr[(size_t)iTopLayer].rows;
	cv::Point2f ptCenter((iTopSrcW - 1) * 0.5f, (iTopSrcH - 1) * 0.5f);

	cv::Size sizePatTop = pTemplData->vecPyramid[(size_t)iTopLayer].size();
	const bool bCalMaxByBlock =
		(vecMatSrcPyr[(size_t)iTopLayer].size().area() / std::max(1, sizePatTop.area()) > 500) &&
		(m_iMaxPos > 10);

	//  Tìm cực đại sơ bộ ở top layer cho mỗi góc 
	std::vector<s_MatchParameter> vecMatchParameter;
	vecMatchParameter.reserve(vecAngles.size() * (size_t)std::min(m_iMaxPos + MATCH_CANDIDATE_NUM, 64));

	for (size_t ai = 0; ai < vecAngles.size(); ++ai)
	{
		try {
			const double angDeg = vecAngles[ai];
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
			cv::warpAffine(vecMatSrcPyr[(size_t)iTopLayer], matRotatedSrc, matR, sizeBest,
				cv::INTER_LINEAR, cv::BORDER_CONSTANT, cv::Scalar(pTemplData->iBorderColor));

			cv::Mat matResult;
			img->MatchTemplate(matRotatedSrc, pTemplData, matResult, iTopLayer, false, m_ckSIMD);
			if (matResult.type() != CV_32F) matResult.convertTo(matResult, CV_32F);

			const double layerThresh = vecLayerScore[(size_t)iTopLayer];
			const int want = std::min(m_iMaxPos + MATCH_CANDIDATE_NUM - 1, MAX_NEXT_MAX);

			if (bCalMaxByBlock) {
				s_BlockMax blockMax(matResult, pTemplData->vecPyramid[(size_t)iTopLayer].size());

				double dMaxVal = -1.0;
				cv::Point ptMaxLoc;
				blockMax.GetMaxValueLoc(dMaxVal, ptMaxLoc);

				if (dMaxVal >= layerThresh) {
					vecMatchParameter.emplace_back(cv::Point2f(ptMaxLoc.x - fTx, ptMaxLoc.y - fTy), dMaxVal, angDeg);

					for (int k = 0; k < want; ++k) {
						double valueF = -1.f;
						const double overlapF = (float)m_dMaxOverlap;

						ptMaxLoc = img->GetNextMaxLoc(matResult, ptMaxLoc,
							cv::Size(sizePatTop.width, sizePatTop.height),
							valueF, overlapF, blockMax);

						if ((double)valueF < layerThresh) break;
						vecMatchParameter.emplace_back(
							cv::Point2f(ptMaxLoc.x - fTx, ptMaxLoc.y - fTy),
							(double)valueF, angDeg);
					}
				}
			}
			else {
				double dMaxVal = -1.0;
				cv::Point ptMaxLoc;
				cv::minMaxLoc(matResult, nullptr, &dMaxVal, nullptr, &ptMaxLoc);

				if (dMaxVal >= layerThresh) {
					vecMatchParameter.emplace_back(cv::Point2f(ptMaxLoc.x - fTx, ptMaxLoc.y - fTy), dMaxVal, angDeg);

					for (int k = 0; k < want; ++k) {
						double valueF = -1.f;
						const double overlapF = (float)m_dMaxOverlap;

						ptMaxLoc = img->GetNextMaxLoc(matResult, ptMaxLoc,
							cv::Size(sizePatTop.width, sizePatTop.height),
							valueF, overlapF);

						if ((double)valueF < layerThresh) break;
						vecMatchParameter.emplace_back(
							cv::Point2f(ptMaxLoc.x - fTx, ptMaxLoc.y - fTy),
							(double)valueF, angDeg);
					}
				}
			}
		}
		catch (const std::exception&) { /* có thể log nếu cần */ }
	}

	std::sort(vecMatchParameter.begin(), vecMatchParameter.end(), compareScoreBig2Small);
	if (vecMatchParameter.empty()) return results;

	//  Refine xuống các layer dưới 
	const int iDstWTop = pTemplData->vecPyramid[(size_t)iTopLayer].cols;
	const int iDstHTop = pTemplData->vecPyramid[(size_t)iTopLayer].rows;

	const bool bSubPixelEstimation = m_bSubPixel;
	const int  iStopLayer = m_bStopLayer1 ? 1 : 0;  // 1 = chỉ coarse

	std::vector<s_MatchParameter> vecAllResult;
	vecAllResult.reserve(vecMatchParameter.size());

	for (size_t mi = 0; mi < vecMatchParameter.size(); ++mi)
	{
		double dRAngle = -vecMatchParameter[mi].dMatchAngle * D2R;
		cv::Point2f ptLT = img->ptRotatePt2f(vecMatchParameter[mi].pt, ptCenter, dRAngle);

		// bước góc local ở top (dùng fixed nếu có)
		double localAngleStepAuto = std::atan(2.0 / std::max(iDstWTop, iDstHTop)) * R2D;
		if (localAngleStepAuto < epsTiny) localAngleStepAuto = 0.5;
		double localAngleStep = useFixedStep ? std::max(userStep, MIN_ANGLE_STEP) : localAngleStepAuto;

		vecMatchParameter[mi].dAngleStart = vecMatchParameter[mi].dMatchAngle - localAngleStep;
		vecMatchParameter[mi].dAngleEnd = vecMatchParameter[mi].dMatchAngle + localAngleStep;

		if (iTopLayer <= iStopLayer) {
			vecMatchParameter[mi].pt = cv::Point2d(ptLT * ((iTopLayer == 0) ? 1 : 2));
			vecAllResult.push_back(vecMatchParameter[mi]);
		}
		else {
			for (int iLayer = iTopLayer - 1; iLayer >= iStopLayer; --iLayer)
			{
				// danh sách góc quanh góc tốt nhất hiện tại
				double localAngleStepAuto2 = std::atan(2.0 / std::max(pTemplData->vecPyramid[(size_t)iLayer].cols,
					pTemplData->vecPyramid[(size_t)iLayer].rows)) * R2D;
				if (localAngleStepAuto2 < epsTiny) localAngleStepAuto2 = 0.5;
				double localAngleStep = useFixedStep ? std::max(userStep, MIN_ANGLE_STEP) : localAngleStepAuto2;

				const double dMatchedAngle = vecMatchParameter[mi].dMatchAngle;
				double localAngles[3] = {
					dMatchedAngle - localAngleStep,
					dMatchedAngle,
					dMatchedAngle + localAngleStep
				};

				cv::Point2f ptSrcCenter((vecMatSrcPyr[(size_t)iLayer].cols - 1) * 0.5f,
					(vecMatSrcPyr[(size_t)iLayer].rows - 1) * 0.5f);

				s_MatchParameter vecNew[3];

				int    bestIdx = 0;
				double bestVal = -1.0;

				for (int j = 0; j < 3; ++j)
				{
					cv::Mat matRotatedSrc, matResult;
					img->GetRotatedROI(vecMatSrcPyr[(size_t)iLayer],
						pTemplData->vecPyramid[(size_t)iLayer].size(),
						ptLT * 2, localAngles[j], matRotatedSrc);

					img->MatchTemplate(matRotatedSrc, pTemplData, matResult, iLayer, true, m_ckSIMD);
					if (matResult.type() != CV_32F) matResult.convertTo(matResult, CV_32F);

					double dMaxValue = -1.0;
					cv::Point ptMaxLoc;
					cv::minMaxLoc(matResult, nullptr, &dMaxValue, nullptr, &ptMaxLoc);

					vecNew[j] = s_MatchParameter(ptMaxLoc, dMaxValue, localAngles[j]);

					if (dMaxValue > bestVal) { bestVal = dMaxValue; bestIdx = j; }

					// biên
					if (ptMaxLoc.x == 0 || ptMaxLoc.y == 0 ||
						ptMaxLoc.x == matResult.cols - 1 || ptMaxLoc.y == matResult.rows - 1)
						vecNew[j].bPosOnBorder = true;

					if (!vecNew[j].bPosOnBorder) {
						for (int yy = -1; yy <= 1; ++yy)
							for (int xx = -1; xx <= 1; ++xx)
								vecNew[j].vecResult[xx + 1][yy + 1] =
								matResult.at<float>(ptMaxLoc + cv::Point(xx, yy));
					}
				}

				if (vecNew[(size_t)bestIdx].dMatchScore < vecLayerScore[(size_t)iLayer])
					break;

				if (bSubPixelEstimation && iLayer == 0 && !vecNew[(size_t)bestIdx].bPosOnBorder &&
					bestIdx != 0 && bestIdx != 2)
				{
					double dNewX = 0, dNewY = 0, dNewAngle = 0;
					std::vector<s_MatchParameter> tmpV(3);
					tmpV[0] = vecNew[0]; tmpV[1] = vecNew[1]; tmpV[2] = vecNew[2];
					img->SubPixEsimation(&tmpV, &dNewX, &dNewY, &dNewAngle, localAngleStep, bestIdx);
					vecNew[(size_t)bestIdx].pt = cv::Point2d(dNewX, dNewY);
					vecNew[(size_t)bestIdx].dMatchAngle = dNewAngle;
				}

				const double dNewMatchAngle = vecNew[(size_t)bestIdx].dMatchAngle;

				// chuyển toạ độ về hệ của ROI quay
				cv::Point2f ptPaddingLT =
					img->ptRotatePt2f(ptLT * 2, ptSrcCenter, (float)(dNewMatchAngle * D2R)) -
					cv::Point2f(3.f, 3.f);
				cv::Point2f pt((float)(vecNew[(size_t)bestIdx].pt.x + ptPaddingLT.x),
					(float)(vecNew[(size_t)bestIdx].pt.y + ptPaddingLT.y));
				// quay ngược về hệ gốc của layer
				pt = img->ptRotatePt2f(pt, ptSrcCenter, (float)(-dNewMatchAngle * D2R));

				if (iLayer == iStopLayer) {
					vecNew[(size_t)bestIdx].pt = pt * (iStopLayer == 0 ? 1.f : 2.f);
					vecAllResult.push_back(vecNew[(size_t)bestIdx]);
				}
				else {
					// cập nhật cho vòng refine tiếp
					vecMatchParameter[mi].dMatchAngle = dNewMatchAngle;
					vecMatchParameter[mi].dAngleStart = dNewMatchAngle - localAngleStep * 0.5;
					vecMatchParameter[mi].dAngleEnd = dNewMatchAngle + localAngleStep * 0.5;
					ptLT = pt;
				}
			}
		}
	}

	// Lọc theo score thô
	img->FilterWithScore(&vecAllResult, m_dScore);
	if (vecAllResult.empty()) return results;

	// Lọc chồng lắp bằng rotated rect (KHÔNG khai báo lại iStopLayer)
	const int iDstW = pTemplData->vecPyramid[(size_t)iStopLayer].cols * (iStopLayer == 0 ? 1 : 2);
	const int iDstH = pTemplData->vecPyramid[(size_t)iStopLayer].rows * (iStopLayer == 0 ? 1 : 2);
	try
	{
		for (size_t i = 0; i < vecAllResult.size(); ++i)
		{
			// 1) Guard cơ bản
			if (iDstW <= 0 || iDstH <= 0) continue;
			const double angDeg = vecAllResult[i].dMatchAngle;
			if (!std::isfinite(angDeg)) continue;

			// 2) Tính vector cạnh bằng double tránh tràn/NaN sớm
			const double angRad = -angDeg * D2R;
			const double c = std::cos(angRad);
			const double s = std::sin(angRad);

			const cv::Point2f ptLT = vecAllResult[i].pt;

			// vector theo chiều rộng & chiều cao
			const cv::Point2f vW((float)(iDstW * c), (float)(-iDstW * s));
			const cv::Point2f vH((float)(iDstH * s), (float)(iDstH * c));

			// 3) Ba đỉnh liên tiếp
			const cv::Point2f ptRT(ptLT.x + vW.x, ptLT.y + vW.y);
			const cv::Point2f ptRB(ptRT.x + vH.x, ptRT.y + vH.y);

			// 4b) Cách 2 (khuyên dùng): ctor (center, size, angle) an toàn hơn
			const cv::Point2f center(
				ptLT.x + 0.5f * (vW.x + vH.x),
				ptLT.y + 0.5f * (vW.y + vH.y)
			);
			vecAllResult[i].rectR = cv::RotatedRect(
				center,
				cv::Size2f((float)iDstW, (float)iDstH),
				(float)angDeg  // góc theo định nghĩa OpenCV (độ)
			);
		}

	}
	catch (const cv::Exception& e) {
		throw gcnew System::InvalidOperationException(gcnew System::String(e.what()));
	}
	catch (const std::exception& e) {
		throw gcnew System::InvalidOperationException(gcnew System::String(e.what()));
	}

	img->FilterWithRotatedRect(&vecAllResult, CV_TM_CCOEFF_NORMED, m_dMaxOverlap);
	std::sort(vecAllResult.begin(), vecAllResult.end(), compareScoreBig2Small);
	if (vecAllResult.empty()) return results;

	const int iW = pTemplData->vecPyramid[0].cols;
	const int iH = pTemplData->vecPyramid[0].rows;

	// Xuất kết quả tối đa m_iMaxPos
	const size_t takeN = std::min((size_t)m_iMaxPos, vecAllResult.size());
	for (size_t i = 0; i < takeN; ++i)
	{
		const double ang = vecAllResult[i].dMatchAngle;
		const cv::Point2f c = vecAllResult[i].rectR.center;

		Rotaterectangle rr;
		rr.Cx = c.x; rr.Cy = c.y;
		rr.AngleDeg = ang;      // giữ sign như bản gốc
		rr.Width = (double)iW;
		rr.Height = (double)iH;
		rr.Score = vecAllResult[i].dMatchScore * 100.0;
		results->Add(rr);
	}

	return results;
}



List<Rotaterectangle>^ Pattern2::MatchStable(Pattern2StableConfig cfg)
{
	auto results = gcnew List<Rotaterectangle>();

	if (img == nullptr) return results;
	if (img->matRaw.empty()) return results;
	if (img->matSample.empty()) return results;
	if (!img->m_TemplData.bIsPatternLearned) return results;
	if (img->stableScaleBank.empty()) return results;

	const int maxPos = std::max(1, std::min(cfg.MaxPos, 256));
	const double maxOverlap = std::max(0.0, std::min(cfg.MaxOverlap, 0.90));
	const double relaxedRawScore = Clamp01(cfg.RelaxedRawScore);

	const cv::Mat originalSample = img->matSample.clone();
	const s_TemplData originalTemplData = img->m_TemplData;

	cv::Mat srcGray = ToGray8U(img->matRaw);
	if (cfg.BitwiseNot) cv::bitwise_not(srcGray, srcGray);

	StringBuilder^ sb = nullptr;
	if (cfg.DebugLog)
	{
		sb = gcnew StringBuilder();
		sb->AppendLine("==== Pattern2::MatchStable ====");
		sb->AppendLine(DateTime::Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
		sb->AppendLine(System::String::Format(
			CultureInfo::InvariantCulture,
			"range=[{0:F2},{1:F2}], step={2:F3}, minAccept={3:F3}, maxPos={4}, maxOverlap={5:F3}, validator={6}, keep={7}, nms={8}, auto={9}, learnedScales={10}",
			cfg.AngleStartDeg, cfg.AngleEndDeg, cfg.AngleStepDeg,
			cfg.MinAcceptScore, maxPos, maxOverlap,
			cfg.EnableValidator ? 1 : 0,
			cfg.EnableKeepFilter ? 1 : 0,
			cfg.EnableNms ? 1 : 0,
			cfg.EnableAutoThreshold ? 1 : 0,
			(int)img->stableScaleBank.size()));
	}

	std::vector<s_MatchParameter> acceptedAll;
	acceptedAll.reserve((size_t)maxPos * std::max<size_t>(1, img->stableScaleBank.size()) * 4);

	for (const auto& item : img->stableScaleBank)
	{
		const double scale = item.scale;
		const cv::Mat& tplGray = item.tplGray;
		const cv::Mat& tplNorm = item.tplNorm;
		const cv::Mat& tplGrad = item.tplGrad;
		const cv::Mat& tplEdge = item.tplEdge;
		const int tplEdgeCount = std::max(1, item.tplEdgeCount);

		const double thresholdFinal = item.thresholdFinal;
		const double strongBase = item.strongBase;
		const double softEdgeIou = item.softEdgeIou;
		const double softEdgeRatio = item.softEdgeRatio;
		const double hardEdgeIou = item.hardEdgeIou;
		const double hardEdgeRatio = item.hardEdgeRatio;
		const double wBase = item.wBase;
		const double wRaw = item.wRaw;
		const double wGrad = item.wGrad;
		const double wEdgeIou = item.wEdgeIou;
		const double wEdgeRatio = item.wEdgeRatio;

		img->matSample = tplGray;
		img->m_TemplData = item.templData;

		if (sb != nullptr)
		{
			sb->AppendLine(System::String::Format(
				CultureInfo::InvariantCulture,
				"-- scale={0:F3}, tpl={1}x{2}, edgeCount={3}, edgeDensity={4:F5}, entropy={5:F3}, entropyNorm={6:F3}",
				scale, tplGray.cols, tplGray.rows,
				item.templData.modelEdgeCount,
				item.templData.modelEdgeDensity,
				item.templData.modelEntropy,
				item.templData.modelEntropyNorm));
			sb->AppendLine(System::String::Format(
				CultureInfo::InvariantCulture,
				"auto strongBase={0:F3}, softEdgeIou={1:F3}, softEdgeRatio={2:F3}, hardEdgeIou={3:F3}, hardEdgeRatio={4:F3}, finalThreshold={5:F3}",
				strongBase, softEdgeIou, softEdgeRatio, hardEdgeIou, hardEdgeRatio, thresholdFinal));
			sb->AppendLine(System::String::Format(
				CultureInfo::InvariantCulture,
				"weights base={0:F3}, raw={1:F3}, grad={2:F3}, edgeIou={3:F3}, edgeRatio={4:F3}",
				wBase, wRaw, wGrad, wEdgeIou, wEdgeRatio));
		}

		auto coarse = Match(
			false,
			cfg.AngleStepDeg,
			cfg.AngleStartDeg,
			cfg.AngleEndDeg,
			relaxedRawScore,
			false,
			cfg.BitwiseNot,
			cfg.SubPixel,
			std::max(32, maxPos * 8),
			std::max(0.01, std::min(maxOverlap, 0.60)),
			false,
			0);

		if (coarse == nullptr || coarse->Count == 0)
		{
			if (sb != nullptr) sb->AppendLine("coarseCount=0");
			continue;
		}

		if (sb != nullptr)
			sb->AppendLine(System::String::Format(CultureInfo::InvariantCulture, "coarseCount={0}", coarse->Count));

		for each(Rotaterectangle rr in coarse)
		{
			const int w = std::max(1, (int)std::round(rr.Width));
			const int h = std::max(1, (int)std::round(rr.Height));
			if (w <= 4 || h <= 4) continue;

			const double angDeg = rr.AngleDeg;
			const double angRad = angDeg * D2R;

			const cv::Point2f vW(
				(float)(w * std::cos(angRad)),
				(float)(-w * std::sin(angRad)));
			const cv::Point2f vH(
				(float)(h * std::sin(angRad)),
				(float)(h * std::cos(angRad)));

			const cv::Point2f center((float)rr.Cx, (float)rr.Cy);
			const cv::Point2f ptLT(
				center.x - 0.5f * (vW.x + vH.x),
				center.y - 0.5f * (vW.y + vH.y));

			const double coarseScore = Clamp01(rr.Score / 100.0);

			double rawNcc = 0.0;
			double gradNcc = 0.0;
			double edgeIou = 0.0;
			double edgeRatio = 0.0;
			double finalScore = coarseScore;
			bool passScore = finalScore >= thresholdFinal;
			bool keepStrong = false;
			bool keepNormal = false;
			bool keepRescue = false;
			System::String^ reason = "score_only";

			if (cfg.EnableValidator)
			{
				cv::Mat roiPad;
				img->GetRotatedROI(srcGray, cv::Size(w, h), ptLT, angDeg, roiPad);

				if (roiPad.empty() || roiPad.cols < w + 6 || roiPad.rows < h + 6)
				{
					if (sb != nullptr)
					{
						sb->AppendLine(System::String::Format(
							CultureInfo::InvariantCulture,
							"cx={0:F2}, cy={1:F2}, ang={2:F2}, coarse={3:F3}, final=0.000, keep=0, reason=roi_invalid",
							rr.Cx, rr.Cy, rr.AngleDeg, coarseScore));
					}
					continue;
				}

				cv::Mat roiGray = roiPad(cv::Rect(3, 3, w, h)).clone();
				if (roiGray.size() != tplGray.size())
					cv::resize(roiGray, roiGray, tplGray.size(), 0, 0, cv::INTER_LINEAR);

				cv::Mat roiNorm = NormalizeForCompare(roiGray);
				cv::Mat roiGrad = GradientMag8(roiNorm);
				cv::Mat roiEdge = EdgeMaskFromGrad(roiGrad);

				rawNcc = ScoreSameSizeNCC(tplNorm, roiNorm);
				gradNcc = ScoreSameSizeNCC(tplGrad, roiGrad);
				edgeIou = EdgeIoU(tplEdge, roiEdge);
				edgeRatio = RatioSimilarity(tplEdgeCount, cv::countNonZero(roiEdge));

				const double rawPos = std::max(0.0, rawNcc);
				const double gradPos = std::max(0.0, gradNcc);

				finalScore = Clamp01(
					wBase * coarseScore +
					wRaw * rawPos +
					wGrad * gradPos +
					wEdgeIou * edgeIou +
					wEdgeRatio * edgeRatio);

				passScore = finalScore >= thresholdFinal;

				const bool passSoftEdge =
					(edgeIou >= softEdgeIou) ||
					(edgeRatio >= softEdgeRatio);

				const bool passHardEdge =
					(edgeIou >= hardEdgeIou) ||
					(edgeRatio >= hardEdgeRatio);

				keepStrong =
					passScore &&
					(coarseScore >= strongBase) &&
					passSoftEdge;

				keepNormal =
					passScore &&
					passHardEdge;

				keepRescue =
					passScore &&
					(coarseScore >= std::max(0.45, strongBase - 0.25)) &&
					(finalScore >= thresholdFinal + 0.05) &&
					((edgeIou >= hardEdgeIou * 0.70) ||
						(edgeRatio >= hardEdgeRatio * 0.55));

				if (cfg.EnableKeepFilter)
				{
					if (keepStrong) reason = "keepStrong";
					else if (keepNormal) reason = "keepNormal";
					else if (keepRescue) reason = "keepRescue";
					else if (!passScore) reason = "below_threshold";
					else reason = "shape_gate";
				}
				else
				{
					reason = passScore ? "passScore_only" : "below_threshold";
				}
			}
			else
			{
				finalScore = coarseScore;
				passScore = finalScore >= thresholdFinal;
				reason = passScore ? "coarse_only" : "below_threshold";
			}

			const bool keep = cfg.EnableKeepFilter
				? (cfg.EnableValidator ? (keepStrong || keepNormal || keepRescue) : passScore)
				: passScore;

			if (sb != nullptr)
			{
				sb->AppendLine(System::String::Format(
					CultureInfo::InvariantCulture,
					"cx={0:F2}, cy={1:F2}, ang={2:F2}, scale={3:F3}, coarse={4:F3}, raw={5:F3}, grad={6:F3}, edgeIoU={7:F3}, edgeRatio={8:F3}, final={9:F3}, keepStrong={10}, keepNormal={11}, keepRescue={12}, keep={13}, reason={14}",
					rr.Cx, rr.Cy, rr.AngleDeg, scale,
					coarseScore, rawNcc, gradNcc, edgeIou, edgeRatio, finalScore,
					keepStrong ? 1 : 0,
					keepNormal ? 1 : 0,
					keepRescue ? 1 : 0,
					keep ? 1 : 0,
					reason));
			}

			if (!keep) continue;

			s_MatchParameter mp;
			mp.pt = ptLT;
			mp.bDelete = false;
			mp.dMatchAngle = angDeg;
			mp.dCoarseScore = coarseScore;
			mp.dMatchScore = finalScore;
			mp.rectR = cv::RotatedRect(
				center,
				cv::Size2f((float)tplGray.cols, (float)tplGray.rows),
				(float)angDeg);

			acceptedAll.push_back(mp);
		}
	}

	img->matSample = originalSample;
	img->m_TemplData = originalTemplData;

	if (cfg.EnableNms)
		img->FilterWithRotatedRect(&acceptedAll, CV_TM_CCOEFF_NORMED, maxOverlap);

	std::sort(acceptedAll.begin(), acceptedAll.end(), compareScoreBig2Small);

	if (sb != nullptr)
		sb->AppendLine(System::String::Format(CultureInfo::InvariantCulture, "acceptedAfterNms={0}", (int)acceptedAll.size()));

	const int takeN = std::min((int)acceptedAll.size(), maxPos);
	for (int i = 0; i < takeN; ++i)
	{
		Rotaterectangle rr;
		rr.Cx = acceptedAll[i].rectR.center.x;
		rr.Cy = acceptedAll[i].rectR.center.y;
		rr.AngleDeg = acceptedAll[i].dMatchAngle;
		rr.Width = acceptedAll[i].rectR.size.width;
		rr.Height = acceptedAll[i].rectR.size.height;
		rr.Score = acceptedAll[i].dMatchScore * 100.0;
		results->Add(rr);
	}

	if (sb != nullptr && !System::String::IsNullOrWhiteSpace(cfg.DebugLogPath))
	{
		try
		{
			File::AppendAllText(cfg.DebugLogPath, sb->ToString());
		}
		catch (...) {}
	}

	return results;
}


//List<Rotaterectangle>^ Pattern2::Match(
//    bool   m_bStopLayer1,
//    double m_dToleranceAngle,
//    double m_dTolerance1,
//    double m_dTolerance2,
//    double m_dScore,
//    bool   m_ckSIMD,
//    bool   m_ckBitwiseNot,
//    bool   m_bSubPixel,
//    int    m_iMaxPos,
//    double m_dMaxOverlap,
//    bool   useMultiThread,
//    int    numThreads
//)
//{
//    auto results = gcnew List<Rotaterectangle>();
// 
//
//		System:: String^ listMatch = "";
//
//		bool m_bToleranceRange = true;
//
//		double m_dTolerance3 = 0;
//		double m_dTolerance4 = 0;
//	
//
//	
//		
//		/*if (matDraw.type() != CV_8UC3)
//		{
//			cvtColor(matDraw, matDraw, COLOR_GRAY2BGR);
//		}
//		*/
//		if (img->matSample.empty())
//			return results;
//		if (img->matRaw.empty())
//			return results;
//	
//		if (!img->m_TemplData.bIsPatternLearned)
//			return results;
//		//UpdateData (1);
//		double d1 = clock();
//		//決定金字塔層數 總共為1 + iLayer層
//		int iTopLayer = img->GetTopLayer(&img->matSample, (int)sqrt((double)img->m_iMinReduceArea));
//		//建立金字塔
//		vector<Mat> vecMatSrcPyr;
//		if (m_ckBitwiseNot)//m_ckBitwiseNot
//		{
//			Mat matNewSrc = 255 - img->matRaw;
//			buildPyramid(matNewSrc, vecMatSrcPyr, iTopLayer);
//
//		}
//		else
//			buildPyramid(img->matRaw, vecMatSrcPyr, iTopLayer);
//
//		s_TemplData* pTemplData = &img->m_TemplData;
//		double dAngleStep = atan(2.0 / max(pTemplData->vecPyramid[iTopLayer].cols, pTemplData->vecPyramid[iTopLayer].rows)) * R2D;
//
//		vector<double> vecAngles;
//
//		if (m_bToleranceRange)
//		{
//
//			for (double dAngle = m_dTolerance1; dAngle < m_dTolerance2 + dAngleStep; dAngle += dAngleStep)
//				vecAngles.push_back(dAngle);
//			/*for (double dAngle = m_dTolerance3; dAngle < m_dTolerance4 + dAngleStep; dAngle += dAngleStep)
//				vecAngles.push_back(dAngle);*/
//		}
//		else
//		{
//			if (m_dToleranceAngle < VISION_TOLERANCE)
//				vecAngles.push_back(0.0);
//			else
//			{
//				for (double dAngle = 0; dAngle < m_dToleranceAngle + dAngleStep; dAngle += dAngleStep)
//					vecAngles.push_back(dAngle);
//				for (double dAngle = -dAngleStep; dAngle > -m_dToleranceAngle - dAngleStep; dAngle -= dAngleStep)
//					vecAngles.push_back(dAngle);
//			}
//		}
//
//		int iTopSrcW = vecMatSrcPyr[iTopLayer].cols, iTopSrcH = vecMatSrcPyr[iTopLayer].rows;
//		Point2f ptCenter((iTopSrcW - 1) / 2.0f, (iTopSrcH - 1) / 2.0f);
//
//		int iSize = (int)vecAngles.size();
//		//vector<s_MatchParameter> vecMatchParameter (iSize * (m_iMaxPos + MATCH_CANDIDATE_NUM));
//		vector<s_MatchParameter> vecMatchParameter;
//		//Caculate lowest score at every layer
//		vector<double> vecLayerScore(iTopLayer + 1, m_dScore);
//		for (int iLayer = 1; iLayer <= iTopLayer; iLayer++)
//			vecLayerScore[iLayer] = vecLayerScore[iLayer - 1] * 0.9;
//
//		cv::Size sizePat = pTemplData->vecPyramid[iTopLayer].size();
//		bool bCalMaxByBlock = (vecMatSrcPyr[iTopLayer].size().area() / sizePat.area() > 500) && m_iMaxPos > 10;
//		for (int i = 0; i < iSize; i++)
//		{
//			try
//			{
//				Mat matRotatedSrc, matR = getRotationMatrix2D(ptCenter, vecAngles[i], 1);
//				Mat matResult;
//				cv::Point ptMaxLoc;
//				double dValue, dMaxVal;
//				double dRotate = clock();
//				cv::Size sizeBest = img-> GetBestRotationSize(vecMatSrcPyr[iTopLayer].size(), pTemplData->vecPyramid[iTopLayer].size(), vecAngles[i]);
//
//				float fTranslationX = (sizeBest.width - 1) / 2.0f - ptCenter.x;
//				float fTranslationY = (sizeBest.height - 1) / 2.0f - ptCenter.y;
//				matR.at<double>(0, 2) += fTranslationX;
//				matR.at<double>(1, 2) += fTranslationY;
//				warpAffine(vecMatSrcPyr[iTopLayer], matRotatedSrc, matR, sizeBest, INTER_LINEAR, BORDER_CONSTANT, Scalar(pTemplData->iBorderColor));
//				//imshow("r" + to_string(iSize), matRotatedSrc);
//				img->MatchTemplate(matRotatedSrc, pTemplData, matResult, iTopLayer, false, m_ckSIMD);
//				//	imshow("H" + to_string(iSize), matResult);
//				if (bCalMaxByBlock)
//				{
//					s_BlockMax blockMax(matResult, pTemplData->vecPyramid[iTopLayer].size());
//					blockMax.GetMaxValueLoc(dMaxVal, ptMaxLoc);
//					if (dMaxVal < vecLayerScore[iTopLayer])
//						continue;
//					vecMatchParameter.push_back(s_MatchParameter(Point2f(ptMaxLoc.x - fTranslationX, ptMaxLoc.y - fTranslationY), dMaxVal, vecAngles[i]));
//					for (int j = 0; j < m_iMaxPos + MATCH_CANDIDATE_NUM - 1; j++)
//					{
//						ptMaxLoc = img->GetNextMaxLoc(matResult, ptMaxLoc, pTemplData->vecPyramid[iTopLayer].size(), dValue, m_dMaxOverlap, blockMax);
//						if (dValue < vecLayerScore[iTopLayer])
//							break;
//						vecMatchParameter.push_back(s_MatchParameter(Point2f(ptMaxLoc.x - fTranslationX, ptMaxLoc.y - fTranslationY), dValue, vecAngles[i]));
//					}
//				}
//				else
//				{
//
//					minMaxLoc(matResult, 0, &dMaxVal, 0, &ptMaxLoc);
//					if (dMaxVal < vecLayerScore[iTopLayer])
//						continue;
//					vecMatchParameter.push_back(s_MatchParameter(Point2f(ptMaxLoc.x - fTranslationX, ptMaxLoc.y - fTranslationY), dMaxVal, vecAngles[i]));
//					for (int j = 0; j < m_iMaxPos + MATCH_CANDIDATE_NUM - 1; j++)
//					{
//						ptMaxLoc = img->GetNextMaxLoc(matResult, ptMaxLoc, pTemplData->vecPyramid[iTopLayer].size(), dValue, m_dMaxOverlap);
//						if (dValue < vecLayerScore[iTopLayer])
//							break;
//						vecMatchParameter.push_back(s_MatchParameter(Point2f(ptMaxLoc.x - fTranslationX, ptMaxLoc.y - fTranslationY), dValue, vecAngles[i]));
//					}
//				}
//			}
//			catch (exception ex)
//			{
//
//			}
//		}
//		std::sort(vecMatchParameter.begin(), vecMatchParameter.end(), compareScoreBig2Small);
//
//
//		int iMatchSize = (int)vecMatchParameter.size();
//		int iDstW = pTemplData->vecPyramid[iTopLayer].cols, iDstH = pTemplData->vecPyramid[iTopLayer].rows;
//
//	
//
//		bool bSubPixelEstimation = m_bSubPixel;
//		int iStopLayer = m_bStopLayer1 ? 1 : 0; //设置为1时：粗匹配，牺牲精度提升速度。
//		//int iSearchSize = min (m_iMaxPos + MATCH_CANDIDATE_NUM, (int)vecMatchParameter.size ());//可能不需要搜尋到全部 太浪費時間
//		vector<s_MatchParameter> vecAllResult;
//		for (int i = 0; i < (int)vecMatchParameter.size(); i++)
//			//for (int i = 0; i < iSearchSize; i++)
//		{
//			double dRAngle = -vecMatchParameter[i].dMatchAngle * D2R;
//			Point2f ptLT = img->ptRotatePt2f(vecMatchParameter[i].pt, ptCenter, dRAngle);
//
//			double dAngleStep = atan(2.0 / max(iDstW, iDstH)) * R2D;//min改為max
//			vecMatchParameter[i].dAngleStart = vecMatchParameter[i].dMatchAngle - dAngleStep;
//			vecMatchParameter[i].dAngleEnd = vecMatchParameter[i].dMatchAngle + dAngleStep;
//
//			if (iTopLayer <= iStopLayer)
//			{
//				vecMatchParameter[i].pt = Point2d(ptLT * ((iTopLayer == 0) ? 1 : 2));
//				vecAllResult.push_back(vecMatchParameter[i]);
//			}
//			else
//			{
//				for (int iLayer = iTopLayer - 1; iLayer >= iStopLayer; iLayer--)
//				{
//					//搜尋角度
//					dAngleStep = atan(2.0 / max(pTemplData->vecPyramid[iLayer].cols, pTemplData->vecPyramid[iLayer].rows)) * R2D;//min改為max
//					vector<double> vecAngles;
//					//double dAngleS = vecMatchParameter[i].dAngleStart, dAngleE = vecMatchParameter[i].dAngleEnd;
//					double dMatchedAngle = vecMatchParameter[i].dMatchAngle;
//					if (m_bToleranceRange)
//					{
//						for (int i = -1; i <= 1; i++)
//							vecAngles.push_back(dMatchedAngle + dAngleStep * i);
//					}
//					else
//					{
//						if (m_dToleranceAngle < VISION_TOLERANCE)
//							vecAngles.push_back(0.0);
//						else
//							for (int i = -1; i <= 1; i++)
//								vecAngles.push_back(dMatchedAngle + dAngleStep * i);
//					}
//					Point2f ptSrcCenter((vecMatSrcPyr[iLayer].cols - 1) / 2.0f, (vecMatSrcPyr[iLayer].rows - 1) / 2.0f);
//					iSize = (int)vecAngles.size();
//					vector<s_MatchParameter> vecNewMatchParameter(iSize);
//					int iMaxScoreIndex = 0;
//					double dBigValue = -1;
//					for (int j = 0; j < iSize; j++)
//					{
//						Mat matResult, matRotatedSrc;
//						double dMaxValue = 0;
//						cv::Point ptMaxLoc;
//						img->GetRotatedROI(vecMatSrcPyr[iLayer], pTemplData->vecPyramid[iLayer].size(), ptLT * 2, vecAngles[j], matRotatedSrc);
//
//						img->MatchTemplate(matRotatedSrc, pTemplData, matResult, iLayer, true, m_ckSIMD);
//						//matchTemplate (matRotatedSrc, pTemplData->vecPyramid[iLayer], matResult, CV_TM_CCOEFF_NORMED);
//						minMaxLoc(matResult, 0, &dMaxValue, 0, &ptMaxLoc);
//						vecNewMatchParameter[j] = s_MatchParameter(ptMaxLoc, dMaxValue, vecAngles[j]);
//
//						if (vecNewMatchParameter[j].dMatchScore > dBigValue)
//						{
//							iMaxScoreIndex = j;
//							dBigValue = vecNewMatchParameter[j].dMatchScore;
//						}
//						//次像素估計
//						if (ptMaxLoc.x == 0 || ptMaxLoc.y == 0 || ptMaxLoc.x == matResult.cols - 1 || ptMaxLoc.y == matResult.rows - 1)
//							vecNewMatchParameter[j].bPosOnBorder = true;
//						if (!vecNewMatchParameter[j].bPosOnBorder)
//						{
//							for (int y = -1; y <= 1; y++)
//								for (int x = -1; x <= 1; x++)
//									vecNewMatchParameter[j].vecResult[x + 1][y + 1] = matResult.at<float>(ptMaxLoc + cv::Point(x, y));
//						}
//						//次像素估計
//					}
//					if (vecNewMatchParameter[iMaxScoreIndex].dMatchScore < vecLayerScore[iLayer])
//						break;
//					//次像素估計
//					if (bSubPixelEstimation
//						&& iLayer == 0
//						&& (!vecNewMatchParameter[iMaxScoreIndex].bPosOnBorder)
//						&& iMaxScoreIndex != 0
//						&& iMaxScoreIndex != 2)
//					{
//						double dNewX = 0, dNewY = 0, dNewAngle = 0;
//						img->SubPixEsimation(&vecNewMatchParameter, &dNewX, &dNewY, &dNewAngle, dAngleStep, iMaxScoreIndex);
//						vecNewMatchParameter[iMaxScoreIndex].pt = Point2d(dNewX, dNewY);
//						vecNewMatchParameter[iMaxScoreIndex].dMatchAngle = dNewAngle;
//					}
//					//次像素估計
//
//					double dNewMatchAngle = vecNewMatchParameter[iMaxScoreIndex].dMatchAngle;
//
//					//讓坐標系回到旋轉時(GetRotatedROI)的(0, 0)
//					Point2f ptPaddingLT = img->ptRotatePt2f(ptLT * 2, ptSrcCenter, dNewMatchAngle * D2R) - Point2f(3, 3);
//					Point2f pt(vecNewMatchParameter[iMaxScoreIndex].pt.x + ptPaddingLT.x, vecNewMatchParameter[iMaxScoreIndex].pt.y + ptPaddingLT.y);
//					//再旋轉
//					pt = img->ptRotatePt2f(pt, ptSrcCenter, -dNewMatchAngle * D2R);
//
//					if (iLayer == iStopLayer)
//					{
//						vecNewMatchParameter[iMaxScoreIndex].pt = pt * (iStopLayer == 0 ? 1 : 2);
//						vecAllResult.push_back(vecNewMatchParameter[iMaxScoreIndex]);
//					}
//					else
//					{
//						//更新MatchAngle ptLT
//						vecMatchParameter[i].dMatchAngle = dNewMatchAngle;
//						vecMatchParameter[i].dAngleStart = vecMatchParameter[i].dMatchAngle - dAngleStep / 2;
//						vecMatchParameter[i].dAngleEnd = vecMatchParameter[i].dMatchAngle + dAngleStep / 2;
//						ptLT = pt;
//					}
//				}
//
//			}
//		}
//		img->FilterWithScore(&vecAllResult, m_dScore);
//
//		//最後濾掉重疊
//		iDstW = pTemplData->vecPyramid[iStopLayer].cols * (iStopLayer == 0 ? 1 : 2);
//		iDstH = pTemplData->vecPyramid[iStopLayer].rows * (iStopLayer == 0 ? 1 : 2);
//
//		for (int i = 0; i < (int)vecAllResult.size(); i++)
//		{
//			Point2f ptLT, ptRT, ptRB, ptLB;
//			double dRAngle = -vecAllResult[i].dMatchAngle * D2R;
//			ptLT = vecAllResult[i].pt;
//			ptRT = Point2f(ptLT.x + iDstW * (float)cos(dRAngle), ptLT.y - iDstW * (float)sin(dRAngle));
//			ptLB = Point2f(ptLT.x + iDstH * (float)sin(dRAngle), ptLT.y + iDstH * (float)cos(dRAngle));
//			ptRB = Point2f(ptRT.x + iDstH * (float)sin(dRAngle), ptRT.y + iDstH * (float)cos(dRAngle));
//			//紀錄旋轉矩形
//			vecAllResult[i].rectR = RotatedRect(ptLT, ptRT, ptRB);
//		}
//		img->FilterWithRotatedRect(&vecAllResult, CV_TM_CCOEFF_NORMED, m_dMaxOverlap);
//		std::sort(vecAllResult.begin(), vecAllResult.end(), compareScoreBig2Small);
//
//		//m_vecSingleTargetData.clear();
//		iMatchSize = (int)vecAllResult.size();
//		if (vecAllResult.size() == 0)
//		{
//		
//
//			return results;
//		}
//
//		int iW = pTemplData->vecPyramid[0].cols, iH = pTemplData->vecPyramid[0].rows;
//
//		for (int i = 0; i < iMatchSize; i++)
//		{
//			float angle = vecAllResult[i].dMatchAngle;
//
//
//			s_SingleTargetMatch sstm;
//			double dRAngle = -vecAllResult[i].dMatchAngle * D2R;
//
//			sstm.ptLT = vecAllResult[i].pt;
//
//			sstm.ptRT = Point2d(sstm.ptLT.x + iW * cos(dRAngle), sstm.ptLT.y - iW * sin(dRAngle));
//			sstm.ptLB = Point2d(sstm.ptLT.x + iH * sin(dRAngle), sstm.ptLT.y + iH * cos(dRAngle));
//			sstm.ptRB = Point2d(sstm.ptRT.x + iH * sin(dRAngle), sstm.ptRT.y + iH * cos(dRAngle));
//			sstm.ptCenter = Point2d((sstm.ptLT.x + sstm.ptRT.x + sstm.ptRB.x + sstm.ptLB.x) / 4, (sstm.ptLT.y + sstm.ptRT.y + sstm.ptRB.y + sstm.ptLB.y) / 4);
//			sstm.dMatchedAngle = -vecAllResult[i].dMatchAngle;
//			sstm.dMatchScore = vecAllResult[i].dMatchScore;
//
//			if (sstm.dMatchedAngle < -180)
//				sstm.dMatchedAngle += 360;
//			if (sstm.dMatchedAngle > 180)
//				sstm.dMatchedAngle -= 360;
//			//m_vecSingleTargetData.push_back(sstm);
//			Rect rect = vecAllResult[i].rectBounding;
//			listMatch += sstm.ptCenter.x.ToString() + "," + sstm.ptCenter.y.ToString() + "," + (-sstm.dMatchedAngle).ToString() + "," + iW.ToString() + "," + iH.ToString() + "," + vecAllResult[i].dMatchScore * 100 + "\n";
//			double score = vecAllResult[i].dMatchScore * 100;
//			
//			if (i + 1 == m_iMaxPos)
//				break;
//		}
//		for (size_t i = 0; i < vecAllResult.size(); ++i)
//		{
//			cv::Point2f c = vecAllResult[i].rectR.center;
//			Rotaterectangle rr;
//			rr.Cx = c.x; rr.Cy = c.y;
//			rr.AngleDeg = -vecAllResult[i].dMatchAngle;
//			rr.Width = (double)iW;
//			rr.Height = (double)iH;
//			rr.Score = vecAllResult[i].dMatchScore * 100.0;
//			results->Add(rr);
//			if ((int)i + 1 == m_iMaxPos) break;
//		}
//
//	
//	
//  
//   
//        return results;
//    
//}
