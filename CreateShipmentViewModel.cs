using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShipManagement.Models
{
    public class CreateShipmentViewModel
    {
        // تمام پراپرتی‌ها بدون Required و Validation
        public int? Id { get; set; }
        public string ShipName { get; set; } // حذف [Required]
        public string ShippingAgent { get; set; } // حذف [Required]
        public string ArrivalDateString { get; set; }
        public string Dockworker { get; set; }
        public decimal? CargoWeight { get; set; } // حذف [Range]
        public string CargoType { get; set; }
        public string CargoOwner { get; set; }
        public decimal? Tariff { get; set; }
        public decimal? AddedValue { get; set; }
        public decimal? TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? EquivalentAmount { get; set; }
        public string OtherPaymentMethod { get; set; }
        public string UnloadingStartDateString { get; set; }
        public string UnloadingCompletionDateString { get; set; }
        public decimal? UnloadedAmount { get; set; }
        public string UnloadingPermitNumber { get; set; }
        public string VoyageNumber { get; set; }
        public string DepartureDateString { get; set; }

        // پراپرتی‌های برای چک‌باکس‌ها
        public List<string> SelectedOperationTypes { get; set; } = new List<string>();

        public List<SelectListItem> AvailableOperationTypes { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "تخلیه", Text = "تخلیه" },
            new SelectListItem { Value = "حمل", Text = "حمل" },
            new SelectListItem { Value = "نظافت", Text = "نظافت" },
            new SelectListItem { Value = "بارگیری", Text = "بارگیری" },
            new SelectListItem { Value = "انبارداری", Text = "انبارداری" }
        };

        // پراپرتی‌های برای dropdownها
        public SelectList ShipNames { get; set; }
        public SelectList CargoOwners { get; set; }
        public SelectList ShippingAgents { get; set; }
        public SelectList Dockworkers { get; set; }
        public SelectList CargoTypes { get; set; }
        public SelectList PaymentMethods { get; set; }
    }
}