using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Core.Net;
using HslCommunication.Reflection;

namespace HslCommunication.Enthernet.Redis;

public class RedisClient : NetworkDoubleBase
{
	public delegate void RedisMessageReceiveDelegate(string topic, string message);

	private Dictionary<string, RedisClient> clusterClient = new Dictionary<string, RedisClient>();

	private object lockCluster = new object();

	private string password = string.Empty;

	private int dbBlock = 0;

	private Lazy<RedisSubscribe> redisSubscribe;

	public event RedisMessageReceiveDelegate OnRedisMessageReceived;

	public RedisClient(string ipAddress, int port, string password)
	{
		base.ByteTransform = new RegularByteTransform();
		IpAddress = ipAddress;
		Port = port;
		base.ReceiveTimeOut = 30000;
		this.password = password;
		LogMsgFormatBinary = false;
		redisSubscribe = new Lazy<RedisSubscribe>(() => RedisSubscribeInitialize());
	}

	public RedisClient(string password)
	{
		base.ByteTransform = new RegularByteTransform();
		base.ReceiveTimeOut = 30000;
		this.password = password;
		LogMsgFormatBinary = false;
		redisSubscribe = new Lazy<RedisSubscribe>(() => RedisSubscribeInitialize());
	}

	protected override OperateResult InitializationOnConnect(Socket socket)
	{
		if (!string.IsNullOrEmpty(password))
		{
			byte[] send = RedisHelper.PackStringCommand(new string[2] { "AUTH", password });
			OperateResult<byte[]> operateResult = ReadFromCoreServer(socket, send);
			if (!operateResult.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(operateResult);
			}
			string text = Encoding.UTF8.GetString(operateResult.Content);
			if (!text.StartsWith("+"))
			{
				return new OperateResult<string>(text);
			}
		}
		if (dbBlock > 0)
		{
			byte[] send2 = RedisHelper.PackStringCommand(new string[2]
			{
				"SELECT",
				dbBlock.ToString()
			});
			OperateResult<byte[]> operateResult2 = ReadFromCoreServer(socket, send2);
			if (!operateResult2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(operateResult2);
			}
			string text2 = Encoding.UTF8.GetString(operateResult2.Content);
			if (!text2.StartsWith("+"))
			{
				return new OperateResult<string>(text2);
			}
		}
		return base.InitializationOnConnect(socket);
	}

	public override OperateResult<byte[]> ReadFromCoreServer(Socket socket, byte[] send, bool hasResponseData = true, bool usePackHeader = true)
	{
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Send + " : " + (LogMsgFormatBinary ? send.ToHexString(' ') : Encoding.UTF8.GetString(send).Replace("\n", "\\n")));
		OperateResult operateResult = Send(socket, send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(operateResult);
		}
		if (base.ReceiveTimeOut < 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		OperateResult<byte[]> operateResult2 = ReceiveRedisCommand(socket);
		if (operateResult2.IsSuccess)
		{
			base.LogNet?.WriteDebug(ToString(), StringResources.Language.Receive + " : " + (LogMsgFormatBinary ? operateResult2.Content.ToHexString(' ') : Encoding.UTF8.GetString(operateResult2.Content).Replace("\n", "\\n")));
		}
		return operateResult2;
	}

	protected override OperateResult ExtraOnDisconnect(Socket socket)
	{
		closeClusters();
		return base.ExtraOnDisconnect(socket);
	}

	protected override async Task<OperateResult> InitializationOnConnectAsync(Socket socket)
	{
		if (!string.IsNullOrEmpty(password))
		{
			byte[] command2 = RedisHelper.PackStringCommand(new string[2] { "AUTH", password });
			OperateResult<byte[]> read2 = await ReadFromCoreServerAsync(socket, command2);
			if (!read2.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(read2);
			}
			string msg2 = Encoding.UTF8.GetString(read2.Content);
			if (!msg2.StartsWith("+"))
			{
				return new OperateResult<string>(msg2);
			}
		}
		if (dbBlock > 0)
		{
			byte[] command3 = RedisHelper.PackStringCommand(new string[2]
			{
				"SELECT",
				dbBlock.ToString()
			});
			OperateResult<byte[]> read3 = await ReadFromCoreServerAsync(socket, command3);
			if (!read3.IsSuccess)
			{
				return OperateResult.CreateFailedResult<string>(read3);
			}
			string msg3 = Encoding.UTF8.GetString(read3.Content);
			if (!msg3.StartsWith("+"))
			{
				return new OperateResult<string>(msg3);
			}
		}
		return base.InitializationOnConnect(socket);
	}

	public override async Task<OperateResult<byte[]>> ReadFromCoreServerAsync(Socket socket, byte[] send, bool hasResponseData = true, bool usePackHeader = true)
	{
		base.LogNet?.WriteDebug(ToString(), StringResources.Language.Send + " : " + (LogMsgFormatBinary ? send.ToHexString(' ') : Encoding.UTF8.GetString(send).Replace("\n", "\\n")));
		OperateResult sendResult = await SendAsync(socket, send);
		if (!sendResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<byte[]>(sendResult);
		}
		if (base.ReceiveTimeOut < 0)
		{
			return OperateResult.CreateSuccessResult(new byte[0]);
		}
		OperateResult<byte[]> read = await ReceiveRedisCommandAsync(socket);
		if (read.IsSuccess)
		{
			base.LogNet?.WriteDebug(ToString(), StringResources.Language.Receive + " : " + (LogMsgFormatBinary ? read.Content.ToHexString(' ') : Encoding.UTF8.GetString(read.Content).Replace("\n", "\\n")));
		}
		return read;
	}

	protected override async Task<OperateResult> ExtraOnDisconnectAsync(Socket socket)
	{
		closeClusters();
		return await base.ExtraOnDisconnectAsync(socket);
	}

	public OperateResult<string> ReadCustomer(string command)
	{
		byte[] send = RedisHelper.PackStringCommand(command.Split(' '));
		OperateResult<byte[]> operateResult = ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(operateResult);
		}
		return OperateResult.CreateSuccessResult(Encoding.UTF8.GetString(operateResult.Content));
	}

	public async Task<OperateResult<string>> ReadCustomerAsync(string command)
	{
		byte[] byteCommand = RedisHelper.PackStringCommand(command.Split(' '));
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(byteCommand);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string>(read);
		}
		return OperateResult.CreateSuccessResult(Encoding.UTF8.GetString(read.Content));
	}

	private RedisClient getCluster(string ip_port)
	{
		lock (lockCluster)
		{
			if (clusterClient.ContainsKey(ip_port))
			{
				return clusterClient[ip_port];
			}
			string[] array = ip_port.Split(new char[1] { ':' }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length < 2)
			{
				return null;
			}
			if (!int.TryParse(array[1], out var result))
			{
				return null;
			}
			RedisClient redisClient = new RedisClient(array[0], result, password);
			redisClient.SetPersistentConnection();
			clusterClient.Add(ip_port, redisClient);
			return redisClient;
		}
	}

	private void closeClusters()
	{
		lock (lockCluster)
		{
			foreach (RedisClient value in clusterClient.Values)
			{
				value.ConnectClose();
			}
			clusterClient.Clear();
		}
	}

	private OperateResult<string[]> OperateArrayFromServer(string[] commands)
	{
		byte[] send = RedisHelper.PackStringCommand(commands);
		OperateResult<byte[]> operateResult = ReadFromCoreServer(send);
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(operateResult);
		}
		if (operateResult.Content == null || operateResult.Content.Length < 1)
		{
			return new OperateResult<string[]>(("Receive empty data: " + operateResult.Content == null) ? "" : Encoding.UTF8.GetString(operateResult.Content));
		}
		if (operateResult.Content[0] == 36)
		{
			return RedisHelper.GetStringFromCommandLine(operateResult.Content);
		}
		if (operateResult.Content[0] == 42)
		{
			return RedisHelper.GetStringsFromCommandLine(operateResult.Content);
		}
		string text = Encoding.UTF8.GetString(operateResult.Content);
		if (text.StartsWith(":"))
		{
			return OperateResult.CreateSuccessResult(new string[1] { text.TrimEnd('\r', '\n').Substring(1) });
		}
		if (text.StartsWith("+"))
		{
			return OperateResult.CreateSuccessResult(new string[1] { text.Substring(1).TrimEnd('\r', '\n') });
		}
		if (text.StartsWith("-MOVED"))
		{
			RedisClient cluster = getCluster(text.TrimEnd('\r', '\n').Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2]);
			if (cluster == null)
			{
				return new OperateResult<string[]>(text);
			}
			return cluster.OperateArrayFromServer(commands);
		}
		return new OperateResult<string[]>(text);
	}

	public OperateResult<int> OperateNumberFromServer(string[] commands)
	{
		return OperateArrayFromServer(commands).Then((string[] m) => OperateResult.CreateSuccessResult(Convert.ToInt32(m[0])));
	}

	public OperateResult<long> OperateLongNumberFromServer(string[] commands)
	{
		return OperateArrayFromServer(commands).Then((string[] m) => OperateResult.CreateSuccessResult(Convert.ToInt64(m[0])));
	}

	public OperateResult<string> OperateStringFromServer(string[] commands)
	{
		return OperateArrayFromServer(commands).Then((string[] m) => OperateResult.CreateSuccessResult(m[0]));
	}

	public OperateResult<string[]> OperateStringsFromServer(string[] commands)
	{
		return OperateArrayFromServer(commands);
	}

	public OperateResult<string> OperateStatusFromServer(string[] commands)
	{
		return OperateArrayFromServer(commands).Then((string[] m) => OperateResult.CreateSuccessResult(m[0]));
	}

	private async Task<OperateResult<string[]>> OperateArrayFromServerAsync(string[] commands)
	{
		byte[] command = RedisHelper.PackStringCommand(commands);
		OperateResult<byte[]> read = await ReadFromCoreServerAsync(command);
		if (!read.IsSuccess)
		{
			return OperateResult.CreateFailedResult<string[]>(read);
		}
		if (read.Content == null || read.Content.Length < 1)
		{
			return new OperateResult<string[]>(("Receive empty data: " + read.Content == null) ? "" : Encoding.UTF8.GetString(read.Content));
		}
		if (read.Content[0] == 36)
		{
			return RedisHelper.GetStringFromCommandLine(read.Content);
		}
		if (read.Content[0] == 42)
		{
			return RedisHelper.GetStringsFromCommandLine(read.Content);
		}
		string msg = Encoding.UTF8.GetString(read.Content);
		if (msg.StartsWith(":"))
		{
			return OperateResult.CreateSuccessResult(new string[1] { msg.TrimEnd('\r', '\n').Substring(1) });
		}
		if (msg.StartsWith("+"))
		{
			return OperateResult.CreateSuccessResult(new string[1] { msg.Substring(1).TrimEnd('\r', '\n') });
		}
		if (msg.StartsWith("-MOVED"))
		{
			RedisClient client = getCluster(msg.TrimEnd('\r', '\n').Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2]);
			if (client == null)
			{
				return new OperateResult<string[]>(msg);
			}
			return await client.OperateArrayFromServerAsync(commands);
		}
		return new OperateResult<string[]>(msg);
	}

	public async Task<OperateResult<int>> OperateNumberFromServerAsync(string[] commands)
	{
		return (await OperateArrayFromServerAsync(commands)).Then((string[] m) => OperateResult.CreateSuccessResult(Convert.ToInt32(m[0])));
	}

	public async Task<OperateResult<long>> OperateLongNumberFromServerAsync(string[] commands)
	{
		return (await OperateArrayFromServerAsync(commands)).Then((string[] m) => OperateResult.CreateSuccessResult(Convert.ToInt64(m[0])));
	}

	public async Task<OperateResult<string>> OperateStringFromServerAsync(string[] commands)
	{
		return (await OperateArrayFromServerAsync(commands)).Then((string[] m) => OperateResult.CreateSuccessResult(m[0]));
	}

	public async Task<OperateResult<string[]>> OperateStringsFromServerAsync(string[] commands)
	{
		return await OperateArrayFromServerAsync(commands);
	}

	public async Task<OperateResult<string>> OperateStatusFromServerAsync(string[] commands)
	{
		return (await OperateArrayFromServerAsync(commands)).Then((string[] m) => OperateResult.CreateSuccessResult(m[0]));
	}

	public OperateResult<int> DeleteKey(string[] keys)
	{
		return OperateNumberFromServer(SoftBasic.SpliceStringArray("DEL", keys));
	}

	public OperateResult<int> DeleteKey(string key)
	{
		return DeleteKey(new string[1] { key });
	}

	public OperateResult<int> ExistsKey(string key)
	{
		return OperateNumberFromServer(new string[2] { "EXISTS", key });
	}

	public OperateResult<int> ExpireKey(string key, int seconds)
	{
		return OperateNumberFromServer(new string[3]
		{
			"EXPIRE",
			key,
			seconds.ToString()
		});
	}

	public OperateResult<string[]> ReadAllKeys(string pattern)
	{
		return OperateStringsFromServer(new string[2] { "KEYS", pattern });
	}

	public OperateResult MoveKey(string key, int db)
	{
		return OperateStatusFromServer(new string[3]
		{
			"MOVE",
			key,
			db.ToString()
		});
	}

	public OperateResult<int> PersistKey(string key)
	{
		return OperateNumberFromServer(new string[2] { "PERSIST", key });
	}

	public OperateResult<string> ReadRandomKey()
	{
		return OperateStringFromServer(new string[1] { "RANDOMKEY" });
	}

	public OperateResult RenameKey(string key1, string key2)
	{
		return OperateStatusFromServer(new string[3] { "RENAME", key1, key2 });
	}

	public OperateResult<string> ReadKeyType(string key)
	{
		return OperateStatusFromServer(new string[2] { "TYPE", key });
	}

	public OperateResult<int> ReadKeyTTL(string key)
	{
		return OperateNumberFromServer(new string[2] { "TTL", key });
	}

	public async Task<OperateResult<int>> DeleteKeyAsync(string[] keys)
	{
		return await OperateNumberFromServerAsync(SoftBasic.SpliceStringArray("DEL", keys));
	}

	public async Task<OperateResult<int>> DeleteKeyAsync(string key)
	{
		return await DeleteKeyAsync(new string[1] { key });
	}

	public async Task<OperateResult<int>> ExistsKeyAsync(string key)
	{
		return await OperateNumberFromServerAsync(new string[2] { "EXISTS", key });
	}

	public async Task<OperateResult<int>> ExpireKeyAsync(string key, int seconds)
	{
		return await OperateNumberFromServerAsync(new string[3]
		{
			"EXPIRE",
			key,
			seconds.ToString()
		});
	}

	public async Task<OperateResult<string[]>> ReadAllKeysAsync(string pattern)
	{
		return await OperateStringsFromServerAsync(new string[2] { "KEYS", pattern });
	}

	public async Task<OperateResult> MoveKeyAsync(string key, int db)
	{
		return await OperateStatusFromServerAsync(new string[3]
		{
			"MOVE",
			key,
			db.ToString()
		});
	}

	public async Task<OperateResult<int>> PersistKeyAsync(string key)
	{
		return await OperateNumberFromServerAsync(new string[2] { "PERSIST", key });
	}

	public async Task<OperateResult<string>> ReadRandomKeyAsync()
	{
		return await OperateStringFromServerAsync(new string[1] { "RANDOMKEY" });
	}

	public async Task<OperateResult> RenameKeyAsync(string key1, string key2)
	{
		return await OperateStatusFromServerAsync(new string[3] { "RENAME", key1, key2 });
	}

	public async Task<OperateResult<string>> ReadKeyTypeAsync(string key)
	{
		return await OperateStatusFromServerAsync(new string[2] { "TYPE", key });
	}

	public async Task<OperateResult<int>> ReadKeyTTLAsync(string key)
	{
		return await OperateNumberFromServerAsync(new string[2] { "TTL", key });
	}

	public OperateResult<int> AppendKey(string key, string value)
	{
		return OperateNumberFromServer(new string[3] { "APPEND", key, value });
	}

	public OperateResult<long> DecrementKey(string key)
	{
		return OperateLongNumberFromServer(new string[2] { "DECR", key });
	}

	public OperateResult<long> DecrementKey(string key, long value)
	{
		return OperateLongNumberFromServer(new string[3]
		{
			"DECRBY",
			key,
			value.ToString()
		});
	}

	public OperateResult<string> ReadKey(string key)
	{
		return OperateStringFromServer(new string[2] { "GET", key });
	}

	public OperateResult<string> ReadKeyRange(string key, int start, int end)
	{
		return OperateStringFromServer(new string[4]
		{
			"GETRANGE",
			key,
			start.ToString(),
			end.ToString()
		});
	}

	public OperateResult<string> ReadAndWriteKey(string key, string value)
	{
		return OperateStringFromServer(new string[3] { "GETSET", key, value });
	}

	public OperateResult<long> IncrementKey(string key)
	{
		return OperateLongNumberFromServer(new string[2] { "INCR", key });
	}

	public OperateResult<long> IncrementKey(string key, long value)
	{
		return OperateLongNumberFromServer(new string[3]
		{
			"INCRBY",
			key,
			value.ToString()
		});
	}

	public OperateResult<string> IncrementKey(string key, float value)
	{
		return OperateStringFromServer(new string[3]
		{
			"INCRBYFLOAT",
			key,
			value.ToString()
		});
	}

	public OperateResult<string[]> ReadKey(string[] keys)
	{
		return OperateStringsFromServer(SoftBasic.SpliceStringArray("MGET", keys));
	}

	public OperateResult WriteKey(string[] keys, string[] values)
	{
		if (keys == null)
		{
			throw new ArgumentNullException("keys");
		}
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		if (keys.Length != values.Length)
		{
			throw new ArgumentException("Two arguement not same length");
		}
		List<string> list = new List<string>();
		list.Add("MSET");
		for (int i = 0; i < keys.Length; i++)
		{
			list.Add(keys[i]);
			list.Add(values[i]);
		}
		return OperateStatusFromServer(list.ToArray());
	}

	public OperateResult WriteKey(string key, string value)
	{
		return OperateStatusFromServer(new string[3] { "SET", key, value });
	}

	public OperateResult WriteAndPublishKey(string key, string value)
	{
		OperateResult operateResult = WriteKey(key, value);
		if (!operateResult.IsSuccess)
		{
			return operateResult;
		}
		return Publish(key, value);
	}

	public OperateResult WriteExpireKey(string key, string value, long seconds)
	{
		return OperateStatusFromServer(new string[4]
		{
			"SETEX",
			key,
			seconds.ToString(),
			value
		});
	}

	public OperateResult<int> WriteKeyIfNotExists(string key, string value)
	{
		return OperateNumberFromServer(new string[3] { "SETNX", key, value });
	}

	public OperateResult<int> WriteKeyRange(string key, string value, int offset)
	{
		return OperateNumberFromServer(new string[4]
		{
			"SETRANGE",
			key,
			offset.ToString(),
			value
		});
	}

	public OperateResult<int> ReadKeyLength(string key)
	{
		return OperateNumberFromServer(new string[2] { "STRLEN", key });
	}

	public async Task<OperateResult<int>> AppendKeyAsync(string key, string value)
	{
		return await OperateNumberFromServerAsync(new string[3] { "APPEND", key, value });
	}

	public async Task<OperateResult<long>> DecrementKeyAsync(string key)
	{
		return await OperateLongNumberFromServerAsync(new string[2] { "DECR", key });
	}

	public async Task<OperateResult<long>> DecrementKeyAsync(string key, long value)
	{
		return await OperateLongNumberFromServerAsync(new string[3]
		{
			"DECRBY",
			key,
			value.ToString()
		});
	}

	public async Task<OperateResult<string>> ReadKeyAsync(string key)
	{
		return await OperateStringFromServerAsync(new string[2] { "GET", key });
	}

	public async Task<OperateResult<string>> ReadKeyRangeAsync(string key, int start, int end)
	{
		return await OperateStringFromServerAsync(new string[4]
		{
			"GETRANGE",
			key,
			start.ToString(),
			end.ToString()
		});
	}

	public async Task<OperateResult<string>> ReadAndWriteKeyAsync(string key, string value)
	{
		return await OperateStringFromServerAsync(new string[3] { "GETSET", key, value });
	}

	public async Task<OperateResult<long>> IncrementKeyAsync(string key)
	{
		return await OperateLongNumberFromServerAsync(new string[2] { "INCR", key });
	}

	public async Task<OperateResult<long>> IncrementKeyAsync(string key, long value)
	{
		return await OperateLongNumberFromServerAsync(new string[3]
		{
			"INCRBY",
			key,
			value.ToString()
		});
	}

	public async Task<OperateResult<string>> IncrementKeyAsync(string key, float value)
	{
		return await OperateStringFromServerAsync(new string[3]
		{
			"INCRBYFLOAT",
			key,
			value.ToString()
		});
	}

	public async Task<OperateResult<string[]>> ReadKeyAsync(string[] keys)
	{
		return await OperateStringsFromServerAsync(SoftBasic.SpliceStringArray("MGET", keys));
	}

	public async Task<OperateResult> WriteKeyAsync(string[] keys, string[] values)
	{
		if (keys == null)
		{
			throw new ArgumentNullException("keys");
		}
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		if (keys.Length != values.Length)
		{
			throw new ArgumentException("Two arguement not same length");
		}
		List<string> list = new List<string> { "MSET" };
		for (int i = 0; i < keys.Length; i++)
		{
			list.Add(keys[i]);
			list.Add(values[i]);
		}
		return await OperateStatusFromServerAsync(list.ToArray());
	}

	public async Task<OperateResult> WriteKeyAsync(string key, string value)
	{
		return await OperateStatusFromServerAsync(new string[3] { "SET", key, value });
	}

	public async Task<OperateResult> WriteAndPublishKeyAsync(string key, string value)
	{
		OperateResult write = await WriteKeyAsync(key, value);
		if (!write.IsSuccess)
		{
			return write;
		}
		return await PublishAsync(key, value);
	}

	public async Task<OperateResult> WriteExpireKeyAsync(string key, string value, long seconds)
	{
		return await OperateStatusFromServerAsync(new string[4]
		{
			"SETEX",
			key,
			seconds.ToString(),
			value
		});
	}

	public async Task<OperateResult<int>> WriteKeyIfNotExistsAsync(string key, string value)
	{
		return await OperateNumberFromServerAsync(new string[3] { "SETNX", key, value });
	}

	public async Task<OperateResult<int>> WriteKeyRangeAsync(string key, string value, int offset)
	{
		return await OperateNumberFromServerAsync(new string[4]
		{
			"SETRANGE",
			key,
			offset.ToString(),
			value
		});
	}

	public async Task<OperateResult<int>> ReadKeyLengthAsync(string key)
	{
		return await OperateNumberFromServerAsync(new string[2] { "STRLEN", key });
	}

	public OperateResult<int> ListInsertBefore(string key, string value, string pivot)
	{
		return OperateNumberFromServer(new string[5] { "LINSERT", key, "BEFORE", pivot, value });
	}

	public OperateResult<int> ListInsertAfter(string key, string value, string pivot)
	{
		return OperateNumberFromServer(new string[5] { "LINSERT", key, "AFTER", pivot, value });
	}

	public OperateResult<int> GetListLength(string key)
	{
		return OperateNumberFromServer(new string[2] { "LLEN", key });
	}

	public OperateResult<string> ReadListByIndex(string key, long index)
	{
		return OperateStringFromServer(new string[3]
		{
			"LINDEX",
			key,
			index.ToString()
		});
	}

	public OperateResult<string> ListLeftPop(string key)
	{
		return OperateStringFromServer(new string[2] { "LPOP", key });
	}

	public OperateResult<int> ListLeftPush(string key, string value)
	{
		return ListLeftPush(key, new string[1] { value });
	}

	public OperateResult<int> ListLeftPush(string key, string[] values)
	{
		return OperateNumberFromServer(SoftBasic.SpliceStringArray("LPUSH", key, values));
	}

	public OperateResult<int> ListLeftPushX(string key, string value)
	{
		return OperateNumberFromServer(new string[3] { "LPUSHX", key, value });
	}

	public OperateResult<string[]> ListRange(string key, long start, long stop)
	{
		return OperateStringsFromServer(new string[4]
		{
			"LRANGE",
			key,
			start.ToString(),
			stop.ToString()
		});
	}

	public OperateResult<int> ListRemoveElementMatch(string key, long count, string value)
	{
		return OperateNumberFromServer(new string[4]
		{
			"LREM",
			key,
			count.ToString(),
			value
		});
	}

	public OperateResult ListSet(string key, long index, string value)
	{
		return OperateStatusFromServer(new string[4]
		{
			"LSET",
			key.ToString(),
			index.ToString(),
			value
		});
	}

	public OperateResult ListTrim(string key, long start, long end)
	{
		return OperateStatusFromServer(new string[4]
		{
			"LTRIM",
			key,
			start.ToString(),
			end.ToString()
		});
	}

	public OperateResult<string> ListRightPop(string key)
	{
		return OperateStringFromServer(new string[2] { "RPOP", key });
	}

	public OperateResult<string> ListRightPopLeftPush(string key1, string key2)
	{
		return OperateStringFromServer(new string[3] { "RPOPLPUSH", key1, key2 });
	}

	public OperateResult<int> ListRightPush(string key, string value)
	{
		return ListRightPush(key, new string[1] { value });
	}

	public OperateResult<int> ListRightPush(string key, string[] values)
	{
		return OperateNumberFromServer(SoftBasic.SpliceStringArray("RPUSH", key, values));
	}

	public OperateResult<int> ListRightPushX(string key, string value)
	{
		return OperateNumberFromServer(new string[3] { "RPUSHX", key, value });
	}

	public async Task<OperateResult<int>> ListInsertBeforeAsync(string key, string value, string pivot)
	{
		return await OperateNumberFromServerAsync(new string[5] { "LINSERT", key, "BEFORE", pivot, value });
	}

	public async Task<OperateResult<int>> ListInsertAfterAsync(string key, string value, string pivot)
	{
		return await OperateNumberFromServerAsync(new string[5] { "LINSERT", key, "AFTER", pivot, value });
	}

	public async Task<OperateResult<int>> GetListLengthAsync(string key)
	{
		return await OperateNumberFromServerAsync(new string[2] { "LLEN", key });
	}

	public async Task<OperateResult<string>> ReadListByIndexAsync(string key, long index)
	{
		return await OperateStringFromServerAsync(new string[3]
		{
			"LINDEX",
			key,
			index.ToString()
		});
	}

	public async Task<OperateResult<string>> ListLeftPopAsync(string key)
	{
		return await OperateStringFromServerAsync(new string[2] { "LPOP", key });
	}

	public async Task<OperateResult<int>> ListLeftPushAsync(string key, string value)
	{
		return await ListLeftPushAsync(key, new string[1] { value });
	}

	public async Task<OperateResult<int>> ListLeftPushAsync(string key, string[] values)
	{
		return await OperateNumberFromServerAsync(SoftBasic.SpliceStringArray("LPUSH", key, values));
	}

	public async Task<OperateResult<int>> ListLeftPushXAsync(string key, string value)
	{
		return await OperateNumberFromServerAsync(new string[3] { "LPUSHX", key, value });
	}

	public async Task<OperateResult<string[]>> ListRangeAsync(string key, long start, long stop)
	{
		return await OperateStringsFromServerAsync(new string[4]
		{
			"LRANGE",
			key,
			start.ToString(),
			stop.ToString()
		});
	}

	public async Task<OperateResult<int>> ListRemoveElementMatchAsync(string key, long count, string value)
	{
		return await OperateNumberFromServerAsync(new string[4]
		{
			"LREM",
			key,
			count.ToString(),
			value
		});
	}

	public async Task<OperateResult> ListSetAsync(string key, long index, string value)
	{
		return await OperateStatusFromServerAsync(new string[4]
		{
			"LSET",
			key.ToString(),
			index.ToString(),
			value
		});
	}

	public async Task<OperateResult> ListTrimAsync(string key, long start, long end)
	{
		return await OperateStatusFromServerAsync(new string[4]
		{
			"LTRIM",
			key,
			start.ToString(),
			end.ToString()
		});
	}

	public async Task<OperateResult<string>> ListRightPopAsync(string key)
	{
		return await OperateStringFromServerAsync(new string[2] { "RPOP", key });
	}

	public async Task<OperateResult<string>> ListRightPopLeftPushAsync(string key1, string key2)
	{
		return await OperateStringFromServerAsync(new string[3] { "RPOPLPUSH", key1, key2 });
	}

	public async Task<OperateResult<int>> ListRightPushAsync(string key, string value)
	{
		return await ListRightPushAsync(key, new string[1] { value });
	}

	public async Task<OperateResult<int>> ListRightPushAsync(string key, string[] values)
	{
		return await OperateNumberFromServerAsync(SoftBasic.SpliceStringArray("RPUSH", key, values));
	}

	public async Task<OperateResult<int>> ListRightPushXAsync(string key, string value)
	{
		return await OperateNumberFromServerAsync(new string[3] { "RPUSHX", key, value });
	}

	public OperateResult<int> DeleteHashKey(string key, string field)
	{
		return DeleteHashKey(key, new string[1] { field });
	}

	public OperateResult<int> DeleteHashKey(string key, string[] fields)
	{
		return OperateNumberFromServer(SoftBasic.SpliceStringArray("HDEL", key, fields));
	}

	public OperateResult<int> ExistsHashKey(string key, string field)
	{
		return OperateNumberFromServer(new string[3] { "HEXISTS", key, field });
	}

	public OperateResult<string> ReadHashKey(string key, string field)
	{
		return OperateStringFromServer(new string[3] { "HGET", key, field });
	}

	public OperateResult<string[]> ReadHashKeyAll(string key)
	{
		return OperateStringsFromServer(new string[2] { "HGETALL", key });
	}

	public OperateResult<long> IncrementHashKey(string key, string field, long value)
	{
		return OperateLongNumberFromServer(new string[4]
		{
			"HINCRBY",
			key,
			field,
			value.ToString()
		});
	}

	public OperateResult<string> IncrementHashKey(string key, string field, float value)
	{
		return OperateStringFromServer(new string[4]
		{
			"HINCRBYFLOAT",
			key,
			field,
			value.ToString()
		});
	}

	public OperateResult<string[]> ReadHashKeys(string key)
	{
		return OperateStringsFromServer(new string[2] { "HKEYS", key });
	}

	public OperateResult<int> ReadHashKeyLength(string key)
	{
		return OperateNumberFromServer(new string[2] { "HLEN", key });
	}

	public OperateResult<string[]> ReadHashKey(string key, string[] fields)
	{
		return OperateStringsFromServer(SoftBasic.SpliceStringArray("HMGET", key, fields));
	}

	public OperateResult<int> WriteHashKey(string key, string field, string value)
	{
		return OperateNumberFromServer(new string[4] { "HSET", key, field, value });
	}

	public OperateResult WriteHashKey(string key, string[] fields, string[] values)
	{
		if (fields == null)
		{
			throw new ArgumentNullException("fields");
		}
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		if (fields.Length != values.Length)
		{
			throw new ArgumentException("Two arguement not same length");
		}
		List<string> list = new List<string>();
		list.Add("HMSET");
		list.Add(key);
		for (int i = 0; i < fields.Length; i++)
		{
			list.Add(fields[i]);
			list.Add(values[i]);
		}
		return OperateStatusFromServer(list.ToArray());
	}

	public OperateResult<int> WriteHashKeyNx(string key, string field, string value)
	{
		return OperateNumberFromServer(new string[4] { "HSETNX", key, field, value });
	}

	public OperateResult<string[]> ReadHashValues(string key)
	{
		return OperateStringsFromServer(new string[2] { "HVALS", key });
	}

	public async Task<OperateResult<int>> DeleteHashKeyAsync(string key, string field)
	{
		return await DeleteHashKeyAsync(key, new string[1] { field });
	}

	public async Task<OperateResult<int>> DeleteHashKeyAsync(string key, string[] fields)
	{
		return await OperateNumberFromServerAsync(SoftBasic.SpliceStringArray("HDEL", key, fields));
	}

	public async Task<OperateResult<int>> ExistsHashKeyAsync(string key, string field)
	{
		return await OperateNumberFromServerAsync(new string[3] { "HEXISTS", key, field });
	}

	public async Task<OperateResult<string>> ReadHashKeyAsync(string key, string field)
	{
		return await OperateStringFromServerAsync(new string[3] { "HGET", key, field });
	}

	public async Task<OperateResult<string[]>> ReadHashKeyAllAsync(string key)
	{
		return await OperateStringsFromServerAsync(new string[2] { "HGETALL", key });
	}

	public async Task<OperateResult<long>> IncrementHashKeyAsync(string key, string field, long value)
	{
		return await OperateLongNumberFromServerAsync(new string[4]
		{
			"HINCRBY",
			key,
			field,
			value.ToString()
		});
	}

	public async Task<OperateResult<string>> IncrementHashKeyAsync(string key, string field, float value)
	{
		return await OperateStringFromServerAsync(new string[4]
		{
			"HINCRBYFLOAT",
			key,
			field,
			value.ToString()
		});
	}

	public async Task<OperateResult<string[]>> ReadHashKeysAsync(string key)
	{
		return await OperateStringsFromServerAsync(new string[2] { "HKEYS", key });
	}

	public async Task<OperateResult<int>> ReadHashKeyLengthAsync(string key)
	{
		return await OperateNumberFromServerAsync(new string[2] { "HLEN", key });
	}

	public async Task<OperateResult<string[]>> ReadHashKeyAsync(string key, string[] fields)
	{
		return await OperateStringsFromServerAsync(SoftBasic.SpliceStringArray("HMGET", key, fields));
	}

	public async Task<OperateResult<int>> WriteHashKeyAsync(string key, string field, string value)
	{
		return await OperateNumberFromServerAsync(new string[4] { "HSET", key, field, value });
	}

	public async Task<OperateResult> WriteHashKeyAsync(string key, string[] fields, string[] values)
	{
		if (fields == null)
		{
			throw new ArgumentNullException("fields");
		}
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		if (fields.Length != values.Length)
		{
			throw new ArgumentException("Two arguement not same length");
		}
		List<string> list = new List<string> { "HMSET", key };
		for (int i = 0; i < fields.Length; i++)
		{
			list.Add(fields[i]);
			list.Add(values[i]);
		}
		return await OperateStatusFromServerAsync(list.ToArray());
	}

	public async Task<OperateResult<int>> WriteHashKeyNxAsync(string key, string field, string value)
	{
		return await OperateNumberFromServerAsync(new string[4] { "HSETNX", key, field, value });
	}

	public async Task<OperateResult<string[]>> ReadHashValuesAsync(string key)
	{
		return await OperateStringsFromServerAsync(new string[2] { "HVALS", key });
	}

	public OperateResult<int> SetAdd(string key, string member)
	{
		return SetAdd(key, new string[1] { member });
	}

	public OperateResult<int> SetAdd(string key, string[] members)
	{
		return OperateNumberFromServer(SoftBasic.SpliceStringArray("SADD", key, members));
	}

	public OperateResult<int> SetCard(string key)
	{
		return OperateNumberFromServer(new string[2] { "SCARD", key });
	}

	public OperateResult<string[]> SetDiff(string key, string diffKey)
	{
		return SetDiff(key, new string[1] { diffKey });
	}

	public OperateResult<string[]> SetDiff(string key, string[] diffKeys)
	{
		return OperateStringsFromServer(SoftBasic.SpliceStringArray("SDIFF", key, diffKeys));
	}

	public OperateResult<int> SetDiffStore(string destination, string key, string diffKey)
	{
		return SetDiffStore(destination, key, new string[1] { diffKey });
	}

	public OperateResult<int> SetDiffStore(string destination, string key, string[] diffKeys)
	{
		return OperateNumberFromServer(SoftBasic.SpliceStringArray("SDIFFSTORE", destination, key, diffKeys));
	}

	public OperateResult<string[]> SetInter(string key, string interKey)
	{
		return SetInter(key, new string[1] { interKey });
	}

	public OperateResult<string[]> SetInter(string key, string[] interKeys)
	{
		return OperateStringsFromServer(SoftBasic.SpliceStringArray("SINTER", key, interKeys));
	}

	public OperateResult<int> SetInterStore(string destination, string key, string interKey)
	{
		return SetInterStore(destination, key, new string[1] { interKey });
	}

	public OperateResult<int> SetInterStore(string destination, string key, string[] interKeys)
	{
		return OperateNumberFromServer(SoftBasic.SpliceStringArray("SINTERSTORE", destination, key, interKeys));
	}

	public OperateResult<int> SetIsMember(string key, string member)
	{
		return OperateNumberFromServer(new string[3] { "SISMEMBER", key, member });
	}

	public OperateResult<string[]> SetMembers(string key)
	{
		return OperateStringsFromServer(new string[2] { "SMEMBERS", key });
	}

	public OperateResult<int> SetMove(string source, string destination, string member)
	{
		return OperateNumberFromServer(new string[4] { "SMOVE", source, destination, member });
	}

	public OperateResult<string> SetPop(string key)
	{
		return OperateStringFromServer(new string[2] { "SPOP", key });
	}

	public OperateResult<string> SetRandomMember(string key)
	{
		return OperateStringFromServer(new string[2] { "SRANDMEMBER", key });
	}

	public OperateResult<string[]> SetRandomMember(string key, int count)
	{
		return OperateStringsFromServer(new string[3]
		{
			"SRANDMEMBER",
			key,
			count.ToString()
		});
	}

	public OperateResult<int> SetRemove(string key, string member)
	{
		return SetRemove(key, new string[1] { member });
	}

	public OperateResult<int> SetRemove(string key, string[] members)
	{
		return OperateNumberFromServer(SoftBasic.SpliceStringArray("SREM", key, members));
	}

	public OperateResult<string[]> SetUnion(string key, string unionKey)
	{
		return SetUnion(key, new string[1] { unionKey });
	}

	public OperateResult<string[]> SetUnion(string key, string[] unionKeys)
	{
		return OperateStringsFromServer(SoftBasic.SpliceStringArray("SUNION", key, unionKeys));
	}

	public OperateResult<int> SetUnionStore(string destination, string key, string unionKey)
	{
		return SetUnionStore(destination, key, unionKey);
	}

	public OperateResult<int> SetUnionStore(string destination, string key, string[] unionKeys)
	{
		return OperateNumberFromServer(SoftBasic.SpliceStringArray("SUNIONSTORE", destination, key, unionKeys));
	}

	public async Task<OperateResult<int>> SetAddAsync(string key, string member)
	{
		return await SetAddAsync(key, new string[1] { member });
	}

	public async Task<OperateResult<int>> SetAddAsync(string key, string[] members)
	{
		return await OperateNumberFromServerAsync(SoftBasic.SpliceStringArray("SADD", key, members));
	}

	public async Task<OperateResult<int>> SetCardAsync(string key)
	{
		return await OperateNumberFromServerAsync(new string[2] { "SCARD", key });
	}

	public async Task<OperateResult<string[]>> SetDiffAsync(string key, string diffKey)
	{
		return await SetDiffAsync(key, new string[1] { diffKey });
	}

	public async Task<OperateResult<string[]>> SetDiffAsync(string key, string[] diffKeys)
	{
		return await OperateStringsFromServerAsync(SoftBasic.SpliceStringArray("SDIFF", key, diffKeys));
	}

	public async Task<OperateResult<int>> SetDiffStoreAsync(string destination, string key, string diffKey)
	{
		return await SetDiffStoreAsync(destination, key, new string[1] { diffKey });
	}

	public async Task<OperateResult<int>> SetDiffStoreAsync(string destination, string key, string[] diffKeys)
	{
		return await OperateNumberFromServerAsync(SoftBasic.SpliceStringArray("SDIFFSTORE", destination, key, diffKeys));
	}

	public async Task<OperateResult<string[]>> SetInterAsync(string key, string interKey)
	{
		return await SetInterAsync(key, new string[1] { interKey });
	}

	public async Task<OperateResult<string[]>> SetInterAsync(string key, string[] interKeys)
	{
		return await OperateStringsFromServerAsync(SoftBasic.SpliceStringArray("SINTER", key, interKeys));
	}

	public async Task<OperateResult<int>> SetInterStoreAsync(string destination, string key, string interKey)
	{
		return await SetInterStoreAsync(destination, key, new string[1] { interKey });
	}

	public async Task<OperateResult<int>> SetInterStoreAsync(string destination, string key, string[] interKeys)
	{
		return await OperateNumberFromServerAsync(SoftBasic.SpliceStringArray("SINTERSTORE", destination, key, interKeys));
	}

	public async Task<OperateResult<int>> SetIsMemberAsync(string key, string member)
	{
		return await OperateNumberFromServerAsync(new string[3] { "SISMEMBER", key, member });
	}

	public async Task<OperateResult<string[]>> SetMembersAsync(string key)
	{
		return await OperateStringsFromServerAsync(new string[2] { "SMEMBERS", key });
	}

	public async Task<OperateResult<int>> SetMoveAsync(string source, string destination, string member)
	{
		return await OperateNumberFromServerAsync(new string[4] { "SMOVE", source, destination, member });
	}

	public async Task<OperateResult<string>> SetPopAsync(string key)
	{
		return await OperateStringFromServerAsync(new string[2] { "SPOP", key });
	}

	public async Task<OperateResult<string>> SetRandomMemberAsync(string key)
	{
		return await OperateStringFromServerAsync(new string[2] { "SRANDMEMBER", key });
	}

	public async Task<OperateResult<string[]>> SetRandomMemberAsync(string key, int count)
	{
		return await OperateStringsFromServerAsync(new string[3]
		{
			"SRANDMEMBER",
			key,
			count.ToString()
		});
	}

	public async Task<OperateResult<int>> SetRemoveAsync(string key, string member)
	{
		return await SetRemoveAsync(key, new string[1] { member });
	}

	public async Task<OperateResult<int>> SetRemoveAsync(string key, string[] members)
	{
		return await OperateNumberFromServerAsync(SoftBasic.SpliceStringArray("SREM", key, members));
	}

	public async Task<OperateResult<string[]>> SetUnionAsync(string key, string unionKey)
	{
		return await SetUnionAsync(key, new string[1] { unionKey });
	}

	public async Task<OperateResult<string[]>> SetUnionAsync(string key, string[] unionKeys)
	{
		return await OperateStringsFromServerAsync(SoftBasic.SpliceStringArray("SUNION", key, unionKeys));
	}

	public async Task<OperateResult<int>> SetUnionStoreAsync(string destination, string key, string unionKey)
	{
		return await SetUnionStoreAsync(destination, key, unionKey);
	}

	public async Task<OperateResult<int>> SetUnionStoreAsync(string destination, string key, string[] unionKeys)
	{
		return await OperateNumberFromServerAsync(SoftBasic.SpliceStringArray("SUNIONSTORE", destination, key, unionKeys));
	}

	public OperateResult<int> ZSetAdd(string key, string member, double score)
	{
		return ZSetAdd(key, new string[1] { member }, new double[1] { score });
	}

	public OperateResult<int> ZSetAdd(string key, string[] members, double[] scores)
	{
		if (members.Length != scores.Length)
		{
			throw new Exception(StringResources.Language.TwoParametersLengthIsNotSame);
		}
		List<string> list = new List<string>();
		list.Add("ZADD");
		list.Add(key);
		for (int i = 0; i < members.Length; i++)
		{
			list.Add(scores[i].ToString());
			list.Add(members[i]);
		}
		return OperateNumberFromServer(list.ToArray());
	}

	public OperateResult<int> ZSetCard(string key)
	{
		return OperateNumberFromServer(new string[2] { "ZCARD", key });
	}

	public OperateResult<int> ZSetCount(string key, double min, double max)
	{
		return OperateNumberFromServer(new string[4]
		{
			"ZCOUNT",
			key,
			min.ToString(),
			max.ToString()
		});
	}

	public OperateResult<string> ZSetIncreaseBy(string key, string member, double increment)
	{
		return OperateStringFromServer(new string[4]
		{
			"ZINCRBY",
			key,
			increment.ToString(),
			member
		});
	}

	public OperateResult<string[]> ZSetRange(string key, int start, int stop, bool withScore = false)
	{
		if (withScore)
		{
			return OperateStringsFromServer(new string[5]
			{
				"ZRANGE",
				key,
				start.ToString(),
				stop.ToString(),
				"WITHSCORES"
			});
		}
		return OperateStringsFromServer(new string[4]
		{
			"ZRANGE",
			key,
			start.ToString(),
			stop.ToString()
		});
	}

	public OperateResult<string[]> ZSetRangeByScore(string key, string min, string max, bool withScore = false)
	{
		if (withScore)
		{
			return OperateStringsFromServer(new string[5] { "ZRANGEBYSCORE", key, min, max, "WITHSCORES" });
		}
		return OperateStringsFromServer(new string[4] { "ZRANGEBYSCORE", key, min, max });
	}

	public OperateResult<int> ZSetRank(string key, string member)
	{
		return OperateNumberFromServer(new string[3] { "ZRANK", key, member });
	}

	public OperateResult<int> ZSetRemove(string key, string member)
	{
		return ZSetRemove(key, new string[1] { member });
	}

	public OperateResult<int> ZSetRemove(string key, string[] members)
	{
		return OperateNumberFromServer(SoftBasic.SpliceStringArray("ZREM", key, members));
	}

	public OperateResult<int> ZSetRemoveRangeByRank(string key, int start, int stop)
	{
		return OperateNumberFromServer(new string[4]
		{
			"ZREMRANGEBYRANK",
			key,
			start.ToString(),
			stop.ToString()
		});
	}

	public OperateResult<int> ZSetRemoveRangeByScore(string key, string min, string max)
	{
		return OperateNumberFromServer(new string[4] { "ZREMRANGEBYSCORE", key, min, max });
	}

	public OperateResult<string[]> ZSetReverseRange(string key, int start, int stop, bool withScore = false)
	{
		if (withScore)
		{
			return OperateStringsFromServer(new string[5]
			{
				"ZREVRANGE",
				key,
				start.ToString(),
				stop.ToString(),
				"WITHSCORES"
			});
		}
		return OperateStringsFromServer(new string[4]
		{
			"ZREVRANGE",
			key,
			start.ToString(),
			stop.ToString()
		});
	}

	public OperateResult<string[]> ZSetReverseRangeByScore(string key, string max, string min, bool withScore = false)
	{
		if (withScore)
		{
			return OperateStringsFromServer(new string[5] { "ZREVRANGEBYSCORE", key, max, min, "WITHSCORES" });
		}
		return OperateStringsFromServer(new string[4] { "ZREVRANGEBYSCORE", key, max, min });
	}

	public OperateResult<int> ZSetReverseRank(string key, string member)
	{
		return OperateNumberFromServer(new string[3] { "ZREVRANK", key, member });
	}

	public OperateResult<string> ZSetScore(string key, string member)
	{
		return OperateStringFromServer(new string[3] { "ZSCORE", key, member });
	}

	public async Task<OperateResult<int>> ZSetAddAsync(string key, string member, double score)
	{
		return await ZSetAddAsync(key, new string[1] { member }, new double[1] { score });
	}

	public async Task<OperateResult<int>> ZSetAddAsync(string key, string[] members, double[] scores)
	{
		if (members.Length != scores.Length)
		{
			throw new Exception(StringResources.Language.TwoParametersLengthIsNotSame);
		}
		List<string> lists = new List<string> { "ZADD", key };
		for (int i = 0; i < members.Length; i++)
		{
			lists.Add(scores[i].ToString());
			lists.Add(members[i]);
		}
		return await OperateNumberFromServerAsync(lists.ToArray());
	}

	public async Task<OperateResult<int>> ZSetCardAsync(string key)
	{
		return await OperateNumberFromServerAsync(new string[2] { "ZCARD", key });
	}

	public async Task<OperateResult<int>> ZSetCountAsync(string key, double min, double max)
	{
		return await OperateNumberFromServerAsync(new string[4]
		{
			"ZCOUNT",
			key,
			min.ToString(),
			max.ToString()
		});
	}

	public async Task<OperateResult<string>> ZSetIncreaseByAsync(string key, string member, double increment)
	{
		return await OperateStringFromServerAsync(new string[4]
		{
			"ZINCRBY",
			key,
			increment.ToString(),
			member
		});
	}

	public async Task<OperateResult<string[]>> ZSetRangeAsync(string key, int start, int stop, bool withScore = false)
	{
		if (withScore)
		{
			return await OperateStringsFromServerAsync(new string[5]
			{
				"ZRANGE",
				key,
				start.ToString(),
				stop.ToString(),
				"WITHSCORES"
			});
		}
		return await OperateStringsFromServerAsync(new string[4]
		{
			"ZRANGE",
			key,
			start.ToString(),
			stop.ToString()
		});
	}

	public async Task<OperateResult<string[]>> ZSetRangeByScoreAsync(string key, string min, string max, bool withScore = false)
	{
		if (withScore)
		{
			return await OperateStringsFromServerAsync(new string[5] { "ZRANGEBYSCORE", key, min, max, "WITHSCORES" });
		}
		return await OperateStringsFromServerAsync(new string[4] { "ZRANGEBYSCORE", key, min, max });
	}

	public async Task<OperateResult<int>> ZSetRankAsync(string key, string member)
	{
		return await OperateNumberFromServerAsync(new string[3] { "ZRANK", key, member });
	}

	public async Task<OperateResult<int>> ZSetRemoveAsync(string key, string member)
	{
		return await ZSetRemoveAsync(key, new string[1] { member });
	}

	public async Task<OperateResult<int>> ZSetRemoveAsync(string key, string[] members)
	{
		return await OperateNumberFromServerAsync(SoftBasic.SpliceStringArray("ZREM", key, members));
	}

	public async Task<OperateResult<int>> ZSetRemoveRangeByRankAsync(string key, int start, int stop)
	{
		return await OperateNumberFromServerAsync(new string[4]
		{
			"ZREMRANGEBYRANK",
			key,
			start.ToString(),
			stop.ToString()
		});
	}

	public async Task<OperateResult<int>> ZSetRemoveRangeByScoreAsync(string key, string min, string max)
	{
		return await OperateNumberFromServerAsync(new string[4] { "ZREMRANGEBYSCORE", key, min, max });
	}

	public async Task<OperateResult<string[]>> ZSetReverseRangeAsync(string key, int start, int stop, bool withScore = false)
	{
		if (withScore)
		{
			return await OperateStringsFromServerAsync(new string[5]
			{
				"ZREVRANGE",
				key,
				start.ToString(),
				stop.ToString(),
				"WITHSCORES"
			});
		}
		return await OperateStringsFromServerAsync(new string[4]
		{
			"ZREVRANGE",
			key,
			start.ToString(),
			stop.ToString()
		});
	}

	public async Task<OperateResult<string[]>> ZSetReverseRangeByScoreAsync(string key, string max, string min, bool withScore = false)
	{
		if (withScore)
		{
			return await OperateStringsFromServerAsync(new string[5] { "ZREVRANGEBYSCORE", key, max, min, "WITHSCORES" });
		}
		return await OperateStringsFromServerAsync(new string[4] { "ZREVRANGEBYSCORE", key, max, min });
	}

	public async Task<OperateResult<int>> ZSetReverseRankAsync(string key, string member)
	{
		return await OperateNumberFromServerAsync(new string[3] { "ZREVRANK", key, member });
	}

	public async Task<OperateResult<string>> ZSetScoreAsync(string key, string member)
	{
		return await OperateStringFromServerAsync(new string[3] { "ZSCORE", key, member });
	}

	public OperateResult<T> Read<T>() where T : class, new()
	{
		return HslReflectionHelper.Read<T>(this);
	}

	public OperateResult Write<T>(T data) where T : class, new()
	{
		return HslReflectionHelper.Write(data, this);
	}

	public async Task<OperateResult<T>> ReadAsync<T>() where T : class, new()
	{
		return await HslReflectionHelper.ReadAsync<T>(this);
	}

	public async Task<OperateResult> WriteAsync<T>(T data) where T : class, new()
	{
		return await HslReflectionHelper.WriteAsync(data, this);
	}

	public OperateResult Save()
	{
		return OperateStatusFromServer(new string[1] { "SAVE" });
	}

	public OperateResult SaveAsync()
	{
		return OperateStatusFromServer(new string[1] { "BGSAVE" });
	}

	public OperateResult<DateTime> ReadServerTime()
	{
		OperateResult<string[]> operateResult = OperateStringsFromServer(new string[1] { "TIME" });
		if (!operateResult.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(operateResult);
		}
		long num = long.Parse(operateResult.Content[0]);
		DateTime value = new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds(num);
		return OperateResult.CreateSuccessResult(value);
	}

	public OperateResult Ping()
	{
		return OperateStatusFromServer(new string[1] { "PING" });
	}

	public OperateResult<long> DBSize()
	{
		return OperateLongNumberFromServer(new string[1] { "DBSIZE" });
	}

	public OperateResult FlushDB()
	{
		return OperateStatusFromServer(new string[1] { "FLUSHDB" });
	}

	public OperateResult ChangePassword(string password)
	{
		return OperateStatusFromServer(new string[4] { "CONFIG", "SET", "requirepass", password });
	}

	public async Task<OperateResult<DateTime>> ReadServerTimeAsync()
	{
		OperateResult<string[]> times = await OperateStringsFromServerAsync(new string[1] { "TIME" });
		if (!times.IsSuccess)
		{
			return OperateResult.CreateFailedResult<DateTime>(times);
		}
		long timeTick = long.Parse(times.Content[0]);
		DateTime dateTime = new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds(timeTick);
		return OperateResult.CreateSuccessResult(dateTime);
	}

	public async Task<OperateResult> PingAsync()
	{
		return await OperateStringsFromServerAsync(new string[1] { "PING" });
	}

	public async Task<OperateResult<long>> DBSizeAsync()
	{
		return await OperateLongNumberFromServerAsync(new string[1] { "DBSIZE" });
	}

	public async Task<OperateResult> FlushDBAsync()
	{
		return await OperateStatusFromServerAsync(new string[1] { "FLUSHDB" });
	}

	public async Task<OperateResult> ChangePasswordAsync(string password)
	{
		return await OperateStatusFromServerAsync(new string[4] { "CONFIG", "SET", "requirepass", password });
	}

	public OperateResult<int> Publish(string channel, string message)
	{
		return OperateNumberFromServer(new string[3] { "PUBLISH", channel, message });
	}

	public async Task<OperateResult<int>> PublishAsync(string channel, string message)
	{
		return await OperateNumberFromServerAsync(new string[3] { "PUBLISH", channel, message });
	}

	public OperateResult SelectDB(int db)
	{
		OperateResult operateResult = OperateStatusFromServer(new string[2]
		{
			"SELECT",
			db.ToString()
		});
		if (operateResult.IsSuccess)
		{
			dbBlock = db;
		}
		return operateResult;
	}

	public async Task<OperateResult> SelectDBAsync(int db)
	{
		OperateResult select = await OperateStatusFromServerAsync(new string[2]
		{
			"SELECT",
			db.ToString()
		});
		if (select.IsSuccess)
		{
			dbBlock = db;
		}
		return select;
	}

	private RedisSubscribe RedisSubscribeInitialize()
	{
		RedisSubscribe redisSubscribe = new RedisSubscribe(IpAddress, Port);
		redisSubscribe.Password = password;
		redisSubscribe.OnRedisMessageReceived += delegate(string topic, string message)
		{
			this.OnRedisMessageReceived?.Invoke(topic, message);
		};
		return redisSubscribe;
	}

	public OperateResult SubscribeMessage(string topic)
	{
		return SubscribeMessage(new string[1] { topic });
	}

	public OperateResult SubscribeMessage(string[] topics)
	{
		return redisSubscribe.Value.SubscribeMessage(topics);
	}

	public OperateResult UnSubscribeMessage(string topic)
	{
		return UnSubscribeMessage(new string[1] { topic });
	}

	public OperateResult UnSubscribeMessage(string[] topics)
	{
		return redisSubscribe.Value.UnSubscribeMessage(topics);
	}

	public override string ToString()
	{
		return $"RedisClient[{IpAddress}:{Port}]";
	}
}
