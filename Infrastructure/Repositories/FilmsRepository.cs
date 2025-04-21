using Domain.Entities;
using Domain.Enums;
using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;

namespace Infrastructure.Repositories;

public class FilmsRepository : BaseRepository<Films>, IFilmsRepository
{
    private readonly NotificationContext _notificationContext;

    public FilmsRepository(AppDbContext context, IUnitOfWork unitOfWork, NotificationContext notificationContext) :
        base(context, unitOfWork, notificationContext)
    {
        _notificationContext = notificationContext;
    }

    public Films GetByElement(FilterByItem filterByItem)
    {
        (Films films, bool validadeIncludes) = GetElementEqual(filterByItem);

        if (validadeIncludes) return films;

        if (filterByItem.Field == "Id" && films is null) _notificationContext.AddNotification("Filme não registrado!");

        return films;
    }

    public FilterReturn<Films> GetFilter(FilterFilmsTable filter)
    {
        var filters = new Dictionary<string, string>();

        if (!string.IsNullOrEmpty(filter.NameContains))
            filters.Add(nameof(filter.NameContains), filter.NameContains);

        if (filter.FilmGenresEqual.HasValue)
            filters.Add(nameof(filter.FilmGenresEqual), ((int)filter.FilmGenresEqual.Value).ToString());

        if (filter.DurationEqual.HasValue)
            filters.Add(nameof(filter.DurationEqual), filter.DurationEqual.Value.ToString());
        
        if (filter.AgeRangeEqual.HasValue)
            filters.Add(nameof(filter.AgeRangeEqual), filter.AgeRangeEqual.Value.ToString());

        (var result, bool validadeIncludes) = GetFilters(filters, filter.PageSize, filter.PageNumber, filter.Includes);

        return result;
    }

    public bool ValidateInput(object dto, bool isUpdate, Films existingFilms = null)
    {
        var isValid = true;
        dynamic filmsDto = dto;

        if (!isUpdate && GetByElement(new FilterByItem { Field = "Name", Value = filmsDto.Name, Key = "Equal" }) is not null)
        {
            _notificationContext.AddNotification("Já existe um filme com esse nome.");
            isValid = false;
        }
        
        if(!IsNameInUse(existingFilms, filmsDto.Name)) isValid = false;

        if (!ValidadeAgeAndGenre(filmsDto.AgeRange, filmsDto.FilmGenres)) isValid = false;

        return isValid;
    }
    
    private bool IsNameInUse(object? existingFilms, string name)
    {
        if ((existingFilms == null || ((dynamic)existingFilms).Name != name) &&
            GetByElement(new FilterByItem { Field = "Name", Value = name, Key = "Equal" }) is not null)
        {
            _notificationContext.AddNotification("Esse nome de filme já está em uso!");
            return false;
        }
        return true;
    }

    public bool ValidadeAgeAndGenre(int ageRange, FilmGenres filmGenres)
    {
        var restrictedGenres = new List<FilmGenres> { FilmGenres.Horror, FilmGenres.Thriller, FilmGenres.Crime };
        var freeAgeGenres = new List<FilmGenres> { FilmGenres.Musical, FilmGenres.Family };

        if (ageRange < 18 && restrictedGenres.Contains(filmGenres))
        {
            _notificationContext.AddNotification($"Para filme de {filmGenres} a faixa etária tem que ser maior que 18.");
            return false;
        }

        if (ageRange != 0 && freeAgeGenres.Contains(filmGenres))
        {
            _notificationContext.AddNotification($"Para filme de {filmGenres} a faixa etária tem que ser livre.");
            return false;
        }

        return true;
    }
}