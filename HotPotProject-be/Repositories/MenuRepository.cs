using HotPotProject.Context;
using HotPotProject.Exceptions;
using HotPotProject.Interfaces;
using HotPotProject.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPotProject.Repositories
{
    public class MenuRepository : IRepository<int, String, Menu>
    {
        private readonly ApplicationTrackerContext _context;
        private readonly ILogger<MenuRepository> _logger;
        public MenuRepository(ApplicationTrackerContext context, ILogger<MenuRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new menu item to the database.
        /// </summary>
        /// <param name="item">The menu item to add.</param>
        /// <returns>The added menu item.</returns>
        public async Task<Menu> Add(Menu item)
        {
            _context.Menus.Add(item);
            await _context.SaveChangesAsync();
            LogInformation($"Menu Added: {item.MenuId}");
            return item;
        }

        /// <summary>
        /// Deletes an existing menu item from the database.
        /// </summary>
        /// <param name="key">The ID of the menu item to delete.</param>
        /// <returns>The deleted menu item, if found.</returns>
        public async Task<Menu> Delete(int key)
        {
            var menu = await GetAsync(key);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
                LogInformation($"Menu deleted: {menu.MenuId}");
                return menu;
            }
            return null;
        }

        /// <summary>
        /// Retrieves a menu item by its ID.
        /// </summary>
        /// <param name="key">The ID of the menu item to retrieve.</param>
        /// <returns>The retrieved menu item, if found.</returns>
        public async Task<Menu> GetAsync(int key)
        {
            var menus = await GetAsync();
            var menu = menus.FirstOrDefault(m => m.MenuId == key);
            if (menu != null)
            {
                return menu;
            }
            throw new NoMenuAvailableException();
        }

        /// <summary>
        /// Retrieves all menu items from the database, including their associated nutritional information and restaurant.
        /// </summary>
        /// <returns>A list of all menu items with their related data.</returns>
        public async Task<List<Menu>> GetAsync()
        {
            var menus = await _context.Menus
                .Include(m => m.NutritionalInfo)
                .Include(m => m.Restaurant)
                .ToListAsync();
            LogInformation("Menu items retrieved successfully.");
            return menus;
        }

        /// <summary>
        /// Updates an existing menu item in the database.
        /// </summary>
        /// <param name="item">The menu item to update.</param>
        /// <returns>The updated menu item.</returns>
        public async Task<Menu> Update(Menu item)
        {
            var menu = await GetAsync(item.MenuId);
            if (menu != null)
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                LogInformation($"Menu Updated: {item.MenuId}");
            }
            return menu;
        }

        /// <summary>
        /// Retrieves a menu item by its name.
        /// </summary>
        /// <param name="name">The name of the menu item to retrieve.</param>
        /// <returns>The retrieved menu item, if found.</returns>
        public async Task<Menu> GetAsync(string name)
        {
            var menus = _context.Menus.ToList();
            var menu = menus.FirstOrDefault(m => m.Name == name);
            if (menu != null)
            {
                return menu;
            }
            throw new NoMenuAvailableException();
        }

        /// <summary>
        /// Logs an informational message using the provided logger.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
