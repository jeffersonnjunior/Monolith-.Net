using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class FilmsController : Controller
{
    private readonly IFilmsService _filmService;

    public FilmsController(IFilmsService filmService)
    {
        _filmService = filmService;
    }
}
