using System.ComponentModel.DataAnnotations;

namespace BIT.ViewModels
{
    public class CartDetailsViewModel
    {
        [Required]
        public int CartId { get; set; }

        [Required(ErrorMessage = "Поле 'PaymentMethod' обов'язкове.")]
        public string PaymentMethod { get; set; }

        
        [Required(ErrorMessage = "Поле 'PhoneNumber' обов'язкове.")]        
        [Phone(ErrorMessage = "Невірний формат телефонного номера.")]
        [Display(Name = "Телефон")]       
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Поле 'ShippingAdrees' обов'язкове.")]
        [RegularExpression(@"^[a-zA-Z0-9\s,'-]*$", ErrorMessage = "Дозволені лише літери, цифри та деякі спецсимволи.")]
        public string ShippingAdrees { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9\s,'-]*$", ErrorMessage = "Дозволені лише літери, цифри та деякі спецсимволи.")]
        public string? Notes { get; set; }
    }
}
