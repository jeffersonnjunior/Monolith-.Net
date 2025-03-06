using Application.Dtos;
using Application.Interfaces.IServices;
using Application.Specification;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Services;

public class SessionsService : ISessionsService
{
    private readonly ISessionsRepository _sessionsRepository;
    private readonly IMapper _mapper;
    private readonly NotificationContext _notificationContext;
    private readonly SessionsSpecification _sessionsSpecification;
    
    public SessionsService(ISessionsRepository sessionsRepository, IMapper mapper, NotificationContext notificationContext)
    {
        _sessionsRepository = sessionsRepository;
        _mapper = mapper;
        _notificationContext = notificationContext;
        _sessionsSpecification = new SessionsSpecification(notificationContext);
    }


    public SessionsReadDto GetById(FilterSessionsById filterSessionsById)
    {
        SessionsReadDto sessionsReadDto = null;
        
        if (!_sessionsSpecification.IsSatisfiedBy(filterSessionsById)) return sessionsReadDto;
        
        return _mapper.Map(_sessionsRepository.GetByElement(new FilterByItem { Field = "Id", Value = filterSessionsById.Id, Key = "Equal", Includes = filterSessionsById.Includes }), sessionsReadDto);
    }

    public FilterReturn<SessionsReadDto> GetFilter(FilterSessionsTable filter)
    {
        var filterResult = _sessionsRepository.GetFilter(filter);
        return new FilterReturn<SessionsReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = _mapper.Map<IEnumerable<SessionsReadDto>>(filterResult.ItensList)
        };
    }

    public SessionsUpdateDto Add(SessionsCreateDto sessionsCreateDto)
    {
        SessionsUpdateDto sessionsUpdateDto = null;

        if(!_sessionsSpecification.IsSatisfiedBy(sessionsCreateDto)) return sessionsUpdateDto;
        
        if(!_sessionsRepository.ValidateInput(sessionsCreateDto, false)) return sessionsUpdateDto;
        
        Sessions sessions = _mapper.Map<Sessions>(sessionsCreateDto);
        
        sessionsUpdateDto = _mapper.Map<SessionsUpdateDto>(_sessionsRepository.Add(sessions));

        return sessionsUpdateDto;
    }

    public void Update(SessionsUpdateDto sessionsUpdateDto)
    {
        if(!_sessionsSpecification.IsSatisfiedBy(sessionsUpdateDto)) return;
        
        var existingSessions = _sessionsRepository.GetByElement(new FilterByItem { Field = "Id", Value = sessionsUpdateDto.Id, Key = "Equal" });
        
        if(!_sessionsRepository.ValidateInput(sessionsUpdateDto, false)) return;
        
        var sessions = _mapper.Map<Sessions>(sessionsUpdateDto);
        _sessionsRepository.Update(sessions);
    }

    public void Delete(Guid id)
    {
        var existingSessions = _sessionsRepository.GetByElement(new FilterByItem { Field = "Id", Value = id, Key = "Equal" });
        
        if(existingSessions is null) return;
        
        _sessionsRepository.Delete(existingSessions);
    }
}