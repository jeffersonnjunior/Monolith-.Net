using Application.Dtos;
using Application.Interfaces.IFactories;
using Application.Interfaces.IServices;
using Application.Specification;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Infrastructure.Interfaces.ICache.IServices;

namespace Application.Services;

public class TicketsService : ITicketsService
{
    private readonly ITicketsRepository _ticketsRepository;
    private readonly ITicketsFactory _ticketsFactory;
    private readonly ICacheService _cacheService;
    private readonly NotificationContext _notificationContext;
    private readonly TicketsSpecification _ticketsSpecification;

    public TicketsService(
        ITicketsRepository ticketsRepository,
        ITicketsFactory ticketsFactory,
        ICacheService cacheService,
        NotificationContext notifierContext)
    {
        _ticketsRepository = ticketsRepository;
        _ticketsFactory = ticketsFactory;
        _cacheService = cacheService;
        _notificationContext = notifierContext;
        _ticketsSpecification = new TicketsSpecification(notifierContext);
    }

    public TicketsReadDto GetById(FilterTicketsById filterTicketsById)
    {
        string cacheKey = $"Tickets:Id:{filterTicketsById.Id}";
        var cached = _cacheService.Get<TicketsReadDto>(cacheKey);
        if (cached != null) return cached;

        var ticket = _ticketsRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = filterTicketsById.Id,
            Key = "Equal",
            Includes = filterTicketsById.Includes
        });

        var dto = _ticketsFactory.MapToTicketReadDto(ticket);
        _cacheService.Set(cacheKey, dto, TimeSpan.FromMinutes(10));
        return dto;
    }

    public FilterReturn<TicketsReadDto> GetFilter(FilterTicketsTable filter)
    {
        string cacheKey = $"Tickets:Filter:{filter.GetHashCode()}";
        var cached = _cacheService.Get<FilterReturn<TicketsReadDto>>(cacheKey);
        if (cached != null) return cached;

        var filterResult = _ticketsRepository.GetFilter(filter);
        var result = new FilterReturn<TicketsReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = filterResult.ItensList.Select(_ticketsFactory.MapToTicketReadDto)
        };

        _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
        return result;
    }

    public TicketsUpdateDto Add(TicketsCreateDto ticketsCreateDto)
    {
        TicketsUpdateDto ticketsUpdateDto = null;
        if (!_ticketsSpecification.IsSatisfiedBy(ticketsCreateDto)) return ticketsUpdateDto;
        if (!_ticketsRepository.ValidateInput(ticketsCreateDto, false)) return ticketsUpdateDto;

        var ticket = _ticketsFactory.MapToTicket(ticketsCreateDto);
        ticketsUpdateDto = _ticketsFactory.MapToTicketUpdateDto(_ticketsRepository.Add(ticket));
        return ticketsUpdateDto;
    }

    public void Update(TicketsUpdateDto ticketsUpdateDto)
    {
        if (!_ticketsSpecification.IsSatisfiedBy(ticketsUpdateDto)) return;
        if (!_ticketsRepository.ValidateInput(ticketsUpdateDto, true)) return;

        var ticket = _ticketsFactory.MapToTicketFromUpdateDto(ticketsUpdateDto);
        _ticketsRepository.Update(ticket);

        _cacheService.Remove($"Tickets:Id:{ticketsUpdateDto.Id}");
        _cacheService.RemoveByPrefix("Tickets:Filter:");
    }

    public void Delete(Guid id)
    {
        var existingTicket = _ticketsRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = id,
            Key = "Equal"
        });

        if (existingTicket is null) return;

        _ticketsRepository.Delete(existingTicket);

        _cacheService.Remove($"Tickets:Id:{id}");
        _cacheService.RemoveByPrefix("Tickets:Filter:");
    }
}