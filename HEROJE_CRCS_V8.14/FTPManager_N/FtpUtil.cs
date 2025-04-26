using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace FTPManager_N
{
	public class FtpUtil
	{
		private string ftpUserName;

		private string ftpPassword;

		private string currentpath;

		private bool IsRtosFtp = false;

		public void InitFtpParam(string userName, string pwd, bool is_rtos = false)
		{
			ftpUserName = userName;
			ftpPassword = pwd;
			currentpath = "/";
			IsRtosFtp = is_rtos;
		}

		public string UploadDirectory(string localDir, string ftpPath, string dirName)
		{
			localDir = FolderFormat(localDir, '\\');
			ftpPath = FolderFormat(ftpPath, '/');
			string text = localDir + dirName + "\\";
			if (!Directory.Exists(text))
			{
				return text + "目录不存在";
			}
			if (DirExist(ftpPath, dirName))
			{
				DeleteDir(ftpPath, dirName);
			}
			MakeDir(ftpPath, dirName);
			List<List<string>> localDirDetails = GetLocalDirDetails(text);
			for (int i = 0; i < localDirDetails[0].Count; i++)
			{
				Console.WriteLine(localDirDetails[0][i]);
				UploadFile(text + localDirDetails[0][i], ftpPath + dirName + "/" + localDirDetails[0][i]);
			}
			for (int j = 0; j < localDirDetails[1].Count; j++)
			{
				UploadDirectory(text, ftpPath + dirName + "/", localDirDetails[1][j]);
			}
			return "0";
		}

		public bool UploadFile(string localFile, string ftpFile)
		{
			if (!File.Exists(localFile))
			{
				return false;
			}
			using (FileStream fileStream = new FileInfo(localFile).OpenRead())
			{
				byte[] array = new byte[fileStream.Length];
				fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
				return UploadFile(array, (int)fileStream.Length, ftpFile);
			}
		}

		public bool UploadFile(string localFile, string ftpPath, string dirName, string fileName)
		{
			if (!File.Exists(localFile))
			{
				return false;
			}
			if (!DirExist(ftpPath, dirName))
			{
				MakeDir(ftpPath, dirName);
			}
			string ftpFile = ftpPath + "/" + dirName + "/" + fileName;
			using (FileStream fileStream = new FileInfo(localFile).OpenRead())
			{
				byte[] array = new byte[fileStream.Length];
				fileStream.Read(array, 0, Convert.ToInt32(fileStream.Length));
				return UploadFile(array, (int)fileStream.Length, ftpFile);
			}
		}

		public bool UploadFile(byte[] fileBuff, int fileNameLen, string ftpFile)
		{
			FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(ftpFile));
			try
			{
				ftpWebRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
				ftpWebRequest.UseBinary = true;
				ftpWebRequest.UsePassive = true;
				ftpWebRequest.Method = "STOR";
				ftpWebRequest.Proxy = null;
				ftpWebRequest.ContentLength = fileNameLen;
				using (Stream stream = ftpWebRequest.GetRequestStream())
				{
					stream.Write(fileBuff, 0, fileBuff.Length);
					stream.Close();
				}
				return true;
			}
			catch (Exception)
			{
			}
			finally
			{
				if (ftpWebRequest != null)
				{
					ftpWebRequest.Abort();
				}
			}
			return false;
		}

		public bool DownloadDirectory(string ftpPath, string localDir, string dirName)
		{
			try
			{
				string text = FolderFormat(ftpPath, '/') + dirName;
				string text2 = FolderFormat(localDir, '\\') + dirName;
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
				}
				List<DirItemInfo> ftpFileInfos = GetFtpFileInfos(text);
				for (int i = 0; i < ftpFileInfos.Count; i++)
				{
					DirItemInfo dirItemInfo = ftpFileInfos[i];
					if (ftpFileInfos[i].FileType == 0)
					{
						DownloadDirectory(text, text2, ftpFileInfos[i].Name);
					}
					else if (ftpFileInfos[i].FileType == 1)
					{
						DownloadFile(text, text2, ftpFileInfos[i].Name);
					}
				}
				return true;
			}
			catch (Exception)
			{
			}
			return false;
		}

		public byte[] DownloadFile(string ftpDir, string fileName)
		{
			try
			{
				string uriString = FolderFormat(ftpDir, '/') + fileName;
				FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(uriString));
				ftpWebRequest.Method = "RETR";
				ftpWebRequest.UseBinary = true;
				ftpWebRequest.Proxy = null;
				ftpWebRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
				FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
				Stream responseStream = ftpWebResponse.GetResponseStream();
				int num = 0;
				List<byte> list = new List<byte>();
				int num2 = -1;
				while ((num2 = responseStream.ReadByte()) != -1)
				{
					list.Add(Convert.ToByte(num2));
					num++;
				}
				responseStream.Close();
				ftpWebResponse.Close();
				return list.ToArray();
			}
			catch (Exception)
			{
				return null;
			}
		}

		public byte[] DownloadFile(string fileName)
		{
			try
			{
				FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(fileName));
				ftpWebRequest.Method = "RETR";
				ftpWebRequest.UseBinary = true;
				ftpWebRequest.Proxy = null;
				ftpWebRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
				FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
				Stream responseStream = ftpWebResponse.GetResponseStream();
				int num = 0;
				List<byte> list = new List<byte>();
				int num2 = -1;
				while ((num2 = responseStream.ReadByte()) != -1)
				{
					list.Add(Convert.ToByte(num2));
					num++;
				}
				responseStream.Close();
				ftpWebResponse.Close();
				return list.ToArray();
			}
			catch (Exception)
			{
				return null;
			}
		}

		public bool DownloadFile(string ftpDir, string localDir, string fileName)
		{
			try
			{
				byte[] array = DownloadFile(ftpDir, fileName);
				if (array.Length != 0)
				{
					FileStream fileStream = new FileStream(FolderFormat(localDir, '\\') + fileName, FileMode.Create);
					int count = array.Length;
					fileStream.Write(array, 0, count);
					fileStream.Close();
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public List<DirItemInfo> GetFtpFileInfos(string ftpPath)
		{
			try
			{
				if (ftpUserName == null && ftpPassword == null)
				{
					return null;
				}
				List<DirItemInfo> list = new List<DirItemInfo>();
				FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath));
				ftpWebRequest.Method = "LIST";
				ftpWebRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
				ftpWebRequest.Proxy = null;
				WebResponse response = ftpWebRequest.GetResponse();
				StreamReader streamReader = new StreamReader(response.GetResponseStream(), IsRtosFtp ? Encoding.Default : Encoding.UTF8);
				StringBuilder stringBuilder = new StringBuilder();
				for (string text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
				{
					DirItemInfo dirItemInfo = new DirItemInfo();
					char[] separator = new char[1] { ' ' };
					string[] array = ((!IsRtosFtp) ? text.Split(separator) : text.Split(separator, 9));
					if (array.Length != 0)
					{
						dirItemInfo.Name = array[array.Length - 1].Trim();
						if (array[0].StartsWith("d"))
						{
							dirItemInfo.FileType = 0;
						}
						else
						{
							dirItemInfo.FileType = 1;
						}
						int result;
						if (int.TryParse(array[4], out result))
						{
							dirItemInfo.Size = result;
						}
						else
						{
							dirItemInfo.Size = 0;
						}
						dirItemInfo.ParentFolder = FolderFormat(ftpPath, '/');
						dirItemInfo.FullFileName = dirItemInfo.ParentFolder + dirItemInfo.Name;
						if (!array[0].StartsWith("d") || (!(dirItemInfo.Name == ".") && !(dirItemInfo.Name == "..")))
						{
							list.Add(dirItemInfo);
						}
					}
				}
				streamReader.Close();
				response.Close();
				return list;
			}
			catch (Exception ex)
			{
				ex.ToString();
			}
			return null;
		}

		public List<string> GetFtpFileList(string ftpPath, string type)
		{
			try
			{
				List<string> list = new List<string>();
				FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath));
				ftpWebRequest.Method = type;
				ftpWebRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
				WebResponse response = ftpWebRequest.GetResponse();
				StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
				StringBuilder stringBuilder = new StringBuilder();
				for (string text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
				{
					list.Add(text);
				}
				streamReader.Close();
				response.Close();
				return list;
			}
			catch (Exception ex)
			{
				ex.ToString();
			}
			return null;
		}

		public bool MakeDir(string ftpPath, string dirName)
		{
			try
			{
				FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(FolderFormat(ftpPath, '/') + dirName));
				ftpWebRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
				ftpWebRequest.Method = "MKD";
				ftpWebRequest.Proxy = null;
				FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
				ftpWebResponse.Close();
				return true;
			}
			catch (Exception)
			{
			}
			return false;
		}

		public bool DirExist(string ftpPath, string dirName)
		{
			bool result = false;
			try
			{
				FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath));
				ftpWebRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
				ftpWebRequest.Method = "LIST";
				ftpWebRequest.Proxy = null;
				FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
				StreamReader streamReader = new StreamReader(ftpWebResponse.GetResponseStream(), Encoding.Default);
				StringBuilder stringBuilder = new StringBuilder();
				for (string text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
				{
					stringBuilder.Append(text);
					stringBuilder.Append("|");
				}
				string[] array = stringBuilder.ToString().Split('|');
				for (int i = 0; i < array.Length; i++)
				{
					if (!array[i].StartsWith("d"))
					{
						continue;
					}
					string[] array2 = array[i].Split(' ');
					if (array2.Length != 0)
					{
						string text2 = array2[array2.Length - 1].Trim();
						if (text2 == dirName)
						{
							result = true;
							break;
						}
					}
				}
				streamReader.Close();
				streamReader.Dispose();
				ftpWebResponse.Close();
			}
			catch (Exception)
			{
			}
			return result;
		}

		public bool DeleteDir(string ftpPath, string folderName)
		{
			string ftpPath2 = FolderFormat(ftpPath, '/') + folderName;
			List<DirItemInfo> ftpFileInfos = GetFtpFileInfos(ftpPath2);
			for (int i = 0; i < ftpFileInfos.Count; i++)
			{
				DirItemInfo dirItemInfo = ftpFileInfos[i];
				if (dirItemInfo.FileType == 0)
				{
					DeleteDir(ftpPath2, dirItemInfo.Name);
				}
				else if (dirItemInfo.FileType == 1)
				{
					DeleteFile(ftpPath2, dirItemInfo.Name);
				}
			}
			DeleteFolder(ftpPath, folderName);
			return true;
		}

		public string DeleteFile(string ftpPath, string fileName)
		{
			try
			{
				FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(FolderFormat(ftpPath, '/') + fileName));
				ftpWebRequest.UseBinary = true;
				ftpWebRequest.UsePassive = true;
				ftpWebRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
				ftpWebRequest.Method = "DELE";
				FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
				ftpWebResponse.Close();
				return "0";
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}
		}

		public bool RenameFile(string ftpPath, string fileName, string newname)
		{
			try
			{
				FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(FolderFormat(ftpPath, '/') + fileName));
				ftpWebRequest.UseBinary = true;
				ftpWebRequest.UsePassive = true;
				ftpWebRequest.RenameTo = newname;
				ftpWebRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
				ftpWebRequest.Method = "RENAME";
				FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
				ftpWebResponse.Close();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private string DeleteFolder(string ftpPath, string dirName)
		{
			try
			{
				FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(FolderFormat(ftpPath, '/') + dirName));
				ftpWebRequest.UseBinary = true;
				ftpWebRequest.UsePassive = true;
				ftpWebRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
				ftpWebRequest.Method = "RMD";
				ftpWebRequest.Proxy = null;
				FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
				ftpWebResponse.Close();
				return "0";
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}
		}

		public bool DisconnectFtp(string ftpPath)
		{
			try
			{
				if (ftpUserName == null && ftpPassword == null)
				{
					return false;
				}
				FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(FolderFormat(ftpPath, '/')));
				ftpWebRequest.UseBinary = true;
				ftpWebRequest.UsePassive = true;
				ftpWebRequest.KeepAlive = false;
				ftpWebRequest.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
				ftpWebRequest.Method = "PWD";
				ftpWebRequest.Proxy = null;
				FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
				ftpWebResponse.Close();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static string FolderFormat(string folderPath, char fromatChar)
		{
			int num = -1;
			num = folderPath.LastIndexOf(fromatChar);
			if (num != folderPath.Length - 1)
			{
				folderPath += fromatChar;
			}
			return folderPath;
		}

		private List<List<string>> GetLocalDirDetails(string localDir)
		{
			List<List<string>> list = new List<List<string>>();
			try
			{
				list.Add(Directory.GetFiles(localDir).ToList());
				list.Add(Directory.GetDirectories(localDir).ToList());
				for (int i = 0; i < list[0].Count; i++)
				{
					int num = list[0][i].LastIndexOf("\\");
					list[0][i] = list[0][i].Substring(num + 1);
				}
				for (int j = 0; j < list[1].Count; j++)
				{
					int num2 = list[1][j].LastIndexOf("\\");
					list[1][j] = list[1][j].Substring(num2 + 1);
				}
			}
			catch (Exception ex)
			{
				ex.ToString();
			}
			return list;
		}

		public bool FtpConnectCheck()
		{
			if (ftpUserName == null)
			{
				return false;
			}
			if (ftpPassword == null)
			{
				return false;
			}
			return true;
		}
	}
}
