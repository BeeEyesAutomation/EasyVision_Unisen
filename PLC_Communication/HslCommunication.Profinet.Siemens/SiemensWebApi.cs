using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Device;
using HslCommunication.Reflection;
using Newtonsoft.Json.Linq;

namespace HslCommunication.Profinet.Siemens;

public class SiemensWebApi : DeviceWebApi
{
	private string rawUrl = "api/jsonrpc";

	private string token = string.Empty;

	private SoftIncrementCount incrementCount = new SoftIncrementCount(65535L, 1L);

	public string Token
	{
		get
		{
			return token;
		}
		set
		{
			token = value;
		}
	}

	public SiemensWebApi()
		: this("127.0.0.1")
	{
	}

	public SiemensWebApi(string ipAddress, int port = 443)
		: base(ipAddress, port)
	{
		base.WordLength = 2;
		base.UseHttps = true;
		base.DefaultContentType = "application/json";
		base.ByteTransform = new ReverseBytesTransform();
	}

	protected override void AddRequestHeaders(HttpContentHeaders headers)
	{
		if (!string.IsNullOrEmpty(token))
		{
			headers.Add("X-Auth-Token", token);
		}
	}

	public OperateResult ConnectServer()
	{
		JArray jArray = BuildConnectBody(incrementCount.GetCurrentValue(), base.UserName, base.Password);
		OperateResult<string> operateResult = Post(rawUrl, jArray.ToString());
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return CheckLoginResult(operateResult.Content);
	}

	public OperateResult ConnectClose()
	{
		return Logout();
	}

	public async Task<OperateResult> ConnectServerAsync()
	{
		OperateResult<string> read = await PostAsync(body: BuildConnectBody(incrementCount.GetCurrentValue(), base.UserName, base.Password).ToString(), rawUrl: rawUrl);
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckLoginResult(read.Content);
	}

	public async Task<OperateResult> ConnectCloseAsync()
	{
		return await LogoutAsync();
	}

	private OperateResult CheckLoginResult(string response)
	{
		try
		{
			JArray jArray = JArray.Parse(response);
			JObject jObject = (JObject)jArray[0];
			OperateResult operateResult = CheckErrorResult(jObject);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<JToken>(operateResult);
			}
			if (jObject.ContainsKey("result"))
			{
				JObject jObject2 = jObject["result"] as JObject;
				token = jObject2.Value<string>("token");
				return OperateResult.CreateSuccessResult();
			}
			return new OperateResult("Can't find result key and none token, login failed:" + Environment.NewLine + response);
		}
		catch (Exception ex)
		{
			return new OperateResult("CheckLoginResult failed:" + ex.Message + Environment.NewLine + response);
		}
	}

	[HslMqttApi("ReadByteArray", "")]
	public override OperateResult<byte[]> Read(string address, ushort length)
	{
		JArray jArray = BuildReadRawBody(incrementCount.GetCurrentValue(), address);
		OperateResult<string> operateResult = Post(rawUrl, jArray.ToString());
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		return CheckReadRawResult(operateResult.Content);
	}

	[HslMqttApi("ReadByte", "")]
	public OperateResult<byte> ReadByte(string address)
	{
		return ByteTransformHelper.GetResultFromArray(Read(address, 1));
	}

	[HslMqttApi("ReadBoolArray", "")]
	public override OperateResult<bool[]> ReadBool(string address, ushort length)
	{
		OperateResult<byte[]> operateResult = Read(address, 0);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content.ToBoolArray().SelectBegin(length));
	}

	[HslMqttApi("WriteByteArray", "")]
	public override OperateResult Write(string address, byte[] value)
	{
		JArray jArray = BuildWriteRawBody(incrementCount.GetCurrentValue(), address, value);
		OperateResult<string> operateResult = Post(rawUrl, jArray.ToString());
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return CheckWriteResult(operateResult.Content);
	}

	[HslMqttApi("WriteByte", "")]
	public OperateResult Write(string address, byte value)
	{
		return Write(address, new byte[1] { value });
	}

	[HslMqttApi("WriteBoolArray", "")]
	public override OperateResult Write(string address, bool[] value)
	{
		byte[] value2 = value.ToByteArray();
		return Write(address, value2);
	}

	[HslMqttApi("ReadString", "读取字符串")]
	public override OperateResult<string> ReadString(string address, ushort length)
	{
		JArray jArray = BuildReadJTokenBody(incrementCount.GetCurrentValue(), address);
		OperateResult<string> operateResult = Post(rawUrl, jArray.ToString());
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		OperateResult<JToken> operateResult2 = CheckAndExtraOneJsonResult(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(operateResult2.Content.Value<string>());
	}

	[HslMqttApi("WriteString", "")]
	public override OperateResult Write(string address, string value)
	{
		JArray jArray = BuildWriteJTokenBody(incrementCount.GetCurrentValue(), address, new JValue(value));
		OperateResult<string> operateResult = Post(rawUrl, jArray.ToString());
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return CheckWriteResult(operateResult.Content);
	}

	public override async Task<OperateResult<byte[]>> ReadAsync(string address, ushort length)
	{
		OperateResult<string> read = await PostAsync(body: BuildReadRawBody(incrementCount.GetCurrentValue(), address).ToString(), rawUrl: rawUrl);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(read);
		}
		return CheckReadRawResult(read.Content);
	}

	public async Task<OperateResult<byte>> ReadByteAsync(string address)
	{
		return ByteTransformHelper.GetResultFromArray(await ReadAsync(address, 1));
	}

	public override async Task<OperateResult<bool[]>> ReadBoolAsync(string address, ushort length)
	{
		OperateResult<byte[]> read = await ReadAsync(address, 0);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<bool[]>(read);
		}
		return OperateResult.CreateSuccessResult(read.Content.ToBoolArray().SelectBegin(length));
	}

	public override async Task<OperateResult<string>> ReadStringAsync(string address, ushort length)
	{
		OperateResult<string> read = await PostAsync(body: BuildReadJTokenBody(incrementCount.GetCurrentValue(), address).ToString(), rawUrl: rawUrl);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		OperateResult<JToken> extra = CheckAndExtraOneJsonResult(read.Content);
		if (!extra.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(extra);
		}
		return OperateResult.CreateSuccessResult(extra.Content.Value<string>());
	}

	public override async Task<OperateResult> WriteAsync(string address, byte[] value)
	{
		OperateResult<string> read = await PostAsync(body: BuildWriteRawBody(incrementCount.GetCurrentValue(), address, value).ToString(), rawUrl: rawUrl);
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckWriteResult(read.Content);
	}

	public async Task<OperateResult> WriteAsync(string address, byte value)
	{
		return await WriteAsync(address, new byte[1] { value });
	}

	public override async Task<OperateResult> WriteAsync(string address, bool[] value)
	{
		byte[] buffer = value.ToByteArray();
		return await WriteAsync(address, buffer);
	}

	public override async Task<OperateResult> WriteAsync(string address, string value)
	{
		OperateResult<string> read = await PostAsync(body: BuildWriteJTokenBody(incrementCount.GetCurrentValue(), address, new JValue(value)).ToString(), rawUrl: rawUrl);
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckWriteResult(read.Content);
	}

	[HslMqttApi("读取当前PLC的操作模式，如果读取成功，结果将会是如下值之一：STOP, STARTUP, RUN, HOLD, -")]
	public OperateResult<string> ReadOperatingMode()
	{
		JArray jArray = BuildRequestBody("Plc.ReadOperatingMode", null, incrementCount.GetCurrentValue());
		OperateResult<string> operateResult = Post(rawUrl, jArray.ToString());
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		OperateResult<JToken> operateResult2 = CheckAndExtraOneJsonResult(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(operateResult2.Content.Value<string>());
	}

	public async Task<OperateResult<string>> ReadOperatingModeAsync()
	{
		OperateResult<string> read = await PostAsync(body: BuildRequestBody("Plc.ReadOperatingMode", null, incrementCount.GetCurrentValue()).ToString(), rawUrl: rawUrl);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		OperateResult<JToken> extra = CheckAndExtraOneJsonResult(read.Content);
		if (!extra.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(extra);
		}
		return OperateResult.CreateSuccessResult(extra.Content.Value<string>());
	}

	[HslMqttApi("ReadJTokens", "从PLC读取多个地址的数据信息，每个地址的数据类型可以不一致")]
	public OperateResult<JToken[]> Read(string[] address)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<JToken[]>(StringResources.Language.InsufficientPrivileges);
		}
		JArray jArray = BuildReadJTokenBody(incrementCount.GetCurrentValue(), address);
		OperateResult<string> operateResult = Post(rawUrl, jArray.ToString());
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<JToken[]>(operateResult);
		}
		return CheckAndExtraJsonResult(operateResult.Content);
	}

	public async Task<OperateResult<JToken[]>> ReadAsync(string[] address)
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<JToken[]>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<string> read = await PostAsync(body: BuildReadJTokenBody(incrementCount.GetCurrentValue(), address).ToString(), rawUrl: rawUrl);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<JToken[]>(read);
		}
		return CheckAndExtraJsonResult(read.Content);
	}

	[HslMqttApi("ReadDateTime", "读取PLC的时间格式的数据，这个格式是s7格式的一种")]
	public OperateResult<DateTime> ReadDateTime(string address)
	{
		OperateResult<byte[]> operateResult = Read(address, 8);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(operateResult);
		}
		return SiemensDateTime.FromByteArray(operateResult.Content);
	}

	[HslMqttApi("WriteDateTime", "写入PLC的时间格式的数据，这个格式是s7格式的一种")]
	public OperateResult Write(string address, DateTime dateTime)
	{
		return Write(address, SiemensDateTime.ToByteArray(dateTime));
	}

	public async Task<OperateResult<DateTime>> ReadDateTimeAsync(string address)
	{
		OperateResult<byte[]> read = await ReadAsync(address, 8);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(read);
		}
		return SiemensDateTime.FromByteArray(read.Content);
	}

	public async Task<OperateResult> WriteAsync(string address, DateTime dateTime)
	{
		return await WriteAsync(address, SiemensDateTime.ToByteArray(dateTime));
	}

	[HslMqttApi("读取PLC的RPC接口的版本号信息")]
	public OperateResult<double> ReadVersion()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<double>(StringResources.Language.InsufficientPrivileges);
		}
		JArray jArray = BuildRequestBody("Api.Version", null, incrementCount.GetCurrentValue());
		OperateResult<string> operateResult = Post(rawUrl, jArray.ToString());
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<double>(operateResult);
		}
		OperateResult<JToken> operateResult2 = CheckAndExtraOneJsonResult(operateResult.Content);
		if (!operateResult2.IsSuccess)
		{
			return OperateResult.CreateFailedResult<double>(operateResult2);
		}
		return OperateResult.CreateSuccessResult(operateResult2.Content.Value<double>());
	}

	public async Task<OperateResult<double>> ReadVersionAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<double>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<string> read = await PostAsync(body: BuildRequestBody("Api.Version", null, incrementCount.GetCurrentValue()).ToString(), rawUrl: rawUrl);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<double>(read);
		}
		OperateResult<JToken> extra = CheckAndExtraOneJsonResult(read.Content);
		if (!extra.IsSuccess)
		{
			return OperateResult.CreateFailedResult<double>(extra);
		}
		return OperateResult.CreateSuccessResult(extra.Content.Value<double>());
	}

	[HslMqttApi("对PLC对象进行PING操作")]
	public OperateResult ReadPing()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<double>(StringResources.Language.InsufficientPrivileges);
		}
		JArray jArray = BuildRequestBody("Api.Ping", null, incrementCount.GetCurrentValue());
		OperateResult<string> operateResult = Post(rawUrl, jArray.ToString());
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return CheckErrorResult(JArray.Parse(operateResult.Content)[0] as JObject);
	}

	public async Task<OperateResult> ReadPingAsync()
	{
		if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			return new OperateResult<double>(StringResources.Language.InsufficientPrivileges);
		}
		OperateResult<string> read = await PostAsync(body: BuildRequestBody("Api.Ping", null, incrementCount.GetCurrentValue()).ToString(), rawUrl: rawUrl);
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckErrorResult(JArray.Parse(read.Content)[0] as JObject);
	}

	[HslMqttApi("从PLC退出登录，当前的token信息失效，需要再次调用ConnectServer获取新的token信息才可以")]
	public OperateResult Logout()
	{
		JArray jArray = BuildRequestBody("Api.Logout", null, incrementCount.GetCurrentValue());
		OperateResult<string> operateResult = Post(rawUrl, jArray.ToString());
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return CheckErrorResult(JArray.Parse(operateResult.Content)[0] as JObject);
	}

	public async Task<OperateResult> LogoutAsync()
	{
		OperateResult<string> read = await PostAsync(body: BuildRequestBody("Api.Logout", null, incrementCount.GetCurrentValue()).ToString(), rawUrl: rawUrl);
		if (!read.IsSuccess)
		{
			return read;
		}
		return CheckErrorResult(JArray.Parse(read.Content)[0] as JObject);
	}

	private static JObject GetJsonRpc(string method, JObject paramsJson, long id)
	{
		JObject jObject = new JObject();
		jObject.Add("jsonrpc", new JValue("2.0"));
		jObject.Add("method", new JValue(method));
		jObject.Add("id", new JValue(id));
		if (paramsJson != null)
		{
			jObject.Add("params", paramsJson);
		}
		return jObject;
	}

	private static JArray BuildRequestBody(string method, JObject paramsJson, long id)
	{
		return new JArray { GetJsonRpc(method, paramsJson, id) };
	}

	private static JArray BuildConnectBody(long id, string name, string password)
	{
		JObject jObject = new JObject();
		jObject.Add("user", new JValue(name));
		jObject.Add("password", new JValue(password));
		return BuildRequestBody("Api.Login", jObject, id);
	}

	private static JArray BuildReadRawBody(long id, string address)
	{
		JObject jObject = new JObject();
		jObject.Add("var", new JValue(address));
		jObject.Add("mode", new JValue("raw"));
		return BuildRequestBody("PlcProgram.Read", jObject, id);
	}

	private static JArray BuildWriteRawBody(long id, string address, byte[] value)
	{
		JObject jObject = new JObject();
		jObject.Add("var", new JValue(address));
		jObject.Add("mode", new JValue("raw"));
		jObject.Add("value", new JArray(((IEnumerable<byte>)value).Select((Func<byte, int>)((byte m) => m)).ToArray()));
		return BuildRequestBody("PlcProgram.Write", jObject, id);
	}

	private static JArray BuildWriteJTokenBody(long id, string address, JToken value)
	{
		JObject jObject = new JObject();
		jObject.Add("var", new JValue(address));
		jObject.Add("value", value);
		return BuildRequestBody("PlcProgram.Write", jObject, id);
	}

	private static JArray BuildReadJTokenBody(long id, string address)
	{
		JObject jObject = new JObject();
		jObject.Add("var", new JValue(address));
		return BuildRequestBody("PlcProgram.Read", jObject, id);
	}

	private static JArray BuildReadJTokenBody(long id, string[] address)
	{
		JArray jArray = new JArray();
		for (int i = 0; i < address.Length; i++)
		{
			JObject jObject = new JObject();
			jObject.Add("var", new JValue(address[i]));
			jArray.Add(GetJsonRpc("PlcProgram.Read", jObject, id + i));
		}
		return jArray;
	}

	private static OperateResult CheckErrorResult(JObject json)
	{
		if (json.ContainsKey("error"))
		{
			JObject jObject = json["error"] as JObject;
			int err = jObject.Value<int>("code");
			string msg = jObject.Value<string>("message");
			return new OperateResult(err, msg);
		}
		return OperateResult.CreateSuccessResult();
	}

	private static OperateResult<byte[]> CheckReadRawResult(string response)
	{
		OperateResult<JToken> operateResult = CheckAndExtraOneJsonResult(response);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		JArray source = operateResult.Content as JArray;
		return OperateResult.CreateSuccessResult(source.Select((JToken m) => m.Value<byte>()).ToArray());
	}

	private static OperateResult CheckWriteResult(string response)
	{
		JArray jArray = JArray.Parse(response);
		JObject jObject = (JObject)jArray[0];
		OperateResult operateResult = CheckErrorResult(jObject);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<JToken>(operateResult);
		}
		if (jObject.ContainsKey("result"))
		{
			return jObject["result"].Value<bool>() ? OperateResult.CreateSuccessResult() : new OperateResult(jObject.ToString());
		}
		return new OperateResult<JToken>("Can't find result key and none token, login failed:" + Environment.NewLine + response);
	}

	private static OperateResult<JToken> CheckAndExtraOneJsonResult(string response)
	{
		OperateResult<JToken[]> operateResult = CheckAndExtraJsonResult(response);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<JToken>(operateResult);
		}
		return OperateResult.CreateSuccessResult(operateResult.Content[0]);
	}

	private static OperateResult<JToken[]> CheckAndExtraJsonResult(string response)
	{
		try
		{
			JArray jArray = JArray.Parse(response);
			List<JToken> list = new List<JToken>();
			for (int i = 0; i < jArray.Count; i++)
			{
				if (jArray[i] is JObject jObject)
				{
					OperateResult operateResult = CheckErrorResult(jObject);
					if (!operateResult.IsSuccess)
					{
						return OperateResult.CreateFailedResult<JToken[]>(operateResult);
					}
					if (!jObject.ContainsKey("result"))
					{
						return new OperateResult<JToken[]>("Can't find result key and none token, login failed:" + Environment.NewLine + response);
					}
					list.Add(jObject["result"]);
				}
			}
			return OperateResult.CreateSuccessResult(list.ToArray());
		}
		catch (Exception ex)
		{
			return new OperateResult<JToken[]>("CheckAndExtraJsonResult failed: " + ex.Message + Environment.NewLine + "Content: " + response);
		}
	}
}
