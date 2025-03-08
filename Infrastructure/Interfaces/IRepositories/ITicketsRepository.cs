using Domain.Entities;
using Infrastructure.Utilities.FiltersModel;

namespace Infrastructure.Interfaces.IRepositories;

public interface ITicketsRepository : IBaseRepository<Tickets>
{
    Tickets GetByElement(FilterByItem filterByItem);
    FilterReturn<Tickets> GetFilter(FilterTicketsTable filter);
    bool ValidateInput(object dto, bool isUpdate, Tickets existingTickets = null);
}
