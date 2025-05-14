using Infrastructure.Cache.Connection;
using Infrastructure.Interfaces.ICache.IServices;
using StackExchange.Redis;

namespace Infrastructure.Cache.Services;

public sealed class CacheService : ICacheService
{
    private readonly IDatabase _database;

    public CacheService(RedisConnection redisFactory)
    {
        _database = redisFactory.GetDatabase();
    }

    public void Set<T>(string key, T value, TimeSpan? expiry = null)
    {
        _database.StringSet(
            key,
            System.Text.Json.JsonSerializer.Serialize(value),
            expiry
        );
    }

    public T? Get<T>(string key)
    {
        var value = _database.StringGet(key);
        return value.HasValue ?
            System.Text.Json.JsonSerializer.Deserialize<T>(value!) :
            default;
    }

    public void Remove(string key) => _database.KeyDelete(key);

    public bool Exists(string key) => _database.KeyExists(key);

    public void FlushDatabase() => _database.Execute("FLUSHDB");

    public void RemoveByPrefix(string prefix)
    {
        var server = _database.Multiplexer.GetServer(_database.Multiplexer.GetEndPoints().First());
        var keys = server.Keys(pattern: $"{prefix}*").ToArray();

        foreach (var key in keys)
        {
            _database.KeyDelete(key);
        }
    }
}