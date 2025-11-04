using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.Core.Pipe;
using HslCommunication.Core.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HslCommunication.MQTT;

public class MqttSyncClient : TcpNetCommunication
{
	private SoftIncrementCount incrementCount;

	private MqttConnectionOptions connectionOptions;

	private Encoding stringEncoding = Encoding.UTF8;

	private RSACryptoServiceProvider cryptoServiceProvider = null;

	private AesCryptography aesCryptography = null;

	public MqttConnectionOptions ConnectionOptions
	{
		get
		{
			return connectionOptions;
		}
		set
		{
			connectionOptions = value;
		}
	}

	public Encoding StringEncoding
	{
		get
		{
			return stringEncoding;
		}
		set
		{
			stringEncoding = value;
		}
	}

	public MqttSyncClient(MqttConnectionOptions options)
	{
		connectionOptions = options;
		IpAddress = options.IpAddress;
		Port = options.Port;
		incrementCount = new SoftIncrementCount(65535L, 1L);
		ConnectTimeOut = options.ConnectTimeout;
		base.ReceiveTimeOut = 60000;
	}

	public MqttSyncClient(string ipAddress, int port)
	{
		connectionOptions = new MqttConnectionOptions
		{
			IpAddress = ipAddress,
			Port = port
		};
		IpAddress = ipAddress;
		Port = port;
		incrementCount = new SoftIncrementCount(65535L, 1L);
		base.ReceiveTimeOut = 60000;
	}

	public MqttSyncClient(IPAddress ipAddress, int port)
	{
		connectionOptions = new MqttConnectionOptions
		{
			IpAddress = ipAddress.ToString(),
			Port = port
		};
		IpAddress = ipAddress.ToString();
		Port = port;
		incrementCount = new SoftIncrementCount(65535L, 1L);
	}

	private OperateResult InitializationMqttSocket(CommunicationPipe pipe, string protocol)
	{
		RSACryptoServiceProvider rsa = null;
		if (connectionOptions.UseRSAProvider)
		{
			cryptoServiceProvider = new RSACryptoServiceProvider();
			OperateResult operateResult = pipe.Send(MqttHelper.BuildMqttCommand(byte.MaxValue, null, HslSecurity.ByteEncrypt(cryptoServiceProvider.GetPEMPublicKey())).Content);
			if (!operateResult.IsSuccess)
			{
				return operateResult;
			}
			OperateResult<byte, byte[]> operateResult2 = MqttHelper.ReceiveMqttMessage(pipe, base.ReceiveTimeOut);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			try
			{
				byte[] publicKey = cryptoServiceProvider.DecryptLargeData(HslSecurity.ByteDecrypt(operateResult2.Content2));
				rsa = RSAHelper.CreateRsaProviderFromPublicKey(publicKey);
			}
			catch (Exception ex)
			{
				pipe?.CloseCommunication();
				return new OperateResult("RSA check failed: " + ex.Message);
			}
		}
		OperateResult<byte[]> operateResult3 = MqttHelper.BuildConnectMqttCommand(connectionOptions, protocol, rsa);
		if (!operateResult3.IsSuccess)
		{
			return operateResult3;
		}
		OperateResult operateResult4 = pipe.Send(operateResult3.Content);
		if (!operateResult4.IsSuccess)
		{
			return operateResult4;
		}
		OperateResult<byte, byte[]> operateResult5 = MqttHelper.ReceiveMqttMessage(pipe, base.ReceiveTimeOut);
		if (!operateResult5.IsSuccess)
		{
			return operateResult5;
		}
		OperateResult operateResult6 = MqttHelper.CheckConnectBack(operateResult5.Content1, operateResult5.Content2);
		if (!operateResult6.IsSuccess)
		{
			pipe?.CloseCommunication();
			return operateResult6;
		}
		if (connectionOptions.UseRSAProvider)
		{
			string key = Encoding.UTF8.GetString(cryptoServiceProvider.Decrypt(operateResult5.Content2.RemoveBegin(2), fOAEP: false));
			aesCryptography = new AesCryptography(key);
		}
		return OperateResult.CreateSuccessResult();
	}

	protected override OperateResult InitializationOnConnect()
	{
		OperateResult operateResult = InitializationMqttSocket(CommunicationPipe, "HUSL");
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		incrementCount.ResetCurrentValue();
		return OperateResult.CreateSuccessResult();
	}

	private async Task<OperateResult> InitializationMqttSocketAsync(CommunicationPipe pipe, string protocol)
	{
		RSACryptoServiceProvider rsa = null;
		if (connectionOptions.UseRSAProvider)
		{
			cryptoServiceProvider = new RSACryptoServiceProvider();
			OperateResult sendKey = await pipe.SendAsync(MqttHelper.BuildMqttCommand(byte.MaxValue, null, HslSecurity.ByteEncrypt(cryptoServiceProvider.GetPEMPublicKey())).Content);
			if (!sendKey.IsSuccess)
			{
				return sendKey;
			}
			OperateResult<byte, byte[]> key2 = await MqttHelper.ReceiveMqttMessageAsync(pipe, base.ReceiveTimeOut);
			if (!key2.IsSuccess)
			{
				return key2;
			}
			try
			{
				byte[] serverPublicToken = cryptoServiceProvider.DecryptLargeData(HslSecurity.ByteDecrypt(key2.Content2));
				rsa = RSAHelper.CreateRsaProviderFromPublicKey(serverPublicToken);
			}
			catch (Exception ex)
			{
				pipe?.CloseCommunication();
				return new OperateResult("RSA check failed: " + ex.Message);
			}
		}
		OperateResult<byte[]> command = MqttHelper.BuildConnectMqttCommand(connectionOptions, protocol, rsa);
		if (!command.IsSuccess)
		{
			return command;
		}
		OperateResult send = await pipe.SendAsync(command.Content);
		if (!send.IsSuccess)
		{
			return send;
		}
		OperateResult<byte, byte[]> receive = await MqttHelper.ReceiveMqttMessageAsync(pipe, base.ReceiveTimeOut);
		if (!receive.IsSuccess)
		{
			return receive;
		}
		OperateResult check = MqttHelper.CheckConnectBack(receive.Content1, receive.Content2);
		if (!check.IsSuccess)
		{
			pipe?.CloseCommunication();
			return check;
		}
		if (connectionOptions.UseRSAProvider)
		{
			string key3 = Encoding.UTF8.GetString(cryptoServiceProvider.Decrypt(receive.Content2.RemoveBegin(2), fOAEP: false));
			aesCryptography = new AesCryptography(key3);
		}
		return OperateResult.CreateSuccessResult();
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync()
	{
		OperateResult ini = await InitializationMqttSocketAsync(CommunicationPipe, "HUSL");
		if (!ini.IsSuccess)
		{
			return ini;
		}
		incrementCount.ResetCurrentValue();
		return OperateResult.CreateSuccessResult();
	}

	public override OperateResult<byte[]> ReadFromCoreServer(CommunicationPipe pipe, byte[] send, bool hasResponseData = true, bool usePackHeader = true)
	{
		OperateResult<byte, byte[]> operateResult = ReadMqttFromCoreServer(pipe, send, null, null, null);
		if (operateResult.IsSuccess)
		{
			return OperateResult.CreateSuccessResult(operateResult.Content2);
		}
		return OperateResult.CreateFailedResult<byte[]>(operateResult);
	}

	private OperateResult<byte, byte[]> ReadMqttFromCoreServer(CommunicationPipe pipe, byte[] send, Action<long, long> sendProgress, Action<string, string> handleProgress, Action<long, long> receiveProgress)
	{
		OperateResult operateResult = pipe.Send(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte, byte[]>(operateResult);
		}
		long num;
		long num2;
		do
		{
			OperateResult<byte, byte[]> operateResult2 = MqttHelper.ReceiveMqttMessage(pipe, base.ReceiveTimeOut);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
			OperateResult<string, byte[]> operateResult3 = MqttHelper.ExtraMqttReceiveData(operateResult2.Content1, operateResult2.Content2);
			if (!operateResult3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte, byte[]>(operateResult3);
			}
			if (operateResult3.Content2.Length != 16)
			{
				return new OperateResult<byte, byte[]>(StringResources.Language.ReceiveDataLengthTooShort);
			}
			num = BitConverter.ToInt64(operateResult3.Content2, 0);
			num2 = BitConverter.ToInt64(operateResult3.Content2, 8);
			sendProgress?.Invoke(num, num2);
		}
		while (num != num2);
		OperateResult<byte, byte[]> operateResult4;
		while (true)
		{
			operateResult4 = MqttHelper.ReceiveMqttMessage(pipe, base.ReceiveTimeOut, receiveProgress);
			if (!operateResult4.IsSuccess)
			{
				return operateResult4;
			}
			if (operateResult4.Content1 >> 4 != 15)
			{
				break;
			}
			OperateResult<string, byte[]> operateResult5 = MqttHelper.ExtraMqttReceiveData(operateResult4.Content1, operateResult4.Content2);
			handleProgress?.Invoke(operateResult5.Content1, Encoding.UTF8.GetString(operateResult5.Content2));
		}
		return OperateResult.CreateSuccessResult(operateResult4.Content1, operateResult4.Content2);
	}

	private OperateResult<byte[]> ReadMqttFromCoreServer(byte control, byte flags, byte[] variableHeader, byte[] payLoad, Action<long, long> sendProgress, Action<string, string> handleProgress, Action<long, long> receiveProgress)
	{
		OperateResult<byte[]> operateResult = new OperateResult<byte[]>();
		OperateResult operateResult2 = CommunicationPipe.CommunicationLock.EnterLock(base.ReceiveTimeOut);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult2);
		}
		try
		{
			OperateResult<bool> operateResult3 = CommunicationPipe.OpenCommunication();
			if (!operateResult3.IsSuccess)
			{
				CommunicationPipe.CommunicationLock.LeaveLock();
				return OperateResult.CreateFailedResult<byte[]>(operateResult3);
			}
			if (operateResult3.Content)
			{
				OperateResult operateResult4 = InitializationOnConnect();
				if (!operateResult4.IsSuccess)
				{
					CommunicationPipe.CommunicationLock.LeaveLock();
					return OperateResult.CreateFailedResult<byte[]>(operateResult4);
				}
			}
			OperateResult<byte[]> operateResult5 = MqttHelper.BuildMqttCommand(control, flags, variableHeader, payLoad, aesCryptography);
			if (!operateResult5.IsSuccess)
			{
				CommunicationPipe.CommunicationLock.LeaveLock();
				operateResult.CopyErrorFromOther(operateResult5);
				return operateResult;
			}
			OperateResult<byte, byte[]> operateResult6 = ReadMqttFromCoreServer(CommunicationPipe, operateResult5.Content, sendProgress, handleProgress, receiveProgress);
			if (operateResult6.IsSuccess)
			{
				if (operateResult6.Content1 >> 4 == 0)
				{
					OperateResult<string, byte[]> operateResult7 = MqttHelper.ExtraMqttReceiveData(operateResult6.Content1, operateResult6.Content2, aesCryptography);
					operateResult.IsSuccess = false;
					operateResult.ErrorCode = int.Parse(operateResult7.Content1);
					operateResult.Message = Encoding.UTF8.GetString(operateResult7.Content2);
				}
				else
				{
					operateResult.IsSuccess = operateResult6.IsSuccess;
					operateResult.Content = operateResult6.Content2;
					operateResult.Message = StringResources.Language.SuccessText;
				}
			}
			else
			{
				operateResult.CopyErrorFromOther(operateResult6);
			}
			ExtraAfterReadFromCoreServer(operateResult6);
			CommunicationPipe.CommunicationLock.LeaveLock();
		}
		catch
		{
			CommunicationPipe.CommunicationLock.LeaveLock();
			throw;
		}
		if (!CommunicationPipe.IsPersistentConnection)
		{
			ExtraOnDisconnect();
			CommunicationPipe.CloseCommunication();
		}
		return operateResult;
	}

	public override async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(CommunicationPipe pipe, byte[] send, bool hasResponseData = true, bool usePackHeader = true)
	{
		OperateResult<byte, byte[]> read = await ReadMqttFromCoreServerAsync(pipe, send, null, null, null).ConfigureAwait(continueOnCapturedContext: false);
		if (read.IsSuccess)
		{
			return OperateResult.CreateSuccessResult(read.Content2);
		}
		return OperateResult.CreateFailedResult<byte[]>(read);
	}

	private async Task<OperateResult<byte, byte[]>> ReadMqttFromCoreServerAsync(CommunicationPipe pipe, byte[] send, Action<long, long> sendProgress, Action<string, string> handleProgress, Action<long, long> receiveProgress)
	{
		OperateResult sendResult = await pipe.SendAsync(send).ConfigureAwait(continueOnCapturedContext: false);
		if (!sendResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte, byte[]>(sendResult);
		}
		long already;
		long total;
		do
		{
			OperateResult<byte, byte[]> server_receive = await MqttHelper.ReceiveMqttMessageAsync(pipe, base.ReceiveTimeOut).ConfigureAwait(continueOnCapturedContext: false);
			if (!server_receive.IsSuccess)
			{
				return server_receive;
			}
			OperateResult<string, byte[]> server_back = MqttHelper.ExtraMqttReceiveData(server_receive.Content1, server_receive.Content2);
			if (!server_back.IsSuccess)
			{
				return OperateResult.CreateFailedResult<byte, byte[]>(server_back);
			}
			if (server_back.Content2.Length != 16)
			{
				return new OperateResult<byte, byte[]>(StringResources.Language.ReceiveDataLengthTooShort);
			}
			already = BitConverter.ToInt64(server_back.Content2, 0);
			total = BitConverter.ToInt64(server_back.Content2, 8);
			sendProgress?.Invoke(already, total);
		}
		while (already != total);
		OperateResult<byte, byte[]> receive;
		while (true)
		{
			receive = await MqttHelper.ReceiveMqttMessageAsync(pipe, base.ReceiveTimeOut, receiveProgress).ConfigureAwait(continueOnCapturedContext: false);
			if (!receive.IsSuccess)
			{
				return receive;
			}
			if (receive.Content1 >> 4 != 15)
			{
				break;
			}
			OperateResult<string, byte[]> extra = MqttHelper.ExtraMqttReceiveData(receive.Content1, receive.Content2);
			handleProgress?.Invoke(extra.Content1, Encoding.UTF8.GetString(extra.Content2));
		}
		return OperateResult.CreateSuccessResult(receive.Content1, receive.Content2);
	}

	private async Task<OperateResult<byte[]>> ReadMqttFromCoreServerAsync(byte control, byte flags, byte[] variableHeader, byte[] payLoad, Action<long, long> sendProgress, Action<string, string> handleProgress, Action<long, long> receiveProgress)
	{
		OperateResult<byte[]> result = new OperateResult<byte[]>();
		OperateResult enter = await Task.Run(() => CommunicationPipe.CommunicationLock.EnterLock(base.ReceiveTimeOut)).ConfigureAwait(continueOnCapturedContext: false);
		if (!enter.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(enter);
		}
		try
		{
			OperateResult<bool> pipe = await CommunicationPipe.OpenCommunicationAsync().ConfigureAwait(continueOnCapturedContext: false);
			if (!pipe.IsSuccess)
			{
				CommunicationPipe.CommunicationLock.LeaveLock();
				return OperateResult.CreateFailedResult<byte[]>(pipe);
			}
			if (pipe.Content)
			{
				OperateResult ini = await InitializationOnConnectAsync().ConfigureAwait(continueOnCapturedContext: false);
				if (!ini.IsSuccess)
				{
					CommunicationPipe.CommunicationLock.LeaveLock();
					return OperateResult.CreateFailedResult<byte[]>(ini);
				}
			}
			OperateResult<byte[]> command = MqttHelper.BuildMqttCommand(control, flags, variableHeader, payLoad, aesCryptography);
			if (!command.IsSuccess)
			{
				CommunicationPipe.CommunicationLock.LeaveLock();
				result.CopyErrorFromOther(command);
				return result;
			}
			OperateResult<byte, byte[]> read = await ReadMqttFromCoreServerAsync(CommunicationPipe, command.Content, sendProgress, handleProgress, receiveProgress).ConfigureAwait(continueOnCapturedContext: false);
			if (read.IsSuccess)
			{
				if (read.Content1 >> 4 == 0)
				{
					OperateResult<string, byte[]> extra = MqttHelper.ExtraMqttReceiveData(read.Content1, read.Content2, aesCryptography);
					result.IsSuccess = false;
					result.ErrorCode = int.Parse(extra.Content1);
					result.Message = Encoding.UTF8.GetString(extra.Content2);
				}
				else
				{
					result.IsSuccess = read.IsSuccess;
					result.Content = read.Content2;
					result.Message = StringResources.Language.SuccessText;
				}
			}
			else
			{
				result.CopyErrorFromOther(read);
			}
			ExtraAfterReadFromCoreServer(read);
			CommunicationPipe.CommunicationLock.LeaveLock();
		}
		catch
		{
			CommunicationPipe.CommunicationLock.LeaveLock();
			throw;
		}
		if (!CommunicationPipe.IsPersistentConnection)
		{
			ExtraOnDisconnect();
			CommunicationPipe.CloseCommunication();
		}
		return result;
	}

	public OperateResult<string, byte[]> Read(string topic, byte[] payload, Action<long, long> sendProgress = null, Action<string, string> handleProgress = null, Action<long, long> receiveProgress = null)
	{
		OperateResult<byte[]> operateResult = ReadMqttFromCoreServer(3, 0, MqttHelper.BuildSegCommandByString(topic), payload, sendProgress, handleProgress, receiveProgress);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string, byte[]>(operateResult);
		}
		return MqttHelper.ExtraMqttReceiveData(3, operateResult.Content, aesCryptography);
	}

	public OperateResult<string, string> ReadString(string topic, string payload, Action<long, long> sendProgress = null, Action<string, string> handleProgress = null, Action<long, long> receiveProgress = null)
	{
		OperateResult<string, byte[]> operateResult = Read(topic, string.IsNullOrEmpty(payload) ? null : stringEncoding.GetBytes(payload), sendProgress, handleProgress, receiveProgress);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string, string>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content1, stringEncoding.GetString(operateResult.Content2));
	}

	public OperateResult<T> ReadRpc<T>(string topic, string payload)
	{
		OperateResult<string, string> operateResult = ReadString(topic, payload);
		if (!operateResult.IsSuccess)
		{
			return operateResult.ConvertFailed<T>();
		}
		try
		{
			return OperateResult.CreateSuccessResult(JsonConvert.DeserializeObject<T>(operateResult.Content2));
		}
		catch (Exception ex)
		{
			return new OperateResult<T>("JSON failed: " + ex.Message + Environment.NewLine + "Source Data: " + operateResult.Content2);
		}
	}

	public OperateResult<T> ReadRpc<T>(string topic, object payload)
	{
		return ReadRpc<T>(topic, (payload == null) ? "{}" : payload.ToJsonString());
	}

	public OperateResult<MqttRpcApiInfo[]> ReadRpcApis()
	{
		OperateResult<byte[]> operateResult = ReadMqttFromCoreServer(8, 0, MqttHelper.BuildSegCommandByString(""), null, null, null, null);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<MqttRpcApiInfo[]>(operateResult);
		}
		OperateResult<string, byte[]> operateResult2 = MqttHelper.ExtraMqttReceiveData(3, operateResult.Content, aesCryptography);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<MqttRpcApiInfo[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(JArray.Parse(Encoding.UTF8.GetString(operateResult2.Content2)).ToObject<MqttRpcApiInfo[]>());
	}

	public OperateResult<long[]> ReadRpcApiLog(string api)
	{
		OperateResult<byte[]> operateResult = ReadMqttFromCoreServer(6, 0, MqttHelper.BuildSegCommandByString(api), null, null, null, null);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<long[]>(operateResult);
		}
		OperateResult<string, byte[]> operateResult2 = MqttHelper.ExtraMqttReceiveData(3, operateResult.Content, aesCryptography);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<long[]>(operateResult2);
		}
		string value = Encoding.UTF8.GetString(operateResult2.Content2);
		return OperateResult.CreateSuccessResult(value.ToStringArray<long>());
	}

	public OperateResult<string[]> ReadRetainTopics()
	{
		OperateResult<byte[]> operateResult = ReadMqttFromCoreServer(4, 0, MqttHelper.BuildSegCommandByString(""), null, null, null, null);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(operateResult);
		}
		OperateResult<string, byte[]> operateResult2 = MqttHelper.ExtraMqttReceiveData(3, operateResult.Content, aesCryptography);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(HslProtocol.UnPackStringArrayFromByte(operateResult2.Content2));
	}

	public OperateResult<MqttClientApplicationMessage> ReadTopicPayload(string topic, Action<long, long> receiveProgress = null)
	{
		OperateResult<byte[]> operateResult = ReadMqttFromCoreServer(5, 0, MqttHelper.BuildSegCommandByString(topic), null, null, null, receiveProgress);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<MqttClientApplicationMessage>(operateResult);
		}
		OperateResult<string, byte[]> operateResult2 = MqttHelper.ExtraMqttReceiveData(3, operateResult.Content, aesCryptography);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<MqttClientApplicationMessage>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(JObject.Parse(Encoding.UTF8.GetString(operateResult2.Content2)).ToObject<MqttClientApplicationMessage>());
	}

	public OperateResult<MqttSessionInfo[]> ReadSessions()
	{
		OperateResult<byte[]> operateResult = ReadMqttFromCoreServer(11, 0, MqttHelper.BuildSegCommandByString(""), null, null, null, null);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<MqttSessionInfo[]>(operateResult);
		}
		OperateResult<string, byte[]> operateResult2 = MqttHelper.ExtraMqttReceiveData(3, operateResult.Content, aesCryptography);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<MqttSessionInfo[]>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(JArray.Parse(Encoding.UTF8.GetString(operateResult2.Content2)).ToObject<MqttSessionInfo[]>());
	}

	public async Task<OperateResult<string, byte[]>> ReadAsync(string topic, byte[] payload, Action<long, long> sendProgress = null, Action<string, string> handleProgress = null, Action<long, long> receiveProgress = null)
	{
		OperateResult<byte[]> read = await ReadMqttFromCoreServerAsync(3, 0, MqttHelper.BuildSegCommandByString(topic), payload, sendProgress, handleProgress, receiveProgress);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string, byte[]>(read);
		}
		return MqttHelper.ExtraMqttReceiveData(3, read.Content, aesCryptography);
	}

	public async Task<OperateResult<string, string>> ReadStringAsync(string topic, string payload, Action<long, long> sendProgress = null, Action<string, string> handleProgress = null, Action<long, long> receiveProgress = null)
	{
		OperateResult<string, byte[]> read = await ReadAsync(topic, string.IsNullOrEmpty(payload) ? null : stringEncoding.GetBytes(payload), sendProgress, handleProgress, receiveProgress);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string, string>(read);
		}
		return OperateResult.CreateSuccessResult(read.Content1, stringEncoding.GetString(read.Content2));
	}

	public async Task<OperateResult<T>> ReadRpcAsync<T>(string topic, string payload)
	{
		OperateResult<string, string> read = await ReadStringAsync(topic, payload);
		if (!read.IsSuccess)
		{
			return read.ConvertFailed<T>();
		}
		try
		{
			return OperateResult.CreateSuccessResult(JsonConvert.DeserializeObject<T>(read.Content2));
		}
		catch (Exception ex)
		{
			return new OperateResult<T>("JSON failed: " + ex.Message + Environment.NewLine + "Source Data: " + read.Content2);
		}
	}

	public async Task<OperateResult<T>> ReadRpcAsync<T>(string topic, object payload)
	{
		return await ReadRpcAsync<T>(topic, (payload == null) ? "{}" : payload.ToJsonString());
	}

	public async Task<OperateResult<MqttRpcApiInfo[]>> ReadRpcApisAsync()
	{
		OperateResult<byte[]> read = await ReadMqttFromCoreServerAsync(8, 0, MqttHelper.BuildSegCommandByString(""), null, null, null, null);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<MqttRpcApiInfo[]>(read);
		}
		OperateResult<string, byte[]> mqtt = MqttHelper.ExtraMqttReceiveData(3, read.Content, aesCryptography);
		if (!mqtt.IsSuccess)
		{
			return OperateResult.CreateFailedResult<MqttRpcApiInfo[]>(mqtt);
		}
		return OperateResult.CreateSuccessResult(JArray.Parse(Encoding.UTF8.GetString(mqtt.Content2)).ToObject<MqttRpcApiInfo[]>());
	}

	public async Task<OperateResult<long[]>> ReadRpcApiLogAsync(string api)
	{
		OperateResult<byte[]> read = await ReadMqttFromCoreServerAsync(6, 0, MqttHelper.BuildSegCommandByString(api), null, null, null, null);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<long[]>(read);
		}
		OperateResult<string, byte[]> mqtt = MqttHelper.ExtraMqttReceiveData(3, read.Content, aesCryptography);
		if (!mqtt.IsSuccess)
		{
			return OperateResult.CreateFailedResult<long[]>(mqtt);
		}
		string content = Encoding.UTF8.GetString(mqtt.Content2);
		return OperateResult.CreateSuccessResult(content.ToStringArray<long>());
	}

	public async Task<OperateResult<string[]>> ReadRetainTopicsAsync()
	{
		OperateResult<byte[]> read = await ReadMqttFromCoreServerAsync(4, 0, MqttHelper.BuildSegCommandByString(""), null, null, null, null);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(read);
		}
		OperateResult<string, byte[]> mqtt = MqttHelper.ExtraMqttReceiveData(3, read.Content, aesCryptography);
		if (!mqtt.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(mqtt);
		}
		return OperateResult.CreateSuccessResult(HslProtocol.UnPackStringArrayFromByte(mqtt.Content2));
	}

	public async Task<OperateResult<MqttClientApplicationMessage>> ReadTopicPayloadAsync(string topic, Action<long, long> receiveProgress = null)
	{
		OperateResult<byte[]> read = await ReadMqttFromCoreServerAsync(5, 0, MqttHelper.BuildSegCommandByString(topic), null, null, null, receiveProgress);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<MqttClientApplicationMessage>(read);
		}
		OperateResult<string, byte[]> mqtt = MqttHelper.ExtraMqttReceiveData(3, read.Content, aesCryptography);
		if (!mqtt.IsSuccess)
		{
			return OperateResult.CreateFailedResult<MqttClientApplicationMessage>(mqtt);
		}
		return OperateResult.CreateSuccessResult(JObject.Parse(Encoding.UTF8.GetString(mqtt.Content2)).ToObject<MqttClientApplicationMessage>());
	}

	private OperateResult<CommunicationPipe> ConnectMqttFileServer(byte opCode, string groups, string[] fileNames)
	{
		PipeTcpNet pipeTcpNet = new PipeTcpNet(IpAddress, Port);
		pipeTcpNet.ConnectTimeOut = ConnectTimeOut;
		OperateResult operateResult = pipeTcpNet.OpenCommunication();
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<CommunicationPipe>(operateResult);
		}
		OperateResult operateResult2 = InitializationMqttSocket(pipeTcpNet, "FILE");
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<CommunicationPipe>(operateResult2);
		}
		OperateResult operateResult3 = pipeTcpNet.Send(MqttHelper.BuildMqttCommand(opCode, null, HslProtocol.PackStringArrayToByte(string.IsNullOrEmpty(groups) ? null : groups.Split(new char[2] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries))).Content);
		if (!operateResult3.IsSuccess)
		{
			return OperateResult.CreateFailedResult<CommunicationPipe>(operateResult3);
		}
		OperateResult operateResult4 = pipeTcpNet.Send(MqttHelper.BuildMqttCommand(opCode, null, HslProtocol.PackStringArrayToByte(fileNames)).Content);
		if (!operateResult4.IsSuccess)
		{
			return OperateResult.CreateFailedResult<CommunicationPipe>(operateResult4);
		}
		OperateResult<byte, byte[]> operateResult5 = MqttHelper.ReceiveMqttMessage(pipeTcpNet, base.ReceiveTimeOut);
		if (!operateResult5.IsSuccess)
		{
			return OperateResult.CreateFailedResult<CommunicationPipe>(operateResult5);
		}
		if (operateResult5.Content1 == 0)
		{
			pipeTcpNet.CloseCommunication();
			return new OperateResult<CommunicationPipe>(Encoding.UTF8.GetString(operateResult5.Content2));
		}
		return OperateResult.CreateSuccessResult((CommunicationPipe)pipeTcpNet);
	}

	private OperateResult DownloadFileBase(string groups, string fileName, Action<long, long> processReport, object source, HslCancelToken cancelToken)
	{
		OperateResult<CommunicationPipe> operateResult = ConnectMqttFileServer(101, groups, new string[1] { fileName });
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		OperateResult operateResult2 = MqttHelper.ReceiveMqttFile(operateResult.Content, source, processReport, aesCryptography, cancelToken);
		if (!operateResult2.IsSuccess)
		{
			return operateResult2;
		}
		operateResult.Content?.CloseCommunication();
		return OperateResult.CreateSuccessResult();
	}

	private OperateResult<T> OperateMqttFileContent<T>(byte opCode, string groups, string[] fileNames, Func<OperateResult<byte, byte[]>, T> trans)
	{
		OperateResult<CommunicationPipe> operateResult = ConnectMqttFileServer(opCode, groups, fileNames);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T>(operateResult);
		}
		OperateResult<byte, byte[]> operateResult2 = MqttHelper.ReceiveMqttMessage(operateResult.Content, 60000);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T>(operateResult2);
		}
		operateResult.Content?.CloseCommunication();
		if (operateResult2.Content1 == 0)
		{
			return new OperateResult<T>((operateResult2.Content2 == null) ? string.Empty : Encoding.UTF8.GetString(operateResult2.Content2));
		}
		try
		{
			return OperateResult.CreateSuccessResult(trans(operateResult2));
		}
		catch (Exception ex)
		{
			return new OperateResult<T>(ex.Message);
		}
	}

	public OperateResult DownloadFile(string groups, string fileName, Action<long, long> processReport, string fileSaveName, HslCancelToken cancelToken = null)
	{
		return DownloadFileBase(groups, fileName, processReport, fileSaveName, cancelToken);
	}

	public OperateResult DownloadFile(string groups, string fileName, Action<long, long> processReport, Stream stream, HslCancelToken cancelToken = null)
	{
		return DownloadFileBase(groups, fileName, processReport, stream, cancelToken);
	}

	private OperateResult UploadFileBase(object source, string groups, string serverName, string fileTag, Action<long, long> processReport, HslCancelToken cancelToken)
	{
		OperateResult<CommunicationPipe> operateResult = ConnectMqttFileServer(102, groups, new string[1] { serverName });
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		if (source is string filename)
		{
			OperateResult operateResult2 = MqttHelper.SendMqttFile(operateResult.Content, filename, serverName, fileTag, processReport, aesCryptography, cancelToken);
			if (!operateResult2.IsSuccess)
			{
				return operateResult2;
			}
		}
		else
		{
			if (!(source is Stream stream))
			{
				operateResult.Content?.CloseCommunication();
				base.LogNet?.WriteError(ToString(), StringResources.Language.DataSourceFormatError);
				return new OperateResult(StringResources.Language.DataSourceFormatError);
			}
			OperateResult operateResult3 = MqttHelper.SendMqttFile(operateResult.Content, stream, serverName, fileTag, processReport, aesCryptography, cancelToken);
			if (!operateResult3.IsSuccess)
			{
				return operateResult3;
			}
		}
		OperateResult<byte, byte[]> operateResult4 = MqttHelper.ReceiveMqttMessage(operateResult.Content, 60000);
		if (!operateResult4.IsSuccess)
		{
			return operateResult4;
		}
		operateResult.Content?.CloseCommunication();
		return (operateResult4.Content1 != 0) ? OperateResult.CreateSuccessResult() : new OperateResult(Encoding.UTF8.GetString(operateResult4.Content2));
	}

	public OperateResult UploadFile(string fileName, string groups, string serverName, string fileTag, Action<long, long> processReport, HslCancelToken cancelToken = null)
	{
		if (!File.Exists(fileName))
		{
			return new OperateResult(StringResources.Language.FileNotExist);
		}
		return UploadFileBase(fileName, groups, serverName, fileTag, processReport, cancelToken);
	}

	public OperateResult UploadFile(string fileName, string groups, string fileTag, Action<long, long> processReport, HslCancelToken cancelToken = null)
	{
		if (!File.Exists(fileName))
		{
			return new OperateResult(StringResources.Language.FileNotExist);
		}
		FileInfo fileInfo = new FileInfo(fileName);
		return UploadFileBase(fileName, groups, fileInfo.Name, fileTag, processReport, cancelToken);
	}

	public OperateResult UploadFile(Stream stream, string groups, string serverName, string fileTag, Action<long, long> processReport, HslCancelToken cancelToken = null)
	{
		return UploadFileBase(stream, groups, serverName, fileTag, processReport, cancelToken);
	}

	private OperateResult<T[]> DownloadStringArrays<T>(byte protocol, string groups, string[] fileNames)
	{
		return OperateMqttFileContent(protocol, groups, fileNames, (OperateResult<byte, byte[]> m) => JArray.Parse(Encoding.UTF8.GetString(m.Content2)).ToObject<T[]>());
	}

	public OperateResult<GroupFileItem[]> DownloadPathFileNames(string groups)
	{
		return DownloadStringArrays<GroupFileItem>(105, groups, null);
	}

	public OperateResult<string[]> DownloadPathFolders(string groups)
	{
		return DownloadStringArrays<string>(106, groups, null);
	}

	public OperateResult<bool> IsFileExists(string groups, string fileName)
	{
		return OperateMqttFileContent(107, groups, new string[1] { fileName }, (OperateResult<byte, byte[]> m) => m.Content1 == 1);
	}

	public OperateResult DeleteFile(string groups, string fileName)
	{
		return DeleteFile(groups, new string[1] { fileName });
	}

	public OperateResult DeleteFile(string groups, string[] fileNames)
	{
		return OperateMqttFileContent(103, groups, fileNames, (OperateResult<byte, byte[]> m) => m.Content1);
	}

	public OperateResult DeleteFolderFiles(string groups)
	{
		return OperateMqttFileContent(110, groups, null, (OperateResult<byte, byte[]> m) => m.Content1);
	}

	public OperateResult DeleteFolder(string groups)
	{
		return OperateMqttFileContent(104, groups, null, (OperateResult<byte, byte[]> m) => m.Content1);
	}

	public OperateResult RenameFolder(string groups, string newName)
	{
		return OperateMqttFileContent(111, groups, new string[1] { newName }, (OperateResult<byte, byte[]> m) => m.Content1);
	}

	public OperateResult<GroupFileInfo> GetGroupFileInfo(string groups)
	{
		return OperateMqttFileContent(108, groups, null, (OperateResult<byte, byte[]> m) => JObject.Parse(Encoding.UTF8.GetString(m.Content2)).ToObject<GroupFileInfo>());
	}

	public OperateResult<GroupFileInfo[]> GetSubGroupFileInfos(string groups, bool withLastFileInfo = false)
	{
		return OperateMqttFileContent(109, groups, (!withLastFileInfo) ? null : new string[1] { "1" }, (OperateResult<byte, byte[]> m) => JArray.Parse(Encoding.UTF8.GetString(m.Content2)).ToObject<GroupFileInfo[]>());
	}

	private async Task<OperateResult<CommunicationPipe>> ConnectMqttFileServerAsync(byte opCode, string groups, string[] fileNames)
	{
		PipeTcpNet pipe = new PipeTcpNet(IpAddress, Port)
		{
			ConnectTimeOut = ConnectTimeOut
		};
		OperateResult open = await pipe.OpenCommunicationAsync();
		if (!open.IsSuccess)
		{
			return OperateResult.CreateFailedResult<CommunicationPipe>(open);
		}
		OperateResult ini = await InitializationMqttSocketAsync(pipe, "FILE");
		if (!ini.IsSuccess)
		{
			return OperateResult.CreateFailedResult<CommunicationPipe>(ini);
		}
		OperateResult sendClass = await pipe.SendAsync(MqttHelper.BuildMqttCommand(opCode, null, HslProtocol.PackStringArrayToByte(string.IsNullOrEmpty(groups) ? null : groups.Split(new char[2] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries))).Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!sendClass.IsSuccess)
		{
			return OperateResult.CreateFailedResult<CommunicationPipe>(sendClass);
		}
		OperateResult sendString = await pipe.SendAsync(MqttHelper.BuildMqttCommand(opCode, null, HslProtocol.PackStringArrayToByte(fileNames)).Content).ConfigureAwait(continueOnCapturedContext: false);
		if (!sendString.IsSuccess)
		{
			return OperateResult.CreateFailedResult<CommunicationPipe>(sendString);
		}
		OperateResult<byte, byte[]> legal = await MqttHelper.ReceiveMqttMessageAsync(pipe, 60000).ConfigureAwait(continueOnCapturedContext: false);
		if (!legal.IsSuccess)
		{
			return OperateResult.CreateFailedResult<CommunicationPipe>(legal);
		}
		if (legal.Content1 == 0)
		{
			pipe?.CloseCommunication();
			return new OperateResult<CommunicationPipe>(Encoding.UTF8.GetString(legal.Content2));
		}
		return OperateResult.CreateSuccessResult((CommunicationPipe)pipe);
	}

	private async Task<OperateResult> DownloadFileBaseAsync(string groups, string fileName, Action<long, long> processReport, object source, HslCancelToken cancelToken)
	{
		OperateResult<CommunicationPipe> socketResult = await ConnectMqttFileServerAsync(101, groups, new string[1] { fileName }).ConfigureAwait(continueOnCapturedContext: false);
		if (!socketResult.IsSuccess)
		{
			return socketResult;
		}
		OperateResult result = await MqttHelper.ReceiveMqttFileAsync(socketResult.Content, source, processReport, aesCryptography, cancelToken).ConfigureAwait(continueOnCapturedContext: false);
		if (!result.IsSuccess)
		{
			return result;
		}
		socketResult.Content?.CloseCommunication();
		return OperateResult.CreateSuccessResult();
	}

	private async Task<OperateResult<T>> OperateMqttFileContentAsync<T>(byte opCode, string groups, string[] fileNames, Func<OperateResult<byte, byte[]>, T> trans)
	{
		OperateResult<CommunicationPipe> socketResult = await ConnectMqttFileServerAsync(opCode, groups, fileNames).ConfigureAwait(continueOnCapturedContext: false);
		if (!socketResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T>(socketResult);
		}
		OperateResult<byte, byte[]> receive = await MqttHelper.ReceiveMqttMessageAsync(socketResult.Content, 60000);
		if (!receive.IsSuccess)
		{
			return OperateResult.CreateFailedResult<T>(receive);
		}
		socketResult.Content?.CloseCommunication();
		if (receive.Content1 == 0)
		{
			return new OperateResult<T>((receive.Content2 == null) ? string.Empty : Encoding.UTF8.GetString(receive.Content2));
		}
		try
		{
			return OperateResult.CreateSuccessResult(trans(receive));
		}
		catch (Exception ex)
		{
			return new OperateResult<T>(ex.Message);
		}
	}

	public async Task<OperateResult> DownloadFileAsync(string groups, string fileName, Action<long, long> processReport, string fileSaveName, HslCancelToken cancelToken = null)
	{
		return await DownloadFileBaseAsync(groups, fileName, processReport, fileSaveName, cancelToken);
	}

	public async Task<OperateResult> DownloadFileAsync(string groups, string fileName, Action<long, long> processReport, Stream stream, HslCancelToken cancelToken = null)
	{
		return await DownloadFileBaseAsync(groups, fileName, processReport, stream, cancelToken);
	}

	private async Task<OperateResult> UploadFileBaseAsync(object source, string groups, string serverName, string fileTag, Action<long, long> processReport, HslCancelToken cancelToken)
	{
		OperateResult<CommunicationPipe> socketResult = await ConnectMqttFileServerAsync(102, groups, new string[1] { serverName }).ConfigureAwait(continueOnCapturedContext: false);
		if (!socketResult.IsSuccess)
		{
			return socketResult;
		}
		if (source is string fileName)
		{
			OperateResult result = await MqttHelper.SendMqttFileAsync(socketResult.Content, fileName, serverName, fileTag, processReport, aesCryptography, cancelToken).ConfigureAwait(continueOnCapturedContext: false);
			if (!result.IsSuccess)
			{
				return result;
			}
		}
		else
		{
			if (!(source is Stream stream))
			{
				socketResult.Content?.CloseCommunication();
				base.LogNet?.WriteError(ToString(), StringResources.Language.DataSourceFormatError);
				return new OperateResult(StringResources.Language.DataSourceFormatError);
			}
			OperateResult result2 = await MqttHelper.SendMqttFileAsync(socketResult.Content, stream, serverName, fileTag, processReport, aesCryptography, cancelToken).ConfigureAwait(continueOnCapturedContext: false);
			if (!result2.IsSuccess)
			{
				return result2;
			}
		}
		OperateResult<byte, byte[]> resultCheck = await MqttHelper.ReceiveMqttMessageAsync(socketResult.Content, 60000).ConfigureAwait(continueOnCapturedContext: false);
		if (!resultCheck.IsSuccess)
		{
			return resultCheck;
		}
		socketResult.Content?.CloseCommunication();
		return (resultCheck.Content1 != 0) ? OperateResult.CreateSuccessResult() : new OperateResult(Encoding.UTF8.GetString(resultCheck.Content2));
	}

	public async Task<OperateResult> UploadFileAsync(string fileName, string groups, string serverName, string fileTag, Action<long, long> processReport, HslCancelToken cancelToken = null)
	{
		if (!File.Exists(fileName))
		{
			return new OperateResult(StringResources.Language.FileNotExist);
		}
		return await UploadFileBaseAsync(fileName, groups, serverName, fileTag, processReport, cancelToken);
	}

	public async Task<OperateResult> UploadFileAsync(string fileName, string groups, string fileTag, Action<long, long> processReport, HslCancelToken cancelToken = null)
	{
		if (!File.Exists(fileName))
		{
			return new OperateResult(StringResources.Language.FileNotExist);
		}
		FileInfo fileInfo = new FileInfo(fileName);
		return await UploadFileBaseAsync(fileName, groups, fileInfo.Name, fileTag, processReport, cancelToken);
	}

	public async Task<OperateResult> UploadFileAsync(Stream stream, string groups, string serverName, string fileTag, Action<long, long> processReport, HslCancelToken cancelToken = null)
	{
		return await UploadFileBaseAsync(stream, groups, serverName, fileTag, processReport, cancelToken);
	}

	private async Task<OperateResult<T[]>> DownloadStringArraysAsync<T>(byte protocol, string groups, string[] fileNames)
	{
		return await OperateMqttFileContentAsync(protocol, groups, fileNames, (OperateResult<byte, byte[]> m) => JArray.Parse(Encoding.UTF8.GetString(m.Content2)).ToObject<T[]>());
	}

	public async Task<OperateResult<GroupFileItem[]>> DownloadPathFileNamesAsync(string groups)
	{
		return await DownloadStringArraysAsync<GroupFileItem>(105, groups, null);
	}

	public async Task<OperateResult<string[]>> DownloadPathFoldersAsync(string groups)
	{
		return await DownloadStringArraysAsync<string>(106, groups, null);
	}

	public async Task<OperateResult<bool>> IsFileExistsAsync(string groups, string fileName)
	{
		return await OperateMqttFileContentAsync(107, groups, new string[1] { fileName }, (OperateResult<byte, byte[]> m) => m.Content1 == 1);
	}

	public async Task<OperateResult> DeleteFileAsync(string groups, string fileName)
	{
		return await DeleteFileAsync(groups, new string[1] { fileName });
	}

	public async Task<OperateResult> DeleteFileAsync(string groups, string[] fileNames)
	{
		return await OperateMqttFileContentAsync(103, groups, fileNames, (OperateResult<byte, byte[]> m) => m.Content1);
	}

	public async Task<OperateResult> DeleteFolderFilesAsync(string groups)
	{
		return await OperateMqttFileContentAsync(110, groups, null, (OperateResult<byte, byte[]> m) => m.Content1);
	}

	public async Task<OperateResult> DeleteFolderAsync(string groups)
	{
		return await OperateMqttFileContentAsync(104, groups, null, (OperateResult<byte, byte[]> m) => m.Content1);
	}

	public async Task<OperateResult> RenameFolderAsync(string groups, string newName)
	{
		return await OperateMqttFileContentAsync(111, groups, new string[1] { newName }, (OperateResult<byte, byte[]> m) => m.Content1);
	}

	public async Task<OperateResult<GroupFileInfo>> GetGroupFileInfoAsync(string groups)
	{
		return await OperateMqttFileContentAsync(108, groups, null, (OperateResult<byte, byte[]> m) => JObject.Parse(Encoding.UTF8.GetString(m.Content2)).ToObject<GroupFileInfo>());
	}

	public async Task<OperateResult<GroupFileInfo[]>> GetSubGroupFileInfosAsync(string groups, bool withLastFileInfo = false)
	{
		return await OperateMqttFileContentAsync(109, groups, (!withLastFileInfo) ? null : new string[1] { "1" }, (OperateResult<byte, byte[]> m) => JArray.Parse(Encoding.UTF8.GetString(m.Content2)).ToObject<GroupFileInfo[]>());
	}

	public override string ToString()
	{
		return $"MqttSyncClient[{connectionOptions.IpAddress}:{connectionOptions.Port}]";
	}
}
