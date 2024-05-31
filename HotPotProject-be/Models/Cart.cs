using System.ComponentModel.DataAnnotations.Schema;

namespace HotPotProject.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public int RestaurantId { get; set; }

        [ForeignKey("RestaurantId")]
        public Restaurant Restaurant { get; set; }
        public int MenuItemId { get; set; }

        [ForeignKey("MenuItemId")]
        public Menu Menu { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } //"added", "purchased", "deleted"

        public Cart()
        {

        }

        public Cart(int customerId, int restaurantId, int menuItemId, int quantity, string status)
        {
            CustomerId = customerId;
            RestaurantId = restaurantId;
            MenuItemId = menuItemId;
            Quantity = quantity;
            Status = status;
        }

        public Cart(int id, int customerId, int restaurantId, int menuItemId, int quantity, string status)
        {
            Id = id;
            CustomerId = customerId;
            RestaurantId = restaurantId;
            MenuItemId = menuItemId;
            Quantity = quantity;
            Status = status;
        }
    }
}
