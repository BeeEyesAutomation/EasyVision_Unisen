using System;
using HslCommunication.BasicFramework;

namespace HslCommunication.Core;

public class GroupFileInfo
{
	public string PathName { get; set; }

	public long FileTotalSize { get; set; }

	public int FileCount { get; set; }

	public DateTime LastModifyTime { get; set; }

	public GroupFileItem LastModifyFile { get; set; }

	public GroupFileInfo()
	{
		LastModifyTime = DateTime.Now;
	}

	public override string ToString()
	{
		return $"Count: {FileCount} TotalSize: {FileTotalSize} [{SoftBasic.GetSizeDescription(FileTotalSize)}] ModifyTime:{LastModifyTime:yyyy-MM-dd HH:mm:ss}";
	}
}
