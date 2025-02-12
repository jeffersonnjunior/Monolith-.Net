using Application.Dtos;

namespace Application.Interfaces.IServices;

public interface ITheaterLocationService
{
    TheaterLocationDto GetById(Guid id);
    List<TheaterLocationDto> GetFilter();
    void Add(TheaterLocationDto theaterLocationDto);
    void Update(TheaterLocationDto theaterLocationDto);
    void Delete(Guid id);
}
