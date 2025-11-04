using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.Core;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Net;

namespace HslCommunication.Enthernet;

public class NetSimplifyClient : NetworkDoubleBase
{
	public NetSimplifyClient(string ipAddress, int port)
	{
		base.ByteTransform = new RegularByteTransform();
		IpAddress = ipAddress;
		Port = port;
	}

	public NetSimplifyClient(IPAddress ipAddress, int port)
	{
		base.ByteTransform = new RegularByteTransform();
		IpAddress = ipAddress.ToString();
		Port = port;
	}

	public NetSimplifyClient()
	{
		base.ByteTransform = new RegularByteTransform();
	}

	protected override INetMessage GetNewNetMessage()
	{
		return new HslMessage();
	}

	protected override OperateResult InitializationOnConnect(Socket socket)
	{
		if (isUseAccountCertificate)
		{
			return AccountCertificate(socket);
		}
		return OperateResult.CreateSuccessResult();
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync(Socket socket)
	{
		if (isUseAccountCertificate)
		{
			return await AccountCertificateAsync(socket);
		}
		return OperateResult.CreateSuccessResult();
	}

	public OperateResult<string> ReadFromServer(NetHandle customer, string send)
	{
		OperateResult<byte[]> operateResult = ReadFromServerBase(HslProtocol.CommandBytes(customer, base.Token, send));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		return OperateResult.CreateSuccessResult(Encoding.Unicode.GetString(operateResult.Content));
	}

	public OperateResult<string[]> ReadFromServer(NetHandle customer, string[] send)
	{
		OperateResult<byte[]> operateResult = ReadFromServerBase(HslProtocol.CommandBytes(customer, base.Token, send));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(HslProtocol.UnPackStringArrayFromByte(operateResult.Content));
	}

	public OperateResult<byte[]> ReadFromServer(NetHandle customer, byte[] send)
	{
		return ReadFromServerBase(HslProtocol.CommandBytes(customer, base.Token, send));
	}

	public OperateResult<NetHandle, string> ReadCustomerFromServer(NetHandle customer, string send)
	{
		OperateResult<NetHandle, byte[]> operateResult = ReadCustomerFromServerBase(HslProtocol.CommandBytes(customer, base.Token, send));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<NetHandle, string>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content1, Encoding.Unicode.GetString(operateResult.Content2));
	}

	public OperateResult<NetHandle, string[]> ReadCustomerFromServer(NetHandle customer, string[] send)
	{
		OperateResult<NetHandle, byte[]> operateResult = ReadCustomerFromServerBase(HslProtocol.CommandBytes(customer, base.Token, send));
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<NetHandle, string[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content1, HslProtocol.UnPackStringArrayFromByte(operateResult.Content2));
	}

	public OperateResult<NetHandle, byte[]> ReadCustomerFromServer(NetHandle customer, byte[] send)
	{
		return ReadCustomerFromServerBase(HslProtocol.CommandBytes(customer, base.Token, send));
	}

	private OperateResult<byte[]> ReadFromServerBase(byte[] send)
	{
		OperateResult<NetHandle, byte[]> operateResult = ReadCustomerFromServerBase(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content2);
	}

	private OperateResult<NetHandle, byte[]> ReadCustomerFromServerBase(byte[] send)
	{
		OperateResult<byte[]> operateResult = ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<NetHandle, byte[]>(operateResult);
		}
		return HslProtocol.ExtractHslData(operateResult.Content);
	}

	public async Task<OperateResult<string>> ReadFromServerAsync(NetHandle customer, string send)
	{
		OperateResult<byte[]> read = await ReadFromServerBaseAsync(HslProtocol.CommandBytes(customer, base.Token, send));
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		return OperateResult.CreateSuccessResult(Encoding.Unicode.GetString(read.Content));
	}

	public async Task<OperateResult<string[]>> ReadFromServerAsync(NetHandle customer, string[] send)
	{
		OperateResult<byte[]> read = await ReadFromServerBaseAsync(HslProtocol.CommandBytes(customer, base.Token, send));
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(read);
		}
		return OperateResult.CreateSuccessResult(HslProtocol.UnPackStringArrayFromByte(read.Content));
	}

	public async Task<OperateResult<byte[]>> ReadFromServerAsync(NetHandle customer, byte[] send)
	{
		return await ReadFromServerBaseAsync(HslProtocol.CommandBytes(customer, base.Token, send));
	}

	public async Task<OperateResult<NetHandle, string>> ReadCustomerFromServerAsync(NetHandle customer, string send)
	{
		OperateResult<NetHandle, byte[]> read = await ReadCustomerFromServerBaseAsync(HslProtocol.CommandBytes(customer, base.Token, send));
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<NetHandle, string>(read);
		}
		return OperateResult.CreateSuccessResult(read.Content1, Encoding.Unicode.GetString(read.Content2));
	}

	public async Task<OperateResult<NetHandle, string[]>> ReadCustomerFromServerAsync(NetHandle customer, string[] send)
	{
		OperateResult<NetHandle, byte[]> read = await ReadCustomerFromServerBaseAsync(HslProtocol.CommandBytes(customer, base.Token, send));
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<NetHandle, string[]>(read);
		}
		return OperateResult.CreateSuccessResult(read.Content1, HslProtocol.UnPackStringArrayFromByte(read.Content2));
	}

	public async Task<OperateResult<NetHandle, byte[]>> ReadCustomerFromServerAsync(NetHandle customer, byte[] send)
	{
		return await ReadCustomerFromServerBaseAsync(HslProtocol.CommandBytes(customer, base.Token, send));
	}

	private async Task<OperateResult<byte[]>> ReadFromServerBaseAsync(byte[] send)
	{
		OperateResult<NetHandle, byte[]> read = await ReadCustomerFromServerBaseAsync(send);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult(read.Content2);
	}

	private async Task<OperateResult<NetHandle, byte[]>> ReadCustomerFromServerBaseAsync(byte[] send)
	{
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(send);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<NetHandle, byte[]>(read);
		}
		return HslProtocol.ExtractHslData(read.Content);
	}

	public override string ToString()
	{
		return $"NetSimplifyClient[{IpAddress}:{Port}]";
	}
}
