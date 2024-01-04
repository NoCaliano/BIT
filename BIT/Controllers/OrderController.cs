﻿using BIT.Areas.Identity.Data;
using BIT.Attributes;
using BIT.DataStuff;
using BIT.Models;
using BIT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.Security.Claims;

namespace BIT.Controllers
{
    [CustomAuthorize]
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
        public async Task<IActionResult> PartTest(int dishId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);            
            if (dish != null && user != null)
            {
                OrderDetailsViewModel d = new OrderDetailsViewModel()
                {
                    DishId = dish.Id,
                    PhoneNumber = user.PhoneNumber,
                    ShippingAdrees = user.Address,
                    Img = dish.Img,
                    Total = dish.Price
                    
                };
                return PartialView("_SingleOrder", d);
            }
            return NotFound();
        }

              
        [HttpPost]        
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> OrderDone(OrderDetailsViewModel detail) 
        {

            if (ModelState.IsValid)
            {
                var dish = _context.Dishes.FirstOrDefault(d => d.Id == detail.DishId);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                if (dish != null)
                {
                    Order order = new Order()
                    {
                        UserId = userId,
                        Phonenumber = detail.PhoneNumber,
                        ShippingAddress = detail.ShippingAdrees,
                        CustomerName = user.Email,
                        PaymentMethod = detail.PaymentMethod.ToString(),
                        OrderDate = DateTime.Now,
                        TotalAmount = dish.Price,
                        Product = new List<Dish> { dish },
                        Quanity = 1,
                        Notes = detail.Notes,
                        ProductName = dish.Name,
                        Category = dish.Category,
                        Status = OrderStatus.New,
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
            else
            {
                return PartialView("_SingleOrder", detail);
            }
        }
        
    }
}