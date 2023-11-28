using BIT.Areas.Identity.Data;
using BIT.DataStuff;
using BIT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BIT.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShopCartController(ILogger<HomeController> logger, AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult AddToListOfCartItems(int dishId)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.Id == dishId);

            if (dish != null)
            {
                CartItem cartItem = new CartItem()
                {
                    Dish = dish,
                    Quantity = 1,
                };

                return Cart(cartItem);
            }

            // Обробка ситуації, коли страву не знайдено (наприклад, виведення повідомлення або перенаправлення на іншу сторінку)
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCart(int CartId)
        {
            var cart = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.Id == CartId);

            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        /* старий метод
        public async Task<IActionResult> OrderCart(int CartId)
        {
            var cart = await _context.Carts.Include(c => c.CartItems)
                                            .ThenInclude(ci => ci.Dish)
                                            .FirstOrDefaultAsync(c => c.Id == CartId);
            var orderDate = DateTime.Now;

            string CustName = User.Identity.Name;
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
                            CustomerName = CustName,
                            Status = "New",
                            OrderDate = orderDate,

                            ShippingAddress = "South Centrel LA",
                            Phonenumber = "+380688037364",
                            PaymentMethod = "MasterCard",
                            Notes = "All my fellas",
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

        */
        public IActionResult Cart(CartItem item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Cart cart = _context.Carts.Include(c => c.CartItems)
                                     .ThenInclude(ci => ci.Dish)
                                     .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
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


            return PartialView("_MyCart", cart);
        }

        public IActionResult MyCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Отримати кошик користувача з бази даних
            var cart = _context.Carts.Include(c => c.CartItems)
                                     .ThenInclude(ci => ci.Dish)
                                     .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                // Створити пустий кошик, якщо його не існує
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>(),
                    GrandTotal = 0
                };
            }

            return View(cart);
        }

        public IActionResult DecreaseQuantity(int dishId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Cart cart = _context.Carts.Include(c => c.CartItems)
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

            return PartialView("_MyCart", cart);
        }

        public IActionResult IncreaseQuantity(int dishId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Cart cart = _context.Carts.Include(c => c.CartItems)
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

            return PartialView("_MyCart", cart);
        }

        public IActionResult CleanSubject(int dishId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Cart cart = _context.Carts.Include(c => c.CartItems)
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

            return PartialView("_MyCart", cart);
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



        //Сторінка яка приймає в себе модель
        public async Task<IActionResult> OrderDetails(int CartId)
        {
            var cart = await _context.Carts.Include(c => c.CartItems)
                                            .ThenInclude(ci => ci.Dish)
                                            .FirstOrDefaultAsync(c => c.Id == CartId);
            return View(cart);
        }



        //Новий метод. надає можливість заповнити дані для замовлення кошика
        public async Task<IActionResult> Confirm(int CartId, string Payment, string Notes, string Address, string Number)
        {
            var cart = await _context.Carts        
                .Include(c => c.CartItems)        
                .ThenInclude(ci => ci.Dish)        
                .FirstOrDefaultAsync(c => c.Id == CartId && c.UserId == _userManager.GetUserId(User)); // важлива штука, тепер не можна через код елементу приколи вводити

            var orderDate = DateTime.Now;

            string CustName = User.Identity.Name;

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
                            CustomerName = CustName,
                            Status = "New",
                            OrderDate = orderDate,

                            ShippingAddress = Address,
                            Phonenumber = Number,
                            PaymentMethod = Payment,
                            Notes = Notes,
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
    }
}

// TODO зробити люту валідацію, або хімія з html