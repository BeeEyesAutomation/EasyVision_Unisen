using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Reflection;
using Newtonsoft.Json.Linq;

namespace HslCommunication.Enthernet;

public class IntegrationFileClient : FileClientBase
{
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

	public IntegrationFileClient()
	{
	}

	public IntegrationFileClient(string ipAddress, int port)
	{
		base.ServerIpEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
	}

	[HslMqttApi(ApiTopic = "DeleteFileFactoryGroupId", Description = "Delete the file operation of the server, you need to specify the file name and the three-level classification information of the file")]
	public OperateResult DeleteFile(string fileName, string factory, string group, string id)
	{
		return DeleteFileBase(fileName, factory, group, id);
	}

	[HslMqttApi(Description = "Delete the file operation of the server, the classification of the file is empty here")]
	public OperateResult DeleteFile(string fileName)
	{
		return DeleteFileBase(fileName, "", "", "");
	}

	[HslMqttApi(ApiTopic = "DeleteFilesFactoryGroupId", Description = "Delete the file operation of the server, you need to specify the file names and the three-level classification information of the file")]
	public OperateResult DeleteFile(string[] fileNames, string factory, string group, string id)
	{
		return DeleteFileBase(fileNames, factory, group, id);
	}

	[HslMqttApi(Description = "Delete all file operations of the server folder, the three-level classification information of the file")]
	public OperateResult DeleteFolderFiles(string factory, string group, string id)
	{
		return DeleteFolderBase(factory, group, id);
	}

	[HslMqttApi(Description = "To delete all empty sub-file directories of the server's folder, you need to pass in the three-level classification information of the file")]
	public OperateResult DeleteEmptyFolders(string factory, string group, string id)
	{
		return DeleteEmptyFoldersBase(factory, group, id);
	}

	[HslMqttApi(Description = "Get the file statistics of the specified directory of the server folder, including the number of files, the total size, and the last update time")]
	public OperateResult<GroupFileInfo> GetGroupFileInfo(string factory, string group, string id)
	{
		OperateResult<Socket> operateResult = CreateSocketAndConnect(base.ServerIpEndPoint, base.ConnectTimeOut);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<GroupFileInfo>(operateResult);
		}
		OperateResult operateResult2 = SendStringAndCheckReceive(operateResult.Content, 2016, "");
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<GroupFileInfo>(operateResult2);
		}
		OperateResult operateResult3 = SendFactoryGroupId(operateResult.Content, factory, group, id);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<GroupFileInfo>(operateResult3);
		}
		OperateResult<int, string> operateResult4 = ReceiveStringContentFromSocket(operateResult.Content);
		if (!operateResult4.IsSuccess)
		{
			return OperateResult.CreateFailedResult<GroupFileInfo>(operateResult4);
		}
		return OperateResult.CreateSuccessResult(JObject.Parse(operateResult4.Content2).ToObject<GroupFileInfo>());
	}

	[HslMqttApi(Description = "Get the file information of all subdirectories of the specified directory of the server folder, including the number of files in each subdirectory, the total size, and the last update time")]
	public OperateResult<GroupFileInfo[]> GetSubGroupFileInfos(string factory, string group, string id)
	{
		OperateResult<Socket> operateResult = CreateSocketAndConnect(base.ServerIpEndPoint, base.ConnectTimeOut);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<GroupFileInfo[]>(operateResult);
		}
		OperateResult operateResult2 = SendStringAndCheckReceive(operateResult.Content, 2016, "");
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<GroupFileInfo[]>(operateResult2);
		}
		OperateResult operateResult3 = SendFactoryGroupId(operateResult.Content, factory, group, id);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<GroupFileInfo[]>(operateResult3);
		}
		OperateResult<int, string> operateResult4 = ReceiveStringContentFromSocket(operateResult.Content);
		if (!operateResult4.IsSuccess)
		{
			return OperateResult.CreateFailedResult<GroupFileInfo[]>(operateResult4);
		}
		return OperateResult.CreateSuccessResult(JArray.Parse(operateResult4.Content2).ToObject<GroupFileInfo[]>());
	}

	public async Task<OperateResult> DeleteFileAsync(string fileName, string factory, string group, string id)
	{
		return await DeleteFileBaseAsync(fileName, factory, group, id);
	}

	public async Task<OperateResult> DeleteFileAsync(string fileName)
	{
		return await DeleteFileBaseAsync(fileName, "", "", "");
	}

	public async Task<OperateResult> DeleteFileAsync(string[] fileNames, string factory, string group, string id)
	{
		return await DeleteFileBaseAsync(fileNames, factory, group, id);
	}

	public async Task<OperateResult> DeleteFolderFilesAsync(string factory, string group, string id)
	{
		return await DeleteFolderBaseAsync(factory, group, id);
	}

	public async Task<OperateResult> DeleteEmptyFoldersAsync(string factory, string group, string id)
	{
		return await DeleteEmptyFoldersBaseAsync(factory, group, id);
	}

	public async Task<OperateResult<GroupFileInfo>> GetGroupFileInfoAsync(string factory, string group, string id)
	{
		OperateResult<Socket> socketResult = await CreateSocketAndConnectAsync(base.ServerIpEndPoint, base.ConnectTimeOut);
		if (!socketResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<GroupFileInfo>(socketResult);
		}
		OperateResult sendString = await SendStringAndCheckReceiveAsync(socketResult.Content, 2015, "");
		if (!sendString.IsSuccess)
		{
			return OperateResult.CreateFailedResult<GroupFileInfo>(sendString);
		}
		OperateResult sendFileInfo = await SendFactoryGroupIdAsync(socketResult.Content, factory, group, id);
		if (!sendFileInfo.IsSuccess)
		{
			return OperateResult.CreateFailedResult<GroupFileInfo>(sendFileInfo);
		}
		OperateResult<int, string> receiveBack = await ReceiveStringContentFromSocketAsync(socketResult.Content);
		if (!receiveBack.IsSuccess)
		{
			return OperateResult.CreateFailedResult<GroupFileInfo>(receiveBack);
		}
		return OperateResult.CreateSuccessResult(JObject.Parse(receiveBack.Content2).ToObject<GroupFileInfo>());
	}

	public async Task<OperateResult<GroupFileInfo[]>> GetSubGroupFileInfosAsync(string factory, string group, string id)
	{
		OperateResult<Socket> socketResult = await CreateSocketAndConnectAsync(base.ServerIpEndPoint, base.ConnectTimeOut);
		if (!socketResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<GroupFileInfo[]>(socketResult);
		}
		OperateResult sendString = await SendStringAndCheckReceiveAsync(socketResult.Content, 2016, "");
		if (!sendString.IsSuccess)
		{
			return OperateResult.CreateFailedResult<GroupFileInfo[]>(sendString);
		}
		OperateResult sendFileInfo = await SendFactoryGroupIdAsync(socketResult.Content, factory, group, id);
		if (!sendFileInfo.IsSuccess)
		{
			return OperateResult.CreateFailedResult<GroupFileInfo[]>(sendFileInfo);
		}
		OperateResult<int, string> receiveBack = await ReceiveStringContentFromSocketAsync(socketResult.Content);
		if (!receiveBack.IsSuccess)
		{
			return OperateResult.CreateFailedResult<GroupFileInfo[]>(receiveBack);
		}
		return OperateResult.CreateSuccessResult(JArray.Parse(receiveBack.Content2).ToObject<GroupFileInfo[]>());
	}

	[HslMqttApi(Description = "To download a file from the server to a local file, you need to specify the name of the downloaded file and three-level classification information")]
	public OperateResult DownloadFile(string fileName, string factory, string group, string id, Action<long, long> processReport, string fileSaveName)
	{
		return DownloadFileBase(factory, group, id, fileName, processReport, fileSaveName);
	}

	public OperateResult DownloadFile(string fileName, string factory, string group, string id, Action<long, long> processReport, Stream stream)
	{
		return DownloadFileBase(factory, group, id, fileName, processReport, stream);
	}

	public async Task<OperateResult> DownloadFileAsync(string fileName, string factory, string group, string id, Action<long, long> processReport, string fileSaveName)
	{
		return await DownloadFileBaseAsync(factory, group, id, fileName, processReport, fileSaveName);
	}

	public async Task<OperateResult> DownloadFileAsync(string fileName, string factory, string group, string id, Action<long, long> processReport, Stream stream)
	{
		return await DownloadFileBaseAsync(factory, group, id, fileName, processReport, stream);
	}

	[HslMqttApi(Description = "Upload a local file to the server. If the file already exists, update the file.")]
	public OperateResult UploadFile(string fileName, string serverName, string factory, string group, string id, string fileTag, string fileUpload, Action<long, long> processReport)
	{
		if (!File.Exists(fileName))
		{
			return new OperateResult(StringResources.Language.FileNotExist);
		}
		return UploadFileBase(fileName, serverName, factory, group, id, fileTag, fileUpload, processReport);
	}

	public OperateResult UploadFile(string fileName, string factory, string group, string id, string fileTag, string fileUpload, Action<long, long> processReport)
	{
		if (!File.Exists(fileName))
		{
			return new OperateResult(StringResources.Language.FileNotExist);
		}
		FileInfo fileInfo = new FileInfo(fileName);
		return UploadFileBase(fileName, fileInfo.Name, factory, group, id, fileTag, fileUpload, processReport);
	}

	public OperateResult UploadFile(string fileName, string factory, string group, string id, Action<long, long> processReport)
	{
		if (!File.Exists(fileName))
		{
			return new OperateResult(StringResources.Language.FileNotExist);
		}
		FileInfo fileInfo = new FileInfo(fileName);
		return UploadFileBase(fileName, fileInfo.Name, factory, group, id, "", "", processReport);
	}

	public OperateResult UploadFile(string fileName, Action<long, long> processReport)
	{
		if (!File.Exists(fileName))
		{
			return new OperateResult(StringResources.Language.FileNotExist);
		}
		FileInfo fileInfo = new FileInfo(fileName);
		return UploadFileBase(fileName, fileInfo.Name, "", "", "", "", "", processReport);
	}

	public OperateResult UploadFile(Stream stream, string serverName, string factory, string group, string id, string fileTag, string fileUpload, Action<long, long> processReport)
	{
		return UploadFileBase(stream, serverName, factory, group, id, fileTag, fileUpload, processReport);
	}

	public async Task<OperateResult> UploadFileAsync(string fileName, string serverName, string factory, string group, string id, string fileTag, string fileUpload, Action<long, long> processReport)
	{
		if (!File.Exists(fileName))
		{
			return new OperateResult(StringResources.Language.FileNotExist);
		}
		return await UploadFileBaseAsync(fileName, serverName, factory, group, id, fileTag, fileUpload, processReport);
	}

	public async Task<OperateResult> UploadFileAsync(string fileName, string factory, string group, string id, string fileTag, string fileUpload, Action<long, long> processReport)
	{
		if (!File.Exists(fileName))
		{
			return new OperateResult(StringResources.Language.FileNotExist);
		}
		FileInfo fileInfo = new FileInfo(fileName);
		return await UploadFileBaseAsync(fileName, fileInfo.Name, factory, group, id, fileTag, fileUpload, processReport);
	}

	public async Task<OperateResult> UploadFileAsync(string fileName, string factory, string group, string id, Action<long, long> processReport)
	{
		if (!File.Exists(fileName))
		{
			return new OperateResult(StringResources.Language.FileNotExist);
		}
		FileInfo fileInfo = new FileInfo(fileName);
		return await UploadFileBaseAsync(fileName, fileInfo.Name, factory, group, id, "", "", processReport);
	}

	public async Task<OperateResult> UploadFileAsync(string fileName, Action<long, long> processReport)
	{
		if (!File.Exists(fileName))
		{
			return new OperateResult(StringResources.Language.FileNotExist);
		}
		FileInfo fileInfo = new FileInfo(fileName);
		return await UploadFileBaseAsync(fileName, fileInfo.Name, "", "", "", "", "", processReport);
	}

	public async Task<OperateResult> UploadFileAsync(Stream stream, string serverName, string factory, string group, string id, string fileTag, string fileUpload, Action<long, long> processReport)
	{
		return await UploadFileBaseAsync(stream, serverName, factory, group, id, fileTag, fileUpload, processReport);
	}

	[HslMqttApi(Description = "Get all documents in the specified path")]
	public OperateResult<GroupFileItem[]> DownloadPathFileNames(string factory, string group, string id)
	{
		return DownloadStringArrays<GroupFileItem>(2007, factory, group, id);
	}

	public async Task<OperateResult<GroupFileItem[]>> DownloadPathFileNamesAsync(string factory, string group, string id)
	{
		return await DownloadStringArraysAsync<GroupFileItem>(2007, factory, group, id);
	}

	[HslMqttApi(Description = "Get all directories under the specified path")]
	public OperateResult<string[]> DownloadPathFolders(string factory, string group, string id)
	{
		return DownloadStringArrays<string>(2008, factory, group, id);
	}

	public async Task<OperateResult<string[]>> DownloadPathFoldersAsync(string factory, string group, string id)
	{
		return await DownloadStringArraysAsync<string>(2008, factory, group, id);
	}

	public OperateResult<bool> IsFileExists(string fileName, string factory, string group, string id)
	{
		OperateResult<Socket> operateResult = CreateSocketAndConnect(base.ServerIpEndPoint, base.ConnectTimeOut);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(operateResult);
		}
		OperateResult operateResult2 = SendStringAndCheckReceive(operateResult.Content, 2013, fileName);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(operateResult2);
		}
		OperateResult operateResult3 = SendFactoryGroupId(operateResult.Content, factory, group, id);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(operateResult3);
		}
		OperateResult<int, string> operateResult4 = ReceiveStringContentFromSocket(operateResult.Content);
		if (!operateResult4.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(operateResult4);
		}
		OperateResult<bool> result = OperateResult.CreateSuccessResult(operateResult4.Content1 == 1);
		operateResult.Content?.Close();
		return result;
	}

	public async Task<OperateResult<bool>> IsFileExistsAsync(string fileName, string factory, string group, string id)
	{
		OperateResult<Socket> socketResult = await CreateSocketAndConnectAsync(base.ServerIpEndPoint, base.ConnectTimeOut);
		if (!socketResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(socketResult);
		}
		OperateResult sendString = await SendStringAndCheckReceiveAsync(socketResult.Content, 2013, fileName);
		if (!sendString.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(sendString);
		}
		OperateResult sendFileInfo = await SendFactoryGroupIdAsync(socketResult.Content, factory, group, id);
		if (!sendFileInfo.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(sendFileInfo);
		}
		OperateResult<int, string> receiveBack = await ReceiveStringContentFromSocketAsync(socketResult.Content);
		if (!receiveBack.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool>(receiveBack);
		}
		OperateResult<bool> result = OperateResult.CreateSuccessResult(receiveBack.Content1 == 1);
		socketResult.Content?.Close();
		return result;
	}

	private OperateResult<T[]> DownloadStringArrays<T>(int protocol, string factory, string group, string id)
	{
		OperateResult<Socket> operateResult = CreateSocketAndConnect(base.ServerIpEndPoint, base.ConnectTimeOut);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T[]>(operateResult);
		}
		OperateResult operateResult2 = SendStringAndCheckReceive(operateResult.Content, protocol, "nosense");
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T[]>(operateResult2);
		}
		OperateResult operateResult3 = SendFactoryGroupId(operateResult.Content, factory, group, id);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T[]>(operateResult3);
		}
		OperateResult<int, string> operateResult4 = ReceiveStringContentFromSocket(operateResult.Content);
		if (!operateResult4.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T[]>(operateResult4);
		}
		operateResult.Content?.Close();
		try
		{
			return OperateResult.CreateSuccessResult(JArray.Parse(operateResult4.Content2).ToObject<T[]>());
		}
		catch (Exception ex)
		{
			return new OperateResult<T[]>(ex.Message);
		}
	}

	private async Task<OperateResult<T[]>> DownloadStringArraysAsync<T>(int protocol, string factory, string group, string id)
	{
		OperateResult<Socket> socketResult = await CreateSocketAndConnectAsync(base.ServerIpEndPoint, base.ConnectTimeOut);
		if (!socketResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T[]>(socketResult);
		}
		OperateResult send = await SendStringAndCheckReceiveAsync(socketResult.Content, protocol, "nosense");
		if (!send.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T[]>(send);
		}
		OperateResult sendClass = await SendFactoryGroupIdAsync(socketResult.Content, factory, group, id);
		if (!sendClass.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T[]>(sendClass);
		}
		OperateResult<int, string> receive = await ReceiveStringContentFromSocketAsync(socketResult.Content);
		if (!receive.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T[]>(receive);
		}
		socketResult.Content?.Close();
		try
		{
			return OperateResult.CreateSuccessResult(JArray.Parse(receive.Content2).ToObject<T[]>());
		}
		catch (Exception ex)
		{
			return new OperateResult<T[]>(ex.Message);
		}
	}

	public override string ToString()
	{
		return $"IntegrationFileClient[{base.ServerIpEndPoint}]";
	}
}
