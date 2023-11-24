using BIT.DataStuff;
using Microsoft.AspNetCore.Mvc;

namespace BIT.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult MyOrders() { return View(); }  

    }
}
