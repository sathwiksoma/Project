using HotPotProject.Exceptions;
using HotPotProject.Interfaces;
using HotPotProject.Models.DTO;
using HotPotProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotPotProject.Controllers
{
    [EnableCors("ReactPolicy")]
    [Route("api/[controller]")]
    [ApiController]

    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantUserServices _services;
        private readonly ILogger<RestaurantController> _logger;
        private readonly IWebHostEnvironment _environment;



        public RestaurantController(IRestaurantUserServices services, ILogger<RestaurantController> logger, IWebHostEnvironment environment)
        {
            _services = services;
            _logger = logger;
            _environment = environment;
        }

        //[Route("RegisterRestaurant")]
        //[HttpPost]
        //public async Task<LoginUserDTO> RegisterRestaurant(RegisterRestaurantDTO registerRestaurant)
        //{
        //    try
        //    {
        //        var newRestaurant=await _services.RegisterRestaurant
        //    }
        //}
        //[Authorize(Roles ="RestautrantOwner,Admin")]
        [Route("AddMenuItem")]
        [HttpPost]
        public async Task<ActionResult<Menu>> AddMenuItem([FromForm] MenuDTO menuDto)
        {
            try
            {
                if (menuDto.ItemImage != null)
                {
                    var filePath = Path.Combine(_environment.WebRootPath, "uploads", menuDto.ItemImage.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await menuDto.ItemImage.CopyToAsync(stream);
                    }

                    var menudata = new Menu
                    {
                        Name = menuDto.Name,
                        Type = menuDto.Type,
                        Price = menuDto.Price,
                        Description = menuDto.Description,
                        Cuisine = menuDto.Cuisine,
                        Category = menuDto.Category,

                        CookingTime = menuDto.CookingTime,
                        TasteInfo = menuDto.TasteInfo,
                        ItemImage = $"uploads/{menuDto.ItemImage.FileName}",
                        NutritionId= menuDto.NutritionId,
                        RestaurantId= menuDto.RestaurantId,

                    };
                    await _services.AddMenuItem(menudata);
                    return menudata;
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (RestaurantNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return Unauthorized("Can't add the menu item");
            }
        }
        //[Authorize(Roles = "RestautrantOwner,Admin")]
        [Route("ChangeOrderStatus")]
        [HttpPut]
        public async Task<ActionResult<Order>> ChangeOrderStatus(int orderId, string newStatus)
        {
            try
            {
                var order = await _services.ChangeOrderStatus(orderId, newStatus);
                return order;
            }
            catch (OrdersNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return Unauthorized("Can't change the order status");
            }
        }
       //[Authorize(Roles = "Admin")]
        [Route("AddRestaurant")]
        [HttpPost]
        public async Task<Restaurant> AddRestaurant(Restaurant restaurant)
        {
            restaurant = await _services.AddRestaurant(restaurant);
            return restaurant;
        }

        //[Authorize(Roles = "RestautrantOwner,Admin")]
        [Route("GetAllOrders")]
        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetALlOrders()
        {
            try
            {
                var result = await _services.GetAllOrders();
                return result;
            }
            catch (OrdersNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound("No order data found");
            }
        }

       //[Authorize(Roles = "RestautrantOwner")]
        [Route("GetAllOrdersByRestaurant")]
        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetALlOrders(int restaurantId)
        {
            try
            {
                var result = await _services.GetAllOrders(restaurantId);
                return result;
            }
            catch (OrdersNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound("No order data found");
            }
        }

        //[Authorize(Roles = "RestautrantOwner,Admin")]
        [Route("GetAllPayments")]
        [HttpGet]
        public async Task<ActionResult<List<Payment>>> GetPaymentsByRestaurants()
        {
            try
            {
                var payments = await _services.GetAllPayments();
                return payments;
            }
            catch (PaymentsNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound("No payment data found");
            }
        }

        
       //[Authorize(Roles = "RestautrantOwner,Admin")]
        [Route("GetAllPaymentsByRestaurants")]
        [HttpGet]
        public async Task<ActionResult<List<Payment>>> GetPaymentsByRestaurants(int restaurantId)
        {
            try
            {
                var payments = await _services.GetAllPayments(restaurantId);
                return payments;
            }
            catch (PaymentsNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound("No payment data found");
            }
        }

       //[Authorize(Roles = "RestautrantOwner,Admin")]
        [Route("AddRestaurantSpeciality")]
        [HttpPost]
        public async Task<ActionResult<RestaurantSpeciality>> AddRestaurantSpeciality(RestaurantSpeciality restaurantspeciality)
        {
            try
            {
                var addedSpeciality = await _services.AddRestaurantSpeciality(restaurantspeciality);
                // Return 201 Created status along with the added restaurant speciality
                return addedSpeciality;
                
            }
            catch (RestaurantNotFoundException)
            {
                // Return 404 Not Found status if the restaurant is not found
                return NotFound("Restaurant not found.");
            }
            catch (Exception)
            {
                // Return 500 Internal Server Error status for other exceptions
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [Route("GetSpecialities")]
        [HttpGet]
        public async Task<IActionResult> GetSpecialities()
        {
            try
            {
                var specialities = await _services.GetAllSpecialities();
                return Ok(specialities);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                return BadRequest(e.Message);
            }
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDTO loginUser)
        {
            try
            {
                loginUser = await _services.LogInRestaurant(loginUser);
                return Ok(loginUser);
            }
            catch (InvalidUserException e)
            {
                _logger.LogCritical(e.Message);
                return Unauthorized(e.Message);
            }
        }

        [Route("RegisterRestaurantOwner")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRestaurantDTO registerRestaurant)
        {
            try
            {
                var registeredRestaurant = await _services.RegisterRestaurant(registerRestaurant);
                return Ok(registeredRestaurant);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                return BadRequest(e.Message);
            }
        }

         [Route("GetAllReviews")]
        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            try
            {
                var reviews = await _services.GetCustomerReviews();
                return Ok(reviews);
            }
            catch (NoCustomerReviewFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound(e.Message);
            }
        }

        //[Authorize(Roles = "RestautrantOwner,Admin")]
        [Route("DeleteMenuItem")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMenuItem(int menuId)
        {
            try
            {
                var menuItem = await _services.DeleteMenuItem(menuId);
                return Ok(menuItem);
            }
            catch (NoMenuAvailableException e)
            {
                _logger.LogCritical(e.Message);
                return BadRequest(e.Message);
            }
        }
       
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAllRestaurants")]
        public async Task<ActionResult<List<Restaurant>>> GetAllRestaurants()
        {
            try
            {
                var restaurants = await _services.GetAllRestaurants();
                return Ok(restaurants);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                return BadRequest(e.Message);
            }
        }
       
        //[Authorize(Roles = "Admin")]
        [Route("GetAllMenus")]
        [HttpGet]
        public async Task<IActionResult> GetAllMenus()
        {
            try
            {
                var menus = await _services.GetAllMenus();
                return Ok(menus);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                return BadRequest(e.Message);
            }
        }
       
        //[Authorize(Roles = "Admin")]
        [Route("DeleteRestaurant")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRestaurant(int restaurantId)
        {
            try
            {
                var deletedRestaurant = await _services.DeleteRestaurant(restaurantId);
                return Ok(deletedRestaurant);
            }
            catch (RestaurantNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound(e.Message);
            }
        }
        
        [Route("GetRestaurantOwnerByUsername")]
        [HttpGet]
        public async Task<IActionResult> GetRestaurantOwnerByUsername(string username)
        {
            try
            {
                var restaurantOwner = await _services.GetRestaurantOwnerByUsername(username);
                return Ok(restaurantOwner);
            }
            catch (RestaurantOwnerNotFoundException e)
            {
                _logger.LogCritical(e.Message);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


    }
}
