using System.Threading.Tasks;
using HslCommunication.Core.Net;

namespace HslCommunication.Core.Pipe;

public class PipeDtuNet : PipeTcpNet
{
	public string DTU { get; set; }

	public ushort DTUPort { get; set; }

	public int DTUIpAddress { get; set; }

	public string Pwd { get; set; }

	public NetworkAlienClient DtuServer { get; set; }

	public PipeDtuNet()
	{
	}

	public PipeDtuNet(PipeTcpNet pipeTcpNet)
	{
		base.Socket = pipeTcpNet.Socket;
		base.ReceiveTimeOut = pipeTcpNet.ReceiveTimeOut;
		base.IpAddress = pipeTcpNet.IpAddress;
		base.Port = pipeTcpNet.Port;
	}

	public override OperateResult<bool> OpenCommunication()
	{
		if (IsConnectError())
		{
			NetSupport.CloseSocket(base.Socket);
			return new OperateResult<bool>(StringResources.Language.ConnectionIsNotAvailable);
		}
		return OperateResult.CreateSuccessResult(value: false);
	}

	public override async Task<OperateResult<bool>> OpenCommunicationAsync()
	{
		if (IsConnectError())
		{
			return await Task.FromResult(new OperateResult<bool>(StringResources.Language.ConnectionIsNotAvailable)).ConfigureAwait(continueOnCapturedContext: false);
		}
		return await Task.FromResult(OperateResult.CreateSuccessResult(value: false)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override string ToString()
	{
		return $"PipeDtuNet[{base.IpAddress}:{base.Port}-{DTU}]";
	}
}
