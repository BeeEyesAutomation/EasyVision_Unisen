using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using HslCommunication.BasicFramework;

namespace HslCommunication.Core.Net;

public class NetworkAuthenticationServerBase : NetworkServerBase, IDisposable
{
	public delegate void OnClientStatusChangeDelegate(object server, AppSession session);

	private Dictionary<string, string> accounts = new Dictionary<string, string>();

	private SimpleHybirdLock lockLoginAccount = new SimpleHybirdLock();

	private List<string> TrustedClients = null;

	private bool IsTrustedClientsOnly = false;

	private SimpleHybirdLock lock_trusted_clients;

	private Timer timerHeart;

	private List<AppSession> listsOnlineClient;

	private object lockOnlineClient;

	private int onlineCount = 0;

	private bool disposedValue = false;

	public bool IsUseAccountCertificate { get; set; }

	public int OnlineCount => onlineCount;

	public AppSession[] GetOnlineSessions
	{
		get
		{
			lock (lockOnlineClient)
			{
				return listsOnlineClient.ToArray();
			}
		}
	}

	public TimeSpan ActiveTimeSpan { get; set; }

	public event OnClientStatusChangeDelegate OnClientOnline;

	public event OnClientStatusChangeDelegate OnClientOffline;

	public NetworkAuthenticationServerBase()
	{
		lock_trusted_clients = new SimpleHybirdLock();
		lockOnlineClient = new object();
		listsOnlineClient = new List<AppSession>();
		timerHeart = new Timer(ThreadTimerHeartCheck, null, 2000, 10000);
		ActiveTimeSpan = TimeSpan.FromHours(24.0);
	}

	protected override OperateResult SocketAcceptExtraCheck(Socket socket, IPEndPoint endPoint)
	{
		if (IsUseAccountCertificate)
		{
			OperateResult<byte[], byte[]> operateResult = ReceiveAndCheckBytes(socket, 2000);
			if (!operateResult.IsSuccess)
			{
				return new OperateResult($"Client login failed[{endPoint}]");
			}
			if (BitConverter.ToInt32(operateResult.Content1, 0) != 5)
			{
				base.LogNet?.WriteError(ToString(), StringResources.Language.NetClientAccountTimeout);
				socket?.Close();
				return new OperateResult($"Authentication failed[{endPoint}]");
			}
			string[] array = HslProtocol.UnPackStringArrayFromByte(operateResult.Content2);
			string text = CheckAccountLegal(array);
			SendStringAndCheckReceive(socket, (text == "success") ? 1 : 0, new string[1] { text });
			if (text != "success")
			{
				return new OperateResult($"Client login failed[{endPoint}]:{text} {SoftBasic.ArrayFormat(array)}");
			}
			base.LogNet?.WriteDebug(ToString(), $"Account Login:{array[0]} Endpoint:[{endPoint}]");
		}
		return OperateResult.CreateSuccessResult();
	}

	public void AddAccount(string userName, string password)
	{
		if (!string.IsNullOrEmpty(userName))
		{
			lockLoginAccount.Enter();
			if (accounts.ContainsKey(userName))
			{
				accounts[userName] = password;
			}
			else
			{
				accounts.Add(userName, password);
			}
			lockLoginAccount.Leave();
		}
	}

	public void DeleteAccount(string userName)
	{
		lockLoginAccount.Enter();
		if (accounts.ContainsKey(userName))
		{
			accounts.Remove(userName);
		}
		lockLoginAccount.Leave();
	}

	private string CheckAccountLegal(string[] infos)
	{
		if (infos != null && infos.Length < 2)
		{
			return "User Name input wrong";
		}
		string text = "";
		lockLoginAccount.Enter();
		text = ((!accounts.ContainsKey(infos[0])) ? "User Name input wrong" : ((!(accounts[infos[0]] != infos[1])) ? "success" : "Password is not corrent"));
		lockLoginAccount.Leave();
		return text;
	}

	protected virtual void ThreadPoolLoginAfterClientCheck(Socket socket, IPEndPoint endPoint)
	{
		AppSession appSession = new AppSession(socket);
		try
		{
			socket.BeginReceive(new byte[0], 0, 0, SocketFlags.None, SocketAsyncCallBack, appSession);
			AddClient(appSession);
		}
		catch
		{
			socket.Close();
			base.LogNet?.WriteDebug(ToString(), string.Format(StringResources.Language.ClientOfflineInfo, endPoint));
		}
	}

	protected virtual void SocketAsyncCallBack(IAsyncResult ar)
	{
		if (ar.AsyncState is AppSession appSession)
		{
			appSession?.WorkSocket?.Close();
		}
	}

	protected override void ThreadPoolLogin(Socket socket, IPEndPoint endPoint)
	{
		string ipAddress = ((endPoint == null) ? string.Empty : endPoint.Address.ToString());
		if (IsTrustedClientsOnly && !CheckIpAddressTrusted(ipAddress))
		{
			base.LogNet?.WriteDebug(ToString(), string.Format(StringResources.Language.ClientDisableLogin, endPoint));
			socket.Close();
			return;
		}
		if (!IsUseAccountCertificate)
		{
			base.LogNet?.WriteDebug(ToString(), string.Format(StringResources.Language.ClientOnlineInfo, endPoint));
		}
		ThreadPoolLoginAfterClientCheck(socket, endPoint);
	}

	public void SetTrustedIpAddress(List<string> clients)
	{
		lock_trusted_clients.Enter();
		if (clients != null)
		{
			TrustedClients = clients.Select(delegate(string m)
			{
				IPAddress iPAddress = IPAddress.Parse(m);
				return iPAddress.ToString();
			}).ToList();
			IsTrustedClientsOnly = true;
		}
		else
		{
			TrustedClients = new List<string>();
			IsTrustedClientsOnly = false;
		}
		lock_trusted_clients.Leave();
	}

	private bool CheckIpAddressTrusted(string ipAddress)
	{
		if (IsTrustedClientsOnly)
		{
			bool result = false;
			lock_trusted_clients.Enter();
			for (int i = 0; i < TrustedClients.Count; i++)
			{
				if (TrustedClients[i] == ipAddress)
				{
					result = true;
					break;
				}
			}
			lock_trusted_clients.Leave();
			return result;
		}
		return false;
	}

	public string[] GetTrustedClients()
	{
		string[] result = new string[0];
		lock_trusted_clients.Enter();
		if (TrustedClients != null)
		{
			result = TrustedClients.ToArray();
		}
		lock_trusted_clients.Leave();
		return result;
	}

	protected override void CloseAction()
	{
		lock (lockOnlineClient)
		{
			for (int i = 0; i < listsOnlineClient.Count; i++)
			{
				listsOnlineClient[i]?.WorkSocket?.Close();
				base.LogNet?.WriteDebug(ToString(), string.Format(StringResources.Language.ClientOfflineInfo, listsOnlineClient[i].IpEndPoint));
			}
			listsOnlineClient.Clear();
			onlineCount = 0;
		}
		base.CloseAction();
	}

	protected void AddClient(AppSession session)
	{
		lock (lockOnlineClient)
		{
			listsOnlineClient.Add(session);
			onlineCount++;
		}
		this.OnClientOnline?.Invoke(this, session);
	}

	protected void RemoveClient(AppSession session, string reason = "")
	{
		bool flag = false;
		lock (lockOnlineClient)
		{
			if (listsOnlineClient.Remove(session))
			{
				base.LogNet?.WriteDebug(ToString(), string.Format(StringResources.Language.ClientOfflineInfo, session.IpEndPoint) + " " + reason);
				session.WorkSocket?.Close();
				onlineCount--;
				flag = true;
			}
		}
		if (flag)
		{
			this.OnClientOffline?.Invoke(this, session);
		}
	}

	private void ThreadTimerHeartCheck(object obj)
	{
		AppSession[] array = null;
		lock (lockOnlineClient)
		{
			array = listsOnlineClient.ToArray();
		}
		if (array == null || array.Length == 0)
		{
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (DateTime.Now - array[i].HeartTime > ActiveTimeSpan)
			{
				RemoveClient(array[i]);
			}
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				ServerClose();
				lockLoginAccount?.Dispose();
				lock_trusted_clients?.Dispose();
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public override string ToString()
	{
		return $"NetworkAuthenticationServerBase[{base.Port}]";
	}
}
