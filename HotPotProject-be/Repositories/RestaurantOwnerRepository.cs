using HotPotProject.Context;
using HotPotProject.Interfaces;
using HotPotProject.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPotProject.Repositories
{
    public class RestaurantOwnerRepository : IRepository<int, string, RestaurantOwner>
    {
        private readonly ApplicationTrackerContext _context;
        private readonly ILogger<RestaurantOwnerRepository> _logger;

        public RestaurantOwnerRepository(ApplicationTrackerContext context, ILogger<RestaurantOwnerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<RestaurantOwner> Add(RestaurantOwner item)
        {
            _context.RestaurantOwners.Add(item);
            _context.SaveChanges();
            LogInformation($"Restaurant Owner Added: {item.OwnerId}");
            return item;
        }

        public async Task<RestaurantOwner> Delete(int key)
        {
            var owner = await GetAsync(key);
            if (owner != null)
            {
                _context.RestaurantOwners.Remove(owner);
                await _context.SaveChangesAsync();
                LogInformation($"Restaurant Owner Deleted: {owner.OwnerId}");
                return owner;
            }
            return null;
        }

        public async Task<RestaurantOwner> GetAsync(int key)
        {
            var owner = await _context.RestaurantOwners.FindAsync(key);
            return owner;
        }

        public async Task<List<RestaurantOwner>> GetAsync()
        {
            var owners = await _context.RestaurantOwners.ToListAsync();
            LogInformation("Restaurant Owners retrieved successfully.");
            return owners;
        }

        public async Task<RestaurantOwner> Update(RestaurantOwner item)
        {
            var owner = await GetAsync(item.OwnerId);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            LogInformation($"Restaurant Owner Updated: {item.OwnerId}");
            return item;

        }

        public async Task<RestaurantOwner> GetAsync(string name)
        {
            var restaurantOwners = _context.RestaurantOwners.ToList();
            var restaurantOwner = restaurantOwners.FirstOrDefault(r => r.Name == name);
            return restaurantOwner;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
