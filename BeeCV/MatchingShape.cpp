#include "MatchingShape.h"
using namespace CvPlus;


extern "C" __declspec(dllexport) uchar *  GetTemp(int* rows, int* cols, int* Type,int threshold,int minArea,int method,bool Invert)
{
	
    Mat raw = matCrop.clone();
    Mat matProccess = Mat();
    if (raw.type() == CV_8UC3)
        cvtColor(raw, raw, COLOR_BGR2GRAY);
    cv::threshold(raw, matProccess, threshold, 255, method);
	cv::bitwise_not(matProccess, matProccess);
	//cv::imshow("AA", matProccess);
	vector<vector < cv::Point> > contours;
	vector<cv::Vec4i> hierarchy;
	Mat matContour = cv::Mat(matProccess.rows, matProccess.cols, CV_8UC1, Scalar(0, 0, 0));
	cv::findContours(matProccess, contours, hierarchy, CV_RETR_TREE, CV_CHAIN_APPROX_SIMPLE, cv::Point(0, 0));
	
	//cv::findContours(matProcess,  contours,  hierarchy,RetrievalModes::RETR_EXTERNAL, ContourApproximationModes::CHAIN_APPROX_SIMPLE);
	if(contours.size()==0)
		return  MatToBytes(matContour);
	else
	{
		int index = getMaxAreaContourId(contours);
		if(Invert)
		cv::drawContours(matContour, contours, index, Scalar(255, 255, 255),3, LineTypes::LINE_4);
		////cv::cvtColor(matContour, matContour, COLOR_GRAY2BGR);
		//cv::bitwise_and(matContour, raw, RS);
		//cv::GaussianBlur(RS, RS, cv::Size(5, 5), 5, 5);
		//cv::erode(RS, RS, kernel);
		else
		for (int i = 0; i < contours.size(); i++)
		{
			int  Parent = hierarchy[i][3];
			if (hierarchy[i][2] == -1&& Parent == index) // means it has child contour
			{

				drawContours(matContour, contours, i, Scalar(255,255,255), -1, 8, hierarchy, 0, cv::Point());

			}
			/*if (i != index)
			{
				cv::drawContours(matContour, contours, index, Scalar(0,0,0), -1, LineTypes::LINE_4);


			}*/
		}
	
	}
	int rows_ = matContour.rows;
	int cols_ = matContour.cols;
	int Type_ = matContour.type();
	*rows = rows_;
	*cols = cols_;
	*Type = Type_;
	matTemp = matContour.clone();
	return  MatToBytes(matContour);

}
extern "C" __declspec(dllexport) void SetTemp(uchar * uc, int image_rows, int image_cols, int image_type)
{
	matTemp = BytesToMat(uc, image_rows, image_cols, image_type);

}
float MatchingShape::CheckShape(  int threshold, int minArea, int method, bool Invert) {
	double d1 = clock();
	Mat raw = matCrop.clone();
	Mat matProccess = Mat();
	if (raw.type() == CV_8UC3)
		cvtColor(raw, raw, COLOR_BGR2GRAY);
	cv::threshold(raw, matProccess, threshold, 255, method);
	cv::bitwise_not(matProccess, matProccess);
	vector<vector < cv::Point> > contours;
	vector<cv::Vec4i> hierarchy;
	Mat matMatching = Mat();
	matResult = cv::Mat(matProccess.rows, matProccess.cols, CV_8UC3, Scalar(0, 0, 0));
	matMatching = cv::Mat(matProccess.rows, matProccess.cols, CV_8UC1, Scalar(0, 0, 0));
	cv::findContours(matProccess, contours, hierarchy, CV_RETR_TREE, CV_CHAIN_APPROX_SIMPLE, cv::Point(0, 0));
	if (contours.size() == 0)
		return  0;
	else
	{
		int index = getMaxAreaContourId(contours);
		if (Invert)
		cv::drawContours(matMatching, contours, index, Scalar(255, 255, 255), 6, LineTypes::LINE_4);
		else
		for (int i = 0; i < contours.size(); i++)
		{
			int  Parent = hierarchy[i][3];
			if (hierarchy[i][2] == -1 && Parent == index) // means it has child contour
			{

				drawContours(matMatching, contours, i, Scalar(255, 255, 255), -1, 8, hierarchy, 0, cv::Point());

			}
			
		}

	}

	
	cv::bitwise_xor(matMatching, matTemp, matMatching);
	Mat kernel = cv::getStructuringElement(MORPH_ELLIPSE, cv::Size(3,3));
	cv::erode(matMatching, matMatching, kernel);
	cv::findContours(matMatching, contours, hierarchy, RETR_EXTERNAL, CV_CHAIN_APPROX_SIMPLE, cv::Point(0, 0));
	if (contours.size() == 0)
		return   0;
	int numNG = 0;
	for  (int i =0;i<contours.size();i++)
	{
		
		if (contourArea(contours[i])>minArea)
		{
			Rect rect = boundingRect(contours[i]);
			int min= Math::Min(rect.width, rect.height);
			int max = Math::Max(rect.width, rect.height);
			if ((float)(min * 1.0 / max) * 100 > 10)
			{
				drawContours(matResult, contours, i, Scalar(255, 255, 0, 0), -1, 8, hierarchy, 0, cv::Point());
				numNG++;
			}
		}
	}
	cycleTool = int(clock() - d1);
	return numNG;
}
