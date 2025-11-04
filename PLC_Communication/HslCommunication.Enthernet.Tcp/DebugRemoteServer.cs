using System;
using System.Collections.Generic;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;

namespace HslCommunication.Enthernet.Tcp;

public class DebugRemoteServer : DeviceServer
{
	private Dictionary<int, BinaryCommunication> binaryCommunication;

	public DebugRemoteServer()
	{
		binaryCommunication = new Dictionary<int, BinaryCommunication>();
	}

	public void SetDeviceCommunication(BinaryCommunication communication)
	{
		if (binaryCommunication.ContainsKey(0))
		{
			binaryCommunication[0] = communication;
		}
		else
		{
			binaryCommunication.Add(0, communication);
		}
	}

	public void AddDeviceCommunication(ushort key, BinaryCommunication communication)
	{
		binaryCommunication.Add(key, communication);
	}

	public bool RemoveDeviceCommunication(ushort key)
	{
		if (binaryCommunication.ContainsKey(key))
		{
			return binaryCommunication.Remove(key);
		}
		return false;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new AdsNetMessage();
	}

	protected override OperateResult<byte[]> ReadFromCoreServer(PipeSession session, byte[] receive)
	{
		if (receive == null || receive.Length < 6)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		if (this.binaryCommunication == null)
		{
			base.LogNet?.WriteError(ToString(), "BinaryCommunication is null");
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		int num = BitConverter.ToUInt16(receive, 0);
		byte[] send = receive.RemoveBegin(6);
		if (!this.binaryCommunication.ContainsKey(num))
		{
			base.LogNet?.WriteError(ToString(), $"{session.Communication} Communication Key[{num}] is not exist. ");
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		BinaryCommunication binaryCommunication = this.binaryCommunication[num];
		OperateResult<byte[]> operateResult = binaryCommunication.ReadFromCoreServer(send, hasResponseData: true, usePackAndUnpack: false);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content);
	}

	public override string ToString()
	{
		return $"DebugRemoteServer[{base.Port}]";
	}
}
