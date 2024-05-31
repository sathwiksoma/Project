using System.ComponentModel.DataAnnotations;

namespace HotPotProject.Models
{
    public class DeliveryPartner
    {
        [Key]
        public int PartnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; }
        public string Email { get; set; } = string.Empty;
        public int CityId { get; set; }
        public string UserName { get; set; } = string.Empty;

        public DeliveryPartner()
        {
            PartnerId = 0;
        }

        public DeliveryPartner(int partnerid, string name, string phone, string email)
        {
            PartnerId = partnerid;
            Name = name;
            Phone = phone;
            Email = email;
        }

        public DeliveryPartner(string name, string phone, string email)
        {
            Name = name;
            Phone = phone;
            Email = email;
        }

        public bool Equals(DeliveryPartner? other)
        {
            var deliveryPartner = other ?? new DeliveryPartner();
            return this.PartnerId.Equals(deliveryPartner.PartnerId);
        }
    }
}
