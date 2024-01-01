using BIT.DataStuff;
using BIT.Models;
using Microsoft.EntityFrameworkCore;

namespace BIT.Services
{
    public class OrderProcessingService
    {
        private readonly AppDbContext _dbContext;

        public OrderProcessingService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CheckAndCancelOrders()
        {
            var newOrders = await _dbContext.Orders
                .Where(o => o.Status == OrderStatus.New)
                .ToListAsync();

            // Перевірити час створення і оновити статус, якщо потрібно
            foreach (var order in newOrders)
            {
                if (DateTime.Now - order.OrderDate > TimeSpan.FromMinutes(30))
                {
                    order.Status = OrderStatus.Canceled;

                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
