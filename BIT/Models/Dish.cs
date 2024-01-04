using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIT.Models
{
    
    public class Dish
    {
        public int Id { get; set; }
        [Column("Назва")]
        [Display(Name ="Назва")]
        [MaxLength(25, ErrorMessage = "Максимальна довжина 'Назва' - 25 символів.")]
        public string Name { get; set; }
        [Column("Опис")]
        [Display(Name="Опис")]
        [MaxLength(300, ErrorMessage = "Максимальна довжина 'Опис' - 300 символів.")]
        public string? Description { get; set; }
        [Column("Картинка")]
        [Display(Name="Картинка")]
        [MaxLength(20, ErrorMessage = "Максимальна довжина 'Картинка' - 100 символів.")]
        public string? Img { get; set; }
        [Column("Ціна")]
        [Display(Name="Ціна")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Дозволені тільки цифри, можливо десятковий роздільник та не більше двох знаків після коми.")]
        public float Price { get; set; }
        [Column("Калорії")]
        [Display(Name="Калорійність")]
        public int? Calories { get; set; }
        [Column("Доступно")]
        [Display(Name = "Доступно")]
        public bool IsAvaileble { get; set; }
        [Column("На головній")]
        [Display(Name = "На головній")]
        public bool IsFavorite { get; set; }
        [Column("Категорія")]
        [Display(Name = "Категорія")]
        public string? Category { get; set; }
    }
}
