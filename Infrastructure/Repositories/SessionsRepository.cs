using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;

namespace Infrastructure.Repositories;

public class SessionsRepository : BaseRepository<Sessions>, ISessionsRepository
{
    public SessionsRepository(AppDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}
