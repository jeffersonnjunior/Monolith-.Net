using Application.Specification;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

public static class AddSpecificationDependencyInjection
{
    public static void SpecificationDependencyInjection(this IServiceCollection service)
    {
        service.AddScoped<TheaterLocationSpecification>();
    }
}