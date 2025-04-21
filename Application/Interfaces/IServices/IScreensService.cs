using Application.Dtos;
using Infrastructure.FiltersModel;

namespace Application.Interfaces.IServices;

public interface IScreensService
{
    ScreensReadDto GetById(FilterScreensById filterScreensById);
    FilterReturn<ScreensReadDto> GetFilter(FilterScreensTable filter);
    ScreensUpdateDto Add(ScreensCreateDto screensCreateDto);
    void Update(ScreensUpdateDto screensUpdateDto);
    void Delete(Guid id);
}
