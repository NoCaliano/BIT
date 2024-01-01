using BIT.DataStuff;
using BIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BIT.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly AppDbContext _context;
        private const int DispalayedAmount = 9;

        public CategoryController(ILogger<CategoryController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        [Route("Category")]
        public IActionResult Chose()
        {
            return View();
        }

        public async Task<IActionResult> Donuts(int pageNumber = 1)
        {
            var donutsDishes = _context.Dishes.Where(d => d.Category == "Donuts");
            var paginatedList = await PaginatedList<Dish>.CreateAsync(donutsDishes, pageNumber, DispalayedAmount);

            return View(paginatedList);
        }

        public async Task<IActionResult> Sets(int pageNumber = 1)
        {
            var setsDishes = _context.Dishes.Where(d => d.Category == "Sets");
            var paginatedList = await PaginatedList<Dish>.CreateAsync(setsDishes, pageNumber, DispalayedAmount);

            return View(paginatedList);
        }

        public async Task<IActionResult> Pizza(int pageNumber = 1)
        {
            var pizzaDishes = _context.Dishes.Where(d => d.Category == "Pizza");
            var paginatedList = await PaginatedList<Dish>.CreateAsync(pizzaDishes, pageNumber, DispalayedAmount);

            return View(paginatedList);
        }


        public async Task<IActionResult> Sushi(int pageNumber = 1)
        {
            var sushiDishes = _context.Dishes.Where(d => d.Category == "Sushi");
            var paginatedList = await PaginatedList<Dish>.CreateAsync(sushiDishes, pageNumber, DispalayedAmount);

            return View(paginatedList);
        }

    }
}
