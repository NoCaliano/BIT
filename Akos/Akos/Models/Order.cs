using Akos.Models;

namespace Akos.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Product {  get; set; }
        public List<Dish> Products { get; set; }
        public string Category { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }
        public string Phonenumber { get; set; }
        public string Status { get; set; }
    }
}