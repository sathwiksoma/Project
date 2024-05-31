using HotPotProject.Context;
using HotPotProject.Exceptions;
using HotPotProject.Interfaces;
using HotPotProject.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPotProject.Repositories
{
    public class RestaurantRepository : IRepository<int, String, Restaurant>
    {
        ApplicationTrackerContext _context;

        public RestaurantRepository(ApplicationTrackerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Takes a Restaurant object and adds it to the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A Restaurant object which has been added to the database</returns>
        public async Task<Restaurant> Add(Restaurant item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        /// <summary>
        /// Takes a primary key as parameter and deletes it's corresponding object from the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A Restaurant object which has been deleted from the database</returns>
        /// <exception cref="RestaurantNotFoundException"></exception>
        public async Task<Restaurant> Delete(int key)
        {
            var restaurant = await GetAsync(key);
            _context.Restaurants.Remove(restaurant);
            _context.SaveChanges();
            return restaurant;
        }

        /// <summary>
        /// Takes a primary key as parameter and returns a Restaurant object that corresponds to it
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A Restaurant object with primary key as the parameter</returns>
        /// <exception cref="RestaurantNotFoundException"></exception>
        public async Task<Restaurant> GetAsync(int key)
        {
            var restaurants = await GetAsync();
            var restaurant = restaurants.FirstOrDefault(r => r.RestaurantId == key);
            return restaurant;
        }

        /// <summary>
        /// Returns all the available restaurant records in the database
        /// </summary>
        /// <returns>A List of Restaurant type which contains all the restaurant information</returns>
        /// <exception cref="RestaurantNotFoundException"></exception>
        public async Task<List<Restaurant>> GetAsync()
        {
            var restaurants = _context.Restaurants.ToList();
            return restaurants;
        }

        /// <summary>
        /// Takes a restaurant object and updates it's reference in the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Updated Restaurant object</returns>
        /// <exception cref="RestaurantNotFoundException"></exception>
        public async Task<Restaurant> Update(Restaurant item)
        {
            var restaurant = await GetAsync(item.RestaurantId);
            _context.Entry<Restaurant>(item).State = EntityState.Modified;
            _context.SaveChanges();
            return restaurant;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="RestaurantNotFoundException"></exception>
        public async Task<Restaurant> GetAsync(String name)
        {
            var restaurants = _context.Restaurants.ToList();
            var restaurant = restaurants.FirstOrDefault(r => r.RestaurantName.ToLower() == name.ToLower());
            return restaurant;
        }
        public async Task<Restaurant> DeleteAsync(string name)
        {
            var restaurant = await GetAsync(name);
            if (restaurant == null)
            {
                throw new RestaurantNotFoundException("Restaurant not found.");
            }

            _context.Restaurants.Remove(restaurant);
            _context.SaveChanges();
            return restaurant;
        }

    }
}
