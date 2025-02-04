using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TicketsRepository : BaseRepository<Tickets>, ITicketsRepository
{
    public TicketsRepository(DbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}