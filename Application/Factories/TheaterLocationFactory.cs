using Application.Dtos;
using Application.Interfaces.IFactories;
using Domain.Entities;

namespace Application.Factories;

public class TheaterLocationFactory : ITheaterLocationFactory
{
    public TheaterLocation CreateTheaterLocation()
    {
        return new TheaterLocation
        {
            Id = Guid.NewGuid(),
            Street = string.Empty,
            UnitNumber = string.Empty,
            PostalCode = string.Empty
        };
    }

    public TheaterLocationCreateDto CreateTheaterLocationCreateDto()
    {
        return new TheaterLocationCreateDto
        {
            Street = string.Empty,
            UnitNumber = string.Empty,
            PostalCode = string.Empty
        };
    }

    public TheaterLocationReadDto CreateTheaterLocationReadDto()
    {
        return new TheaterLocationReadDto
        {
            Id = Guid.NewGuid(),
            Street = string.Empty,
            UnitNumber = string.Empty,
            PostalCode = string.Empty
        };
    }

    public TheaterLocationUpdateDto CreateTheaterLocationUpdateDto()
    {
        return new TheaterLocationUpdateDto
        {
            Id = Guid.NewGuid(),
            Street = string.Empty,
            UnitNumber = string.Empty,
            PostalCode = string.Empty
        };
    }

    public TheaterLocation MapToTheaterLocation(TheaterLocationCreateDto dto)
    {
        return new TheaterLocation
        {
            Id = Guid.NewGuid(),
            Street = dto.Street,
            UnitNumber = dto.UnitNumber,
            PostalCode = dto.PostalCode
        };
    }

    public TheaterLocationCreateDto MapToTheaterLocationCreateDto(TheaterLocation entity)
    {
        return new TheaterLocationCreateDto
        {
            Street = entity.Street,
            UnitNumber = entity.UnitNumber,
            PostalCode = entity.PostalCode
        };
    }

    public TheaterLocationReadDto MapToTheaterLocationReadDto(TheaterLocation entity)
    {
        return new TheaterLocationReadDto
        {
            Id = entity.Id,
            Street = entity.Street,
            UnitNumber = entity.UnitNumber,
            PostalCode = entity.PostalCode
        };
    }

    public TheaterLocationUpdateDto MapToTheaterLocationUpdateDto(TheaterLocation entity)
    {
        return new TheaterLocationUpdateDto
        {
            Id = entity.Id,
            Street = entity.Street,
            UnitNumber = entity.UnitNumber,
            PostalCode = entity.PostalCode
        };
    }

    public TheaterLocation MapToTheaterLocationFromUpdateDto(TheaterLocationUpdateDto dto)
    {
        return new TheaterLocation
        {
            Id = dto.Id,
            Street = dto.Street,
            UnitNumber = dto.UnitNumber,
            PostalCode = dto.PostalCode
        };
    }
}