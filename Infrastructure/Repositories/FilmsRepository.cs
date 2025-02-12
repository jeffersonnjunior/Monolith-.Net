using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FilmsRepository : BaseRepository<Films>, IFilmsRepository
{
    public FilmsRepository(AppDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}
