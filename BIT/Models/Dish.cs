using System.ComponentModel.DataAnnotations.Schema;

namespace BIT.Models
{
    public class Dish
    {
        public int Id { get; set; }
        [Column("Назва")]
        public string Name { get; set; }
        [Column("Опис")]
        public string? Description { get; set; }
        [Column("Картинка")]
        public string? Img { get; set; }
        [Column("Ціна")]
        public float Price { get; set; }
        [Column("Калорії")]
        public int? Calories { get; set; }
        [Column("Доступно")]
        public bool IsAvaileble { get; set; }
        [Column("На головній")]
        public bool IsFavorite { get; set; }
        [Column("Категорія")]
        public string? Category { get; set; }
    }
}
