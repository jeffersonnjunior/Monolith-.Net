using Domain.Entities;
using Infrastructure.Utilities.FiltersModel;

namespace Infrastructure.Interfaces.IRepositories;

public interface IFilmsRepository : IBaseRepository<Films>
{
    Films GetByElement(FilterByItem filterByItem);
    FilterReturn<Films> GetFilter(FilterFilmsTable filter);
    bool ValidateInput(object dto, bool isUpdate, Films existingFilms = null);
}
