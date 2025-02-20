using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Produces("application/json")]
[Route("api/MovieTheaters")]

public class MovieTheatersController : Controller
{
    private readonly IMovieTheatersService _movieTheatersService;
    public MovieTheatersController(IMovieTheatersService movieTheatersService)
    {
        _movieTheatersService = movieTheatersService;
    }

    //[HttpPost]
    //[Route("add")]

    //public IActionResult Post()
    //{
    //    _movieTheatersService.Add();
    //    return Ok();
    //}
}
