using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.Core.Pipe;

namespace HslCommunication.WebSocket;

public class WebSocketQANet : TcpNetCommunication
{
	public WebSocketQANet(string ipAddress, int port)
	{
		IpAddress = HslHelper.GetIpAddressFromInput(ipAddress);
		Port = port;
	}

	protected override OperateResult InitializationOnConnect()
	{
		byte[] data = WebSocketHelper.BuildWsQARequest(IpAddress, Port);
		OperateResult operateResult = CommunicationPipe.Send(data);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult<byte[]> operateResult2 = CommunicationPipe.Receive(-1, 10000);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		return OperateResult.CreateSuccessResult();
	}

	public override OperateResult<byte[]> ReadFromCoreServer(CommunicationPipe pipe, byte[] send, bool hasResponseData = true, bool usePackHeader = true)
	{
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Send + " : " + SoftBasic.ByteToHexString(send, ' '));
		OperateResult operateResult = pipe.Send(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		if (base.ReceiveTimeOut < 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		OperateResult<WebSocketMessage> operateResult2 = WebSocketHelper.ReceiveWebSocketPayload(pipe);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		base.LogNet?.WriteDebug(ToString(), $"{StringResources.Language.Receive} : OpCode[{operateResult2.Content.OpCode}] Mask[{operateResult2.Content.HasMask}] {SoftBasic.ByteToHexString(operateResult2.Content.Payload, ' ')}");
		return OperateResult.CreateSuccessResult(operateResult2.Content.Payload);
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync()
	{
		byte[] command = WebSocketHelper.BuildWsQARequest(IpAddress, Port);
		OperateResult send = await CommunicationPipe.SendAsync(command);
		if (!send.IsSuccess)
		{
			return send;
		}
		OperateResult<byte[]> rece = await CommunicationPipe.ReceiveAsync(-1, 10000);
		if (!rece.IsSuccess)
		{
			return rece;
		}
		return OperateResult.CreateSuccessResult();
	}

	public override async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(CommunicationPipe pipe, byte[] send, bool hasResponseData = true, bool usePackHeader = true)
	{
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Send + " : " + SoftBasic.ByteToHexString(send, ' '));
		OperateResult sendResult = await pipe.SendAsync(send);
		if (!sendResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(sendResult);
		}
		if (base.ReceiveTimeOut < 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		OperateResult<WebSocketMessage> read = await WebSocketHelper.ReceiveWebSocketPayloadAsync(pipe).ConfigureAwait(continueOnCapturedContext: false);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		base.LogNet?.WriteDebug(ToString(), $"{StringResources.Language.Receive} : OpCode[{read.Content.OpCode}] Mask[{read.Content.HasMask}] {SoftBasic.ByteToHexString(read.Content.Payload, ' ')}");
		return OperateResult.CreateSuccessResult(read.Content.Payload);
	}

	public OperateResult<string> ReadFromServer(string payload)
	{
		return ByteTransformHelper.GetSuccessResultFromOther(ReadFromCoreServer(WebSocketHelper.WebScoketPackData(1, isMask: true, payload)), Encoding.UTF8.GetString);
	}

	public async Task<OperateResult<string>> ReadFromServerAsync(string payload)
	{
		return ByteTransformHelper.GetSuccessResultFromOther(await ReadFromCoreServerAsync(WebSocketHelper.WebScoketPackData(1, isMask: true, payload)), Encoding.UTF8.GetString);
	}

	public override string ToString()
	{
		return $"WebSocketQANet[{IpAddress}:{Port}]";
	}
}
