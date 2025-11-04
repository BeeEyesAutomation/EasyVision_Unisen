using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;

namespace HslCommunication.Core.Net;

public class NetworkFileServerBase : NetworkServerBase
{
	public delegate void FileUploadDelegate(FileServerInfo fileInfo);

	private readonly Dictionary<string, FileMarkId> dictionaryFilesMarks;

	private readonly object dictHybirdLock;

	private string m_FilesDirectoryPath = null;

	public string FilesDirectoryPath
	{
		get
		{
			return m_FilesDirectoryPath;
		}
		set
		{
			m_FilesDirectoryPath = value;
		}
	}

	public int FileMarkIdCount => dictionaryFilesMarks.Count;

	public int FileCacheSize
	{
		get
		{
			return fileCacheSize;
		}
		set
		{
			fileCacheSize = value;
		}
	}

	public event FileUploadDelegate OnFileUploadEvent;

	public NetworkFileServerBase()
	{
		dictionaryFilesMarks = new Dictionary<string, FileMarkId>(100);
		dictHybirdLock = new object();
	}

	protected FileMarkId GetFileMarksFromDictionaryWithFileName(string fileName)
	{
		FileMarkId fileMarkId;
		lock (dictHybirdLock)
		{
			if (dictionaryFilesMarks.ContainsKey(fileName))
			{
				fileMarkId = dictionaryFilesMarks[fileName];
			}
			else
			{
				fileMarkId = new FileMarkId(base.LogNet, fileName);
				dictionaryFilesMarks.Add(fileName, fileMarkId);
			}
		}
		return fileMarkId;
	}

	protected OperateResult<FileGroupInfo> ReceiveInformationHead(Socket socket)
	{
		FileGroupInfo fileGroupInfo = new FileGroupInfo();
		OperateResult<byte[], byte[]> operateResult = ReceiveAndCheckBytes(socket, 30000);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FileGroupInfo>(operateResult);
		}
		fileGroupInfo.Command = BitConverter.ToInt32(operateResult.Content1, 4);
		switch (BitConverter.ToInt32(operateResult.Content1, 0))
		{
		case 1001:
			fileGroupInfo.FileName = Encoding.Unicode.GetString(operateResult.Content2);
			break;
		case 1005:
			fileGroupInfo.FileNames = HslProtocol.UnPackStringArrayFromByte(operateResult.Content2);
			break;
		}
		OperateResult<int, string> operateResult2 = ReceiveStringContentFromSocket(socket);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FileGroupInfo>(operateResult2);
		}
		fileGroupInfo.Factory = operateResult2.Content2;
		OperateResult<int, string> operateResult3 = ReceiveStringContentFromSocket(socket);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FileGroupInfo>(operateResult3);
		}
		fileGroupInfo.Group = operateResult3.Content2;
		OperateResult<int, string> operateResult4 = ReceiveStringContentFromSocket(socket);
		if (!operateResult4.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FileGroupInfo>(operateResult4);
		}
		fileGroupInfo.Identify = operateResult4.Content2;
		return OperateResult.CreateSuccessResult(fileGroupInfo);
	}

	protected async Task<OperateResult<FileGroupInfo>> ReceiveInformationHeadAsync(Socket socket)
	{
		FileGroupInfo ret = new FileGroupInfo();
		OperateResult<byte[], byte[]> receive = await ReceiveAndCheckBytesAsync(socket, 30000);
		if (!receive.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FileGroupInfo>(receive);
		}
		ret.Command = BitConverter.ToInt32(receive.Content1, 4);
		switch (BitConverter.ToInt32(receive.Content1, 0))
		{
		case 1001:
			ret.FileName = Encoding.Unicode.GetString(receive.Content2);
			break;
		case 1005:
			ret.FileNames = HslProtocol.UnPackStringArrayFromByte(receive.Content2);
			break;
		}
		OperateResult<int, string> factoryResult = await ReceiveStringContentFromSocketAsync(socket);
		if (!factoryResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FileGroupInfo>(factoryResult);
		}
		ret.Factory = factoryResult.Content2;
		OperateResult<int, string> groupResult = await ReceiveStringContentFromSocketAsync(socket);
		if (!groupResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FileGroupInfo>(groupResult);
		}
		ret.Group = groupResult.Content2;
		OperateResult<int, string> idResult = await ReceiveStringContentFromSocketAsync(socket);
		if (!idResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FileGroupInfo>(idResult);
		}
		ret.Identify = idResult.Content2;
		return OperateResult.CreateSuccessResult(ret);
	}

	protected string CreateRandomFileName()
	{
		return SoftBasic.GetUniqueStringByGuidAndRandom();
	}

	protected string ReturnAbsoluteFilePath(string factory, string group, string id)
	{
		string text = m_FilesDirectoryPath;
		if (!string.IsNullOrEmpty(factory))
		{
			text = text + "\\" + factory;
		}
		if (!string.IsNullOrEmpty(group))
		{
			text = text + "\\" + group;
		}
		if (!string.IsNullOrEmpty(id))
		{
			text = text + "\\" + id;
		}
		return text;
	}

	protected string ReturnAbsoluteFileName(string factory, string group, string id, string fileName)
	{
		return ReturnAbsoluteFilePath(factory, group, id) + "\\" + fileName;
	}

	protected string GetRelativeFileName(string factory, string group, string id, string fileName)
	{
		string text = "";
		if (!string.IsNullOrEmpty(factory))
		{
			text = text + factory + "\\";
		}
		if (!string.IsNullOrEmpty(group))
		{
			text = text + group + "\\";
		}
		if (!string.IsNullOrEmpty(id))
		{
			text = text + id + "\\";
		}
		return text + fileName;
	}

	protected bool MoveFileToNewFile(string fileNameOld, string fileNameNew)
	{
		try
		{
			FileInfo fileInfo = new FileInfo(fileNameNew);
			if (!Directory.Exists(fileInfo.DirectoryName))
			{
				Directory.CreateDirectory(fileInfo.DirectoryName);
			}
			if (File.Exists(fileNameNew))
			{
				File.Delete(fileNameNew);
			}
			File.Move(fileNameOld, fileNameNew);
			return true;
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), "Move a file to new file failed: ", ex);
			return false;
		}
	}

	protected OperateResult DeleteFileAndCheck(Socket socket, string fullname)
	{
		int customer = 0;
		int num = 0;
		while (num < 3)
		{
			num++;
			if (DeleteFileByName(fullname))
			{
				customer = 1;
				break;
			}
			HslHelper.ThreadSleep(500);
		}
		return SendStringAndCheckReceive(socket, customer, StringResources.Language.SuccessText);
	}

	protected void OnFileUpload(FileServerInfo fileInfo)
	{
		this.OnFileUploadEvent?.Invoke(fileInfo);
	}

	protected override void StartInitialization()
	{
		if (string.IsNullOrEmpty(FilesDirectoryPath))
		{
			throw new ArgumentNullException("FilesDirectoryPath", "No saved path is specified");
		}
		CheckFolderAndCreate();
		base.StartInitialization();
	}

	protected virtual void CheckFolderAndCreate()
	{
		if (!Directory.Exists(FilesDirectoryPath))
		{
			Directory.CreateDirectory(FilesDirectoryPath);
		}
	}

	public virtual string[] GetDirectoryFiles(string factory, string group, string id)
	{
		if (string.IsNullOrEmpty(FilesDirectoryPath))
		{
			return new string[0];
		}
		string path = ReturnAbsoluteFilePath(factory, group, id);
		if (!Directory.Exists(path))
		{
			return new string[0];
		}
		return Directory.GetFiles(path);
	}

	public string[] GetDirectories(string factory, string group, string id)
	{
		if (string.IsNullOrEmpty(FilesDirectoryPath))
		{
			return new string[0];
		}
		string path = ReturnAbsoluteFilePath(factory, group, id);
		if (!Directory.Exists(path))
		{
			return new string[0];
		}
		return Directory.GetDirectories(path);
	}

	public override string ToString()
	{
		return "NetworkFileServerBase";
	}
}
