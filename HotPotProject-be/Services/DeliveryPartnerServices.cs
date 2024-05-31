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
    public class DeliveryPartnerServices : IDeliveryPartnerServices
    {
        private readonly IRepository<int, string, Order> _orderRepo;
        private readonly IRepository<int, string, DeliveryPartner> _deliveryPartnerRepo;
        private readonly IRepository<int, string, User> _userRepo;
        private readonly ITokenServices _tokenServices;
        private readonly ILogger<DeliveryPartnerServices> _logger;

        public DeliveryPartnerServices(IRepository<int, string, Order> orderRepo,
                                       IRepository<int, string, DeliveryPartner> deliveryPartnerRepo,
                                       IRepository<int, string, User> userRepo,
                                       ITokenServices tokenServices,
                                       ILogger<DeliveryPartnerServices> logger)
        {
            _orderRepo = orderRepo;
            _deliveryPartnerRepo = deliveryPartnerRepo;
            _userRepo = userRepo;
            _tokenServices = tokenServices;
            _logger = logger;
        }
        public async Task<Order> ChangeOrderStatus(int orderId)
        {
            var order = await _orderRepo.GetAsync(orderId);
            if (order == null)
                throw new OrdersNotFoundException();
            order.Status = "delivered";
            order = await _orderRepo.Update(order);
            return order;
        }

        public async Task<DeliveryPartner> GetDeliveryPartnerDetails(int partnerId)
        {
            var deliveryPartner = await _deliveryPartnerRepo.GetAsync(partnerId);
            return deliveryPartner;
        }

        [ExcludeFromCodeCoverage]
        public async Task<LoginUserDTO> LoginDeliveryPartner(LoginUserDTO loginUser)
        {
            var user = await _userRepo.GetAsync(loginUser.UserName);
            var deliveryPartners = await _deliveryPartnerRepo.GetAsync();
            var deliveryPartner = deliveryPartners.FirstOrDefault(d => d.UserName == loginUser.UserName);
            if (user == null)
                throw new NoDeliveryPartnerFoundException();
            var password = getEncryptedPassword(loginUser.Password, user.Key);
            bool matchPassword = passwordMatch(password, user.Password);
            if (matchPassword)
            {
                loginUser.UserId = deliveryPartner.PartnerId;
                loginUser.UserName = loginUser.UserName;
                loginUser.Password = "";
                loginUser.Role = user.Role;
                loginUser.Token = await _tokenServices.GenerateToken(loginUser);
                return loginUser;
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

        public async Task<LoginUserDTO> RegisterDeliveryPartner(RegisterDeliveryPartnerDTO deliveryPartner)
        {
            User myUser = new RegisterToDeliveryPartnerUser(deliveryPartner).GetUser();
            myUser = await _userRepo.Add(myUser);
            DeliveryPartner myDeliveryPartner = new RegisterToDeliveryPartner(deliveryPartner).GetDeliveryPartner();
            myDeliveryPartner = await _deliveryPartnerRepo.Add(myDeliveryPartner);

            LoginUserDTO loginUser = new LoginUserDTO
            {
                UserId = myDeliveryPartner.PartnerId,
                UserName = myUser.UserName,
                Role = myUser.Role
            };

            return loginUser;
        }

        public async Task<DeliveryPartner> UpdateDeliveryPartnerDetails(DeliveryPartner deliveryPartner)
        {
            var updatedPartner = await _deliveryPartnerRepo.Update(deliveryPartner);
            return updatedPartner;
        }

        public async Task<List<Order>> GetAllOrders(int partnerId)
        {
            var orders = await _orderRepo.GetAsync();
            var ordersForPartner = orders.Where(o => o.PartnerId == partnerId).ToList();
            if (ordersForPartner == null)
                throw new OrdersNotFoundException();
            return ordersForPartner;
        }
    }
}
