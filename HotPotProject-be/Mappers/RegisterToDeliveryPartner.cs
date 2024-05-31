using HotPotProject.Models.DTO;
using HotPotProject.Models;

namespace HotPotProject.Mappers
{
    public class RegisterToDeliveryPartner
    {
        DeliveryPartner newDeliveryPartner;

        public RegisterToDeliveryPartner(RegisterDeliveryPartnerDTO deliveryPartner)
        {
            newDeliveryPartner = new DeliveryPartner();
            newDeliveryPartner.Name = deliveryPartner.Name;
            newDeliveryPartner.Email = deliveryPartner.Email;
            newDeliveryPartner.Phone = deliveryPartner.Phone;
            newDeliveryPartner.CityId = deliveryPartner.cityId;
            newDeliveryPartner.UserName = deliveryPartner.UserName;
        }

        public DeliveryPartner GetDeliveryPartner()
        {
            return newDeliveryPartner;
        }
    }
}
