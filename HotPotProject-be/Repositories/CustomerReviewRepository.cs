using HotPotProject.Context;
using HotPotProject.Exceptions;
using HotPotProject.Interfaces;
using HotPotProject.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPotProject.Repositories
{
    public class CustomerReviewRepository : IRepository<int, String, CustomerReview>
    {
        ApplicationTrackerContext _context;

        public CustomerReviewRepository(ApplicationTrackerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Takes a UserReview object and adds it to the database table
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A UserReview Object that has been added to the database</returns>
        public async Task<CustomerReview> Add(CustomerReview item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        /// <summary>
        /// Takes Review Id as a parameter and deletes the object from the database.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A UserReview Object that has been deleted from the database</returns>
        /// <exception cref="ReviewNotFoundException"></exception>
        public async Task<CustomerReview> Delete(int key)
        {
            var review = await GetAsync(key);
            if (review != null)
            {
                _context.CustomerReviews.Remove(review);
                _context.SaveChanges();
                return review;
            }
            throw new ReviewNotFoundException();
        }

        /// <summary>
        /// Takes primary key as a parameter and returns UserReview if it exists in the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A UserReview Object with primary key as the parameter which was passed</returns>
        /// <exception cref="ReviewNotFoundException"></exception>
        public async Task<CustomerReview> GetAsync(int key)
        {
            var reviews = await GetAsync();
            var review = reviews.FirstOrDefault(r => r.ReviewId == key);
            if (review != null)
                return review;
            throw new ReviewNotFoundException();
        }

        /// <summary>
        /// Method to get list of all the objects from the database
        /// </summary>
        /// <returns>A List of UserReview type which contains all the reviews in the database</returns>
        public async Task<List<CustomerReview>> GetAsync()
        {
            var reviews = _context.CustomerReviews.ToList();
            return reviews;
        }

        public Task<CustomerReview> GetAsync(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Takes an object and updates its reference in the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>An updated UserReview Object</returns>
        /// <exception cref="ReviewNotFoundException"></exception>
        public async Task<CustomerReview> Update(CustomerReview item)
        {
            var review = await GetAsync(item.ReviewId);
            if (review != null)
            {
                _context.Entry<CustomerReview>(item).State = EntityState.Modified;
                _context.SaveChanges();
                return review;
            }
            throw new ReviewNotFoundException();
        }
    }
}
