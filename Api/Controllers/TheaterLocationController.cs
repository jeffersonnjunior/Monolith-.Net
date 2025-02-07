using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Produces("application/json")]
[Route("api/TheaterLocation")]

public class TheaterLocationController : Controller
{
    private readonly ITheaterLocationService _theaterLocationService;
    public TheaterLocationController(ITheaterLocationService theaterLocationService)
    {
        _theaterLocationService = theaterLocationService;
    }

    [HttpPost]
    
    public IActionResult Post([FromBody] TheaterLocationDto theaterLocationDto)
    {
        _theaterLocationService.Add(theaterLocationDto);
        return Ok();
    }
}
