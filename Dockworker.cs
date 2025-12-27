// در پوشه Models
using System.ComponentModel.DataAnnotations;
namespace ShipManagement.Models
{
    public class Dockworker
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "نام خنکار")]
        public string Name { get; set; }
        // در مدل Dockworker
        [Display(Name = "توضیحات")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "فعال")]
        public bool IsActive { get; set; } = true;
    }
}