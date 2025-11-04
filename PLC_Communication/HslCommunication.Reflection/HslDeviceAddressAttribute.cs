using System;
using System.Text;

namespace HslCommunication.Reflection;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class HslDeviceAddressAttribute : Attribute
{
	public Type DeviceType { get; set; }

	public string Address { get; }

	public int Length { get; }

	public string Encoding { get; set; } = "ASCII";

	public HslDeviceAddressAttribute(string address)
	{
		Address = address;
		Length = -1;
		DeviceType = null;
	}

	public HslDeviceAddressAttribute(string address, Type deviceType)
	{
		Address = address;
		Length = -1;
		DeviceType = deviceType;
	}

	public HslDeviceAddressAttribute(string address, int length)
	{
		Address = address;
		Length = length;
		DeviceType = null;
	}

	public HslDeviceAddressAttribute(string address, int length, string encoding)
	{
		Address = address;
		Length = length;
		DeviceType = null;
		Encoding = encoding;
	}

	public HslDeviceAddressAttribute(string address, int length, Type deviceType)
	{
		Address = address;
		Length = length;
		DeviceType = deviceType;
	}

	public HslDeviceAddressAttribute(string address, int length, Type deviceType, string encoding)
	{
		Address = address;
		Length = length;
		DeviceType = deviceType;
		Encoding = encoding;
	}

	public int GetDataLength()
	{
		return (Length < 0) ? 1 : Length;
	}

	public override string ToString()
	{
		return $"HslDeviceAddressAttribute[{Address}:{Length}]";
	}

	public Encoding GetEncoding()
	{
		if (Encoding.Equals("ASCII", StringComparison.OrdinalIgnoreCase))
		{
			return System.Text.Encoding.ASCII;
		}
		if (Encoding.Equals("Unicode", StringComparison.OrdinalIgnoreCase))
		{
			return System.Text.Encoding.Unicode;
		}
		if (Encoding.Equals("BigEndianUnicode", StringComparison.OrdinalIgnoreCase))
		{
			return System.Text.Encoding.BigEndianUnicode;
		}
		if (Encoding.Equals("UTF8", StringComparison.OrdinalIgnoreCase))
		{
			return System.Text.Encoding.UTF8;
		}
		if (Encoding.Equals("ANSI", StringComparison.OrdinalIgnoreCase))
		{
			return System.Text.Encoding.Default;
		}
		if (Encoding.Equals("UTF32", StringComparison.OrdinalIgnoreCase))
		{
			return System.Text.Encoding.UTF32;
		}
		if (Encoding.Equals("UTF7", StringComparison.OrdinalIgnoreCase))
		{
			return System.Text.Encoding.UTF7;
		}
		if (Encoding.Equals("GB2312", StringComparison.OrdinalIgnoreCase))
		{
			return System.Text.Encoding.GetEncoding("gb2312");
		}
		return System.Text.Encoding.GetEncoding(Encoding);
	}
}
