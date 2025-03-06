using Domain.Entities;
using Domain.Enums;
using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;

namespace Infrastructure.Repositories;

public class SessionsRepository : BaseRepository<Sessions>, ISessionsRepository
{
    private readonly NotificationContext _notificationContext;
    private readonly IFilmsRepository _filmsRepository;
    public SessionsRepository(AppDbContext context, IUnitOfWork unitOfWork, NotificationContext notificationContext, IFilmsRepository filmsRepository) : base(context, unitOfWork, notificationContext)
    {
        _notificationContext = notificationContext;
        _filmsRepository = filmsRepository;
    }

    public Sessions GetByElement(FilterByItem filterByItem)
    {
        (Sessions sessions, bool validadeIncludes) = GetElementEqual(filterByItem);

        if (validadeIncludes) return sessions;
        
        if (filterByItem.Field == "Id" && sessions is null) _notificationContext.AddNotification("Sessão não registrado!");
        
        return sessions;
    }
    
    public FilterReturn<Sessions> GetFilter(FilterSessionsTable filter)
    {
        var filters = new Dictionary<string, string>();

        if (filter.SessionTimeEqual.HasValue)
            filters.Add(nameof(filter.SessionTimeEqual), filter.SessionTimeEqual.Value.ToString("o")); 
        
        if (filter.FilmIdEqual.HasValue)
            filters.Add(nameof(filter.FilmIdEqual), filter.FilmIdEqual.Value.ToString());
        
        if (filter.FilmFormatEqual.HasValue)
            filters.Add(nameof(filter.FilmFormatEqual), ((int)filter.FilmFormatEqual.Value).ToString()); 
        
        if (filter.FilmAudioOptionEqual.HasValue)
            filters.Add(nameof(filter.FilmAudioOptionEqual), ((int)filter.FilmAudioOptionEqual.Value).ToString());

        (var result, bool validadeIncludes) = GetFilters(filters, filter.PageSize, filter.PageNumber, filter.Includes);

        return result;
    }
    
    public bool ValidateInput(object dto, bool isUpdate, Sessions existingSessions = null)
    {
        var isValid = true;
        dynamic sessionsDto = dto;

        if (_filmsRepository.GetByElement(new FilterByItem { Field = "Id", Value = sessionsDto.FilmId, Key = "Equal" }) is null) isValid = false;

        if (ValidadeSessionInUse(existingSessions, (Guid)sessionsDto.FilmId, (DateTime)sessionsDto.SessionTime, (FilmAudioOption)sessionsDto.FilmAudioOption, (FilmFormat)sessionsDto.FilmFormat)) isValid = false;

        return isValid;
    }

    private bool ValidadeSessionInUse(Sessions existingSessions, Guid filmId, DateTime sessionDateTime, FilmAudioOption filmAudioOption, FilmFormat filmFormat)
    {
        var validadeSession = GetElementByExpression(sc => sc.FilmId == filmId && sc.SessionTime == sessionDateTime && sc.FilmAudioOption == filmAudioOption && sc.FilmFormat == filmFormat);

        if (validadeSession != null && (existingSessions == null || existingSessions.Id != validadeSession.Id))
        {
            _notificationContext.AddNotification("Já existe uma sessão para esse filme nesse horário com o mesmo formato e opção de áudio.");
            return true;
        }

        return false;
    }
}