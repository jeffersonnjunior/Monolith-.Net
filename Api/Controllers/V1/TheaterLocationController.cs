using Application.Dtos;
using Application.Interfaces.IServices;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
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
    public IActionResult GetById([FromQuery] FilterTheaterLocationById filterTheaterLocationById)
    {
        return Ok(_theaterLocationService.GetById(filterTheaterLocationById));
    }

    [HttpGet]
    [Route("get-filter")]
    public IActionResult GetFilter([FromQuery] FilterTheaterLocationTable filter)
    {
        return Ok(_theaterLocationService.GetFilter(filter));
    }

    [HttpPost]
    [Route("add")]
    public IActionResult Post([FromBody] TheaterLocationCreateDto theaterLocationCreateDto)
    {
        var theaterLocation = _theaterLocationService.Add(theaterLocationCreateDto);

        if (_notificationContext.HasNotifications()) return BadRequest(new { errors = _notificationContext.GetNotifications() });
        
        var uri = Url.Action(nameof(GetById), new { id = theaterLocation.Id });
        return Created(uri, theaterLocation);
    }

    [HttpPut]
    [Route("update")]
    public IActionResult Update([FromBody] TheaterLocationUpdateDto theaterLocationUpdateDto)
    {
        _theaterLocationService.Update(theaterLocationUpdateDto);
        return Ok();
    }
    
    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(Guid id)
    {
        _theaterLocationService.Delete(id);
        return Ok();
    }
}