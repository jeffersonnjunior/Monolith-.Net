using Application.Dtos;
using Application.Interfaces.IServices;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Produces("application/json")]
[Route("api/TheaterLocation")]
public class TheaterLocationController : Controller
{
    private readonly ITheaterLocationService _theaterLocationService;
    private readonly NotificationContext _notificationContext;

    public TheaterLocationController(ITheaterLocationService theaterLocationService, NotificationContext notificationContext)
    {
        _theaterLocationService = theaterLocationService;
        _notificationContext = notificationContext;
    }

    [HttpGet]
    [Route("get-by-id")]
    public IActionResult GetById(Guid id)
    {
        return Ok(_theaterLocationService.GetById(id));
    }

    [HttpGet]
    [Route("get-filter")]
    public IActionResult GetFilter([FromQuery] FilterTheaterLocation filter, [FromQuery] string[] includes)
    {
        var result = _theaterLocationService.GetFilter(filter, includes);
        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public IActionResult Post([FromBody] TheaterLocationCreateDto theaterLocationCreateDto)
    {
        var createdLocation = _theaterLocationService.Add(theaterLocationCreateDto);

        if (_notificationContext.HasNotifications()) return BadRequest(new { errors = _notificationContext.GetNotifications() });
        
        var uri = Url.Action(nameof(GetById), new { id = createdLocation.Id });
        return Created(uri, createdLocation);
    }

    [HttpPut]
    [Route("update")]
    public IActionResult Update(TheaterLocationUpdateDto theaterLocationUpdateDto)
    {
        _theaterLocationService.Update(theaterLocationUpdateDto);
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