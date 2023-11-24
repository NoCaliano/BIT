using BIT.DataStuff;
using BIT.Interfaces;
using BIT.Models;

namespace BIT.Mocks
{
    public class MockRequisition : IAllRequisitions
    {
        private readonly AppDbContext _context;
        public MockRequisition(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Requisition> Requisitions => _context.Requisitions.ToList();
    }
}
