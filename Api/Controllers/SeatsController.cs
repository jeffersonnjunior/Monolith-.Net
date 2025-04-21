using Application.Dtos;
using Application.Interfaces.IServices;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class SeatsController : Controller
{
    private readonly ISeatsService _seatsService;
    private readonly NotificationContext _notificationContext;
    public SeatsController(ISeatsService seatsService, NotificationContext notificationContext)
    {
        _seatsService = seatsService;
        _notificationContext = notificationContext;
    }
    
    [HttpGet]
    [Route("get-by-id")]
    public IActionResult GetById([FromQuery] FilterSeatsById filterSeatsById)
    {
        return Ok(_seatsService.GetById(filterSeatsById));
    }
    
    [HttpGet]
    [Route("get-filter")]
    public IActionResult GetFilter([FromQuery] FilterSeatsTable filter)
    {
        return Ok(_seatsService.GetFilter(filter));
    }
    
    [HttpPost]
    [Route("add")]
    public IActionResult Add([FromBody] SeatsCreateDto seatsCreateDto)
    {
        var seats = _seatsService.Add(seatsCreateDto);
        
        if (_notificationContext.HasNotifications()) return BadRequest(new { errors = _notificationContext.GetNotifications() });
        
        var uri = Url.Action(nameof(GetById), new { id = seats.Id });
        return Created(uri, seats);
    }
    
    [HttpPut]
    [Route("update")]
    public IActionResult Update([FromBody] SeatsUpdateDto seatsUpdateDto)
    {
        _seatsService.Update(seatsUpdateDto);
        
        return Ok();
    }
    
    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(Guid id)
    {
        _seatsService.Delete(id);
        return Ok();
    }
}