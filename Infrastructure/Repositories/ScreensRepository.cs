using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;

namespace Infrastructure.Repositories;

public class ScreensRepository : BaseRepository<Screens>, IScreensRepository
{
    private readonly NotificationContext _notificationContext;
    private readonly IMovieTheatersRepository _movieTheatersRepository;
    public ScreensRepository(AppDbContext context, IUnitOfWork unitOfWork, NotificationContext notificationContext, IMovieTheatersRepository movieTheatersRepository) : base(context, unitOfWork, notificationContext)
    {
        _notificationContext = notificationContext;
        _movieTheatersRepository = movieTheatersRepository;
    }
    
    public Screens GetByElement(FilterByItem filterByItem)
    {
        (Screens screens, bool validadeIncludes) = GetElementEqual(filterByItem);
        
        if (validadeIncludes) return screens;

        if (filterByItem.Field == "Id" && screens is null) _notificationContext.AddNotification("Sala do cinema não registrada!");
        
        return screens;
    }
    
    public FilterReturn<Screens> GetFilter(FilterScreensTable filter)
    {
        var filters = new Dictionary<string, string>();

        if (!string.IsNullOrEmpty(filter.ScreenNumberContains))
            filters.Add(nameof(Screens.ScreenNumber), filter.ScreenNumberContains);

        if (filter.SeatingCapacityEqual.HasValue)
            filters.Add(nameof(Screens.SeatingCapacity), filter.SeatingCapacityEqual.Value.ToString());

        if (filter.MovieTheaterIdEqual.HasValue)
            filters.Add(nameof(Screens.MovieTheaterId), filter.MovieTheaterIdEqual.Value.ToString());

        (var result, bool validadeIncludes) = GetFilters(filters, filter.PageSize, filter.PageNumber, filter.Includes);

        return result;
    }
    public bool ValidateInput(object dto, bool isUpdate, Screens existingScreens = null)
    {
        var isValid = true;
        dynamic screensDto = dto;

        if (_movieTheatersRepository.GetByElement(new FilterByItem { Field = "Id", Value = screensDto.MovieTheaterId, Key = "Equal" }) is null) isValid = false;

        if (IsScreenNumberInUse(existingScreens, screensDto.MovieTheaterId, screensDto.ScreenNumber))
        {
            _notificationContext.AddNotification("Já existe uma sala com esse número para o mesmo cinema.");
            isValid = false;
        }

        return isValid;
    }
    public bool IsScreenNumberInUse(object? existingScreens, Guid movieTheatersId, string screenNumber)
    {
        bool isInUse = (existingScreens == null || ((dynamic)existingScreens).ScreenNumber != screenNumber) &&
                       GetByElement(new FilterByItem { Field = "MovieTheaterId", Value = movieTheatersId, Key = "Equal" }) is not null &&
                       GetByElement(new FilterByItem { Field = "ScreenNumber", Value = screenNumber, Key = "Equal" }) is not null;

        if (isInUse)
        {
            _notificationContext.AddNotification("Já existe uma sala com esse número para o mesmo cinema.");
        }

        return isInUse;
    }
}