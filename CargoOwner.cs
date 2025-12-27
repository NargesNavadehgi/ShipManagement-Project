using System.ComponentModel.DataAnnotations;

namespace ShipManagement.Models
{
    public class CargoOwner
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "نام صاحب کالا الزامی است")]
        [Display(Name = "نام صاحب کالا")]
        public string Name { get; set; }

        [Display(Name = "شماره تماس")]
        public string ?PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "فرمت ایمیل نامعتبر است")]
        [Display(Name = "آدرس ایمیل")]
        public string ?Email { get; set; }

        [Display(Name = "آدرس")]
        public string ?Address { get; set; }
    }
}
