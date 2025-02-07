using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class SeatsController : Controller
{
    private readonly ISeatsService _seatsService;
    public SeatsController(ISeatsService seatsService)
    {
        _seatsService = seatsService;
    }
}
