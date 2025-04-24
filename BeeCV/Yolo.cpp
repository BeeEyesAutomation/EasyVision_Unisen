#include "Yolo.h"

namespace py = pybind11;


using namespace CvPlus;

struct gil_scoped_acquire_local {
    gil_scoped_acquire_local() : state(PyGILState_Ensure()) {}
    gil_scoped_acquire_local(const gil_scoped_acquire_local&) = delete;
    gil_scoped_acquire_local& operator=(const gil_scoped_acquire_local&) = delete;
    ~gil_scoped_acquire_local() { PyGILState_Release(state); }
    const PyGILState_STATE state;
};
py::object _yolo;
bool Yolo::ClosePython()
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

   // py::gil_scoped_release release;
   if (Py_IsInitialized())
       Py_Finalize();
    return true;
}
bool Yolo::InitializeYolo()
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
		py::module processor_module = py::module::import("yolo");
		_yolo =   processor_module.attr("ObjectDetector")();
		auto ptr = std::make_unique<int[]>(10);
		//lock.unlock();
		//py::gil_scoped_release release;
		//PyEval_SaveThread
		//PylonInitialize();
		return true;
       
    
    }
    catch (exception ex)
    {
        string s = ex.what();
        return false;
    }
    return true;
}
int TypeYolo = 1;
bool  Yolo::LoadModel (System::String^ nameModel,int Type)
{
    try
    {
		TypeYolo = Type;
        std::string model = msclr::interop::marshal_as<std::string>(nameModel);
       // Thay "your_module_name" bằng tên module Python chứa hàm load_model và predict
        _yolo.attr("load_model")(model, Type);
        // Khởi tạo interpreter Python
        //py::scoped_interpreter guard{};
    }
    catch (exception ex)
    {
        string s = ex.what();
        return false;
    }
  
    return true;

}
cv::Mat numpy_array_to_cv_mat(const py::array_t<unsigned char>& arr) {
		// Chuyển đổi mảng NumPy sang cv::Mat
		py::buffer_info buf_info = arr.request();

		// Kích thước hình ảnh
		int height = buf_info.shape[0];
		int width = buf_info.shape[1];
		int channels = buf_info.shape[2];

		// Tạo Mat từ dữ liệu
		return cv::Mat(height, width, CV_8UC3, (unsigned char*)buf_info.ptr);
	}
py::array_t<unsigned char> mat_to_numpy(const cv::Mat& mat) {
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
    float ScoreYolo=0;
    string listYolo;

System::String^  Yolo::Check(float Score)
	{

	ScoreYolo = Score;
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
		py::array_t<uint8_t> image_array = mat_to_numpy(matCrop);
		auto result = _yolo.attr("predict")(image_array, ScoreYolo);

		

		if (py::isinstance<py::tuple>(result)) {
			auto result_tuple = result.cast<py::tuple>();
			//numDetected = result_tuple[0].cast<int>(); // Số vật thể được phát hiện

			
				py::list boxes = result_tuple[0].cast<py::list>();
				//std::printf(boxes.str());
				py::list scores =result_tuple[1].cast<py::list>();
				py::list labels = result_tuple[2].cast<py::list>();
			//	matProc = matRaw.clone();

				for (size_t i = 0; i < boxes.size(); ++i) {
					auto box = boxes[i].cast<py::tuple>();
					int x1 = box[0].cast<int>(), y1 = box[1].cast<int>();
					int x2 = box[2].cast<int>(), y2 = box[3].cast<int>();
					float score =scores[i].cast<float>();
					string label = "";
					if(TypeYolo==1)
						label=labels[i].cast<string>();
					else if (TypeYolo == 2)
						label =	labels[i].cast<int>();
					int xCenter = Math::Min(x1,x2)+ Math::Abs(x1 - x2)/2;
					int yCenter = Math::Min(y1, y2) + Math::Abs(y1 - y2) / 2;
					int w =  Math::Abs(x1 - x2) ;
					int h =  Math::Abs(y1 - y2);
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

	Cycle =  clock()- startTime ;
	return gcnew System::String(listRS.c_str());
	}
	std::vector<float> scores;
	PYBIND11_MODULE(example, m) {
		m.def("numpy_array_to_cv_mat", &numpy_array_to_cv_mat, "Gui hinh Len Python");
		//m.def("Checing", &Checking, "Checing");
	}


