using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class MovieTheatersController : Controller
{
    private readonly IMovieTheatersService _movieTheatersService;
    public MovieTheatersController(IMovieTheatersService movieTheatersService)
    {
        _movieTheatersService = movieTheatersService;
    }
}
