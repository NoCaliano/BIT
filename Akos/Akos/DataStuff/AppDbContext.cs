using Microsoft.EntityFrameworkCore;
using Akos.Models;

namespace Akos.DataStuff
{
    public class AppDbContext : DbContext
    {
        public IConfiguration _config { get; set; }

        public AppDbContext(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
        }

        public DbSet<Dish> dishes { get; set; }
        public DbSet<Category> categories { get; set; }

        public DbSet<Order> orders { get; set; }

        public DbSet<Cart> Carts { get; set; }
    }
}
