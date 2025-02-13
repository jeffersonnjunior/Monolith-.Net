using Application.Dtos;

namespace Application.Interfaces.IServices;

public interface ITheaterLocationService
{
    TheaterLocationDto GetById(Guid id);
    List<TheaterLocationDto> GetFilter();
    TheaterLocationDto Add(TheaterLocationDto theaterLocationDto);
    void Update(TheaterLocationDto theaterLocationDto);
    void Delete(Guid id);
}
