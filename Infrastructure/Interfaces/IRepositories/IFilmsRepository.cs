using Domain.Entities;
using Domain.Enums;
using Infrastructure.FiltersModel;

namespace Infrastructure.Interfaces.IRepositories;

public interface IFilmsRepository : IBaseRepository<Films>
{
    Films GetByElement(FilterByItem filterByItem);
    FilterReturn<Films> GetFilter(FilterFilmsTable filter);
    bool ValidateInput(object dto, bool isUpdate, Films existingFilms = null);
    bool ValidadeAgeAndGenre(int ageRange, FilmGenres filmGenres);
}
