﻿using BIT.Areas.Identity.Data;
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

        [HttpPost]
        public IActionResult OrderDetails(Order order)
        {
            if (ModelState.IsValid)
            {
 
                _context.Orders.Add(order);         
                _context.SaveChanges();                  
                return RedirectToAction("Thanks");                
            }

            return View("Order", order);
        }




        [HttpGet]
        public async Task<IActionResult> Order(int dishId)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user =  await _userManager.FindByIdAsync(userId);
            string Address;
            string phone;
            string customerName;
            if (dish != null)
            {                

                if(user.Address != null) 
                { 
                    Address = user.Address; 
                }
                else
                {
                    Address = "Unluck";
                }
                if(user.PhoneNumber != null)
                {
                    phone = user.PhoneNumber;
                }
                else
                {
                    phone = "Nothing";
                }
                if(user.FirstName != null)
                {
                    customerName = user.FirstName;
                }
                else
                {
                    customerName = "Guest";
                }



                Order newOrder = new Order
                {
                    UserId = userId,
                    Phonenumber = phone,
                    ShippingAddress = Address,
                    CustomerName = customerName,
                    OrderDate = DateTime.Now,
                    TotalAmount = dish.Price,
                    Product = new List<Dish> { dish },
                    Quanity = 1,
                    ProductName = dish.Name,
                    Category = dish.Category,
                    Status = "New",
                    Courier = GetReadyToWorkCourierNames()[0].Name,
                    CourId = GetReadyToWorkCourierNames()[0].Id.ToString(),
                };


                return View("Order", newOrder);
            }

            else
            {
                // Обробка випадку, коли страва не знайдена
                return RedirectToAction("Dashboard", "Admin");
            }
        }

        public List<Courier> GetReadyToWorkCourierNames()
        {
            // Отримуємо готових до роботи кур'єрів
            var readyToWorkCouriers = _context.Couriers
                .Where(c => c.ReadyToWork == true)
                .ToList();

            // Фільтруємо кур'єрів, у яких не більше 5 активних замовлень ("In Process")
            var couriersWithFewerOrders = readyToWorkCouriers
                .Where(courier =>
                {
                    var activeOrdersCount = _context.Orders
                        .Count(order => order.CourId == courier.Id.ToString() && order.Status == "InProcess");

                    return activeOrdersCount <= 2;
                })
                .ToList();

            return couriersWithFewerOrders;
        }


    }
}
