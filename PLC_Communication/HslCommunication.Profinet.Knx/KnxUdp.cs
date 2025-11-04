using System;
using System.Net;
using System.Net.Sockets;
using HslCommunication.Core;
using HslCommunication.LogNet;

namespace HslCommunication.Profinet.Knx;

public class KnxUdp
{
	private const int stateRequestTimerInterval = 60000;

	private IPEndPoint _localEndpoint;

	private IPEndPoint _rouEndpoint;

	private KnxCode KNX_CODE;

	private UdpClient udpClient;

	private ILogNet logNet;

	public byte Channel { get; set; }

	public IPEndPoint RouEndpoint
	{
		get
		{
			return _rouEndpoint;
		}
		set
		{
			_rouEndpoint = value;
		}
	}

	public IPEndPoint LocalEndpoint
	{
		get
		{
			return _localEndpoint;
		}
		set
		{
			_localEndpoint = value;
		}
	}

	public ILogNet LogNet
	{
		get
		{
			return logNet;
		}
		set
		{
			logNet = value;
		}
	}

	public bool IsConnect => KNX_CODE.IsConnect;

	public KnxCode KnxCode => KNX_CODE;

	public KnxUdp()
	{
		KNX_CODE = new KnxCode();
	}

	public void ConnectKnx()
	{
		if (udpClient == null)
		{
			udpClient = new UdpClient(LocalEndpoint)
			{
				Client = 
				{
					DontFragment = true,
					SendBufferSize = 0,
					ReceiveTimeout = 120000
				}
			};
		}
		int num = udpClient.Send(KNX_CODE.Handshake(LocalEndpoint), 26, RouEndpoint);
		udpClient.BeginReceive(ReceiveCallback, null);
		HslHelper.ThreadSleep(1000);
		if (KNX_CODE.IsConnect)
		{
			KNX_CODE.Return_data_msg += KNX_CODE_Return_data_msg;
			KNX_CODE.GetData_msg += KNX_CODE_GetData_msg;
			KNX_CODE.Set_knx_data += KNX_CODE_Set_knx_data;
			KNX_CODE.knx_server_is_real(LocalEndpoint);
		}
	}

	public void KeepConnection()
	{
		KNX_CODE.knx_server_is_real(LocalEndpoint);
	}

	public void DisConnectKnx()
	{
		if (KNX_CODE.Channel != 0)
		{
			byte[] array = KNX_CODE.Disconnect_knx(KNX_CODE.Channel, LocalEndpoint);
			udpClient.Send(array, array.Length, RouEndpoint);
		}
	}

	public void SetKnxData(short addr, byte len, byte[] data)
	{
		KNX_CODE.Knx_Write(addr, len, data);
	}

	public void ReadKnxData(short addr)
	{
		KNX_CODE.knx_server_is_real(LocalEndpoint);
		KNX_CODE.Knx_Resd_step1(addr);
	}

	private void KNX_CODE_Set_knx_data(byte[] data)
	{
		udpClient.Send(data, data.Length, RouEndpoint);
	}

	private void KNX_CODE_GetData_msg(short addr, byte len, byte[] data)
	{
		logNet?.WriteDebug("收到数据 地址：" + addr + " 长度:" + len + "数据：" + BitConverter.ToString(data));
	}

	private void KNX_CODE_Return_data_msg(byte[] data)
	{
		udpClient.Send(data, data.Length, RouEndpoint);
	}

	private void ReceiveCallback(IAsyncResult iar)
	{
		byte[] array = udpClient.EndReceive(iar, ref _rouEndpoint);
		logNet?.WriteDebug("收到报文 {0}", BitConverter.ToString(array));
		KNX_CODE.KNX_check(array);
		if (KNX_CODE.IsConnect)
		{
			udpClient.BeginReceive(ReceiveCallback, null);
		}
	}
}
