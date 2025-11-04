using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using HslCommunication.Core.Net;

namespace HslCommunication.Enthernet;

public abstract class FileClientBase : NetworkXBase
{
	private IPEndPoint m_ipEndPoint = null;

	public IPEndPoint ServerIpEndPoint
	{
		get
		{
			return m_ipEndPoint;
		}
		set
		{
			m_ipEndPoint = value;
		}
	}

	public int ConnectTimeOut { get; set; } = 10000;

	protected OperateResult SendFactoryGroupId(Socket socket, string factory, string group, string id)
	{
		OperateResult operateResult = SendStringAndCheckReceive(socket, 1, factory);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = SendStringAndCheckReceive(socket, 2, group);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult operateResult3 = SendStringAndCheckReceive(socket, 3, id);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		return OperateResult.CreateSuccessResult();
	}

	protected async Task<OperateResult> SendFactoryGroupIdAsync(Socket socket, string factory, string group, string id)
	{
		OperateResult factoryResult = await SendStringAndCheckReceiveAsync(socket, 1, factory);
		if (!factoryResult.IsSuccess)
		{
			return factoryResult;
		}
		OperateResult groupResult = await SendStringAndCheckReceiveAsync(socket, 2, group);
		if (!groupResult.IsSuccess)
		{
			return groupResult;
		}
		OperateResult idResult = await SendStringAndCheckReceiveAsync(socket, 3, id);
		if (!idResult.IsSuccess)
		{
			return idResult;
		}
		return OperateResult.CreateSuccessResult();
	}

	protected OperateResult DeleteFileBase(string fileName, string factory, string group, string id)
	{
		OperateResult<Socket> operateResult = CreateSocketAndConnect(ServerIpEndPoint, ConnectTimeOut);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = SendStringAndCheckReceive(operateResult.Content, 2003, fileName);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult operateResult3 = SendFactoryGroupId(operateResult.Content, factory, group, id);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		OperateResult<int, string> operateResult4 = ReceiveStringContentFromSocket(operateResult.Content);
		if (!operateResult4.IsSuccess)
		{
			return operateResult4;
		}
		OperateResult operateResult5 = new OperateResult();
		if (operateResult4.Content1 == 1)
		{
			operateResult5.IsSuccess = true;
		}
		operateResult5.Message = operateResult4.Message;
		operateResult.Content?.Close();
		return operateResult5;
	}

	protected OperateResult DeleteFileBase(string[] fileNames, string factory, string group, string id)
	{
		OperateResult<Socket> operateResult = CreateSocketAndConnect(ServerIpEndPoint, ConnectTimeOut);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = SendStringAndCheckReceive(operateResult.Content, 2011, fileNames);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult operateResult3 = SendFactoryGroupId(operateResult.Content, factory, group, id);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		OperateResult<int, string> operateResult4 = ReceiveStringContentFromSocket(operateResult.Content);
		if (!operateResult4.IsSuccess)
		{
			return operateResult4;
		}
		OperateResult operateResult5 = new OperateResult();
		if (operateResult4.Content1 == 1)
		{
			operateResult5.IsSuccess = true;
		}
		operateResult5.Message = operateResult4.Message;
		operateResult.Content?.Close();
		return operateResult5;
	}

	protected OperateResult DeleteFolderBase(string factory, string group, string id)
	{
		OperateResult<Socket> operateResult = CreateSocketAndConnect(ServerIpEndPoint, ConnectTimeOut);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = SendStringAndCheckReceive(operateResult.Content, 2012, "");
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult operateResult3 = SendFactoryGroupId(operateResult.Content, factory, group, id);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		OperateResult<int, string> operateResult4 = ReceiveStringContentFromSocket(operateResult.Content);
		if (!operateResult4.IsSuccess)
		{
			return operateResult4;
		}
		OperateResult operateResult5 = new OperateResult();
		if (operateResult4.Content1 == 1)
		{
			operateResult5.IsSuccess = true;
		}
		operateResult5.Message = operateResult4.Message;
		operateResult.Content?.Close();
		return operateResult5;
	}

	protected OperateResult DeleteEmptyFoldersBase(string factory, string group, string id)
	{
		OperateResult<Socket> operateResult = CreateSocketAndConnect(ServerIpEndPoint, ConnectTimeOut);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = SendStringAndCheckReceive(operateResult.Content, 2014, "");
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult operateResult3 = SendFactoryGroupId(operateResult.Content, factory, group, id);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		OperateResult<int, string> operateResult4 = ReceiveStringContentFromSocket(operateResult.Content);
		if (!operateResult4.IsSuccess)
		{
			return operateResult4;
		}
		OperateResult operateResult5 = new OperateResult();
		if (operateResult4.Content1 == 1)
		{
			operateResult5.IsSuccess = true;
		}
		operateResult5.Message = operateResult4.Message;
		operateResult.Content?.Close();
		return operateResult5;
	}

	protected async Task<OperateResult> DeleteFileBaseAsync(string fileName, string factory, string group, string id)
	{
		OperateResult<Socket> socketResult = await CreateSocketAndConnectAsync(ServerIpEndPoint, ConnectTimeOut);
		if (!socketResult.IsSuccess)
		{
			return socketResult;
		}
		OperateResult sendString = await SendStringAndCheckReceiveAsync(socketResult.Content, 2003, fileName);
		if (!sendString.IsSuccess)
		{
			return sendString;
		}
		OperateResult sendFileInfo = await SendFactoryGroupIdAsync(socketResult.Content, factory, group, id);
		if (!sendFileInfo.IsSuccess)
		{
			return sendFileInfo;
		}
		OperateResult<int, string> receiveBack = await ReceiveStringContentFromSocketAsync(socketResult.Content);
		if (!receiveBack.IsSuccess)
		{
			return receiveBack;
		}
		OperateResult result = new OperateResult();
		if (receiveBack.Content1 == 1)
		{
			result.IsSuccess = true;
		}
		result.Message = receiveBack.Message;
		socketResult.Content?.Close();
		return result;
	}

	protected async Task<OperateResult> DeleteFileBaseAsync(string[] fileNames, string factory, string group, string id)
	{
		OperateResult<Socket> socketResult = await CreateSocketAndConnectAsync(ServerIpEndPoint, ConnectTimeOut);
		if (!socketResult.IsSuccess)
		{
			return socketResult;
		}
		OperateResult sendString = await SendStringAndCheckReceiveAsync(socketResult.Content, 2011, fileNames);
		if (!sendString.IsSuccess)
		{
			return sendString;
		}
		OperateResult sendFileInfo = await SendFactoryGroupIdAsync(socketResult.Content, factory, group, id);
		if (!sendFileInfo.IsSuccess)
		{
			return sendFileInfo;
		}
		OperateResult<int, string> receiveBack = await ReceiveStringContentFromSocketAsync(socketResult.Content);
		if (!receiveBack.IsSuccess)
		{
			return receiveBack;
		}
		OperateResult result = new OperateResult();
		if (receiveBack.Content1 == 1)
		{
			result.IsSuccess = true;
		}
		result.Message = receiveBack.Message;
		socketResult.Content?.Close();
		return result;
	}

	protected async Task<OperateResult> DeleteFolderBaseAsync(string factory, string group, string id)
	{
		OperateResult<Socket> socketResult = await CreateSocketAndConnectAsync(ServerIpEndPoint, ConnectTimeOut);
		if (!socketResult.IsSuccess)
		{
			return socketResult;
		}
		OperateResult sendString = await SendStringAndCheckReceiveAsync(socketResult.Content, 2012, "");
		if (!sendString.IsSuccess)
		{
			return sendString;
		}
		OperateResult sendFileInfo = await SendFactoryGroupIdAsync(socketResult.Content, factory, group, id);
		if (!sendFileInfo.IsSuccess)
		{
			return sendFileInfo;
		}
		OperateResult<int, string> receiveBack = await ReceiveStringContentFromSocketAsync(socketResult.Content);
		if (!receiveBack.IsSuccess)
		{
			return receiveBack;
		}
		OperateResult result = new OperateResult();
		if (receiveBack.Content1 == 1)
		{
			result.IsSuccess = true;
		}
		result.Message = receiveBack.Message;
		socketResult.Content?.Close();
		return result;
	}

	protected async Task<OperateResult> DeleteEmptyFoldersBaseAsync(string factory, string group, string id)
	{
		OperateResult<Socket> socketResult = await CreateSocketAndConnectAsync(ServerIpEndPoint, ConnectTimeOut);
		if (!socketResult.IsSuccess)
		{
			return socketResult;
		}
		OperateResult sendString = await SendStringAndCheckReceiveAsync(socketResult.Content, 2014, "");
		if (!sendString.IsSuccess)
		{
			return sendString;
		}
		OperateResult sendFileInfo = await SendFactoryGroupIdAsync(socketResult.Content, factory, group, id);
		if (!sendFileInfo.IsSuccess)
		{
			return sendFileInfo;
		}
		OperateResult<int, string> receiveBack = await ReceiveStringContentFromSocketAsync(socketResult.Content);
		if (!receiveBack.IsSuccess)
		{
			return receiveBack;
		}
		OperateResult result = new OperateResult();
		if (receiveBack.Content1 == 1)
		{
			result.IsSuccess = true;
		}
		result.Message = receiveBack.Message;
		socketResult.Content?.Close();
		return result;
	}

	protected OperateResult DownloadFileBase(string factory, string group, string id, string fileName, Action<long, long> processReport, object source)
	{
		OperateResult<Socket> operateResult = CreateSocketAndConnect(ServerIpEndPoint, ConnectTimeOut);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = SendStringAndCheckReceive(operateResult.Content, 2001, fileName);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult operateResult3 = SendFactoryGroupId(operateResult.Content, factory, group, id);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		if (source is string savename)
		{
			OperateResult operateResult4 = ReceiveFileFromSocket(operateResult.Content, savename, processReport);
			if (!operateResult4.IsSuccess)
			{
				return operateResult4;
			}
		}
		else
		{
			if (!(source is Stream stream))
			{
				operateResult.Content?.Close();
				base.LogNet?.WriteError(ToString(), StringResources.Language.NotSupportedDataType);
				return new OperateResult(StringResources.Language.NotSupportedDataType);
			}
			OperateResult operateResult5 = ReceiveFileFromSocket(operateResult.Content, stream, processReport);
			if (!operateResult5.IsSuccess)
			{
				return operateResult5;
			}
		}
		operateResult.Content?.Close();
		return OperateResult.CreateSuccessResult();
	}

	protected async Task<OperateResult> DownloadFileBaseAsync(string factory, string group, string id, string fileName, Action<long, long> processReport, object source)
	{
		OperateResult<Socket> socketResult = await CreateSocketAndConnectAsync(ServerIpEndPoint, ConnectTimeOut);
		if (!socketResult.IsSuccess)
		{
			return socketResult;
		}
		OperateResult sendString = await SendStringAndCheckReceiveAsync(socketResult.Content, 2001, fileName);
		if (!sendString.IsSuccess)
		{
			return sendString;
		}
		OperateResult sendClass = await SendFactoryGroupIdAsync(socketResult.Content, factory, group, id);
		if (!sendClass.IsSuccess)
		{
			return sendClass;
		}
		if (source is string fileSaveName)
		{
			OperateResult result2 = await ReceiveFileFromSocketAsync(socketResult.Content, fileSaveName, processReport);
			if (!result2.IsSuccess)
			{
				return result2;
			}
		}
		else
		{
			if (!(source is Stream stream))
			{
				socketResult.Content?.Close();
				base.LogNet?.WriteError(ToString(), StringResources.Language.NotSupportedDataType);
				return new OperateResult(StringResources.Language.NotSupportedDataType);
			}
			OperateResult result3 = await ReceiveFileFromSocketAsync(socketResult.Content, stream, processReport);
			if (!result3.IsSuccess)
			{
				return result3;
			}
		}
		socketResult.Content?.Close();
		return OperateResult.CreateSuccessResult();
	}

	protected OperateResult UploadFileBase(object source, string serverName, string factory, string group, string id, string fileTag, string fileUpload, Action<long, long> processReport)
	{
		OperateResult<Socket> operateResult = CreateSocketAndConnect(ServerIpEndPoint, ConnectTimeOut);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = SendStringAndCheckReceive(operateResult.Content, 2002, serverName);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		OperateResult operateResult3 = SendFactoryGroupId(operateResult.Content, factory, group, id);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		if (source is string filename)
		{
			OperateResult operateResult4 = SendFileAndCheckReceive(operateResult.Content, filename, serverName, fileTag, fileUpload, processReport);
			if (!operateResult4.IsSuccess)
			{
				return operateResult4;
			}
		}
		else
		{
			if (!(source is Stream stream))
			{
				operateResult.Content?.Close();
				base.LogNet?.WriteError(ToString(), StringResources.Language.DataSourceFormatError);
				return new OperateResult(StringResources.Language.DataSourceFormatError);
			}
			OperateResult operateResult5 = SendFileAndCheckReceive(operateResult.Content, stream, serverName, fileTag, fileUpload, processReport);
			if (!operateResult5.IsSuccess)
			{
				return operateResult5;
			}
		}
		OperateResult<int, string> operateResult6 = ReceiveStringContentFromSocket(operateResult.Content);
		if (!operateResult6.IsSuccess)
		{
			return operateResult6;
		}
		return (operateResult6.Content1 == 1) ? OperateResult.CreateSuccessResult() : new OperateResult(StringResources.Language.ServerFileCheckFailed);
	}

	protected async Task<OperateResult> UploadFileBaseAsync(object source, string serverName, string factory, string group, string id, string fileTag, string fileUpload, Action<long, long> processReport)
	{
		OperateResult<Socket> socketResult = await CreateSocketAndConnectAsync(ServerIpEndPoint, ConnectTimeOut);
		if (!socketResult.IsSuccess)
		{
			return socketResult;
		}
		OperateResult sendString = await SendStringAndCheckReceiveAsync(socketResult.Content, 2002, serverName);
		if (!sendString.IsSuccess)
		{
			return sendString;
		}
		OperateResult sendClass = await SendFactoryGroupIdAsync(socketResult.Content, factory, group, id);
		if (!sendClass.IsSuccess)
		{
			return sendClass;
		}
		if (source is string fileName)
		{
			OperateResult result2 = await SendFileAndCheckReceiveAsync(socketResult.Content, fileName, serverName, fileTag, fileUpload, processReport);
			if (!result2.IsSuccess)
			{
				return result2;
			}
		}
		else
		{
			if (!(source is Stream stream))
			{
				socketResult.Content?.Close();
				base.LogNet?.WriteError(ToString(), StringResources.Language.DataSourceFormatError);
				return new OperateResult(StringResources.Language.DataSourceFormatError);
			}
			OperateResult result3 = await SendFileAndCheckReceiveAsync(socketResult.Content, stream, serverName, fileTag, fileUpload, processReport);
			if (!result3.IsSuccess)
			{
				return result3;
			}
		}
		OperateResult<int, string> resultCheck = await ReceiveStringContentFromSocketAsync(socketResult.Content);
		if (!resultCheck.IsSuccess)
		{
			return resultCheck;
		}
		if (resultCheck.Content1 == 1)
		{
			return OperateResult.CreateSuccessResult();
		}
		return new OperateResult(StringResources.Language.ServerFileCheckFailed);
	}

	public override string ToString()
	{
		return $"FileClientBase[{m_ipEndPoint}]";
	}
}
