using BIT.Models;

namespace BIT.Interfaces
{
    public interface IAllCouriers
    {
        IEnumerable<Courier> Couriers { get; }

        bool ReadyCouriersCount();
    }
}
