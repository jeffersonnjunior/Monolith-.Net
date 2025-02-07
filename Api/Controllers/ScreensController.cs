using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class ScreensController : Controller
{
    private readonly IScreensService _screensService;
    public ScreensController(IScreensService screensService)
    {
        _screensService = screensService;
    }
}
