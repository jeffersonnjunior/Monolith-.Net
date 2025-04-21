using System.Reflection;
using Application.Dtos;
using Domain.Enums;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;

namespace Application.Specification;

public class FilmsSpecification
{
    private readonly NotificationContext _notificationContext;

    public FilmsSpecification(NotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }

    public bool IsSatisfiedBy(object dto)
    {
        return dto switch
        {
            FilmsCreateDto createDto => ValidateCreateDto(createDto),
            FilmsUpdateDto updateDto => ValidateUpdateDto(updateDto),
            FilterFilmsById filterSeatsById => ValidateFilterFilmsById(filterSeatsById),
            _ => false
        };
    }

    public bool ValidateCreateDto(FilmsCreateDto createDto)
    {
        return ValidateName(createDto.Name)
            && ValidateDuration(createDto.Duration)
            && ValidateAgeRange(createDto.AgeRange)
            && ValidateFilmGenresRange(createDto.FilmGenres);
    }

    public bool ValidateUpdateDto(FilmsUpdateDto updateDto)
    {
        return ValidateId(updateDto.Id)
            && ValidateName(updateDto.Name)
            && ValidateDuration(updateDto.Duration)
            && ValidateAgeRange(updateDto.AgeRange)
            && ValidateFilmGenresRange(updateDto.FilmGenres);
    }
    
    public bool ValidateFilterFilmsById(FilterFilmsById filterSeatsById)
    {
        return ValidateId(filterSeatsById.Id)
            && ValidateIncludes(filterSeatsById.Includes);
    }

    private bool ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            _notificationContext.AddNotification("Nome é obrigatório.");
            return false;
        }
        return true;
    }

    private bool ValidateDuration(int duration)
    {
        if (duration <= 0)
        {
            _notificationContext.AddNotification("Duração deve ser maior que 0.");
            return false;
        }
        return true;
    }

    private bool ValidateAgeRange(int ageRange)
    {
        if (ageRange < 0)
        {
            _notificationContext.AddNotification("Faixa etária não pode ser menor que 0.");
            return false;
        }
        return true;
    }
    
    private bool ValidateFilmGenresRange(FilmGenres filmGenres)
    {
        if (!Enum.IsDefined(typeof(FilmGenres), filmGenres))
        {
            _notificationContext.AddNotification("O gênero do filme é inválido.");
            return false;
        }
        return true;
    }

    private bool ValidateId(Guid id)
    {
        if (id == Guid.Empty)
        {
            _notificationContext.AddNotification("Id é obrigatório.");
            return false;
        }
        return true;
    }
    
    private bool ValidateIncludes(IEnumerable<string> includes)
    {
        if (includes == null || !includes.Any())
        {
            return true;
        }

        var validProperties = typeof(FilmsReadDto).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var invalidIncludes = includes.Where(include => !validProperties.Contains(include)).ToList();

        if (invalidIncludes.Any())
        {
            _notificationContext.AddNotification($"Esses Includes ({string.Join(", ", invalidIncludes)}) não são válidos.");
            return false;
        }

        return true;
    }
}