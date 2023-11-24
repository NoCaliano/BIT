using Microsoft.AspNetCore.Identity;

namespace BIT.Interfaces
{
    public interface IAllRoles
    {
        IEnumerable<IdentityRole> Roles { get; }
    }
}
