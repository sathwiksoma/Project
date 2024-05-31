using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotPotProject.Models
{
    public class RestaurantOwner
    {
        [Key]
        public int OwnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public Restaurant Restaurant { get; set; }
        public string UserName { get; set; }
        [ForeignKey("UserName")]
        public User? User { get; set; }

        public RestaurantOwner()
        {

        }

        public RestaurantOwner(string name, int restaurantId, string userName)
        {
            Name = name;
            RestaurantId = restaurantId;
            UserName = userName;
        }

        public RestaurantOwner(int ownerId, string name, int restaurantId, string userName)
        {
            OwnerId = ownerId;
            Name = name;
            RestaurantId = restaurantId;
            UserName = userName;
        }
    }
}
