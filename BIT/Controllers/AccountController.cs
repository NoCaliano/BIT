using BIT.Areas.Identity.Data;
using BIT.Attributes;
using BIT.DataStuff;
using BIT.Models;
using BIT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace BIT.Controllers
{
    [CustomAuthorize]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(ILogger<AccountController> logger, AppDbContext context, UserManager<ApplicationUser> userManager)
        {
           
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult MyOrders() 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Запит на отримання списку замовлень для конкретного користувача
            var userOrders = _context.Orders
                .Where(o => o.UserId == userId)
                .ToList();
            return View(userOrders); 
        }


        [HttpPost]        
        public async Task<IActionResult> CancelOrder(int ordId)
        {
            var ord = _context.Orders.FirstOrDefault(o => o.Id == ordId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ord != null && ord.UserId == userId)
            {
                ord.Status = OrderStatus.Canceled;
                await _context.SaveChangesAsync();
                return RedirectToAction("MyOrders", "Account");
            }

            return NotFound();
        }

        public async Task<IActionResult> Settings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            SettingViewModel model = new SettingViewModel()
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail(SettingViewModel settings)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (settings.Email != null)
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }

                var emailExists = await _userManager.FindByEmailAsync(settings.Email);
                if (emailExists != null && emailExists.Id != userId)
                {
                    ModelState.AddModelError("Email", "Цей емейл вже зайнятий.");
                    return View("Settings", settings); 
                }

                var token = await _userManager.GenerateChangeEmailTokenAsync(user, settings.Email);

                await _userManager.ChangeEmailAsync(user, settings.Email, token);
                await _userManager.SetUserNameAsync(user, settings.Email);
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Settings", "Account");
            }

            return NotFound();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(SettingViewModel setting)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (setting.NewPassword != null && setting.Password != null && setting.ConfirmPassword != null)
            {

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userManager.ChangePasswordAsync(user, setting.Password, setting.NewPassword);

                if (result.Succeeded)
                {
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction("Settings", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    // Повертаємо перегляд разом із моделлю, щоб вивести повідомлення про помилки
                    return View("Settings", new SettingViewModel
                    {
                        // Передаємо модель знову на сторінку, щоб дані залишалися на місці
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address
                    });
                }
            }
            // Повертаємо перегляд разом із моделлю, щоб вивести повідомлення про помилки
            return View("Settings", setting);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeAddress(SettingViewModel setting)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (setting.Address != null)
            {

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    user.Address = setting.Address;
                    await _userManager.UpdateAsync(user);
                }

                return RedirectToAction("Settings", "Account");
            }
            return View("Settings");
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePhone(SettingViewModel setting)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(setting.PhoneNumber != null) 
            { 
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    var Token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, setting.PhoneNumber);
                    await _userManager.ChangePhoneNumberAsync(user, setting.PhoneNumber, Token);
                    await _userManager.UpdateAsync(user);
                }

                return RedirectToAction("Settings", "Account");
            }
            return View("Settigns");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SetName(SettingViewModel setting)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(setting.FirstName != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    user.FirstName = setting.FirstName;
                    await _userManager.UpdateAsync(user);
                }

                return RedirectToAction("Settings", "Account");
            }
            return View("Setting");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SetLastName(SettingViewModel setting)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            
            if(setting.LastName != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    user.LastName = setting.LastName;
                    await _userManager.UpdateAsync(user);
                }

                return RedirectToAction("Settings", "Account");
            }
            return View("Setting");
        }




    }
}
