using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using Newtonsoft.Json.Linq;

namespace HslCommunication.Enthernet;

public class AdvancedFileServer : NetworkFileServerBase
{
	private string m_FilesDirectoryPathTemp = null;

	public string FilesDirectoryPathTemp
	{
		get
		{
			return m_FilesDirectoryPathTemp;
		}
		set
		{
			m_FilesDirectoryPathTemp = value;
		}
	}

	protected override async void ThreadPoolLogin(Socket socket, IPEndPoint endPoint)
	{
		new OperateResult();
		string IpAddress = endPoint.Address.ToString();
		OperateResult<FileGroupInfo> infoResult = await ReceiveInformationHeadAsync(socket);
		if (!infoResult.IsSuccess)
		{
			return;
		}
		int customer = infoResult.Content.Command;
		string Factory = infoResult.Content.Factory;
		string Group = infoResult.Content.Group;
		string Identify = infoResult.Content.Identify;
		string fileName = infoResult.Content.FileName;
		string relativeName = GetRelativeFileName(Factory, Group, Identify, fileName);
		switch (customer)
		{
		case 2001:
		{
			string fullFileName7 = ReturnAbsoluteFileName(Factory, Group, Identify, fileName);
			OperateResult sendFile = await SendFileAndCheckReceiveAsync(socket, fullFileName7, fileName, "", "");
			if (!sendFile.IsSuccess)
			{
				base.LogNet?.WriteError(ToString(), StringResources.Language.FileDownloadFailed + ":" + relativeName + " ip:" + IpAddress + " reason：" + sendFile.Message);
			}
			else
			{
				socket?.Close();
				base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileDownloadSuccess + ":" + relativeName);
			}
			break;
		}
		case 2002:
		{
			string tempFileName = Path.Combine(FilesDirectoryPathTemp, CreateRandomFileName());
			string fullFileName9 = ReturnAbsoluteFileName(Factory, Group, Identify, fileName);
			CheckFolderAndCreate();
			try
			{
				FileInfo info3 = new FileInfo(fullFileName9);
				if (!Directory.Exists(info3.DirectoryName))
				{
					Directory.CreateDirectory(info3.DirectoryName);
				}
			}
			catch (Exception ex)
			{
				Exception ex6 = ex;
				Exception ex7 = ex6;
				base.LogNet?.WriteException(ToString(), StringResources.Language.FilePathCreateFailed + fullFileName9, ex7);
				socket?.Close();
				break;
			}
			OperateResult<FileBaseInfo> receiveFile = await ReceiveFileFromSocketAndMoveFileAsync(socket, tempFileName, fullFileName9);
			if (receiveFile.IsSuccess)
			{
				socket?.Close();
				OnFileUpload(new FileServerInfo
				{
					ActualFileFullName = fullFileName9,
					Name = receiveFile.Content.Name,
					Size = receiveFile.Content.Size,
					Tag = receiveFile.Content.Tag,
					Upload = receiveFile.Content.Upload
				});
				base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileUploadSuccess + ":" + relativeName);
			}
			else
			{
				base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileUploadFailed + ":" + relativeName + " " + StringResources.Language.TextDescription + receiveFile.Message);
			}
			break;
		}
		case 2003:
		{
			string fullFileName8 = ReturnAbsoluteFileName(Factory, Group, Identify, fileName);
			bool deleteResult5 = DeleteFileByName(fullFileName8);
			if ((await SendStringAndCheckReceiveAsync(socket, deleteResult5 ? 1 : 0, deleteResult5 ? StringResources.Language.FileDeleteSuccess : StringResources.Language.FileDeleteFailed)).IsSuccess)
			{
				socket?.Close();
			}
			if (deleteResult5)
			{
				base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileDeleteSuccess + ":" + relativeName);
			}
			break;
		}
		case 2011:
		{
			bool deleteResult3 = true;
			string[] fileNames2 = infoResult.Content.FileNames;
			string[] array = fileNames2;
			foreach (string item in array)
			{
				string fullFileName5 = ReturnAbsoluteFileName(Factory, Group, Identify, item);
				deleteResult3 = DeleteFileByName(fullFileName5);
				if (deleteResult3)
				{
					base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileDeleteSuccess + ":" + relativeName);
					continue;
				}
				deleteResult3 = false;
				break;
			}
			if ((await SendStringAndCheckReceiveAsync(socket, deleteResult3 ? 1 : 0, deleteResult3 ? StringResources.Language.FileDeleteSuccess : StringResources.Language.FileDeleteFailed)).IsSuccess)
			{
				socket?.Close();
			}
			break;
		}
		case 2012:
		{
			string fullPath2 = ReturnAbsoluteFileName(Factory, Group, Identify, string.Empty);
			DirectoryInfo info4 = new DirectoryInfo(fullPath2);
			bool deleteResult6 = false;
			try
			{
				if (info4.Exists)
				{
					info4.Delete(recursive: true);
				}
				deleteResult6 = true;
			}
			catch (Exception ex)
			{
				Exception ex8 = ex;
				Exception ex9 = ex8;
				base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileDeleteFailed + " [" + fullPath2 + "] " + ex9.Message);
			}
			if ((await SendStringAndCheckReceiveAsync(socket, deleteResult6 ? 1 : 0, deleteResult6 ? StringResources.Language.FileDeleteSuccess : StringResources.Language.FileDeleteFailed)).IsSuccess)
			{
				socket?.Close();
			}
			if (deleteResult6)
			{
				base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileDeleteSuccess + ":" + fullPath2);
			}
			break;
		}
		case 2014:
		{
			string fullPath = ReturnAbsoluteFileName(Factory, Group, Identify, string.Empty);
			DirectoryInfo info2 = new DirectoryInfo(fullPath);
			bool deleteResult4 = false;
			try
			{
				DirectoryInfo[] directories = info2.GetDirectories();
				DirectoryInfo[] array3 = directories;
				foreach (DirectoryInfo path in array3)
				{
					FileInfo[] files = path.GetFiles();
					if (files != null && files.Length == 0 && path.Exists)
					{
						path.Delete(recursive: true);
					}
				}
				deleteResult4 = true;
			}
			catch (Exception ex)
			{
				Exception ex4 = ex;
				Exception ex5 = ex4;
				base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileDeleteFailed + " [" + fullPath + "] " + ex5.Message);
			}
			if ((await SendStringAndCheckReceiveAsync(socket, deleteResult4 ? 1 : 0, deleteResult4 ? StringResources.Language.FileDeleteSuccess : StringResources.Language.FileDeleteFailed)).IsSuccess)
			{
				socket?.Close();
			}
			if (deleteResult4)
			{
				base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileDeleteSuccess + ":" + fullPath);
			}
			break;
		}
		case 2007:
		{
			List<GroupFileItem> fileNames3 = new List<GroupFileItem>();
			string[] directoryFiles = GetDirectoryFiles(Factory, Group, Identify);
			string[] array2 = directoryFiles;
			foreach (string j2 in array2)
			{
				FileInfo fileInfo = new FileInfo(j2);
				fileNames3.Add(new GroupFileItem
				{
					FileName = fileInfo.Name,
					FileSize = fileInfo.Length
				});
			}
			JArray jArray2 = JArray.FromObject(fileNames3.ToArray());
			if ((await SendStringAndCheckReceiveAsync(socket, 2007, jArray2.ToString())).IsSuccess)
			{
				socket?.Close();
			}
			break;
		}
		case 2008:
		{
			List<string> folders = new List<string>();
			string[] directories2 = GetDirectories(Factory, Group, Identify);
			string[] array4 = directories2;
			foreach (string i2 in array4)
			{
				DirectoryInfo directory = new DirectoryInfo(i2);
				folders.Add(directory.Name);
			}
			JArray jArray3 = JArray.FromObject(folders.ToArray());
			if ((await SendStringAndCheckReceiveAsync(socket, 2007, jArray3.ToString())).IsSuccess)
			{
				socket?.Close();
			}
			break;
		}
		case 2013:
		{
			string fullFileName6 = ReturnAbsoluteFileName(Factory, Group, Identify, fileName);
			bool isExists = File.Exists(fullFileName6);
			if ((await SendStringAndCheckReceiveAsync(socket, isExists ? 1 : 0, StringResources.Language.FileNotExist)).IsSuccess)
			{
				socket?.Close();
			}
			break;
		}
		default:
			socket?.Close();
			break;
		}
	}

	protected override void StartInitialization()
	{
		if (string.IsNullOrEmpty(FilesDirectoryPathTemp))
		{
			throw new ArgumentNullException("FilesDirectoryPathTemp", "No saved path is specified");
		}
		base.StartInitialization();
	}

	protected override void CheckFolderAndCreate()
	{
		if (!Directory.Exists(FilesDirectoryPathTemp))
		{
			Directory.CreateDirectory(FilesDirectoryPathTemp);
		}
		base.CheckFolderAndCreate();
	}

	private OperateResult<FileBaseInfo> ReceiveFileFromSocketAndMoveFile(Socket socket, string savename, string fileNameNew)
	{
		OperateResult<FileBaseInfo> operateResult = ReceiveFileFromSocket(socket, savename, null);
		if (!operateResult.IsSuccess)
		{
			DeleteFileByName(savename);
			return OperateResult.CreateFailedResult<FileBaseInfo>(operateResult);
		}
		int num = 0;
		int num2 = 0;
		while (num2 < 3)
		{
			num2++;
			if (MoveFileToNewFile(savename, fileNameNew))
			{
				num = 1;
				break;
			}
			HslHelper.ThreadSleep(500);
		}
		if (num == 0)
		{
			DeleteFileByName(savename);
		}
		OperateResult operateResult2 = SendStringAndCheckReceive(socket, num, "success");
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FileBaseInfo>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content);
	}

	private async Task<OperateResult<FileBaseInfo>> ReceiveFileFromSocketAndMoveFileAsync(Socket socket, string savename, string fileNameNew)
	{
		OperateResult<FileBaseInfo> fileInfo = await ReceiveFileFromSocketAsync(socket, savename, null);
		if (!fileInfo.IsSuccess)
		{
			DeleteFileByName(savename);
			return OperateResult.CreateFailedResult<FileBaseInfo>(fileInfo);
		}
		int customer = 0;
		int times = 0;
		while (times < 3)
		{
			times++;
			if (MoveFileToNewFile(savename, fileNameNew))
			{
				customer = 1;
				break;
			}
			HslHelper.ThreadSleep(500);
		}
		if (customer == 0)
		{
			DeleteFileByName(savename);
		}
		OperateResult sendString = await SendStringAndCheckReceiveAsync(socket, customer, "success");
		if (!sendString.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FileBaseInfo>(sendString);
		}
		return OperateResult.CreateSuccessResult(fileInfo.Content);
	}

	public override string ToString()
	{
		return $"AdvancedFileServer[{base.Port}]";
	}
}
