using Application.Dtos;
using Application.Interfaces.IServices;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Produces("application/json")]
[Route("api/Films")]
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
        var film = _filmService.GetById(filterFilmsById);
        
        return Ok(film);
    }
    
    [HttpGet]
    [Route("get-filter")]
    public IActionResult GetFilter(FilterFilmsTable filter)
    {
        var films = _filmService.GetFilter(filter);
        
        return Ok(films);
    }
    
    [HttpPost]
    [Route("add")]
    public IActionResult Post([FromBody] FilmsCreateDto filmsCreateDto)
    {
        var createdFilm = _filmService.Add(filmsCreateDto);
        
        if (_notificationContext.HasNotifications()) return BadRequest(new { errors = _notificationContext.GetNotifications() });
        
        var uri = Url.Action(nameof(GetById), new { id = createdFilm.Id });
        return Created(uri, createdFilm);
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
