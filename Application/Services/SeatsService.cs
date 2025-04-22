using Application.Dtos;
using Application.Interfaces.IFactories;
using Application.Interfaces.IServices;
using Application.Specification;
using Infrastructure.FiltersModel;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;

namespace Application.Services;

public class SeatsService : ISeatsService
{
    private readonly ISeatsRepository _seatsRepository;
    private readonly NotificationContext _notificationContext;
    private readonly SeatsSpecification _seatsSpecification;
    private readonly ISeatsFactory _seatsFactory;

    public SeatsService(
        ISeatsRepository seatsRepository,
        NotificationContext notificationContext,
        ISeatsFactory seatsFactory)
    {
        _seatsRepository = seatsRepository;
        _notificationContext = notificationContext;
        _seatsSpecification = new SeatsSpecification(notificationContext);
        _seatsFactory = seatsFactory;
    }

    public SeatsReadDto GetById(FilterSeatsById filterSeatsById)
    {
        var seat = _seatsRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = filterSeatsById.Id,
            Key = "Equal",
            Includes = filterSeatsById.Includes
        });

        return _seatsFactory.MapToSeatReadDto(seat);
    }

    public FilterReturn<SeatsReadDto> GetFilter(FilterSeatsTable filter)
    {
        var filterResult = _seatsRepository.GetFilter(filter);
        return new FilterReturn<SeatsReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = filterResult.ItensList.Select(_seatsFactory.MapToSeatReadDto)
        };
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
    }
}