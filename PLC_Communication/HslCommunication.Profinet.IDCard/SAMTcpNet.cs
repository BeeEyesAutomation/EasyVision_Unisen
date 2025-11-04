using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;
using HslCommunication.Reflection;

namespace HslCommunication.Profinet.IDCard;

public class SAMTcpNet : NetworkDoubleBase
{
	public SAMTcpNet()
	{
		base.ByteTransform = new RegularByteTransform();
	}

	public SAMTcpNet(string ipAddress, int port)
	{
		IpAddress = ipAddress;
		Port = port;
		base.ByteTransform = new RegularByteTransform();
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new SAMMessage();
	}

	[HslMqttApi]
	public OperateResult<string> ReadSafeModuleNumber()
	{
		byte[] send = SAMSerial.PackToSAMCommand(SAMSerial.BuildReadCommand(18, byte.MaxValue, null));
		OperateResult<byte[]> operateResult = ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		OperateResult operateResult2 = SAMSerial.CheckADSCommandAndSum(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult2);
		}
		return SAMSerial.ExtractSafeModuleNumber(operateResult.Content);
	}

	[HslMqttApi]
	public OperateResult CheckSafeModuleStatus()
	{
		byte[] send = SAMSerial.PackToSAMCommand(SAMSerial.BuildReadCommand(18, byte.MaxValue, null));
		OperateResult<byte[]> operateResult = ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		OperateResult operateResult2 = SAMSerial.CheckADSCommandAndSum(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult2);
		}
		if (operateResult.Content[9] != 144)
		{
			return new OperateResult(SAMSerial.GetErrorDescription(operateResult.Content[9]));
		}
		return OperateResult.CreateSuccessResult();
	}

	[HslMqttApi]
	public OperateResult SearchCard()
	{
		byte[] send = SAMSerial.PackToSAMCommand(SAMSerial.BuildReadCommand(32, 1, null));
		OperateResult<byte[]> operateResult = ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		OperateResult operateResult2 = SAMSerial.CheckADSCommandAndSum(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult2);
		}
		if (operateResult.Content[9] != 159)
		{
			return new OperateResult(SAMSerial.GetErrorDescription(operateResult.Content[9]));
		}
		return OperateResult.CreateSuccessResult();
	}

	[HslMqttApi]
	public OperateResult SelectCard()
	{
		byte[] send = SAMSerial.PackToSAMCommand(SAMSerial.BuildReadCommand(32, 2, null));
		OperateResult<byte[]> operateResult = ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		OperateResult operateResult2 = SAMSerial.CheckADSCommandAndSum(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult2);
		}
		if (operateResult.Content[9] != 144)
		{
			return new OperateResult(SAMSerial.GetErrorDescription(operateResult.Content[9]));
		}
		return OperateResult.CreateSuccessResult();
	}

	[HslMqttApi]
	public OperateResult<IdentityCard> ReadCard()
	{
		byte[] send = SAMSerial.PackToSAMCommand(SAMSerial.BuildReadCommand(48, 1, null));
		OperateResult<byte[]> operateResult = ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<IdentityCard>(operateResult);
		}
		OperateResult operateResult2 = SAMSerial.CheckADSCommandAndSum(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<IdentityCard>(operateResult2);
		}
		return SAMSerial.ExtractIdentityCard(operateResult.Content);
	}

	public async Task<OperateResult<string>> ReadSafeModuleNumberAsync()
	{
		byte[] command = SAMSerial.PackToSAMCommand(SAMSerial.BuildReadCommand(18, byte.MaxValue, null));
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		OperateResult check = SAMSerial.CheckADSCommandAndSum(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(check);
		}
		return SAMSerial.ExtractSafeModuleNumber(read.Content);
	}

	public async Task<OperateResult> CheckSafeModuleStatusAsync()
	{
		byte[] command = SAMSerial.PackToSAMCommand(SAMSerial.BuildReadCommand(18, byte.MaxValue, null));
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		OperateResult check = SAMSerial.CheckADSCommandAndSum(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(check);
		}
		if (read.Content[9] != 144)
		{
			return new OperateResult(SAMSerial.GetErrorDescription(read.Content[9]));
		}
		return OperateResult.CreateSuccessResult();
	}

	public async Task<OperateResult> SearchCardAsync()
	{
		byte[] command = SAMSerial.PackToSAMCommand(SAMSerial.BuildReadCommand(32, 1, null));
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		OperateResult check = SAMSerial.CheckADSCommandAndSum(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(check);
		}
		if (read.Content[9] != 159)
		{
			return new OperateResult(SAMSerial.GetErrorDescription(read.Content[9]));
		}
		return OperateResult.CreateSuccessResult();
	}

	public async Task<OperateResult> SelectCardAsync()
	{
		byte[] command = SAMSerial.PackToSAMCommand(SAMSerial.BuildReadCommand(32, 2, null));
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		OperateResult check = SAMSerial.CheckADSCommandAndSum(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(check);
		}
		if (read.Content[9] != 144)
		{
			return new OperateResult(SAMSerial.GetErrorDescription(read.Content[9]));
		}
		return OperateResult.CreateSuccessResult();
	}

	public async Task<OperateResult<IdentityCard>> ReadCardAsync()
	{
		byte[] command = SAMSerial.PackToSAMCommand(SAMSerial.BuildReadCommand(48, 1, null));
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<IdentityCard>(read);
		}
		OperateResult check = SAMSerial.CheckADSCommandAndSum(read.Content);
		if (!check.IsSuccess)
		{
			return OperateResult.CreateFailedResult<IdentityCard>(check);
		}
		return SAMSerial.ExtractIdentityCard(read.Content);
	}

	public override string ToString()
	{
		return $"SAMTcpNet[{IpAddress}:{Port}]";
	}
}
