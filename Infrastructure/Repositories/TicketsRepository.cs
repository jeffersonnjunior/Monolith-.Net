using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;

namespace Infrastructure.Repositories;

public class TicketsRepository : BaseRepository<Tickets>, ITicketsRepository
{
    public TicketsRepository(AppDbContext context, IUnitOfWork unitOfWork, NotificationContext notificationContext ) : base(context, unitOfWork, notificationContext)
    {
    }
}