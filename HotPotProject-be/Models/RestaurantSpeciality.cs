using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotPotProject.Models
{
    public class RestaurantSpeciality
    {
        [Key]
        public int CategoryId { get; set; }
        public int RestaurantId { get; set; }
        public string CategoryName { get; set; }

        [ForeignKey("RestaurantId")]
        public Restaurant? Restaurant { get; set; }
        public string? CategoryImage { get; set; }

        public RestaurantSpeciality()
        {

        }

        public RestaurantSpeciality(int categoryId, int restaurantId, string categoryName, string? categoryImage)
        {
            CategoryId = categoryId;
            RestaurantId = restaurantId;
            CategoryName = categoryName;
            CategoryImage = categoryImage;
        }
    }
}
