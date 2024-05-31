using System.ComponentModel.DataAnnotations.Schema;

namespace HotPotProject.Models
{
    public class OrderItem : IEquatable<OrderItem>
    {
        public int Quantity { get; set; }
        public float SubTotalPrice { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
        public int MenuId { get; set; }
        [ForeignKey("MenuId")]
        public Menu? Menu { get; set; }

        public OrderItem()
        {

        }

        public OrderItem(int quantity, float subTotalPrice, int orderId, int menuId)
        {
            Quantity = quantity;
            SubTotalPrice = subTotalPrice;
            OrderId = orderId;
            MenuId = menuId;
        }

        public OrderItem(int orderItemId, int quantity, float subTotalPrice, int orderId, int menuId)
        {
            Quantity = quantity;
            SubTotalPrice = subTotalPrice;
            OrderId = orderId;
            MenuId = menuId;
        }

        public bool Equals(OrderItem? other)
        {
            var orderItem = other ?? new OrderItem();
            return this.OrderId.Equals(orderItem.OrderId);
        }
    }
}
