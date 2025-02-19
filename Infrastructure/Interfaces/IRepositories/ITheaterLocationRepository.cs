using Domain.Entities;
using Infrastructure.Utilities.FiltersModel;

namespace Infrastructure.Interfaces.IRepositories;

public interface ITheaterLocationRepository : IBaseRepository<TheaterLocation>
{
    TheaterLocation GetById(Guid id);
    TheaterLocation GetExisting(string street);
    ReturnTable<TheaterLocation> GetFilter(TheaterLocationFilter filter, params string[] includes);

}
