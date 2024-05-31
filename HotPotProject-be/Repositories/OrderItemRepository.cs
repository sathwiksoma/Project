using HotPotProject.Context;
using HotPotProject.Interfaces;
using HotPotProject.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPotProject.Repositories
{
    public class OrderItemRepository : IRepository<int, string, OrderItem>
    {
        private readonly ApplicationTrackerContext _context;
        private readonly ILogger<OrderItemRepository> _logger;
        public OrderItemRepository(ApplicationTrackerContext context, ILogger<OrderItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new order item to the database.
        /// </summary>
        /// <param name="item">The order item to add.</param>
        /// <returns>The added order item.</returns>
        public async Task<OrderItem> Add(OrderItem item)
        {
            _context.OrderItems.Add(item);
            await _context.SaveChangesAsync();

            LogInformation($"Order Item Added: OrderId={item.OrderId}, MenuId={item.MenuId}");

            return item;
        }

        /// <summary>
        /// Deletes an existing order item from the database.
        /// </summary>
        /// <param name="key">The composite key of the order item to delete (OrderId, MenuId).</param>
        /// <returns>The deleted order item, if found.</returns>
        public async Task<OrderItem> Delete((int OrderId, int MenuId) key)
        {
            var orderItem = await GetAsync(key);

            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();

                LogInformation($"Order Item Deleted: OrderId={orderItem.OrderId}, MenuId={orderItem.MenuId}");
            }

            return orderItem;
        }

        /// <summary>
        /// Retrieves an order item by its composite key (OrderId, MenuId).
        /// </summary>
        /// <param name="key">The composite key of the order item to retrieve.</param>
        /// <returns>The retrieved order item, if found.</returns>
        public async Task<OrderItem> GetAsync((int OrderId, int MenuId) key)
        {
            var orderItem = await _context.OrderItems
                .Where(oi => oi.OrderId == key.OrderId && oi.MenuId == key.MenuId)
                .FirstOrDefaultAsync();

            return orderItem;
        }

        /// <summary>
        /// Retrieves all order items from the database, including their associated order and menu information.
        /// </summary>
        /// <returns>A list of all order items with their related data.</returns>

        public async Task<List<OrderItem>> GetAsync()
        {
            var orderItems = await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Menu)
                .ToListAsync();

            return orderItems;
        }

        /// <summary>
        /// Updates an existing order item in the database.
        /// </summary>
        /// <param name="item">The order item to update.</param>
        /// <returns>The updated order item.</returns>
        public async Task<OrderItem> Update(OrderItem item)
        {
            var orderItem = await GetAsync((item.OrderId, item.MenuId));

            if (orderItem != null)
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                LogInformation($"Order Item Updated: OrderId={item.OrderId}, MenuId={item.MenuId}");
            }

            return orderItem;
        }


        /// <summary>
        /// Logs an informational message using the provided logger.
        /// </summary>
        /// <param name="message">The message to log.</param>

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public Task<OrderItem> GetAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task<OrderItem> Delete(int key)
        {
            throw new NotImplementedException();
        }

        public Task<OrderItem> GetAsync(string key)
        {
            throw new NotImplementedException();
        }
    }
}
