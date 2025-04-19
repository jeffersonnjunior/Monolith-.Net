using Application.Dtos;
using Application.Interfaces.IFactory;
using Application.Interfaces.IServices;
using Application.Specification;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Services;

public class FilmsService : IFilmsService
{
    private readonly IFilmsRepository _filmsRepository;
    private readonly NotificationContext _notificationContext;
    private readonly FilmsSpecification _filmsSpecification;
    private readonly IFilmeFactory _filmeFactory;

    public FilmsService(IFilmsRepository filmsRepository, NotificationContext notifierContext, IFilmeFactory filmeFactory)
    {
        _filmsRepository = filmsRepository;
        _notificationContext = notifierContext;
        _filmsSpecification = new FilmsSpecification(notifierContext);
        _filmeFactory = filmeFactory;
    }

    public FilmsReadDto GetById(FilterFilmsById filterFilmsById)
    {
        FilmsReadDto filmsReadDto = null;

        if (!_filmsSpecification.IsSatisfiedBy(filterFilmsById)) return filmsReadDto;

        var film = _filmsRepository.GetByElement(new FilterByItem { Field = "Id", Value = filterFilmsById.Id, Key = "Equal", Includes = filterFilmsById.Includes });
        return _filmeFactory.MapToFilmeReadDto(film);
    }

    public FilterReturn<FilmsReadDto> GetFilter(FilterFilmsTable filter)
    {
        var filterResult = _filmsRepository.GetFilter(filter);
        return new FilterReturn<FilmsReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = filterResult.ItensList.Select(_filmeFactory.MapToFilmeReadDto)
        };
    }

    public FilmsUpdateDto Add(FilmsCreateDto filmsCreateDto)
    {
        FilmsUpdateDto filmsUpdateDto = null;

        if (!_filmsSpecification.IsSatisfiedBy(filmsCreateDto)) return filmsUpdateDto;

        if (!_filmsRepository.ValidateInput(filmsCreateDto, false)) return filmsUpdateDto;

        var films = _filmeFactory.MapToFilme(filmsCreateDto);

        filmsUpdateDto = _filmeFactory.MapToFilmeUpdateDto(_filmsRepository.Add(films));

        return filmsUpdateDto;
    }

    public void Update(FilmsUpdateDto filmsUpdateDto)
    {
        if (!_filmsSpecification.IsSatisfiedBy(filmsUpdateDto)) return;

        var existingFilms = _filmsRepository.GetByElement(new FilterByItem { Field = "Id", Value = filmsUpdateDto.Id, Key = "Equal" });

        if (!_filmsRepository.ValidateInput(filmsUpdateDto, true, existingFilms)) return;

        var films = _filmeFactory.MapToFilmeFromUpdateDto(filmsUpdateDto);

        _filmsRepository.Update(films);
    }

    public void Delete(Guid id)
    {
        Films existingFilms = _filmsRepository.GetByElement(new FilterByItem { Field = "Id", Value = id, Key = "Equal" });

        if (_notificationContext.HasNotifications()) return;

        _filmsRepository.Delete(existingFilms);
    }
}