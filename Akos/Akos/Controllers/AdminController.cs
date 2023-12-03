using Akos.DataStuff;
using Akos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Akos.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly ILogger<AdminController> _logger;
        private readonly AppDbContext _context;


        public AdminController(ILogger<AdminController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("Admin")]
        public IActionResult Dashboard()
        {
            return View("~/Views/Admin/Dashboard.cshtml");
        }

        public IActionResult AddNewDish()
        {
            ViewData["categories"] = new SelectList(_context.categories.ToList(), "Name", "Name");
            return View();
        }

        public IActionResult DeleteDish()
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
                _context.categories.Add(model);
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
                _context.dishes.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var dish = _context.dishes.Find(id);

            if (dish == null)
            {
                return NotFound();
            }

            _context.dishes.Remove(dish);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home"); // Перенаправлення до головної сторінки або іншої відповідної дії
        }

    }
}
