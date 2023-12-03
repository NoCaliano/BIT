using Akos.DataStuff;
using Akos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CartController : Controller
{
    private readonly AppDbContext _dbContext;

    public CartController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult CartOrder()
    {
        var userId = User.Identity.Name;
        var cart = _dbContext.Carts.Include(c => c.Products).FirstOrDefault(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                Products = new List<Dish>()
            };
        }

        return View(cart);
    }

    public IActionResult SubmitCart (Cart cart)
    {
        string customerName = User.Identity.Name;
        foreach (var dish in cart.Products)
        {
            Order order = new Order
            {

                CustomerName = customerName,
                OrderDate = DateTime.Now,
                TotalAmount = (int)Math.Round(dish.Price),
                Products = new List<Dish> { dish },
                Product = dish.Name,
                Category = dish.Category,
                Status = "Нове",
                ShippingAddress = " ",
                PaymentMethod = "Visa",
                Phonenumber = "+380",
                Notes = " nothing"
               
            };

            _dbContext.orders.Add(order);
            _dbContext.SaveChanges();
        }
        return RedirectToAction("Index", "Home");
    }
}
