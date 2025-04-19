using Application.Dtos;
using Application.Interfaces.IFactory;
using Application.Interfaces.IServices;
using Application.Specification;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Services;

public class TheaterLocationService : ITheaterLocationService
{
    private readonly ITheaterLocationRepository _theaterLocationRepository;
    private readonly NotificationContext _notifierContext;
    private readonly TheaterLocationSpecification _theaterLocationSpecification;
    private readonly ITheaterLocationFactory _theaterLocationFactory;

    public TheaterLocationService(
        ITheaterLocationRepository theaterLocationRepository,
        NotificationContext notifierContext,
        ITheaterLocationFactory theaterLocationFactory)
    {
        _theaterLocationRepository = theaterLocationRepository;
        _notifierContext = notifierContext;
        _theaterLocationSpecification = new TheaterLocationSpecification(notifierContext);
        _theaterLocationFactory = theaterLocationFactory;
    }

    public TheaterLocationReadDto GetById(FilterTheaterLocationById filterTheaterLocationById)
    {
        var theaterLocation = _theaterLocationRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = filterTheaterLocationById.Id,
            Key = "Equal",
            Includes = filterTheaterLocationById.Includes
        });

        return _theaterLocationFactory.MapToTheaterLocationReadDto(theaterLocation);
    }

    public FilterReturn<TheaterLocationReadDto> GetFilter(FilterTheaterLocationTable filter)
    {
        var filterResult = _theaterLocationRepository.GetFilter(filter);
        return new FilterReturn<TheaterLocationReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = filterResult.ItensList.Select(_theaterLocationFactory.MapToTheaterLocationReadDto)
        };
    }

    public TheaterLocationUpdateDto Add(TheaterLocationCreateDto theaterLocationCreateDto)
    {
        TheaterLocationUpdateDto theaterLocationUpdateDto = null;

        if (!_theaterLocationSpecification.IsSatisfiedBy(theaterLocationCreateDto)) return theaterLocationUpdateDto;

        if (_theaterLocationRepository.GetByElement(new FilterByItem { Field = "Street", Value = theaterLocationCreateDto.Street, Key = "Equal" }) is not null) return theaterLocationUpdateDto;

        var theaterLocation = _theaterLocationFactory.MapToTheaterLocation(theaterLocationCreateDto);
        theaterLocation = _theaterLocationRepository.Add(theaterLocation);

        return _theaterLocationFactory.MapToTheaterLocationUpdateDto(theaterLocation);
    }

    public void Update(TheaterLocationUpdateDto theaterLocationUpdateDto)
    {
        if (!_theaterLocationSpecification.IsSatisfiedBy(theaterLocationUpdateDto)) return;

        var theaterLocation = _theaterLocationRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = theaterLocationUpdateDto.Id,
            Key = "Equal"
        });

        if (_notifierContext.HasNotifications()) return;

        var updatedTheaterLocation = _theaterLocationFactory.MapToTheaterLocationFromUpdateDto(theaterLocationUpdateDto);

        _theaterLocationRepository.Update(updatedTheaterLocation);
    }

    public void Delete(Guid id)
    {
        var theaterLocation = _theaterLocationRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = id,
            Key = "Equal"
        });

        if (theaterLocation is null) return;

        _theaterLocationRepository.Delete(theaterLocation);
    }
}