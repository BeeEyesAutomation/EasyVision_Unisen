using BeeUi.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using BeeCore;
namespace BeeUi
{
    [Serializable()]

    public    class Access
    {
        public static Stream serialize<T>(T objectToSerialize)
        {
            using (MemoryStream mem = new MemoryStream())
            {

                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(mem, objectToSerialize);
                return mem;
            }
        }

        public static void SaveProg(String path, List<List<PropetyTool>> list)
        {
            
            using (MemoryStream ms = new MemoryStream())
            {
               
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, list);
                    ms.Position = 0;
                    byte[] buffer = new byte[(int)ms.Length];
                    ms.Read(buffer, 0, buffer.Length);
                    File.WriteAllText(path, Convert.ToBase64String(buffer));
                    File.Exists(path);
               
              
            }
        }
        public static List<List<PropetyTool>> LoadProg(string Path)
        {
            List<List<PropetyTool>> list = new List<List<PropetyTool>>();
          
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(File.ReadAllText(Path))))
            {
                BinaryFormatter bf = new BinaryFormatter();
                list = (List < List<PropetyTool>>)bf.Deserialize(ms);
            }
            return list;
        }
        public static void SaveConfig(String path, Config Config)
        {

            using (MemoryStream ms = new MemoryStream())
            {

                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, Config);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                File.WriteAllText(path, Convert.ToBase64String(buffer));
                File.Exists(path);


            }
        }
        public static Config LoadConfig(string Path)
        {
            Config Config;

            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(File.ReadAllText(Path))))
            {
                BinaryFormatter bf = new BinaryFormatter();
                Config = (Config)bf.Deserialize(ms);
            }
            return Config;
        }
        public static void SaveParaComon(String path, ParaCommon ParaCam)
        {

            using (MemoryStream ms = new MemoryStream())
            {

                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, ParaCam);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                File.WriteAllText(path, Convert.ToBase64String(buffer));
                File.Exists(path);


            }
        }
        public static ParaCommon LoadParaComon(string Path)
        {
            ParaCommon Config;

            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(File.ReadAllText(Path))))
            {
                BinaryFormatter bf = new BinaryFormatter();
                Config = (ParaCommon)bf.Deserialize(ms);
            }
            return Config;
        }
        public static void SaveParaCamera(String path, List< ParaCamera> ParaCam)
        {

            using (MemoryStream ms = new MemoryStream())
            {

                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, ParaCam);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                File.WriteAllText(path, Convert.ToBase64String(buffer));
                File.Exists(path);


            }
        }
        public static List<ParaCamera> LoadParaCamera(string Path)
        {
            List< ParaCamera> Config;

            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(File.ReadAllText(Path))))
            {
                BinaryFormatter bf = new BinaryFormatter();
                Config = (List<ParaCamera>)bf.Deserialize(ms);
            }
            return Config;
        }
        public void SaveKeys(String Keys, String path)
        {



            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, Keys);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                File.WriteAllText(path, Convert.ToBase64String(buffer));



            }
        }
        public String LoadKeys(String path)
        {
            try
            {
                String text = "";
                text = File.ReadAllText(path);
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(text)))
                {

                    BinaryFormatter bf = new BinaryFormatter();
                    return (String)bf.Deserialize(ms);
                }

            }
            catch (Exception)
            { }
            return null;
        }
    }
}
