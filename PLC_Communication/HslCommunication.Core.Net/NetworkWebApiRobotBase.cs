using System;
using System.Text;
using System.Threading.Tasks;

namespace HslCommunication.Core.Net;

public class NetworkWebApiRobotBase : NetworkWebApiBase
{
	public NetworkWebApiRobotBase(string ipAddress)
		: base(ipAddress)
	{
	}

	public NetworkWebApiRobotBase(string ipAddress, int port)
		: base(ipAddress, port)
	{
	}

	public NetworkWebApiRobotBase(string ipAddress, int port, string name, string password)
		: base(ipAddress, port, name, password)
	{
	}

	protected virtual OperateResult<string> ReadByAddress(string address)
	{
		return new OperateResult<string>(StringResources.Language.NotSupportedFunction);
	}

	protected virtual async Task<OperateResult<string>> ReadByAddressAsync(string address)
	{
		return new OperateResult<string>(StringResources.Language.NotSupportedFunction);
	}

	public virtual OperateResult<byte[]> Read(string address)
	{
		OperateResult<string> operateResult = ReadString(address);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(Encoding.UTF8.GetBytes(operateResult.Content));
	}

	public virtual OperateResult<string> ReadString(string address)
	{
		if (!Authorization.nzugaydgwadawdibbas())
		{
			return new OperateResult<string>(StringResources.Language.AuthorizationFailed);
		}
		if (address.StartsWith("url=", StringComparison.OrdinalIgnoreCase))
		{
			return Get(address.Substring(4));
		}
		return ReadByAddress(address);
	}

	public virtual OperateResult Write(string address, byte[] value)
	{
		return Write(address, Encoding.Default.GetString(value));
	}

	public virtual OperateResult Write(string address, string value)
	{
		if (address.StartsWith("url=", StringComparison.OrdinalIgnoreCase))
		{
			return Post(address.Substring(4), value);
		}
		return new OperateResult<string>(StringResources.Language.NotSupportedFunction);
	}

	public virtual async Task<OperateResult<byte[]>> ReadAsync(string address)
	{
		OperateResult<string> read = await ReadStringAsync(address);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return OperateResult.CreateSuccessResult(Encoding.UTF8.GetBytes(read.Content));
	}

	public virtual async Task<OperateResult<string>> ReadStringAsync(string address)
	{
		if (!Authorization.nzugaydgwadawdibbas())
		{
			return new OperateResult<string>(StringResources.Language.AuthorizationFailed);
		}
		if (address.StartsWith("url=", StringComparison.OrdinalIgnoreCase))
		{
			return await GetAsync(address.Substring(4));
		}
		return await ReadByAddressAsync(address);
	}

	public virtual async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		return await WriteAsync(address, Encoding.Default.GetString(value));
	}

	public virtual async Task<OperateResult> WriteAsync(string address, string value)
	{
		if (address.StartsWith("url=", StringComparison.OrdinalIgnoreCase))
		{
			return await PostAsync(address.Substring(4), value);
		}
		return new OperateResult<string>(StringResources.Language.NotSupportedFunction);
	}

	public override string ToString()
	{
		return $"NetworkWebApiRobotBase[{base.IpAddress}:{base.Port}]";
	}
}
