using BIT.DataStuff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BIT.Controllers
{
    [Authorize]
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


        [HttpPost]
        public async Task<IActionResult> CancelOrder(int ordId)
        {
            var ord = _context.Orders.FirstOrDefault(o => o.Id == ordId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ord != null && ord.UserId == userId)
            {
                ord.Status = "Cancelled";
                await _context.SaveChangesAsync();
                return RedirectToAction("MyOrders", "Account");
            }

            return NotFound();
        }


    }
}
