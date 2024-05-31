namespace HotPotProject.Models.DTO
{
    public class OrderMenuDTO
    {
        public int orderId { get; set; }
        public int customerId { get; set; }
        public int restaurantId { get; set; }
        public string? RestaurantName { get; set; }
        public string? RestaurantImage { get; set; }
        public List<MenuNameDTO> menuName { get; set; }
        public float Price { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public int? partnerId { get; set; }
        public string? PartnerName { get; set; }
    }
}
