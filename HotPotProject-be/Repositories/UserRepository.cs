using HotPotProject.Context;
using HotPotProject.Interfaces;
using HotPotProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotPotProject.Repositories
{
    public class UserRepository : IRepository<int, string, User>
    {
        private readonly ApplicationTrackerContext _context;

        public UserRepository(ApplicationTrackerContext context)
        {
            _context = context;
        }

        public async Task<User> Add(User item)
        {
            _context.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<User> Delete(int key)
        {
            var user = await _context.Users.FindAsync(key);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }

        public async Task<User> GetAsync(int key)
        {
            return await _context.Users.FindAsync(key);
        }

        public async Task<List<User>> GetAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetAsync(string key)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == key);
        }

        public async Task<User> Update(User item)
        {
            var existingUser = await _context.Users.FindAsync(item.UserName);
            if (existingUser != null)
            {
                _context.Entry(existingUser).CurrentValues.SetValues(item);
                await _context.SaveChangesAsync();
            }
            return existingUser;
        }
      
    }
}
