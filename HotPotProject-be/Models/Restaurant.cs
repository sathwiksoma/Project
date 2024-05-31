using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotPotProject.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int CityId { get; set; }
        [ForeignKey("CityId")]
        public City? City { get; set; }
        public string? RestaurantImage { get; set; }

        public Restaurant()
        {

        }

        public Restaurant(int restaurantId, string restaurantName, string phone, string email, int cityId, string? restaurantImage)
        {
            RestaurantId = restaurantId;
            RestaurantName = restaurantName;
            Phone = phone;
            Email = email;
            CityId = cityId;
            RestaurantImage = restaurantImage;
        }

        public Restaurant(string restaurantName, string phone, string email, int cityId, string? restaurantImage)
        {
            RestaurantName = restaurantName;
            Phone = phone;
            Email = email;
            CityId = cityId;
            RestaurantImage = restaurantImage;
        }

    }
}
