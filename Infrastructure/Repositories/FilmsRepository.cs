using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FilmsRepository : BaseRepository<Films>, IFilmsRepository
{
    public FilmsRepository(DbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}
