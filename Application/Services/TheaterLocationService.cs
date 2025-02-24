using Application.Dtos;
using Application.Interfaces.IServices;
using Application.Specification;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Services;

public class TheaterLocationService : ITheaterLocationService
{
    private readonly ITheaterLocationRepository _theaterLocationRepository;
    private readonly IMapper _mapper;
    private readonly NotificationContext _notifierContext;
    private readonly TheaterLocationSpecification<TheaterLocationCreateDto> _createSpecification;
    private readonly TheaterLocationSpecification<TheaterLocationUpdateDto> _updateSpecification;

    public TheaterLocationService(ITheaterLocationRepository theaterLocationRepository, IMapper mapper, NotificationContext notifierContext)
    {
        _theaterLocationRepository = theaterLocationRepository;
        _mapper = mapper;
        _notifierContext = notifierContext;
        _createSpecification = new TheaterLocationSpecification<TheaterLocationCreateDto>(_notifierContext);
        _updateSpecification = new TheaterLocationSpecification<TheaterLocationUpdateDto>(_notifierContext);
    }

    public TheaterLocationReadDto GetById(Guid id)
    {
        return _mapper.Map<TheaterLocationReadDto>(_theaterLocationRepository.GetById(new FilterByItem { Field = "Id", Value = id, Key = "Equal" }));
    }

    public FilterReturn<TheaterLocation> GetFilter(FilterTheaterLocation filter)
    {
        return _theaterLocationRepository.GetFilter(filter);
    }

    public TheaterLocationReadDto Add(TheaterLocationCreateDto theaterLocationCreateDto)
    {
        TheaterLocationReadDto theaterLocationReadDto = new();

        if (!_createSpecification.IsSatisfiedBy(theaterLocationCreateDto)) return theaterLocationReadDto;

        if (_theaterLocationRepository.GetById(new FilterByItem { Field = "Street", Value = theaterLocationCreateDto.Street, Key = "Equal" }) is not null) return theaterLocationReadDto;

        TheaterLocation theaterLocation = _mapper.Map<TheaterLocation>(theaterLocationCreateDto);
        theaterLocation = _theaterLocationRepository.Add(theaterLocation);

        return _mapper.Map(theaterLocation, theaterLocationReadDto);
    }

    public void Update(TheaterLocationUpdateDto theaterLocationUpdateDto)
    {
        if (!_updateSpecification.IsSatisfiedBy(theaterLocationUpdateDto)) return;

        var theaterLocation = _theaterLocationRepository.GetById(new FilterByItem {Field = "Id", Value = theaterLocationUpdateDto.Id, Key = "Equal"});

        if (_notifierContext.HasNotifications()) return; 
        
        _mapper.Map(theaterLocationUpdateDto, theaterLocation);

        _theaterLocationRepository.Update(theaterLocation);
    }

    public void Delete(Guid id)
    {
        TheaterLocation theaterLocation = _theaterLocationRepository.GetById(new FilterByItem { Field = "Id", Value = id, Key = "Equal" });

        if (_notifierContext.HasNotifications()) return;

        _theaterLocationRepository.Delete(theaterLocation);
    }
}