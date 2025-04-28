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



bool OCR::SetModel()
{
	bool success = _ocr.attr("initialize_ocr")().cast<bool>();
	if (!success) {
		std::cerr << "Failed to initialize OCR" << std::endl;
		return 1;
	}
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
		if (matCrop.type() != CV_8UC3)
			cv::cvtColor(matCrop, matCrop, COLOR_GRAY2RGB);
		
			cv::cvtColor(matCrop, matCrop, COLOR_BGR2RGB);
		// Xoay 90 độ chiều kim đồng hồ
		cv::Mat rotated;
		//cv::rotate(matCrop, rotated, cv::ROTATE_90_CLOCKWISE);

		// Xoay 90 độ ngược chiều kim đồng hồ
		//cv::rotate(matCrop, rotated, cv::ROTATE_90_COUNTERCLOCKWISE);

		// Xoay 180 độ
		//cv::rotate(src, rotated, cv::ROTATE_180);
		//cv::imwrite("crop.png", rotated);
		int h = matCrop.rows;
		int w = matCrop.cols;
		py::array_t<uint8_t> image_array = to_numpy(matCrop);
		py::object result = _ocr.attr("find_ocr")(image_array);

		
		
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
				
				// 1. Lấy center
				py::tuple rotated_rect = boxes[i].cast<py::tuple>();
				auto center = rotated_rect[0].cast<py::tuple>();
				float x = center[0].cast<float>();
				float y = center[1].cast<float>();

				// 2. Lấy size
				auto size = rotated_rect[1].cast<py::tuple>();
				float w= size[0].cast<float>();
				float h = size[1].cast<float>();

				// 3. Lấy angle
				float angle = rotated_rect[2].cast<float>();
				float score = scores[i].cast<float>();
				string  label = labels[i].cast<string>();
				//string label = "";
				/*if (TypeYolo == 1)
					label = labels[i].cast<string>();
				else if (TypeYolo == 2)
					label = labels[i].cast<int>();*/
			/*	int xCenter = Math::Min(x1, x2) + Math::Abs(x1 - x2) / 2;
				int yCenter = Math::Min(y1, y2) + Math::Abs(y1 - y2) / 2;
				int w = Math::Abs(x1 - x2);
				int h = Math::Abs(y1 - y2);*/
				listRS.append(std::to_string(x)).append(",").append(std::to_string(y)).append(",").append(std::to_string(w)).append(",").append(std::to_string(h)).append(",").append(std::to_string(angle)).append(",").append(std::to_string(score)).append(",").append(label).append("\n");
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

