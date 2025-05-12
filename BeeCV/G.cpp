
#include "G.h"
#include "MvCamera.h"
using  namespace CvPlus;

namespace CvPlus
{
	std::mutex gilmutex;
	py::object _ocr;
	py::object _yolo;
	//CDeviceInfo nameBasler;
	//Camera_t baslerGigE;
	//CGrabResultPtr ptrGrabResult;
	//CPylonImage image;/////anh output
	//CImageFormatConverter fc;///anh convert
	

	//HIK
	CMvCamera* m_pcMyCamera;               
	HWND                    m_hwndDisplay;                       
	MV_CC_DEVICE_INFO_LIST  m_stDevList;
	cv::Mat matCrop;
	cv::VideoCapture camUSB;
	cv::Mat matTemp, matRaw, matResult, matRsTemp;
	cv::Mat matSetTemp, m_matSrc;
	vector< cv::Mat> m_matDst;
	 uchar* ucRaw; uchar* ucCrop;
	string _toString(System::String^ STR)
	{
		char cStr[1000] = { 0 };
		sprintf(cStr, "%s", STR);
		std::string s(cStr);
		return s;
	}
	struct gil_scoped_acquire_local {
		gil_scoped_acquire_local() : state(PyGILState_Ensure()) {}
		gil_scoped_acquire_local(const gil_scoped_acquire_local&) = delete;
		gil_scoped_acquire_local& operator=(const gil_scoped_acquire_local&) = delete;
		~gil_scoped_acquire_local() { PyGILState_Release(state); }
		const PyGILState_STATE state;
	};
	void import_python_module_from_path(const std::string& full_path, const std::string& module_name) {
		py::module sys = py::module::import("sys");
		py::module importlib_util = py::module::import("importlib.util");

		// Lấy spec từ đường dẫn file
		auto spec = importlib_util.attr("spec_from_file_location")(module_name, full_path);

		// Tạo module từ spec
		auto module = importlib_util.attr("module_from_spec")(spec);

		// Thực thi loader
		auto loader = spec.attr("loader");
		loader.attr("exec_module")(module);

		// Nếu muốn gán vào sys.modules
		sys.attr("modules")[module_name.c_str()] = module;
	}string sEX;
	inline py::object import_from_path(const std::string& module_name, const std::string& file_path) {
		py::module importlib_util = py::module::import("importlib.util");
		py::object spec = importlib_util.attr("spec_from_file_location")(module_name, file_path);
		py::object module = importlib_util.attr("module_from_spec")(spec);
		spec.attr("loader").attr("exec_module")(module);
		return module;
	}
	System::String^ Common::IniPython()
	{
		try
		{
			//std::lock_guard<std::mutex> lock(gilmutex);
			Py_Initialize();
			gil_scoped_acquire_local gil_acquire;

			//	std::lock_guard<std::mutex> lock(gilmutex);
			//std::lock_guard<std::mutex>lock(gilmutex);

				//std::unique_lock<std::mutex> lock(gilmutex);
				//py::gil_scoped_acquire acquire;
				//gil_scoped_acquire_local gil_acquire;
				//	py::gil_scoped_acquire acquire;
				//	py::gil_scoped_acquire acquire;  // Giành GIL
				//pybind11::gil_scoped_acquire acquire; // Tự động đăng ký và giữ GIL
			//_yolo= py::module::import("yolo");
			//import_python_module_from_path("Tool/Learning.py", "Learning");
			//	import_python_module_from_path("Tool/Ocr.py", "Ocr");
			auto sys = py::module::import("sys");
			std::cout << "Python executable: " << std::string(py::str(sys.attr("executable"))) << std::endl;
			std::cout << "Python version: " << std::string(py::str(sys.attr("version"))) << std::endl;
			try {
				auto easyocr = py::module::import("easyocr");
				std::cout << "easyocr is installed and importable!" << std::endl;
			}
			catch (py::error_already_set& e) {
				std::cout << "easyocr is NOT installed or failed to import." << std::endl;
				std::cout << e.what() << std::endl;
			}

			auto module2 = import_from_path("OcrWapper", "Tool/OcrWapper.py");
			//py::module module2 = py::module::import("Ocr");
			_ocr = module2.attr("OCRWrapper")();
			//py::module processor_module = py::module::import("Learning");
			auto module = import_from_path("Tool_Learning", "Tool/Learning.py");
			_yolo = module.attr("ObjectDetector")();
		
		
			auto ptr = std::make_unique<int[]>(10);
			//lock.unlock();
			//py::gil_scoped_release release;
			//PyEval_SaveThread
			//PylonInitialize();
			return "";


		}
		catch (exception ex)
		{
			sEX = ex.what();
			return gcnew System::String(sEX.c_str()); ;
		}
		 	return "true";
	}
	bool Common::ClosePython()
	{
		if (!Py_IsInitialized()) {
			std::cerr << "Python initialization failed!" << std::endl;
			return "Python initialization failed!";
		}
		//	_yolo.attr("close")();
			//Py_FinalizeEx();

			//Py_Finalize
			////py::finalize_interpreter();
			//return SUCCESS;

			   // 🚀 Giải phóng biến global bằng cách đặt về None
		_yolo = py::none();
		_ocr = py::none();
		// py::gil_scoped_release release;
		if (Py_IsInitialized())
			Py_Finalize();
		return true;
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