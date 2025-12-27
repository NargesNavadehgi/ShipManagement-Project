using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipManagement.Models
{
    public class Shipment
    {
        public int Id { get; set; }

        // فیلدهای اصلی
        public string? ShipName { get; set; }
        public string? ShippingAgent { get; set; }

        // فیلدهای تاریخ به صورت DateTime (برای دیتابیس) - همه Nullable
        public DateTime? RegistrationDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? UnloadingStartDate { get; set; }
        public DateTime? UnloadingCompletionDate { get; set; }
        public DateTime? DepartureDate { get; set; }

        // فیلدهای رشته‌ای برای دریافت تاریخ شمسی از کاربر
        [NotMapped]
        public string? ArrivalDateString { get; set; }

        [NotMapped]
        public string? UnloadingStartDateString { get; set; }

        [NotMapped]
        public string? UnloadingCompletionDateString { get; set; }

        [NotMapped]
        public string? DepartureDateString { get; set; }

        // سایر ویژگی‌ها - همه Nullable
        public decimal? CargoWeight { get; set; }
        public string? CargoType { get; set; }
        public string? CargoOwner { get; set; }
        public decimal? AddedValue { get; set; }
        public decimal? Tariff { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? OtherPaymentMethod { get; set; }
        public decimal? EquivalentAmount { get; set; }
        public string? Dockworker { get; set; }
        public string? UnloadingPermitNumber { get; set; }
        public string? VoyageNumber { get; set; }
        public decimal? UnloadedAmount { get; set; }


    }
}