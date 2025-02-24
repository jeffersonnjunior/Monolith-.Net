using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;
using Infrastructure.Utilities.FunctionsDatabase;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repositories;

public class TheaterLocationRepository : BaseRepository<TheaterLocation>, ITheaterLocationRepository
{
    private readonly NotificationContext _notificationContext;
    public TheaterLocationRepository(AppDbContext context, IUnitOfWork unitOfWork, NotificationContext notificationContext) : base(context, unitOfWork)
    {
        _notificationContext = notificationContext;
    }

    public TheaterLocation GetById(FilterByItem filterByItem)
    {
        TheaterLocation theaterLocation = GetElementByParameter(filterByItem);

        if (filterByItem.Field == "Id" && theaterLocation is null) _notificationContext.AddNotification("Endereço do cinema não registrado!");

        if (filterByItem.Field == "Street" && theaterLocation is not null) _notificationContext.AddNotification("Endereço já cadastrado!");

        return theaterLocation;
    }

    public FilterReturn<TheaterLocation> GetFilter(FilterTheaterLocation filter)
    {
        var filters = new Dictionary<string, string>();

        if (!string.IsNullOrEmpty(filter.StreetContains))
            filters.Add(nameof(filter.StreetContains), filter.StreetContains);

        if (!string.IsNullOrEmpty(filter.UnitNumberContains))
            filters.Add(nameof(filter.UnitNumberContains), filter.UnitNumberContains);

        if (!string.IsNullOrEmpty(filter.PostalCodeContains))
            filters.Add(nameof(filter.PostalCodeContains), filter.PostalCodeContains);

        var result = GetFilters(filters, filter.PageSize, filter.PageNumber, filter.Includes);

        return result;
    }
}