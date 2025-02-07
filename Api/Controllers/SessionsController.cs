using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class SessionsController : Controller
{
    private readonly ISessionsService _sessionsService;
    public SessionsController(ISessionsService sessionsService)
    {
        _sessionsService = sessionsService;
    }
}
