using BIT.Areas.Identity.Data;

namespace BIT.Interfaces
{
    public interface IAllUsers
    {
        IEnumerable<ApplicationUser> Users { get; }
    }
}
