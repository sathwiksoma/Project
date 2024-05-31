using HotPotProject.Models.DTO;
using HotPotProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotPotProject.Interfaces
{
    public interface IRestaurantOwnerServices : IRestaurantAdminServices
    {
        public Task<LoginUserDTO> RegisterRestaurant(RegisterRestaurantDTO registerRestaurant);
        public Task<LoginUserDTO> LogInRestaurant(LoginUserDTO loginRestaurant);
        public Task<Menu> AddMenuItem(Menu menu);
        public Task<List<Payment>> GetAllPayments(int RestaurantId);
        public Task<List<Order>> GetAllOrders(int RestaurantId);
        public Task<Order> ChangeOrderStatus(int orderId, string newStatus);
        public Task<Menu> DeleteMenuItem(int menuItemId);
    }
}
