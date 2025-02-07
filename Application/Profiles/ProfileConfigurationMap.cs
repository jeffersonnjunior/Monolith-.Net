using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class ProfileConfigurationMap : Profile
{
    public ProfileConfigurationMap()
    {
        CreateMap<CustomerDetails, CustomerDetailsDto>();
        CreateMap<Films, FilmsDto>();
        CreateMap<MovieTheaters, MovieTheatersDto>();
        CreateMap<Screens, ScreensDto>();
        CreateMap<Seats, SeatsDto>();
        CreateMap<Sessions, SessionsDto>();
        CreateMap<TheaterLocation, TheaterLocationDto>();
        CreateMap<Tickets, TicketsDto>();
    }
}
