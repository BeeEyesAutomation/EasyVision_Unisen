using System.Threading.Tasks;
using HslCommunication.Core.Address;
using HslCommunication.Profinet.Melsec;
using HslCommunication.Profinet.Melsec.Helper;

namespace HslCommunication.Profinet.Keyence;

public class KeyenceMcAsciiNet : MelsecMcAsciiNet
{
	public KeyenceMcAsciiNet()
	{
	}

	public KeyenceMcAsciiNet(string ipAddress, int port)
		: base(ipAddress, port)
	{
	}

	public override OperateResult<McAddressData> McAnalysisAddress(string address, ushort length, bool isBit)
	{
		return McAddressData.ParseKeyenceFrom(address, length, isBit);
	}

	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		if (KeyenceMcNet.CheckKeyenceBoolAddress(address))
		{
			return McHelper.ReadBool(this, address, length, supportWordAdd: false);
		}
		return base.ReadBool(address, length);
	}

	public override OperateResult Write(string address, bool[] values)
	{
		if (KeyenceMcNet.CheckKeyenceBoolAddress(address))
		{
			return McHelper.Write(this, address, values, supportWordAdd: false);
		}
		return base.Write(address, values);
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		if (KeyenceMcNet.CheckKeyenceBoolAddress(address))
		{
			return await McHelper.ReadBoolAsync(this, address, length, supportWordAdd: false).ConfigureAwait(continueOnCapturedContext: false);
		}
		return await base.ReadBoolAsync(address, length).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] values)
	{
		if (KeyenceMcNet.CheckKeyenceBoolAddress(address))
		{
			return await McHelper.WriteAsync(this, address, values, supportWordAdd: false).ConfigureAwait(continueOnCapturedContext: false);
		}
		return await base.WriteAsync(address, values).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override string ToString()
	{
		return $"KeyenceMcAsciiNet[{IpAddress}:{Port}]";
	}
}
