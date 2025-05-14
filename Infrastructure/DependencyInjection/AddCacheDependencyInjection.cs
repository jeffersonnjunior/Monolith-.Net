using Infrastructure.Cache.Connection;
using Infrastructure.Cache.Services;
using Infrastructure.Interfaces.ICache.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class AddCacheDependencyInjection
{
    public static IServiceCollection CacheDependencyInjection(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        services.AddSingleton<RedisConnection>(_ =>
            new RedisConnection(configuration.GetConnectionString("Redis")));

        services.AddScoped<ICacheService, CacheService>();

        return services;
    }
}