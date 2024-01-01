﻿using BIT.Areas.Identity.Data;
using BIT.DataStuff;
using BIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace BIT.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ILogger<AdminController> logger, AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [Route("Admin")]
        public IActionResult Dashboard()
        {
            return View("~/Views/Admin/Dashboard.cshtml");
        }

        public IActionResult Orders()
        {
            return View();
        }

        public IActionResult FindOrder()
        {
            return View();
        }

        public IActionResult Charts()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }

        public IActionResult Couriers()
        {
            return View();
        }

        public IActionResult Roles()
        {
            return View();
        }


        public IActionResult Find()
        {
            return View();
        }

        
        public IActionResult AddNewDish()
        {
            ViewData["categories"] = new SelectList(_context.Categories.ToList(), "Name", "Name");
            return View();
        }

        public IActionResult DeleteDish()
        {
            return View();
        }

        public IActionResult EditDish() 
        {
            return View();
        }
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNewCategory(Category model)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Dashboard", "Admin"); // Повертаємо до панелі адміністратора або іншої сторінки
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult AddNewDish(Dish model)
        {

            if (ModelState.IsValid)
            {
                _context.Dishes.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var dish = _context.Dishes.Find(id);

            if (dish == null)
            {
                return NotFound();
            }

            _context.Dishes.Remove(dish);
            _context.SaveChanges();

            return RedirectToAction("DeleteDish", "Admin"); // Перенаправлення до головної сторінки або іншої відповідної дії
        }


        public async Task<IActionResult> AddRole(string userId, string roleName)
        {
            // Отримати користувача за його ID
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                // Користувач не знайдений, обробити відповідно
                return NotFound();
            }

            // Додати користувачеві роль
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                // Роль успішно додана
                return RedirectToAction("Index");
            }
            else
            {
                // Помилка при додаванні ролі, обробити відповідно
                return View("Error");
            }
        }

        public IActionResult Requisitions()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateRequisitionStatus(int reqId)
        {
            var req = _context.Requisitions.FirstOrDefault(r => r.Id == reqId);
            var user = await _userManager.FindByIdAsync(req.UserId);
            if (req != null)
            {
                req.Accepted = true;

                if (req.Accepted == true)
                {
                    Courier courier = new Courier()
                    {
                        UserId = req.UserId,
                        Email = user.Email,
                        Name = req.Name,
                        LastName = req.LastName,
                        PhoneNumber = req.PhoneNumber,
                        VehicleType = req.VehicleType,

                    };
                    _context.Couriers.Add(courier);

                    if (user != null)
                    {
                        await _userManager.AddToRoleAsync(user, "Courier");
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Requisitions");
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> SetSalary(int couId, int Salary)
        {
            var courier = _context.Couriers.FirstOrDefault(c => c.Id == couId);
            if (courier != null)
            {
                courier.Salary = Salary;                
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Couriers", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> PaySalary(int couId)
        {
            var courier = _context.Couriers.FirstOrDefault(c => c.Id == couId);
            if (courier != null)
            {
                courier.LastSalary = DateTime.Now;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Couriers", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> ShowUserData(string IdOrEmail)
        {
            var user = await _userManager.FindByIdAsync(IdOrEmail);
            if (user != null)
            {
                return PartialView("_UserStatistic", user);
            }
            else
            {
                var userr = await _userManager.FindByEmailAsync(IdOrEmail);
                {
                    if (userr != null)
                    {
                        return PartialView("_UserStatistic", userr);
                    }
                }
            }
            return PartialView("_UserNotFound");
        }

        [HttpGet]
        public IActionResult ShowOrderData(string IdOrPhone)
        {
            // Перевірка, чи IdOrPhone можна конвертувати в int
            if (int.TryParse(IdOrPhone, out int orderId))
            {
                // Конвертація вдалася, шукаємо за Id
                var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
                if (order != null)
                {
                    return PartialView("_OrderData", order);
                }
            }

            // Шукаємо за номером телефону
            var orders = _context.Orders.Where(p => p.Phonenumber == IdOrPhone).ToList();

            if (orders.Count() > 0)
            {
                return PartialView("_ListOfOrdersData", orders);
            }

            // Якщо не вдалося знайти за Id і за номером телефону, повертаємо відповідь про помилку
            return PartialView("_UserNotFound");
        }

        [HttpPost]
        public IActionResult Edit(int dishId)
        {
            var dish = _context.Dishes.FirstOrDefault(p => p.Id == dishId);
            if (dish != null)
            {
                return PartialView("_EditDish", dish);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditThis(Dish model)
        {
            
            var existingDish = await _context.Dishes.FindAsync(model.Id);
            if (existingDish != null)
            {
                existingDish.Img = model.Img;
                existingDish.Category = model.Category;
                existingDish.Calories = model.Calories;
                existingDish.IsAvaileble = model.IsAvaileble;
                existingDish.Description = model.Description;
                existingDish.IsFavorite = model.IsFavorite;
                existingDish.Price = model.Price;
                existingDish.Name = model.Name;
                await _context.SaveChangesAsync();
                return RedirectToAction("Home", "Welcome");
            }

            return PartialView("_EditDish", model);
        }

    }
}
