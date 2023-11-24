using BIT.Areas.Identity.Data;
using BIT.Data;
using BIT.Interfaces;

namespace BIT.Mocks
{
    public class MockUser :IAllUsers
    {
        private readonly AuthDbContext _context;
        public MockUser(AuthDbContext context)
        {
            _context = context;
        }
        public IEnumerable<ApplicationUser> Users => _context.Users.ToList();
    }
}
