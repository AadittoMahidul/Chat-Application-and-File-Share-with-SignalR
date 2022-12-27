using Microsoft.AspNetCore.Mvc;

namespace SignalRChatProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
