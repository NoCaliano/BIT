using System.ComponentModel.DataAnnotations;

namespace BIT.Models
{
    public class FeedBack
    {
        public int Id { get; set; }
        public string? userId { get; set; }

        [Display(Name = "Ім'я")]
        [Required(ErrorMessage = "Поле 'Ім'я' обов'язкове.")]
        [MaxLength(35, ErrorMessage = "Максимальна довжина імені - 35 символів.")]
        [RegularExpression(@"^([А-ЯІ]{1}[а-яі]{1,35}|[A-Z]{1}[a-z]{1,35})$", ErrorMessage = "Дозволені тільки літери алфавіту.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Поле 'Текст' обов'язкове для заповнення.")]
        [MaxLength(400, ErrorMessage = "Максимальна довжина тексту - 400 символів.")]
        public string? Text { get; set; }

        [Required(ErrorMessage = "Поле 'Рейтинг' обов'язкове для заповнення.")]
        [Range(1, 5, ErrorMessage = "Рейтинг повинен бути в діапазоні від 1 до 5.")]
        public int? Rating { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
