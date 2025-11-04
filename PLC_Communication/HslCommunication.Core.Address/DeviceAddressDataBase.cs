using System;

namespace HslCommunication.Core.Address;

public class DeviceAddressDataBase
{
	public int AddressStart { get; set; }

	public ushort Length { get; set; }

	public virtual void Parse(string address, ushort length)
	{
		AddressStart = int.Parse(address);
		Length = length;
	}

	public override string ToString()
	{
		return AddressStart.ToString();
	}

	public static string GetUnsupportedAddressInfo(string address, Exception ex = null)
	{
		if (ex == null)
		{
			return "Address[" + address + "]: " + StringResources.Language.NotSupportedDataType;
		}
		return "Parse [" + address + "] failed: " + ex.Message;
	}
}
