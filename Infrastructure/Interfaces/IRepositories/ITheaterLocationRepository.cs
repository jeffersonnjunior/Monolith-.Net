using Domain.Entities;

namespace Infrastructure.Interfaces.IRepositories;

public interface ITheaterLocationRepository : IBaseRepository<TheaterLocation>
{
    TheaterLocation GetById(Guid id);
}
