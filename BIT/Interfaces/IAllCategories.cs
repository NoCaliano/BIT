using BIT.Models;

namespace BIT.Interfaces
{
    public interface IAllCategories
    {
        IEnumerable<Category> Categories { get; }
    }
}
