using Akos.Models;

namespace Akos.Models
{
    public class Dish
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string img { get; set; }

        public float Price { get; set; }
        public int Calories { get; set; }

        public bool IsAvaileble { get; set; }

        public bool IsFavorite { get; set; }

        public string Category { get; set; }
    }
}
