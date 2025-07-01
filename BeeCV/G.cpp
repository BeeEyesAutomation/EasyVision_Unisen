
#include "G.h"
#include "MvCamera.h"
using  namespace CvPlus;

namespace CvPlus
{
	std::mutex gilmutex;
	
	//CDeviceInfo nameBasler;
	//Camera_t baslerGigE;
	//CGrabResultPtr ptrGrabResult;
	//CPylonImage image;/////anh output
	//CImageFormatConverter fc;///anh convert
	

	//HIK
	CMvCamera* m_pcMyCamera1;   
	CMvCamera* m_pcMyCamera2;
	CMvCamera* m_pcMyCamera3;
	CMvCamera* m_pcMyCamera4;
	HWND                    m_hwndDisplay;                       
	MV_CC_DEVICE_INFO_LIST  m_stDevList;
	cv::Mat matCrop;
	cv::VideoCapture camUSB;
	cv::Mat matTemp, matRaw, matResult, matRsTemp;
	cv::Mat matSetTemp;
	vector<vector< cv::Mat>> m_matDst(4);
	vector< cv::Mat> listmatTemp;
	 uchar* ucRaw; uchar* ucCrop;

	 void LogError(const std::string& message) {
		 std::ofstream logFile("log.txt", std::ios::app); // mở ở chế độ append
		 if (logFile.is_open()) {
			 logFile << "[ERROR] " << message << std::endl;
		 }
	 }
	string _toString(System::String^ STR)
	{
		char cStr[1000] = { 0 };
		sprintf(cStr, "%s", STR);
		std::string s(cStr);
		return s;
	}
	
	Mat CropImage(Mat matCrop, Rect rect)
	{
		return matCrop(rect);
	}
	void Common::BitmapSrc(Bitmap^ bm)
	{
		matRaw = BitmapToMat(bm);
	}
	
	void Common::BitmapDst(Bitmap^ bm,int ix)
	{
		m_matDst[ix]  = BitmapToMat(bm);
	}
	void Common::CropRotate(int x, int y, int w, int  h, float angle) {
		if (matRaw.empty())return;
		matCrop = RotateMat(matRaw.clone(), RotatedRect(cv::Point2f(x, y), cv::Size2f(w, h), angle));
		if (matCrop.type() != CV_8UC3)
		{
			cv::cvtColor(matCrop, matCrop, COLOR_GRAY2RGB);
	}
	}
	//uchar* MatToBytes(cv::Mat image, int* out_size)
	//{
	//	int image_size = image.total() * image.elemSize();
	//	uchar* image_uchar = new uchar[image_size];
	//	std::memcpy(image_uchar, image.data, image_size);
	//	*out_size = image_size;
	//	return image_uchar;
	//}
	uchar* MatToBytes(cv::Mat image)
	{
		int image_size = image.total() * image.elemSize();
		uchar* image_uchar = new uchar[image_size];
		//image_uchar is a class data member of uchar*
		std::memcpy(image_uchar, image.data, image_size * sizeof(uchar));
		return image_uchar;
	}
	cv::Mat BytesToMat(uchar* uc, int image_rows, int image_cols, int image_type)
	{
		cv::Mat img(image_rows, image_cols, image_type, uc, cv::Mat::AUTO_STEP);

		return img;
	}
	int getMaxAreaContourId(vector <vector<cv::Point>> contours) {
		double maxArea = 0;
		int maxAreaContourId = -1;
		for (int j = 0; j < contours.size(); j++) {
			double newArea = cv::contourArea(contours.at(j));
			if (newArea > maxArea) {
				maxArea = newArea;
				maxAreaContourId = j;
			} // End if
		} // End for
		return maxAreaContourId;
	}
	Mat RotateMat(Mat raw, RotatedRect rot)
	{
		Mat matRs, matR = getRotationMatrix2D(rot.center, rot.angle, 1);

		float fTranslationX = (rot.size.width - 1) / 2.0f - rot.center.x;
		float fTranslationY = (rot.size.height - 1) / 2.0f - rot.center.y;
		matR.at<double>(0, 2) += fTranslationX;
		matR.at<double>(1, 2) += fTranslationY;
		warpAffine(raw, matRs, matR, rot.size, INTER_LINEAR, BORDER_CONSTANT);
		return matRs;
	}
	Bitmap^ Common::GetImageRsTemp()
	{
		return MatToBitmap(matRsTemp);
	}

	Bitmap^ Common::GetImageRaw()
	{
		return MatToBitmap(matRaw);
	}
	void Common::LoadSrc(System::String^ path)
	{
		matRaw = cv::imread(_toString(path), ImreadModes::IMREAD_COLOR);
	}
	void Common::LoadDst(System::String^ path)
	{
		matTemp = cv::imread(_toString(path), ImreadModes::IMREAD_GRAYSCALE);
		///int a = matTemp.cols;
		//cv::imshow("a", matTemp);
	}
	

	Mat  BitmapToMat(System::Drawing::Bitmap^ bitmap)
	{
		IplImage* tmp = NULL;

		System::Drawing::Imaging::BitmapData^ bmData = bitmap->LockBits(System::Drawing::Rectangle(0, 0, bitmap->Width, bitmap->Height), System::Drawing::Imaging::ImageLockMode::ReadWrite, bitmap->PixelFormat);
		if (bitmap->PixelFormat == System::Drawing::Imaging::PixelFormat::Format8bppIndexed)
		{
			tmp = cvCreateImage(cvSize(bitmap->Width, bitmap->Height), IPL_DEPTH_8U, 1);
			tmp->imageData = (char*)bmData->Scan0.ToPointer();
		}

		else if (bitmap->PixelFormat == System::Drawing::Imaging::PixelFormat::Format24bppRgb)
		{
			tmp = cvCreateImage(cvSize(bitmap->Width, bitmap->Height), IPL_DEPTH_8U, 3);
			tmp->imageData = (char*)bmData->Scan0.ToPointer();
		}

		bitmap->UnlockBits(bmData);

		return cvarrToMat(tmp);
	}
	Bitmap^ MatToBitmap(Mat img)
	{


		if (img.data == nullptr)
			return nullptr;
		if (img.type() != CV_8UC3)
		{
			//throw gcnew NotSupportedException("Only images of type CV_8UC3 are supported for conversion to Bitmap");
		}

		//create the bitmap and get the pointer to the data
		Bitmap^ bmpimg = gcnew Bitmap(img.cols, img.rows, PixelFormat::Format24bppRgb);

		BitmapData^ data = bmpimg->LockBits(System::Drawing::Rectangle(0, 0, img.cols, img.rows), ImageLockMode::WriteOnly, PixelFormat::Format24bppRgb);

		byte* dstData = reinterpret_cast<byte*>(data->Scan0.ToPointer());

		unsigned char* srcData = img.data;

		for (int row = 0; row < data->Height; ++row)
		{
			memcpy(reinterpret_cast<void*>(&dstData[row * data->Stride]), reinterpret_cast<void*>(&srcData[row * img.step]), img.cols * img.channels());
		}

		bmpimg->UnlockBits(data);
		delete(data);
		img.release();
		return bmpimg;
	}
}