using HotPotProject.Models.DTO;
using HotPotProject.Models;
using System.Security.Cryptography;
using System.Text;

namespace HotPotProject.Mappers
{
    public class RegisterToUser
    {
        User user;

        public RegisterToUser(RegisterCustomerDTO registerCustomer)
        {
            user = new User();
            user.UserName = registerCustomer.UserName;
            user.Role = registerCustomer.Role;
            generatePassword(registerCustomer.Password);
        }

        void generatePassword(string password)
        {
            HMACSHA512 hmac = new HMACSHA512();
            user.Key = hmac.Key;
            user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public User getUser()
        {
            return user;
        }
    }
}
