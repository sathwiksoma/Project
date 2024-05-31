using HotPotProject.Exceptions;
using HotPotProject.Interfaces;
using HotPotProject.Models.DTO;
using HotPotProject.Models;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using HotPotProject.Mappers;

namespace HotPotProject.Services
{
    public class RestaurantUserServices : IRestaurantUserServices
    {
        private readonly IRepository<int, String, Restaurant> _restaurantRepo;
        private readonly IRepository<int, String, City> _cityRepo;
        private readonly IRepository<int, string, Menu> _menuRepo;
        private readonly IRepository<int, string, Payment> _paymentRepo;
        private readonly IRepository<int, string, Order> _orderRepo;
        private readonly IRepository<int, string, User> _userRepo;
        private readonly IRepository<int, string, RestaurantOwner> _restOwnerRepo;
        private readonly IRepository<int, string, RestaurantSpeciality> _specialityRepo;
        private readonly IRepository<int, string, CustomerReview> _reviewRepo;
        private readonly ITokenServices _tokenServices;
        private ILogger<RestaurantUserServices> _logger;

        //[ExcludeFromCodeCoverage]
        public RestaurantUserServices(IRepository<int, String, Restaurant> restaurantRepo,
                                      IRepository<int, String, City> cityRepo,
                                      IRepository<int, String, Menu> menuRepo,
                                      IRepository<int, string, Payment> paymentRepo,
                                      IRepository<int, string, Order> orderRepo,
                                      IRepository<int, string, User> userRepo,
                                      IRepository<int, string, RestaurantOwner> restOwnerRepo,
                                      IRepository<int, string, RestaurantSpeciality> specialityRepo,
                                      IRepository<int, string, CustomerReview> reviewRepo,
                                      ITokenServices tokenServices,
                                      ILogger<RestaurantUserServices> logger)
        {
            _restaurantRepo = restaurantRepo;
            _cityRepo = cityRepo;
            _menuRepo = menuRepo;
            _paymentRepo = paymentRepo;
            _orderRepo = orderRepo;
            _userRepo = userRepo;
            _restOwnerRepo = restOwnerRepo;
            _specialityRepo = specialityRepo;
            _reviewRepo = reviewRepo;
            _tokenServices = tokenServices;
            _logger = logger;
        }

        public async Task<Menu> AddMenuItem(Menu menu)
        {
            var restaurant = await _restaurantRepo.GetAsync(menu.RestaurantId);
            if (restaurant != null)
            {
                var newItem = await _menuRepo.Add(menu);
                return newItem;
            }
            throw new RestaurantNotFoundException();
        }

        public async Task<Restaurant> AddRestaurant(Restaurant restaurant)
        {
            restaurant = await _restaurantRepo.Add(restaurant);
            return restaurant;
        }

        public async Task<Order> ChangeOrderStatus(int orderId, string newStatus)
        {
            var order = await _orderRepo.GetAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus.ToLower();
                order = await _orderRepo.Update(order);
                return order;
            }
            throw new OrdersNotFoundException();
        }

        public async Task<List<Order>> GetAllOrders(int RestaurantId)
        {
            var orders = await _orderRepo.GetAsync();
            var ordersForRestaurant = orders.Where(o => o.RestaurantId == RestaurantId).ToList();
            if (ordersForRestaurant != null || ordersForRestaurant.Count > 0)
                return ordersForRestaurant;
            throw new OrdersNotFoundException();
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var orders = await _orderRepo.GetAsync();
            if (orders != null || orders.Count > 0)
                return orders;
            throw new OrdersNotFoundException();
        }

        public async Task<List<Payment>> GetAllPayments(int RestaurantId)
        {
            var payments = await _paymentRepo.GetAsync();
            var orders = await _orderRepo.GetAsync();
            List<Payment> paymentsForRestaurant = (from payment in payments
                                                   join order in orders on payment.OrderId equals order.OrderId
                                                   where order.RestaurantId == RestaurantId
                                                   select payment).ToList();
            if (paymentsForRestaurant != null || paymentsForRestaurant.Count > 0)
                return paymentsForRestaurant;
            throw new PaymentsNotFoundException();
        }

        public async Task<List<Payment>> GetAllPayments()
        {
            var payments = await _paymentRepo.GetAsync();
            if (payments != null || payments.Count > 0)
                return payments;
            throw new PaymentsNotFoundException();
        }

        public async Task<LoginUserDTO> LogInRestaurant(LoginUserDTO loginRestaurant)
        {
            var user = await _userRepo.GetAsync(loginRestaurant.UserName);
            if (user == null)
                throw new InvalidUserException();
            var owners = await _restOwnerRepo.GetAsync();
            var owner = owners.FirstOrDefault(o => o.UserName == loginRestaurant.UserName);
            var password = getEncryptedPassword(loginRestaurant.Password, user.Key);
            bool matchPassword = passwordMatch(password, user.Password);
            if (matchPassword)
            {
                loginRestaurant.Password = "";
                loginRestaurant.Role = user.Role;
                loginRestaurant.UserId = owner.RestaurantId;
                loginRestaurant.Token = await _tokenServices.GenerateToken(loginRestaurant);
                return loginRestaurant;
            }
            throw new InvalidUserException();
        }

        [ExcludeFromCodeCoverage]
        public bool passwordMatch(byte[] password, byte[] userPassword)
        {
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] != userPassword[i])
                    return false;
            }
            return true;
        }

        [ExcludeFromCodeCoverage]
        public byte[] getEncryptedPassword(string password, byte[] key)
        {
            HMACSHA512 hmac = new HMACSHA512(key);
            var userPwd = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return userPwd;
        }

        public async Task<LoginUserDTO> RegisterRestaurant(RegisterRestaurantDTO registerRestaurant)
        {
            registerRestaurant.Role = "RestaurantOwner";
            User newUser = new RegisterToRestaurantUser(registerRestaurant).getUser();
            newUser = await _userRepo.Add(newUser);
            var newRestaurantOwner = new RegisterToRestaurant(registerRestaurant).GetRestaurantOwner();
            newRestaurantOwner = await _restOwnerRepo.Add(newRestaurantOwner);
            LoginUserDTO result = new LoginUserDTO
            {
                UserName = newUser.UserName,
                Role = newUser.Role
            };
            return result;
        }

        public async Task<List<RestaurantSpeciality>> GetAllSpecialities()
        {
            var specialities = await _specialityRepo.GetAsync();
            return specialities;
        }
        public async Task<RestaurantSpeciality> AddRestaurantSpeciality(RestaurantSpeciality restaurantspeciality)
        {
            // Check if the restaurant exists
            var restaurant = await _restaurantRepo.GetAsync(restaurantspeciality.RestaurantId);
            if (restaurant == null)
            {
                throw new RestaurantNotFoundException("Restaurant not found.");
            }

            // Add the restaurant speciality
            var addedSpeciality = await _specialityRepo.Add(restaurantspeciality);

            // Return the added restaurant speciality
            return addedSpeciality;
        }

        public async Task<List<CustomerReview>> GetCustomerReviews()
        {
            var reviews = await _reviewRepo.GetAsync();
            if (reviews == null)
                throw new NoCustomerReviewFoundException();
            return reviews;
        }

        public async Task<Menu> DeleteMenuItem(int menuItemId)
        {
            var menuItem = await _menuRepo.GetAsync(menuItemId);
            if (menuItem == null)
                throw new NoMenuAvailableException();
            menuItem = await _menuRepo.Delete(menuItemId);
            return menuItem;
        }
        public async Task<Order> DeleteOrder(int orderId)
        {
            // Check if the order exists
            var order = await _orderRepo.GetAsync(orderId);
            if (order == null)
            {
                throw new OrdersNotFoundException("Order not found.");
            }

            // Delete the order
            var deletedOrder = await _orderRepo.Delete(orderId);

            // Return the deleted order
            return deletedOrder;
        }
        public async Task<List<Restaurant>> GetAllRestaurants()
        {
            var restaurants = await _restaurantRepo.GetAsync();
            return restaurants;
        }
        public async Task<List<Menu>> GetAllMenus()
        {
            var menus = await _menuRepo.GetAsync();
            return menus;
        }
        public async Task<bool> DeleteRestaurant(int restaurantId)
        {
            // Check if the restaurant exists
            var restaurant = await _restaurantRepo.GetAsync(restaurantId);
            if (restaurant == null)
            {
                // Throw an exception if the restaurant is not found
                throw new RestaurantNotFoundException("Restaurant not found.");
            }

            // Delete the restaurant
            var deletedRestaurant = await _restaurantRepo.Delete(restaurantId);

            // Return true to indicate successful deletion
            return true;
        }
        public async Task<RestaurantOwner> GetRestaurantOwnerByUsername(string username)
        {
            // Fetch restaurant owner by username from the repository
            var restaurantOwner = await _restOwnerRepo.GetAsync(username);
            if (restaurantOwner == null)
            {
                // If not found, throw an exception
                throw new RestaurantOwnerNotFoundException("Restaurant owner not found for the given username.");
            }
            return restaurantOwner;
        }
    }
}
