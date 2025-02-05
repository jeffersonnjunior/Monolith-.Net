using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class TicketsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
