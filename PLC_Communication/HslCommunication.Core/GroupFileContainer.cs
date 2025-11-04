using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HslCommunication.LogNet;
using Newtonsoft.Json.Linq;

namespace HslCommunication.Core;

public class GroupFileContainer
{
	public delegate void FileCountChangedDelegate(GroupFileContainer container, int fileCount);

	private const string FileListResources = "list.txt";

	private ILogNet LogNet;

	private string jsonArrayContent = "[]";

	private int filesCount = 0;

	private object hybirdLock = new object();

	private HslAsyncCoordinator coordinatorCacheJsonArray;

	private List<GroupFileItem> groupFileItems;

	private string fileFolderPath;

	private string fileFullPath;

	private long totalFileSize = 0L;

	private DateTime lastModifyTime = DateTime.MinValue;

	private GroupFileItem lastModifyFile = null;

	public string JsonArrayContent => jsonArrayContent;

	public int FileCount => filesCount;

	public long TotalDownloadTimes
	{
		get
		{
			long num = 0L;
			lock (hybirdLock)
			{
				for (int i = 0; i < groupFileItems.Count; i++)
				{
					num += groupFileItems[i].DownloadTimes;
				}
			}
			return num;
		}
	}

	public string DirectoryPath => fileFolderPath;

	public event FileCountChangedDelegate FileCountChanged;

	public GroupFileContainer(ILogNet logNet, string path)
	{
		LogNet = logNet;
		if (!string.IsNullOrEmpty(path))
		{
			LoadByPath(path);
		}
	}

	public GroupFileInfo GetGroupFileInfo(bool withLastFileInfo = false)
	{
		GroupFileInfo groupFileInfo = new GroupFileInfo();
		lock (hybirdLock)
		{
			groupFileInfo.PathName = fileFolderPath;
			groupFileInfo.FileCount = filesCount;
			groupFileInfo.FileTotalSize = totalFileSize;
			groupFileInfo.LastModifyTime = lastModifyTime;
			if (withLastFileInfo)
			{
				groupFileInfo.LastModifyFile = lastModifyFile;
			}
		}
		return groupFileInfo;
	}

	public string GetCurrentFileMappingName(string fileName)
	{
		string result = string.Empty;
		lock (hybirdLock)
		{
			for (int i = 0; i < groupFileItems.Count; i++)
			{
				if (groupFileItems[i].FileName == fileName)
				{
					result = groupFileItems[i].MappingName;
					groupFileItems[i].DownloadTimes++;
				}
			}
		}
		coordinatorCacheJsonArray.StartOperaterInfomation();
		return result;
	}

	public string UpdateFileMappingName(string fileName, long fileSize, string mappingName, string owner, string description)
	{
		string text = string.Empty;
		lock (hybirdLock)
		{
			for (int i = 0; i < groupFileItems.Count; i++)
			{
				if (groupFileItems[i].FileName == fileName)
				{
					totalFileSize -= groupFileItems[i].FileSize;
					text = groupFileItems[i].MappingName;
					groupFileItems[i].MappingName = mappingName;
					groupFileItems[i].Description = description;
					groupFileItems[i].FileSize = fileSize;
					groupFileItems[i].Owner = owner;
					groupFileItems[i].UploadTime = DateTime.Now;
					totalFileSize += fileSize;
					if (lastModifyTime < groupFileItems[i].UploadTime)
					{
						lastModifyTime = groupFileItems[i].UploadTime;
						lastModifyFile = groupFileItems[i];
					}
					break;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				GroupFileItem groupFileItem = new GroupFileItem
				{
					FileName = fileName,
					FileSize = fileSize,
					DownloadTimes = 0L,
					Description = description,
					Owner = owner,
					MappingName = mappingName,
					UploadTime = DateTime.Now
				};
				groupFileItems.Add(groupFileItem);
				filesCount = groupFileItems.Count;
				totalFileSize += fileSize;
				if (lastModifyTime < groupFileItem.UploadTime)
				{
					lastModifyTime = groupFileItem.UploadTime;
					lastModifyFile = groupFileItem;
				}
			}
		}
		coordinatorCacheJsonArray.StartOperaterInfomation();
		return text;
	}

	public OperateResult<GroupFileItem> GetUploadTimeByFileName(string fileName)
	{
		GroupFileItem value = null;
		bool flag = false;
		lock (hybirdLock)
		{
			for (int i = 0; i < groupFileItems.Count; i++)
			{
				if (groupFileItems[i].FileName == fileName)
				{
					value = groupFileItems[i];
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			return new OperateResult<GroupFileItem>("File not exist");
		}
		return OperateResult.CreateSuccessResult(value);
	}

	public string DeleteFile(string fileName)
	{
		string result = string.Empty;
		lock (hybirdLock)
		{
			for (int i = 0; i < groupFileItems.Count; i++)
			{
				if (groupFileItems[i].FileName == fileName)
				{
					result = groupFileItems[i].MappingName;
					groupFileItems.RemoveAt(i);
					break;
				}
			}
			UpdatePathInfomation();
		}
		coordinatorCacheJsonArray.StartOperaterInfomation();
		return result;
	}

	public bool FileExists(string fileName)
	{
		bool result = false;
		lock (hybirdLock)
		{
			for (int i = 0; i < groupFileItems.Count; i++)
			{
				if (groupFileItems[i].FileName == fileName)
				{
					result = true;
					if (!File.Exists(Path.Combine(fileFolderPath, groupFileItems[i].MappingName)))
					{
						result = false;
						LogNet?.WriteError("File Check exist failed, find file in list, but mapping file not found");
					}
					break;
				}
			}
		}
		return result;
	}

	public string DeleteFileByGuid(string guidName)
	{
		string result = string.Empty;
		lock (hybirdLock)
		{
			for (int i = 0; i < groupFileItems.Count; i++)
			{
				if (groupFileItems[i].MappingName == guidName)
				{
					result = groupFileItems[i].MappingName;
					groupFileItems.RemoveAt(i);
					break;
				}
			}
			UpdatePathInfomation();
		}
		coordinatorCacheJsonArray.StartOperaterInfomation();
		return result;
	}

	public List<string> ClearAllFiles(out List<string> fileNames)
	{
		List<string> list = new List<string>();
		fileNames = new List<string>();
		lock (hybirdLock)
		{
			for (int i = 0; i < groupFileItems.Count; i++)
			{
				fileNames.Add(groupFileItems[i].FileName);
				list.Add(groupFileItems[i].MappingName);
			}
			groupFileItems.Clear();
			UpdatePathInfomation();
		}
		coordinatorCacheJsonArray.StartOperaterInfomation();
		return list;
	}

	public List<string> GetAllFiles(out List<string> fileNames)
	{
		List<string> list = new List<string>();
		fileNames = new List<string>();
		lock (hybirdLock)
		{
			for (int i = 0; i < groupFileItems.Count; i++)
			{
				fileNames.Add(groupFileItems[i].FileName);
				list.Add(groupFileItems[i].MappingName);
			}
		}
		return list;
	}

	public void DeleteFolder()
	{
		try
		{
			if (Directory.Exists(fileFolderPath))
			{
				Directory.Delete(fileFolderPath, recursive: true);
			}
			LogNet?.WriteInfo("Delete folder[" + fileFolderPath + "] success! ");
		}
		catch (Exception ex)
		{
			LogNet?.WriteError("Delete folder[" + fileFolderPath + "] failed: " + ex.Message);
		}
	}

	public void RenameFolder(string path)
	{
		try
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			if (!directoryInfo.Parent.Exists)
			{
				directoryInfo.Parent.Create();
			}
			if (Directory.Exists(fileFolderPath))
			{
				Directory.Move(fileFolderPath, path);
			}
			else
			{
				Directory.CreateDirectory(path);
			}
			fileFolderPath = path;
			fileFullPath = Path.Combine(path, "list.txt");
			UpdatePathInfomation();
		}
		catch (Exception ex)
		{
			LogNet?.WriteError("Rename folder[" + fileFolderPath + "->" + path + "] failed: " + ex.Message);
		}
	}

	private void UpdatePathInfomation()
	{
		filesCount = groupFileItems.Count;
		long num = 0L;
		lastModifyTime = DateTime.MinValue;
		lastModifyFile = null;
		for (int i = 0; i < groupFileItems.Count; i++)
		{
			num += groupFileItems[i].FileSize;
			if (lastModifyTime < groupFileItems[i].UploadTime)
			{
				lastModifyTime = groupFileItems[i].UploadTime;
				lastModifyFile = groupFileItems[i];
			}
		}
		totalFileSize = num;
	}

	private void CacheJsonArrayContent()
	{
		lock (hybirdLock)
		{
			try
			{
				jsonArrayContent = JArray.FromObject(groupFileItems).ToString();
				using StreamWriter streamWriter = new StreamWriter(fileFullPath, append: false, Encoding.UTF8);
				streamWriter.Write(jsonArrayContent);
				streamWriter.Flush();
			}
			catch (Exception ex)
			{
				LogNet?.WriteException("CacheJsonArrayContent", ex);
			}
		}
		this.FileCountChanged?.Invoke(this, filesCount);
	}

	private void LoadByPath(string path)
	{
		fileFolderPath = path;
		fileFullPath = Path.Combine(path, "list.txt");
		if (!Directory.Exists(fileFolderPath))
		{
			Directory.CreateDirectory(fileFolderPath);
		}
		if (File.Exists(fileFullPath))
		{
			try
			{
				using StreamReader streamReader = new StreamReader(fileFullPath, Encoding.UTF8);
				groupFileItems = JArray.Parse(streamReader.ReadToEnd()).ToObject<List<GroupFileItem>>();
			}
			catch (Exception ex)
			{
				LogNet?.WriteException("GroupFileContainer", "Load files txt failed,", ex);
			}
		}
		if (groupFileItems == null)
		{
			groupFileItems = new List<GroupFileItem>();
		}
		UpdatePathInfomation();
		coordinatorCacheJsonArray = new HslAsyncCoordinator(CacheJsonArrayContent);
		CacheJsonArrayContent();
	}

	public override string ToString()
	{
		return "GroupFileContainer[" + fileFolderPath + "]";
	}
}
