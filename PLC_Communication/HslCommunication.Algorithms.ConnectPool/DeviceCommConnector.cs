using System;
using HslCommunication.Core.Device;

namespace HslCommunication.Algorithms.ConnectPool;

public class DeviceCommConnector : IConnector
{
	private DeviceCommunication device = null;

	public DeviceCommunication Device => device;

	public bool IsConnectUsing { get; set; }

	public string GuidToken { get; set; }

	public DateTime LastUseTime { get; set; }

	public DeviceCommConnector(DeviceCommunication device)
	{
		this.device = device;
	}

	public void Close()
	{
		device.CommunicationPipe.CloseCommunication();
	}

	public void Open()
	{
	}
}
