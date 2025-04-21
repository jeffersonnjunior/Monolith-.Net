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

public class TicketsController : Controller
{
    private readonly ITicketsService _ticketsService;
    private readonly NotificationContext _notificationContext;

    public TicketsController(ITicketsService ticketsService, NotificationContext notificationContext)
    {
        _ticketsService = ticketsService;
        _notificationContext = notificationContext;
    }
    
    [HttpGet]
    [Route("get-by-id")]
    public IActionResult GetById(FilterTicketsById filterTicketsById)
    {
        return Ok(_ticketsService.GetById(filterTicketsById));
    }
    
    [HttpGet]
    [Route("get-filter")]
    public IActionResult GetFilter([FromQuery] FilterTicketsTable filter)
    {
        return Ok(_ticketsService.GetFilter(filter));
    }
    
    [HttpPost]
    [Route("add")]
    public IActionResult Add([FromBody] TicketsCreateDto ticketsCreateDto)
    {
        var tickets = _ticketsService.Add(ticketsCreateDto);
        
        if (_notificationContext.HasNotifications()) return BadRequest(new { errors = _notificationContext.GetNotifications() });
        
        var uri = Url.Action(nameof(GetById), new { id = tickets.Id });
        return Created(uri, tickets);
    }
    
    [HttpPut]
    [Route("update")]
    public IActionResult Update([FromBody] TicketsUpdateDto ticketsUpdateDto)
    {
        _ticketsService.Update(ticketsUpdateDto);
        return Ok();
    }
    
    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(Guid id)
    {
        _ticketsService.Delete(id);
        return Ok();
    }
}
