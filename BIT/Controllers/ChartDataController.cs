using BIT.DataStuff;
using Microsoft.AspNetCore.Mvc;

namespace BIT.Controllers
{
    //Контролер даних для графіків
    public class ChartDataController : Controller
    {
        private readonly ILogger<ChartDataController> _logger;
        private readonly AppDbContext _context;

        public ChartDataController(ILogger<ChartDataController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public List<object> PriceChart()
        {
            List<object> data = new List<object>();

            List<DateTime> labels = _context.Orders.Select(c => c.OrderDate).ToList();

            data.Add(labels);

            List<float> price = _context.Orders.Select(p => p.TotalAmount).ToList();

            data.Add(price);


            return data;
        }

    }
}
