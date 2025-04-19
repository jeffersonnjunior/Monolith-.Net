using Application.Dtos;
using Application.Interfaces.IFactory;
using Domain.Entities;
using Domain.Enums;

namespace Application.Factory;

public class FilmeFactory : IFilmeFactory
{
    public Films CreateFilme()
    {
        return new Films
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Duration = 0,
            AgeRange = 0,
            FilmGenres = FilmGenres.Action, 
            Sessions = new List<Sessions>()
        };
    }

    public FilmsCreateDto CreateFilmeCreateDto()
    {
        return new FilmsCreateDto
        {
            Name = string.Empty,
            Duration = 0,
            AgeRange = 0,
            FilmGenres = FilmGenres.Action 
        };
    }

    public FilmsReadDto CreateFilmeReadDto()
    {
        return new FilmsReadDto
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Duration = 0,
            AgeRange = 0,
            FilmGenres = FilmGenres.Action 
        };
    }

    public FilmsUpdateDto CreateFilmeUpdateDto()
    {
        return new FilmsUpdateDto
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Duration = 0,
            AgeRange = 0,
            FilmGenres = FilmGenres.Action // Valor padrão
        };
    }

    public Films MapToFilme(FilmsCreateDto dto)
    {
        return new Films
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Duration = dto.Duration,
            AgeRange = dto.AgeRange,
            FilmGenres = dto.FilmGenres,
            Sessions = new List<Sessions>()
        };
    }

    public FilmsCreateDto MapToFilmeCreateDto(Films entity)
    {
        return new FilmsCreateDto
        {
            Name = entity.Name,
            Duration = entity.Duration,
            AgeRange = entity.AgeRange,
            FilmGenres = entity.FilmGenres
        };
    }

    public FilmsReadDto MapToFilmeReadDto(Films entity)
    {
        return new FilmsReadDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Duration = entity.Duration,
            AgeRange = entity.AgeRange,
            FilmGenres = entity.FilmGenres
        };
    }

    public FilmsUpdateDto MapToFilmeUpdateDto(Films entity)
    {
        return new FilmsUpdateDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Duration = entity.Duration,
            AgeRange = entity.AgeRange,
            FilmGenres = entity.FilmGenres
        };
    }

    public Films MapToFilmeFromUpdateDto(FilmsUpdateDto dto)
    {
        return new Films
        {
            Id = dto.Id,
            Name = dto.Name,
            Duration = dto.Duration,
            AgeRange = dto.AgeRange,
            FilmGenres = dto.FilmGenres,
            Sessions = new List<Sessions>()
        };
    }
}