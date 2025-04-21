using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;

namespace Infrastructure.Repositories;

public class MovieTheatersRepository : BaseRepository<MovieTheaters>, IMovieTheatersRepository
{
    private readonly NotificationContext _notificationContext;
    private readonly ITheaterLocationRepository _theaterLocationRepository;
    public MovieTheatersRepository(AppDbContext context, IUnitOfWork unitOfWork, NotificationContext notificationContext, ITheaterLocationRepository theaterLocationRepository) : base(context, unitOfWork, notificationContext)
    {
        _notificationContext = notificationContext;
        _theaterLocationRepository = theaterLocationRepository;
    }
    
    public MovieTheaters GetByElement(FilterByItem filterByItem)
    {
        (MovieTheaters movieTheaters, bool validadeIncludes) = GetElementEqual(filterByItem);

        if (validadeIncludes) return movieTheaters;
        
        if (filterByItem.Field == "Id" && movieTheaters is null) _notificationContext.AddNotification("Cinema não registrado!");
        
        return movieTheaters;
    }
    
    public FilterReturn<MovieTheaters> GetFilter(FilterMovieTheatersTable filter)
    {
        var filters = new Dictionary<string, string>();

        if (!string.IsNullOrEmpty(filter.NameContains))
            filters.Add(nameof(filter.NameContains), filter.NameContains);

        if (filter.TheaterLocationIdEqual.HasValue)
            filters.Add(nameof(filter.TheaterLocationIdEqual), filter.TheaterLocationIdEqual.Value.ToString());

        (var result, bool validadeIncludes) = GetFilters(filters, filter.PageSize, filter.PageNumber, filter.Includes);
    
        return result;
    }
    
    public bool ValidateInput(object dto, bool isUpdate, MovieTheaters existingMovieTheater = null)
    {
        var isValid = true;
        dynamic movieTheaterDto = dto;

        if (isUpdate && existingMovieTheater is null)
        {
            _notificationContext.AddNotification("Cinema não registrado!");
            return false;
        }

        if (IsNameInUse(existingMovieTheater, movieTheaterDto.Name))
        {
            _notificationContext.AddNotification("Esse nome de cinema já está em uso!");
            isValid = false;
        }

        if (IsTheaterLocationInUse(existingMovieTheater, movieTheaterDto.TheaterLocationId))
        {
            _notificationContext.AddNotification("Esse endereço já está em uso em outro cinema!");
            isValid = false;
        }

        if (_theaterLocationRepository.GetByElement(new FilterByItem { Field = "Id", Value = movieTheaterDto.TheaterLocationId, Key = "Equal" }) is null)
            isValid = false;

        return isValid;
    }
    
    private bool IsNameInUse(object? existingMovieTheater, string name)
    {
        return (existingMovieTheater == null || ((dynamic)existingMovieTheater).Name != name) &&
               GetByElement(new FilterByItem { Field = "Name", Value = name, Key = "Equal" }) is not null;
    }

    private bool IsTheaterLocationInUse(object? existingMovieTheater, Guid theaterLocationId)
    {
        return (existingMovieTheater == null || ((dynamic)existingMovieTheater).TheaterLocationId != theaterLocationId) &&
               GetByElement(new FilterByItem { Field = "TheaterLocationId", Value = theaterLocationId, Key = "Equal" }) is not null;
    }
}