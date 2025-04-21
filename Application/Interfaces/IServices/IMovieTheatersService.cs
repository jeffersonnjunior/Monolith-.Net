using Application.Dtos;
using Infrastructure.FiltersModel;

namespace Application.Interfaces.IServices;

public interface IMovieTheatersService
{
    MovieTheatersReadDto GetById(FilterMovieTheatersById filterMovieTheatersById);
    FilterReturn<MovieTheatersReadDto> GetFilter(FilterMovieTheatersTable filter);
    MovieTheatersUpdateDto Add(MovieTheatersCreateDto movieTheatersCreateDto);
    void Update(MovieTheatersUpdateDto movieTheatersUpdateDto);
    void Delete(Guid id);
}
