using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class AddInfrastructureDependencyInjection
{
    public static void DependencyInjectionInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.PersistenceDependencyInjection(configuration);
        services.CacheDependencyInjection(configuration);
        services.NotificationsDependencyInjection();
    }
}