
using System;
using Akos.DataStuff;
using Akos.Interfaces;
using Akos.Models;

namespace Akos.Mocks
{
    public class MockDish : IAllDishes
    {
        private readonly AppDbContext _context;
        public MockDish(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Dish> Dishes => _context.dishes.ToList();
    }
}