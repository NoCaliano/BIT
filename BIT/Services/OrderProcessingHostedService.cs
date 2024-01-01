using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BIT.Services
{
    public class OrderProcessingHostedService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _services;
        private Timer _timer;

        public OrderProcessingHostedService(IServiceProvider services)
        {
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(15)); // Перевірка кожні 15 хвилин

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = _services.CreateScope())
            {
                var orderProcessingService = scope.ServiceProvider.GetRequiredService<OrderProcessingService>();
                orderProcessingService.CheckAndCancelOrders().Wait();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
