using Application.Dtos;
using Application.Interfaces;
using Infrastructure.Interfaces;

namespace Application.Services;

public class TheaterLocationService : ITheaterLocationService
{
    private readonly ITheaterLocationRepository _theaterLocationRepository;

    public TheaterLocationService(ITheaterLocationRepository theaterLocationRepository)
    {
        _theaterLocationRepository = theaterLocationRepository;
    }

    public TheaterLocationDto GetById(Guid id)
    {
        throw new NotImplementedException();
    }
    public List<TheaterLocationDto> GetFilter()
    {
        throw new NotImplementedException();
    }
    public void Add(TheaterLocationDto theaterLocationDto)
    {
        throw new NotImplementedException();
    }
    public void Update(TheaterLocationDto theaterLocationDto)
    {
        throw new NotImplementedException();
    }
    public void Delete(TheaterLocationDto theaterLocationDto)
    {
        throw new NotImplementedException();
    }
}
