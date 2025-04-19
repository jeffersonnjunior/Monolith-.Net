using Application.Dtos;
using Application.Interfaces.IFactory;
using Application.Interfaces.IServices;
using Application.Specification;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Services;

public class MovieTheatersService : IMovieTheatersService
{
    private readonly IMovieTheatersRepository _movieTheatersRepository;
    private readonly NotificationContext _notificationContext;
    private readonly MovieTheatersSpecification _movieTheatersSpecification;
    private readonly IMovieTheatersFactory _movieTheatersFactory;

    public MovieTheatersService(
        IMovieTheatersRepository movieTheatersRepository, 
        NotificationContext notifierContext,
        IMovieTheatersFactory movieTheatersFactory)
    {
        _movieTheatersRepository = movieTheatersRepository;
        _notificationContext = notifierContext;
        _movieTheatersSpecification = new MovieTheatersSpecification(notifierContext);
        _movieTheatersFactory = movieTheatersFactory;
    }

    public MovieTheatersReadDto GetById(FilterMovieTheatersById filterMovieTheatersById)
    {
        var movieTheater = _movieTheatersRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = filterMovieTheatersById.Id,
            Key = "Equal",
            Includes = filterMovieTheatersById.Includes
        });

        return _movieTheatersFactory.MapToMovieTheaterReadDto(movieTheater);
    }

    public FilterReturn<MovieTheatersReadDto> GetFilter(FilterMovieTheatersTable filter)
    {
        var filterResult = _movieTheatersRepository.GetFilter(filter);
        return new FilterReturn<MovieTheatersReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = filterResult.ItensList.Select(_movieTheatersFactory.MapToMovieTheaterReadDto)
        };
    }

    public MovieTheatersUpdateDto Add(MovieTheatersCreateDto movieTheatersCreateDto)
    {
        MovieTheatersUpdateDto movieTheatersUpdateDto = null;

        if (!_movieTheatersSpecification.IsSatisfiedBy(movieTheatersCreateDto)) return movieTheatersUpdateDto;

        if (!_movieTheatersRepository.ValidateInput(movieTheatersCreateDto, false)) return movieTheatersUpdateDto;

        var movieTheater = _movieTheatersFactory.MapToMovieTheater(movieTheatersCreateDto);

        movieTheatersUpdateDto = _movieTheatersFactory.MapToMovieTheaterUpdateDto(_movieTheatersRepository.Add(movieTheater));

        return movieTheatersUpdateDto;
    }

    public void Update(MovieTheatersUpdateDto movieTheatersUpdateDto)
    {
        if (!_movieTheatersSpecification.IsSatisfiedBy(movieTheatersUpdateDto)) return;

        var existingMovieTheater = _movieTheatersRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = movieTheatersUpdateDto.Id,
            Key = "Equal"
        });

        if (!_movieTheatersRepository.ValidateInput(movieTheatersUpdateDto, true, existingMovieTheater)) return;

        var movieTheater = _movieTheatersFactory.MapToMovieTheaterFromUpdateDto(movieTheatersUpdateDto);

        _movieTheatersRepository.Update(movieTheater);
    }

    public void Delete(Guid id)
    {
        var existingMovieTheater = _movieTheatersRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = id,
            Key = "Equal"
        });

        if (existingMovieTheater is null) return;

        _movieTheatersRepository.Delete(existingMovieTheater);
    }
}