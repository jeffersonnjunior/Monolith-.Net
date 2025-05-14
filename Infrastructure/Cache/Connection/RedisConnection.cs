using StackExchange.Redis;

namespace Infrastructure.Cache.Connection;

public class RedisConnection : IDisposable
{
    private readonly IConnectionMultiplexer _connection;

    public RedisConnection(string connectionString)
    {
        _connection = ConnectionMultiplexer.Connect(connectionString);
    }

    public IDatabase GetDatabase(int db = -1)
    {
        return _connection.GetDatabase(db);
    }

    public IServer GetServer()
    {
        return _connection.GetServer(_connection.GetEndPoints().First());
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}