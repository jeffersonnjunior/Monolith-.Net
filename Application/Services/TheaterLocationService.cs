using Application.Dtos;
using Application.Interfaces.IServices;
using Application.Specification;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;

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
        throw new NotImplementedException();
    }
    public List<TheaterLocationDto> GetFilter()
    {
        throw new NotImplementedException();
    }
    public void Add(TheaterLocationDto theaterLocationDto)
    {
        TheaterLocation theaterLocation = _mapper.Map<TheaterLocation>(theaterLocationDto);

        _theaterLocationRepository.Add(theaterLocation);
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

        if (theaterLocation == null) throw new Exception("Theater location not found.");

        _theaterLocationRepository.Delete(theaterLocation);
    }
}