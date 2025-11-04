using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Pipe;
using HslCommunication.Core.Security;

namespace HslCommunication.MQTT;

public class MqttSession : ISessionContext
{
	private object objLock = new object();

	public IPEndPoint EndPoint { get; set; }

	public string ClientId { get; set; }

	public DateTime ActiveTime { get; set; }

	public DateTime OnlineTime { get; private set; }

	public TimeSpan ActiveTimeSpan { get; set; }

	internal PipeTcpNet MqttPipe { get; set; }

	private List<string> Topics { get; set; }

	public string UserName { get; set; }

	public string Protocol { get; private set; }

	public string WillTopic { get; set; }

	public byte[] WillMessage { get; set; }

	public bool DeveloperPermissions { get; set; } = false;

	public bool IsAesCryptography => AesCryptography != null;

	internal AesCryptography AesCryptography { get; set; }

	public object Tag { get; set; }

	public bool ForbidPublishTopic { get; set; }

	public MqttSession(IPEndPoint endPoint, string protocol)
	{
		Topics = new List<string>();
		ActiveTime = DateTime.Now;
		OnlineTime = DateTime.Now;
		ActiveTimeSpan = TimeSpan.FromSeconds(1000000.0);
		EndPoint = endPoint;
		Protocol = protocol;
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

	public string[] GetTopics()
	{
		lock (objLock)
		{
			return Topics.ToArray();
		}
	}

	public void AddSubscribe(string topic)
	{
		lock (objLock)
		{
			if (!Topics.Contains(topic))
			{
				Topics.Add(topic);
			}
		}
	}

	public void AddSubscribe(string[] topics)
	{
		if (topics == null)
		{
			return;
		}
		lock (objLock)
		{
			for (int i = 0; i < topics.Length; i++)
			{
				if (!Topics.Contains(topics[i]))
				{
					Topics.Add(topics[i]);
				}
			}
		}
	}

	public void RemoveSubscribe(string topic)
	{
		lock (objLock)
		{
			if (Topics.Contains(topic))
			{
				Topics.Remove(topic);
			}
		}
	}

	public void RemoveSubscribe(string[] topics)
	{
		if (topics == null)
		{
			return;
		}
		lock (objLock)
		{
			for (int i = 0; i < topics.Length; i++)
			{
				if (Topics.Contains(topics[i]))
				{
					Topics.Remove(topics[i]);
				}
			}
		}
	}

	public string GetSessionOnlineInfo()
	{
		StringBuilder stringBuilder = new StringBuilder(ToString());
		stringBuilder.Append(" [" + SoftBasic.GetTimeSpanDescription(DateTime.Now - OnlineTime) + "]");
		return stringBuilder.ToString();
	}

	public OperateResult SendMqttCommand(byte head, byte[] variableHeader, byte[] payLoad, AesCryptography aesCryptography = null)
	{
		OperateResult<byte[]> operateResult = MqttHelper.BuildMqttCommand(head, variableHeader, payLoad, aesCryptography);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (MqttPipe == null)
		{
			return new OperateResult("Pipe is Null");
		}
		return MqttPipe.Send(operateResult.Content);
	}

	public OperateResult SendMqttCommand(byte control, byte flags, byte[] variableHeader, byte[] payLoad, AesCryptography aesCryptography = null)
	{
		control <<= 4;
		byte head = (byte)(control | flags);
		return SendMqttCommand(head, variableHeader, payLoad, aesCryptography);
	}

	public async Task<OperateResult> SendMqttCommandAsync(byte head, byte[] variableHeader, byte[] payLoad, AesCryptography aesCryptography = null)
	{
		OperateResult<byte[]> command = MqttHelper.BuildMqttCommand(head, variableHeader, payLoad, aesCryptography);
		if (!command.IsSuccess)
		{
			return command;
		}
		if (MqttPipe == null)
		{
			return new OperateResult("Pipe is Null");
		}
		return await MqttPipe.SendAsync(command.Content);
	}

	public MqttSessionInfo GetSessionInfo()
	{
		return new MqttSessionInfo
		{
			EndPoint = EndPoint.ToString(),
			ClientId = ClientId,
			ActiveTime = ActiveTime,
			OnlineTime = OnlineTime,
			Topics = Topics.ToArray(),
			UserName = UserName,
			Protocol = Protocol,
			WillTopic = WillTopic,
			DeveloperPermissions = DeveloperPermissions,
			IsAesCryptography = IsAesCryptography,
			ForbidPublishTopic = ForbidPublishTopic
		};
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder($"{Protocol} Session[IP:{EndPoint}]");
		if (!string.IsNullOrEmpty(ClientId))
		{
			stringBuilder.Append(" [ID:" + ClientId + "]");
		}
		if (!string.IsNullOrEmpty(UserName))
		{
			stringBuilder.Append(" [Name:" + UserName + "]");
		}
		if (IsAesCryptography)
		{
			stringBuilder.Append("[RSA/AES]");
		}
		return stringBuilder.ToString();
	}
}
