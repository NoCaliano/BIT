using BIT.DataStuff;
using BIT.Interfaces;
using BIT.Models;

namespace BIT.Mocks
{
    public class MockCourier : IAllCouriers
    {
        private readonly AppDbContext _context;
        public MockCourier(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Courier> Couriers => _context.Couriers.ToList();

        public bool ReadyCouriersCount()
        {
            return _context.Couriers.Any(c => c.ReadyToWork == true);
        }
    }
}
