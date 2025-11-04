using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Core.IMessage;
using HslCommunication.Instrument.DLT.Helper;
using HslCommunication.Reflection;

namespace HslCommunication.Instrument.DLT;

public class DLT645With1997 : DeviceSerialPort, IDlt645, IReadWriteDevice, IReadWriteNet
{
	private string station = "1";

	public string Station
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

	public bool EnableCodeFE { get; set; }

	public DLT645Type DLTType { get; } = DLT645Type.DLT1997;

	public string Password { get; set; }

	public string OpCode { get; set; }

	public bool CheckDataId { get; set; } = true;

	public DLT645With1997()
	{
		base.ByteTransform = new RegularByteTransform();
		base.ReceiveEmptyDataCount = 5;
	}

	public DLT645With1997(string station)
		: this()
	{
		this.station = station;
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new DLT645Message(CheckDataId);
	}

	public override OperateResult<byte[]> ReadFromCoreServer(byte[] send)
	{
		OperateResult<byte[]> operateResult = base.ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		int num = DLT645Helper.FindHeadCode68H(operateResult.Content);
		if (num > 0)
		{
			return OperateResult.CreateSuccessResult(operateResult.Content.RemoveBegin(num));
		}
		return operateResult;
	}

	public override byte[] PackCommandWithHeader(byte[] command)
	{
		if (EnableCodeFE)
		{
			return SoftBasic.SpliceArray<byte>(new byte[4] { 254, 254, 254, 254 }, command);
		}
		return base.PackCommandWithHeader(command);
	}

	public OperateResult ActiveDeveice()
	{
		return ReadFromCoreServer(new byte[4] { 254, 254, 254, 254 }, hasResponseData: false, usePackAndUnpack: false);
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		return DLT645Helper.Read(this, address, length);
	}

	[HslMqttApi("ReadDoubleArray", "")]
	public override OperateResult<double[]> ReadDouble(string address, ushort length)
	{
		return DLT645Helper.ReadDouble(this, address, length);
	}

	public override OperateResult<string> ReadString(string address, ushort length, Encoding encoding)
	{
		return ByteTransformHelper.GetResultFromArray(ReadStringArray(address));
	}

	public OperateResult<string[]> ReadStringArray(string address)
	{
		return DLT645Helper.ReadStringArray(this, address);
	}

	public override async Task<OperateResult<double[]>> ReadDoubleAsync(string address, ushort length)
	{
		return await Task.Run(() => ReadDouble(address, length));
	}

	public override async Task<OperateResult<string>> ReadStringAsync(string address, ushort length, Encoding encoding)
	{
		return await Task.Run(() => ReadString(address, length, encoding));
	}

	public override async Task<OperateResult> WriteAsync(string address, double[] values)
	{
		return await Task.Run(() => Write(address, values));
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		return DLT645Helper.Write(this, "", "", address, value);
	}

	[HslMqttApi("WriteDoubleArray", "")]
	public override OperateResult Write(string address, double[] values)
	{
		return DLT645Helper.Write(this, "", "", address, values.Select((double m) => m.ToString()).ToArray());
	}

	public OperateResult WriteAddress(string address)
	{
		return DLT645Helper.WriteAddress(this, address);
	}

	public OperateResult BroadcastTime(DateTime dateTime)
	{
		return DLT645Helper.BroadcastTime(this, dateTime);
	}

	public OperateResult ChangeBaudRate(string baudRate)
	{
		return DLT645Helper.ChangeBaudRate(this, baudRate);
	}

	public OperateResult<string> ReadAddress()
	{
		return new OperateResult<string>(StringResources.Language.NotSupportedFunction);
	}

	public OperateResult Trip(DateTime validTime)
	{
		return Trip(Station, validTime);
	}

	public OperateResult Trip(string station, DateTime validTime)
	{
		return DLT645Helper.Function1C(this, "", "", station, 26, validTime);
	}

	public OperateResult SwitchingOn(DateTime validTime)
	{
		return SwitchingOn(Station, validTime);
	}

	public OperateResult SwitchingOn(string station, DateTime validTime)
	{
		return DLT645Helper.Function1C(this, "", "", station, 27, validTime);
	}

	public override string ToString()
	{
		return $"DLT645With1997[{base.PortName}:{base.BaudRate}]";
	}
}
