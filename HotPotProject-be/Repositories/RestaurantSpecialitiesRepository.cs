using HotPotProject.Context;
using HotPotProject.Exceptions;
using HotPotProject.Interfaces;
using HotPotProject.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPotProject.Repositories
{
    public class RestaurantSpecialitiesRepository : IRepository<int, String, RestaurantSpeciality>
    {
        ApplicationTrackerContext _context;

        public RestaurantSpecialitiesRepository(ApplicationTrackerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Takes a RestaurantSpeciality object and adds it to the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns> A RestaurantSpeciality object that has been added to the database</returns>
        public async Task<RestaurantSpeciality> Add(RestaurantSpeciality item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        /// <summary>
        /// Takes a primary key <int> as parameter and deletes the object corresponding to it
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A RestaurantSpeciality that has been deleted from the database</returns>
        /// <exception cref="SpecialityNotFoundException"></exception>
        public async Task<RestaurantSpeciality> Delete(int key)
        {
            var speciality = await GetAsync(key);
            if (speciality != null)
            {
                _context.RestaurantSpecialities.Remove(speciality);
                _context.SaveChanges();
                return speciality;
            }
            throw new SpecialityNotFoundException("No speciality found");
        }

        /// <summary>
        /// Takes a primary key int and returns the object corresponding to it
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A RestaurantSpeciality object which corresponds to the primary key parameter</returns>
        /// <exception cref="SpecialityNotFoundException"></exception>
        public async Task<RestaurantSpeciality> GetAsync(int key)
        {
            var specialities = await GetAsync();
            var speciality = specialities.FirstOrDefault(s => s.CategoryId == key);
            if (speciality != null)
                return speciality;
            throw new SpecialityNotFoundException("No speciality found");
        }

        /// <summary>
        /// Returns a list of all the RestaurantSpeciality entities from the database
        /// </summary>
        /// <returns>A List of all the RestaurantSpeciality entities in the database</returns>
        /// <exception cref="SpecialityNotFoundException"></exception>
        public async Task<List<RestaurantSpeciality>> GetAsync()
        {
            var specialities = _context.RestaurantSpecialities.ToList();
            if (specialities != null || specialities.Count > 0)
                return specialities;
            throw new SpecialityNotFoundException("No specialities available at the moment");
        }

        public Task<RestaurantSpeciality> GetAsync(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Takes a RestaurantSpeciality object and updates it's reference in the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>An updated RestaurantSpeciality object</returns>
        /// <exception cref="SpecialityNotFoundException"></exception>
        public async Task<RestaurantSpeciality> Update(RestaurantSpeciality item)
        {
            var speciality = await GetAsync(item.CategoryId);
            if (speciality != null)
            {
                _context.Entry<RestaurantSpeciality>(item).State = EntityState.Modified;
                _context.SaveChanges();
                return speciality;
            }
            throw new SpecialityNotFoundException("No specialities available at the moment");
        }
    }
}

