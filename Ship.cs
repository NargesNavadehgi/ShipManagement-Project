using System.ComponentModel.DataAnnotations;

namespace ShipManagement.Models
{
    public class Ship
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "نام کشتی الزامی است")]
        [Display(Name = "نام کشتی")]
        public string Name { get; set; }

        [Display(Name = "کشور صاحب کشتی")]
        public string ?Country { get; set; }

        [Display(Name = "ظرفیت بار (تن)")]
        public decimal? Capacity { get; set; }

        [Display(Name = "سال ساخت")]
        public int? BuildYear { get; set; }
    }
}
