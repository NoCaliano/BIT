using System.ComponentModel.DataAnnotations.Schema;

namespace BIT.Models
{
    public class Requisition
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        [Column("Ім'я")]
        public required string Name { get; set; }
        [Column("Прізвище")]
        public required string LastName { get; set; }
        [Column("Телефон")]
        public string? PhoneNumber { get; set; }
        [Column("Місце проживання")]
        public string? Address { get; set; }
        [Column("Тип транспорту")]
        public string? VehicleType { get; set; }
        [Column("Заявка прийнята")]
        public bool? Accepted { get; set; }
        [Column("Відповідь")]
        public string? Answer { get; set; }
    }
}
