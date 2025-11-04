using System;
using HslCommunication.BasicFramework;

namespace HslCommunication.Core;

public class GroupFileItem
{
	public string FileName { get; set; }

	public long FileSize { get; set; }

	public string MappingName { get; set; }

	public long DownloadTimes { get; set; }

	public DateTime UploadTime { get; set; }

	public string Owner { get; set; }

	public string Description { get; set; }

	public string GetTextFromFileSize()
	{
		return SoftBasic.GetSizeDescription(FileSize);
	}

	public override string ToString()
	{
		return "GroupFileItem[" + FileName + ":" + MappingName + "]";
	}
}
