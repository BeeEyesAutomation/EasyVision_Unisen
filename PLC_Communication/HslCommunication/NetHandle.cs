using System.Runtime.InteropServices;

namespace HslCommunication;

[StructLayout(LayoutKind.Explicit)]
public struct NetHandle
{
	[FieldOffset(0)]
	private int m_CodeValue;

	[FieldOffset(3)]
	private byte m_CodeMajor;

	[FieldOffset(2)]
	private byte m_CodeMinor;

	[FieldOffset(0)]
	private ushort m_CodeIdentifier;

	public int CodeValue
	{
		get
		{
			return m_CodeValue;
		}
		set
		{
			m_CodeValue = value;
		}
	}

	public byte CodeMajor
	{
		get
		{
			return m_CodeMajor;
		}
		private set
		{
			m_CodeMajor = value;
		}
	}

	public byte CodeMinor
	{
		get
		{
			return m_CodeMinor;
		}
		private set
		{
			m_CodeMinor = value;
		}
	}

	public ushort CodeIdentifier
	{
		get
		{
			return m_CodeIdentifier;
		}
		private set
		{
			m_CodeIdentifier = value;
		}
	}

	public static implicit operator NetHandle(int value)
	{
		return new NetHandle(value);
	}

	public static implicit operator int(NetHandle netHandle)
	{
		return netHandle.m_CodeValue;
	}

	public static bool operator ==(NetHandle netHandle1, NetHandle netHandle2)
	{
		return netHandle1.CodeValue == netHandle2.CodeValue;
	}

	public static bool operator !=(NetHandle netHandle1, NetHandle netHandle2)
	{
		return netHandle1.CodeValue != netHandle2.CodeValue;
	}

	public static NetHandle operator +(NetHandle netHandle1, NetHandle netHandle2)
	{
		return new NetHandle(netHandle1.CodeValue + netHandle2.CodeValue);
	}

	public static NetHandle operator -(NetHandle netHandle1, NetHandle netHandle2)
	{
		return new NetHandle(netHandle1.CodeValue - netHandle2.CodeValue);
	}

	public static bool operator <(NetHandle netHandle1, NetHandle netHandle2)
	{
		return netHandle1.CodeValue < netHandle2.CodeValue;
	}

	public static bool operator >(NetHandle netHandle1, NetHandle netHandle2)
	{
		return netHandle1.CodeValue > netHandle2.CodeValue;
	}

	public NetHandle(int value)
	{
		m_CodeMajor = 0;
		m_CodeMinor = 0;
		m_CodeIdentifier = 0;
		m_CodeValue = value;
	}

	public NetHandle(byte major, byte minor, ushort identifier)
	{
		m_CodeValue = 0;
		m_CodeMajor = major;
		m_CodeMinor = minor;
		m_CodeIdentifier = identifier;
	}

	public override string ToString()
	{
		return m_CodeValue.ToString();
	}

	public override bool Equals(object obj)
	{
		if (obj is NetHandle netHandle)
		{
			return CodeValue.Equals(netHandle.CodeValue);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
