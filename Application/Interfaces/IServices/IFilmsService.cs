using Application.Dtos;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Interfaces.IServices;

public interface IFilmsService
{
    FilmsReadDto GetById(FilterFilmsById filterFilmsById);
    FilterReturn<FilmsReadDto> GetFilter(FilterFilmsTable filter);
    FilmsUpdateDto Add(FilmsCreateDto filmsCreateDto);
    void Update(FilmsUpdateDto filmsUpdateDto);
    void Delete (Guid id);
}