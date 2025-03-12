using Application.Dtos;
using Application.Interfaces.IServices;
using Application.Specification;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Services;

public class ScreensService : IScreensService
{
    private readonly IScreensRepository _screensRepository;
    private readonly IMapper _mapper;
    private readonly NotificationContext _notificationContext;
    private readonly ScreensSpecification _screensSpecification;

    public ScreensService(IScreensRepository screensRepository, IMapper mapper, NotificationContext notificationContext)
    {
        _screensRepository = screensRepository;
        _mapper = mapper;
        _notificationContext = notificationContext;
        _screensSpecification = new ScreensSpecification(notificationContext);
    }

    public ScreensReadDto GetById(FilterScreensById filterScreensById)
    {
        ScreensReadDto screensReadDto = null;

        if (!_screensSpecification.IsSatisfiedBy(filterScreensById)) return screensReadDto;
        
        return _mapper.Map(_screensRepository.GetByElement(new FilterByItem { Field = "Id", Value = filterScreensById.Id, Key = "Equal", Includes = filterScreensById.Includes }), screensReadDto);
    }
    
    public FilterReturn<ScreensReadDto> GetFilter(FilterScreensTable filter)
    {
        var filterResult = _screensRepository.GetFilter(filter);
        return new FilterReturn<ScreensReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = _mapper.Map<IEnumerable<ScreensReadDto>>(filterResult.ItensList)
        };
    }

    public ScreensUpdateDto Add(ScreensCreateDto screensCreateDto)
    {
        ScreensUpdateDto screensUpdateDto = null;
        
        if (!_screensSpecification.IsSatisfiedBy(screensCreateDto)) return screensUpdateDto;
        
        if(!_screensRepository.ValidateInput(screensCreateDto, false)) return screensUpdateDto;
        
        if(!_screensRepository.ValidateInput(screensCreateDto, false)) return screensUpdateDto;
        
        Screens screen = _mapper.Map<Screens>(screensCreateDto);
        
        screensUpdateDto = _mapper.Map<ScreensUpdateDto>(_screensRepository.Add(screen));
    
        return screensUpdateDto;
    }
    
    public void Update(ScreensUpdateDto screensUpdateDto)
    {
        if (!_screensSpecification.IsSatisfiedBy(screensUpdateDto)) return;
        
        var existingScreens = _screensRepository.GetByElement(new FilterByItem { Field = "Id", Value = screensUpdateDto.Id, Key = "Equal" });
        
        if(!_screensRepository.ValidateInput(screensUpdateDto, true, existingScreens)) return;

        var screen = _mapper.Map<Screens>(screensUpdateDto);
        
        _screensRepository.Update(screen);
    }
    
    public void Delete(Guid id)
    {
        Screens existingScreens = _screensRepository.GetByElement(new FilterByItem { Field = "Id", Value = id, Key = "Equal" });

        if (existingScreens is null) return;
        
        _screensRepository.Delete(existingScreens);
    }
}