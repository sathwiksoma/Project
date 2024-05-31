using HotPotProject.Interfaces;
using HotPotProject.Models.DTO;
using HotPotProject.Models;
using System.Security.Cryptography;
using System.Text;
using HotPotProject.Exceptions;
using HotPotProject.Repositories;

namespace HotPotProject.Services
{
    public class AdminServices:IAdminServices
    {
        private readonly IRepository<int, string, User> _userRepo;
        private readonly ITokenServices _tokenServices;

        public AdminServices(IRepository<int, string, User> userRepo, ITokenServices tokenServices)
        {
            _userRepo = userRepo;
            _tokenServices = tokenServices;
        }
        public async Task<LoginUserDTO> LoginAdmin(LoginUserDTO loginUser)
        {
            var adminUser = await _userRepo.GetAsync(loginUser.UserName);
            if (adminUser == null)
                throw new InvalidUserException();
            var password = getEncryptedPassword(loginUser.Password, adminUser.Key);
            var matchPassword = passwordMatch(password, adminUser.Password);
            if (matchPassword)
            {
                loginUser.Password = "";
                loginUser.Role = adminUser.Role;
                loginUser.Token = await _tokenServices.GenerateToken(loginUser);
                return loginUser;
            }
            throw new InvalidUserException();
        }

        private bool passwordMatch(byte[] password, byte[] userPassword)
        {
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] != userPassword[i])
                    return false;
            }
            return true;
        }

        private byte[] getEncryptedPassword(string password, byte[] key)
        {
            HMACSHA512 hmac = new HMACSHA512(key);
            var userPwd = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return userPwd;
        }

        public async Task<LoginUserDTO> RegisterAdmin(LoginUserDTO registerUser)
        {
            User adminUser = new User();
            adminUser.UserName = registerUser.UserName;
            adminUser.Role = "Admin";
            HMACSHA512 hmac = new HMACSHA512();
            adminUser.Key = hmac.Key;
            adminUser.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUser.Password));
            adminUser = await _userRepo.Add(adminUser);
            registerUser.Password = "";
            registerUser.Role = "Admin";
            return registerUser;
        }
        
    }
}
