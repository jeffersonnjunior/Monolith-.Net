using Application.Dtos;
using Domain.Entities;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Interfaces.IServices;

public interface ITheaterLocationService
{
    TheaterLocationReadDto GetById(FilterTheaterLocationById filterTheaterLocationById);
    FilterReturn<TheaterLocationReadDto> GetFilter(FilterTheaterLocationTable filter);
    TheaterLocationReadDto Add(TheaterLocationCreateDto theaterLocationCreateDto);
    void Update(TheaterLocationUpdateDto theaterLocationUpdateDto);
    void Delete(Guid id);
}
