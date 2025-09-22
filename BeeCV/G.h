
#include "pch.h"
#include <MvCameraControl.h>
#include "MvCamera.h"

//#include <pylon/PylonIncludes.h>
//#include <pylon/gige/BaslerGigEInstantCamera.h>
//typedef Pylon::CBaslerGigEInstantCamera Camera_t;
//typedef Camera_t::GrabResultPtr_t GrabResultPtr_t;

//using namespace GenApi;
//using namespace Basler_GigECameraParams;
//using namespace Pylon;
#include <opencv2/opencv.hpp>
#include <opencv2/highgui/highgui_c.h>
#include <opencv2/imgproc/imgproc_c.h>
#include <opencv2/imgproc/types_c.h>
#include <direct.h>
#include <iostream>
#include <fstream>
#include <stdio.h>
#include <process.h>         // needed for _beginthread()
#include <ctime>
#include <sys/types.h>
#include <sys/stat.h>
#include <stdio.h>
#include <stdlib.h>
#include <algorithm>
#define ENUM_TO_STR(ENUM) std::string(#ENUM)
#include <ctime>
#using <System.Windows.Forms.dll> 
#using <System.Drawing.dll> 
//#include "zbar.h"
#include <string>
#include <iostream>
#include <mutex>
#include <condition_variable>
#include <msclr/marshal_cppstd.h>

using namespace cv;
using namespace std;
using namespace System;
using namespace System::Threading;
using namespace System::Drawing;
using namespace System::Drawing::Imaging;

// Include files to use the pylon API.
//#include <pylon/PylonIncludes.h>
#ifdef PYLON_WIN_BUILD
#    include <pylon/PylonGUI.h>
#endif

namespace CvPlus {
	//HIK
	extern vector < CMvCamera*> m_pcMyCamera;
	extern vector< int > listTypeCCD;

	extern HWND                    m_hwndDisplay;                      
	extern MV_CC_DEVICE_INFO_LIST  m_stDevList;
	extern std::mutex gilmutex;
	extern uchar* MatToBytes(cv::Mat image);
	extern cv::Mat BytesToMat(uchar* uc, int image_rows, int image_cols, int image_type);
	extern int getMaxAreaContourId(vector <vector<cv::Point>> contours);
	extern Mat RotateMat(Mat raw, RotatedRect rot);
	void LogError(const std::string& message);
	Mat BitmapToMat(System::Drawing::Bitmap^ bitmap);
	Bitmap^ MatToBitmap(Mat img);
	extern cv::VideoCapture camUSB;
	extern uchar* ucRaw;
	extern cv::Mat matTemp, matRaw, matResult, matRsTemp,matCrop;
	extern cv::Mat matSetTemp;
	extern vector < vector<cv::Mat>> m_matDst;
	Mat CropImage(Mat matCrop, Rect rect);
	
	public ref  class Common
	{
	public:void CropRotate(int x, int y, int w, int  h, float angle);
	public:	Bitmap^	GetImageRaw();
	public:	Bitmap^ GetImageRsTemp();
	public:System::String^ Ex;
	public:void	BitmapSrc(Bitmap^ bm);
	public:void	BitmapDst(Bitmap^ bm, int ix);
	
	public:  void LoadSrc(System::String^ path);
	public:  void LoadDst(System::String^ path);
		 
	};

	 
}