using BIT.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BIT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static readonly Cart cart = new Cart();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public string GenerateRandomCategory()
        {
            var categories = new List<string> { "Pizza", "Donuts", "Торти" };
            var random = new Random();
            return categories[random.Next(categories.Count)];
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}