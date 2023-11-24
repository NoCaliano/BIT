using BIT.DataStuff;
using BIT.Interfaces;
using BIT.Models;

namespace BIT.Mocks
{
    public class MockDish: IAllDishes
    {
        private readonly AppDbContext _context;
        public MockDish(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Dish> Dishes => _context.Dishes.ToList();
    }
}
