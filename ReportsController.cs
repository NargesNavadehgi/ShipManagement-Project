using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipManagement.Data;
using ShipManagement.Models;

namespace ShipManagement.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor Injection
        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShipmentsReport()
        {
            var reportData = _context.Shipments
                .GroupBy(s => new { s.ShipName, s.ShippingAgent, s.UnloadingStartDate })
                .Select(g => new ShipmentReportResultViewModel
                {
                    ShipName = g.Key.ShipName,
                    ShippingAgent = g.Key.ShippingAgent,
                    UnloadingStartDate = g.Key.UnloadingStartDate,
                    ArrivalDate = g.Max(x => x.ArrivalDate),
                    UnloadingCompletionDate = g.Max(x => x.UnloadingCompletionDate),

                    // Use ?? 0 for all decimal properties
                    TotalUnloadedAmount = g.Sum(x => x.UnloadedAmount) ?? 0,
                    CargoWeight = g.Max(x => x.CargoWeight) ?? 0,

                    CargoType = g.First().CargoType,
                    CargoOwner = g.First().CargoOwner,

                    // Fix the UnloadingPercentage calculation
                    UnloadingPercentage = (g.Max(x => x.CargoWeight) ?? 0) > 0 ?
                                          ((g.Sum(x => x.UnloadedAmount) ?? 0) / (g.Max(x => x.CargoWeight) ?? 1)) * 100 : 0,

                    Status = g.Max(x => x.UnloadingCompletionDate) != null ? "تکمیل شده" :
                           g.Key.UnloadingStartDate != null ? "در حال تخلیه" :
                           "در انتظار"
                })
                .OrderBy(s => s.ShipName)
                .ThenBy(s => s.UnloadingStartDate)
                .ToList();

            return View(reportData);
        }
    }
}