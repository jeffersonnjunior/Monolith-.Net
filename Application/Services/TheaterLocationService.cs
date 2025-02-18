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
    private readonly TheaterLocationSpecification _specification;


    public TheaterLocationService(ITheaterLocationRepository theaterLocationRepository, IMapper mapper, NotificationContext notifierContext)
    {
        _theaterLocationRepository = theaterLocationRepository;
        _mapper = mapper;
        _notifierContext = notifierContext;
        _specification = new TheaterLocationSpecification(_notifierContext);
    }

    public TheaterLocationDto GetById(Guid id)
    {
        TheaterLocation theaterLocation = _theaterLocationRepository.GetById(id);

        if (_notifierContext.HasNotifications()) return null;

        return _mapper.Map<TheaterLocationDto>(theaterLocation);
    }
    public ReturnTable<TheaterLocation> GetFilter(TheaterLocationFilter filter, string[] includes)
    {
        return _theaterLocationRepository.GetFilter(filter, includes);
    }
    public TheaterLocationDto Add(TheaterLocationDto theaterLocationDto)
    {
        if (!_specification.IsSatisfiedBy(theaterLocationDto)) return theaterLocationDto;

        TheaterLocation theaterLocation = _mapper.Map<TheaterLocation>(theaterLocationDto);

        _mapper.Map(theaterLocationDto, theaterLocation);

        _theaterLocationRepository.Add(theaterLocation);

        return theaterLocationDto;
    }
    public void Update(TheaterLocationDto theaterLocationDto)
    {
        if(!_specification.IsSatisfiedBy(theaterLocationDto)) return;

        TheaterLocation theaterLocation = _theaterLocationRepository.GetById(theaterLocationDto.Id);

        if(_notifierContext.HasNotifications()) return;

        _mapper.Map(theaterLocationDto, theaterLocation);

        _theaterLocationRepository.Update(theaterLocation);
    }

    public void Delete(Guid id)
    {
        TheaterLocation theaterLocation = _theaterLocationRepository.GetById(id);

        if (_notifierContext.HasNotifications()) return;

        _theaterLocationRepository.Delete(theaterLocation);
    }
}