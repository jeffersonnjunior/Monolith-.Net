using Domain.Entities;
using Infrastructure.Utilities.FiltersModel;

namespace Infrastructure.Interfaces.IRepositories;

public interface IScreensRepository : IBaseRepository<Screens>
{
    Screens GetByElement(FilterByItem filterByItem);
    FilterReturn<Screens> GetFilter(FilterScreensTable filter);
    bool ValidateInput(object dto, bool isUpdate, Screens existingScreens = null);
    bool IsScreenNumberInUse(object? existingScreens, Guid movieTheatersId, string screenNumber);
}
