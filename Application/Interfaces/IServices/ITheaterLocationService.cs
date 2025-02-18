using Application.Dtos;
using Domain.Entities;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Interfaces.IServices;

public interface ITheaterLocationService
{
    TheaterLocationDto GetById(Guid id);
    ReturnTable<TheaterLocation> GetFilter(TheaterLocationFilter filter, string[] includes);
    TheaterLocationDto Add(TheaterLocationDto theaterLocationDto);
    void Update(TheaterLocationDto theaterLocationDto);
    void Delete(Guid id);
}
