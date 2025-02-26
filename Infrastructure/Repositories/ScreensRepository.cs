using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ScreensRepository : BaseRepository<Screens>, IScreensRepository
{
    public ScreensRepository(AppDbContext context, IUnitOfWork unitOfWork, NotificationContext notificationContext ) : base(context, unitOfWork, notificationContext)
    {
    }
}

