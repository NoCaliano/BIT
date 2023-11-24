using BIT.Areas.Identity.Data;
using BIT.DataStuff;
using BIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace BIT.Controllers
{
    [Authorize(Roles = "Admin, Courier")]
    public class CourierController : Controller
    {
        private readonly ILogger<CourierController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public CourierController(ILogger<CourierController> logger, AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int ordId, string newStatus)
        {
            var ord = _context.Orders.FirstOrDefault(o => o.Id == ordId);

            if (ord != null)
            {
                ord.Status = newStatus;
                await _context.SaveChangesAsync();
                return RedirectToAction("index");
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> OnWork(string CourUserId)
        {
            var cour = _context.Couriers.FirstOrDefault(o => o.UserId == CourUserId);

            if (cour != null)
            {
                cour.ReadyToWork = true;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Courier");
            }

            return NotFound();
        }

    }
}
