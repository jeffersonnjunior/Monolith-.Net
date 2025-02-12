using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;

namespace Infrastructure.Repositories;

public class TicketsRepository : BaseRepository<Tickets>, ITicketsRepository
{
    public TicketsRepository(AppDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}