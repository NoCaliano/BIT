using BIT.Areas.Identity.Data;
using BIT.DataStuff;
using BIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BIT.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ILogger<HomeController> logger, AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Thanks()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PartTest(int dishId)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);
            return PartialView("testpart", dish);
        }


        //Метод треба буде ще доробити
        [HttpPost]
        public async Task<IActionResult> OrderDone(int dishId, string Address, string Phone, string Payment, string Notes) 
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (dish != null)
            {
                Order order = new Order() 
                {
                    UserId = userId,
                    Phonenumber = Phone,
                    ShippingAddress = Address,
                    CustomerName = user.Email,
                    PaymentMethod = Payment,
                    OrderDate = DateTime.Now,
                    TotalAmount = dish.Price,
                    Product = new List<Dish> { dish },
                    Quanity = 1,
                    Notes = Notes,
                    ProductName = dish.Name,
                    Category = dish.Category,
                    Status = "New",
                    Courier = GetReadyToWorkCourierNames()[0].Name,
                    CourId = GetReadyToWorkCourierNames()[0].Id.ToString(),
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Thanks", "Order");
            }
            else
            {
                return NotFound();
            }
        }


        public List<Courier> GetReadyToWorkCourierNames()
        {
            // Отримуємо готових до роботи кур'єрів
            var readyToWorkCouriers = _context.Couriers
                .Where(c => c.ReadyToWork == true)
                .ToList();

            // Фільтруємо кур'єрів, у яких не більше 1 активного замовлення ("In Process")
            var couriersWithFewerOrders = readyToWorkCouriers
                .Where(courier =>
                {
                    var activeOrdersCount = _context.Orders
                        .Count(order => order.CourId == courier.Id.ToString() && order.Status == "InProcess");

                    return activeOrdersCount < 2;
                })
                .ToList();

            return couriersWithFewerOrders;
        }



    }
}