using BIT.DataStuff;
using BIT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BIT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        private static readonly Cart cart = new Cart();
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Details(int id)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.Id == id);
            if (dish == null)
            {
                return NotFound(); 
            }
            return View(dish);
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