using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.LogNet;
using HslCommunication.MQTT;
using HslCommunication.Reflection;

namespace HslCommunication.Enthernet;

public class HttpServer
{
	private Dictionary<string, MqttRpcApiInfo> apiTopicServiceDict;

	private object rpcApiLock;

	private int receiveBufferSize = 2048;

	private int port = 80;

	private HttpListener listener;

	private ILogNet logNet;

	private Encoding encoding = Encoding.UTF8;

	private Func<HttpListenerRequest, HttpListenerResponse, string, object> handleRequestFunc;

	private LogStatisticsDict statisticsDict;

	private bool loginAccess = false;

	private MqttCredential[] loginCredentials;

	private Action<HttpApiCalledInfo> apiCalledAction;

	private bool useHttps = false;

	public Action<HttpListenerRequest, ISessionContext> DealWithHttpListenerRequest { get; set; }

	public LogStatisticsDict LogStatistics => statisticsDict;

	public ILogNet LogNet
	{
		get
		{
			return logNet;
		}
		set
		{
			logNet = value;
		}
	}

	public Encoding ServerEncoding
	{
		get
		{
			return encoding;
		}
		set
		{
			encoding = value;
		}
	}

	public bool IsCrossDomain { get; set; } = true;

	public bool LogHttpBody { get; set; } = false;

	public bool LogHttpHeader { get; set; } = false;

	public Func<HttpListenerRequest, HttpListenerResponse, string, object> HandleRequestFunc
	{
		get
		{
			return handleRequestFunc;
		}
		set
		{
			handleRequestFunc = value;
		}
	}

	public Func<HttpListenerRequest, HttpListenerResponse, HttpUploadFile, string> HandleFileUpload { get; set; }

	public int Port => port;

	public Action<HttpApiCalledInfo> ApiCalledAction
	{
		get
		{
			return apiCalledAction;
		}
		set
		{
			apiCalledAction = value;
		}
	}

	public HttpServer()
	{
		statisticsDict = new LogStatisticsDict(GenerateMode.ByEveryDay, 60);
		apiTopicServiceDict = new Dictionary<string, MqttRpcApiInfo>();
		rpcApiLock = new object();
	}

	public void UseHttps()
	{
		useHttps = true;
	}

	public void Start(int port)
	{
		this.port = port;
		listener = new HttpListener();
		if (useHttps)
		{
			listener.Prefixes.Add($"https://+:{port}/");
			listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
		}
		else
		{
			listener.Prefixes.Add($"http://+:{port}/");
		}
		listener.Start();
		listener.BeginGetContext(GetConnectCallBack, listener);
		logNet?.WriteDebug(ToString(), "Server Started, wait for connections");
	}

	public void Close()
	{
		listener?.Close();
	}

	private async void GetConnectCallBack(IAsyncResult ar)
	{
		object asyncState = ar.AsyncState;
		if (!(asyncState is HttpListener listener))
		{
			return;
		}
		HttpListenerContext context = null;
		try
		{
			context = listener.EndGetContext(ar);
		}
		catch (Exception ex)
		{
			Exception ex5 = ex;
			Exception ex6 = ex5;
			logNet?.WriteException(ToString(), ex6);
		}
		int restartcount = 0;
		while (true)
		{
			try
			{
				listener.BeginGetContext(GetConnectCallBack, listener);
			}
			catch (Exception ex)
			{
				Exception ex7 = ex;
				Exception ex8 = ex7;
				logNet?.WriteException(ToString(), ex8);
				restartcount++;
				if (restartcount >= 3)
				{
					logNet?.WriteError(ToString(), "ReGet Content Failed!");
					return;
				}
				HslHelper.ThreadSleep(1000);
				continue;
			}
			break;
		}
		if (context == null)
		{
			return;
		}
		HttpListenerRequest request = context.Request;
		HttpListenerResponse response = context.Response;
		if (response != null)
		{
			try
			{
				if (IsCrossDomain)
				{
					context.Response.AppendHeader("Access-Control-Allow-Origin", request.Headers["Origin"]);
					context.Response.AppendHeader("Access-Control-Allow-Headers", "*");
					context.Response.AppendHeader("Access-Control-Allow-Method", "POST,GET,PUT,OPTIONS,DELETE");
					context.Response.AppendHeader("Access-Control-Allow-Credentials", "true");
					context.Response.AppendHeader("Access-Control-Max-Age", "3600");
				}
				context.Response.AddHeader("Content-type", "text/html; charset=utf-8");
			}
			catch (Exception ex)
			{
				Exception ex9 = ex;
				Exception ex10 = ex9;
				logNet?.WriteError(ToString(), ex10.Message);
			}
		}
		byte[] data = await GetDataFromRequestAsync(request);
		response.StatusCode = 200;
		try
		{
			DateTime start = DateTime.Now;
			object ret = await HandleRequest(request, response, data);
			StringBuilder stringBuilder = new StringBuilder();
			if (LogHttpHeader)
			{
				stringBuilder.AppendLine("Header Request=======================");
				if (request.Headers != null)
				{
					string[] allKeys = request.Headers.AllKeys;
					string[] array = allKeys;
					foreach (string key2 in array)
					{
						stringBuilder.AppendLine(key2 + " : " + request.Headers[key2]);
					}
				}
			}
			if (LogHttpBody)
			{
				stringBuilder.AppendLine("Body Request=========================");
				stringBuilder.AppendLine(Encoding.UTF8.GetString(data));
			}
			if (LogHttpHeader)
			{
				stringBuilder.AppendLine("Header Response =======================");
				if (response.Headers != null)
				{
					string[] allKeys2 = response.Headers.AllKeys;
					string[] array2 = allKeys2;
					foreach (string key3 in array2)
					{
						stringBuilder.AppendLine(key3 + " : " + response.Headers[key3]);
					}
				}
			}
			if (ret == null)
			{
				return;
			}
			using (Stream stream = response.OutputStream)
			{
				if (ret is string ret_str)
				{
					if (string.IsNullOrEmpty(ret_str))
					{
						await stream.WriteAsync(new byte[0], 0, 0);
					}
					else
					{
						byte[] buffer = encoding.GetBytes(ret_str);
						await stream.WriteAsync(buffer, 0, buffer.Length);
					}
					if (LogHttpBody)
					{
						stringBuilder.AppendLine("Body Response ==========================");
						stringBuilder.AppendLine(ret_str);
					}
				}
				else if (ret is byte[] ret_bytes)
				{
					response.ContentLength64 = ret_bytes.Length;
					await stream.WriteAsync(ret_bytes, 0, ret_bytes.Length);
					if (LogHttpBody)
					{
						stringBuilder.AppendLine("Body Response ==========================");
						stringBuilder.AppendLine(SoftBasic.ByteToHexString(ret_bytes));
					}
				}
			}
			logNet?.WriteDebug(ToString(), $"Request [{request.HttpMethod}], Url: {HttpUtility.UrlDecode(request.RawUrl)} Timecost: {(DateTime.Now - start).TotalMilliseconds:F1}");
			if (LogHttpBody || LogHttpHeader)
			{
				logNet?.WriteDebug(ToString(), stringBuilder.ToString());
			}
		}
		catch (Exception ex11)
		{
			logNet?.WriteException(ToString(), "Request[" + request.HttpMethod + "], " + request.RawUrl, ex11);
		}
	}

	private byte[] GetDataFromRequest(HttpListenerRequest request)
	{
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			byte[] array = new byte[receiveBufferSize];
			int num = 0;
			do
			{
				num = request.InputStream.Read(array, 0, array.Length);
				if (num > 0)
				{
					memoryStream.Write(array, 0, num);
				}
			}
			while (num != 0);
			return memoryStream.ToArray();
		}
		catch
		{
			return new byte[0];
		}
	}

	private async Task<byte[]> GetDataFromRequestAsync(HttpListenerRequest request)
	{
		try
		{
			MemoryStream ms = new MemoryStream();
			byte[] byteArr = new byte[receiveBufferSize];
			int readLen;
			do
			{
				readLen = await request.InputStream.ReadAsync(byteArr, 0, byteArr.Length);
				if (readLen > 0)
				{
					ms.Write(byteArr, 0, readLen);
				}
			}
			while (readLen != 0);
			return ms.ToArray();
		}
		catch
		{
			return new byte[0];
		}
	}

	protected virtual async Task<object> HandleRequest(HttpListenerRequest request, HttpListenerResponse response, byte[] data)
	{
		if (request.HttpMethod == "OPTIONS")
		{
			return "OK";
		}
		if (loginAccess)
		{
			string[] values = request.Headers.GetValues("Authorization");
			if (values == null || values.Length < 1 || string.IsNullOrEmpty(values[0]))
			{
				response.StatusCode = 401;
				response.AddHeader("WWW-Authenticate", "Basic realm=\"Secure Area\"");
				return "";
			}
			try
			{
				string base64String = values[0].Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
				string accountString = Encoding.UTF8.GetString(Convert.FromBase64String(base64String));
				string[] account = accountString.Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
				if (account.Length < 2)
				{
					response.StatusCode = 401;
					response.AddHeader("WWW-Authenticate", "Basic realm=\"Secure Area\"");
					return "";
				}
				MqttCredential[] credentials = loginCredentials;
				bool loginEnable = false;
				for (int k = 0; k < credentials.Length; k++)
				{
					if (account[0] == credentials[k].UserName && account[1] == credentials[k].Password)
					{
						loginEnable = true;
						break;
					}
				}
				if (!loginEnable)
				{
					response.StatusCode = 401;
					response.AddHeader("WWW-Authenticate", "Basic realm=\"Secure Area\"");
					return "";
				}
			}
			catch
			{
				response.StatusCode = 401;
				response.AddHeader("WWW-Authenticate", "Basic realm=\"Secure Area\"");
				return "";
			}
		}
		if (request.HttpMethod == "HSL")
		{
			if (request.RawUrl.StartsWith("/Apis"))
			{
				response.AddHeader("Content-type", "application/json; charset=utf-8");
				return GetAllRpcApiInfo().ToJsonString();
			}
			if (request.RawUrl.StartsWith("/Logs"))
			{
				response.AddHeader("Content-type", "application/json; charset=utf-8");
				if (request.RawUrl == "/Logs" || request.RawUrl == "/Logs/")
				{
					return LogStatistics.LogStat.GetStatisticsSnapshot().ToJsonString();
				}
				return LogStatistics.GetStatisticsSnapshot(request.RawUrl.Substring(6)).ToJsonString();
			}
			response.AddHeader("Content-type", "application/json; charset=utf-8");
			return GetAllRpcApiInfo().ToJsonString();
		}
		if (request.HttpMethod == "OPTIONS")
		{
			return "OK";
		}
		MqttRpcApiInfo apiInformation = GetMqttRpcApiInfo(GetMethodName(HttpUtility.UrlDecode(request.RawUrl)));
		if (apiInformation == null)
		{
			if (request.ContentType != null && request.ContentType.StartsWith("multipart/form-data; boundary=--------------------------"))
			{
				int index = -1;
				for (int j = 0; j < data.Length - 4; j++)
				{
					if (data[j] == 13 && data[j + 1] == 10 && data[j + 2] == 13 && data[j + 3] == 10)
					{
						index = j + 4;
						break;
					}
				}
				if (index == -1)
				{
					return "Not file content!";
				}
				int last = data.Length - 3;
				for (int i = last; i > 0; i--)
				{
					if (data[i] == 13 && data[i + 1] == 10)
					{
						last = i;
						break;
					}
				}
				if (HandleFileUpload != null)
				{
					string context = encoding.GetString(data, 0, index - 4);
					HttpUploadFile uploadFile = new HttpUploadFile
					{
						FileName = SoftBasic.UrlDecode(Regex.Match(context, "filename=\"[^\"]+").Value.Substring(10), Encoding.UTF8),
						Name = Regex.Match(context, "name=\"[^\"]+").Value.Substring(6),
						Content = data.SelectMiddle(index, last - index)
					};
					return HandleFileUpload(request, response, uploadFile);
				}
			}
			else if (HandleRequestFunc != null)
			{
				return HandleRequestFunc(request, response, encoding.GetString(data));
			}
			return "This is HslWebServer, Thank you for use!";
		}
		response.AddHeader("Content-type", "application/json; charset=utf-8");
		DateTime dateTime = DateTime.Now;
		string url = HttpUtility.UrlDecode(request.RawUrl);
		string body = encoding.GetString(data);
		string result = await HandleObjectMethod(request, url, body, apiInformation, DealWithHttpListenerRequest);
		double timeSpend = Math.Round((DateTime.Now - dateTime).TotalSeconds, 5);
		apiInformation.CalledCountAddOne((long)(timeSpend * 100000.0));
		statisticsDict.StatisticsAdd(apiInformation.ApiTopic, 1L);
		if (apiCalledAction != null)
		{
			HttpApiCalledInfo httpApiCalledInfo = new HttpApiCalledInfo
			{
				HttpMethod = request.HttpMethod,
				Url = url,
				Body = body,
				Result = result,
				CostTime = timeSpend * 1000.0,
				CalledCount = apiInformation.CalledCount
			};
			apiCalledAction?.Invoke(httpApiCalledInfo);
		}
		return result;
	}

	private MqttRpcApiInfo GetMqttRpcApiInfo(string apiTopic)
	{
		MqttRpcApiInfo result = null;
		lock (rpcApiLock)
		{
			if (apiTopicServiceDict.ContainsKey(apiTopic))
			{
				result = apiTopicServiceDict[apiTopic];
			}
		}
		return result;
	}

	private void MqttRpcAdd(string apiTopic, MqttRpcApiInfo apiInfo)
	{
		if (apiTopicServiceDict.ContainsKey(apiTopic))
		{
			apiTopicServiceDict[apiTopic] = apiInfo;
		}
		else
		{
			apiTopicServiceDict.Add(apiTopic, apiInfo);
		}
	}

	private bool MqttRpcRemove(string apiTopic)
	{
		if (apiTopicServiceDict.ContainsKey(apiTopic))
		{
			return apiTopicServiceDict.Remove(apiTopic);
		}
		return false;
	}

	public MqttRpcApiInfo[] GetAllRpcApiInfo()
	{
		MqttRpcApiInfo[] result = null;
		lock (rpcApiLock)
		{
			result = apiTopicServiceDict.Values.ToArray();
		}
		return result;
	}

	public void RegisterHttpRpcApi(string api, object obj)
	{
		lock (rpcApiLock)
		{
			foreach (MqttRpcApiInfo item in MqttHelper.GetSyncServicesApiInformationFromObject(api, obj))
			{
				MqttRpcAdd(item.ApiTopic, item);
			}
		}
	}

	public void RegisterHttpRpcApi(object obj)
	{
		lock (rpcApiLock)
		{
			foreach (MqttRpcApiInfo item in MqttHelper.GetSyncServicesApiInformationFromObject(obj))
			{
				MqttRpcAdd(item.ApiTopic, item);
			}
		}
	}

	public void UnRegisterHttpRpcApi(string api, object obj)
	{
		lock (rpcApiLock)
		{
			foreach (MqttRpcApiInfo item in MqttHelper.GetSyncServicesApiInformationFromObject(api, obj))
			{
				MqttRpcRemove(item.ApiTopic);
			}
		}
	}

	public void UnRegisterHttpRpcApi(object obj)
	{
		lock (rpcApiLock)
		{
			foreach (MqttRpcApiInfo item in MqttHelper.GetSyncServicesApiInformationFromObject(obj))
			{
				MqttRpcRemove(item.ApiTopic);
			}
		}
	}

	public bool UnRegisterHttpRpcApiSingle(string apiTopic)
	{
		lock (rpcApiLock)
		{
			return MqttRpcRemove(apiTopic);
		}
	}

	public void SetLoginAccessControl(MqttCredential[] credentials)
	{
		if (credentials == null)
		{
			loginAccess = false;
			return;
		}
		if (credentials.Length == 0)
		{
			loginAccess = false;
			return;
		}
		loginAccess = true;
		loginCredentials = credentials;
	}

	public override string ToString()
	{
		return $"HttpServer[{port}]";
	}

	public static async Task<string> HandleObjectMethod(HttpListenerRequest request, string deceodeUrl, string json, object obj, Action<HttpListenerRequest, ISessionContext> action)
	{
		string method = GetMethodName(deceodeUrl);
		if (method.LastIndexOf('/') >= 0)
		{
			method = method.Substring(method.LastIndexOf('/') + 1);
		}
		MethodInfo methodInfo = obj.GetType().GetMethod(method);
		if (methodInfo == null)
		{
			return new OperateResult<string>("Current MqttSync Api ：[" + method + "] not exsist").ToJsonString();
		}
		OperateResult<MqttRpcApiInfo> apiResult = MqttHelper.GetMqttSyncServicesApiFromMethod("", methodInfo, obj);
		if (!apiResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(apiResult).ToJsonString();
		}
		return await HandleObjectMethod(request, deceodeUrl, json, apiResult.Content, action);
	}

	public static string GetMethodName(string url)
	{
		string empty = string.Empty;
		empty = ((url.IndexOf('?') <= 0) ? url : url.Substring(0, url.IndexOf('?')));
		if (empty.EndsWith("/") || empty.StartsWith("/"))
		{
			empty = empty.Trim('/');
		}
		return empty;
	}

	private static ISessionContext GetSessionContextFromHeaders(HttpListenerRequest request, Action<HttpListenerRequest, ISessionContext> userParse)
	{
		try
		{
			string[] values = request.Headers.GetValues("Authorization");
			if (values == null || values.Length < 1 || string.IsNullOrEmpty(values[0]))
			{
				return null;
			}
			string s = values[0].Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
			string text = Encoding.UTF8.GetString(Convert.FromBase64String(s));
			string[] array = text.Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length < 1)
			{
				return null;
			}
			SessionContext sessionContext = new SessionContext
			{
				UserName = array[0]
			};
			userParse?.Invoke(request, sessionContext);
			return sessionContext;
		}
		catch
		{
			return null;
		}
	}

	public static async Task<string> HandleObjectMethod(HttpListenerRequest request, string deceodeUrl, string json, MqttRpcApiInfo apiInformation, Action<HttpListenerRequest, ISessionContext> action)
	{
		ISessionContext context = GetSessionContextFromHeaders(request, action);
		if (apiInformation.PermissionAttribute != null)
		{
			if (!Authorization.asdniasnfaksndiqwhawfskhfaiw())
			{
				return new OperateResult<string>("Permission function need authorization ：" + StringResources.Language.InsufficientPrivileges).ToJsonString();
			}
			try
			{
				string[] values = request.Headers.GetValues("Authorization");
				if (values == null || values.Length < 1 || string.IsNullOrEmpty(values[0]))
				{
					return new OperateResult<string>("Mqtt RPC Api ：[" + apiInformation.ApiTopic + "] has none Authorization information, access not permission").ToJsonString();
				}
				string base64String = values[0].Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
				string accountString = Encoding.UTF8.GetString(Convert.FromBase64String(base64String));
				string[] account = accountString.Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
				if (account.Length < 1)
				{
					return new OperateResult<string>("Mqtt RPC Api ：[" + apiInformation.ApiTopic + "] has none Username information, access not permission").ToJsonString();
				}
				if (!apiInformation.PermissionAttribute.CheckUserName(account[0]))
				{
					return new OperateResult<string>("Mqtt RPC Api ：[" + apiInformation.ApiTopic + "] Check Username[" + account[0] + "] failed, access not permission").ToJsonString();
				}
			}
			catch (Exception ex)
			{
				return new OperateResult<string>("Mqtt RPC Api ：[" + apiInformation.ApiTopic + "] Check Username failed, access not permission, reason:" + ex.Message).ToJsonString();
			}
		}
		try
		{
			if (apiInformation.Method != null)
			{
				MethodInfo methodInfo = apiInformation.Method;
				string apiName2 = apiInformation.ApiTopic;
				if (request.HttpMethod != apiInformation.HttpMethod)
				{
					return new OperateResult("Current Api ：" + apiName2 + " not support diffrent httpMethod").ToJsonString();
				}
				object obj = methodInfo.Invoke(parameters: (!(request.HttpMethod == "GET")) ? HslReflectionHelper.GetParametersFromJson(context, request, methodInfo.GetParameters(), json) : ((deceodeUrl.IndexOf('?') <= 0) ? HslReflectionHelper.GetParametersFromJson(context, request, methodInfo.GetParameters(), json) : HslReflectionHelper.GetParametersFromUrl(context, request, methodInfo.GetParameters(), deceodeUrl)), obj: apiInformation.SourceObject);
				if (obj is Task task)
				{
					await task;
					return task.GetType().GetProperty("Result").GetValue(task, null)
						.ToJsonString();
				}
				return obj.ToJsonString();
			}
			if (apiInformation.Property != null)
			{
				string apiName3 = apiInformation.ApiTopic;
				if (request.HttpMethod != apiInformation.HttpMethod)
				{
					return new OperateResult("Current Api ：" + apiName3 + " not support diffrent httpMethod").ToJsonString();
				}
				if (request.HttpMethod != "GET")
				{
					return new OperateResult("Current Api ：" + apiName3 + " not support POST").ToJsonString();
				}
				return apiInformation.Property.GetValue(apiInformation.SourceObject, null).ToJsonString();
			}
			return new OperateResult("Current Api ：" + deceodeUrl + " not supported").ToJsonString();
		}
		catch (Exception ex2)
		{
			return new OperateResult("Current Api ：" + deceodeUrl + " Wrong，Reason：" + ex2.Message).ToJsonString();
		}
	}
}
