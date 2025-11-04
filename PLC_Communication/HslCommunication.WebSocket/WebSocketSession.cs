using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using HslCommunication.BasicFramework;
using HslCommunication.Core.Net;
using HslCommunication.Core.Pipe;
using HslCommunication.MQTT;

namespace HslCommunication.WebSocket;

public class WebSocketSession : PipeSession
{
	private object objLock = new object();

	private NetworkStream networkStream = null;

	private SslStream ssl = null;

	private bool sslSecure = false;

	public List<string> Topics { get; set; }

	public IPEndPoint Remote { get; set; }

	public bool IsQASession { get; set; }

	public string Url { get; set; }

	public WebSocketSession()
	{
		Topics = new List<string>();
		base.OnlineTime = DateTime.Now;
	}

	public bool IsClientSubscribe(string topic, bool willcard)
	{
		bool result = false;
		lock (objLock)
		{
			if (willcard)
			{
				for (int i = 0; i < Topics.Count; i++)
				{
					if (MqttHelper.CheckMqttTopicWildcards(topic, Topics[i]))
					{
						result = true;
						break;
					}
				}
			}
			else
			{
				result = Topics.Contains(topic);
			}
		}
		return result;
	}

	public void AddTopic(string topic)
	{
		lock (objLock)
		{
			if (!Topics.Contains(topic))
			{
				Topics.Add(topic);
			}
		}
	}

	public bool RemoveTopic(string topic)
	{
		bool result = false;
		lock (objLock)
		{
			result = Topics.Remove(topic);
		}
		return result;
	}

	private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
	{
		if (sslPolicyErrors == SslPolicyErrors.None)
		{
			return true;
		}
		return !sslSecure;
	}

	internal OperateResult<SslStream> CreateSslStream(bool createNew = false, X509Certificate cert = null)
	{
		if (createNew)
		{
			networkStream?.Close();
			ssl?.Close();
			PipeTcpNet pipeTcpNet = base.Communication as PipeTcpNet;
			networkStream = new NetworkStream(pipeTcpNet.Socket, ownsSocket: false);
			ssl = new SslStream(networkStream, leaveInnerStreamOpen: false, ValidateServerCertificate, null);
			try
			{
				ssl.AuthenticateAsServer(cert, clientCertificateRequired: false, SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12, checkCertificateRevocation: true);
				return OperateResult.CreateSuccessResult(ssl);
			}
			catch (Exception ex)
			{
				return new OperateResult<SslStream>(ex.Message);
			}
		}
		return OperateResult.CreateSuccessResult(ssl);
	}

	public override string ToString()
	{
		return $"WebSocketSession[{Remote}][{SoftBasic.GetTimeSpanDescription(DateTime.Now - base.OnlineTime)}]";
	}
}
