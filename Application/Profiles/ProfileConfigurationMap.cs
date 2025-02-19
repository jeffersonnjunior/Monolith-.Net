using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class ProfileConfigurationMap : Profile
{
    public ProfileConfigurationMap()
    {
        CreateMap<CustomerDetails, CustomerDetailsDto>().ReverseMap();
        CreateMap<Films, FilmsDto>().ReverseMap();
        CreateMap<MovieTheaters, MovieTheatersDto>().ReverseMap();
        CreateMap<Screens, ScreensDto>().ReverseMap();
        CreateMap<Seats, SeatsDto>().ReverseMap();
        CreateMap<Sessions, SessionsDto>().ReverseMap();
        CreateMap<TheaterLocation, TheaterLocationCreateDto>().ReverseMap();
        CreateMap<TheaterLocation, TheaterLocationReadDto>().ReverseMap();
        CreateMap<TheaterLocation, TheaterLocationUpdateDto>().ReverseMap();
        CreateMap<Tickets, TicketsDto>().ReverseMap();
    }
}
