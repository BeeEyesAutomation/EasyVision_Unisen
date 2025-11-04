using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.LogNet;

namespace HslCommunication.Core.Net;

public class NetworkWebApiBase
{
	private string host = "127.0.0.1";

	private string ipAddress = "127.0.0.1";

	private int port = 80;

	private string name = string.Empty;

	private string password = string.Empty;

	private HttpClient httpClient;

	public Action<HttpContentHeaders> AddRequestHeadersAction { get; set; }

	public string IpAddress
	{
		get
		{
			return ipAddress;
		}
		set
		{
			host = value;
			ipAddress = HslHelper.GetIpAddressFromInput(value);
		}
	}

	public int Port
	{
		get
		{
			return port;
		}
		set
		{
			port = value;
		}
	}

	public string UserName
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public string Password
	{
		get
		{
			return password;
		}
		set
		{
			password = value;
		}
	}

	public ILogNet LogNet { get; set; }

	public bool UseHttps { get; set; }

	public string DefaultContentType { get; set; }

	public bool UseEncodingISO { get; set; } = false;

	public HttpClient Client => httpClient;

	public string Host => host;

	public NetworkWebApiBase(string ipAddress)
		: this(ipAddress, 80, string.Empty, string.Empty)
	{
	}

	public NetworkWebApiBase(string ipAddress, int port)
		: this(ipAddress, port, string.Empty, string.Empty)
	{
	}

	public NetworkWebApiBase(string ipAddress, int port, string name, string password)
	{
		host = ipAddress;
		this.ipAddress = HslHelper.GetIpAddressFromInput(ipAddress);
		this.port = port;
		this.name = name;
		this.password = password;
		if (!string.IsNullOrEmpty(name))
		{
			HttpClientHandler httpClientHandler = new HttpClientHandler
			{
				Credentials = new NetworkCredential(name, password)
			};
			httpClientHandler.Proxy = null;
			httpClientHandler.UseProxy = false;
			ServicePointManager.ServerCertificateValidationCallback = TrustAllValidationCallback;
			httpClient = new HttpClient(httpClientHandler);
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
		}
		else
		{
			ServicePointManager.ServerCertificateValidationCallback = TrustAllValidationCallback;
			httpClient = new HttpClient();
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
		}
	}

	private bool TrustAllValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
	{
		return true;
	}

	protected virtual void AddRequestHeaders(HttpContentHeaders headers)
	{
	}

	private string GetEntireUrl(string url)
	{
		if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
		{
			return url;
		}
		return string.Format("{0}://{1}:{2}/{3}", UseHttps ? "https" : "http", host, port, url.StartsWith("/") ? url.Substring(1) : url);
	}

	public OperateResult<string> Get(string rawUrl)
	{
		string entireUrl = GetEntireUrl(rawUrl);
		try
		{
			using HttpResponseMessage httpResponseMessage = httpClient.GetAsync(entireUrl).Result;
			using HttpContent httpContent = httpResponseMessage.Content;
			httpResponseMessage.EnsureSuccessStatusCode();
			if (UseEncodingISO)
			{
				using (StreamReader streamReader = new StreamReader(httpContent.ReadAsStreamAsync().Result, Encoding.GetEncoding("iso-8859-1")))
				{
					return OperateResult.CreateSuccessResult(streamReader.ReadToEnd());
				}
			}
			return OperateResult.CreateSuccessResult(httpContent.ReadAsStringAsync().Result);
		}
		catch (Exception ex)
		{
			return new OperateResult<string>(ex.Message);
		}
	}

	public OperateResult<string> Post(string rawUrl, string body)
	{
		string entireUrl = GetEntireUrl(rawUrl);
		try
		{
			using StringContent stringContent = new StringContent(body);
			if (!string.IsNullOrEmpty(DefaultContentType))
			{
				stringContent.Headers.ContentType = new MediaTypeHeaderValue(DefaultContentType);
			}
			AddRequestHeaders(stringContent.Headers);
			AddRequestHeadersAction?.Invoke(stringContent.Headers);
			using HttpResponseMessage httpResponseMessage = httpClient.PostAsync(entireUrl, stringContent).Result;
			using HttpContent httpContent = httpResponseMessage.Content;
			httpResponseMessage.EnsureSuccessStatusCode();
			if (UseEncodingISO)
			{
				using (StreamReader streamReader = new StreamReader(httpContent.ReadAsStreamAsync().Result, Encoding.GetEncoding("iso-8859-1")))
				{
					return OperateResult.CreateSuccessResult(streamReader.ReadToEnd());
				}
			}
			return OperateResult.CreateSuccessResult(httpContent.ReadAsStringAsync().Result);
		}
		catch (Exception ex)
		{
			return new OperateResult<string>(ex.Message + Environment.NewLine + "Url: " + entireUrl);
		}
	}

	public async Task<OperateResult<string>> GetAsync(string rawUrl)
	{
		string url = GetEntireUrl(rawUrl);
		try
		{
			using HttpResponseMessage response = await httpClient.GetAsync(url);
			using HttpContent content = response.Content;
			response.EnsureSuccessStatusCode();
			if (UseEncodingISO)
			{
				using (StreamReader sr = new StreamReader(await content.ReadAsStreamAsync(), Encoding.GetEncoding("iso-8859-1")))
				{
					return OperateResult.CreateSuccessResult(sr.ReadToEnd());
				}
			}
			return OperateResult.CreateSuccessResult(await content.ReadAsStringAsync());
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception ex3 = ex2;
			return new OperateResult<string>(ex3.Message);
		}
	}

	public async Task<OperateResult<string>> PostAsync(string rawUrl, string body)
	{
		string url = GetEntireUrl(rawUrl);
		try
		{
			using StringContent stringContent = new StringContent(body);
			if (!string.IsNullOrEmpty(DefaultContentType))
			{
				stringContent.Headers.ContentType = new MediaTypeHeaderValue(DefaultContentType);
			}
			AddRequestHeaders(stringContent.Headers);
			AddRequestHeadersAction?.Invoke(stringContent.Headers);
			using HttpResponseMessage response = await httpClient.PostAsync(url, stringContent);
			using HttpContent content = response.Content;
			response.EnsureSuccessStatusCode();
			if (UseEncodingISO)
			{
				using (StreamReader sr = new StreamReader(await content.ReadAsStreamAsync(), Encoding.GetEncoding("iso-8859-1")))
				{
					return OperateResult.CreateSuccessResult(sr.ReadToEnd());
				}
			}
			return OperateResult.CreateSuccessResult(await content.ReadAsStringAsync());
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception ex3 = ex2;
			return new OperateResult<string>(ex3.Message);
		}
	}

	public override string ToString()
	{
		return $"NetworkWebApiBase[{ipAddress}:{port}]";
	}
}
