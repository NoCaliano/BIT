using BIT.Areas.Identity.Data;
using BIT.DataStuff;
using BIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;
using System.Collections.Specialized;
using System.Security.Claims;
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

        public IActionResult Info()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int ordId, string newStatus)
        {
            var ord = _context.Orders.FirstOrDefault(o => o.Id == ordId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cour = _context.Couriers.FirstOrDefault(c => c.UserId == userId);

            if (ord != null)
            {
                ord.Status = newStatus;
                await _context.SaveChangesAsync();
                if (cour != null)
                {
                    var upord = _context.Orders.FirstOrDefault(o => o.Id == ordId);
                    if (upord.Status == "Delivered")
                    {
                        cour.Delievered = cour.Delievered + 1 ;
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    return NotFound();
                }
                return RedirectToAction("Index", "Courier");

            }
            else
            {
                return NotFound();
            }
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


        [HttpPost]
        public async Task<IActionResult> EndWork(string CourUserId)
        {
            var cour = _context.Couriers.FirstOrDefault(o => o.UserId == CourUserId);

            if (cour != null)
            {
                cour.ReadyToWork = false;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Courier");
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> TakeOrder(int ordId)
        {
            var ord = _context.Orders.FirstOrDefault(o => o.Id == ordId);

            if (ord != null)
            {
                ord.Courier = GetReadyToWorkCourierNames().Last();
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Courier");
            }

            return NotFound();
        }

        /*
        [HttpPost]
        public async Task<IActionResult> ChangeVeh(int userId)
        {
            var ord = _context.Orders.FirstOrDefault(o => o.Id == ordId);

            if (ord != null)
            {
                ord.Courier = GetReadyToWorkCourierNames().Last();
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Courier");
            }

            return NotFound();
        }
        */

        public List<string> GetReadyToWorkCourierNames()
        {
            // Фільтруємо кур'єрів за умовою ReadyToWork == true і вибираємо їх імена
            var readyToWorkCouriers = _context.Couriers
                .Where(c => c.ReadyToWork == true)
                .Select(c => c.Name)
                .ToList();

            return readyToWorkCouriers;
        }


    }
}
