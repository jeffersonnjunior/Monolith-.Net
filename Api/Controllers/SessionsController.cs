using Application.Dtos;
using Application.Interfaces.IServices;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Produces("application/json")]
[Route("api/Sessions")]
public class SessionsController : Controller
{
    private readonly ISessionsService _sessionsService;
    private readonly NotificationContext _notificationContext;

    public SessionsController(ISessionsService sessionsService, NotificationContext notificationContext)
    {
        _sessionsService = sessionsService;
        _notificationContext = notificationContext;
    }

    [HttpGet]
    [Route("get-by-id")]
    public IActionResult GetById([FromQuery] FilterSessionsById filterSessionsById)
    {
        return Ok(_sessionsService.GetById(filterSessionsById));
    }

    [HttpGet]
    [Route("get-filter")]
    public IActionResult GetFilter([FromQuery] FilterSessionsTable filterSessionsTable)
    {
        return Ok(_sessionsService.GetFilter(filterSessionsTable));
    }
    
    [HttpPost]
    [Route("add")]
    public IActionResult Add([FromBody] SessionsCreateDto sessionsCreateDto)
    {
        var createdSession = _sessionsService.Add(sessionsCreateDto);

        if (_notificationContext.HasNotifications()) return BadRequest(new { errors = _notificationContext.GetNotifications() });

        var uri = Url.Action(nameof(GetById), new { id = createdSession.Id });
        return Created(uri, createdSession);
    }
    
    [HttpPut]
    [Route("update")]
    public IActionResult Update([FromBody] SessionsUpdateDto sessionsUpdateDto)
    {
        _sessionsService.Update(sessionsUpdateDto);
        
        return Ok();
    }
    
    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(Guid id)
    {
        _sessionsService.Delete(id);
        
        return Ok();
    }
}