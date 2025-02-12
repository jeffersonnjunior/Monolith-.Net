using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class AddRepositoryDependencyInjection
{
    public static void RepositorysDependencyInjection(this IServiceCollection repository)
    {
        repository.AddScoped<ICustomerDetailsRepository, CustomerDetailsRepository>();
        repository.AddScoped<IFilmsRepository, FilmsRepository>();
        repository.AddScoped<IMovieTheatersRepository, MovieTheatersRepository>();
        repository.AddScoped<IScreensRepository, ScreensRepository>();
        repository.AddScoped<ISeatsRepository, SeatsRepository>();
        repository.AddScoped<ISessionsRepository, SessionsRepository>();
        repository.AddScoped<ITheaterLocationRepository, TheaterLocationRepository>();
        repository.AddScoped<ITicketsRepository, TicketsRepository>();
        repository.AddScoped<IUnitOfWork, UnitOfWork>();

    }
}