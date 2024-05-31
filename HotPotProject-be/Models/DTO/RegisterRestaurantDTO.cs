namespace HotPotProject.Models.DTO
{
    public class RegisterRestaurantDTO
    {
        public string Name { get; set; }
        public int RestaurantId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
