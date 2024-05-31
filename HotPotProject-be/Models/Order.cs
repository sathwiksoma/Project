using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotPotProject.Models
{
    public class Order : IEquatable<Order>
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public float Amount { get; set; }
        public string Status { get; set; } = string.Empty; //"created", "placed", "accepted", "being prepared", "handed", "delivered", "dropped"
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public Restaurant? Restaurant { get; set; }

        public int? PartnerId { get; set; }
        [ForeignKey("PartnerId")]
        public DeliveryPartner? DeliveryPartner { get; set; }

        public Order()
        {
            OrderId = 0;
        }

        public Order(DateTime orderDate, float amount, string status, int userId, int restaurantId, int partnerId)
        {
            OrderDate = orderDate;
            Amount = amount;
            Status = status;
            CustomerId = userId;
            RestaurantId = restaurantId;
            PartnerId = partnerId;
        }

        public Order(int orderId, DateTime orderDate, float amount, string status, int userId, int restaurantId, int partnerId)
        {
            OrderId = orderId;
            OrderDate = orderDate;
            Amount = amount;
            Status = status;
            CustomerId = userId;
            RestaurantId = restaurantId;
            PartnerId = partnerId;
        }

        public bool Equals(Order? other)
        {
            var order = other ?? new Order();
            return this.OrderId.Equals(order.OrderId);
        }
    }
}
