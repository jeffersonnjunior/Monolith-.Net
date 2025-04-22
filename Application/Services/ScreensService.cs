using Application.Dtos;
using Application.Interfaces.IFactories;
using Application.Interfaces.IServices;
using Application.Specification;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;

namespace Application.Services;

public class ScreensService : IScreensService
{
    private readonly IScreensRepository _screensRepository;
    private readonly NotificationContext _notificationContext;
    private readonly ScreensSpecification _screensSpecification;
    private readonly IScreensFactory _screensFactory;

    public ScreensService(
        IScreensRepository screensRepository,
        NotificationContext notificationContext,
        IScreensFactory screensFactory)
    {
        _screensRepository = screensRepository;
        _notificationContext = notificationContext;
        _screensSpecification = new ScreensSpecification(notificationContext);
        _screensFactory = screensFactory;
    }

    public ScreensReadDto GetById(FilterScreensById filterScreensById)
    {
        var screen = _screensRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = filterScreensById.Id,
            Key = "Equal",
            Includes = filterScreensById.Includes
        });

        return _screensFactory.MapToScreenReadDto(screen);
    }

    public FilterReturn<ScreensReadDto> GetFilter(FilterScreensTable filter)
    {
        var filterResult = _screensRepository.GetFilter(filter);
        return new FilterReturn<ScreensReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = filterResult.ItensList.Select(_screensFactory.MapToScreenReadDto)
        };
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
    }
}