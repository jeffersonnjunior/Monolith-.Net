using Application.Dtos;
using Application.Interfaces.IFactory;
using Application.Interfaces.IServices;
using Application.Specification;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Services;

public class SessionsService : ISessionsService
{
    private readonly ISessionsRepository _sessionsRepository;
    private readonly NotificationContext _notificationContext;
    private readonly SessionsSpecification _sessionsSpecification;
    private readonly ISessionsFactory _sessionsFactory;

    public SessionsService(
        ISessionsRepository sessionsRepository,
        NotificationContext notificationContext,
        ISessionsFactory sessionsFactory)
    {
        _sessionsRepository = sessionsRepository;
        _notificationContext = notificationContext;
        _sessionsSpecification = new SessionsSpecification(notificationContext);
        _sessionsFactory = sessionsFactory;
    }

    public SessionsReadDto GetById(FilterSessionsById filterSessionsById)
    {
        var session = _sessionsRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = filterSessionsById.Id,
            Key = "Equal",
            Includes = filterSessionsById.Includes
        });

        return _sessionsFactory.MapToSessionReadDto(session);
    }

    public FilterReturn<SessionsReadDto> GetFilter(FilterSessionsTable filter)
    {
        var filterResult = _sessionsRepository.GetFilter(filter);
        return new FilterReturn<SessionsReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = filterResult.ItensList.Select(_sessionsFactory.MapToSessionReadDto)
        };
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
    }
}