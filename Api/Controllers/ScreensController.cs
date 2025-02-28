using Application.Dtos;
using Application.Interfaces.IServices;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Produces("application/json")]
[Route("api/Screens")]
public class ScreensController : Controller
{
    private readonly IScreensService _screensService;
    private readonly NotificationContext _notificationContext;
    public ScreensController(IScreensService screensService, NotificationContext notificationContext)
    {
        _screensService = screensService;
        _notificationContext = notificationContext;
    }

    [HttpGet]
    [Route("get-by-id")]
    public IActionResult GetById(FilterScreensById filterScreensById)
    {
        return Ok(_screensService.GetById(filterScreensById));
    }
    
    [HttpGet]
    [Route("get-filter")]
    public IActionResult GetFilter([FromQuery] FilterScreensTable filter)
    {
        return Ok(_screensService.GetFilter(filter));
    }
    
    [HttpPost]
    [Route("add")]
    public IActionResult Add(ScreensCreateDto screensCreateDto)
    {
        var createdScreens = _screensService.Add(screensCreateDto);
        
        if (_notificationContext.HasNotifications()) return BadRequest(new { errors = _notificationContext.GetNotifications() });
        
        var uri = Url.Action(nameof(GetById), new { id = createdScreens.Id });
        return Created(uri, createdScreens);
    }

    [HttpPost]
    [Route("update")]
    public IActionResult Update(ScreensUpdateDto screensUpdateDto)
    {
        _screensService.Update(screensUpdateDto);
        return Ok();
    }
    
    [HttpDelete]
    [Route("delete")]
    public IActionResult Delete(Guid id)
    {
        _screensService.Delete(id);
        return Ok();
    }
}
