using BIT.Areas.Identity.Data;
using BIT.DataStuff;
using BIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace BIT.Controllers
{
    [Route("Admin")]
    [Authorize(Roles ="Admin")]
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

        [Route("/Admin/Dashboard")]
        public IActionResult Dashboard()
        {
            return View("~/Views/Admin/Dashboard.cshtml");
        }

        [Route("Orders/All")]
        public IActionResult Orders()
        {
            return View("~/Views/Admin/Orders/Orders.cshtml");
        }

        [Route("/Admin/Orders/FindOrder")]
        public IActionResult FindOrder()
        {
            return View("~/Views/Admin/Orders/FindOrder.cshtml");
        }

        [Route("/Admin/Charts/Charts")]
        public IActionResult Charts()
        {
            return View("~/Views/Admin/Charts/Charts.cshtml");
        }

        [Route("/Admin/Users/Users")]
        public IActionResult Users()
        {
            return View("~/Views/Admin/Users/Users.cshtml");
        }

        [Route("/Admin/Couriers/Couriers")]
        public IActionResult Couriers()
        {
            return View("~/Views/Admin/Couriers/Couriers.cshtml");
        }

        [Route("/Admin/Couriers/Requisitions")]
        public IActionResult Requisitions()
        {
            return View("~/Views/Admin/Couriers/Requisitions.cshtml");
        }

        [Route("/Admin/Users/Roles")]
        public IActionResult Roles()
        {
            return View("~/Views/Admin/Users/Roles.cshtml");
        }

        [Route("/Admin/Users/Find")]
        public IActionResult Find()
        {
            return View("~/Views/Admin/Users/Find.cshtml");
        }

        [Route("/Admin/Product/AddDish")]
        public IActionResult AddDish()
        {
            return View("~/Views/Admin/Product/AddDish.cshtml");
        }

        [Route("/Admin/Product/DeleteDish")]
        public IActionResult DeleteDish()
        {
            return View("~/Views/Admin/Product/DeleteDish.cshtml");
        }

        [Route("/Admin/Product/EditDish")]
        public IActionResult EditDish() 
        {
            return View("~/Views/Admin/Product/EditDish.cshtml");
        }

        [Route("/Admin/Product/AddCategory")]
        public IActionResult AddCategory()
        {
            return View("~/Views/Admin/Product/AddCategory.cshtml");
        }

        [Route("/Admin/AddNewCategory")]
        [HttpPost]
        public IActionResult AddNewCategory(Category model)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Dashboard", "Admin"); // Повертаємо до панелі адміністратора або іншої сторінки
            }

            return View("~/Views/Admin/Product/AddCategory.cshtml", model);
        }

        [Route("/Admin/AddNewDish")]
        [HttpPost]
        public IActionResult AddNewDish(Dish model)
        {

            if (ModelState.IsValid)
            {
                _context.Dishes.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View("~/Views/Admin/Product/AddDish.cshtml", model);
        }

        [Route("/Admin/Delete")]
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

        [Route("/Admin/UpdateRequisitionStatus")]
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

        [Route("/Admin/SetSalary")]
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

        [Route("/Admin/PaySalary")]
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

        [Route("/Admin/ShowUserData")]
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

        [Route("/Admin/ShowOrderData")]
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
            return PartialView("_OrderData", null);
        }

        [Route("/Admin/Product/Edit/")]
        [HttpPost]
        public IActionResult SentToEdit(int dishId)
        {
            var dish = _context.Dishes.FirstOrDefault(p => p.Id == dishId);
            if (dish != null)
            {
                return View("~/Views/Admin/Product/Edit.cshtml", dish);
            }
            return NotFound();
        }


        [Route("/Admin/Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit( Dish model)
        {
            var dish = _context.Dishes.FirstOrDefault(p => p.Id == model.Id);
            if (ModelState.IsValid)
            {
                if (model != null && dish != null)
                {                    
                    dish.Name = model.Name;
                    dish.IsFavorite = model.IsFavorite;
                    dish.Calories = model.Calories;
                    dish.Description = model.Description;
                    dish.Img = model.Img;
                    dish.Category = model.Category;
                    dish.IsAvaileble = model.IsAvaileble;
                    dish.Price = model.Price;                   
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Home", "Welcome");
                }
            }

            return View("~/Views/Admin/Product/Edit.cshtml", model);
        }

        [Route("/Admin/ShowDishData")]
        [HttpGet]
        public IActionResult ShowDishData(string IdOrName)
        {
            // Перевірка, чи IdOrPhone можна конвертувати в int
            if (int.TryParse(IdOrName, out int dishId))
            {
                // Конвертація вдалася, шукаємо за Id
                var dish = _context.Dishes.FirstOrDefault(o => o.Id == dishId);
                if (dish != null)
                {
                    return PartialView("_FindDish", dish);
                }
            }

            // Шукаємо за номером назвою
            var dish_ = _context.Dishes.FirstOrDefault(p => p.Name == IdOrName);

            if (dish_ != null)
            {
                return PartialView("_FindDish", dish_);
            }

            // Якщо не вдалося знайти за Id і за назвою, повертаємо відповідь про помилку
            return PartialView("_FindDish", null);
        }

        [Route("/Admin/Product/Find")]
        public IActionResult FindDish()
        {
            return View("~/Views/Admin/Product/FindDish.cshtml");
        }

    }
}
