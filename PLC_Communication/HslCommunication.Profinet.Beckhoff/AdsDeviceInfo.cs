using System;
using System.Text;

namespace HslCommunication.Profinet.Beckhoff;

public class AdsDeviceInfo
{
	public byte Major { get; set; }

	public byte Minor { get; set; }

	public ushort Build { get; set; }

	public string DeviceName { get; set; }

	public AdsDeviceInfo()
	{
	}

	public AdsDeviceInfo(byte[] data)
	{
		Major = data[0];
		Minor = data[1];
		Build = BitConverter.ToUInt16(data, 2);
		DeviceName = Encoding.ASCII.GetString(data.RemoveBegin(4)).Trim('\0', ' ');
	}
}
