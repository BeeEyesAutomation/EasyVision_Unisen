#include "OCR.h"


namespace py = pybind11;


using namespace CvPlus;

struct gil_scoped_acquire_local {
    gil_scoped_acquire_local() : state(PyGILState_Ensure()) {}
    gil_scoped_acquire_local(const gil_scoped_acquire_local&) = delete;
    gil_scoped_acquire_local& operator=(const gil_scoped_acquire_local&) = delete;
    ~gil_scoped_acquire_local() { PyGILState_Release(state); }
    const PyGILState_STATE state;
};
py::object Obj;

bool OCR::Close()
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
	Obj = py::none();

	// py::gil_scoped_release release;
	if (Py_IsInitialized())
		Py_Finalize();
	return true;
}
bool OCR::Ini()
{
	py::scoped_interpreter guard{};  // Bắt đầu Python

	try {

		py::module ocr_module = py::module::import("OCR");  // file là ocr_wrapper.py
		py::object OCRWrapper = ocr_module.attr("OCRWrapper");
		Obj = OCRWrapper();

		bool success = Obj.attr("initialize_ocr")().cast<bool>();
		if (!success) {
			std::cerr << "Failed to initialize OCR" << std::endl;
			return 1;
		}
	}
	catch (py::error_already_set& e) {
		std::cerr << "Python Error: " << e.what() << std::endl;
	}

	return true;
}

cv::Mat array_to_cv_mat(const py::array_t<unsigned char>& arr) {
	// Chuyển đổi mảng NumPy sang cv::Mat
	py::buffer_info buf_info = arr.request();

	// Kích thước hình ảnh
	int height = buf_info.shape[0];
	int width = buf_info.shape[1];
	int channels = buf_info.shape[2];

	// Tạo Mat từ dữ liệu
	return cv::Mat(height, width, CV_8UC3, (unsigned char*)buf_info.ptr);
}
py::array_t<unsigned char> to_numpy(const cv::Mat& mat) {
	// Kiểm tra kích thước của mat
	if (mat.empty()) {
		throw std::runtime_error("cv::Mat is empty");
	}

	// Tạo mảng NumPy từ cv::Mat
	return py::array_t<unsigned char>(
		{ mat.rows, mat.cols, mat.channels() },  // Kích thước của mảng
		mat.data                               // Dữ liệu
	);
}
System::String^ OCR::Find(float Score)
{

	
	double startTime = clock();
	int numDetected = 0;
	float pixelCable = 0, sumOfAll = 0;
	int distanceV = 0, cycleTime = 0;
	std::ostringstream resultStream;
	std::string status;
	std::string  listRS = "";
	try {
		std::lock_guard<std::mutex> lock(gilmutex);

		py::gil_scoped_acquire acquire;
	
		py::array_t<uint8_t> image_array = to_numpy(matCrop);
		py::object result = Obj.attr("find_ocr")(image_array);
		//if (!result.is_none()) {
		//	for (auto item : result) {
		//		std::string text = item[0].cast<std::string>();
		//		//float conf = item[1].cast<float>();
		//		std::cout << "Detected: " << text << " (Conf: " << conf << ")" << std::endl;
		//	}
		//}
		
		//auto result = Obj.attr("predict")(image_array, ScoreYolo);



		if (py::isinstance<py::tuple>(result)) {
			auto result_tuple = result.cast<py::tuple>();
			//numDetected = result_tuple[0].cast<int>(); // Số vật thể được phát hiện


			py::list boxes = result_tuple[0].cast<py::list>();
			//std::printf(boxes.str());
			py::list scores = result_tuple[1].cast<py::list>();
			py::list labels = result_tuple[2].cast<py::list>();
			//	matProc = matRaw.clone();

			for (size_t i = 0; i < boxes.size(); ++i) {
				auto box = boxes[i].cast<py::tuple>();
				int x1 = box[0].cast<int>(), y1 = box[1].cast<int>();
				int x2 = box[2].cast<int>(), y2 = box[3].cast<int>();
				float score = scores[i].cast<float>();
				string label = "";
				/*if (TypeYolo == 1)
					label = labels[i].cast<string>();
				else if (TypeYolo == 2)
					label = labels[i].cast<int>();*/
				int xCenter = Math::Min(x1, x2) + Math::Abs(x1 - x2) / 2;
				int yCenter = Math::Min(y1, y2) + Math::Abs(y1 - y2) / 2;
				int w = Math::Abs(x1 - x2);
				int h = Math::Abs(y1 - y2);
				listRS.append(std::to_string(xCenter)).append(",").append(std::to_string(yCenter)).append(",").append(std::to_string(w)).append(",").append(std::to_string(h)).append(",0,").append(std::to_string(score)).append(",").append(label).append(",\n");
			}


		}


	}
	catch (const py::error_already_set& e) {

		return gcnew System::String(e.what());

		status = std::string("PYTHON ERROR: ") + e.what();
	}
	catch (...) {
		status = "UNKNOWN EXCEPTION.";
	}

	Cycle = clock() - startTime;
	return gcnew System::String(listRS.c_str());
}

PYBIND11_MODULE(OCR, m) {
	m.def("array_to_cv_mat", &array_to_cv_mat, "Gui hinh Len Python");
	//m.def("Checing", &Checking, "Checing");
}

