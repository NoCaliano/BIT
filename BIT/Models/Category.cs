using System.ComponentModel.DataAnnotations.Schema;

namespace BIT.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Column("Назва")]
        public string Name { get; set; }
        [Column("Опис")]
        public string? Description { get; set; }
    }
}
