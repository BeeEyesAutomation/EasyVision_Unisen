using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.Reflection;
using Newtonsoft.Json.Linq;

namespace HslCommunication.Enthernet;

public class UltimateFileServer : NetworkFileServerBase
{
	internal Dictionary<string, GroupFileContainer> m_dictionary_group_marks = new Dictionary<string, GroupFileContainer>();

	private SimpleHybirdLock hybirdLock = new SimpleHybirdLock();

	[HslMqttApi(Description = "Get the current number of file management containers for the folder")]
	public int GroupFileContainerCount()
	{
		return m_dictionary_group_marks.Count;
	}

	public GroupFileContainer GetGroupFromFilePath(string filePath)
	{
		GroupFileContainer groupFileContainer = null;
		filePath = filePath.ToUpper();
		hybirdLock.Enter();
		if (m_dictionary_group_marks.ContainsKey(filePath))
		{
			groupFileContainer = m_dictionary_group_marks[filePath];
		}
		else
		{
			groupFileContainer = new GroupFileContainer(base.LogNet, filePath);
			m_dictionary_group_marks.Add(filePath, groupFileContainer);
		}
		hybirdLock.Leave();
		return groupFileContainer;
	}

	public void DeleteGroupFile(GroupFileContainer groupFile)
	{
		hybirdLock.Enter();
		if (m_dictionary_group_marks.ContainsKey(groupFile.DirectoryPath))
		{
			m_dictionary_group_marks.Remove(groupFile.DirectoryPath);
		}
		try
		{
			Directory.Delete(groupFile.DirectoryPath, recursive: true);
		}
		catch
		{
		}
		hybirdLock.Leave();
	}

	private OperateResult<FileBaseInfo> ReceiveFileFromSocketAndUpdateGroup(Socket socket, string savename)
	{
		FileInfo fileInfo = new FileInfo(savename);
		string text = CreateRandomFileName();
		string text2 = Path.Combine(fileInfo.DirectoryName, text);
		OperateResult<FileBaseInfo> operateResult = ReceiveFileFromSocket(socket, text2, null);
		if (!operateResult.IsSuccess)
		{
			DeleteFileByName(text2);
			return operateResult;
		}
		GroupFileContainer groupFromFilePath = GetGroupFromFilePath(fileInfo.DirectoryName);
		string fileName = groupFromFilePath.UpdateFileMappingName(fileInfo.Name, operateResult.Content.Size, text, operateResult.Content.Upload, operateResult.Content.Tag);
		DeleteExsistingFile(fileInfo.DirectoryName, fileName);
		OperateResult operateResult2 = SendStringAndCheckReceive(socket, 1, StringResources.Language.SuccessText);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FileBaseInfo>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content);
	}

	private async Task<OperateResult<FileBaseInfo>> ReceiveFileFromSocketAndUpdateGroupAsync(Socket socket, string savename)
	{
		FileInfo info = new FileInfo(savename);
		string guidName = CreateRandomFileName();
		string fileName = Path.Combine(info.DirectoryName, guidName);
		OperateResult<FileBaseInfo> receive = await ReceiveFileFromSocketAsync(socket, fileName, null);
		if (!receive.IsSuccess)
		{
			DeleteFileByName(fileName);
			return receive;
		}
		GroupFileContainer fileManagment = GetGroupFromFilePath(info.DirectoryName);
		DeleteExsistingFile(fileName: fileManagment.UpdateFileMappingName(info.Name, receive.Content.Size, guidName, receive.Content.Upload, receive.Content.Tag), path: info.DirectoryName);
		OperateResult sendBack = await SendStringAndCheckReceiveAsync(socket, 1, StringResources.Language.SuccessText);
		if (!sendBack.IsSuccess)
		{
			return OperateResult.CreateFailedResult<FileBaseInfo>(sendBack);
		}
		return OperateResult.CreateSuccessResult(receive.Content);
	}

	private string TransformFactFileName(string factory, string group, string id, string fileName)
	{
		string filePath = ReturnAbsoluteFilePath(factory, group, id);
		GroupFileContainer groupFromFilePath = GetGroupFromFilePath(filePath);
		return groupFromFilePath.GetCurrentFileMappingName(fileName);
	}

	private void DeleteExsistingFile(string path, string fileName)
	{
		DeleteExsistingFile(path, new List<string> { fileName });
	}

	private void DeleteExsistingFile(string path, List<string> fileNames)
	{
		foreach (string fileName in fileNames)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				continue;
			}
			string fileUltimatePath = Path.Combine(path, fileName);
			FileMarkId fileMarksFromDictionaryWithFileName = GetFileMarksFromDictionaryWithFileName(fileName);
			fileMarksFromDictionaryWithFileName.AddOperation(delegate
			{
				if (!DeleteFileByName(fileUltimatePath))
				{
					base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileDeleteFailed + fileUltimatePath);
				}
				else
				{
					base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileDeleteSuccess + fileUltimatePath);
				}
			});
		}
	}

	protected override async void ThreadPoolLogin(Socket socket, IPEndPoint endPoint)
	{
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
			string guidName = TransformFactFileName(Factory, Group, Identify, fileName);
			FileMarkId fileMarkId = GetFileMarksFromDictionaryWithFileName(guidName);
			fileMarkId.EnterReadOperator();
			OperateResult send = await SendFileAndCheckReceiveAsync(socket, ReturnAbsoluteFileName(Factory, Group, Identify, guidName), fileName, "", "");
			if (!send.IsSuccess)
			{
				fileMarkId.LeaveReadOperator();
				base.LogNet?.WriteError(ToString(), StringResources.Language.FileDownloadFailed + " : " + send.Message + " :" + relativeName + " ip:" + IpAddress);
			}
			else
			{
				base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileDownloadSuccess + ":" + relativeName);
				fileMarkId.LeaveReadOperator();
				socket?.Close();
			}
			break;
		}
		case 2002:
		{
			string fullFileName3 = ReturnAbsoluteFileName(Factory, Group, Identify, fileName);
			CheckFolderAndCreate();
			FileInfo info3 = new FileInfo(fullFileName3);
			try
			{
				if (!Directory.Exists(info3.DirectoryName))
				{
					Directory.CreateDirectory(info3.DirectoryName);
				}
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				Exception ex3 = ex2;
				base.LogNet?.WriteException(ToString(), StringResources.Language.FilePathCreateFailed + fullFileName3, ex3);
				socket?.Close();
				break;
			}
			OperateResult<FileBaseInfo> receive = await ReceiveFileFromSocketAndUpdateGroupAsync(socket, fullFileName3);
			if (receive.IsSuccess)
			{
				socket?.Close();
				OnFileUpload(new FileServerInfo
				{
					ActualFileFullName = fullFileName3,
					Name = receive.Content.Name,
					Size = receive.Content.Size,
					Tag = receive.Content.Tag,
					Upload = receive.Content.Upload
				});
				base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileUploadSuccess + ":" + relativeName);
			}
			else
			{
				base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileUploadFailed + ":" + relativeName);
			}
			break;
		}
		case 2003:
		{
			string fullFileName5 = ReturnAbsoluteFileName(Factory, Group, Identify, fileName);
			FileInfo info5 = new FileInfo(fullFileName5);
			DeleteExsistingFile(fileName: GetGroupFromFilePath(info5.DirectoryName).DeleteFile(info5.Name), path: info5.DirectoryName);
			if ((await SendStringAndCheckReceiveAsync(socket, 1, "success")).IsSuccess)
			{
				socket?.Close();
			}
			base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileDeleteSuccess + ":" + relativeName);
			break;
		}
		case 2011:
		{
			string[] fileNames = infoResult.Content.FileNames;
			string[] array = fileNames;
			foreach (string item in array)
			{
				string fullFileName2 = ReturnAbsoluteFileName(Factory, Group, Identify, item);
				FileInfo info2 = new FileInfo(fullFileName2);
				DeleteExsistingFile(fileName: GetGroupFromFilePath(info2.DirectoryName).DeleteFile(info2.Name), path: info2.DirectoryName);
				relativeName = GetRelativeFileName(Factory, Group, Identify, fileName);
				base.LogNet?.WriteInfo(ToString(), StringResources.Language.FileDeleteSuccess + ":" + relativeName);
			}
			if ((await SendStringAndCheckReceiveAsync(socket, 1, "success")).IsSuccess)
			{
				socket?.Close();
			}
			break;
		}
		case 2012:
		{
			string fullFileName4 = ReturnAbsoluteFileName(Factory, Group, Identify, "123.txt");
			FileInfo info4 = new FileInfo(fullFileName4);
			GroupFileContainer fileManagment13 = GetGroupFromFilePath(info4.DirectoryName);
			DeleteGroupFile(fileManagment13);
			if ((await SendStringAndCheckReceiveAsync(socket, 1, "success")).IsSuccess)
			{
				socket?.Close();
			}
			base.LogNet?.WriteInfo(ToString(), "FolderDelete : " + relativeName);
			break;
		}
		case 2014:
		{
			string[] directories2 = GetDirectories(Factory, Group, Identify);
			string[] array3 = directories2;
			foreach (string k2 in array3)
			{
				DirectoryInfo directory3 = new DirectoryInfo(k2);
				GroupFileContainer fileManagment11 = GetGroupFromFilePath(directory3.FullName);
				if (fileManagment11.FileCount == 0)
				{
					DeleteGroupFile(fileManagment11);
				}
			}
			if ((await SendStringAndCheckReceiveAsync(socket, 1, "success")).IsSuccess)
			{
				socket?.Close();
			}
			base.LogNet?.WriteInfo(ToString(), "FolderEmptyDelete : " + relativeName);
			break;
		}
		case 2007:
		{
			GroupFileContainer fileManagment6 = GetGroupFromFilePath(ReturnAbsoluteFilePath(Factory, Group, Identify));
			if ((await SendStringAndCheckReceiveAsync(socket, 2007, fileManagment6.JsonArrayContent)).IsSuccess)
			{
				socket?.Close();
			}
			break;
		}
		case 2015:
		{
			GroupFileContainer fileManagment12 = GetGroupFromFilePath(ReturnAbsoluteFilePath(Factory, Group, Identify));
			if ((await SendStringAndCheckReceiveAsync(socket, 2007, fileManagment12.GetGroupFileInfo().ToJsonString())).IsSuccess)
			{
				socket?.Close();
			}
			break;
		}
		case 2016:
		{
			List<GroupFileInfo> folders2 = new List<GroupFileInfo>();
			string[] directories = GetDirectories(Factory, Group, Identify);
			string[] array2 = directories;
			foreach (string j2 in array2)
			{
				DirectoryInfo directory2 = new DirectoryInfo(j2);
				if (string.IsNullOrEmpty(Factory))
				{
					GroupFileContainer fileManagment8 = GetGroupFromFilePath(ReturnAbsoluteFilePath(directory2.Name, string.Empty, string.Empty));
					GroupFileInfo groupFileInfo3 = fileManagment8.GetGroupFileInfo();
					groupFileInfo3.PathName = directory2.Name;
					folders2.Add(groupFileInfo3);
				}
				else if (string.IsNullOrEmpty(Group))
				{
					GroupFileContainer fileManagment9 = GetGroupFromFilePath(ReturnAbsoluteFilePath(Factory, directory2.Name, string.Empty));
					GroupFileInfo groupFileInfo4 = fileManagment9.GetGroupFileInfo();
					groupFileInfo4.PathName = directory2.Name;
					folders2.Add(groupFileInfo4);
				}
				else
				{
					GroupFileContainer fileManagment10 = GetGroupFromFilePath(ReturnAbsoluteFilePath(Factory, Group, directory2.Name));
					GroupFileInfo groupFileInfo5 = fileManagment10.GetGroupFileInfo();
					groupFileInfo5.PathName = directory2.Name;
					folders2.Add(groupFileInfo5);
				}
			}
			if ((await SendStringAndCheckReceiveAsync(socket, 2016, folders2.ToJsonString())).IsSuccess)
			{
				socket?.Close();
			}
			break;
		}
		case 2008:
		{
			List<string> folders3 = new List<string>();
			string[] directories3 = GetDirectories(Factory, Group, Identify);
			string[] array4 = directories3;
			foreach (string i2 in array4)
			{
				DirectoryInfo directory4 = new DirectoryInfo(i2);
				folders3.Add(directory4.Name);
			}
			JArray jArray = JArray.FromObject(folders3.ToArray());
			if ((await SendStringAndCheckReceiveAsync(socket, 2015, jArray.ToString())).IsSuccess)
			{
				socket?.Close();
			}
			break;
		}
		case 2013:
		{
			string fullPath = ReturnAbsoluteFilePath(Factory, Group, Identify);
			GroupFileContainer fileManagment7 = GetGroupFromFilePath(fullPath);
			bool isExists = fileManagment7.FileExists(fileName);
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

	public override string ToString()
	{
		return $"UltimateFileServer[{base.Port}]";
	}
}
