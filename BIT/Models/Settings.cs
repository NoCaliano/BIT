using System.ComponentModel.DataAnnotations;

namespace BIT.Models
{
    // Моделька для інпутів у всякі форми
    public class Settings
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Розширена реєстрація")]
        public bool IsExtendedRegistration { get; set; }

        [Required]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; }



        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Телефон для зв'язку")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Місце проживання")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Вид транспорту")]
        public string VehicleType { get; set; }
    }
}
