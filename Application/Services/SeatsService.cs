using Application.Dtos;
using Application.Interfaces.IFactories;
using Application.Interfaces.IServices;
using Application.Specification;
using Infrastructure.FiltersModel;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Interfaces.ICache.IServices;

namespace Application.Services;

public class SeatsService : ISeatsService
{
    private readonly ISeatsRepository _seatsRepository;
    private readonly ISeatsFactory _seatsFactory;
    private readonly ICacheService _cacheService;
    private readonly NotificationContext _notificationContext;
    private readonly SeatsSpecification _seatsSpecification;

    public SeatsService(
        ISeatsRepository seatsRepository,
        ISeatsFactory seatsFactory,
        ICacheService cacheService,
        NotificationContext notificationContext)
    {
        _seatsRepository = seatsRepository;
        _seatsFactory = seatsFactory;
        _cacheService = cacheService;
        _notificationContext = notificationContext;
        _seatsSpecification = new SeatsSpecification(notificationContext);
    }

    public SeatsReadDto GetById(FilterSeatsById filterSeatsById)
    {
        string cacheKey = $"Seats:Id:{filterSeatsById.Id}";
        var cached = _cacheService.Get<SeatsReadDto>(cacheKey);
        if (cached != null) return cached;

        var seat = _seatsRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = filterSeatsById.Id,
            Key = "Equal",
            Includes = filterSeatsById.Includes
        });

        var dto = _seatsFactory.MapToSeatReadDto(seat);
        _cacheService.Set(cacheKey, dto, TimeSpan.FromMinutes(10));
        return dto;
    }

    public FilterReturn<SeatsReadDto> GetFilter(FilterSeatsTable filter)
    {
        string cacheKey = $"Seats:Filter:{filter.GetHashCode()}";
        var cached = _cacheService.Get<FilterReturn<SeatsReadDto>>(cacheKey);
        if (cached != null) return cached;

        var filterResult = _seatsRepository.GetFilter(filter);
        var result = new FilterReturn<SeatsReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = filterResult.ItensList.Select(_seatsFactory.MapToSeatReadDto)
        };

        _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
        return result;
    }

    public SeatsUpdateDto Add(SeatsCreateDto seatsCreateDto)
    {
        SeatsUpdateDto seatsUpdateDto = null;
        if (!_seatsSpecification.IsSatisfiedBy(seatsCreateDto)) return seatsUpdateDto;
        if (!_seatsRepository.ValidateInput(seatsCreateDto, false)) return seatsUpdateDto;

        var seat = _seatsFactory.MapToSeat(seatsCreateDto);
        seatsUpdateDto = _seatsFactory.MapToSeatUpdateDto(_seatsRepository.Add(seat));
        return seatsUpdateDto;
    }

    public void Update(SeatsUpdateDto seatsUpdateDto)
    {
        if (!_seatsSpecification.IsSatisfiedBy(seatsUpdateDto)) return;

        var existingSeat = _seatsRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = seatsUpdateDto.Id,
            Key = "Equal"
        });

        if (!_seatsRepository.ValidateInput(seatsUpdateDto, true, existingSeat)) return;

        var seat = _seatsFactory.MapToSeatFromUpdateDto(seatsUpdateDto);
        _seatsRepository.Update(seat);

        _cacheService.Remove($"Seats:Id:{seatsUpdateDto.Id}");
        _cacheService.RemoveByPrefix("Seats:Filter:");
    }

    public void Delete(Guid id)
    {
        var existingSeat = _seatsRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = id,
            Key = "Equal"
        });

        if (existingSeat is null) return;

        _seatsRepository.Delete(existingSeat);

        _cacheService.Remove($"Seats:Id:{id}");
        _cacheService.RemoveByPrefix("Seats:Filter:");
    }
}