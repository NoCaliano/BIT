using BIT.DataStuff;
using BIT.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BIT.Mocks
{
    public class MockData : IAllDishData
    {
        private readonly AppDbContext _context;

        public MockData(AppDbContext context)
        {
            _context = context;
        }
        public string RetDishImageByName(string dishName)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.Name == dishName);
            return dish.Img;
        }

        public string RetDishImageById(int id)
        {
            var dish = _context.Dishes.FirstOrDefault(d => d.Id == id);
            return dish.Img;
        }

        public bool IsCartNotEmpty(int cartId) 
        {
            var cart = _context.Carts.FirstOrDefault(d => d.Id == cartId);
            return cart.CartItems.Count > 0;
        }
    }
}
