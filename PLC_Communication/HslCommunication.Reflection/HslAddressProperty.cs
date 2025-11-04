using System.Reflection;

namespace HslCommunication.Reflection;

public class HslAddressProperty
{
	public HslDeviceAddressAttribute DeviceAddressAttribute { get; set; }

	public PropertyInfo PropertyInfo { get; set; }

	public int ByteOffset { get; set; }

	public int ByteLength { get; set; }

	public byte[] Buffer { get; set; }
}
