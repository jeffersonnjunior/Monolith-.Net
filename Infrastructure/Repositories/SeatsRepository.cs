using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;

namespace Infrastructure.Repositories;

public class SeatsRepository : BaseRepository<Seats>, ISeatsRepository
{
    private readonly NotificationContext _notificationContext;
    private readonly IScreensRepository _screensRepository;

    public SeatsRepository(AppDbContext context, IUnitOfWork unitOfWork, NotificationContext notificationContext,
        IScreensRepository screensRepository) : base(context, unitOfWork, notificationContext)
    {
        _notificationContext = notificationContext;
        _screensRepository = screensRepository;

    }

    public Seats GetByElement(FilterByItem filterByItem)
    {
        (Seats seats, bool validadeIncludes) = GetElementEqual(filterByItem);

        if (validadeIncludes) return seats;
        
        if (filterByItem.Field == "Id" && seats is null) _notificationContext.AddNotification("Cinema não registrado!");
        
        return seats;
    }

    public FilterReturn<Seats> GetFilter(FilterSeatsTable filter)
    {
        var filters = new Dictionary<string, string>();
        
        if (filter.SeatNumber.HasValue)
            filters.Add(nameof(filter.SeatNumber), filter.SeatNumber.Value.ToString());

        if (!string.IsNullOrEmpty(filter.RowLetter))
            filters.Add(nameof(filter.RowLetter), filter.RowLetter);

        if (filter.ScreenId.HasValue)
            filters.Add(nameof(filter.ScreenId), filter.ScreenId.Value.ToString());

        (var result, bool validadeIncludes) = GetFilters(filters, filter.PageSize, filter.PageNumber, filter.Includes);
    
        return result;
    }

    public bool ValidateInput(object dto, bool isUpdate, Seats existingSeats = null)
    {
        var isValid = true;
        dynamic seatsDto = dto;

        if (IsSeatNumberAndRowInUse(seatsDto, isUpdate, existingSeats))
        {
            _notificationContext.AddNotification("Esté assento e a letra da fileira já estão em uso.");
            isValid = false;
        }
        
        if (_screensRepository.GetByElement(new FilterByItem { Field = "Id", Value = seatsDto.ScreenId, Key = "Equal" }) is null)
            isValid = false;
        
        if (!isUpdate && CheckSeatCapacity(seatsDto))
        {
            _notificationContext.AddNotification("Essa sala não tem espaço para novas cadeiras");
            isValid = false;
        }
        
        return isValid;
    }
    private bool IsSeatNumberAndRowInUse(dynamic dto, bool isUpdate, Seats seatsDto)
    {
        int seatNumber = (int) dto.SeatNumber;
        string rowLetter = dto.RowLetter.ToString();
        Guid screenId = (Guid)dto.ScreenId;

        var validadeSeat = GetElementByExpression(sc => sc.ScreenId == screenId && sc.SeatNumber == seatNumber && sc.RowLetter == rowLetter);

        if (validadeSeat == null) return false;

        if (isUpdate && seatsDto != null && validadeSeat.Id == seatsDto.Id) return false;

        return true;
    }
    private bool CheckSeatCapacity(dynamic seatsDto)
    {
        var screen = _screensRepository.GetByElement(new FilterByItem { Field = "Id", Value = seatsDto.ScreenId, Key = "Equal" });

        if (screen is null) return false;

        Guid screenId = (Guid)seatsDto.ScreenId;
        var seatsCount = GetByExpression(sc => sc.ScreenId == screenId).ToList().Count();
    
        return seatsCount + 1 > screen.SeatingCapacity;
    }
}
