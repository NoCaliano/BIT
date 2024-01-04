using BIT.Attributes;
using BIT.DataStuff;
using BIT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BIT.Controllers
{
    public class WelcomeController : Controller
    {
        private readonly ILogger<WelcomeController> _logger;
        private readonly AppDbContext _context;
        public WelcomeController(ILogger<WelcomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult FeedBacks()
        {
            return View(_context.FeedBacks.ToList());
        }

        public IActionResult FeedBackForm()
        {
            return PartialView("_FeedBackForm");
        }

        [CustomAuthorize]
        [HttpPost]
        public async Task<IActionResult> CreateFeedBack(FeedBack model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                model.userId = userId;
                model.DateTime = DateTime.Now;
                _context.FeedBacks.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("FeedBacks", "Welcome");
            }
            return RedirectToAction("FAQ", "Welcome");
        }
    }
}
