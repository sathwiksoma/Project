using HotPotProject.Context;
using HotPotProject.Exceptions;
using HotPotProject.Interfaces;
using HotPotProject.Models;

namespace HotPotProject.Repositories
{
    public class CustomerRepository : IRepository<int, String, Customer>
    {
        ApplicationTrackerContext _context;

        public CustomerRepository(ApplicationTrackerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Takes a User object as a parameter and adds it to the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A User object that has been added to the database</returns>
        public async Task<Customer> Add(Customer item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        /// <summary>
        /// Takes a key int which is a primary key and deletes the record corresponding to it in the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A User object that has been deleted from the database</returns>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task<Customer> Delete(int key)
        {
            var user = await GetAsync(key);
            if (user != null)
            {
                _context.Customers.Remove(user);
                _context.SaveChanges();
                return user;
            }
            throw new UserNotFoundException();
        }

        /// <summary>
        /// Takes a key int as parameter which is a primary key and returns a User record corresponding to it from the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A User object with key as it's primary key</returns>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task<Customer> GetAsync(int key)
        {
            var users = _context.Customers.ToList();
            var user = users.FirstOrDefault(u => u.Id == key);
            if (user != null)
                return user;
            throw new UserNotFoundException();
        }

        /// <summary>
        /// Method to return all the user entities from the database
        /// </summary>
        /// <returns>A List of User type which contains all the User records from the database</returns>
        /// <exception cref="NoUsersAvailableException"></exception>
        public async Task<List<Customer>> GetAsync()
        {
            var users = _context.Customers.ToList();
            if (users != null || users.Count > 0)
                return users;
            throw new NoUsersAvailableException();
        }

        public Task<Customer> GetAsync(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Takes an User object and updates it's reference in the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>An updated User object</returns>
        /// <exception cref="UserNotFoundException"></exception>
      public async Task<Customer> Update(Customer item)
          {
            var user = await GetAsync(item.Id);
            if (user != null)
            {
                //_context.Entry<Customer>(item).State = EntityState.Modified;
                user.Name = item.Name;
                user.Email = item.Email;
                user.Phone = item.Phone;
                user.UserName = item.UserName;
                _context.SaveChanges();
                return user;
            }
            throw new UserNotFoundException();
        }
    }
}
