using Application.Profiles;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

public static class AddAutoMapperDependencyInjection
{
    public static void AutoMapperDependencyInjection(this IServiceCollection services)
    {
        services.AddSingleton<IConfigurationProvider>(AutoMapperConfig.RegisterMappings());
        services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));
    }
}