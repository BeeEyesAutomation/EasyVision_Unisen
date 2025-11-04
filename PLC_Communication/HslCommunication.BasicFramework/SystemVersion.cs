using System;

namespace HslCommunication.BasicFramework;

[Serializable]
public sealed class SystemVersion
{
	private int m_MainVersion = 2;

	private int m_SecondaryVersion = 0;

	private int m_EditVersion = 0;

	private int m_InnerVersion = 0;

	public int MainVersion => m_MainVersion;

	public int SecondaryVersion => m_SecondaryVersion;

	public int EditVersion => m_EditVersion;

	public int InnerVersion => m_InnerVersion;

	public SystemVersion(string VersionString)
	{
		string[] array = VersionString.Split(new char[1] { '.' }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length >= 1)
		{
			m_MainVersion = Convert.ToInt32(array[0]);
		}
		if (array.Length >= 2)
		{
			m_SecondaryVersion = Convert.ToInt32(array[1]);
		}
		if (array.Length >= 3)
		{
			m_EditVersion = Convert.ToInt32(array[2]);
		}
		if (array.Length >= 4)
		{
			m_InnerVersion = Convert.ToInt32(array[3]);
		}
	}

	public SystemVersion(int main, int sec, int edit)
	{
		m_MainVersion = main;
		m_SecondaryVersion = sec;
		m_EditVersion = edit;
	}

	public SystemVersion(int main, int sec, int edit, int inner)
	{
		m_MainVersion = main;
		m_SecondaryVersion = sec;
		m_EditVersion = edit;
		m_InnerVersion = inner;
	}

	public string ToString(string format)
	{
		if (1 == 0)
		{
		}
		string result = format switch
		{
			"C" => $"{MainVersion}.{SecondaryVersion}.{EditVersion}.{InnerVersion}", 
			"N" => $"{MainVersion}.{SecondaryVersion}.{EditVersion}", 
			"S" => $"{MainVersion}.{SecondaryVersion}", 
			_ => ToString(), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public override string ToString()
	{
		if (InnerVersion == 0)
		{
			return ToString("N");
		}
		return ToString("C");
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public static bool operator ==(SystemVersion SV1, SystemVersion SV2)
	{
		if (SV1.MainVersion != SV2.MainVersion)
		{
			return false;
		}
		if (SV1.SecondaryVersion != SV2.SecondaryVersion)
		{
			return false;
		}
		if (SV1.m_EditVersion != SV2.m_EditVersion)
		{
			return false;
		}
		if (SV1.InnerVersion != SV2.InnerVersion)
		{
			return false;
		}
		return true;
	}

	public static bool operator !=(SystemVersion SV1, SystemVersion SV2)
	{
		if (SV1.MainVersion != SV2.MainVersion)
		{
			return true;
		}
		if (SV1.SecondaryVersion != SV2.SecondaryVersion)
		{
			return true;
		}
		if (SV1.m_EditVersion != SV2.m_EditVersion)
		{
			return true;
		}
		if (SV1.InnerVersion != SV2.InnerVersion)
		{
			return true;
		}
		return false;
	}

	public static bool operator >(SystemVersion SV1, SystemVersion SV2)
	{
		if (SV1.MainVersion > SV2.MainVersion)
		{
			return true;
		}
		if (SV1.MainVersion < SV2.MainVersion)
		{
			return false;
		}
		if (SV1.SecondaryVersion > SV2.SecondaryVersion)
		{
			return true;
		}
		if (SV1.SecondaryVersion < SV2.SecondaryVersion)
		{
			return false;
		}
		if (SV1.EditVersion > SV2.EditVersion)
		{
			return true;
		}
		if (SV1.EditVersion < SV2.EditVersion)
		{
			return false;
		}
		if (SV1.InnerVersion > SV2.InnerVersion)
		{
			return true;
		}
		if (SV1.InnerVersion < SV2.InnerVersion)
		{
			return false;
		}
		return false;
	}

	public static bool operator <(SystemVersion SV1, SystemVersion SV2)
	{
		if (SV1.MainVersion < SV2.MainVersion)
		{
			return true;
		}
		if (SV1.MainVersion > SV2.MainVersion)
		{
			return false;
		}
		if (SV1.SecondaryVersion < SV2.SecondaryVersion)
		{
			return true;
		}
		if (SV1.SecondaryVersion > SV2.SecondaryVersion)
		{
			return false;
		}
		if (SV1.EditVersion < SV2.EditVersion)
		{
			return true;
		}
		if (SV1.EditVersion > SV2.EditVersion)
		{
			return false;
		}
		if (SV1.InnerVersion < SV2.InnerVersion)
		{
			return true;
		}
		if (SV1.InnerVersion > SV2.InnerVersion)
		{
			return false;
		}
		return false;
	}
}
