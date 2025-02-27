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
    private readonly NotificationContext _notifierContext;
    private readonly MovieTheatersSpecification<MovieTheatersCreateDto> _createSpecification;
    private readonly MovieTheatersSpecification<MovieTheatersUpdateDto> _updateSpecification;

    public MovieTheatersService(IMovieTheatersRepository movieTheatersRepository, IMapper mapper, NotificationContext notifierContext )
    {
        _movieTheatersRepository = movieTheatersRepository;
        _mapper = mapper;
        _notifierContext = notifierContext;
        _createSpecification = new MovieTheatersSpecification<MovieTheatersCreateDto>(_notifierContext);
        _updateSpecification = new MovieTheatersSpecification<MovieTheatersUpdateDto>(_notifierContext);
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
    
        if (!_createSpecification.IsSatisfiedBy(movieTheatersCreateDto)) return movieTheatersUpdateDto;
    
        if(!_movieTheatersRepository.ValidateInput(movieTheatersCreateDto, false)) return movieTheatersUpdateDto;
    
        MovieTheaters movieTheaters = _mapper.Map<MovieTheaters>(movieTheatersCreateDto);
        movieTheaters = _movieTheatersRepository.Add(movieTheaters);
    
        return _mapper.Map(movieTheaters, movieTheatersUpdateDto);
    }
    
    public void Update(MovieTheatersUpdateDto movieTheatersUpdateDto)
    {
        if (!_updateSpecification.IsSatisfiedBy(movieTheatersUpdateDto)) return;

        var existingMovieTheater = _movieTheatersRepository.GetByElement(new FilterByItem { Field = "Id", Value = movieTheatersUpdateDto.Id, Key = "Equal" });

        if (!_movieTheatersRepository.ValidateInput(movieTheatersUpdateDto, true, existingMovieTheater)) return;

        var movieTheaters = _mapper.Map<MovieTheaters>(movieTheatersUpdateDto);

        _movieTheatersRepository.Update(movieTheaters);
    }

    public void Delete(Guid id)
    {
        var existingMovieTheater = _movieTheatersRepository.GetByElement(new FilterByItem { Field = "Id", Value = id, Key = "Equal" });
        
        if(existingMovieTheater is null) return;
        
        _movieTheatersRepository.Delete(existingMovieTheater);
    }
}