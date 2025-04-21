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
public class FilmsController : Controller
{
    private readonly IFilmsService _filmService;
    private readonly NotificationContext _notificationContext;


    public FilmsController(IFilmsService filmService, NotificationContext notificationContext)
    {
        _filmService = filmService;
        _notificationContext = notificationContext;

    }
    
    [HttpGet]
    [Route("get-by-id")]
    public IActionResult GetById(FilterFilmsById filterFilmsById)
    {
        return Ok(_filmService.GetById(filterFilmsById));
    }
    
    [HttpGet]
    [Route("get-filter")]
    public IActionResult GetFilter(FilterFilmsTable filter)
    {
        return Ok(_filmService.GetFilter(filter));
    }
    
    [HttpPost]
    [Route("add")]
    public IActionResult Post([FromBody] FilmsCreateDto filmsCreateDto)
    {
        var film = _filmService.Add(filmsCreateDto);
        
        if (_notificationContext.HasNotifications()) return BadRequest(new { errors = _notificationContext.GetNotifications() });
        
        var uri = Url.Action(nameof(GetById), new { id = film.Id });
        return Created(uri, film);
    }
    
    [HttpPut]
    [Route("update")]
    public IActionResult Update([FromBody] FilmsUpdateDto filmsUpdateDto)
    {
        _filmService.Update(filmsUpdateDto);
        return Ok();
    }
    
    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(Guid id)
    {
        _filmService.Delete(id);
        return Ok();
    }
}
