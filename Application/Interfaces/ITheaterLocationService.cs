using Application.Dtos;

namespace Application.Interfaces;

public interface ITheaterLocationService
{
    void Add(TheaterLocationDto theaterLocationDto);
    void Update(TheaterLocationDto theaterLocationDto);
    void Delete(TheaterLocationDto theaterLocationDto);
    TheaterLocationDto GetById(Guid id);
    List<TheaterLocationDto> GetFilter();
}
