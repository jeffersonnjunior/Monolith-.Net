using Application.Dtos;
using Application.Interfaces.IFactories;
using Application.Interfaces.IServices;
using Application.Specification;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Infrastructure.Interfaces.ICache.IServices;

namespace Application.Services;

public class TheaterLocationService : ITheaterLocationService
{
    private readonly ITheaterLocationRepository _theaterLocationRepository;
    private readonly ITheaterLocationFactory _theaterLocationFactory;
    private readonly ICacheService _cacheService;
    private readonly NotificationContext _notifierContext;
    private readonly TheaterLocationSpecification _theaterLocationSpecification;

    public TheaterLocationService(
        ITheaterLocationRepository theaterLocationRepository,
        ITheaterLocationFactory theaterLocationFactory,
        ICacheService cacheService,
        NotificationContext notifierContext)
    {
        _theaterLocationRepository = theaterLocationRepository;
        _theaterLocationFactory = theaterLocationFactory;
        _cacheService = cacheService;
        _notifierContext = notifierContext;
        _theaterLocationSpecification = new TheaterLocationSpecification(notifierContext);
    }

    public TheaterLocationReadDto GetById(FilterTheaterLocationById filterTheaterLocationById)
    {
        string cacheKey = $"TheaterLocation:Id:{filterTheaterLocationById.Id}";
        var cached = _cacheService.Get<TheaterLocationReadDto>(cacheKey);
        if (cached != null) return cached;

        var theaterLocation = _theaterLocationRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = filterTheaterLocationById.Id,
            Key = "Equal",
            Includes = filterTheaterLocationById.Includes
        });

        var dto = _theaterLocationFactory.MapToTheaterLocationReadDto(theaterLocation);
        _cacheService.Set(cacheKey, dto, TimeSpan.FromMinutes(10));
        return dto;
    }

    public FilterReturn<TheaterLocationReadDto> GetFilter(FilterTheaterLocationTable filter)
    {
        string cacheKey = $"TheaterLocation:Filter:{filter.GetHashCode()}";
        var cached = _cacheService.Get<FilterReturn<TheaterLocationReadDto>>(cacheKey);
        if (cached != null) return cached;

        var filterResult = _theaterLocationRepository.GetFilter(filter);
        var result = new FilterReturn<TheaterLocationReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = filterResult.ItensList.Select(_theaterLocationFactory.MapToTheaterLocationReadDto)
        };

        _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
        return result;
    }

    public TheaterLocationUpdateDto Add(TheaterLocationCreateDto theaterLocationCreateDto)
    {
        TheaterLocationUpdateDto theaterLocationUpdateDto = null;
        if (!_theaterLocationSpecification.IsSatisfiedBy(theaterLocationCreateDto)) return theaterLocationUpdateDto;
        if (_theaterLocationRepository.GetByElement(new FilterByItem { Field = "Street", Value = theaterLocationCreateDto.Street, Key = "Equal" }) != null) return theaterLocationUpdateDto;

        var theaterLocation = _theaterLocationFactory.MapToTheaterLocation(theaterLocationCreateDto);
        theaterLocation = _theaterLocationRepository.Add(theaterLocation);
        return _theaterLocationFactory.MapToTheaterLocationUpdateDto(theaterLocation);
    }

    public void Update(TheaterLocationUpdateDto theaterLocationUpdateDto)
    {
        if (!_theaterLocationSpecification.IsSatisfiedBy(theaterLocationUpdateDto)) return;

        var theaterLocation = _theaterLocationRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = theaterLocationUpdateDto.Id,
            Key = "Equal"
        });

        if (_notifierContext.HasNotifications()) return;

        var updatedTheaterLocation = _theaterLocationFactory.MapToTheaterLocationFromUpdateDto(theaterLocationUpdateDto);
        _theaterLocationRepository.Update(updatedTheaterLocation);

        _cacheService.Remove($"TheaterLocation:Id:{theaterLocationUpdateDto.Id}");
        _cacheService.RemoveByPrefix("TheaterLocation:Filter:");
    }

    public void Delete(Guid id)
    {
        var theaterLocation = _theaterLocationRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = id,
            Key = "Equal"
        });

        if (theaterLocation is null) return;

        _theaterLocationRepository.Delete(theaterLocation);

        _cacheService.Remove($"TheaterLocation:Id:{id}");
        _cacheService.RemoveByPrefix("TheaterLocation:Filter:");
    }
}