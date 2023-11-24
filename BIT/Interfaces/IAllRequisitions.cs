using BIT.Models;

namespace BIT.Interfaces
{
    public interface IAllRequisitions
    {
        IEnumerable<Requisition> Requisitions { get; }
    }
}
