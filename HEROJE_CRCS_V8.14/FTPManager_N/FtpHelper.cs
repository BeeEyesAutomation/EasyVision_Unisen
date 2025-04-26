using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace FTPManager_N
{
	internal class FtpHelper
	{
		public static FtpFileInfo[] GetFtpFileInfos(string ftpPath, string userName, string passWord)
		{
			LinkedList<FtpFileInfo> linkedList = new LinkedList<FtpFileInfo>();
			FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath));
			ftpWebRequest.UsePassive = false;
			ftpWebRequest.UseBinary = true;
			ftpWebRequest.Credentials = new NetworkCredential(userName, passWord);
			ftpWebRequest.Method = "LIST";
			ftpWebRequest.Proxy = null;
			FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
			StreamReader streamReader = new StreamReader(ftpWebResponse.GetResponseStream(), Encoding.UTF8);
			for (string text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
			{
				linkedList.AddLast(new FtpFileInfo(text));
			}
			streamReader.Close();
			ftpWebResponse.Close();
			return linkedList.ToArray();
		}
	}
}
