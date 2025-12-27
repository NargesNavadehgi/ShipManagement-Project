using Microsoft.EntityFrameworkCore;
using ShipManagement.Data;
using ShipManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipManagement.Services
{
    public class ShipmentReportService
    {
        private readonly ApplicationDbContext _context;

        public ShipmentReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShipmentReportViewModel>> GetGroupedShipmentReport()
        {
            var query = @"
                SELECT 
                    ShipName,
                    ShippingAgent,
                    MAX(ArrivalDate) as ArrivalDate,
                    UnloadingStartDate,
                    MAX(UnloadingCompletionDate) as UnloadingCompletionDate,
                    SUM(UnloadedAmount) as TotalUnloadedAmount,
                    MAX(CargoWeight) as CargoWeight,
                    MAX(CargoType) as CargoType,
                    MAX(CargoOwner) as CargoOwner,
                    CASE 
                        WHEN MAX(CargoWeight) > 0 THEN 
                            (SUM(UnloadedAmount) / MAX(CargoWeight)) * 100 
                        ELSE 0 
                    END as UnloadingPercentage,
                    CASE 
                        WHEN MAX(UnloadingCompletionDate) IS NOT NULL THEN N'تکمیل شده'
                        WHEN UnloadingStartDate IS NOT NULL THEN N'در حال تخلیه'
                        ELSE N'در انتظار'
                    END as Status
                FROM Shipments
                GROUP BY ShipName, ShippingAgent, UnloadingStartDate
                ORDER BY ShipName, UnloadingStartDate";

            return await _context.Database.SqlQueryRaw<ShipmentReportViewModel>(query).ToListAsync();
        }

        public async Task<decimal> GetDistinctCargoWeightSum()
        {
            try
            {

                // یا کوئری ساده‌تر
                var query3 = @"
            SELECT ISNULL(SUM(CargoWeight), 0)
            FROM (
                SELECT ShipName, MAX(CargoWeight) as CargoWeight
                FROM Shipments
                WHERE ISNULL(CargoWeight, 0) > 0
                GROUP BY ShipName
            ) as MaxWeights";

                var result = await _context.Database.SqlQueryRaw<decimal>(query3).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                // برای دیباگ
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }
    }
}