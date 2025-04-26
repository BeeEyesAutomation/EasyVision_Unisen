using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace IniConfigFile_n
{
	internal class IniConfigFile
	{
		public Dictionary<string, string> configData;

		private string fullFileName;

		public void Config(string _fileName)
		{
			configData = new Dictionary<string, string>();
			fullFileName = Application.StartupPath + "\\" + _fileName;
			if (!File.Exists(Application.StartupPath + "\\" + _fileName))
			{
				StreamWriter streamWriter = new StreamWriter(File.Create(Application.StartupPath + "\\" + _fileName), Encoding.Default);
				streamWriter.Close();
			}
			StreamReader streamReader = new StreamReader(Application.StartupPath + "\\" + _fileName, Encoding.Default);
			int num = 0;
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				if (text.StartsWith(";") || string.IsNullOrEmpty(text))
				{
					configData.Add(";" + num++, text);
					continue;
				}
				string[] array = text.Split('=');
				if (array.Length >= 2)
				{
					configData.Add(array[0], array[1]);
				}
				else
				{
					configData.Add(";" + num++, text);
				}
			}
			streamReader.Close();
		}

		public string get(string key)
		{
			if (configData.Count <= 0)
			{
				return null;
			}
			if (configData.ContainsKey(key))
			{
				return configData[key].ToString();
			}
			return null;
		}

		public void set(string key, string value)
		{
			if (configData.ContainsKey(key))
			{
				configData[key] = value;
			}
			else
			{
				configData.Add(key, value);
			}
		}

		public void save()
		{
			StreamWriter streamWriter = new StreamWriter(fullFileName, false, Encoding.Default);
			IDictionaryEnumerator dictionaryEnumerator = configData.GetEnumerator();
			while (dictionaryEnumerator.MoveNext())
			{
				if (dictionaryEnumerator.Key.ToString().StartsWith(";"))
				{
					streamWriter.WriteLine(dictionaryEnumerator.Value);
				}
				else
				{
					streamWriter.WriteLine(string.Concat(dictionaryEnumerator.Key, "=", dictionaryEnumerator.Value));
				}
			}
			streamWriter.Close();
		}
	}
}
