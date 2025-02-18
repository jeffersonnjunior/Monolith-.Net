using Application.Dtos;
using Application.Interfaces.IServices;
using Infrastructure.Utilities.FiltersModel;
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

    [HttpGet]
    [Route("getById")]
    public IActionResult GetById(Guid id)
    {
        return Ok(_theaterLocationService.GetById(id));
    }

    [HttpPost]
    [Route("add")]
    public IActionResult Post([FromBody] TheaterLocationDto theaterLocationDto)
    {
        var createdLocation = _theaterLocationService.Add(theaterLocationDto);
        var uri = Url.Action(nameof(GetById), new { id = createdLocation.Id });

        return Created(uri, createdLocation);
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

    [HttpGet]
    [Route("filter")]
    public IActionResult GetByFilter([FromQuery] TheaterLocationFilter filter, [FromQuery] string[] includes)
    {
        var result = _theaterLocationService.GetFilter(filter, includes);
        return Ok(result);
    }
}