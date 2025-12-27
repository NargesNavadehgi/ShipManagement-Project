using System.ComponentModel.DataAnnotations;

namespace ShipManagement.Models
{
    public class CargoType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "نام نوع کالا الزامی است")]
        [Display(Name = "نام نوع کالا")]
        public string Name { get; set; }

        [Display(Name = "توضیحات")]
        public string ?Description { get; set; }

        [Display(Name = "واحد اندازه‌گیری")]
        public string ?Unit { get; set; }

        [Display(Name = "حداکثر وزن مجاز")]
        public decimal? MaxWeight { get; set; }

        public bool IsActive { get; set; } = true;
    }
}