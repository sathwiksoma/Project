using HotPotProject.Models;

namespace HotPotProject.Interfaces
{
    public interface IRestaurantAdminServices
    {
        public Task<Restaurant> AddRestaurant(Restaurant restaurant);
        public Task<List<Payment>> GetAllPayments();
        public Task<List<Order>> GetAllOrders();
    }
}
