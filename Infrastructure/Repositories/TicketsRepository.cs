using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;

namespace Infrastructure.Repositories;

public class TicketsRepository : BaseRepository<Tickets>, ITicketsRepository
{
    private readonly NotificationContext _notificationContext;
    private readonly ISessionsRepository _sessionsRepository;
    private readonly ISeatsRepository _seatsRepository;
    private readonly ICustomerDetailsRepository _customerDetailsRepository;
    public TicketsRepository(AppDbContext context, IUnitOfWork unitOfWork, NotificationContext notificationContext, ISessionsRepository sessionsRepository, ISeatsRepository seatsRepository, ICustomerDetailsRepository customerDetailsRepository) : base(context, unitOfWork, notificationContext)
    {
        _notificationContext = notificationContext;
        _sessionsRepository = sessionsRepository;
        _seatsRepository = seatsRepository;
        _customerDetailsRepository = customerDetailsRepository;
    }

    public Tickets GetByElement(FilterByItem filterByItem)
    {
        (Tickets tickets, bool validadeIncludes) = GetElementEqual(filterByItem);

        if (validadeIncludes) return tickets;

        if (filterByItem.Field == "Id" && tickets is null) _notificationContext.AddNotification("Filme não registrado!");

        return tickets;
    }

    public FilterReturn<Tickets> GetFilter(FilterTicketsTable filter)
    {
        var filters = new Dictionary<string, string>();

        if (filter.SessionIdEqual.HasValue)
            filters.Add(nameof(filter.SessionIdEqual), filter.SessionIdEqual.Value.ToString());
        
        if (filter.SeatIdEqual.HasValue)
            filters.Add(nameof(filter.SeatIdEqual), filter.SeatIdEqual.Value.ToString());

        if (filter.ClientIdEqual.HasValue)
            filters.Add(nameof(filter.ClientIdEqual), (filter.ClientIdEqual.Value).ToString());
        
        (var result, bool validadeIncludes) = GetFilters(filters, filter.PageSize, filter.PageNumber, filter.Includes);

        return result;
    }

    public bool ValidateInput(object dto, bool isUpdate, Tickets existingTickets = null)
    {
        var isValid = true;
        dynamic ticketsDto = dto;
        
        if (_sessionsRepository.GetByElement(new FilterByItem { Field = "Id", Value = ticketsDto.SessionId, Key = "Equal" }) is null)
            isValid = false;
        
        if (_seatsRepository.GetByElement(new FilterByItem { Field = "Id", Value = ticketsDto.SeatId, Key = "Equal" }) is null)
            isValid = false;
        
        if (_customerDetailsRepository.GetByElement(new FilterByItem { Field = "Id", Value = ticketsDto.ClientId, Key = "Equal" }) is null)
            isValid = false;
        
        if (!isUpdate && GetByElement(new FilterByItem { Field = "SeatId", Value = ticketsDto.SeatId, Key = "Equal" }) is not null)
        {
            _notificationContext.AddNotification("Já está reservado esse lugar.");
            isValid = false;
        }

        return isValid;
    }
}