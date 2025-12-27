
namespace ShipManagement.Models
{
    public class ShipmentReportViewModel
    {
        public string ShipName { get; set; }
        public string ShippingAgent { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? UnloadingStartDate { get; set; }
        public DateTime? UnloadingCompletionDate { get; set; }
        public decimal? TotalUnloadedAmount { get; set; }
        public decimal? TotalCargoWeight { get; set; }
        public decimal? ShipCapacity { get; set; }
        public decimal? CargoWeight { get; set; }
        public string CargoType { get; set; }
        public string CargoOwner { get; set; }
        public double UnloadedAmount { get; set; }
    }
}