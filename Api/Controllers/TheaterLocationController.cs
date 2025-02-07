using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class TheaterLocationController : Controller
{
    private readonly ITheaterLocationService _theaterLocationService;
    public TheaterLocationController(ITheaterLocationService theaterLocationService)
    {
        _theaterLocationService = theaterLocationService;
    }
}
