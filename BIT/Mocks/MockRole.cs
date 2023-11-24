using BIT.Data;
using BIT.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BIT.Mocks
{
    public class MockRole :IAllRoles
    {
        private readonly AuthDbContext _context;
        public MockRole(AuthDbContext context)
        {
            _context = context;
        }
        public IEnumerable<IdentityRole> Roles => _context.Roles.ToList();
    }
}
