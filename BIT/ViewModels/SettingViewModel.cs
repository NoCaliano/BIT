using System.ComponentModel.DataAnnotations;

namespace BIT.ViewModels
{
    public class SettingViewModel
    {
        [Required(ErrorMessage = "Поле Адреса обов'язкове")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Поле Ім'я обов'язкове")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле Прізвище обов'язкове")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле Емейл обов'язкове")]
        [EmailAddress(ErrorMessage = "Введіть коректну адресу електронної пошти")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле Телефон обов'язкове")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Поле Поточний пароль обов'язкове")]
        [MinLength(6, ErrorMessage = "Мінімальна довжина паролю - 6 символів")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле Новий пароль обов'язкове")]
        [MinLength(6, ErrorMessage = "Мінімальна довжина паролю - 6 символів")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Поле Підтвердження паролю обов'язкове")]
        [Compare("NewPassword", ErrorMessage = "Пароль і підтвердження паролю не співпадають")]
        public string ConfirmPassword { get; set; }
    }
}
