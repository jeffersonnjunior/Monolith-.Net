using Domain.Entities;
using Infrastructure.Utilities.FiltersModel;

namespace Infrastructure.Interfaces.IRepositories;

public interface ITheaterLocationRepository : IBaseRepository<TheaterLocation>
{
    TheaterLocation GetById(FilterByItem filterByItem);
    FilterReturn<TheaterLocation> GetFilter(FilterTheaterLocation filter, params string[] includes);
}
