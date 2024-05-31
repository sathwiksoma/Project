using HotPotProject.Models.DTO;
using HotPotProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotPotProject.Interfaces
{
    public interface ICustomerServices
    {
        public Task<LoginUserDTO> RegisterCustomer(RegisterCustomerDTO registerCustomer);
        public Task<LoginUserDTO> LogIn(LoginUserDTO loginCustomer);
        public Task<List<Restaurant>> GetRestaurantsByCity(string city);
        public Task<List<Menu>> GetMenuByRestaurant(int RestaurantId);
        public Task<Restaurant> GetRestaurantByName(string name);
        public Task<OrderMenuDTO> PlaceOrder(int customerId, string paymentMode);
        public Task<OrderMenuDTO> PlaceOrderForOne(int cartItemId, string paymentMode);
        public Task<Payment> RecordPayment(Order order);
        public  Task<int> AddToCart(int userId, int menuItemId);
        public Task<List<CartMenuDTO>> GetCarts(int customerId);
        public Task DeleteCartItem(int cartItemId);
        public Task EmptyCart(int customerId);
        public Task IncreaseCartItemQuantity(int cartId);
        public Task DecreaseCartItemQuantity(int cartId);
        public Task<OrderMenuDTO> ViewOrderStatus(int orderId);
        public Task<List<OrderMenuDTO>> ViewOrderHistory(int customerId);
        public Task<Customer> GetCustomerDetails(int customerId);
        public Task<Customer> UpdateCustomerDetails(Customer customer);
        public Task<List<City>> GetAllCities();
        public Task<Order> CancelOrderFromCustomer(int orderId);
        //set 2
        public Task<CustomerAddress> AddCustomerAddress(CustomerAddress customerAddress);
        public Task<CustomerAddress> UpdateCustomerAddress(int addressId, CustomerAddressUpdateDTO addressUpdateDto);
        public Task<CustomerAddress> ViewCustomerAddressByCustomerId(int customerId);
        public Task<CustomerReview> AddCustomerReview(CustomerReview customerReview);
        public Task<CustomerReview> ViewCustomerReview(int customerReviewId);
        public Task<CustomerReview> UpdateCustomerReviewText(CustomerReviewUpdateDTO reviewUpdateDTO);
        public Task<CustomerReview> DeleteCustomerReview(int reviewId);
        public Task<List<Menu>> SearchMenu(int restaurantId, string query);
        public Task<List<Menu>> FilterMenuByPriceRange(int restaurantId, float minPrice, float maxPrice);
        public Task<List<Menu>> FilterMenuByType(int restaurantId, string type);
        public Task<List<Menu>> FilterMenuByCuisine(int restaurantId, string cuisine);
        public Task<IActionResult> GetCustomerByUsername(string username);
    }
}
