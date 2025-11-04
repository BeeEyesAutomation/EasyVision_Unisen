using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using HslCommunication.Core.IMessage;
using HslCommunication.Core.Pipe;

namespace HslCommunication.Core.Net;

public class NetworkAlienClient : CommunicationTcpServer, IDisposable
{
	public delegate void OnClientConnectedDelegate(PipeDtuNet pipe);

	private byte[] password;

	private List<string> trustOnline;

	private SimpleHybirdLock trustLock;

	private bool isResponseAck = true;

	private bool isCheckPwd = true;

	private bool disposedValue;

	private bool useRegistrationPackage = true;

	public const byte StatusOk = 0;

	public const byte StatusLoginRepeat = 1;

	public const byte StatusLoginForbidden = 2;

	public const byte StatusPasswodWrong = 3;

	public bool IsResponseAck
	{
		get
		{
			return isResponseAck;
		}
		set
		{
			isResponseAck = value;
		}
	}

	public bool IsCheckPwd
	{
		get
		{
			return isCheckPwd;
		}
		set
		{
			isCheckPwd = value;
		}
	}

	public bool UseRegistrationPackage
	{
		get
		{
			return useRegistrationPackage;
		}
		set
		{
			useRegistrationPackage = value;
		}
	}

	public event OnClientConnectedDelegate OnClientConnected = null;

	public NetworkAlienClient()
	{
		password = new byte[6];
		trustOnline = new List<string>();
		trustLock = new SimpleHybirdLock();
	}

	protected override async void ThreadPoolLogin(PipeTcpNet pipeTcpNet, IPEndPoint endPoint)
	{
		if (useRegistrationPackage)
		{
			OperateResult<byte[]> check = await pipeTcpNet.ReceiveMessageAsync(new AlienMessage(), null, useActivePush: false);
			if (!check.IsSuccess)
			{
				base.LogNet?.WriteDebug(ToString(), $"Initialize [{endPoint}] DTU failed: " + check.Message);
				return;
			}
			byte[] content = check.Content;
			if (content != null && content.Length < 22)
			{
				pipeTcpNet.CloseCommunication();
				return;
			}
			if (check.Content[0] != 72)
			{
				pipeTcpNet.CloseCommunication();
				return;
			}
			string dtu = Encoding.ASCII.GetString(check.Content, 5, 11).Trim('\0', ' ');
			bool needAckResult = true;
			if (check.Content.Length >= 29 && (check.Content[28] == 1 || check.Content[28] == 49))
			{
				needAckResult = false;
			}
			bool isPasswrodRight = true;
			if (isCheckPwd)
			{
				for (int i = 0; i < password.Length; i++)
				{
					if (check.Content[16 + i] != password[i])
					{
						isPasswrodRight = false;
						break;
					}
				}
			}
			if (!isPasswrodRight)
			{
				if (isResponseAck && needAckResult)
				{
					OperateResult send4 = pipeTcpNet.Send(GetResponse(3));
					if (send4.IsSuccess)
					{
						pipeTcpNet.CloseCommunication();
					}
				}
				else
				{
					pipeTcpNet.CloseCommunication();
				}
				base.LogNet?.WriteWarn(ToString(), $"[{endPoint}] DTU:{dtu} Login Password Wrong, Id:" + dtu);
				return;
			}
			PipeDtuNet pipeDtuNet = new PipeDtuNet(pipeTcpNet)
			{
				DTU = dtu,
				DtuServer = this,
				Pwd = check.Content.SelectMiddle(16, 6).ToHexString()
			};
			if (check.Content.Length >= 28)
			{
				pipeDtuNet.DTUIpAddress = BitConverter.ToInt32(check.Content, 22);
				pipeDtuNet.DTUPort = BitConverter.ToUInt16(check.Content, 26);
			}
			if (!IsClientPermission(pipeDtuNet))
			{
				if (isResponseAck && needAckResult)
				{
					OperateResult send5 = pipeTcpNet.Send(GetResponse(2));
					if (send5.IsSuccess)
					{
						pipeTcpNet.CloseCommunication();
					}
				}
				else
				{
					pipeTcpNet.CloseCommunication();
				}
				base.LogNet?.WriteWarn(ToString(), $"Initialize [{endPoint}] DTU:{dtu} Login Forbidden");
				return;
			}
			int status = IsClientOnline(pipeDtuNet);
			if (status != 0)
			{
				if (isResponseAck && needAckResult)
				{
					OperateResult send6 = pipeTcpNet.Send(GetResponse(1));
					if (send6.IsSuccess)
					{
						pipeTcpNet.CloseCommunication();
					}
				}
				else
				{
					pipeTcpNet.CloseCommunication();
				}
				base.LogNet?.WriteDebug(ToString(), GetMsgFromCode($"Initialize [{endPoint}] DTU:{dtu}", status, "  Ack :" + (isResponseAck && needAckResult)));
				return;
			}
			if (isResponseAck && needAckResult)
			{
				OperateResult send7 = pipeTcpNet.Send(GetResponse(0));
				if (!send7.IsSuccess)
				{
					return;
				}
			}
			base.LogNet?.WriteDebug(ToString(), GetMsgFromCode($"Initialize [{endPoint}] DTU:{dtu}", status, "  Ack :" + (isResponseAck && needAckResult)));
			RaiseClientConnected(pipeDtuNet);
		}
		else
		{
			RaiseClientConnected(new PipeDtuNet(pipeTcpNet)
			{
				DtuServer = this
			});
		}
	}

	private void RaiseClientConnected(PipeDtuNet pipeDtuNet)
	{
		base.LogNet?.WriteDebug(ToString(), $"Dtu Session<{pipeDtuNet}> Connected");
		this.OnClientConnected?.Invoke(pipeDtuNet);
	}

	private byte[] GetResponse(byte status)
	{
		byte[] array = new byte[6] { 72, 115, 110, 0, 1, 0 };
		array[5] = status;
		return array;
	}

	public virtual int IsClientOnline(PipeDtuNet pipe)
	{
		return 0;
	}

	private bool IsClientPermission(PipeDtuNet session)
	{
		bool result = false;
		trustLock.Enter();
		if (trustOnline.Count == 0)
		{
			result = true;
		}
		else
		{
			for (int i = 0; i < trustOnline.Count; i++)
			{
				if (trustOnline[i] == session.DTU)
				{
					result = true;
					break;
				}
			}
		}
		trustLock.Leave();
		return result;
	}

	public void SetPassword(byte[] password)
	{
		if (password != null && password.Length == 6)
		{
			password.CopyTo(this.password, 0);
		}
	}

	public void SetTrustClients(string[] clients)
	{
		trustOnline = new List<string>(clients);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				trustLock?.Dispose();
				this.OnClientConnected = null;
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public override string ToString()
	{
		return "NetworkAlienBase";
	}

	public static string GetMsgFromCode(string head, int code, string info)
	{
		if (1 == 0)
		{
		}
		string result = code switch
		{
			0 => head + "  Login Success" + info, 
			1 => head + "  Login Repeat" + info, 
			2 => head + "  Login Forbidden" + info, 
			3 => head + "  Login Passwod Wrong" + info, 
			_ => head + "  Login Unknow reason:" + info, 
		};
		if (1 == 0)
		{
		}
		return result;
	}
}
