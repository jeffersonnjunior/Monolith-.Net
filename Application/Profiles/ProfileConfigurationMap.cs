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
        CreateMap<MovieTheaters, MovieTheatersCreateDto>().ReverseMap();
        CreateMap<MovieTheaters, MovieTheatersReadDto>().ReverseMap();
        CreateMap<MovieTheaters, MovieTheatersUpdateDto>().ReverseMap();
        CreateMap<Screens, ScreensCreateDto>().ReverseMap();
        CreateMap<Screens, ScreensReadDto>().ReverseMap();
        CreateMap<Screens, ScreensUpdateDto>().ReverseMap();
        CreateMap<Seats, SeatsCreateDto>().ReverseMap();
        CreateMap<Seats, SeatsReadDto>().ReverseMap();
        CreateMap<Seats, SeatsUpdateDto>().ReverseMap();
        CreateMap<Sessions, SessionsDto>().ReverseMap();
        CreateMap<TheaterLocation, TheaterLocationCreateDto>().ReverseMap();
        CreateMap<TheaterLocation, TheaterLocationReadDto>().ReverseMap();
        CreateMap<TheaterLocation, TheaterLocationUpdateDto>().ReverseMap();
        CreateMap<Tickets, TicketsDto>().ReverseMap();
    }
}
