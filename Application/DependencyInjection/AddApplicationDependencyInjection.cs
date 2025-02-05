using Application.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

public static class AddDependencyInjectionApplication
{
    public static void DependencyInjectionApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.ServicesDependencyInjection();
    }
}