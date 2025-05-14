using Application.Dtos;
using Application.Interfaces.IFactories;
using Application.Interfaces.IServices;
using Application.Specification;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Infrastructure.Interfaces.ICache.IServices;

namespace Application.Services;

public class SessionsService : ISessionsService
{
    private readonly ISessionsRepository _sessionsRepository;
    private readonly ISessionsFactory _sessionsFactory;
    private readonly ICacheService _cacheService;
    private readonly NotificationContext _notificationContext;
    private readonly SessionsSpecification _sessionsSpecification;

    public SessionsService(
        ISessionsRepository sessionsRepository,
        ISessionsFactory sessionsFactory,
        ICacheService cacheService,
        NotificationContext notificationContext)
    {
        _sessionsRepository = sessionsRepository;
        _sessionsFactory = sessionsFactory;
        _cacheService = cacheService;
        _notificationContext = notificationContext;
        _sessionsSpecification = new SessionsSpecification(notificationContext);
    }

    public SessionsReadDto GetById(FilterSessionsById filterSessionsById)
    {
        string cacheKey = $"Sessions:Id:{filterSessionsById.Id}";
        var cached = _cacheService.Get<SessionsReadDto>(cacheKey);
        if (cached != null) return cached;

        var session = _sessionsRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = filterSessionsById.Id,
            Key = "Equal",
            Includes = filterSessionsById.Includes
        });

        var dto = _sessionsFactory.MapToSessionReadDto(session);
        _cacheService.Set(cacheKey, dto, TimeSpan.FromMinutes(10));
        return dto;
    }

    public FilterReturn<SessionsReadDto> GetFilter(FilterSessionsTable filter)
    {
        string cacheKey = $"Sessions:Filter:{filter.GetHashCode()}";
        var cached = _cacheService.Get<FilterReturn<SessionsReadDto>>(cacheKey);
        if (cached != null) return cached;

        var filterResult = _sessionsRepository.GetFilter(filter);
        var result = new FilterReturn<SessionsReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = filterResult.ItensList.Select(_sessionsFactory.MapToSessionReadDto)
        };

        _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
        return result;
    }

    public SessionsUpdateDto Add(SessionsCreateDto sessionsCreateDto)
    {
        SessionsUpdateDto sessionsUpdateDto = null;
        if (!_sessionsSpecification.IsSatisfiedBy(sessionsCreateDto)) return sessionsUpdateDto;
        if (!_sessionsRepository.ValidateInput(sessionsCreateDto, false)) return sessionsUpdateDto;

        var session = _sessionsFactory.MapToSession(sessionsCreateDto);
        sessionsUpdateDto = _sessionsFactory.MapToSessionUpdateDto(_sessionsRepository.Add(session));
        return sessionsUpdateDto;
    }

    public void Update(SessionsUpdateDto sessionsUpdateDto)
    {
        if (!_sessionsSpecification.IsSatisfiedBy(sessionsUpdateDto)) return;

        var existingSession = _sessionsRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = sessionsUpdateDto.Id,
            Key = "Equal"
        });

        if (!_sessionsRepository.ValidateInput(sessionsUpdateDto, false, existingSession)) return;

        var session = _sessionsFactory.MapToSessionFromUpdateDto(sessionsUpdateDto);
        _sessionsRepository.Update(session);

        _cacheService.Remove($"Sessions:Id:{sessionsUpdateDto.Id}");
        _cacheService.RemoveByPrefix("Sessions:Filter:");
    }

    public void Delete(Guid id)
    {
        var existingSession = _sessionsRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = id,
            Key = "Equal"
        });

        if (existingSession is null) return;

        _sessionsRepository.Delete(existingSession);

        _cacheService.Remove($"Sessions:Id:{id}");
        _cacheService.RemoveByPrefix("Sessions:Filter:");
    }
}