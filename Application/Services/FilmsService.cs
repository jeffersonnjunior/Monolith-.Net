using Application.Dtos;
using Application.Interfaces.IServices;
using Application.Specification;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Services;

public class FilmsService : IFilmsService
{
    private readonly IFilmsRepository _filmsRepository;
    private readonly IMapper _mapper;
    private readonly NotificationContext _notificationContext;
    private readonly FilmsSpecification _filmsSpecification;

    public FilmsService(IFilmsRepository filmsRepository, IMapper mapper, NotificationContext notifierContext )
    {
        _filmsRepository = filmsRepository;
        _mapper = mapper;
        _notificationContext = notifierContext;
        _filmsSpecification = new FilmsSpecification(notifierContext);
    }


    public FilmsReadDto GetById(FilterFilmsById filterFilmsById)
    {
        FilmsReadDto filmsReadDto = null;

        if (!_filmsSpecification.IsSatisfiedBy(filterFilmsById)) return filmsReadDto;

        return _mapper.Map(_filmsRepository.GetByElement(new FilterByItem { Field = "Id", Value = filterFilmsById.Id, Key = "Equal", Includes = filterFilmsById.Includes }), filmsReadDto);
    }

    public FilterReturn<FilmsReadDto> GetFilter(FilterFilmsTable filter)
    {
        var filterResult = _filmsRepository.GetFilter(filter);
        return new FilterReturn<FilmsReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = _mapper.Map<IEnumerable<FilmsReadDto>>(filterResult.ItensList)
        };
    }

    public FilmsUpdateDto Add(FilmsCreateDto filmsCreateDto)
    {
        FilmsUpdateDto filmsUpdateDto = null;

        if (!_filmsSpecification.IsSatisfiedBy(filmsCreateDto)) return filmsUpdateDto;

        if(!_filmsRepository.ValidateInput(filmsCreateDto, false)) return filmsUpdateDto;
        
        Films films = _mapper.Map<Films>(filmsCreateDto);
        
        filmsUpdateDto = _mapper.Map<FilmsUpdateDto>(_filmsRepository.Add(films));
    
        return filmsUpdateDto;
    }

    public void Update(FilmsUpdateDto filmsUpdateDto)
    {
        if (!_filmsSpecification.IsSatisfiedBy(filmsUpdateDto)) return;
        
        var existingFilms = _filmsRepository.GetByElement(new FilterByItem { Field = "Id", Value = filmsUpdateDto.Id, Key = "Equal" });
        
        if(!_filmsRepository.ValidateInput(filmsUpdateDto, true, existingFilms)) return;
        
        var films = _mapper.Map<Films>(filmsUpdateDto);

        _filmsRepository.Update(films);
    }

    public void Delete(Guid id)
    {
        Films existingFilms = _filmsRepository.GetByElement(new FilterByItem { Field = "Id", Value = id, Key = "Equal" });
        
        if (_notificationContext.HasNotifications()) return;
        
        _filmsRepository.Delete(existingFilms);

    }
}