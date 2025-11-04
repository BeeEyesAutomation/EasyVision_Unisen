using System;
using System.IO;
using HslCommunication.Core.IMessage;
using HslCommunication.MQTT;

namespace HslCommunication.Core.Pipe;

public class PipeMqttClient : CommunicationPipe
{
	private MqttClient mqttClient;

	private string writeTopic = string.Empty;

	private string readTopic = string.Empty;

	public MqttClient MqttClient => mqttClient;

	public string ReadTopic => readTopic;

	public string WriteTopic => writeTopic;

	public PipeMqttClient(MqttClient mqttClient, string readTopic, string writeTopic)
	{
		this.readTopic = readTopic;
		this.writeTopic = writeTopic;
		if (mqttClient != null)
		{
			SubscribeTopic subscribeTopic = mqttClient.GetSubscribeTopic(readTopic);
			if (subscribeTopic == null)
			{
				mqttClient.OnClientConnected += MqttClient_OnClientConnected;
				mqttClient.SubscribeMessage(readTopic);
				subscribeTopic = mqttClient.GetSubscribeTopic(readTopic);
				subscribeTopic.OnMqttMessageReceived += SubscribeTopic_OnMqttMessageReceived;
			}
		}
		base.UseServerActivePush = true;
		this.mqttClient = mqttClient;
	}

	private void MqttClient_OnClientConnected(MqttClient client)
	{
		client.SubscribeMessage(readTopic);
	}

	private void SubscribeTopic_OnMqttMessageReceived(MqttClient client, MqttApplicationMessage message)
	{
		SetBufferQA(message.Payload);
	}

	public override OperateResult<bool> OpenCommunication()
	{
		SubscribeTopic subscribeTopic = mqttClient.GetSubscribeTopic(readTopic);
		if (subscribeTopic == null)
		{
			mqttClient.OnClientConnected += MqttClient_OnClientConnected;
			mqttClient.SubscribeMessage(readTopic);
			subscribeTopic = mqttClient.GetSubscribeTopic(readTopic);
			subscribeTopic.OnMqttMessageReceived += SubscribeTopic_OnMqttMessageReceived;
		}
		return OperateResult.CreateSuccessResult(value: false);
	}

	public override OperateResult CloseCommunication()
	{
		SubscribeTopic subscribeTopic = mqttClient.GetSubscribeTopic(readTopic);
		if (subscribeTopic != null)
		{
			subscribeTopic.OnMqttMessageReceived -= SubscribeTopic_OnMqttMessageReceived;
		}
		mqttClient.OnClientConnected -= MqttClient_OnClientConnected;
		mqttClient.UnSubscribeMessage(readTopic);
		return OperateResult.CreateSuccessResult();
	}

	public override OperateResult Send(byte[] data, int offset, int size)
	{
		if (data == null)
		{
			return OperateResult.CreateSuccessResult();
		}
		return mqttClient.PublishMessage(new MqttApplicationMessage
		{
			Topic = writeTopic,
			Payload = ((size == data.Length) ? data : data.SelectMiddle(offset, size))
		});
	}

	public override OperateResult<byte[]> ReceiveMessage(INetMessage netMessage, byte[] sendValue, bool useActivePush = true, Action<long, long> reportProgress = null, Action<byte[]> logMessage = null)
	{
		DateTime now = DateTime.Now;
		MemoryStream ms = new MemoryStream();
		do
		{
			if (base.ReceiveTimeOut >= 0 && (DateTime.Now - now).TotalMilliseconds > (double)base.ReceiveTimeOut)
			{
				return new OperateResult<byte[]>(StringResources.Language.ReceiveDataTimeout + base.ReceiveTimeOut);
			}
			if (autoResetEvent.WaitOne(base.ReceiveTimeOut))
			{
				byte[] array = bufferQA;
				if (array != null && array.Length != 0)
				{
					ms.Write(bufferQA);
					logMessage?.Invoke(bufferQA);
				}
				continue;
			}
			return new OperateResult<byte[]>(-10000, StringResources.Language.ReceiveDataTimeout + base.ReceiveTimeOut + " Received: " + ms.ToArray().ToHexString(' '));
		}
		while (netMessage != null && !CheckMessageComplete(netMessage, sendValue, ref ms));
		return OperateResult.CreateSuccessResult(ms.ToArray());
	}

	public override string ToString()
	{
		return $"PipeMqttClient[{mqttClient}]";
	}
}
