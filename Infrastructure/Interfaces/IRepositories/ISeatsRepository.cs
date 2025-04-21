using Domain.Entities;
using Infrastructure.FiltersModel;

namespace Infrastructure.Interfaces.IRepositories;

public interface ISeatsRepository : IBaseRepository<Seats>
{
    Seats GetByElement(FilterByItem filterByItem);
    FilterReturn<Seats> GetFilter(FilterSeatsTable filter);
    bool ValidateInput(object dto, bool isUpdate, Seats existingSeats = null);
}
