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

    public TheaterLocation GetById(Guid id)
    {
        TheaterLocation theaterLocation = GetElementByExpression(x => x.Id == id);

        if (theaterLocation is null) _notificationContext.AddNotification("Endereço do cinema não registrado!");

        return theaterLocation;
    }

    public TheaterLocation GetExisting(string street)
    {
        TheaterLocation theaterLocation = GetElementByExpression(x => x.Street == street);

        if (theaterLocation is not null) _notificationContext.AddNotification("Endereço já cadastrado!");

        return theaterLocation;
    }

    public ReturnTable<TheaterLocation> GetFilter(TheaterLocationFilter filter, params string[] includes)
    {
        var filters = new Dictionary<string, string>();

        if (!string.IsNullOrEmpty(filter.StreetContains))
            filters.Add(nameof(filter.StreetContains), filter.StreetContains);

        if (!string.IsNullOrEmpty(filter.UnitNumberContains))
            filters.Add(nameof(filter.UnitNumberContains), filter.UnitNumberContains);

        if (!string.IsNullOrEmpty(filter.PostalCodeContains))
            filters.Add(nameof(filter.PostalCodeContains), filter.PostalCodeContains);

        var query = GetFilters(filters, includes);
        var totalRegister = _dbSet.Count();
        var totalRegisterFilter = query.Count();

        var paginatedList = query.ApplyDynamicFilters(filters, filter.PageSize, filter.PageNumber).ToList();

        var totalPages = (int)Math.Ceiling((double)totalRegisterFilter / filter.PageSize);

        return new ReturnTable<TheaterLocation>
        {
            TotalRegister = totalRegister,
            TotalRegisterFilter = totalRegisterFilter,
            TotalPages = totalPages,
            ItensList = paginatedList
        };
    }
}
