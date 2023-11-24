using BIT.DataStuff;
using BIT.Interfaces;
using BIT.Models;

namespace BIT.Mocks
{
    public class MockOrder : IAllOrders
    {
        private readonly AppDbContext _context;
        public MockOrder(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Order> Orders => _context.Orders.ToList();
    }
}
