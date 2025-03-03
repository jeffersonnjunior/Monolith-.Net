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
    private readonly TheaterLocationSpecification _theaterLocationSpecification;

    public TheaterLocationService(ITheaterLocationRepository theaterLocationRepository, IMapper mapper, NotificationContext notifierContext)
    {
        _theaterLocationRepository = theaterLocationRepository;
        _mapper = mapper;
        _notifierContext = notifierContext;
        _theaterLocationSpecification = new TheaterLocationSpecification(notifierContext);
    }

    public TheaterLocationReadDto GetById(FilterTheaterLocationById filterTheaterLocationById)
    {
        return _mapper.Map<TheaterLocationReadDto>(_theaterLocationRepository.GetByElement(new FilterByItem { Field = "Id", Value = filterTheaterLocationById.Id, Key = "Equal", Includes = filterTheaterLocationById.Includes }));
    }

    public FilterReturn<TheaterLocationReadDto> GetFilter(FilterTheaterLocationTable filter)
    {
        var filterResult = _theaterLocationRepository.GetFilter(filter);
        return new FilterReturn<TheaterLocationReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = _mapper.Map<IEnumerable<TheaterLocationReadDto>>(filterResult.ItensList)
        };
    }

    public TheaterLocationUpdateDto Add(TheaterLocationCreateDto theaterLocationCreateDto)
    {
        TheaterLocationUpdateDto theaterLocationUpdateDto = null;

        if (!_theaterLocationSpecification.IsSatisfiedBy(theaterLocationCreateDto)) return theaterLocationUpdateDto;

        if (_theaterLocationRepository.GetByElement(new FilterByItem { Field = "Street", Value = theaterLocationCreateDto.Street, Key = "Equal" }) is not null) return theaterLocationUpdateDto;

        TheaterLocation theaterLocation = _mapper.Map<TheaterLocation>(theaterLocationCreateDto);
        theaterLocation = _theaterLocationRepository.Add(theaterLocation);

        return _mapper.Map(theaterLocation, theaterLocationUpdateDto);
    }

    public void Update(TheaterLocationUpdateDto theaterLocationUpdateDto)
    {
        if (!_theaterLocationSpecification.IsSatisfiedBy(theaterLocationUpdateDto)) return;

        var theaterLocation = _theaterLocationRepository.GetByElement(new FilterByItem {Field = "Id", Value = theaterLocationUpdateDto.Id, Key = "Equal"});

        if (_notifierContext.HasNotifications()) return; 
        
        _mapper.Map(theaterLocationUpdateDto, theaterLocation);

        _theaterLocationRepository.Update(theaterLocation);
    }

    public void Delete(Guid id)
    {
        TheaterLocation theaterLocation = _theaterLocationRepository.GetByElement(new FilterByItem { Field = "Id", Value = id, Key = "Equal" });

        if (_notifierContext.HasNotifications()) return;

        _theaterLocationRepository.Delete(theaterLocation);
    }
}