using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BIT.ViewModels
{
    public class SettingViewModel
    {
        [Display(Name = "Адреса")]
        [Required(ErrorMessage = "Поле 'Адреса' обов'язкове.")]
        [MaxLength(70, ErrorMessage = "Максимальна довжина адреси - 70 символів.")]
        public string Address { get; set; }

        [Display(Name ="Ім'я")]
        [Required(ErrorMessage = "Поле 'Ім'я' обов'язкове.")]
        [MaxLength(35, ErrorMessage = "Максимальна довжина імені - 35 символів.")]
        [RegularExpression(@"^([А-ЯІ]{1}[а-яі]{1,35}|[A-Z]{1}[a-z]{1,35})$", ErrorMessage = "Дозволені тільки літери алфавіту.")]
        public string FirstName { get; set; }

        [Display(Name ="Прізвище")]
        [Required(ErrorMessage = "Поле 'Прізвище' обов'язкове.")]
        [MaxLength(35, ErrorMessage = "Максимальна довжина прізвища - 35 символів.")]
        [RegularExpression(@"^([А-ЯІ]{1}[а-яі]{1,35}|[A-Z]{1}[a-z]{1,35})$", ErrorMessage = "Дозволені тільки літери алфавіту.")]
        public string LastName { get; set; }

        [Display(Name ="Електронна пошта")]
        [Required(ErrorMessage = "Поле Емейл обов'язкове")]
        [EmailAddress(ErrorMessage = "Введіть коректну адресу електронної пошти")]
        public string Email { get; set; }

        [Display(Name ="Телефон")]
        [Required(ErrorMessage = "Поле 'Телефон' обов'язкове.")]
        [RegularExpression(@"^\+\d{7,13}$", ErrorMessage = "Телефонний номер повинен бути у форматі '+XXXXXXXXXXXXX', де X - це цифра.")]
        public string PhoneNumber { get; set; }

        [Display(Name ="Ваш пароль")]
        [Required(ErrorMessage = "Поле Поточний пароль обов'язкове")]
        [MinLength(8, ErrorMessage = "Мінімальна довжина паролю - 8 символів")]
        public string Password { get; set; }

        [Display(Name ="Новий пароль")]
        [Required(ErrorMessage = "Поле Новий пароль обов'язкове")]
        [MinLength(8, ErrorMessage = "Мінімальна довжина паролю - 8 символів")]
        public string NewPassword { get; set; }

        [Display(Name ="Підтвердження паролю")]
        [Required(ErrorMessage = "Поле Підтвердження паролю обов'язкове")]
        [Compare("NewPassword", ErrorMessage = "Пароль і підтвердження паролю не співпадають")]
        public string ConfirmPassword { get; set; }

          
    }

}
