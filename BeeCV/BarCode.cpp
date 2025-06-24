//#include "BarCode.h"
//using namespace CvPlus;
// // Định nghĩa và khởi tạo biến static
//void BarCode::Config() {
//scanner.set_config(ZBAR_NONE, ZBAR_CFG_ENABLE, 1);
//
//
//}
//System::String^ BarCode::Check() {
//    zbar::Image image;
//    try
//    {
//        // Chuyển đổi ảnh OpenCV Mat sang định dạng ZBar
//        int width = matRaw.cols;
//        int height = matRaw.rows;
//        uchar* raw = matRaw.data;
//        if (width == 0) return "FAIL W";
//        if (height == 0) return "FAIL H ";
//        zbar::Image image(width, height, "Y800", raw, width * height);
//        std::string strQRCODE = "";
//        // Quét ảnh để tìm barcode
//        int n = scanner.scan(image);
//        if (n > 0) {
//            for (zbar::Image::SymbolIterator symbol = image.symbol_begin(); symbol != image.symbol_end(); ++symbol) {
//                cout << "Loại mã vạch: " << symbol->get_type_name() << endl;
//                cout << "Dữ liệu: " << symbol->get_data() << endl;
//                strQRCODE += symbol->get_type_name() + ",";
//                strQRCODE += symbol->get_data() + "\n";
//            }
//        }
//        else {
//            strQRCODE = "404";
//        }
//        // Giải phóng tài nguyên
//            image.set_data(nullptr, 0);
//        return   gcnew  System::String(strQRCODE.c_str());;
//    }
//    catch (exception ex)
//    {
//        return "FAIL " + gcnew  System::String(ex.what());;
//      
//    }
//    finally {
//       
//    }
//   
//	return "";
//}