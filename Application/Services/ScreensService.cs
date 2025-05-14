using Application.Dtos;
using Application.Interfaces.IFactories;
using Application.Interfaces.IServices;
using Application.Specification;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Infrastructure.Interfaces.ICache.IServices;

namespace Application.Services;

public class ScreensService : IScreensService
{
    private readonly IScreensRepository _screensRepository;
    private readonly IScreensFactory _screensFactory;
    private readonly ICacheService _cacheService;
    private readonly NotificationContext _notificationContext;
    private readonly ScreensSpecification _screensSpecification;

    public ScreensService(
        IScreensRepository screensRepository,
        IScreensFactory screensFactory,
        ICacheService cacheService,
        NotificationContext notificationContext)
    {
        _screensRepository = screensRepository;
        _screensFactory = screensFactory;
        _cacheService = cacheService;
        _notificationContext = notificationContext;
        _screensSpecification = new ScreensSpecification(notificationContext);
    }

    public ScreensReadDto GetById(FilterScreensById filterScreensById)
    {
        string cacheKey = $"Screens:Id:{filterScreensById.Id}";
        var cached = _cacheService.Get<ScreensReadDto>(cacheKey);
        if (cached != null) return cached;

        var screen = _screensRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = filterScreensById.Id,
            Key = "Equal",
            Includes = filterScreensById.Includes
        });

        var dto = _screensFactory.MapToScreenReadDto(screen);
        _cacheService.Set(cacheKey, dto, TimeSpan.FromMinutes(10));
        return dto;
    }

    public FilterReturn<ScreensReadDto> GetFilter(FilterScreensTable filter)
    {
        string cacheKey = $"Screens:Filter:{filter.GetHashCode()}";
        var cached = _cacheService.Get<FilterReturn<ScreensReadDto>>(cacheKey);
        if (cached != null) return cached;

        var filterResult = _screensRepository.GetFilter(filter);
        var result = new FilterReturn<ScreensReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = filterResult.ItensList.Select(_screensFactory.MapToScreenReadDto)
        };

        _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
        return result;
    }

    public ScreensUpdateDto Add(ScreensCreateDto screensCreateDto)
    {
        ScreensUpdateDto screensUpdateDto = null;
        if (!_screensSpecification.IsSatisfiedBy(screensCreateDto)) return screensUpdateDto;
        if (!_screensRepository.ValidateInput(screensCreateDto, false)) return screensUpdateDto;

        var screen = _screensFactory.MapToScreen(screensCreateDto);
        screensUpdateDto = _screensFactory.MapToScreenUpdateDto(_screensRepository.Add(screen));
        return screensUpdateDto;
    }

    public void Update(ScreensUpdateDto screensUpdateDto)
    {
        if (!_screensSpecification.IsSatisfiedBy(screensUpdateDto)) return;

        var existingScreen = _screensRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = screensUpdateDto.Id,
            Key = "Equal"
        });

        if (!_screensRepository.ValidateInput(screensUpdateDto, true, existingScreen)) return;

        var screen = _screensFactory.MapToScreenFromUpdateDto(screensUpdateDto);
        _screensRepository.Update(screen);

        _cacheService.Remove($"Screens:Id:{screensUpdateDto.Id}");
        _cacheService.RemoveByPrefix("Screens:Filter:");
    }

    public void Delete(Guid id)
    {
        var existingScreen = _screensRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = id,
            Key = "Equal"
        });

        if (existingScreen is null) return;

        _screensRepository.Delete(existingScreen);

        _cacheService.Remove($"Screens:Id:{id}");
        _cacheService.RemoveByPrefix("Screens:Filter:");
    }
}