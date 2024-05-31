using HotPotProject.Context;
using HotPotProject.Exceptions;
using HotPotProject.Interfaces;
using HotPotProject.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPotProject.Repositories
{
    public class OrderRepository : IRepository<int, string, Order>
    {
        private readonly ApplicationTrackerContext _context;
        private readonly ILogger<OrderRepository> _logger;
        public OrderRepository(ApplicationTrackerContext context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new order to the database.
        /// </summary>
        /// <param name="item">The order to add.</param>
        /// <returns>The added order.</returns>
        public async Task<Order> Add(Order item)
        {
            _context.Orders.Add(item);
            await _context.SaveChangesAsync();

            LogInformation($"Order Added: {item.OrderId}");

            return item;
        }

        /// <summary>
        /// Deletes an existing order from the database.
        /// </summary>
        /// <param name="key">The ID of the order to delete.</param>
        /// <returns>The deleted order, if found.</returns>
        public async Task<Order> Delete(int key)
        {
            try
            {
                var order = await GetAsync(key);

                if (order != null)
                {
                    _context.Orders.Remove(order);
                    await _context.SaveChangesAsync();

                    LogInformation($"Order Deleted: {order.OrderId}");
                }

                return order;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Retrieves an order by its ID.
        /// </summary>
        /// <param name="key">The ID of the order to retrieve.</param>
        /// <returns>The retrieved order, if found.</returns>
        public async Task<Order> GetAsync(int key)
        {
            var orders = await GetAsync();
            var order = orders.FirstOrDefault(o => o.OrderId == key);

            if (order != null)
            {
                return order;
            }

            throw new OrdersNotFoundException();
        }

        /// <summary>
        /// Retrieves all orders from the database, including their associated user, restaurant, delivery partner, and order items.
        /// </summary>
        /// <returns>A list of all orders with their related data.</returns>
        public async Task<List<Order>> GetAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Restaurant)
                .Include(o => o.DeliveryPartner)
                .ToListAsync();

            return orders;
        }

        /// <summary>
        /// Updates an existing order in the database.
        /// </summary>
        /// <param name="item">The order to update.</param>
        /// <returns>The updated order.</returns>
        public async Task<Order> Update(Order item)
        {
            var order = await GetAsync(item.OrderId);

            if (order != null)
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                LogInformation($"Order Updated: {item.OrderId}");
            }

            return order;
        }

        /// <summary>
        /// Logs an informational message using the provided logger.
        /// </summary>
        /// <param name="message">The message to log.</param>

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public Task<Order> GetAsync(string key)
        {
            throw new NotImplementedException();
        }
    }
}
