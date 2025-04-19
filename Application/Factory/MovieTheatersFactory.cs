using Application.Dtos;
using Application.Interfaces.IFactory;
using Domain.Entities;

namespace Application.Factory;

public class MovieTheatersFactory : IMovieTheatersFactory
{
    public MovieTheaters CreateMovieTheater()
    {
        return new MovieTheaters
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            TheaterLocationId = Guid.Empty,
            TheaterLocation = null,
            Screens = new List<Screens>()
        };
    }

    public MovieTheatersCreateDto CreateMovieTheaterCreateDto()
    {
        return new MovieTheatersCreateDto
        {
            Name = string.Empty,
            TheaterLocationId = Guid.Empty
        };
    }

    public MovieTheatersReadDto CreateMovieTheaterReadDto()
    {
        return new MovieTheatersReadDto
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            TheaterLocationId = Guid.Empty
        };
    }

    public MovieTheatersUpdateDto CreateMovieTheaterUpdateDto()
    {
        return new MovieTheatersUpdateDto
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            TheaterLocationId = Guid.Empty
        };
    }

    public MovieTheaters MapToMovieTheater(MovieTheatersCreateDto dto)
    {
        return new MovieTheaters
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            TheaterLocationId = dto.TheaterLocationId,
            Screens = new List<Screens>()
        };
    }

    public MovieTheatersCreateDto MapToMovieTheaterCreateDto(MovieTheaters entity)
    {
        return new MovieTheatersCreateDto
        {
            Name = entity.Name,
            TheaterLocationId = entity.TheaterLocationId
        };
    }

    public MovieTheatersReadDto MapToMovieTheaterReadDto(MovieTheaters entity)
    {
        return new MovieTheatersReadDto
        {
            Id = entity.Id,
            Name = entity.Name,
            TheaterLocationId = entity.TheaterLocationId
        };
    }

    public MovieTheatersUpdateDto MapToMovieTheaterUpdateDto(MovieTheaters entity)
    {
        return new MovieTheatersUpdateDto
        {
            Id = entity.Id,
            Name = entity.Name,
            TheaterLocationId = entity.TheaterLocationId
        };
    }

    public MovieTheaters MapToMovieTheaterFromUpdateDto(MovieTheatersUpdateDto dto)
    {
        return new MovieTheaters
        {
            Id = dto.Id,
            Name = dto.Name,
            TheaterLocationId = dto.TheaterLocationId,
            Screens = new List<Screens>()
        };
    }
}