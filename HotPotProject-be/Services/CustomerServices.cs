using HotPotProject.Exceptions;
using HotPotProject.Interfaces;
using HotPotProject.Models.DTO;
using HotPotProject.Models;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using HotPotProject.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace HotPotProject.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly IRepository<int, string, Customer> _custRepo;
        private readonly IRepository<int, string, User> _userRepo;
        private readonly IRepository<int, string, Menu> _menuRepo;
        private readonly IRepository<int, string, Cart> _cartRepo;
        private readonly IRepository<int, string, Order> _orderRepo;
        private readonly IRepository<int, string, OrderItem> _orderItemRepo;
        private readonly IRepository<int, string, Payment> _paymentRepo;
        private readonly IRepository<int, string, Restaurant> _restaurantRepo;
        private readonly IRepository<int, string, City> _cityRepo;
        private readonly IRepository<int, string, CustomerAddress> _custAddressRepo;
        private readonly IRepository<int, string, CustomerReview> _custReviewRepo;
        private readonly IRepository<int, string, DeliveryPartner> _deliveryPartnerRepo;
        private readonly ITokenServices _tokenServices;
        private readonly ILogger<CustomerServices> _logger;

        public CustomerServices(IRepository<int, string, Customer> custRepo,
                                IRepository<int, string, User> userRepo,
                                IRepository<int, string, Menu> menuRepo,
                                IRepository<int, string, Cart> cartRepo,
                                IRepository<int, string, Order> orderRepo,
                                IRepository<int, string, OrderItem> orderItemRepo,
                                IRepository<int, string, Payment> paymentRepo,
                                IRepository<int, String, Restaurant> restaurantRepo,
                                IRepository<int, String, City> cityRepo,
                                IRepository<int, string, CustomerAddress> custAddressRepo,
                                IRepository<int, string, CustomerReview> custReviewRepo,
                                IRepository<int, string, DeliveryPartner> deliveryPartnerRepo,
                                ITokenServices tokenServices,
                                ILogger<CustomerServices> logger)
        {
            _custRepo = custRepo;
            _userRepo = userRepo;
            _menuRepo = menuRepo;
            _cartRepo = cartRepo;
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _paymentRepo = paymentRepo;
            _restaurantRepo = restaurantRepo;
            _cityRepo = cityRepo;
            _custAddressRepo = custAddressRepo;
            _custReviewRepo = custReviewRepo;
            _deliveryPartnerRepo = deliveryPartnerRepo;
            _tokenServices = tokenServices;
            _logger = logger;
        }

        [ExcludeFromCodeCoverage]
        public async Task<LoginUserDTO> LogIn(LoginUserDTO loginCustomer)
        {
            var user = await _userRepo.GetAsync(loginCustomer.UserName);
            var customers = await _custRepo.GetAsync();
            var customer = customers.FirstOrDefault(c => c.UserName == loginCustomer.UserName);
            if (user == null)
                throw new InvalidUserException();
            var password = getEncryptedPassword(loginCustomer.Password, user.Key);
            bool matchPassword = passwordMatch(password, user.Password);
            if (matchPassword)
            {
                loginCustomer.UserId = customer.Id;
                loginCustomer.Password = "";
                loginCustomer.Role = user.Role;
                loginCustomer.Token = await _tokenServices.GenerateToken(loginCustomer);
                return loginCustomer;
            }
            throw new InvalidUserException();
        }

        [ExcludeFromCodeCoverage]
        private bool passwordMatch(byte[] password, byte[] userPassword)
        {
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] != userPassword[i])
                    return false;
            }
            return true;
        }

        [ExcludeFromCodeCoverage]
        private byte[] getEncryptedPassword(string password, byte[] key)
        {
            HMACSHA512 hmac = new HMACSHA512(key);
            var userPwd = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return userPwd;
        }

        public async Task<LoginUserDTO> RegisterCustomer(RegisterCustomerDTO registerCustomer)
        {
            registerCustomer.Role = "Customer";
            User myUser = new RegisterToUser(registerCustomer).getUser();
            myUser = await _userRepo.Add(myUser);
            Customer myCustomer = new RegisterToCustomer(registerCustomer).GetCustomer();
            myCustomer = await _custRepo.Add(myCustomer);
            CustomerAddress customerAddress = new CustomerAddress()
            {
                CustomerId = myCustomer.Id,
                CityId = 1
            };
            customerAddress = await _custAddressRepo.Add(customerAddress);
            LoginUserDTO result = new LoginUserDTO
            {
                UserId = myCustomer.Id,
                UserName = myUser.UserName,
                Role = myUser.Role
            };
            return result;
        }

        public async Task<List<Menu>> GetMenuByRestaurant(int RestaurantId)
        {
            var menusItems = await _menuRepo.GetAsync();
            var menuForRestaurant = menusItems.Where(m => m.RestaurantId == RestaurantId).ToList();
            if (menuForRestaurant == null || menuForRestaurant.Count == 0)
                throw new NoMenuAvailableException();
            return menuForRestaurant;
        }

        public async Task<Restaurant> GetRestaurantByName(string name)
        {
            var restaurant = await _restaurantRepo.GetAsync(name);
            if (restaurant == null)
                throw new RestaurantNotFoundException();
            return restaurant;
        }

        public async Task<List<Restaurant>> GetRestaurantsByCity(string city)
        {
            var myCity = await _cityRepo.GetAsync(city);
            if (myCity == null)
            {
                throw new CityNotFoundException();
            }
            var restaurants = await _restaurantRepo.GetAsync();
            var myCityRestaurants = restaurants.Where(r => r.CityId == myCity.CityId).ToList();
            if (myCityRestaurants == null || myCityRestaurants.Count == 0)
                throw new RestaurantNotFoundException();
            return myCityRestaurants;
        }

        [ExcludeFromCodeCoverage]
        public async Task<OrderMenuDTO> PlaceOrder(int customerId, string paymentMode)
        {
            var carts = await _cartRepo.GetAsync();
            var cartItems = carts.Where(c => c.CustomerId == customerId && c.Status == "added").ToList();

            // Check if cartItems is not empty
            if (cartItems.Any())
            {
                float totalAmount = 0;
                foreach (var cart in cartItems)
                {
                    var menuItem = await _menuRepo.GetAsync(cart.MenuItemId);
                    totalAmount += menuItem.Price * cart.Quantity;
                }

                Order newOrder = new Order
                {
                    OrderDate = DateTime.Now,
                    Amount = totalAmount,
                    Status = "created",
                    CustomerId = cartItems[0].CustomerId,
                    RestaurantId = cartItems[0].RestaurantId
                };

                var restaurant = await _restaurantRepo.GetAsync(cartItems[0].RestaurantId);
                var deliveryPartners = await _deliveryPartnerRepo.GetAsync();
                var deliveryPartnersFromCity = deliveryPartners.Where(d => d.CityId == restaurant.CityId).ToList();
                Random random = new Random();

                if (deliveryPartnersFromCity.Any())
                {
                    var deliveryPartner = deliveryPartnersFromCity[random.Next(deliveryPartnersFromCity.Count)];
                    var payment = await RecordPayment(newOrder);

                    if (payment.Status == "successful")
                    {
                        newOrder.Status = "placed";
                        newOrder.PartnerId = deliveryPartner.PartnerId;
                        newOrder = await _orderRepo.Add(newOrder);

                        List<MenuNameDTO> names = new List<MenuNameDTO>();
                        foreach (var cart in cartItems)
                        {
                            var menuItem = await _menuRepo.GetAsync(cart.MenuItemId);
                            OrderItem newOrderItem = new OrderItem
                            {
                                OrderId = newOrder.OrderId,
                                MenuId = cart.MenuItemId,
                                SubTotalPrice = menuItem.Price * cart.Quantity,
                                Quantity = cart.Quantity
                            };
                            await _orderItemRepo.Add(newOrderItem);

                            cart.Status = "purchased";
                            await _cartRepo.Update(cart);

                            MenuNameDTO menuNameDTO = new MenuNameDTO
                            {
                                ManuItemName = menuItem.Name,
                                Quantity = cart.Quantity
                            };
                            names.Add(menuNameDTO);
                        }

                        payment.OrderId = newOrder.OrderId;
                        payment = await _paymentRepo.Update(payment);

                        OrderMenuDTO orderItems = new OrderMenuDTO
                        {
                            orderId = newOrder.OrderId,
                            customerId = newOrder.CustomerId,
                            restaurantId = newOrder.RestaurantId,
                            menuName = names,
                            Price = totalAmount,
                            Status = newOrder.Status,
                            partnerId = deliveryPartner.PartnerId,
                            PartnerName = deliveryPartner.Name
                        };

                        return orderItems;
                    }
                    else
                    {
                        throw new PaymentFailedException();
                    }
                }
                else
                {
                    throw new Exception("No delivery partners available for the restaurant's city.");
                }
            }
            else
            {
                throw new Exception("No items in the cart to place an order.");
            }
        }


        [ExcludeFromCodeCoverage]
        //The PlaceOrderForOne controller method in the controller layer invokes the PlaceOrderForOne method in the CustomerServices class,
        //passing the cartItemId and paymentMode as parameters. This method handles placing an order for a single item in the cart.
        public async Task<OrderMenuDTO> PlaceOrderForOne(int cartItemId, string paymentMode)
        {
            // Fetch cart item and corresponding menu item
            Cart cartItem = await _cartRepo.GetAsync(cartItemId);
            Menu menuItem = await _menuRepo.GetAsync(cartItem.MenuItemId);

            // Calculate total amount for the order
            float amount = menuItem.Price * cartItem.Quantity;

            // Create a new order object
            Order newOrder = new Order
            {
                OrderDate = DateTime.Now,
                Amount = amount,
                Status = "created",
                CustomerId = cartItem.CustomerId,
                RestaurantId = cartItem.RestaurantId,
                PartnerId = 0 // Initialize to 0 initially
            };

            // Fetch restaurant details to get the city ID
            Restaurant restaurant = await _restaurantRepo.GetAsync(cartItem.RestaurantId);
            int restaurantCityId = restaurant.CityId;

            // Fetch delivery partners from the same city
            var deliveryPartners = await _deliveryPartnerRepo.GetAsync();
            var deliveryPartnersFromCity = deliveryPartners.Where(d => d.CityId == restaurantCityId).ToList();

            // Select a random delivery partner from the list
            Random random = new Random();
            var deliveryPartner = deliveryPartnersFromCity[random.Next(deliveryPartnersFromCity.Count)];

            // Record payment
            var payment = await RecordPayment(newOrder);

            // Check if payment was successful
            if (payment.Status == "successful")
            {
                // Update order status to "placed" and add it to the database
                newOrder.Status = "placed";
                newOrder.PartnerId = deliveryPartner.PartnerId; // Assign partner ID
                newOrder = await _orderRepo.Add(newOrder);

                // Create a new order item and add it to the database
                OrderItem newOrderItem = new OrderItem
                {
                    OrderId = newOrder.OrderId,
                    MenuId = menuItem.MenuId,
                    Quantity = cartItem.Quantity,
                    SubTotalPrice = amount
                };
                newOrderItem = await _orderItemRepo.Add(newOrderItem);

                // Update payment with order ID
                payment.OrderId = newOrder.OrderId;
                payment = await _paymentRepo.Update(payment);

                // Update cart item status to "purchased"
                cartItem.Status = "purchased";
                cartItem = await _cartRepo.Update(cartItem);

                // Create a list of menu names DTO
                List<MenuNameDTO> names = new List<MenuNameDTO>();
                MenuNameDTO menuNameDTO = new MenuNameDTO
                {
                    ManuItemName = menuItem.Name,
                    Quantity = cartItem.Quantity
                };
                names.Add(menuNameDTO);

                // Create and return order DTO
                OrderMenuDTO orderItems = new OrderMenuDTO
                {
                    orderId = newOrder.OrderId,
                    customerId = newOrder.CustomerId,
                    restaurantId = newOrder.RestaurantId,
                    menuName = names,
                    Price = amount,
                    partnerId = deliveryPartner.PartnerId, // Assigning the partner ID
                    PartnerName = deliveryPartner.Name
                };
                return orderItems;
            }
            else
            {
                // If payment was not successful, throw PaymentFailedException
                throw new PaymentFailedException();
            }
        }


        public async Task<Payment> RecordPayment(Order order)
        {
            Payment payment = new Payment
            {
                PaymentMode = "online",
                Amount = order.Amount,
                Status = "successful",
                Date = DateTime.Now
            };
            payment = await _paymentRepo.Add(payment);
            return payment;
        }

        public async Task<int> AddToCart(int userId, int menuItemId)
        {
            var menuItem = await _menuRepo.GetAsync(menuItemId);
            if (menuItem != null)
            {
                var cartItems = await _cartRepo.GetAsync();
                var customerCart = cartItems.Where(c => c.CustomerId == userId && c.Status == "added").ToList();
                var checkMenuInCart = customerCart.FirstOrDefault(c => c.MenuItemId == menuItemId);
                if (checkMenuInCart == null)
                {
                    Cart cartItem = new Cart
                    {
                        CustomerId = userId,
                        RestaurantId = menuItem.RestaurantId,
                        MenuItemId = menuItemId,
                        Quantity = 1,
                        Status = "added"
                    };
                    cartItem = await _cartRepo.Add(cartItem);
                    return cartItem.Id; // Return just the CartId
                }
                await IncreaseCartItemQuantity(checkMenuInCart.Id);
                return checkMenuInCart.Id; // Return the existing CartId
            }
            throw new NoMenuAvailableException();
        }

        public async Task<List<CartMenuDTO>> GetCarts(int customerId)
        {
            var cartItems = await _cartRepo.GetAsync();
            var cartForCustomer = cartItems.Where(c => c.CustomerId == customerId).Where(c => c.Status == "added").ToList();
            var restaurant = await _restaurantRepo.GetAsync(cartForCustomer[0].RestaurantId);
            List<CartMenuDTO> cartMenus = new List<CartMenuDTO>();
            if (cartForCustomer != null || cartForCustomer.Count > 0)
            {
                foreach (var cartItem in cartForCustomer)
                {
                    var menuItem = await _menuRepo.GetAsync(cartItem.MenuItemId);
                    CartMenuDTO cartMenu = new CartMenuDTO
                    {
                        CartId = cartItem.Id,
                        CustomerId = cartItem.CustomerId,
                        RestaurantId = menuItem.RestaurantId,
                        MenuItemId = menuItem.MenuId,
                        MenuTitle = menuItem.Name,
                        Quantity = cartItem.Quantity,
                        Price = menuItem.Price * cartItem.Quantity,
                      //  ItemImage = menuItem.ItemImage,
                        RestaurantName = restaurant.RestaurantName,
                        RestaurantCityId = restaurant.CityId
                    };
                    cartMenus.Add(cartMenu);
                }
                if (cartMenus == null || cartMenus.Count == 0)
                    return null;
                else
                    return cartMenus;
            }
            throw new EmptyCartException();
        }

        public async Task DeleteCartItem(int cartItemId)
        {
            var cartItem = await _cartRepo.GetAsync(cartItemId);
            await _cartRepo.Delete(cartItem.Id);
        }

        public async Task EmptyCart(int customerId)
        {
            var cartItems = await _cartRepo.GetAsync();
            var cartForCustomer = cartItems.Where(c => c.CustomerId == customerId).Where(c => c.Status == "added").ToList();
            foreach (var cartItem in cartItems)
            {
                cartItem.Status = "deleted";
                await _cartRepo.Update(cartItem);
            }
        }

        public async Task IncreaseCartItemQuantity(int cartId)
        {
            var cartItem = await _cartRepo.GetAsync(cartId);
            cartItem.Quantity++;
            cartItem = await _cartRepo.Update(cartItem);
        }

        public async Task DecreaseCartItemQuantity(int cartId)
        {
            var cartItem = await _cartRepo.GetAsync(cartId);
            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
                cartItem = await _cartRepo.Update(cartItem);
            }
            else
            {
                await DeleteCartItem(cartId);
            }
        }

        public async Task<OrderMenuDTO> ViewOrderStatus(int orderId)
        {
            var order = await _orderRepo.GetAsync(orderId);
            if (order == null)
                throw new OrdersNotFoundException();

            var restaurant = await _restaurantRepo.GetAsync(order.RestaurantId);

            var orderitems = await _orderItemRepo.GetAsync();
            var orderItemsForCustomer = orderitems.Where(oi => oi.OrderId == orderId).ToList();

            List<MenuNameDTO> menuList = new List<MenuNameDTO>();
            float totalPrice = 0;

            foreach (var orderItem in orderItemsForCustomer)
            {
                var menu = await _menuRepo.GetAsync(orderItem.MenuId);
                MenuNameDTO menuNameDTO = new MenuNameDTO
                {
                    ManuItemName = menu.Name,
                    Quantity = orderItem.Quantity
                };
                menuList.Add(menuNameDTO);
                totalPrice += orderItem.SubTotalPrice;
            }

            OrderMenuDTO orderMenuDTO = new OrderMenuDTO
            {
                orderId = orderId,
                customerId = order.CustomerId,
                restaurantId = order.RestaurantId,
                menuName = menuList,
                Price = totalPrice,
                Status = order.Status,
                RestaurantName = restaurant.RestaurantName,
                RestaurantImage = restaurant.RestaurantImage,
                OrderDate = order.OrderDate
            };

            return orderMenuDTO;
        }

        public async Task<List<OrderMenuDTO>> ViewOrderHistory(int customerId)
        {
            var orders = await _orderRepo.GetAsync();
            var ordersForCustomers = orders.Where(o => o.CustomerId == customerId).ToList();
            var orderItems = await _orderItemRepo.GetAsync();

            List<OrderMenuDTO> orderHistory = new List<OrderMenuDTO>();

            foreach (var order in ordersForCustomers)
            {
                var restaurant = await _restaurantRepo.GetAsync(order.RestaurantId);
                var orderItemsForOrder = orderItems.Where(oi => oi.OrderId == order.OrderId).ToList();
                List<MenuNameDTO> menuNames = new List<MenuNameDTO>();
                float price = 0;
                foreach (var orderItem in orderItemsForOrder)
                {
                    var menu = await _menuRepo.GetAsync(orderItem.MenuId);
                    MenuNameDTO menuName = new MenuNameDTO
                    {
                        ManuItemName = menu.Name,
                        Quantity = orderItem.Quantity
                    };
                    menuNames.Add(menuName);
                    price += orderItem.SubTotalPrice;
                }
                OrderMenuDTO orderMenu = new OrderMenuDTO
                {
                    orderId = order.OrderId,
                    customerId = customerId,
                    restaurantId = order.RestaurantId,
                    menuName = menuNames,
                    Price = price,
                    Status = order.Status,
                    RestaurantName = restaurant.RestaurantName,
                    RestaurantImage = restaurant.RestaurantImage,
                    OrderDate = order.OrderDate,
                    partnerId = order.PartnerId
                };
                orderHistory.Add(orderMenu);
            }

            if (orderHistory == null || orderHistory.Count() == 0)
                throw new OrdersNotFoundException();
            return orderHistory;
        }

        public async Task<Customer> GetCustomerDetails(int customerId)
        {
            var customer = await _custRepo.GetAsync(customerId);
            if (customer == null)
                throw new NoUsersAvailableException();
            return customer;
        }
        public async Task<Customer> UpdateCustomerDetails(Customer customer)
        {
            var myCustomer = await _custRepo.GetAsync(customer.Id);
            if (myCustomer == null)
                throw new NoUsersAvailableException();
            myCustomer = await _custRepo.Update(customer);
            return myCustomer;
        }

        public async Task<List<City>> GetAllCities()
        {
            var cities = await _cityRepo.GetAsync();
            if (cities == null)
                throw new CityNotFoundException();
            return cities;
        }

        public async Task<Order> CancelOrderFromCustomer(int orderId)
        {
            var order = await _orderRepo.GetAsync(orderId);
            if (order == null)
                throw new OrdersNotFoundException();
            order.Status = "cancelled";
            order = await _orderRepo.Update(order);
            return order;
        }

        //Method set 2
        public async Task<CustomerAddress> AddCustomerAddress(CustomerAddress customerAddress)
        {
            var myCustomerAddress = await _custAddressRepo.Add(customerAddress);
            _logger.LogInformation($"Customer Address added successfully. AddressId: {myCustomerAddress.AddressId}");
            return myCustomerAddress;
        }

        public async Task<CustomerAddress> UpdateCustomerAddress(int addressId, CustomerAddressUpdateDTO addressUpdateDto)
        {
            var existingAddress = await _custAddressRepo.GetAsync(addressId);
            if (existingAddress == null)
            {
                throw new NoCustomerAddressFoundException($"Customer address with ID {addressId} not found.");
            }
            _logger.LogInformation("Existing Customer Address Details:");
            _logger.LogInformation($"House Number: {existingAddress.HouseNumber}");
            _logger.LogInformation($"Building Name: {existingAddress.BuildingName}");
            _logger.LogInformation($"Locality: {existingAddress.Locality}");
            _logger.LogInformation($"City ID: {existingAddress.CityId}");
            _logger.LogInformation($"Landmark: {existingAddress.LandMark}");

            if (!string.IsNullOrEmpty(addressUpdateDto.HouseNumber))
            {
                existingAddress.HouseNumber = addressUpdateDto.HouseNumber;
            }
            if (!string.IsNullOrEmpty(addressUpdateDto.BuildingName))
            {
                existingAddress.BuildingName = addressUpdateDto.BuildingName;
            }
            if (!string.IsNullOrEmpty(addressUpdateDto.Locality))
            {
                existingAddress.Locality = addressUpdateDto.Locality;
            }
            if (addressUpdateDto.CityId.HasValue)
            {
                existingAddress.CityId = addressUpdateDto.CityId.Value;
            }
            if (!string.IsNullOrEmpty(addressUpdateDto.LandMark))
            {
                existingAddress.LandMark = addressUpdateDto.LandMark;
            }

            var updatedAddress = await _custAddressRepo.Update(existingAddress);
            _logger.LogInformation($"Customer address updated successfully. AddressId: {updatedAddress.AddressId}");
            return updatedAddress;
        }

        public async Task<CustomerAddress> ViewCustomerAddressByCustomerId(int customerId)
        {
            var customerAddress = await _custAddressRepo.GetAsync(customerId);
            if (customerAddress == null)
            {
                throw new NoCustomerAddressFoundException($"No address found for customer with ID {customerId}");
            }
            return customerAddress;
        }

        public async Task<CustomerReview> AddCustomerReview(CustomerReview customerReview)
        {
            customerReview = await _custReviewRepo.Add(customerReview);
            _logger.LogInformation($"Customer Review added successfully. ReviewId: {customerReview.ReviewId}");
            return customerReview;
        }

        public async Task<CustomerReview> ViewCustomerReview(int customerReviewId)
        {
            var customerReview = await _custReviewRepo.GetAsync(customerReviewId);
            if (customerReview == null)
            {
                throw new NoCustomerReviewFoundException();
            }
            return customerReview;
        }

        public async Task<CustomerReview> UpdateCustomerReviewText(CustomerReviewUpdateDTO reviewUpdateDTO)
        {
            var existingReview = await _custReviewRepo.GetAsync(reviewUpdateDTO.ReviewId);
            if (existingReview == null)
            {
                throw new NoCustomerReviewFoundException();
            }
            _logger.LogInformation($"Existing Text Review: {existingReview.TextReview}");
            existingReview.TextReview = reviewUpdateDTO.TextReview;
            var updatedReview = await _custReviewRepo.Update(existingReview);
            _logger.LogInformation($"Text Review updated successfully. ReviewId: {updatedReview.ReviewId}");
            return updatedReview;
        }

        public async Task<CustomerReview> DeleteCustomerReview(int reviewId)
        {
            var existingReview = await _custReviewRepo.GetAsync(reviewId);
            if (existingReview == null)
            {
                throw new NoCustomerReviewFoundException();
            }
            //existingReview.IsDeleted = true;
            var updatedReview = await _custReviewRepo.Delete(reviewId);
            _logger.LogInformation($"Customer Review soft deleted successfully. ReviewId: {updatedReview.ReviewId}");
            return updatedReview;
        }

        //Additional menu services
        public async Task<List<Menu>> SearchMenu(int restaurantId, string query)
        {
            var allMenus = await _menuRepo.GetAsync();
            var allMenuItems = allMenus.Where(m => m.RestaurantId == restaurantId);
            if (allMenuItems == null)
            {
                throw new NoMenuAvailableException();
            }
            var matchingMenuItems = allMenuItems.Where(menu => menu.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            return matchingMenuItems;
        }

        public async Task<List<Menu>> FilterMenuByPriceRange(int restaurantId, float minPrice, float maxPrice)
        {
            var allMenus = await _menuRepo.GetAsync();
            var allMenuItems = allMenus.Where(m => m.RestaurantId == restaurantId);
            if (allMenuItems == null)
            {
                throw new NoMenuAvailableException();
            }
            var filteredMenuItems = allMenuItems.Where(m => m.Price >= minPrice && m.Price <= maxPrice).ToList();
            return filteredMenuItems;
        }

        public async Task<List<Menu>> FilterMenuByType(int restaurantId, string type)
        {
            var allMenus = await _menuRepo.GetAsync();
            var allMenuItems = allMenus.Where(m => m.RestaurantId == restaurantId);
            if (allMenuItems == null)
            {
                throw new NoMenuAvailableException();
            }
            var filteredMenuItems = allMenuItems.Where(m => string.Equals(m.Type, type, StringComparison.OrdinalIgnoreCase)).ToList();
            return filteredMenuItems;
        }

        public async Task<List<Menu>> FilterMenuByCuisine(int restaurantId, string cuisine)
        {
            var allMenus = await _menuRepo.GetAsync();
            var allMenuItems = allMenus.Where(m => m.RestaurantId == restaurantId);
            if (allMenuItems == null)
            {
                throw new NoMenuAvailableException();
            }
            var filteredMenuItems = allMenuItems.Where(m => string.Equals(m.Cuisine, cuisine, StringComparison.OrdinalIgnoreCase)).ToList();
            return filteredMenuItems;
        }
        public async Task<IActionResult> GetCustomerByUsername(string username)
        {
            try
            {
                var customers = await _custRepo.GetAsync();
                var customer = customers.FirstOrDefault(c => c.UserName == username);
                if (customer != null)
                {
                    return new OkObjectResult(customer);
                }
                else
                {
                    return new NotFoundResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching customer by username: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    }
}
