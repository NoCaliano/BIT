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
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PasswordHasher<ApplicationUser> _passwordHasher;
        public AccountController(ILogger<AccountController> logger, AppDbContext context, UserManager<ApplicationUser> userManager)
        {
           
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult MyOrders() { return View(); }


        [HttpPost]
        public async Task<IActionResult> CancelOrder(int ordId)
        {
            var ord = _context.Orders.FirstOrDefault(o => o.Id == ordId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ord != null && ord.UserId == userId)
            {
                ord.Status = "Cancelled";
                await _context.SaveChangesAsync();
                return RedirectToAction("MyOrders", "Account");
            }

            return NotFound();
        }

        public IActionResult Settings()
        {
           return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeEmail(string userId, string NewEmail)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (loggedInUserId != userId)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.FindByIdAsync(userId);
                var Token = await _userManager.GenerateChangeEmailTokenAsync(user, NewEmail);
                if (user == null)
                {
                    return NotFound();
                }
                await _userManager.ChangeEmailAsync(user, NewEmail, Token);
                await _userManager.SetUserNameAsync(user, NewEmail);
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Settings", "Account");
            }
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(string userId, string OldPassword, string NewPassword)
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (loggedInUserId != userId)
            {
                return NotFound();
            }
            else
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userManager.ChangePasswordAsync(user, OldPassword, NewPassword);

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
                    return View("Settings", new Settings
                    {
                        // Тут ви можете передати інші дані, які вам потрібні для відображення на сторінці
                    });
                }
            }
        }




    }
}
