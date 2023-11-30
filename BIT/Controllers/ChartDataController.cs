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
            var salesData = _context.Orders
            .GroupBy(o => o.OrderDate.Date)
            .Select(group => new    
            {
        
                Date = group.Key,
        
                TotalSales = group.Sum(o => o.TotalAmount)
    
            })
    
            .OrderBy(item => item.Date)
    
            .ToList();

            List<DateTime> labels = salesData.Select(item => item.Date).ToList();
            List<float> totalSales = salesData.Select(item => item.TotalSales).ToList();

            data.Add(labels);
            data.Add(totalSales);


            return data;
        }

        [HttpPost]
        public IActionResult MostSellChart()
        {
            List<object> data = new List<object>();

            // Групуємо замовлення за категорією та підраховуємо кількість продажів для кожної категорії
            var categorySales = _context.Orders
                .GroupBy(o => o.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalSales = g.Count()
                })
                .OrderByDescending(g => g.TotalSales)
                .ToList();

            // Розділяємо результати на окремі списки
            List<string> labels = categorySales.Select(s => s.Category).ToList();
            List<int> count = categorySales.Select(s => s.TotalSales).ToList();

            // Додаємо дані до списку
            data.Add(labels);
            data.Add(count);

            return Json(data);
        }

    }
}
