using System;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Pipe;
using HslCommunication.Instrument.IEC.Helper;

namespace HslCommunication.Instrument.IEC;

public class IEC104 : DeviceTcpNet
{
	public class TypeID
	{
		public const byte M_SP_NA_1 = 1;

		public const byte M_DP_NA_1 = 3;

		public const byte M_ST_NA_1 = 5;

		public const byte M_BO_NA_1 = 7;

		public const byte M_ME_NA_1 = 9;

		public const byte M_ME_NB_1 = 11;

		public const byte M_ME_NC_1 = 13;

		public const byte M_IT_NA_1 = 15;

		public const byte M_PS_NA_1 = 20;

		public const byte M_ME_ND_1 = 21;

		public const byte M_SP_TB_1 = 30;

		public const byte M_DP_TB_1 = 31;

		public const byte M_ST_TB_1 = 32;

		public const byte M_BO_TB_1 = 33;

		public const byte M_ME_TD_1 = 34;

		public const byte M_ME_TE_1 = 35;

		public const byte M_ME_TF_1 = 36;

		public const byte M_IT_TB_1 = 37;

		public const byte C_SC_NA_1 = 45;

		public const byte C_DC_NA_1 = 46;

		public const byte C_RC_NA_1 = 47;

		public const byte C_SE_NA_1 = 48;

		public const byte C_SE_NB_1 = 49;

		public const byte C_SE_NC_1 = 50;

		public const byte C_SE_ND_1 = 51;

		public const byte C_SE_TA_1 = 58;

		public const byte C_SE_TB_1 = 59;

		public const byte C_SE_TC_1 = 60;

		public const byte C_SE_TD_1 = 61;

		public const byte C_SE_TE_1 = 62;

		public const byte C_SE_TF_1 = 63;

		public const byte C_SE_TG_1 = 64;

		public const byte C_SE_NE_1 = 136;

		public const byte M_EI_NA_1 = 70;

		public const byte C_IC_NA_1 = 100;

		public const byte C_CI_NA_1 = 101;

		public const byte C_RD_NA_1 = 102;

		public const byte C_CS_NA_1 = 103;

		public const byte C_RS_NA_1 = 105;

		public const byte C_TS_TA_1 = 107;
	}

	private int deviceSendLastID = 0;

	private object receiveLock = new object();

	private readonly SoftIncrementCount sendIncrementCount;

	private int receiveIncrementCount = 0;

	private int station = 1;

	public int Station
	{
		get
		{
			return station;
		}
		set
		{
			station = value;
		}
	}

	public event EventHandler<IEC104MessageEventArgs> OnIEC104MessageReceived;

	public IEC104()
	{
		base.WordLength = 2;
		base.ByteTransform = new RegularByteTransform();
		sendIncrementCount = new SoftIncrementCount(32767L, 0L);
	}

	public IEC104(string ipAddress, int port = 2404)
		: this()
	{
		IpAddress = ipAddress;
		Port = port;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new IEC104Message();
	}

	protected override OperateResult InitializationOnConnect()
	{
		OperateResult operateResult = CommunicationPipe.Send(IECHelper.BuildFrameUMessage(7));
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		LogSendMessage(IECHelper.BuildFrameUMessage(7));
		OperateResult operateResult2 = CommunicationPipe.ReceiveMessage(GetNewNetMessage(), null, useActivePush: false, null, base.LogRevcMessage);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		CommunicationPipe.UseServerActivePush = true;
		sendIncrementCount.ResetCurrentValue(0L);
		receiveIncrementCount = 0;
		deviceSendLastID = 0;
		return base.InitializationOnConnect();
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync()
	{
		OperateResult send1 = await CommunicationPipe.SendAsync(IECHelper.BuildFrameUMessage(7)).ConfigureAwait(continueOnCapturedContext: false);
		if (!send1.IsSuccess)
		{
			return send1;
		}
		LogSendMessage(IECHelper.BuildFrameUMessage(7));
		OperateResult recv1 = await CommunicationPipe.ReceiveMessageAsync(GetNewNetMessage(), null, useActivePush: false, null, base.LogRevcMessage).ConfigureAwait(continueOnCapturedContext: false);
		if (!recv1.IsSuccess)
		{
			return recv1;
		}
		CommunicationPipe.UseServerActivePush = true;
		sendIncrementCount.ResetCurrentValue(0L);
		receiveIncrementCount = 0;
		deviceSendLastID = 0;
		return await base.InitializationOnConnectAsync();
	}

	public override OperateResult Write(string address, bool value)
	{
		byte[] obj = new byte[10] { 45, 1, 3, 0, 0, 0, 0, 0, 0, 0 };
		obj[4] = BitConverter.GetBytes(station)[0];
		obj[5] = BitConverter.GetBytes(station)[1];
		byte[] array = obj;
		BitConverter.GetBytes(ushort.Parse(address)).CopyTo(array, 6);
		if (value)
		{
			array[9] = 1;
		}
		return SendFrameIMessage(array);
	}

	public OperateResult WriteIec(byte type, ushort reason, ushort address, bool value)
	{
		return WriteIec(type, reason, address, (!value) ? new byte[1] : new byte[1] { 1 });
	}

	public OperateResult WriteIec(byte type, ushort reason, ushort address, short value)
	{
		return WriteIec(type, reason, address, BitConverter.GetBytes(value));
	}

	public OperateResult WriteIec(byte type, ushort reason, ushort address, float value)
	{
		return WriteIec(type, reason, address, BitConverter.GetBytes(value));
	}

	public OperateResult WriteIec(byte type, ushort reason, ushort address, byte[] value)
	{
		return SendFrameIMessage(IECHelper.BuildWriteIec(type, reason, (ushort)station, address, value));
	}

	private int GetReceiveIncrementCount(int deviceSendID)
	{
		lock (receiveLock)
		{
			if (deviceSendID != deviceSendLastID)
			{
				deviceSendLastID = deviceSendID;
				receiveIncrementCount++;
			}
			int result = receiveIncrementCount;
			if (receiveIncrementCount > 32767)
			{
				receiveIncrementCount = 0;
			}
			return result;
		}
	}

	protected override bool DecideWhetherQAMessage(CommunicationPipe pipe, OperateResult<byte[]> receive)
	{
		if (!receive.IsSuccess)
		{
			return false;
		}
		LogRevcMessage(receive.Content);
		byte[] content = receive.Content;
		if (content.Length < 6)
		{
			return false;
		}
		if (!content[2].GetBoolByIndex(0) && !content[4].GetBoolByIndex(0))
		{
			int num = BitConverter.ToUInt16(content, 2) / 2;
			int num2 = BitConverter.ToUInt16(content, 4) / 2;
			int num3 = GetReceiveIncrementCount(num);
			byte[] array = IECHelper.BuildFrameSMessage(num3);
			LogSendMessage(array);
			pipe.Send(array);
			if (num3 != num)
			{
				base.LogNet?.WriteError(ToString(), $"send[{num}] receive[{num3}] message id check failed: ");
			}
			IEC104MessageEventArgs e = null;
			try
			{
				e = new IEC104MessageEventArgs(content.RemoveBegin(6));
			}
			catch (Exception ex)
			{
				base.LogNet?.WriteException(ToString(), "IEC104MessageEventArg..ctor failed: " + content.ToHexString(' '), ex);
			}
			if (e != null)
			{
				this.OnIEC104MessageReceived?.Invoke(this, e);
			}
		}
		else if (((content[2] & 3) != 1 || content[4].GetBoolByIndex(0)) && (content[2] & 3) == 3 && !content[4].GetBoolByIndex(0) && content[2] == 67)
		{
			byte[] array2 = IECHelper.BuildFrameUMessage(131);
			LogSendMessage(array2);
			pipe.Send(array2);
		}
		return false;
	}

	public OperateResult SendFrameIMessage(byte[] asdu)
	{
		if (CommunicationPipe.IsConnectError())
		{
			OperateResult operateResult = ConnectServer();
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
		}
		int sendID = (int)sendIncrementCount.GetCurrentValue();
		int receiveID = receiveIncrementCount;
		byte[] array = IECHelper.BuildFrameIMessage(sendID, receiveID, asdu[0], asdu[1], base.ByteTransform.TransUInt16(asdu, 2), base.ByteTransform.TransUInt16(asdu, 4), asdu.RemoveBegin(6));
		LogSendMessage(array);
		return CommunicationPipe.Send(array);
	}

	public OperateResult SendFrameUMessage(byte controlField)
	{
		if (CommunicationPipe.IsConnectError())
		{
			OperateResult operateResult = ConnectServer();
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
		}
		byte[] array = IECHelper.BuildFrameUMessage(controlField);
		LogSendMessage(array);
		return CommunicationPipe.Send(array);
	}

	public OperateResult TotalSubscriptions(byte code = 20)
	{
		return TotalSubscriptions(code, 6);
	}

	public OperateResult TotalSubscriptions(byte code, byte reason)
	{
		return TotalSubscriptions((ushort)Station, code, reason);
	}

	public OperateResult TotalSubscriptions(ushort station, byte code, byte reason)
	{
		byte[] array = new byte[10] { 100, 1, 0, 0, 1, 0, 0, 0, 0, 0 };
		array[2] = reason;
		array[9] = code;
		byte[] array2 = array;
		array2[4] = BitConverter.GetBytes(station)[0];
		array2[5] = BitConverter.GetBytes(station)[1];
		return SendFrameIMessage(array2);
	}

	public override string ToString()
	{
		return $"IEC104[{IpAddress}:{Port}]";
	}
}
