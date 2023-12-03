using Microsoft.AspNetCore.Mvc;

namespace Akos.Controllers
{
    public class WelcomeController: Controller
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
