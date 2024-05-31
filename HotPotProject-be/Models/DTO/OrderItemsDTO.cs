namespace HotPotProject.Models.DTO
{
    public class OrderItemsDTO
    {
        public Order Order { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
