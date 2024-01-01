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

        public IActionResult Orders()
        {
            var orders = _context.Orders.Where(o => o.Status == OrderStatus.New && o.CourId == null).ToList();
            return View(orders);
        }

        public IActionResult MyDeliveries()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cour = _context.Couriers.FirstOrDefault(c => c.UserId == userId);

            if (cour != null)
            {
                var orders = _context.Orders.Where(o => o.CourId == cour.Id.ToString()).ToList();

                return View(orders);
            }
            return NotFound();
        }

        public IActionResult Info()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cour = _context.Couriers.FirstOrDefault(c => c.UserId == userId);
            return View(cour);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int ordId, OrderStatus newStatus)
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
                    if (upord.Status == OrderStatus.Delivered)
                    {
                        var deliveredCount = _context.Orders.Count(o =>o.CourId == cour.Id.ToString() && o.Status == OrderStatus.Delivered);

                        cour.Delievered = deliveredCount;
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    return NotFound();
                }
                return RedirectToAction("MyDeliveries", "Courier");

            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> OnWork()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cour = _context.Couriers.FirstOrDefault(c => c.UserId == userId);

            if (cour != null)
            {
                cour.ReadyToWork = true;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Courier");
            }

            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> EndWork()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cour = _context.Couriers.FirstOrDefault(c => c.UserId == userId);

            if (cour != null)
            {
                cour.ReadyToWork = false;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Courier");
            }

            return NotFound();
        }

        /* [HttpPost]
        public async Task<IActionResult> TakeOrder(int ordId)
        {
            var ord = _context.Orders.FirstOrDefault(o => o.Id == ordId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cour = _context.Couriers.FirstOrDefault(c => c.UserId == userId);
            if(ord != null && ord.CourId == null)
            {
                ord.Courier = cour.Name;
                ord.CourId = cour.Id.ToString();
                await _context.SaveChangesAsync();
                return RedirectToAction("MyDeliveries", "Courier");
            }
            return NotFound();

        } */


        [HttpPost]
        public async Task<IActionResult> TakeOrder(int ordId)
        {
            var orderToTake = _context.Orders.FirstOrDefault(o => o.Id == ordId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var courier = _context.Couriers.FirstOrDefault(c => c.UserId == userId);

            if (orderToTake != null && orderToTake.CourId == null)
            {
                // Отримайте всі замовлення з такою ж самою адресою
                var ordersWithSameAddress = _context.Orders
                    .Where(o => o.ShippingAddress == orderToTake.ShippingAddress && o.CourId == null)
                    .ToList();

                foreach (var order in ordersWithSameAddress)
                {
                    order.Courier = courier.Name;
                    order.CourId = courier.Id.ToString();
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("MyDeliveries", "Courier");
            }

            return NotFound();
        }



    }
}
