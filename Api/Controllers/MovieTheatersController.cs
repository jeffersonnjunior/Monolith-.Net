using Application.Dtos;
using Application.Interfaces.IServices;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Produces("application/json")]
[Route("api/MovieTheaters")]

public class MovieTheatersController : Controller
{
    private readonly IMovieTheatersService _movieTheatersService;
    private readonly NotificationContext _notificationContext;

    public MovieTheatersController(IMovieTheatersService movieTheatersService, NotificationContext notificationContext)
    {
        _movieTheatersService = movieTheatersService;
        _notificationContext = notificationContext;
    }
    
    [HttpGet]
    [Route("get-by-id")]
    public IActionResult GetById([FromQuery] FilterMovieTheatersById filterMovieTheatersById)
    {
        return Ok(_movieTheatersService.GetById(filterMovieTheatersById));
    }
    
    [HttpGet]
    [Route("get-filter")]
    public IActionResult GetFilter([FromQuery] FilterMovieTheatersTable filter)
    {
        return Ok(_movieTheatersService.GetFilter(filter));
    }

    [HttpPost]
    [Route("add")]
    public IActionResult Post([FromBody] MovieTheatersCreateDto movieTheatersCreateDto)
    {
        var createdTheaterLocation = _movieTheatersService.Add(movieTheatersCreateDto);
        
        if (_notificationContext.HasNotifications()) return BadRequest(new { errors = _notificationContext.GetNotifications() });
        
        var uri = Url.Action(nameof(GetById), new { id = createdTheaterLocation.Id });
        return Created(uri, createdTheaterLocation);
    }
    
    [HttpPut]
    [Route("update")]
    public IActionResult Update([FromBody] MovieTheatersUpdateDto movieTheatersUpdateDto)
    {
        _movieTheatersService.Update(movieTheatersUpdateDto);
        return Ok();
    }
    
    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(Guid id)
    {
        _movieTheatersService.Delete(id);
        return Ok();
    }
}