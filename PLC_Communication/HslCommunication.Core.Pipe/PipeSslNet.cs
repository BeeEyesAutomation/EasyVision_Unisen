using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace HslCommunication.Core.Pipe;

public class PipeSslNet : PipeTcpNet
{
	private bool isServerMode = true;

	private NetworkStream networkStream;

	private SslStream sslStream = null;

	private X509Certificate certificate = null;

	public X509Certificate Certificate
	{
		get
		{
			return certificate;
		}
		set
		{
			certificate = value;
		}
	}

	public bool RemoteCertificateCheck { get; set; } = false;

	public SslProtocols SslProtocols { get; set; } = SslProtocols.Tls;

	public PipeSslNet(bool serverMode)
	{
		isServerMode = serverMode;
		sslInit();
	}

	public PipeSslNet(string ipAddress, int port, bool serverMode)
		: base(ipAddress, port)
	{
		isServerMode = serverMode;
		sslInit();
	}

	public PipeSslNet(Socket socket, IPEndPoint iPEndPoint, bool serverMode)
		: base(socket, iPEndPoint)
	{
		isServerMode = serverMode;
		sslInit();
	}

	private void sslInit()
	{
		SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;
	}

	public void SetCertficate(string certificateFile)
	{
		if (!string.IsNullOrEmpty(certificateFile))
		{
			certificate = X509Certificate.CreateFromCertFile(certificateFile);
		}
	}

	protected override OperateResult OnCommunicationOpen(Socket socket)
	{
		OperateResult<SslStream> operateResult = CreateSslStream(socket, createNew: true);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		return base.OnCommunicationOpen(socket);
	}

	public OperateResult<SslStream> CreateSslStream(Socket socket, bool createNew = false)
	{
		if (createNew)
		{
			networkStream?.Close();
			sslStream?.Close();
			networkStream = new NetworkStream(socket, ownsSocket: false);
			sslStream = new SslStream(networkStream, leaveInnerStreamOpen: false, ValidateCertificate, null);
			try
			{
				if (isServerMode)
				{
					sslStream.AuthenticateAsServer(certificate, clientCertificateRequired: false, SslProtocols, checkCertificateRevocation: true);
					return OperateResult.CreateSuccessResult(sslStream);
				}
				if (certificate == null)
				{
					sslStream.AuthenticateAsClient(host);
				}
				else
				{
					X509CertificateCollection clientCertificates = new X509CertificateCollection(new X509Certificate[1] { certificate });
					sslStream.AuthenticateAsClient(host, clientCertificates, SslProtocols, checkCertificateRevocation: false);
				}
				return OperateResult.CreateSuccessResult(sslStream);
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null)
				{
					return new OperateResult<SslStream>(ex.InnerException.Message);
				}
				return new OperateResult<SslStream>(ex.Message);
			}
		}
		return OperateResult.CreateSuccessResult(sslStream);
	}

	private bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	{
		if (sslPolicyErrors == SslPolicyErrors.None)
		{
			return true;
		}
		return !RemoteCertificateCheck;
	}

	public override OperateResult Send(byte[] data, int offset, int size)
	{
		OperateResult<SslStream> operateResult = CreateSslStream(base.Socket);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		OperateResult operateResult2 = NetSupport.SocketSend(operateResult.Content, data, offset, size);
		if (!operateResult2.IsSuccess && operateResult2.ErrorCode == NetSupport.SocketErrorCode)
		{
			CloseCommunication();
			return new OperateResult<byte[]>(-IncrConnectErrorCount(), operateResult2.Message);
		}
		return operateResult2;
	}

	public override OperateResult<int> Receive(byte[] buffer, int offset, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		OperateResult<SslStream> operateResult = CreateSslStream(base.Socket);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int>(operateResult);
		}
		OperateResult<int> operateResult2 = NetSupport.SocketReceive(operateResult.Content, buffer, offset, length, timeOut, reportProgress);
		if (!operateResult2.IsSuccess && operateResult2.ErrorCode == NetSupport.SocketErrorCode)
		{
			CloseCommunication();
			return new OperateResult<int>(-IncrConnectErrorCount(), "Socket Exception -> " + operateResult2.Message);
		}
		return operateResult2;
	}

	public override async Task<OperateResult> SendAsync(byte[] data, int offset, int size)
	{
		OperateResult<SslStream> ssl = CreateSslStream(base.Socket);
		if (!ssl.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(ssl);
		}
		OperateResult send = await NetSupport.SocketSendAsync(ssl.Content, data, offset, size).ConfigureAwait(continueOnCapturedContext: false);
		if (!send.IsSuccess && send.ErrorCode == NetSupport.SocketErrorCode)
		{
			await CloseCommunicationAsync().ConfigureAwait(continueOnCapturedContext: false);
			return new OperateResult<byte[]>(-IncrConnectErrorCount(), send.Message);
		}
		return send;
	}

	public override async Task<OperateResult<int>> ReceiveAsync(byte[] buffer, int offset, int length, int timeOut = 60000, Action<long, long> reportProgress = null)
	{
		OperateResult<SslStream> ssl = CreateSslStream(base.Socket);
		if (!ssl.IsSuccess)
		{
			return OperateResult.CreateFailedResult<int>(ssl);
		}
		OperateResult<int> receive = await NetSupport.SocketReceiveAsync(ssl.Content, buffer, offset, length, timeOut, reportProgress).ConfigureAwait(continueOnCapturedContext: false);
		if (!receive.IsSuccess && receive.ErrorCode == NetSupport.SocketErrorCode)
		{
			await CloseCommunicationAsync().ConfigureAwait(continueOnCapturedContext: false);
			return new OperateResult<int>(-IncrConnectErrorCount(), "Socket Exception -> " + receive.Message);
		}
		return receive;
	}

	public override string ToString()
	{
		return $"PipeSslNet[{host}:{base.Port}]";
	}
}
