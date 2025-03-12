using Application.Dtos;
using Application.Interfaces.IServices;
using Application.Specification;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Services;

public class TicketsService : ITicketsService
{
    private readonly ITicketsRepository _ticketsRepository;
    private readonly IMapper _mapper;
    private readonly NotificationContext _notificationContext;
    private readonly TicketsSpecification _ticketsSpecification;
    public TicketsService(ITicketsRepository ticketsRepository, IMapper mapper, NotificationContext notifierContext)
    {
        _ticketsRepository = ticketsRepository;
        _mapper = mapper;
        _notificationContext = notifierContext;
        _ticketsSpecification = new TicketsSpecification(notifierContext);
    }

    public TicketsReadDto GetById(FilterTicketsById filterTicketsById)
    {
        return _mapper.Map<TicketsReadDto>(_ticketsRepository.GetByElement(new FilterByItem { Field = "Id", Value = filterTicketsById.Id, Key = "Equal", Includes = filterTicketsById.Includes }));

    }

    public FilterReturn<TicketsReadDto> GetFilter(FilterTicketsTable filter)
    {
        var filterResult = _ticketsRepository.GetFilter(filter);
        return new FilterReturn<TicketsReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = _mapper.Map<IEnumerable<TicketsReadDto>>(filterResult.ItensList)
        };
    }

    public TicketsUpdateDto Add(TicketsCreateDto ticketsCreateDto)
    {
        TicketsUpdateDto ticketsUpdateDto = null;

        if (!_ticketsSpecification.IsSatisfiedBy(ticketsCreateDto)) return ticketsUpdateDto;

        if (!_ticketsRepository.ValidateInput(ticketsCreateDto, false)) return ticketsUpdateDto;
         
        Tickets tickets = _mapper.Map<Tickets>(ticketsCreateDto);
        
        ticketsUpdateDto = _mapper.Map<TicketsUpdateDto>(_ticketsRepository.Add(tickets));

        return ticketsUpdateDto;
    }

    public void Update(TicketsUpdateDto ticketsUpdateDto)
    {
        if (!_ticketsSpecification.IsSatisfiedBy(ticketsUpdateDto)) return;

        if (!_ticketsRepository.ValidateInput(ticketsUpdateDto, true)) return;

        Tickets tickets = _mapper.Map<Tickets>(ticketsUpdateDto);
        
        _ticketsRepository.Update(tickets);
    }

    public void Delete(Guid id)
    {
        Tickets existingTickets = _ticketsRepository.GetByElement(new FilterByItem { Field = "Id", Value = id, Key = "Equal" });

        if (existingTickets is null) return;
        
        _ticketsRepository.Delete(existingTickets);
    }
}
