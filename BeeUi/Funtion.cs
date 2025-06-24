using BeeCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BeeUi
{
    public class Funtion
    {
         public static void SaveModel(String path, Model Model)
        {
            using (MemoryStream ms = new MemoryStream())
            {

                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, Model);
                ms.Position = 0;
                byte[] buffer = new byte[(int)ms.Length];
                ms.Read(buffer, 0, buffer.Length);
                File.WriteAllText(path, Convert.ToBase64String(buffer));
                File.Exists(path);


            }
        }
        public static Model LoadModel(string Path)
        {
            Model model = new Model();

            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(File.ReadAllText(Path))))
            {
                BinaryFormatter bf = new BinaryFormatter();
                model = (Model)bf.Deserialize(ms);
            }
            return model;
        }
    }
}
