using BIT.Models;

namespace BIT.Interfaces
{
    public interface IAllDishes
    {
        IEnumerable<Dish> Dishes { get; }
    }
}
