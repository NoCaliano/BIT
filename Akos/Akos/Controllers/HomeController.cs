using Akos.DataStuff;
using Akos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static Azure.Core.HttpHeader;

namespace Akos.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AppDbContext _dbContext;
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _dbContext = context;
        }


        public IActionResult Details(int id)
        {
            var dish = _dbContext.dishes.FirstOrDefault(d => d.Id == id);
            if (dish == null)
            {
                return NotFound(); // Повернути 404, якщо страва не знайдена
            }
            return View(dish);
        }

        public IActionResult Order()
        {
            return View();
        }

        public IActionResult CartDetail()
        {
            return View();
        }

        public IActionResult CartOrder()
        {
            return View();

        }

        [HttpPost]
        public IActionResult AddToCart(int dishId)
        {
            var dish = _dbContext.dishes.FirstOrDefault(d => d.Id == dishId);

            if (dish != null)
            {
                var userId = User.Identity.Name;
                var cart = _dbContext.Carts.Include(c => c.Products).FirstOrDefault(c => c.UserId == userId);

                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = userId,
                        Products = new List<Dish> { dish }
                    };

                    _dbContext.Carts.Add(cart);
                }
                else
                {
                    cart.Products.Add(dish);
                }

                _dbContext.SaveChanges();

                TempData["SuccessMessage"] = "Страва додана до корзини.";
                return RedirectToAction("CartOrder", "Cart");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public IActionResult Cart(int CartId)
        {
            var cart = _dbContext.Carts.FirstOrDefault(d => d.Id == CartId);

            if (cart != null)
            {
                List<Order> orders = new List<Order>();
                string customerName = User.Identity.Name;

                foreach (var item in cart.Products)
                {
                    Order newOrder = new Order
                    {
                        CustomerName = customerName,
                        OrderDate = DateTime.Now,
                        TotalAmount = (int)Math.Round(item.Price),
                        Products = new List<Dish> { item },
                        Product = item.Name,
                        Category = item.Category,
                        Status = "Нове",
                    };

                    orders.Add(newOrder);
                }

                return View("Cart", orders);
            }
            else
            {
                // Обробка випадку, коли корзина не знайдена
                return RedirectToAction("Dashboard", "Admin");
            }
        }

        [HttpPost]
        public IActionResult ProcessCart(List<Order> orders)
        {
            if (ModelState.IsValid /* || (!ModelState.IsValid && orders.All(order => order.Notes == null))*/)
            {
                foreach (var order in orders)
                {
                    if (order.Notes == null)
                    {
                        order.Notes = "";
                    }

                    _dbContext.orders.Add(order);
                }

                _dbContext.SaveChanges();

                return View("OrderConfirmation", orders);
            }

            return View("Orders", orders);
        }



        [HttpGet]
        public IActionResult OrderCart(int CartId)
        {
            var cart = _dbContext.Carts.FirstOrDefault(d => d.Id == CartId);
            
            foreach(var dish in cart.Products)
            {
                Order order = new Order()
                {
                    OrderDate = DateTime.Now,
                    Product = dish.Name,
                    CustomerName = "Test",
                    Category = dish.Category,
                    Notes = " ",
                    ShippingAddress = " ",
                    PaymentMethod = "VSCODE",
                    TotalAmount = (int)Math.Round(dish.Price),
                    Status = "SUCK",
                };
                _dbContext.orders.Add(order);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }





        /*
        [HttpGet]
        public IActionResult OrderDetails(int dishId)
        {
            var dish = _dbContext.dishes.FirstOrDefault(d => d.Id == dishId);

            if (dish != null)
            {
                string customerName = User.Identity.Name;
                Order newOrder = new Order
                {

                    CustomerName = customerName,
                    OrderDate = DateTime.Now,
                    TotalAmount = (int)Math.Round(dish.Price),
                    Products = new List<Dish> { dish },
                    Product = dish.Name,
                    Category = dish.Category,
                    Status = "Нове",
                };


                return View("Order", newOrder);
            }
            else
            {
                // Обробка випадку, коли страва не знайдена
                return RedirectToAction("Dashboard", "Admin");
            }
        }
        */

        [HttpPost]
        public IActionResult ProcessOrder(Order order)
        {
            if (ModelState.IsValid || (!ModelState.IsValid && order.Notes == null))
            {
                if(order.Notes == null)
                {
                    order.Notes = "";
                }
                
                _dbContext.orders.Add(order);
                _dbContext.SaveChanges();

                return View("OrderConfirmation", order);
            }
            _dbContext.SaveChanges();

            return View("Orders", order); // Ви можете вибрати відповідний вид у випадку недійсних даних
        }

        public IActionResult OrderConfirmation(int orderId)
        {
            var order = _dbContext.orders.FirstOrDefault(o => o.Id == orderId);

            if (order != null)
            {
                return View(order);
            }
            else
            {
                // Обробка випадку, коли замовлення не знайдено
                return RedirectToAction("Index", "Home");
            }
        }



        public IActionResult OrderConfirmation()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}