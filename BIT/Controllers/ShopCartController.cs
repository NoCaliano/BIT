using BIT.Areas.Identity.Data;
using BIT.Attributes;
using BIT.DataStuff;
using BIT.Hubs;
using BIT.Models;
using BIT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BIT.Controllers
{
    [CustomAuthorize]
    public class ShopCartController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;
        public ShopCartController(ILogger<HomeController> logger, AppDbContext context, UserManager<ApplicationUser> userManager, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _hubContext = hubContext;
        }


        public IActionResult Checkout()
        { 
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CartForm()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var cart = _context.Carts.Include(c => c.CartItems)
                                     .ThenInclude(ci => ci.Dish)
                                     .FirstOrDefault(c => c.UserId == userId);
            
            CartDetailsViewModel cartDetailsViewModel = new CartDetailsViewModel()
            {                   
               CartId = cart.Id,                   
               PhoneNumber = user.PhoneNumber,                    
               ShippingAdrees = user.Address,                          
            };     
            return PartialView("_CartForm", cartDetailsViewModel);
            
        }

        public IActionResult PartCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Отримати кошик користувача з бази даних
            var cart = _context.Carts.Include(c => c.CartItems)
                                     .ThenInclude(ci => ci.Dish)
                                     .FirstOrDefault(c => c.UserId == userId);
            return PartialView("_MyCart", cart);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToListOfCartItems(int dishId)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);

            if (dish != null)
            {
                CartItem cartItem = new CartItem()
                {
                    Dish = dish,
                    Quantity = 1,
                };

                IActionResult result = await Cart(cartItem);

                return result;
            }


            // Обробка ситуації, коли страву не знайдено (наприклад, виведення повідомлення або перенаправлення на іншу сторінку)
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCart(int CartId)
        {
            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.Id == CartId);

            if (cart != null)
            {
                cart.GrandTotal = 0;
                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Cart(CartItem item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = _context.Carts.Include(c => c.CartItems)
                                     .ThenInclude(ci => ci.Dish)
                                     .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId!,
                    CartItems = new List<CartItem>() { item },
                    GrandTotal = item.Dish.Price
                };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }
            else
            {
                var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.Dish.Id == item.Dish.Id);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity++;
                }
                else
                {
                    cart.CartItems.Add(item);
                }

                cart.GrandTotal = cart.CartItems.Sum(ci => ci.Quantity * ci.Dish.Price);
                _context.SaveChanges();
            }
            await _hubContext.Clients.All.SendAsync("CartUpdated");
            return PartialView("_MyCart", cart);
        }

        public async Task<IActionResult> DecreaseQuantity(int dishId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = _context.Carts.Include(c => c.CartItems)
                                     .ThenInclude(ci => ci.Dish)
                                     .FirstOrDefault(c => c.UserId == userId);

            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Dish.Id == dishId);

                if (cartItem != null)
                {
                    if (cartItem.Quantity > 1)
                    {
                        cartItem.Quantity--;
                        cart.GrandTotal = cart.CartItems.Sum(ci => ci.Quantity * ci.Dish.Price);
                        _context.SaveChanges();
                    }
                    else
                    {
                        // Якщо кількість дорівнює 1, можливо, ви хочете видалити цей товар з корзини
                        cart.CartItems.Remove(cartItem);
                        cart.GrandTotal = cart.CartItems.Sum(ci => ci.Quantity * ci.Dish.Price);
                        _context.SaveChanges();
                    }
                }
            }
            await _hubContext.Clients.All.SendAsync("CartUpdated");

            return PartialView("_MyCart", cart);
        }

        public async Task<IActionResult> IncreaseQuantity(int dishId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = _context.Carts.Include(c => c.CartItems)
                                     .ThenInclude(ci => ci.Dish)
                                     .FirstOrDefault(c => c.UserId == userId);

            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Dish.Id == dishId);

                if (cartItem != null)
                {
                    cartItem.Quantity++;
                    cart.GrandTotal = cart.CartItems.Sum(ci => ci.Quantity * ci.Dish.Price);
                    _context.SaveChanges();
                }
            }
            await _hubContext.Clients.All.SendAsync("CartUpdated");

            return PartialView("_MyCart", cart);
        }

        public async Task<IActionResult> CleanSubject(int dishId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = _context.Carts.Include(c => c.CartItems)
                                     .ThenInclude(ci => ci.Dish)
                                     .FirstOrDefault(c => c.UserId == userId);

            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Dish.Id == dishId);

                if (cartItem != null)
                {
                    cart.CartItems.Remove(cartItem);
                    cart.GrandTotal = cart.CartItems.Sum(ci => ci.Quantity * ci.Dish.Price);
                    _context.SaveChanges();

                }
            }
            await _hubContext.Clients.All.SendAsync("CartUpdated");
            return PartialView("_MyCart", cart);
        }

        //Метод можна покращити
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(CartDetailsViewModel d)
        {   
            
            if (ModelState.IsValid)
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Dish)
                    .FirstOrDefaultAsync(c => c.Id == d.CartId && c.UserId == _userManager.GetUserId(User)); // важлива штука, тепер не можна через код елементу приколи вводити

                var orderDate = DateTime.Now;

                string NameOfUser = "Guest";
                var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
                if (user.FirstName != null) { NameOfUser = user.FirstName; }

                if (cart != null)
                {
                    for (int i = 0; i < cart.CartItems.Count(); i++)
                    {
                        var dish = cart.CartItems[i];

                        if (dish != null)
                        {
                            Order order = new Order()
                            {
                                UserId = _userManager.GetUserId(User),
                                Quanity = dish.Quantity,
                                Product = new List<Dish> { dish.Dish },
                                Category = dish.Dish.Category,
                                ProductName = dish.Dish.Name,
                                TotalAmount = dish.Dish.Price * dish.Quantity,
                                CustomerName = NameOfUser,
                                Status = OrderStatus.New,
                                OrderDate = orderDate,
                                ShippingAddress = d.ShippingAdrees,
                                Phonenumber = d.PhoneNumber,
                                PaymentMethod = d.PaymentMethod.ToString(),
                                Notes = d.Notes,
                            };

                            _context.Orders.Add(order);
                            await _context.SaveChangesAsync();
                        }
                    }
                    await DeleteCart(cart.Id);
                }
                else
                {
                    return RedirectToAction("Privacy");
                }
                return RedirectToAction("Thanks", "Order");
            }
            else 
            { 
                return PartialView("_CartOrder",d); 
            }
        }

    }
}