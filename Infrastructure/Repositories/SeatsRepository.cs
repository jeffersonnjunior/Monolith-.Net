using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SeatsRepository : BaseRepository<Seats>, ISeatsRepository
{
    public SeatsRepository(AppDbContext context, IUnitOfWork unitOfWork, NotificationContext notificationContext ) : base(context, unitOfWork, notificationContext)
    {
    }
}

