using BIT.Attributes;
using BIT.DataStuff;
using BIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace BIT.Controllers
{
    [Route("Category")]
    [CustomAuthorize]
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly AppDbContext _context;
        private const int DispalayedAmount = 12;

        public CategoryController(ILogger<CategoryController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Chose()
        {
            return View();
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Category(string name, int pageNumber = 1)
        {
            var categoryDishes = _context.Dishes.Where(d => d.Category == name);

            if (!categoryDishes.Any())
            {
                return NotFound(); // Повертаємо 404, якщо категорія не знайдена
            }

            var paginatedList = await PaginatedList<Dish>.CreateAsync(categoryDishes, pageNumber, DispalayedAmount);

            return View(paginatedList);
        }

    }
}
