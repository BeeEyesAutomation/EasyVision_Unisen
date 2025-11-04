using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using Newtonsoft.Json.Linq;

namespace HslCommunication.Core.Net;

public class NetworkXBase : NetworkBase
{
	protected Socket CoreSocket = null;

	protected OperateResult SendFileStreamToSocket(Socket socket, string filename, long filelength, Action<long, long> report = null)
	{
		try
		{
			OperateResult result = new OperateResult();
			using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				result = SendStreamToSocket(socket, stream, filelength, report, reportByPercent: true);
			}
			return result;
		}
		catch (Exception ex)
		{
			socket?.Close();
			base.LogNet?.WriteException(ToString(), ex);
			return new OperateResult(ex.Message);
		}
	}

	protected OperateResult SendFileAndCheckReceive(Socket socket, string filename, string servername, string filetag, string fileupload, Action<long, long> sendReport = null)
	{
		FileInfo fileInfo = new FileInfo(filename);
		if (!File.Exists(filename))
		{
			OperateResult operateResult = SendStringAndCheckReceive(socket, 0, "");
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
			socket?.Close();
			return new OperateResult(StringResources.Language.FileNotExist);
		}
		JObject jObject = new JObject
		{
			{
				"FileName",
				new JValue(servername)
			},
			{
				"FileSize",
				new JValue(fileInfo.Length)
			},
			{
				"FileTag",
				new JValue(filetag)
			},
			{
				"FileUpload",
				new JValue(fileupload)
			}
		};
		OperateResult operateResult2 = SendStringAndCheckReceive(socket, 1, jObject.ToString());
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return SendFileStreamToSocket(socket, filename, fileInfo.Length, sendReport);
	}

	protected OperateResult SendFileAndCheckReceive(Socket socket, Stream stream, string servername, string filetag, string fileupload, Action<long, long> sendReport = null)
	{
		JObject jObject = new JObject
		{
			{
				"FileName",
				new JValue(servername)
			},
			{
				"FileSize",
				new JValue(stream.Length)
			},
			{
				"FileTag",
				new JValue(filetag)
			},
			{
				"FileUpload",
				new JValue(fileupload)
			}
		};
		OperateResult operateResult = SendStringAndCheckReceive(socket, 1, jObject.ToString());
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return SendStreamToSocket(socket, stream, stream.Length, sendReport, reportByPercent: true);
	}

	protected OperateResult<FileBaseInfo> ReceiveFileHeadFromSocket(Socket socket)
	{
		OperateResult<int, string> operateResult = ReceiveStringContentFromSocket(socket);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FileBaseInfo>(operateResult);
		}
		if (operateResult.Content1 == 0)
		{
			socket?.Close();
			base.LogNet?.WriteWarn(ToString(), StringResources.Language.FileRemoteNotExist);
			return new OperateResult<FileBaseInfo>(StringResources.Language.FileNotExist);
		}
		OperateResult<FileBaseInfo> operateResult2 = new OperateResult<FileBaseInfo>
		{
			Content = new FileBaseInfo()
		};
		try
		{
			JObject json = JObject.Parse(operateResult.Content2);
			operateResult2.Content.Name = SoftBasic.GetValueFromJsonObject(json, "FileName", "");
			operateResult2.Content.Size = SoftBasic.GetValueFromJsonObject(json, "FileSize", 0L);
			operateResult2.Content.Tag = SoftBasic.GetValueFromJsonObject(json, "FileTag", "");
			operateResult2.Content.Upload = SoftBasic.GetValueFromJsonObject(json, "FileUpload", "");
			operateResult2.IsSuccess = true;
		}
		catch (Exception ex)
		{
			socket?.Close();
			operateResult2.Message = "Extra File Head Wrong:" + ex.Message;
		}
		return operateResult2;
	}

	protected OperateResult<FileBaseInfo> ReceiveFileFromSocket(Socket socket, string savename, Action<long, long> receiveReport)
	{
		OperateResult<FileBaseInfo> operateResult = ReceiveFileHeadFromSocket(socket);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		try
		{
			OperateResult operateResult2 = null;
			using (FileStream stream = new FileStream(savename, FileMode.Create, FileAccess.Write))
			{
				operateResult2 = WriteStreamFromSocket(socket, stream, operateResult.Content.Size, receiveReport, reportByPercent: true);
			}
			if (!operateResult2.IsSuccess)
			{
				if (File.Exists(savename))
				{
					File.Delete(savename);
				}
				return OperateResult.CreateFailedResult<FileBaseInfo>(operateResult2);
			}
			return operateResult;
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), ex);
			socket?.Close();
			return new OperateResult<FileBaseInfo>
			{
				Message = ex.Message
			};
		}
	}

	protected OperateResult<FileBaseInfo> ReceiveFileFromSocket(Socket socket, Stream stream, Action<long, long> receiveReport)
	{
		OperateResult<FileBaseInfo> operateResult = ReceiveFileHeadFromSocket(socket);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		try
		{
			WriteStreamFromSocket(socket, stream, operateResult.Content.Size, receiveReport, reportByPercent: true);
			return operateResult;
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), ex);
			socket?.Close();
			return new OperateResult<FileBaseInfo>
			{
				Message = ex.Message
			};
		}
	}

	protected async Task<OperateResult> SendFileStreamToSocketAsync(Socket socket, string filename, long filelength, Action<long, long> report = null)
	{
		try
		{
			OperateResult result = new OperateResult();
			using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				result = await SendStreamToSocketAsync(socket, fs, filelength, report, reportByPercent: true);
			}
			return result;
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception ex3 = ex2;
			socket?.Close();
			base.LogNet?.WriteException(ToString(), ex3);
			return new OperateResult(ex3.Message);
		}
	}

	protected async Task<OperateResult> SendFileAndCheckReceiveAsync(Socket socket, string filename, string servername, string filetag, string fileupload, Action<long, long> sendReport = null)
	{
		FileInfo info = new FileInfo(filename);
		if (!File.Exists(filename))
		{
			OperateResult stringResult = await SendStringAndCheckReceiveAsync(socket, 0, "");
			if (!stringResult.IsSuccess)
			{
				return stringResult;
			}
			socket?.Close();
			return new OperateResult(StringResources.Language.FileNotExist);
		}
		JObject json = new JObject
		{
			{
				"FileName",
				new JValue(servername)
			},
			{
				"FileSize",
				new JValue(info.Length)
			},
			{
				"FileTag",
				new JValue(filetag)
			},
			{
				"FileUpload",
				new JValue(fileupload)
			}
		};
		OperateResult sendResult = await SendStringAndCheckReceiveAsync(socket, 1, json.ToString());
		if (!sendResult.IsSuccess)
		{
			return sendResult;
		}
		return await SendFileStreamToSocketAsync(socket, filename, info.Length, sendReport);
	}

	protected async Task<OperateResult> SendFileAndCheckReceiveAsync(Socket socket, Stream stream, string servername, string filetag, string fileupload, Action<long, long> sendReport = null)
	{
		JObject json = new JObject
		{
			{
				"FileName",
				new JValue(servername)
			},
			{
				"FileSize",
				new JValue(stream.Length)
			},
			{
				"FileTag",
				new JValue(filetag)
			},
			{
				"FileUpload",
				new JValue(fileupload)
			}
		};
		OperateResult fileResult = await SendStringAndCheckReceiveAsync(socket, 1, json.ToString());
		if (!fileResult.IsSuccess)
		{
			return fileResult;
		}
		return await SendStreamToSocketAsync(socket, stream, stream.Length, sendReport, reportByPercent: true);
	}

	protected async Task<OperateResult<FileBaseInfo>> ReceiveFileHeadFromSocketAsync(Socket socket)
	{
		OperateResult<int, string> receiveString = await ReceiveStringContentFromSocketAsync(socket);
		if (!receiveString.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FileBaseInfo>(receiveString);
		}
		if (receiveString.Content1 == 0)
		{
			socket?.Close();
			base.LogNet?.WriteWarn(ToString(), StringResources.Language.FileRemoteNotExist);
			return new OperateResult<FileBaseInfo>(StringResources.Language.FileNotExist);
		}
		OperateResult<FileBaseInfo> result = new OperateResult<FileBaseInfo>
		{
			Content = new FileBaseInfo()
		};
		try
		{
			JObject json = JObject.Parse(receiveString.Content2);
			result.Content.Name = SoftBasic.GetValueFromJsonObject(json, "FileName", "");
			result.Content.Size = SoftBasic.GetValueFromJsonObject(json, "FileSize", 0L);
			result.Content.Tag = SoftBasic.GetValueFromJsonObject(json, "FileTag", "");
			result.Content.Upload = SoftBasic.GetValueFromJsonObject(json, "FileUpload", "");
			result.IsSuccess = true;
		}
		catch (Exception ex)
		{
			socket?.Close();
			result.Message = "Extra File Head Wrong:" + ex.Message;
		}
		return result;
	}

	protected async Task<OperateResult<FileBaseInfo>> ReceiveFileFromSocketAsync(Socket socket, string savename, Action<long, long> receiveReport)
	{
		OperateResult<FileBaseInfo> fileResult = await ReceiveFileHeadFromSocketAsync(socket);
		if (!fileResult.IsSuccess)
		{
			return fileResult;
		}
		try
		{
			OperateResult write = null;
			using (FileStream fs = new FileStream(savename, FileMode.Create, FileAccess.Write))
			{
				write = await WriteStreamFromSocketAsync(socket, fs, fileResult.Content.Size, receiveReport, reportByPercent: true);
			}
			if (!write.IsSuccess)
			{
				if (File.Exists(savename))
				{
					File.Delete(savename);
				}
				return OperateResult.CreateFailedResult<FileBaseInfo>(write);
			}
			return fileResult;
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), ex);
			socket?.Close();
			return new OperateResult<FileBaseInfo>(ex.Message);
		}
	}

	protected async Task<OperateResult<FileBaseInfo>> ReceiveFileFromSocketAsync(Socket socket, Stream stream, Action<long, long> receiveReport)
	{
		OperateResult<FileBaseInfo> fileResult = await ReceiveFileHeadFromSocketAsync(socket);
		if (!fileResult.IsSuccess)
		{
			return fileResult;
		}
		try
		{
			await WriteStreamFromSocketAsync(socket, stream, fileResult.Content.Size, receiveReport, reportByPercent: true);
			return fileResult;
		}
		catch (Exception ex)
		{
			base.LogNet?.WriteException(ToString(), ex);
			socket?.Close();
			return new OperateResult<FileBaseInfo>(ex.Message);
		}
	}

	public override string ToString()
	{
		return "NetworkXBase";
	}
}
