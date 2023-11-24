using BIT.Areas.Identity.Data;
using BIT.DataStuff;
using BIT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BIT.Controllers
{
    
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

        [HttpPost]
        public IActionResult OrderDetails(Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Orders.Add(order);
                _context.SaveChanges();

                return RedirectToAction("Thanks");
            }
            _context.SaveChanges();

            return View("Privacy", order); // Ви можете вибрати відповідний вид у випадку недійсних даних
        }


        [HttpGet]
        public IActionResult Order(int dishId)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);

            
            if (dish != null)
            {
                string userId = null;

                if (User.Identity.IsAuthenticated)
                {                    
                    userId = _userManager.GetUserId(User);
                }

                string customerName = "Guest";
                if (User.Identity.Name != null)
                {
                    customerName = User.Identity.Name;

                }

                Order newOrder = new Order
                {
                    UserId = userId,
                    CustomerName = customerName,
                    OrderDate = DateTime.Now,
                    TotalAmount = dish.Price,
                    Product = new List<Dish> { dish },
                    Quanity = 1,
                    ProductName = dish.Name,
                    Category = dish.Category,
                    Status = "New",
                    Courier = GetReadyToWorkCourierNames()[0],
                };


                return View("Order", newOrder);
            }

            else
            {
                // Обробка випадку, коли страва не знайдена
                return RedirectToAction("Dashboard", "Admin");
            }
        }


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
