using Domain.Entities;
using Infrastructure.FiltersModel;

namespace Infrastructure.Interfaces.IRepositories;

public interface ISessionsRepository : IBaseRepository<Sessions>
{
    Sessions GetByElement(FilterByItem filterByItem);
    FilterReturn<Sessions> GetFilter(FilterSessionsTable filter);
    bool ValidateInput(object dto, bool isUpdate, Sessions existingSessions = null);
}
