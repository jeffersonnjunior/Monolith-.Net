using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;

namespace Infrastructure.Repositories;

public class TheaterLocationRepository : BaseRepository<TheaterLocation>, ITheaterLocationRepository
{
    private readonly NotificationContext _notificationContext;
    public TheaterLocationRepository(AppDbContext context, IUnitOfWork unitOfWork, NotificationContext notificationContext) : base(context, unitOfWork)
    {
        _notificationContext = notificationContext;
    }
    public TheaterLocation GetById(Guid id)
    {
        TheaterLocation theaterLocation = GetElementByExpression(x => x.Id == id);

        if (theaterLocation is null) _notificationContext.AddNotification("Endereço do cinema não registrado!");

        return theaterLocation;
    }
}