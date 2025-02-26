using Domain.Entities;
using Infrastructure.Utilities.FiltersModel;

namespace Infrastructure.Interfaces.IRepositories;

public interface ITheaterLocationRepository : IBaseRepository<TheaterLocation>
{
    TheaterLocation GetByElement(FilterByItem filterByItem);
    FilterReturn<TheaterLocation> GetFilter(FilterTheaterLocationTable filter);
}
