using BeeGlobal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeInterface
{
    public class Load
    { public static void NewTool()
        {
            Global.itemNews = new List<ItemNew>();
            Global.itemNews.Add(new ItemNew(TypeTool.Position_Adjustment, GroupTool.Basic_Tool, Properties.Resources.Position_Adjustment, Properties.Resources.contentPosition_Adjustment, "🔧 Căn chỉnh vị trí thông minh\r\nTự động phát hiện và hiệu chỉnh sai lệch xoay, trượt hoặc lệch góc của sản phẩm.\r\n➡️ Đảm bảo hình ảnh luôn được chuẩn hóa trước khi kiểm tra, phù hợp với dây chuyền tốc độ cao."));
            Global.itemNews.Add(new ItemNew(TypeTool.Pattern, GroupTool.Basic_Tool, Properties.Resources.Pattern, Properties.Resources.contentPattern, "📐 Nhận dạng mẫu chuẩn xác\r\nSo khớp với mẫu tham chiếu để xác định vị trí, hướng và độ chính xác của chi tiết.\r\n➡️ Ứng dụng trong định vị linh kiện, gắp đặt robot hoặc kiểm tra lắp ráp."));
            Global.itemNews.Add(new ItemNew(TypeTool.OKNG, GroupTool.Basic_Tool, Properties.Resources.OKNG, Properties.Resources.contentOKNG, "✅❌ Phân loại tức thì\r\nĐưa ra kết quả OK (đạt) hoặc NG (không đạt) theo tiêu chí cài đặt.\r\n➡️ Tích hợp trực tiếp với PLC/I/O, phản hồi ngay trên dây chuyền sản xuất."));
            Global.itemNews.Add(new ItemNew(TypeTool.Color_Area, GroupTool.Basic_Tool, Properties.Resources.Color_Area, Properties.Resources.Color_Area, "🎨 Kiểm tra vùng màu công nghiệp\r\nĐo lường chính xác màu sắc và diện tích vùng cần kiểm tra.\r\n➡️ Ứng dụng trong phân loại sản phẩm, in ấn bao bì, nhận diện logo hoặc nhãn mác."));
            Global.itemNews.Add(new ItemNew(TypeTool.VisualMatch, GroupTool.Basic_Tool, Properties.Resources.MatchingShape, Properties.Resources.contentVisualMatch, "So khớp mẫu (Visual Matching): So sánh hình ảnh sản phẩm thực tế với ảnh mẫu chuẩn (master).\r\n\r\nPhân loại OK/NG: Kết quả tự động phân loại sản phẩm đạt chuẩn (OK) hoặc lỗi (NG).\r\n\r\nNhận diện lỗi phổ biến:\r\n\r\nSai màu (Color Mismatch)\r\n\r\nXước, vết dơ (Scratch, Stain)\r\n\r\nSai biên dạng/hình dạng (Shape Mismatch)\r\n\r\nThiếu/dư chi tiết (Missing/Extra Region)"));
          
            Global.itemNews.Add(new ItemNew(TypeTool.Width, GroupTool.Extra_Tool_1, Properties.Resources.Width, Properties.Resources.Width, "Content.."));
            Global.itemNews.Add(new ItemNew(TypeTool.Circle, GroupTool.Extra_Tool_1, Properties.Resources.Circle, Properties.Resources.Circle, "Content.."));
            Global.itemNews.Add(new ItemNew(TypeTool.Corner, GroupTool.Extra_Tool_1, Properties.Resources.Width, Properties.Resources.Width, "Content.."));
            Global.itemNews.Add(new ItemNew(TypeTool.Measure, GroupTool.Extra_Tool_1, Properties.Resources.Measure, Properties.Resources.Measure, "Content.."));
            Global.itemNews.Add(new ItemNew(TypeTool.MatchingShape, GroupTool.Extra_Tool_1, Properties.Resources.MatchingShape, Properties.Resources.contentMatchingShape, "📏 Đối sánh hình dạng\r\nNhận diện và phân tích hình dáng của vật thể dù bị xoay, phóng to/thu nhỏ hoặc nhiễu nền.\r\n➡️ Giải pháp mạnh cho kiểm tra ngoại quan, phát hiện sai hình dạng, thiếu chi tiết."));

            Global.itemNews.Add(new ItemNew(TypeTool.Learning, GroupTool.Extra_Tool_2, Properties.Resources.Learning, Properties.Resources.Learning, "Content.."));
            Global.itemNews.Add(new ItemNew(TypeTool.OCR, GroupTool.Extra_Tool_2, Properties.Resources.OCR, Properties.Resources.OCR, "Content.."));
            Global.itemNews.Add(new ItemNew(TypeTool.BarCode, GroupTool.Extra_Tool_2, Properties.Resources.BarCode, Properties.Resources.BarCode, "Content.."));
            Global.itemNews.Add(new ItemNew(TypeTool.Crop, GroupTool.Extra_Tool_2, Properties.Resources.Crop, Properties.Resources.contentCrop, "✂️ Cắt vùng quan sát\r\nLựa chọn và cắt chính xác khu vực cần xử lý để tăng tốc độ tính toán và giảm nhiễu.\r\n➡️ Tối ưu hiệu năng và tập trung vào điểm kiểm tra quan trọng."));

        }
    }
}
