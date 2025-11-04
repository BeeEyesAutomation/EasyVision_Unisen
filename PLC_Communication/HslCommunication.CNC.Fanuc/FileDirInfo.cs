using System;
using System.Text;
using HslCommunication.BasicFramework;
using HslCommunication.Core;

namespace HslCommunication.CNC.Fanuc;

public class FileDirInfo
{
	public bool IsDirectory { get; set; }

	public string Name { get; set; }

	public DateTime LastModified { get; set; }

	public int Size { get; set; }

	public FileDirInfo()
	{
	}

	public FileDirInfo(IByteTransform byteTransform, byte[] buffer, int index)
	{
		IsDirectory = byteTransform.TransInt16(buffer, index) == 0;
		Name = buffer.GetStringOrEndChar(index + 28, 36, Encoding.ASCII);
		if (!IsDirectory)
		{
			LastModified = new DateTime(byteTransform.TransInt16(buffer, index + 2), byteTransform.TransInt16(buffer, index + 4), byteTransform.TransInt16(buffer, index + 6), byteTransform.TransInt16(buffer, index + 8), byteTransform.TransInt16(buffer, index + 10), byteTransform.TransInt16(buffer, index + 12));
			Size = byteTransform.TransInt32(buffer, index + 20);
		}
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(IsDirectory ? "[PATH]   " : "[FILE]   ");
		stringBuilder.Append(Name.PadRight(40));
		if (!IsDirectory)
		{
			stringBuilder.Append("     ");
			stringBuilder.Append(LastModified.ToString("yyyy-MM-dd HH:mm:ss"));
			stringBuilder.Append("         ");
			stringBuilder.Append(SoftBasic.GetSizeDescription(Size));
		}
		return stringBuilder.ToString();
	}
}
