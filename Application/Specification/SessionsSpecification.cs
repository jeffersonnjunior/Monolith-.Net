using System.Reflection;
using Application.Dtos;
using Domain.Enums;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;

namespace Application.Specification;

public class SessionsSpecification
{
    private readonly NotificationContext _notificationContext;
    
    public SessionsSpecification(NotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }
    
    public bool IsSatisfiedBy(object dto)
    {
        return dto switch
        {
            SessionsCreateDto createDto => ValidateCreateDto(createDto),
            SessionsUpdateDto updateDto => ValidateUpdateDto(updateDto),
            FilterSessionsById filterSessionsById => ValidateFilterFilmsById(filterSessionsById),
            _ => false
        };
    }
    
    private bool ValidateCreateDto(SessionsCreateDto createDto)
    {
        return 
            ValidadeSessionTime(createDto.SessionTime)
            && ValidadeFilmId(createDto.FilmId)
            && ValidateFilmAudioOptionRange(createDto.FilmAudioOption)
            && ValidateFilmFormatRange(createDto.FilmFormat);
    }
    
    private bool ValidateUpdateDto(SessionsUpdateDto updateDto)
    {
        return 
            ValidateId(updateDto.Id)
            && ValidadeSessionTime(updateDto.SessionTime)
            && ValidadeFilmId(updateDto.FilmId)
            && ValidateFilmAudioOptionRange(updateDto.FilmAudioOption)
            && ValidateFilmFormatRange(updateDto.FilmFormat);
    }
    
    private bool ValidateFilterFilmsById(FilterSessionsById filterSessionsById)
    {
        return 
            ValidateId(filterSessionsById.Id)
            && ValidateIncludes(filterSessionsById.Includes);
    }
    
    private bool ValidadeSessionTime(DateTime sessionTime)
    {
        if (sessionTime == DateTime.MinValue || sessionTime == DateTime.MaxValue)
        {
            _notificationContext.AddNotification("O horário da sessão é obrigatório.");
            return false;
        }
        return true;
    }
    
    private bool ValidadeFilmId(Guid filmId)
    {
        if (filmId == Guid.Empty)
        {
            _notificationContext.AddNotification("Id do filme é obrigatório.");
            return false;
        }
        return true;
    }
    
    private bool ValidateFilmAudioOptionRange(FilmAudioOption filmAudioOption)
    {
        if (!Enum.IsDefined(typeof(FilmAudioOption), filmAudioOption))
        {
            _notificationContext.AddNotification("O áudio do filme é inválido.");
            return false;
        }
        return true;
    }
    
    private bool ValidateFilmFormatRange(FilmFormat filmFormat)
    {
        if (!Enum.IsDefined(typeof(FilmFormat), filmFormat))
        {
            _notificationContext.AddNotification("O formato do filme é inválido.");
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

        var validProperties = typeof(SessionsReadDto).GetProperties(BindingFlags.Public | BindingFlags.Instance)
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