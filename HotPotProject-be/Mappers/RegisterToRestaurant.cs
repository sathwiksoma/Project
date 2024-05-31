using HotPotProject.Models.DTO;
using HotPotProject.Models;

namespace HotPotProject.Mappers
{
    public class RegisterToRestaurant
    {
        RestaurantOwner restaurantOwner;
        public RegisterToRestaurant(RegisterRestaurantDTO registerRestaurant)
        {
            restaurantOwner = new RestaurantOwner();
            restaurantOwner.Name = registerRestaurant.Name;
            restaurantOwner.UserName = registerRestaurant.UserName;
            restaurantOwner.RestaurantId = registerRestaurant.RestaurantId;
        }
        public RestaurantOwner GetRestaurantOwner()
        {
            return restaurantOwner;
        }
    }
}
