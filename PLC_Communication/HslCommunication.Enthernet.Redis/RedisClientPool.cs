using System;
using System.Threading.Tasks;
using HslCommunication.Algorithms.ConnectPool;

namespace HslCommunication.Enthernet.Redis;

public class RedisClientPool
{
	private ConnectPool<IRedisConnector> redisConnectPool;

	public ConnectPool<IRedisConnector> GetRedisConnectPool => redisConnectPool;

	public int MaxConnector
	{
		get
		{
			return redisConnectPool.MaxConnector;
		}
		set
		{
			redisConnectPool.MaxConnector = value;
		}
	}

	public RedisClientPool(string ipAddress, int port, string password)
	{
		if (Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			redisConnectPool = new ConnectPool<IRedisConnector>(() => new IRedisConnector
			{
				Redis = new RedisClient(ipAddress, port, password)
			});
			redisConnectPool.MaxConnector = int.MaxValue;
			return;
		}
		throw new Exception(StringResources.Language.InsufficientPrivileges);
	}

	public RedisClientPool(string ipAddress, int port, string password, Action<RedisClient> initialize)
	{
		if (Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			redisConnectPool = new ConnectPool<IRedisConnector>(delegate
			{
				RedisClient redisClient = new RedisClient(ipAddress, port, password);
				initialize(redisClient);
				return new IRedisConnector
				{
					Redis = redisClient
				};
			});
			redisConnectPool.MaxConnector = int.MaxValue;
			return;
		}
		throw new Exception(StringResources.Language.InsufficientPrivileges);
	}

	private OperateResult<T> ConnectPoolExecute<T>(Func<RedisClient, OperateResult<T>> exec)
	{
		if (Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			IRedisConnector availableConnector = redisConnectPool.GetAvailableConnector();
			OperateResult<T> result = exec(availableConnector.Redis);
			redisConnectPool.ReturnConnector(availableConnector);
			return result;
		}
		throw new Exception(StringResources.Language.InsufficientPrivileges);
	}

	private OperateResult ConnectPoolExecute(Func<RedisClient, OperateResult> exec)
	{
		if (Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			IRedisConnector availableConnector = redisConnectPool.GetAvailableConnector();
			OperateResult result = exec(availableConnector.Redis);
			redisConnectPool.ReturnConnector(availableConnector);
			return result;
		}
		throw new Exception(StringResources.Language.InsufficientPrivileges);
	}

	private async Task<OperateResult<T>> ConnectPoolExecuteAsync<T>(Func<RedisClient, Task<OperateResult<T>>> execAsync)
	{
		if (Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			IRedisConnector client = redisConnectPool.GetAvailableConnector();
			OperateResult<T> result = await execAsync(client.Redis);
			redisConnectPool.ReturnConnector(client);
			return result;
		}
		throw new Exception(StringResources.Language.InsufficientPrivileges);
	}

	private async Task<OperateResult> ConnectPoolExecuteAsync(Func<RedisClient, Task<OperateResult>> execAsync)
	{
		if (Authorization.asdniasnfaksndiqwhawfskhfaiw())
		{
			IRedisConnector client = redisConnectPool.GetAvailableConnector();
			OperateResult result = await execAsync(client.Redis);
			redisConnectPool.ReturnConnector(client);
			return result;
		}
		throw new Exception(StringResources.Language.InsufficientPrivileges);
	}

	public OperateResult<int> DeleteKey(string[] keys)
	{
		return ConnectPoolExecute((RedisClient m) => m.DeleteKey(keys));
	}

	public OperateResult<int> DeleteKey(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.DeleteKey(key));
	}

	public OperateResult<int> ExistsKey(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.ExistsKey(key));
	}

	public OperateResult<int> ExpireKey(string key, int seconds)
	{
		return ConnectPoolExecute((RedisClient m) => m.ExpireKey(key, seconds));
	}

	public OperateResult<string[]> ReadAllKeys(string pattern)
	{
		return ConnectPoolExecute((RedisClient m) => m.ReadAllKeys(pattern));
	}

	public OperateResult MoveKey(string key, int db)
	{
		return ConnectPoolExecute((RedisClient m) => m.MoveKey(key, db));
	}

	public OperateResult<int> PersistKey(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.PersistKey(key));
	}

	public OperateResult<string> ReadRandomKey()
	{
		return ConnectPoolExecute((RedisClient m) => m.ReadRandomKey());
	}

	public OperateResult RenameKey(string key1, string key2)
	{
		return ConnectPoolExecute((RedisClient m) => m.RenameKey(key1, key2));
	}

	public OperateResult<string> ReadKeyType(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.ReadKeyType(key));
	}

	public OperateResult<int> ReadKeyTTL(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.ReadKeyTTL(key));
	}

	public async Task<OperateResult<int>> DeleteKeyAsync(string[] keys)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.DeleteKeyAsync(keys));
	}

	public async Task<OperateResult<int>> DeleteKeyAsync(string key)
	{
		return await DeleteKeyAsync(new string[1] { key });
	}

	public async Task<OperateResult<int>> ExistsKeyAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ExistsKeyAsync(key));
	}

	public async Task<OperateResult<int>> ExpireKeyAsync(string key, int seconds)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ExpireKeyAsync(key, seconds));
	}

	public async Task<OperateResult<string[]>> ReadAllKeysAsync(string pattern)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadAllKeysAsync(pattern));
	}

	public async Task<OperateResult> MoveKeyAsync(string key, int db)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.MoveKeyAsync(key, db));
	}

	public async Task<OperateResult<int>> PersistKeyAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.PersistKeyAsync(key));
	}

	public async Task<OperateResult<string>> ReadRandomKeyAsync()
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadRandomKeyAsync());
	}

	public async Task<OperateResult> RenameKeyAsync(string key1, string key2)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.RenameKeyAsync(key1, key2));
	}

	public async Task<OperateResult<string>> ReadKeyTypeAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadKeyTypeAsync(key));
	}

	public async Task<OperateResult<int>> ReadKeyTTLAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadKeyTTLAsync(key));
	}

	public OperateResult<int> AppendKey(string key, string value)
	{
		return ConnectPoolExecute((RedisClient m) => m.AppendKey(key, value));
	}

	public OperateResult<long> DecrementKey(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.DecrementKey(key));
	}

	public OperateResult<long> DecrementKey(string key, long value)
	{
		return ConnectPoolExecute((RedisClient m) => m.DecrementKey(key, value));
	}

	public OperateResult<string> ReadKey(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.ReadKey(key));
	}

	public OperateResult<string> ReadKeyRange(string key, int start, int end)
	{
		return ConnectPoolExecute((RedisClient m) => m.ReadKeyRange(key, start, end));
	}

	public OperateResult<string> ReadAndWriteKey(string key, string value)
	{
		return ConnectPoolExecute((RedisClient m) => m.ReadAndWriteKey(key, value));
	}

	public OperateResult<long> IncrementKey(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.IncrementKey(key));
	}

	public OperateResult<long> IncrementKey(string key, long value)
	{
		return ConnectPoolExecute((RedisClient m) => m.IncrementKey(key, value));
	}

	public OperateResult<string> IncrementKey(string key, float value)
	{
		return ConnectPoolExecute((RedisClient m) => m.IncrementKey(key, value));
	}

	public OperateResult<string[]> ReadKey(string[] keys)
	{
		return ConnectPoolExecute((RedisClient m) => m.ReadKey(keys));
	}

	public OperateResult WriteKey(string[] keys, string[] values)
	{
		return ConnectPoolExecute((RedisClient m) => m.WriteKey(keys, values));
	}

	public OperateResult WriteKey(string key, string value)
	{
		return ConnectPoolExecute((RedisClient m) => m.WriteKey(key, value));
	}

	public OperateResult WriteAndPublishKey(string key, string value)
	{
		return ConnectPoolExecute((RedisClient m) => m.WriteAndPublishKey(key, value));
	}

	public OperateResult WriteExpireKey(string key, string value, long seconds)
	{
		return ConnectPoolExecute((RedisClient m) => m.WriteExpireKey(key, value, seconds));
	}

	public OperateResult<int> WriteKeyIfNotExists(string key, string value)
	{
		return ConnectPoolExecute((RedisClient m) => m.WriteKeyIfNotExists(key, value));
	}

	public OperateResult<int> WriteKeyRange(string key, string value, int offset)
	{
		return ConnectPoolExecute((RedisClient m) => m.WriteKeyRange(key, value, offset));
	}

	public OperateResult<int> ReadKeyLength(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.ReadKeyLength(key));
	}

	public async Task<OperateResult<int>> AppendKeyAsync(string key, string value)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.AppendKeyAsync(key, value));
	}

	public async Task<OperateResult<long>> DecrementKeyAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.DecrementKeyAsync(key));
	}

	public async Task<OperateResult<long>> DecrementKeyAsync(string key, long value)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.DecrementKeyAsync(key, value));
	}

	public async Task<OperateResult<string>> ReadKeyAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadKeyAsync(key));
	}

	public async Task<OperateResult<string>> ReadKeyRangeAsync(string key, int start, int end)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadKeyRangeAsync(key, start, end));
	}

	public async Task<OperateResult<string>> ReadAndWriteKeyAsync(string key, string value)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadAndWriteKeyAsync(key, value));
	}

	public async Task<OperateResult<long>> IncrementKeyAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.IncrementKeyAsync(key));
	}

	public async Task<OperateResult<long>> IncrementKeyAsync(string key, long value)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.IncrementKeyAsync(key, value));
	}

	public async Task<OperateResult<string>> IncrementKeyAsync(string key, float value)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.IncrementKeyAsync(key, value));
	}

	public async Task<OperateResult<string[]>> ReadKeyAsync(string[] keys)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadKeyAsync(keys));
	}

	public async Task<OperateResult> WriteKeyAsync(string[] keys, string[] values)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.WriteKeyAsync(keys, values));
	}

	public async Task<OperateResult> WriteKeyAsync(string key, string value)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.WriteKeyAsync(key, value));
	}

	public async Task<OperateResult> WriteAndPublishKeyAsync(string key, string value)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.WriteAndPublishKeyAsync(key, value));
	}

	public async Task<OperateResult> WriteExpireKeyAsync(string key, string value, long seconds)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.WriteExpireKeyAsync(key, value, seconds));
	}

	public async Task<OperateResult<int>> WriteKeyIfNotExistsAsync(string key, string value)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.WriteKeyIfNotExistsAsync(key, value));
	}

	public async Task<OperateResult<int>> WriteKeyRangeAsync(string key, string value, int offset)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.WriteKeyRangeAsync(key, value, offset));
	}

	public async Task<OperateResult<int>> ReadKeyLengthAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadKeyLengthAsync(key));
	}

	public OperateResult<int> ListInsertBefore(string key, string value, string pivot)
	{
		return ConnectPoolExecute((RedisClient m) => m.ListInsertBefore(key, value, pivot));
	}

	public OperateResult<int> ListInsertAfter(string key, string value, string pivot)
	{
		return ConnectPoolExecute((RedisClient m) => m.ListInsertAfter(key, value, pivot));
	}

	public OperateResult<int> GetListLength(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.GetListLength(key));
	}

	public OperateResult<string> ReadListByIndex(string key, long index)
	{
		return ConnectPoolExecute((RedisClient m) => m.ReadListByIndex(key, index));
	}

	public OperateResult<string> ListLeftPop(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.ListLeftPop(key));
	}

	public OperateResult<int> ListLeftPush(string key, string value)
	{
		return ConnectPoolExecute((RedisClient m) => m.ListLeftPush(key, value));
	}

	public OperateResult<int> ListLeftPush(string key, string[] values)
	{
		return ConnectPoolExecute((RedisClient m) => m.ListLeftPush(key, values));
	}

	public OperateResult<int> ListLeftPushX(string key, string value)
	{
		return ConnectPoolExecute((RedisClient m) => m.ListLeftPushX(key, value));
	}

	public OperateResult<string[]> ListRange(string key, long start, long stop)
	{
		return ConnectPoolExecute((RedisClient m) => m.ListRange(key, start, stop));
	}

	public OperateResult<int> ListRemoveElementMatch(string key, long count, string value)
	{
		return ConnectPoolExecute((RedisClient m) => m.ListRemoveElementMatch(key, count, value));
	}

	public OperateResult ListSet(string key, long index, string value)
	{
		return ConnectPoolExecute((RedisClient m) => m.ListSet(key, index, value));
	}

	public OperateResult ListTrim(string key, long start, long end)
	{
		return ConnectPoolExecute((RedisClient m) => m.ListTrim(key, start, end));
	}

	public OperateResult<string> ListRightPop(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.ListRightPop(key));
	}

	public OperateResult<string> ListRightPopLeftPush(string key1, string key2)
	{
		return ConnectPoolExecute((RedisClient m) => m.ListRightPopLeftPush(key1, key2));
	}

	public OperateResult<int> ListRightPush(string key, string value)
	{
		return ConnectPoolExecute((RedisClient m) => m.ListRightPush(key, value));
	}

	public OperateResult<int> ListRightPush(string key, string[] values)
	{
		return ConnectPoolExecute((RedisClient m) => m.ListRightPush(key, values));
	}

	public OperateResult<int> ListRightPushX(string key, string value)
	{
		return ConnectPoolExecute((RedisClient m) => m.ListRightPushX(key, value));
	}

	public async Task<OperateResult<int>> ListInsertBeforeAsync(string key, string value, string pivot)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ListInsertBeforeAsync(key, value, pivot));
	}

	public async Task<OperateResult<int>> ListInsertAfterAsync(string key, string value, string pivot)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ListInsertAfterAsync(key, value, pivot));
	}

	public async Task<OperateResult<int>> GetListLengthAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.GetListLengthAsync(key));
	}

	public async Task<OperateResult<string>> ReadListByIndexAsync(string key, long index)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadListByIndexAsync(key, index));
	}

	public async Task<OperateResult<string>> ListLeftPopAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ListLeftPopAsync(key));
	}

	public async Task<OperateResult<int>> ListLeftPushAsync(string key, string value)
	{
		return await ListLeftPushAsync(key, new string[1] { value });
	}

	public async Task<OperateResult<int>> ListLeftPushAsync(string key, string[] values)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ListLeftPushAsync(key, values));
	}

	public async Task<OperateResult<int>> ListLeftPushXAsync(string key, string value)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ListLeftPushXAsync(key, value));
	}

	public async Task<OperateResult<string[]>> ListRangeAsync(string key, long start, long stop)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ListRangeAsync(key, start, stop));
	}

	public async Task<OperateResult<int>> ListRemoveElementMatchAsync(string key, long count, string value)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ListRemoveElementMatchAsync(key, count, value));
	}

	public async Task<OperateResult> ListSetAsync(string key, long index, string value)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ListSetAsync(key, index, value));
	}

	public async Task<OperateResult> ListTrimAsync(string key, long start, long end)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ListTrimAsync(key, start, end));
	}

	public async Task<OperateResult<string>> ListRightPopAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ListRightPopAsync(key));
	}

	public async Task<OperateResult<string>> ListRightPopLeftPushAsync(string key1, string key2)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ListRightPopLeftPushAsync(key1, key2));
	}

	public async Task<OperateResult<int>> ListRightPushAsync(string key, string value)
	{
		return await ListRightPushAsync(key, new string[1] { value });
	}

	public async Task<OperateResult<int>> ListRightPushAsync(string key, string[] values)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ListRightPushAsync(key, values));
	}

	public async Task<OperateResult<int>> ListRightPushXAsync(string key, string value)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ListRightPushXAsync(key, value));
	}

	public OperateResult<int> DeleteHashKey(string key, string field)
	{
		return ConnectPoolExecute((RedisClient m) => m.DeleteHashKey(key, field));
	}

	public OperateResult<int> DeleteHashKey(string key, string[] fields)
	{
		return ConnectPoolExecute((RedisClient m) => m.DeleteHashKey(key, fields));
	}

	public OperateResult<int> ExistsHashKey(string key, string field)
	{
		return ConnectPoolExecute((RedisClient m) => m.ExistsHashKey(key, field));
	}

	public OperateResult<string> ReadHashKey(string key, string field)
	{
		return ConnectPoolExecute((RedisClient m) => m.ReadHashKey(key, field));
	}

	public OperateResult<string[]> ReadHashKeyAll(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.ReadHashKeyAll(key));
	}

	public OperateResult<long> IncrementHashKey(string key, string field, long value)
	{
		return ConnectPoolExecute((RedisClient m) => m.IncrementHashKey(key, field, value));
	}

	public OperateResult<string> IncrementHashKey(string key, string field, float value)
	{
		return ConnectPoolExecute((RedisClient m) => m.IncrementHashKey(key, field, value));
	}

	public OperateResult<string[]> ReadHashKeys(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.ReadHashKeys(key));
	}

	public OperateResult<int> ReadHashKeyLength(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.ReadHashKeyLength(key));
	}

	public OperateResult<string[]> ReadHashKey(string key, string[] fields)
	{
		return ConnectPoolExecute((RedisClient m) => m.ReadHashKey(key, fields));
	}

	public OperateResult<int> WriteHashKey(string key, string field, string value)
	{
		return ConnectPoolExecute((RedisClient m) => m.WriteHashKey(key, field, value));
	}

	public OperateResult WriteHashKey(string key, string[] fields, string[] values)
	{
		return ConnectPoolExecute((RedisClient m) => m.WriteHashKey(key, fields, values));
	}

	public OperateResult<int> WriteHashKeyNx(string key, string field, string value)
	{
		return ConnectPoolExecute((RedisClient m) => m.WriteHashKeyNx(key, field, value));
	}

	public OperateResult<string[]> ReadHashValues(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.ReadHashValues(key));
	}

	public async Task<OperateResult<int>> DeleteHashKeyAsync(string key, string field)
	{
		return await DeleteHashKeyAsync(key, new string[1] { field });
	}

	public async Task<OperateResult<int>> DeleteHashKeyAsync(string key, string[] fields)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.DeleteHashKeyAsync(key, fields));
	}

	public async Task<OperateResult<int>> ExistsHashKeyAsync(string key, string field)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ExistsHashKeyAsync(key, field));
	}

	public async Task<OperateResult<string>> ReadHashKeyAsync(string key, string field)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadHashKeyAsync(key, field));
	}

	public async Task<OperateResult<string[]>> ReadHashKeyAllAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadHashKeyAllAsync(key));
	}

	public async Task<OperateResult<long>> IncrementHashKeyAsync(string key, string field, long value)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.IncrementHashKeyAsync(key, field, value));
	}

	public async Task<OperateResult<string>> IncrementHashKeyAsync(string key, string field, float value)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.IncrementHashKeyAsync(key, field, value));
	}

	public async Task<OperateResult<string[]>> ReadHashKeysAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadHashKeysAsync(key));
	}

	public async Task<OperateResult<int>> ReadHashKeyLengthAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadHashKeyLengthAsync(key));
	}

	public async Task<OperateResult<string[]>> ReadHashKeyAsync(string key, string[] fields)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadHashKeyAsync(key, fields));
	}

	public async Task<OperateResult<int>> WriteHashKeyAsync(string key, string field, string value)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.WriteHashKeyAsync(key, field, value));
	}

	public async Task<OperateResult> WriteHashKeyAsync(string key, string[] fields, string[] values)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.WriteHashKeyAsync(key, fields, values));
	}

	public async Task<OperateResult<int>> WriteHashKeyNxAsync(string key, string field, string value)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.WriteHashKeyNxAsync(key, field, value));
	}

	public async Task<OperateResult<string[]>> ReadHashValuesAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadHashValuesAsync(key));
	}

	public OperateResult<int> SetAdd(string key, string member)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetAdd(key, member));
	}

	public OperateResult<int> SetAdd(string key, string[] members)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetAdd(key, members));
	}

	public OperateResult<int> SetCard(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetCard(key));
	}

	public OperateResult<string[]> SetDiff(string key, string diffKey)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetDiff(key, diffKey));
	}

	public OperateResult<string[]> SetDiff(string key, string[] diffKeys)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetDiff(key, diffKeys));
	}

	public OperateResult<int> SetDiffStore(string destination, string key, string diffKey)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetDiffStore(destination, key, diffKey));
	}

	public OperateResult<int> SetDiffStore(string destination, string key, string[] diffKeys)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetDiffStore(destination, key, diffKeys));
	}

	public OperateResult<string[]> SetInter(string key, string interKey)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetInter(key, interKey));
	}

	public OperateResult<string[]> SetInter(string key, string[] interKeys)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetInter(key, interKeys));
	}

	public OperateResult<int> SetInterStore(string destination, string key, string interKey)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetInterStore(destination, key, interKey));
	}

	public OperateResult<int> SetInterStore(string destination, string key, string[] interKeys)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetInterStore(destination, key, interKeys));
	}

	public OperateResult<int> SetIsMember(string key, string member)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetIsMember(key, member));
	}

	public OperateResult<string[]> SetMembers(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetMembers(key));
	}

	public OperateResult<int> SetMove(string source, string destination, string member)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetMove(source, destination, member));
	}

	public OperateResult<string> SetPop(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetPop(key));
	}

	public OperateResult<string> SetRandomMember(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetRandomMember(key));
	}

	public OperateResult<string[]> SetRandomMember(string key, int count)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetRandomMember(key, count));
	}

	public OperateResult<int> SetRemove(string key, string member)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetRemove(key, member));
	}

	public OperateResult<int> SetRemove(string key, string[] members)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetRemove(key, members));
	}

	public OperateResult<string[]> SetUnion(string key, string unionKey)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetUnion(key, unionKey));
	}

	public OperateResult<string[]> SetUnion(string key, string[] unionKeys)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetUnion(key, unionKeys));
	}

	public OperateResult<int> SetUnionStore(string destination, string key, string unionKey)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetUnionStore(destination, key, unionKey));
	}

	public OperateResult<int> SetUnionStore(string destination, string key, string[] unionKeys)
	{
		return ConnectPoolExecute((RedisClient m) => m.SetUnionStore(destination, key, unionKeys));
	}

	public async Task<OperateResult<int>> SetAddAsync(string key, string member)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetAddAsync(key, member));
	}

	public async Task<OperateResult<int>> SetAddAsync(string key, string[] members)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetAddAsync(key, members));
	}

	public async Task<OperateResult<int>> SetCardAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetCardAsync(key));
	}

	public async Task<OperateResult<string[]>> SetDiffAsync(string key, string diffKey)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetDiffAsync(key, diffKey));
	}

	public async Task<OperateResult<string[]>> SetDiffAsync(string key, string[] diffKeys)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetDiffAsync(key, diffKeys));
	}

	public async Task<OperateResult<int>> SetDiffStoreAsync(string destination, string key, string diffKey)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetDiffStoreAsync(destination, key, diffKey));
	}

	public async Task<OperateResult<int>> SetDiffStoreAsync(string destination, string key, string[] diffKeys)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetDiffStoreAsync(destination, key, diffKeys));
	}

	public async Task<OperateResult<string[]>> SetInterAsync(string key, string interKey)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetInterAsync(key, interKey));
	}

	public async Task<OperateResult<string[]>> SetInterAsync(string key, string[] interKeys)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetInterAsync(key, interKeys));
	}

	public async Task<OperateResult<int>> SetInterStoreAsync(string destination, string key, string interKey)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetInterStoreAsync(destination, key, interKey));
	}

	public async Task<OperateResult<int>> SetInterStoreAsync(string destination, string key, string[] interKeys)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetInterStoreAsync(destination, key, interKeys));
	}

	public async Task<OperateResult<int>> SetIsMemberAsync(string key, string member)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetIsMemberAsync(key, member));
	}

	public async Task<OperateResult<string[]>> SetMembersAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetMembersAsync(key));
	}

	public async Task<OperateResult<int>> SetMoveAsync(string source, string destination, string member)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetMoveAsync(source, destination, member));
	}

	public async Task<OperateResult<string>> SetPopAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetPopAsync(key));
	}

	public async Task<OperateResult<string>> SetRandomMemberAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetRandomMemberAsync(key));
	}

	public async Task<OperateResult<string[]>> SetRandomMemberAsync(string key, int count)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetRandomMemberAsync(key, count));
	}

	public async Task<OperateResult<int>> SetRemoveAsync(string key, string member)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetRemoveAsync(key, member));
	}

	public async Task<OperateResult<int>> SetRemoveAsync(string key, string[] members)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetRemoveAsync(key, members));
	}

	public async Task<OperateResult<string[]>> SetUnionAsync(string key, string unionKey)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetUnionAsync(key, unionKey));
	}

	public async Task<OperateResult<string[]>> SetUnionAsync(string key, string[] unionKeys)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetUnionAsync(key, unionKeys));
	}

	public async Task<OperateResult<int>> SetUnionStoreAsync(string destination, string key, string unionKey)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetUnionStoreAsync(destination, key, unionKey));
	}

	public async Task<OperateResult<int>> SetUnionStoreAsync(string destination, string key, string[] unionKeys)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SetUnionStoreAsync(destination, key, unionKeys));
	}

	public OperateResult<int> ZSetAdd(string key, string member, double score)
	{
		return ConnectPoolExecute((RedisClient m) => m.ZSetAdd(key, member, score));
	}

	public OperateResult<int> ZSetAdd(string key, string[] members, double[] scores)
	{
		return ConnectPoolExecute((RedisClient m) => m.ZSetAdd(key, members, scores));
	}

	public OperateResult<int> ZSetCard(string key)
	{
		return ConnectPoolExecute((RedisClient m) => m.ZSetCard(key));
	}

	public OperateResult<int> ZSetCount(string key, double min, double max)
	{
		return ConnectPoolExecute((RedisClient m) => m.ZSetCount(key, min, max));
	}

	public OperateResult<string> ZSetIncreaseBy(string key, string member, double increment)
	{
		return ConnectPoolExecute((RedisClient m) => m.ZSetIncreaseBy(key, member, increment));
	}

	public OperateResult<string[]> ZSetRange(string key, int start, int stop, bool withScore = false)
	{
		return ConnectPoolExecute((RedisClient m) => m.ZSetRange(key, start, stop, withScore));
	}

	public OperateResult<string[]> ZSetRangeByScore(string key, string min, string max, bool withScore = false)
	{
		return ConnectPoolExecute((RedisClient m) => m.ZSetRangeByScore(key, min, max, withScore));
	}

	public OperateResult<int> ZSetRank(string key, string member)
	{
		return ConnectPoolExecute((RedisClient m) => m.ZSetRank(key, member));
	}

	public OperateResult<int> ZSetRemove(string key, string member)
	{
		return ConnectPoolExecute((RedisClient m) => m.ZSetRemove(key, member));
	}

	public OperateResult<int> ZSetRemove(string key, string[] members)
	{
		return ConnectPoolExecute((RedisClient m) => m.ZSetRemove(key, members));
	}

	public OperateResult<int> ZSetRemoveRangeByRank(string key, int start, int stop)
	{
		return ConnectPoolExecute((RedisClient m) => m.ZSetRemoveRangeByRank(key, start, stop));
	}

	public OperateResult<int> ZSetRemoveRangeByScore(string key, string min, string max)
	{
		return ConnectPoolExecute((RedisClient m) => m.ZSetRemoveRangeByScore(key, min, max));
	}

	public OperateResult<string[]> ZSetReverseRange(string key, int start, int stop, bool withScore = false)
	{
		return ConnectPoolExecute((RedisClient m) => m.ZSetReverseRange(key, start, stop, withScore));
	}

	public OperateResult<string[]> ZSetReverseRangeByScore(string key, string max, string min, bool withScore = false)
	{
		return ConnectPoolExecute((RedisClient m) => m.ZSetReverseRangeByScore(key, max, min, withScore));
	}

	public OperateResult<int> ZSetReverseRank(string key, string member)
	{
		return ConnectPoolExecute((RedisClient m) => m.ZSetReverseRank(key, member));
	}

	public OperateResult<string> ZSetScore(string key, string member)
	{
		return ConnectPoolExecute((RedisClient m) => m.ZSetScore(key, member));
	}

	public async Task<OperateResult<int>> ZSetAddAsync(string key, string member, double score)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ZSetAddAsync(key, member, score));
	}

	public async Task<OperateResult<int>> ZSetAddAsync(string key, string[] members, double[] scores)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ZSetAddAsync(key, members, scores));
	}

	public async Task<OperateResult<int>> ZSetCardAsync(string key)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ZSetCardAsync(key));
	}

	public async Task<OperateResult<int>> ZSetCountAsync(string key, double min, double max)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ZSetCountAsync(key, min, max));
	}

	public async Task<OperateResult<string>> ZSetIncreaseByAsync(string key, string member, double increment)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ZSetIncreaseByAsync(key, member, increment));
	}

	public async Task<OperateResult<string[]>> ZSetRangeAsync(string key, int start, int stop, bool withScore = false)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ZSetRangeAsync(key, start, stop, withScore));
	}

	public async Task<OperateResult<string[]>> ZSetRangeByScoreAsync(string key, string min, string max, bool withScore = false)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ZSetRangeByScoreAsync(key, min, max, withScore));
	}

	public async Task<OperateResult<int>> ZSetRankAsync(string key, string member)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ZSetRankAsync(key, member));
	}

	public async Task<OperateResult<int>> ZSetRemoveAsync(string key, string member)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ZSetRemoveAsync(key, member));
	}

	public async Task<OperateResult<int>> ZSetRemoveAsync(string key, string[] members)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ZSetRemoveAsync(key, members));
	}

	public async Task<OperateResult<int>> ZSetRemoveRangeByRankAsync(string key, int start, int stop)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ZSetRemoveRangeByRankAsync(key, start, stop));
	}

	public async Task<OperateResult<int>> ZSetRemoveRangeByScoreAsync(string key, string min, string max)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ZSetRemoveRangeByScoreAsync(key, min, max));
	}

	public async Task<OperateResult<string[]>> ZSetReverseRangeAsync(string key, int start, int stop, bool withScore = false)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ZSetReverseRangeAsync(key, start, stop, withScore));
	}

	public async Task<OperateResult<string[]>> ZSetReverseRangeByScoreAsync(string key, string max, string min, bool withScore = false)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ZSetReverseRangeByScoreAsync(key, max, min, withScore));
	}

	public async Task<OperateResult<int>> ZSetReverseRankAsync(string key, string member)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ZSetReverseRankAsync(key, member));
	}

	public async Task<OperateResult<string>> ZSetScoreAsync(string key, string member)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ZSetScoreAsync(key, member));
	}

	public OperateResult<T> Read<T>() where T : class, new()
	{
		return ConnectPoolExecute((RedisClient m) => m.Read<T>());
	}

	public OperateResult Write<T>(T data) where T : class, new()
	{
		return ConnectPoolExecute((RedisClient m) => m.Write(data));
	}

	public async Task<OperateResult<T>> ReadAsync<T>() where T : class, new()
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadAsync<T>());
	}

	public async Task<OperateResult> WriteAsync<T>(T data) where T : class, new()
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.WriteAsync(data));
	}

	public OperateResult Save()
	{
		return ConnectPoolExecute((RedisClient m) => m.Save());
	}

	public OperateResult SaveAsync()
	{
		return ConnectPoolExecute((RedisClient m) => m.SaveAsync());
	}

	public OperateResult<DateTime> ReadServerTime()
	{
		return ConnectPoolExecute((RedisClient m) => m.ReadServerTime());
	}

	public OperateResult Ping()
	{
		return ConnectPoolExecute((RedisClient m) => m.Ping());
	}

	public OperateResult<long> DBSize()
	{
		return ConnectPoolExecute((RedisClient m) => m.DBSize());
	}

	public OperateResult FlushDB()
	{
		return ConnectPoolExecute((RedisClient m) => m.FlushDB());
	}

	public OperateResult ChangePassword(string password)
	{
		return ConnectPoolExecute((RedisClient m) => m.ChangePassword(password));
	}

	public async Task<OperateResult<DateTime>> ReadServerTimeAsync()
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ReadServerTimeAsync());
	}

	public async Task<OperateResult> PingAsync()
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.PingAsync());
	}

	public async Task<OperateResult<long>> DBSizeAsync()
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.DBSizeAsync());
	}

	public async Task<OperateResult> FlushDBAsync()
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.FlushDBAsync());
	}

	public async Task<OperateResult> ChangePasswordAsync(string password)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.ChangePasswordAsync(password));
	}

	public OperateResult<int> Publish(string channel, string message)
	{
		return ConnectPoolExecute((RedisClient m) => m.Publish(channel, message));
	}

	public async Task<OperateResult<int>> PublishAsync(string channel, string message)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.PublishAsync(channel, message));
	}

	public OperateResult SelectDB(int db)
	{
		return ConnectPoolExecute((RedisClient m) => m.SelectDB(db));
	}

	public async Task<OperateResult> SelectDBAsync(int db)
	{
		return await ConnectPoolExecuteAsync((RedisClient m) => m.SelectDBAsync(db));
	}

	public override string ToString()
	{
		return $"RedisConnectPool[{redisConnectPool.MaxConnector}]";
	}
}
