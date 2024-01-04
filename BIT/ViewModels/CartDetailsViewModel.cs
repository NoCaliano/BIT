using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BIT.ViewModels
{
    public class CartDetailsViewModel 
    {
        [Required]
        public int CartId { get; set; }

        [Display(Name ="Спосіб оплати")]
        [Required(ErrorMessage = "Поле 'Спосіб оплати' обов'язкове.")]
        public Payment PaymentMethod { get; set; }


        [Display(Name = "Телефон")]
        [Required(ErrorMessage = "Поле 'Телефон' обов'язкове.")]
        [RegularExpression(@"^\+\d{7,13}$", ErrorMessage = "Телефонний номер повинен бути у форматі '+XXXXXXXXXXXXX', де X - це цифра.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Адреса")]
        [Required(ErrorMessage = "Поле 'Адреса' обов'язкове.")]
        [MaxLength(70, ErrorMessage = "Максимальна довжина адреси - 70 символів.")]
        public string ShippingAdrees { get; set; }

        [Display(Name = "Замітки")]
        [MaxLength(100, ErrorMessage = "Максимальна довжина нотаток - 100 символів.")]
        public string? Notes { get; set; }

    }

}
