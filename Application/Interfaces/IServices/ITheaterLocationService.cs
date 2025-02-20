using Application.Dtos;
using Domain.Entities;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Interfaces.IServices;

public interface ITheaterLocationService
{
    TheaterLocationReadDto GetById(Guid id);
    FilterReturn<TheaterLocation> GetFilter(FilterTheaterLocation filter, string[] includes);
    TheaterLocationReadDto Add(TheaterLocationCreateDto theaterLocationCreateDto);
    void Update(TheaterLocationUpdateDto theaterLocationUpdateDto);
    void Delete(Guid id);
}
