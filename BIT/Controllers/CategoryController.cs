using BIT.DataStuff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BIT.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly AppDbContext _context;


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

        public IActionResult Donuts()
        {
            return View();
        }

        public IActionResult Cakes()
        {
            return View();
        }

        public IActionResult Pizza()
        {
            return View();
        }

        public IActionResult Sushi()
        {
            return View();
        }
    }
}
