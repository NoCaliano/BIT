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
                        cour.Delievered++;
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

        public async Task<IActionResult> RefuseOrder(int ordId)
        {
            var ord = _context.Orders.FirstOrDefault(o => o.Id == ordId);

            if (ord != null)
            {
                var readyToWorkCouriers = GetReadyToWorkCourierNames();

                if (readyToWorkCouriers.Count >= 2)
                {
                    var currentCourier = readyToWorkCouriers.First();
                    var nextCourier = readyToWorkCouriers.Skip(1).First();

                    ord.Courier = nextCourier.Name;
                    ord.CourId = nextCourier.Id.ToString();
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Courier");
                }
                else
                {
                    // Handle the case when there are not enough couriers to transfer the order
                    // You may want to log this event or handle it differently based on your requirements
                    return RedirectToAction("Index", "Courier");
                }
            }

            return NotFound();
        }



        [HttpPost]
        public async Task<IActionResult> ToNextCourier(int ordId)
        {
            var ord = _context.Orders.FirstOrDefault(o => o.Id == ordId);

            if (ord != null)
            {
                ord.Courier = GetReadyToWorkCourierNames()[GetReadyToWorkCourierCount()].Name;
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

        public int GetReadyToWorkCourierCount()
        {
            var count = _context.Couriers
                .Count(c => c.ReadyToWork == true && c.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier));

            return count;
        }

        public List<Courier> GetReadyToWorkCourierNames()
        {
            var cour = _context.Couriers.FirstOrDefault(o => o.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            var readyToWorkCouriers = _context.Couriers
                .Where(c => c.ReadyToWork == true && c.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                .ToList();

            if (cour != null)
            {
                readyToWorkCouriers.Add(cour);
            }
            return readyToWorkCouriers;
        }



    }
}
