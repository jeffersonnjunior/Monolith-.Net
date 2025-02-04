using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TheaterLocationRepository : BaseRepository<TheaterLocation>, ITheaterLocationRepository
{
    public TheaterLocationRepository(DbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}
