using Microsoft.AspNetCore.Mvc;

namespace SignalRChatProject.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
