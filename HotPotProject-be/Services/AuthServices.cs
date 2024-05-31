using HotPotProject.Context;
using HotPotProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HotPotProject.Services
{
    public class AuthServices
    {
        private readonly ApplicationTrackerContext _context;

        public AuthServices(ApplicationTrackerContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

    }
}