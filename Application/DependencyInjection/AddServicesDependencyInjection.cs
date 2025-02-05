using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extension;

public static class AddServicesDependencyInjection
{
    public static void ServicesDependencyInjection(this IServiceCollection service)
    {
        service.AddScoped<ICustomerDetailsService, CustomerDetailsService>();
        service.AddScoped<IFilmsService, FilmsService>();   
        service.AddScoped<IMovieTheatersService, MovieTheatersService>();
        service.AddScoped<IScreensService, ScreensService>();
        service.AddScoped<ISeatsService, SeatsService>();
        service.AddScoped<ISessionsService, SessionsService>();
        service.AddScoped<ITheaterLocationService, TheaterLocationService>();
        service.AddScoped<ITicketsService, TicketsService>();
    }
}