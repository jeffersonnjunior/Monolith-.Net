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
    private readonly ScreensSpecification<ScreensCreateDto> _createSpecification;
    private readonly ScreensSpecification<ScreensUpdateDto> _updateSpecification;

    public ScreensService(IScreensRepository screensRepository, IMapper mapper, NotificationContext notificationContext)
    {
        _screensRepository = screensRepository;
        _mapper = mapper;
        _notificationContext = notificationContext;
        _createSpecification = new ScreensSpecification<ScreensCreateDto>(_notificationContext);
        _updateSpecification = new ScreensSpecification<ScreensUpdateDto>(_notificationContext);
    }

    public ScreensReadDto GetById(FilterScreensById filterScreensById)
    {
        return _mapper.Map<ScreensReadDto>(_screensRepository.GetByElement(new FilterByItem { Field = "Id", Value = filterScreensById.Id, Key = "Equal", Includes = filterScreensById.Includes }));
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
        
        if (!_createSpecification.IsSatisfiedBy(screensCreateDto)) return screensUpdateDto;
        
        if(!_screensRepository.ValidateInput(screensCreateDto, false)) return screensUpdateDto;

        var screen = _mapper.Map<Screens>(screensCreateDto);
        screen = _screensRepository.Add(screen);

        return _mapper.Map(screen, screensUpdateDto);
    }
    
    public void Update(ScreensUpdateDto screensUpdateDto)
    {
        if (!_updateSpecification.IsSatisfiedBy(screensUpdateDto)) return;
        
        var existingScreens = _screensRepository.GetByElement(new FilterByItem { Field = "Id", Value = screensUpdateDto.Id, Key = "Equal" });
        
        if(!_screensRepository.ValidateInput(screensUpdateDto, true, existingScreens)) return;

        var screen = _mapper.Map<Screens>(screensUpdateDto);
        _screensRepository.Update(screen);
    }
    
    public void Delete(Guid id)
    {
        var existingScreens = _screensRepository.GetByElement(new FilterByItem { Field = "Id", Value = id, Key = "Equal" });

        if(existingScreens is null) return;
        
        _screensRepository.Delete(existingScreens);
    }
}