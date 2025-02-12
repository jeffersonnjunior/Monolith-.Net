using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class TicketsController : Controller
{
    private readonly ITicketsService _ticketsService;

    public TicketsController(ITicketsService ticketsService)
    {
        _ticketsService = ticketsService;
    }
}
