using System;
using System.IO;
using System.IO.Compression;

namespace HslCommunication.BasicFramework;

public class SoftZipped
{
	public static byte[] CompressBytes(byte[] bytes)
	{
		if (bytes == null)
		{
			throw new ArgumentNullException("bytes");
		}
		using MemoryStream memoryStream = new MemoryStream();
		using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
		{
			gZipStream.Write(bytes, 0, bytes.Length);
		}
		return memoryStream.ToArray();
	}

	public static byte[] Decompress(byte[] bytes)
	{
		if (bytes == null)
		{
			throw new ArgumentNullException("bytes");
		}
		using MemoryStream stream = new MemoryStream(bytes);
		using GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress);
		using MemoryStream memoryStream = new MemoryStream();
		int num = 1024;
		byte[] buffer = new byte[num];
		int num2 = 0;
		while ((num2 = gZipStream.Read(buffer, 0, num)) > 0)
		{
			memoryStream.Write(buffer, 0, num2);
		}
		return memoryStream.ToArray();
	}
}
