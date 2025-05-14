using Application.Dtos;
using Application.Interfaces.IFactories;
using Application.Interfaces.IServices;
using Application.Specification;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Infrastructure.Interfaces.ICache.IServices;

namespace Application.Services;

public class MovieTheatersService : IMovieTheatersService
{
    private readonly IMovieTheatersRepository _movieTheatersRepository;
    private readonly IMovieTheatersFactory _movieTheatersFactory;
    private readonly ICacheService _cacheService;
    private readonly NotificationContext _notificationContext;
    private readonly MovieTheatersSpecification _movieTheatersSpecification;

    public MovieTheatersService(
        IMovieTheatersRepository movieTheatersRepository,
        IMovieTheatersFactory movieTheatersFactory,
        ICacheService cacheService,
        NotificationContext notifierContext)
    {
        _movieTheatersRepository = movieTheatersRepository;
        _notificationContext = notifierContext;
        _movieTheatersFactory = movieTheatersFactory;
        _cacheService = cacheService;
        _movieTheatersSpecification = new MovieTheatersSpecification(notifierContext);
    }

    public MovieTheatersReadDto GetById(FilterMovieTheatersById filterMovieTheatersById)
    {
        string cacheKey = $"MovieTheaters:Id:{filterMovieTheatersById.Id}";
        var cached = _cacheService.Get<MovieTheatersReadDto>(cacheKey);
        if (cached != null) return cached;

        var movieTheater = _movieTheatersRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = filterMovieTheatersById.Id,
            Key = "Equal",
            Includes = filterMovieTheatersById.Includes
        });

        var dto = _movieTheatersFactory.MapToMovieTheaterReadDto(movieTheater);
        _cacheService.Set(cacheKey, dto, TimeSpan.FromMinutes(10));
        return dto;
    }

    public FilterReturn<MovieTheatersReadDto> GetFilter(FilterMovieTheatersTable filter)
    {
        string cacheKey = $"MovieTheaters:Filter:{filter.GetHashCode()}";
        var cached = _cacheService.Get<FilterReturn<MovieTheatersReadDto>>(cacheKey);
        if (cached != null) return cached;

        var filterResult = _movieTheatersRepository.GetFilter(filter);
        var result = new FilterReturn<MovieTheatersReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = filterResult.ItensList.Select(_movieTheatersFactory.MapToMovieTheaterReadDto)
        };

        _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
        return result;
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

        _cacheService.Remove($"MovieTheaters:Id:{movieTheatersUpdateDto.Id}");
        _cacheService.RemoveByPrefix("MovieTheaters:Filter:");
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

        _cacheService.Remove($"MovieTheaters:Id:{id}");
        _cacheService.RemoveByPrefix("MovieTheaters:Filter:");
    }
}