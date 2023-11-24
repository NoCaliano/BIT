using Microsoft.AspNetCore.Mvc;

namespace BIT.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }
    }
}
