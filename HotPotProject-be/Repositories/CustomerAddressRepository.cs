using HotPotProject.Context;
using HotPotProject.Interfaces;
using HotPotProject.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPotProject.Repositories
{
    public class CustomerAddressRepository : IRepository<int, String, CustomerAddress>
    {
        ApplicationTrackerContext _context;
        readonly ILogger<CustomerAddressRepository> _logger;

        public CustomerAddressRepository(ApplicationTrackerContext context, ILogger<CustomerAddressRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Takes a UserAddress object and adds it to the database table
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A UserAddress object that has been added to the database table</returns>
        public async Task<CustomerAddress> Add(CustomerAddress item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        /// <summary>
        /// Takes a primary key int and deletes the UserAddress record corresponding to it from the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A UserAddress object that has been deleted from the database</returns>
        /// <exception cref="Exception"></exception>
        public async Task<CustomerAddress> Delete(int key)
        {
            var address = await GetAsync(key);
            if (address != null)
            {
                _context.CustomerAddresses.Remove(address);
                _context.SaveChanges();
                return address;
            }
            throw new Exception();
        }

        /// <summary>
        /// Takes a primary key int as parameter and returns UserAddress record corresponding to it from the database
        /// </summary>
        /// <param name="key"></param>
        /// <returns>An UserAddress object with key as the primary key</returns>
        /// <exception cref="Exception"></exception>
        public async Task<CustomerAddress> GetAsync(int key)
        {
            var addresses = await GetAsync();
            var address = addresses.FirstOrDefault(a => a.CustomerId == key);
            if (address != null)
                return address;
            throw new Exception();
        }

        /// <summary>
        /// Method to return list of all the entities in the UserAddress table
        /// </summary>
        /// <returns>A List of UserAddress which contains all the entities in the database</returns>
        public async Task<List<CustomerAddress>> GetAsync()
        {
            var addresses = _context.CustomerAddresses.Include(a => a.City).ToList();
            return addresses;
        }

        /// <summary>
        /// Takes an UserAddress object and updates it's reference in the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>An updated UserAddress object</returns>
        /// <exception cref="Exception"></exception>
        public async Task<CustomerAddress> Update(CustomerAddress item)
        {
            var address = await GetAsync(item.AddressId);
            if (address != null)
            {
                _context.Entry<CustomerAddress>(item).State = EntityState.Modified;
                _context.SaveChanges();
                return address;
            }
            throw new Exception();
        }

        public Task<CustomerAddress> GetAsync(string key)
        {
            throw new NotImplementedException();
        }
    }
}
