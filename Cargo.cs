using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipManagement.Models
{
    public class Cargo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "نام کالا الزامی است")]
        [Display(Name = "نام کالا")]
        [StringLength(100, ErrorMessage = "نام کالا نمی‌تواند بیش از 100 کاراکتر باشد")]
        public string Name { get; set; }

        [Display(Name = "وزن (تن)")]
        [Range(0.1, double.MaxValue, ErrorMessage = "وزن باید بیشتر از 0 باشد")]
        public double Weight { get; set; }

        [Display(Name = "توضیحات")]
        [StringLength(500, ErrorMessage = "توضیحات نمی‌تواند بیش از 500 کاراکتر باشد")]
        public string? Description { get; set; }

        // Foreign keys
        [Display(Name = "نوع کالا")]
        public int CargoTypeId { get; set; }

        [Display(Name = "صاحب کالا")]
        public int CargoOwnerId { get; set; }

        // Navigation properties
        [ForeignKey("CargoTypeId")]
        public virtual CargoType? CargoType { get; set; }

        [ForeignKey("CargoOwnerId")]
        public virtual CargoOwner? CargoOwner { get; set; }
    }
}