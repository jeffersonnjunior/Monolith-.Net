using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces.IFactory;

public interface ITheaterLocationFactory
{
    TheaterLocation CreateTheaterLocation();
    TheaterLocationCreateDto CreateTheaterLocationCreateDto();
    TheaterLocationReadDto CreateTheaterLocationReadDto();
    TheaterLocationUpdateDto CreateTheaterLocationUpdateDto();
    TheaterLocation MapToTheaterLocation(TheaterLocationCreateDto dto);
    TheaterLocationCreateDto MapToTheaterLocationCreateDto(TheaterLocation entity);
    TheaterLocationReadDto MapToTheaterLocationReadDto(TheaterLocation entity);
    TheaterLocationUpdateDto MapToTheaterLocationUpdateDto(TheaterLocation entity);
    TheaterLocation MapToTheaterLocationFromUpdateDto(TheaterLocationUpdateDto dto);
}