using System.ComponentModel.DataAnnotations.Schema;

namespace BIT.Models
{
    public class Courier
    {
        public int Id { get; set; }        
        public required string UserId { get; set; }        
        [Column("Email")]
        public required string Email { get; set; }
        [Column("Ім'я")]
        public required string Name { get; set; }
        [Column("Прізвище")]
        public required string LastName { get; set; }
        [Column("Телефон")]
        public string? PhoneNumber { get; set; }
        [Column("Транспорт")]
        public string? VehicleType { get; set; }
        [Column("Працює зараз")]
        public bool? ReadyToWork { get; set; }
        [Column("Остання зарплата")]
        public DateTime? LastSalary { get; set; }
        [Column("Зарплата")]
        public int? Salary { get; set; }
        [Column("Виконано доставок")]
        public int? Delievered { get; set; }

    }
}
