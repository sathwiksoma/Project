using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotPotProject.Models
{
    public class Menu
    {
        [Key]
        public int MenuId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public float Price { get; set; }
        public string? Description { get; set; } = string.Empty;
        public string? Cuisine { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;

        public TimeSpan? CookingTime { get; set; }
        public string? TasteInfo { get; set; } = string.Empty;
       
        public string? ItemImage { get; set; } 
        public int NutritionId { get; set; }
        [ForeignKey("NutritionId")]
        public NutritionalInfo? NutritionalInfo { get; set; }
        public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public Restaurant? Restaurant { get; set; }

        public Menu()
        {

        }

        //public Menu(string name, string type, float price, string? description, string cuisine, TimeSpan? cookingTime, string? tasteInfo, string itemImage, int nutritionId, int restaurantId)
        //{
        //    Name = name;
        //    Type = type;
        //    Price = price;
        //    Description = description;
        //    Cuisine = cuisine;
        //    CookingTime = cookingTime;
        //    TasteInfo = tasteInfo;
        //    ItemImage = itemImage;
        //    NutritionId = nutritionId;
        //    RestaurantId = restaurantId;
        //}

        //public Menu(int menuId, string name, string type, float price, string? description, string cuisine, TimeSpan? cookingTime, string? tasteInfo, IFormFile itemImage, int nutritionId, int restaurantId)
        //{
        //    MenuId = menuId;
        //    Name = name;
        //    Type = type;
        //    Price = price;
        //    Description = description;
        //    Cuisine = cuisine;
        //    CookingTime = cookingTime;
        //    TasteInfo = tasteInfo;
        //    ItemImage = itemImage;
        //    NutritionId = nutritionId;
        //    RestaurantId = restaurantId;
        //}

        public bool Equals(Menu? other)
        {
            var menu = other ?? new Menu();
            return this.MenuId.Equals(menu.MenuId);
        }
    }
}
