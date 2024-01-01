using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIT.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Column("Страва")]
        public string ProductName { get; set; }
        [Column("Кількість")]
        public int Quanity { get; set; }
        [Column("Клієнт")]
        public string CustomerName { get; set; }
        [Column("Номер телефону")]
        [RegularExpression(@"^\+\d{8,}$", ErrorMessage = "Потрібно вказати плюс та мінімум 8 цифр")]
        public string? Phonenumber { get; set; }
        [Column("Адреса")]        
        public string ShippingAddress { get; set; }
        public string? UserId { get; set; }
        public List<Dish> Product { get; set; }
        [Column("Категорія")]
        public string? Category { get; set; }
        [Column("Дата")]
        public DateTime OrderDate { get; set; }
        [Column("Сума")]
        public float TotalAmount { get; set; }
        [Column("Коментар")]
        public string? Notes { get; set; }
        [Column("Спосіб оплати")]
        [RegularExpression(@"^[a-zA-Z]{4,}$", ErrorMessage = "Мінімум 4 букви")]
        public string PaymentMethod { get; set; }
        [Column("Кур'єр")]
        public string? Courier { get; set; }
        [Column("Статус")]
        public OrderStatus Status { get; set; }
        public string? CourId { get; set; }
    }

    public enum OrderStatus
    {
        New,
        Processing,
        Delivered,
        Canceled
    }
}
