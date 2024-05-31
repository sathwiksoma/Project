using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotPotProject.Models
{
    public class CustomerReview
    {
        [Key]
        public int ReviewId { get; set; }
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public int Rating { get; set; }
        public string TextReview { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        [ForeignKey("RestaurantId")]
        public Restaurant? Restaurant { get; set; }
        public CustomerReview()
        {

        }

        public CustomerReview(int reviewId, int userId, int restaurantId, int rating, string textReview)
        {
            ReviewId = reviewId;
            CustomerId = userId;
            RestaurantId = restaurantId;
            Rating = rating;
            TextReview = textReview;
        }

        public CustomerReview(int userId, int restaurantId, int rating, string textReview)
        {
            CustomerId = userId;
            RestaurantId = restaurantId;
            Rating = rating;
            TextReview = textReview;
        }
    }
}
