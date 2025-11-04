using System;
using System.Collections.Generic;
using System.Text;
using HslCommunication.BasicFramework;

namespace HslCommunication;

internal class HslProtocol
{
	internal const int HeadByteLength = 32;

	internal const int ProtocolBufferSize = 1024;

	internal const int ProtocolCheckSecends = 1;

	internal const int ProtocolClientQuit = 2;

	internal const int ProtocolClientRefuseLogin = 3;

	internal const int ProtocolClientAllowLogin = 4;

	internal const int ProtocolAccountLogin = 5;

	internal const int ProtocolAccountRejectLogin = 6;

	internal const int ProtocolUserString = 1001;

	internal const int ProtocolUserBytes = 1002;

	internal const int ProtocolUserBitmap = 1003;

	internal const int ProtocolUserException = 1004;

	internal const int ProtocolUserStringArray = 1005;

	internal const int ProtocolFileDownload = 2001;

	internal const int ProtocolFileUpload = 2002;

	internal const int ProtocolFileDelete = 2003;

	internal const int ProtocolFileCheckRight = 2004;

	internal const int ProtocolFileCheckError = 2005;

	internal const int ProtocolFileSaveError = 2006;

	internal const int ProtocolFileDirectoryFiles = 2007;

	internal const int ProtocolFileDirectories = 2008;

	internal const int ProtocolProgressReport = 2009;

	internal const int ProtocolErrorMsg = 2010;

	internal const int ProtocolFilesDelete = 2011;

	internal const int ProtocolFolderDelete = 2012;

	internal const int ProtocolFileExists = 2013;

	internal const int ProtocolEmptyFolderDelete = 2014;

	internal const int ProtocolFolderInfo = 2015;

	internal const int ProtocolFolderInfos = 2016;

	internal const int ProtocolNoZipped = 3001;

	internal const int ProtocolZipped = 3002;

	internal static byte[] CommandBytes(int command, int customer, Guid token, byte[] data)
	{
		int value = 3001;
		int num = ((data != null) ? data.Length : 0);
		byte[] array = new byte[32 + num];
		BitConverter.GetBytes(command).CopyTo(array, 0);
		BitConverter.GetBytes(customer).CopyTo(array, 4);
		BitConverter.GetBytes(value).CopyTo(array, 8);
		token.ToByteArray().CopyTo(array, 12);
		if (num > 0)
		{
			BitConverter.GetBytes(num).CopyTo(array, 28);
			Array.Copy(data, 0, array, 32, num);
			HslSecurity.ByteEncrypt(array, 32, num);
		}
		return array;
	}

	internal static byte[] CommandAnalysis(byte[] head, byte[] content)
	{
		if (content != null)
		{
			int num = BitConverter.ToInt32(head, 8);
			if (num == 3002)
			{
				content = SoftZipped.Decompress(content);
			}
			return HslSecurity.ByteDecrypt(content);
		}
		return null;
	}

	internal static byte[] CommandBytes(int customer, Guid token, byte[] data)
	{
		return CommandBytes(1002, customer, token, data);
	}

	internal static byte[] CommandBytes(int customer, Guid token, string data)
	{
		if (data == null)
		{
			return CommandBytes(1001, customer, token, null);
		}
		return CommandBytes(1001, customer, token, Encoding.Unicode.GetBytes(data));
	}

	internal static byte[] CommandBytes(int customer, Guid token, string[] data)
	{
		return CommandBytes(1005, customer, token, PackStringArrayToByte(data));
	}

	internal static byte[] PackStringArrayToByte(string data)
	{
		return PackStringArrayToByte(new string[1] { data });
	}

	internal static byte[] PackStringArrayToByte(string[] data)
	{
		if (data == null)
		{
			data = new string[0];
		}
		List<byte> list = new List<byte>();
		list.AddRange(BitConverter.GetBytes(data.Length));
		for (int i = 0; i < data.Length; i++)
		{
			if (!string.IsNullOrEmpty(data[i]))
			{
				byte[] bytes = Encoding.Unicode.GetBytes(data[i]);
				list.AddRange(BitConverter.GetBytes(bytes.Length));
				list.AddRange(bytes);
			}
			else
			{
				list.AddRange(BitConverter.GetBytes(0));
			}
		}
		return list.ToArray();
	}

	internal static string[] UnPackStringArrayFromByte(byte[] content)
	{
		if (content != null && content.Length < 4)
		{
			return null;
		}
		int num = 0;
		int num2 = BitConverter.ToInt32(content, num);
		string[] array = new string[num2];
		num += 4;
		for (int i = 0; i < num2; i++)
		{
			int num3 = BitConverter.ToInt32(content, num);
			num += 4;
			if (num3 > 0)
			{
				array[i] = Encoding.Unicode.GetString(content, num, num3);
			}
			else
			{
				array[i] = string.Empty;
			}
			num += num3;
		}
		return array;
	}

	public static OperateResult<NetHandle, byte[]> ExtractHslData(byte[] content)
	{
		if (content.Length == 0)
		{
			return OperateResult.CreateSuccessResult((NetHandle)0, new byte[0]);
		}
		byte[] array = new byte[32];
		byte[] array2 = new byte[content.Length - 32];
		Array.Copy(content, 0, array, 0, 32);
		if (array2.Length != 0)
		{
			Array.Copy(content, 32, array2, 0, content.Length - 32);
		}
		if (BitConverter.ToInt32(array, 0) == 2010)
		{
			return new OperateResult<NetHandle, byte[]>(Encoding.Unicode.GetString(array2));
		}
		int num = BitConverter.ToInt32(array, 0);
		int num2 = BitConverter.ToInt32(array, 4);
		array2 = CommandAnalysis(array, array2);
		if (num == 6)
		{
			return new OperateResult<NetHandle, byte[]>(Encoding.Unicode.GetString(array2));
		}
		return OperateResult.CreateSuccessResult((NetHandle)num2, array2);
	}
}
