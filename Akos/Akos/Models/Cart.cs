using Akos.Models;

namespace Akos.Models
{
    public class Cart
    {
        public Cart()
        {
            Products = new List<Dish>();
        }

        public int Id { get; set; }

        // Ідентифікатор користувача, до якого належить корзина
        public string UserId { get; set; }

        // Список товарів у корзині
        public virtual List<Dish> Products { get; set; }
    }

}