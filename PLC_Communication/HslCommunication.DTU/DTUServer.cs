using System.Collections.Generic;
using System.Linq;
using HslCommunication.Core.Device;
using HslCommunication.Core.Net;
using HslCommunication.Core.Pipe;

namespace HslCommunication.DTU;

public class DTUServer : NetworkAlienClient
{
	private Dictionary<string, DeviceCommunication> devices;

	public DeviceCommunication this[string dtuId] => devices.ContainsKey(dtuId) ? devices[dtuId] : null;

	public DTUServer(List<DTUSettingType> dTUSettings)
	{
		devices = new Dictionary<string, DeviceCommunication>();
		SetTrustClients(dTUSettings.Select((DTUSettingType m) => m.DtuId).ToArray());
		for (int num = 0; num < dTUSettings.Count; num++)
		{
			devices.Add(dTUSettings[num].DtuId, dTUSettings[num].GetClient());
			devices[dTUSettings[num].DtuId].SetDtuPipe(new PipeDtuNet
			{
				DTU = dTUSettings[num].DtuId
			});
		}
		base.OnClientConnected += DTUServer_OnClientConnected;
	}

	public DTUServer(string[] dtuId, DeviceTcpNet[] networkDevices)
	{
		devices = new Dictionary<string, DeviceCommunication>();
		SetTrustClients(dtuId);
		for (int i = 0; i < dtuId.Length; i++)
		{
			devices.Add(dtuId[i], networkDevices[i]);
			devices[dtuId[i]].SetDtuPipe(new PipeDtuNet
			{
				DTU = dtuId[i]
			});
		}
	}

	protected override void ExtraOnClose()
	{
		foreach (KeyValuePair<string, DeviceCommunication> device in devices)
		{
			if (device.Value.CommunicationPipe is PipeDtuNet pipeDtuNet)
			{
				pipeDtuNet.CloseCommunication();
			}
		}
		base.ExtraOnClose();
	}

	public override int IsClientOnline(PipeDtuNet pipe)
	{
		if (devices[pipe.DTU].CommunicationPipe.IsConnectError())
		{
			return 0;
		}
		return 1;
	}

	private void DTUServer_OnClientConnected(PipeDtuNet dtu)
	{
		devices[dtu.DTU].SetDtuPipe(dtu);
	}

	public PipeDtuNet[] GetPipeSessions()
	{
		return devices.Values.Select((DeviceCommunication m) => m.CommunicationPipe as PipeDtuNet).ToArray();
	}

	public DeviceCommunication[] GetDevices()
	{
		return devices.Values.ToArray();
	}

	public override string ToString()
	{
		return $"DTUServer[{base.Port}]";
	}
}
