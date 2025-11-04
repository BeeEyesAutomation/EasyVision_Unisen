using System;

namespace HslCommunication.Reflection;

public class HslStructAttribute : Attribute
{
	public int Index { get; set; }

	public int Length { get; set; }

	public string Encoding { get; set; }

	public int StringMode { get; set; }

	public HslStructAttribute(int index)
	{
		Index = index;
	}

	public HslStructAttribute(int index, int length)
		: this(index)
	{
		Length = length;
	}

	public HslStructAttribute(int index, int length, string encoding)
		: this(index, length)
	{
		Encoding = encoding;
	}

	public HslStructAttribute(int index, int length, string encoding, int mode)
		: this(index, length, encoding)
	{
		StringMode = mode;
	}
}
