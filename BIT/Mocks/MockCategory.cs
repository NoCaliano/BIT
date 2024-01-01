using BIT.DataStuff;
using BIT.Interfaces;
using BIT.Models;

namespace BIT.Mocks
{
    public class MockCategory : IAllCategories
    {
        private readonly AppDbContext _context;
        public MockCategory(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Category> Categories => _context.Categories.ToList();
    }
}
