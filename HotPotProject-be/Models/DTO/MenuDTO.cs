namespace HotPotProject.Models.DTO
{
    public class MenuDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public float Price { get; set; }
        public string? Description { get; set; } = string.Empty;
        public string? Cuisine { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;

        public TimeSpan? CookingTime { get; set; }
        public string? TasteInfo { get; set; } = string.Empty;

        public IFormFile ItemImage { get; set; }
        public int NutritionId {  get; set; }
        public int RestaurantId {  get; set; }
    }
}
