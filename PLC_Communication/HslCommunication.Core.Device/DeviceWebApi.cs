using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HslCommunication.Core.Net;

namespace HslCommunication.Core.Device;

public class DeviceWebApi : DeviceCommunication
{
	private NetworkWebApiBase webApi;

	public string IpAddress
	{
		get
		{
			return webApi.IpAddress;
		}
		set
		{
			webApi.IpAddress = value;
		}
	}

	public int Port
	{
		get
		{
			return webApi.Port;
		}
		set
		{
			webApi.Port = value;
		}
	}

	public string UserName
	{
		get
		{
			return webApi.UserName;
		}
		set
		{
			webApi.UserName = value;
		}
	}

	public string Password
	{
		get
		{
			return webApi.Password;
		}
		set
		{
			webApi.Password = value;
		}
	}

	public bool UseHttps
	{
		get
		{
			return webApi.UseHttps;
		}
		set
		{
			webApi.UseHttps = value;
		}
	}

	public string DefaultContentType
	{
		get
		{
			return webApi.DefaultContentType;
		}
		set
		{
			webApi.DefaultContentType = value;
		}
	}

	public bool UseEncodingISO
	{
		get
		{
			return webApi.UseEncodingISO;
		}
		set
		{
			webApi.UseEncodingISO = value;
		}
	}

	public HttpClient Client => webApi.Client;

	public DeviceWebApi(string ipAddress)
		: this(ipAddress, 80, string.Empty, string.Empty)
	{
	}

	public DeviceWebApi(string ipAddress, int port)
		: this(ipAddress, port, string.Empty, string.Empty)
	{
	}

	public DeviceWebApi(string ipAddress, int port, string name, string password)
	{
		webApi = new NetworkWebApiBase(ipAddress, port, name, password);
		webApi.AddRequestHeadersAction = AddRequestHeaders;
	}

	protected virtual void AddRequestHeaders(HttpContentHeaders headers)
	{
	}

	public OperateResult<string> Get(string rawUrl)
	{
		return webApi.Get(rawUrl);
	}

	public OperateResult<string> Post(string rawUrl, string body)
	{
		return webApi.Post(rawUrl, body);
	}

	public async Task<OperateResult<string>> GetAsync(string rawUrl)
	{
		return await webApi.GetAsync(rawUrl);
	}

	public async Task<OperateResult<string>> PostAsync(string rawUrl, string body)
	{
		return await webApi.PostAsync(rawUrl, body);
	}

	public override string ToString()
	{
		return $"DeviceWebApi[{IpAddress}:{Port}]";
	}
}
