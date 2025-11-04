using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using HslCommunication.Core.Pipe;
using HslCommunication.Reflection;

namespace HslCommunication.Core.Device;

public class DeviceTcpNet : DeviceCommunication
{
	private PipeTcpNet pipeTcpNet;

	private Lazy<Ping> ping = new Lazy<Ping>(() => new Ping());

	[HslMqttApi(HttpMethod = "GET", Description = "Gets or sets the timeout for the connection, in milliseconds")]
	public virtual int ConnectTimeOut
	{
		get
		{
			if (!(CommunicationPipe is PipeTcpNet { ConnectTimeOut: var connectTimeOut }))
			{
				return this.pipeTcpNet.ConnectTimeOut;
			}
			return connectTimeOut;
		}
		set
		{
			if (value >= 0 && CommunicationPipe is PipeTcpNet pipeTcpNet)
			{
				pipeTcpNet.ConnectTimeOut = value;
			}
		}
	}

	[HslMqttApi(HttpMethod = "GET", Description = "Get or set the IP address of the remote server. If it is a local test, then it needs to be set to 127.0.0.1")]
	public virtual string IpAddress
	{
		get
		{
			if (!(CommunicationPipe is PipeTcpNet { IpAddress: var ipAddress }))
			{
				return this.pipeTcpNet.IpAddress;
			}
			return ipAddress;
		}
		set
		{
			if (CommunicationPipe is PipeTcpNet pipeTcpNet)
			{
				pipeTcpNet.IpAddress = value;
			}
		}
	}

	[HslMqttApi(HttpMethod = "GET", Description = "Gets or sets the port number of the server. The specific value depends on the configuration of the other party.")]
	public virtual int Port
	{
		get
		{
			if (!(CommunicationPipe is PipeTcpNet { Port: var port }))
			{
				return this.pipeTcpNet.Port;
			}
			return port;
		}
		set
		{
			if (CommunicationPipe is PipeTcpNet pipeTcpNet)
			{
				pipeTcpNet.Port = value;
			}
		}
	}

	public IPEndPoint LocalBinding
	{
		get
		{
			if (!(CommunicationPipe is PipeTcpNet { LocalBinding: var localBinding }))
			{
				return this.pipeTcpNet.LocalBinding;
			}
			return localBinding;
		}
		set
		{
			if (CommunicationPipe is PipeTcpNet pipeTcpNet)
			{
				pipeTcpNet.LocalBinding = value;
			}
		}
	}

	public int SocketKeepAliveTime
	{
		get
		{
			if (!(CommunicationPipe is PipeTcpNet { SocketKeepAliveTime: var socketKeepAliveTime }))
			{
				return this.pipeTcpNet.SocketKeepAliveTime;
			}
			return socketKeepAliveTime;
		}
		set
		{
			if (CommunicationPipe is PipeTcpNet pipeTcpNet)
			{
				pipeTcpNet.SocketKeepAliveTime = value;
			}
		}
	}

	public DeviceTcpNet()
		: this("127.0.0.1", 5000)
	{
	}

	public DeviceTcpNet(string ipAddress, int port)
	{
		pipeTcpNet = new PipeTcpNet();
		pipeTcpNet.IpAddress = ipAddress;
		pipeTcpNet.Port = port;
		CommunicationPipe = pipeTcpNet;
	}

	[Obsolete]
	public void SetPersistentConnection()
	{
	}

	public IPStatus IpAddressPing()
	{
		return ping.Value.Send(IpAddress).Status;
	}

	public OperateResult ConnectServer()
	{
		CommunicationPipe?.CloseCommunication();
		OperateResult<bool> operateResult = CommunicationPipe.OpenCommunication();
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.ConnectedSuccess);
		if (operateResult.Content && CommunicationPipe.GetType() != typeof(PipeDebugRemote))
		{
			return InitializationOnConnect();
		}
		return OperateResult.CreateSuccessResult();
	}

	public async Task<OperateResult> ConnectServerAsync()
	{
		await CommunicationPipe.CloseCommunicationAsync().ConfigureAwait(continueOnCapturedContext: false);
		OperateResult<bool> open = await CommunicationPipe.OpenCommunicationAsync().ConfigureAwait(continueOnCapturedContext: false);
		if (!open.IsSuccess)
		{
			return open;
		}
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.ConnectedSuccess);
		if (open.Content && CommunicationPipe.GetType() != typeof(PipeDebugRemote))
		{
			return await InitializationOnConnectAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		return OperateResult.CreateSuccessResult();
	}

	public OperateResult ConnectClose()
	{
		OperateResult operateResult = ExtraOnDisconnect();
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Close);
		return CommunicationPipe.CloseCommunication();
	}

	public async Task<OperateResult> ConnectCloseAsync()
	{
		OperateResult result = await ExtraOnDisconnectAsync().ConfigureAwait(continueOnCapturedContext: false);
		if (!result.IsSuccess)
		{
			return result;
		}
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Close);
		return await CommunicationPipe.CloseCommunicationAsync().ConfigureAwait(continueOnCapturedContext: false);
	}

	public override string ToString()
	{
		return $"DeviceTcpNet<{base.ByteTransform}>{{{CommunicationPipe}}}";
	}
}
