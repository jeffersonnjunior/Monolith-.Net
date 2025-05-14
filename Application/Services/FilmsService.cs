using Application.Dtos;
using Application.Interfaces.IFactories;
using Application.Interfaces.IServices;
using Application.Specification;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Infrastructure.Interfaces.ICache.IServices;

namespace Application.Services;

public class FilmsService : IFilmsService
{
    private readonly IFilmsRepository _filmsRepository;
    private readonly IFilmeFactory _filmeFactory;
    private readonly ICacheService _cacheService;
    private readonly NotificationContext _notificationContext;
    private readonly FilmsSpecification _filmsSpecification;

    public FilmsService(
        IFilmsRepository filmsRepository,
        IFilmeFactory filmeFactory,
        ICacheService cacheService,
        NotificationContext notifierContext)
    {
        _filmsRepository = filmsRepository;
        _notificationContext = notifierContext;
        _filmeFactory = filmeFactory;
        _cacheService = cacheService;
        _filmsSpecification = new FilmsSpecification(notifierContext);
    }

    public FilmsReadDto GetById(FilterFilmsById filterFilmsById)
    {
        if (!_filmsSpecification.IsSatisfiedBy(filterFilmsById)) return null;

        string cacheKey = $"Films:Id:{filterFilmsById.Id}";
        var cached = _cacheService.Get<FilmsReadDto>(cacheKey);
        if (cached != null) return cached;

        var film = _filmsRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = filterFilmsById.Id,
            Key = "Equal",
            Includes = filterFilmsById.Includes
        });

        var filmsReadDto = _filmeFactory.MapToFilmeReadDto(film);
        _cacheService.Set(cacheKey, filmsReadDto, TimeSpan.FromMinutes(10));
        return filmsReadDto;
    }

    public FilterReturn<FilmsReadDto> GetFilter(FilterFilmsTable filter)
    {
        string cacheKey = $"Films:Filter:{filter.GetHashCode()}";
        var cached = _cacheService.Get<FilterReturn<FilmsReadDto>>(cacheKey);
        if (cached != null) return cached;

        var filterResult = _filmsRepository.GetFilter(filter);
        var result = new FilterReturn<FilmsReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = filterResult.ItensList.Select(_filmeFactory.MapToFilmeReadDto)
        };

        _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
        return result;
    }

    public FilmsUpdateDto Add(FilmsCreateDto filmsCreateDto)
    {
        FilmsUpdateDto filmsUpdateDto = null;
        if (!_filmsSpecification.IsSatisfiedBy(filmsCreateDto)) return filmsUpdateDto;
        if (!_filmsRepository.ValidateInput(filmsCreateDto, false)) return filmsUpdateDto;

        var films = _filmeFactory.MapToFilme(filmsCreateDto);
        filmsUpdateDto = _filmeFactory.MapToFilmeUpdateDto(_filmsRepository.Add(films));
        return filmsUpdateDto;
    }

    public void Update(FilmsUpdateDto filmsUpdateDto)
    {
        if (!_filmsSpecification.IsSatisfiedBy(filmsUpdateDto)) return;

        var existingFilms = _filmsRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = filmsUpdateDto.Id,
            Key = "Equal"
        });

        if (!_filmsRepository.ValidateInput(filmsUpdateDto, true, existingFilms)) return;

        var films = _filmeFactory.MapToFilmeFromUpdateDto(filmsUpdateDto);
        _filmsRepository.Update(films);

        _cacheService.Remove($"Films:Id:{filmsUpdateDto.Id}");
        _cacheService.RemoveByPrefix("Films:Filter:");
    }

    public void Delete(Guid id)
    {
        Films existingFilms = _filmsRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = id,
            Key = "Equal"
        });

        if (_notificationContext.HasNotifications()) return;

        _filmsRepository.Delete(existingFilms);

        _cacheService.Remove($"Films:Id:{id}");
        _cacheService.RemoveByPrefix("Films:Filter:");
    }
}