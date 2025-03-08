using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class ProfileConfigurationMap : Profile
{
    public ProfileConfigurationMap()
    {
        CreateMap<CustomerDetails, CustomerDetailsCreateDto>().ReverseMap();
        CreateMap<CustomerDetails, CustomerDetailsReadDto>().ReverseMap();
        CreateMap<CustomerDetails, CustomerDetailsUpdateDto>().ReverseMap();
        CreateMap<Films, FilmsCreateDto>().ReverseMap();
        CreateMap<Films, FilmsReadDto>().ReverseMap();
        CreateMap<Films, FilmsUpdateDto>().ReverseMap();
        CreateMap<MovieTheaters, MovieTheatersCreateDto>().ReverseMap();
        CreateMap<MovieTheaters, MovieTheatersReadDto>().ReverseMap();
        CreateMap<MovieTheaters, MovieTheatersUpdateDto>().ReverseMap();
        CreateMap<Screens, ScreensCreateDto>().ReverseMap();
        CreateMap<Screens, ScreensReadDto>().ReverseMap();
        CreateMap<Screens, ScreensUpdateDto>().ReverseMap();
        CreateMap<Seats, SeatsCreateDto>().ReverseMap();
        CreateMap<Seats, SeatsReadDto>().ReverseMap();
        CreateMap<Seats, SeatsUpdateDto>().ReverseMap();
        CreateMap<Sessions, SessionsCreateDto>().ReverseMap();
        CreateMap<Sessions, SessionsReadDto>().ReverseMap();
        CreateMap<Sessions, SessionsUpdateDto>().ReverseMap();
        CreateMap<TheaterLocation, TheaterLocationCreateDto>().ReverseMap();
        CreateMap<TheaterLocation, TheaterLocationReadDto>().ReverseMap();
        CreateMap<TheaterLocation, TheaterLocationUpdateDto>().ReverseMap();
        CreateMap<Tickets, TicketsCreateDto>().ReverseMap();
        CreateMap<Tickets, TicketsReadDto>().ReverseMap();
        CreateMap<Tickets, TicketsUpdateDto>().ReverseMap();
    }
}
