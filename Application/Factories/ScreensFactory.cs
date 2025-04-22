using Application.Dtos;
using Application.Interfaces.IFactories;
using Domain.Entities;

namespace Application.Factories;

public class ScreensFactory : IScreensFactory
{
    public Screens CreateScreen()
    {
        return new Screens
        {
            Id = Guid.NewGuid(),
            ScreenNumber = string.Empty,
            SeatingCapacity = 0,
            MovieTheaterId = Guid.Empty,
            MovieTheater = null,
            Seats = new List<Seats>()
        };
    }

    public ScreensCreateDto CreateScreenCreateDto()
    {
        return new ScreensCreateDto
        {
            ScreenNumber = string.Empty,
            SeatingCapacity = 0,
            MovieTheaterId = Guid.Empty
        };
    }

    public ScreensReadDto CreateScreenReadDto()
    {
        return new ScreensReadDto
        {
            Id = Guid.NewGuid(),
            ScreenNumber = string.Empty,
            SeatingCapacity = 0,
            MovieTheaterId = Guid.Empty
        };
    }

    public ScreensUpdateDto CreateScreenUpdateDto()
    {
        return new ScreensUpdateDto
        {
            Id = Guid.NewGuid(),
            ScreenNumber = string.Empty,
            SeatingCapacity = 0,
            MovieTheaterId = Guid.Empty
        };
    }

    public Screens MapToScreen(ScreensCreateDto dto)
    {
        return new Screens
        {
            Id = Guid.NewGuid(),
            ScreenNumber = dto.ScreenNumber,
            SeatingCapacity = dto.SeatingCapacity,
            MovieTheaterId = dto.MovieTheaterId,
            Seats = new List<Seats>()
        };
    }

    public ScreensCreateDto MapToScreenCreateDto(Screens entity)
    {
        return new ScreensCreateDto
        {
            ScreenNumber = entity.ScreenNumber,
            SeatingCapacity = entity.SeatingCapacity,
            MovieTheaterId = entity.MovieTheaterId
        };
    }

    public ScreensReadDto MapToScreenReadDto(Screens entity)
    {
        return new ScreensReadDto
        {
            Id = entity.Id,
            ScreenNumber = entity.ScreenNumber,
            SeatingCapacity = entity.SeatingCapacity,
            MovieTheaterId = entity.MovieTheaterId
        };
    }

    public ScreensUpdateDto MapToScreenUpdateDto(Screens entity)
    {
        return new ScreensUpdateDto
        {
            Id = entity.Id,
            ScreenNumber = entity.ScreenNumber,
            SeatingCapacity = entity.SeatingCapacity,
            MovieTheaterId = entity.MovieTheaterId
        };
    }

    public Screens MapToScreenFromUpdateDto(ScreensUpdateDto dto)
    {
        return new Screens
        {
            Id = dto.Id,
            ScreenNumber = dto.ScreenNumber,
            SeatingCapacity = dto.SeatingCapacity,
            MovieTheaterId = dto.MovieTheaterId,
            Seats = new List<Seats>()
        };
    }
}