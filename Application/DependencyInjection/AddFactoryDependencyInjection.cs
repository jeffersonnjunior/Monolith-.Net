using Application.Factories;
using Application.Interfaces.IFactories;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

public static class AddFactoryDependencyInjection
{
    public static void FactoryDependencyInjection(this IServiceCollection service)
    {
        service.AddScoped<ICustomerDetailsFactory, CustomerDetailsFactory>();
        service.AddScoped<IFilmeFactory, FilmeFactory>();
        service.AddScoped<IMovieTheatersFactory, MovieTheatersFactory>();
        service.AddScoped<IScreensFactory, ScreensFactory>();
        service.AddScoped<ISeatsFactory, SeatsFactory>();
        service.AddScoped<ISessionsFactory, SessionsFactory>();
        service.AddScoped<ITheaterLocationFactory, TheaterLocationFactory>();
        service.AddScoped<ITicketsFactory, TicketsFactory>();
    }
}