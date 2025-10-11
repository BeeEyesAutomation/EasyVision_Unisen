using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using BeeGlobal;

namespace BeeCore
{
    [Serializable]
    public class Access
    {
        // ===================== Generic helpers =====================

        // Serialize object -> Base64 string (hạn chế copy bộ nhớ)
        private static string SerializeToBase64<T>(T obj)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream(capacity: 64 * 1024))
            {
                bf.Serialize(ms, obj);
                if (ms.TryGetBuffer(out ArraySegment<byte> seg))
                    return Convert.ToBase64String(seg.Array, seg.Offset, (int)ms.Length);
                // Fallback (hiếm khi cần)
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        // Base64 file -> object (đọc cả file rồi decode)
        private static T DeserializeFromBase64File<T>(string path)
        {
            // Đọc text xong là file đã đóng, không lock
            string b64 = File.ReadAllText(path, Encoding.UTF8);
            byte[] bytes = Convert.FromBase64String(b64);
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream(bytes, writable: false))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }

        // Ghi text "atomic": ghi vào .tmp rồi Replace để tránh file dở dang
        private static void AtomicWriteAllText(string path, string content, Encoding enc = null)
        {
            enc = enc ?? Encoding.UTF8;
            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            string tmp = path + ".tmp";
            File.WriteAllText(tmp, content, enc);

            // Nếu file đích chưa tồn tại, Move sẽ nhanh hơn Replace
            if (!File.Exists(path))
            {
                File.Move(tmp, path);
            }
            else
            {
                // Replace đảm bảo atomic update (có tạo file backup .bak)
                string bak = path + ".bak";
                File.Replace(tmp, path, bak, ignoreMetadataErrors: true);
                // Thử xóa backup (không quan trọng nếu xóa lỗi)
                try { File.Delete(bak); } catch { /* ignore */ }
            }
        }

        // Save generic
        private static void SaveBase64<T>(string path, T obj)
        {
            string b64 = SerializeToBase64(obj);
            AtomicWriteAllText(path, b64, Encoding.UTF8);
        }

        // Load generic với default-factory khi lỗi/không có file
        private static T LoadBase64<T>(string path, Func<T> defaultFactory = null)
        {
            try
            {
                if (!File.Exists(path))
                    return defaultFactory != null ? defaultFactory() : default(T);
                return DeserializeFromBase64File<T>(path);
            }
            catch
            {
                return defaultFactory != null ? defaultFactory() : default(T);
            }
        }

        // ===================== Public API =====================

        // Trả về mảng bytes thay vì Stream (tránh trả về stream đã Dispose)
        public static byte[] SerializeBytes<T>(T objectToSerialize)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream(capacity: 32 * 1024))
            {
                bf.Serialize(ms, objectToSerialize);
                return ms.ToArray();
            }
        }

        // ----- Programs (List<List<PropetyTool>>) -----
        public static void SaveProg(string path, List<List<PropetyTool>> list)
        {
            SaveBase64(path, list);
        }

        public static List<List<PropetyTool>> LoadProg(string path)
        {
            return LoadBase64(path, defaultFactory: () => new List<List<PropetyTool>>());
        }

        // ----- Config -----
        public static void SaveConfig(string path, Config config)
        {
            SaveBase64(path, config);
        }

        public static Config LoadConfig(string path)
        {Config config = new Config();
            config= LoadBase64<Config>(path, defaultFactory: () => new Config());
            if (config.ColorOK.Name == "0")
                config.ColorOK = Color.FromArgb(0, 172, 73);
            if (config.ColorNG.Name == "0")
                config.ColorNG = Color.DarkRed;
            if (config.ColorInfor.Name == "0")
                config.ColorNG = Color.Blue;
            if (config.ColorNone.Name == "0")
                config.ColorNG = Color.Gray;
            if (config.ColorChoose.Name == "0")
                config.ColorChoose = Color.FromArgb(246, 204, 120);
            if (config.TextColor.Name == "0")
                config.TextColor = Color.White;
            if (config.RadEdit==0)
                config.RadEdit = 40;
            if (config.FontSize == 0)
                config.FontSize = 12;
            if (config.ThicknessLine == 0)
                config.ThicknessLine = 4;
            if (config.Opacity == 0)
                config.Opacity = 50;
          
            return config;


        }

        // ----- ParaCommon -----
        public static void SaveParaComon(string path, ParaCommon para)
        {
            SaveBase64(path, para);
        }

        public static ParaCommon LoadParaComon(string path)
        {
            return LoadBase64<ParaCommon>(path, defaultFactory: () => new ParaCommon());
        }

        // ----- ParaCamera -----
        public static void SaveParaCamera(string path, List<ParaCamera> list)
        {
            SaveBase64(path, list);
        }

        public static List<ParaCamera> LoadParaCamera(string path)
        {
            return LoadBase64(path, defaultFactory: () => new List<ParaCamera>());
        }

        // ----- Keys (instance methods trước đây giữ nguyên chữ ký) -----
        public void SaveKeys(string keys, string path)
        {
            SaveBase64(path, keys ?? string.Empty);
        }

        public string LoadKeys(string path)
        {
            return LoadBase64<string>(path, defaultFactory: () => null);
        }
    }
}
