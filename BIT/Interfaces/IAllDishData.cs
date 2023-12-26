namespace BIT.Interfaces
{
    public interface IAllDishData
    {
        string RetDishImageByName(string dishName);
        string RetDishImageById(int id);
        bool IsCartNotEmpty(int cartid);
    }
}
