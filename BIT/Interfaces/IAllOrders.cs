using BIT.Models;

namespace BIT.Interfaces
{
    public interface IAllOrders
    {
        IEnumerable<Order> Orders { get; }

        IEnumerable<Order> LastOrders { get; }
    }
}
