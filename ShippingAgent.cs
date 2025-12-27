// در پوشه Models فایل ShippingAgent.cs
namespace ShipManagement.Models
{
    public class ShippingAgent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}