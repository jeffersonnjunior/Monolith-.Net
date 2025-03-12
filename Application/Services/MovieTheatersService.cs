using Application.Dtos;
using Application.Interfaces.IServices;
using Application.Specification;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Services;

public class MovieTheatersService : IMovieTheatersService
{
    private readonly IMovieTheatersRepository _movieTheatersRepository;
    private readonly IMapper _mapper;
    private readonly NotificationContext _notificationContext;
    private readonly MovieTheatersSpecification  _movieTheatersSpecification;
    public MovieTheatersService(IMovieTheatersRepository movieTheatersRepository, IMapper mapper, NotificationContext notifierContext )
    {
        _movieTheatersRepository = movieTheatersRepository;
        _mapper = mapper;
        _notificationContext = notifierContext;
        _movieTheatersSpecification = new MovieTheatersSpecification(notifierContext);
    }

    public MovieTheatersReadDto GetById(FilterMovieTheatersById filterMovieTheatersById)
    {
        return _mapper.Map<MovieTheatersReadDto>(_movieTheatersRepository.GetByElement(new FilterByItem { Field = "Id", Value = filterMovieTheatersById.Id, Key = "Equal", Includes = filterMovieTheatersById.Includes }));
    }
    
    public FilterReturn<MovieTheatersReadDto> GetFilter(FilterMovieTheatersTable filter)
    {
        var filterResult = _movieTheatersRepository.GetFilter(filter);
        return new FilterReturn<MovieTheatersReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = _mapper.Map<IEnumerable<MovieTheatersReadDto>>(filterResult.ItensList)
        };
    }

    public MovieTheatersUpdateDto Add(MovieTheatersCreateDto movieTheatersCreateDto)
    {
        MovieTheatersUpdateDto movieTheatersUpdateDto = null;
    
        if (!_movieTheatersSpecification.IsSatisfiedBy(movieTheatersCreateDto)) return movieTheatersUpdateDto;
        
        if(!_movieTheatersRepository.ValidateInput(movieTheatersCreateDto, false)) return movieTheatersUpdateDto;
        
        MovieTheaters movieTheaters = _mapper.Map<MovieTheaters>(movieTheatersCreateDto);
        
        movieTheatersUpdateDto = _mapper.Map<MovieTheatersUpdateDto>(_movieTheatersRepository.Add(movieTheaters));
    
        return movieTheatersUpdateDto;
    }
    
    public void Update(MovieTheatersUpdateDto movieTheatersUpdateDto)
    {
        if (!_movieTheatersSpecification.IsSatisfiedBy(movieTheatersUpdateDto)) return;

        var existingMovieTheater = _movieTheatersRepository.GetByElement(new FilterByItem { Field = "Id", Value = movieTheatersUpdateDto.Id, Key = "Equal" });

        if (!_movieTheatersRepository.ValidateInput(movieTheatersUpdateDto, true, existingMovieTheater)) return;

        var movieTheaters = _mapper.Map<MovieTheaters>(movieTheatersUpdateDto);

        _movieTheatersRepository.Update(movieTheaters);
    }

    public void Delete(Guid id)
    {
        MovieTheaters existingMovieTheater = _movieTheatersRepository.GetByElement(new FilterByItem { Field = "Id", Value = id, Key = "Equal" });
        
        if (_notificationContext.HasNotifications()) return;
        
        _movieTheatersRepository.Delete(existingMovieTheater);
    }
}