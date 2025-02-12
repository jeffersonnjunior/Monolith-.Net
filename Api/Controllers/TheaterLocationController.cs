using Application.Dtos;
using Application.Interfaces.IServices;
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
    [Route("add")]
    public IActionResult Post([FromBody] TheaterLocationDto theaterLocationDto)
    {
        _theaterLocationService.Add(theaterLocationDto);
        return Ok();
    }

    [HttpPut]
    [Route("update")]
    public IActionResult Update(TheaterLocationDto theaterLocationDto)
    {
        _theaterLocationService.Update(theaterLocationDto);
        return Ok();
    }

    [HttpDelete]
    [Route("delete")]
    public IActionResult Delete(Guid id)
    {
        _theaterLocationService.Delete(id);
        return Ok();
    }
}
