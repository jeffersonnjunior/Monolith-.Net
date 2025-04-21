using Application.Dtos;
using Application.Interfaces.IFactory;
using Application.Interfaces.IServices;
using Application.Specification;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;

namespace Application.Services;

public class TicketsService : ITicketsService
{
    private readonly ITicketsRepository _ticketsRepository;
    private readonly NotificationContext _notificationContext;
    private readonly TicketsSpecification _ticketsSpecification;
    private readonly ITicketsFactory _ticketsFactory;

    public TicketsService(
        ITicketsRepository ticketsRepository,
        NotificationContext notifierContext,
        ITicketsFactory ticketsFactory)
    {
        _ticketsRepository = ticketsRepository;
        _notificationContext = notifierContext;
        _ticketsSpecification = new TicketsSpecification(notifierContext);
        _ticketsFactory = ticketsFactory;
    }

    public TicketsReadDto GetById(FilterTicketsById filterTicketsById)
    {
        var ticket = _ticketsRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = filterTicketsById.Id,
            Key = "Equal",
            Includes = filterTicketsById.Includes
        });

        return _ticketsFactory.MapToTicketReadDto(ticket);
    }

    public FilterReturn<TicketsReadDto> GetFilter(FilterTicketsTable filter)
    {
        var filterResult = _ticketsRepository.GetFilter(filter);
        return new FilterReturn<TicketsReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = filterResult.ItensList.Select(_ticketsFactory.MapToTicketReadDto)
        };
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
    }
}