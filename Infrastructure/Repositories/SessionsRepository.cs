using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SessionsRepository : BaseRepository<Sessions>, ISessionsRepository
{
    public SessionsRepository(DbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}
