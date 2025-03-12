using Application.Dtos;
using Application.Interfaces.IServices;
using Application.Specification;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Services;

public class SeatsService : ISeatsService
{
    private readonly ISeatsRepository _seatsRepository;
    private readonly IMapper _mapper;
    private readonly NotificationContext _notificationContext;
    private readonly SeatsSpecification _seatsSpecification;

    public SeatsService(ISeatsRepository seatsRepository, IMapper mapper ,NotificationContext notificationContext)
    {
        _seatsRepository = seatsRepository;
        _mapper = mapper;
        _notificationContext = notificationContext;
        _seatsSpecification = new SeatsSpecification(notificationContext);
    }

    public SeatsReadDto GetById(FilterSeatsById filterSeatsById)
    {
        SeatsReadDto seatsReadDto = null;

        if (!_seatsSpecification.IsSatisfiedBy(filterSeatsById)) return seatsReadDto;

        return _mapper.Map(_seatsRepository.GetByElement(new FilterByItem { Field = "Id", Value = filterSeatsById.Id, Key = "Equal", Includes = filterSeatsById.Includes }), seatsReadDto);
    }
    
    public FilterReturn<SeatsReadDto> GetFilter(FilterSeatsTable filter)
    {
        var filterResult = _seatsRepository.GetFilter(filter);
        return new FilterReturn<SeatsReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = _mapper.Map<IEnumerable<SeatsReadDto>>(filterResult.ItensList)
        };
    }
    
    public SeatsUpdateDto Add(SeatsCreateDto seatsCreateDto)
    {
        SeatsUpdateDto seatsUpdateDto = null;

        if (!_seatsSpecification.IsSatisfiedBy(seatsCreateDto)) return seatsUpdateDto;
        
        if(!_seatsRepository.ValidateInput(seatsCreateDto, false)) return seatsUpdateDto;

        Seats seats = _mapper.Map<Seats>(seatsCreateDto);
        
        seatsUpdateDto = _mapper.Map<SeatsUpdateDto>(_seatsRepository.Add(seats));

        return seatsUpdateDto;
    }

    public void Update(SeatsUpdateDto seatsUpdateDto)
    {
        if (!_seatsSpecification.IsSatisfiedBy(seatsUpdateDto)) return;

        var existingSeatsUpdate = _seatsRepository.GetByElement(new FilterByItem { Field = "Id", Value = seatsUpdateDto.Id, Key = "Equal" });

        if (!_seatsRepository.ValidateInput(seatsUpdateDto, true, existingSeatsUpdate)) return;

        var seats = _mapper.Map<Seats>(seatsUpdateDto);

        _seatsRepository.Update(seats);
    }

    public void Delete(Guid id)
    {
        Seats existingSeats = _seatsRepository.GetByElement(new FilterByItem { Field = "Id", Value = id, Key = "Equal" });
        
        if (_notificationContext.HasNotifications()) return;
        
        _seatsRepository.Delete(existingSeats);
    }
}