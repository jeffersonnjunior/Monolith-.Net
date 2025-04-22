using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces.IFactories;

public interface IScreensFactory
{
    Screens CreateScreen();
    ScreensCreateDto CreateScreenCreateDto();
    ScreensReadDto CreateScreenReadDto();
    ScreensUpdateDto CreateScreenUpdateDto();
    Screens MapToScreen(ScreensCreateDto dto);
    ScreensCreateDto MapToScreenCreateDto(Screens entity);
    ScreensReadDto MapToScreenReadDto(Screens entity);
    ScreensUpdateDto MapToScreenUpdateDto(Screens entity);
    Screens MapToScreenFromUpdateDto(ScreensUpdateDto dto);
}