using HotPotProject.Context;
using HotPotProject.Exceptions;
using HotPotProject.Interfaces;
using HotPotProject.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPotProject.Repositories
{
    public class PaymentRepository : IRepository<int, string, Payment>
    {
        private readonly ApplicationTrackerContext _context;
        private readonly ILogger<PaymentRepository> _logger;
        public PaymentRepository(ApplicationTrackerContext context, ILogger<PaymentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new payment to the database.
        /// </summary>
        /// <param name="item">The payment to add.</param>
        /// <returns>The added payment.</returns>
        public async Task<Payment> Add(Payment item)
        {
            _context.Payments.Add(item);
            await _context.SaveChangesAsync();

            LogInformation($"Payment Added: {item.PaymentId}");

            return item;
        }

        /// <summary>
        /// Deletes an existing payment from the database.
        /// </summary>
        /// <param name="key">The ID of the payment to delete.</param>
        /// <returns>The deleted payment, if found.</returns>
        public async Task<Payment> Delete(int key)
        {
            var payment = await GetAsync(key);

            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();

                LogInformation($"Payment Deleted: {payment.PaymentId}");
            }

            return payment;
        }

        /// <summary>
        /// Retrieves a payment by its ID.
        /// </summary>
        /// <param name="key">The ID of the payment to retrieve.</param>
        /// <returns>The retrieved payment, if found.</returns>
        public async Task<Payment> GetAsync(int key)
        {
            var payments = await GetAsync();
            var payment = payments.FirstOrDefault(p => p.PaymentId == key);

            if (payment != null)
            {
                return payment;
            }

            throw new PaymentsNotFoundException();
        }

        /// <summary>
        /// Retrieves all payments from the database, including their associated orders.
        /// </summary>
        /// <returns>A list of all payments with their related data.</returns>
        public async Task<List<Payment>> GetAsync()
        {
            var payments = await _context.Payments
                .Include(p => p.Order)
                .ToListAsync();

            return payments;
        }

        /// <summary>
        /// Updates an existing payment in the database.
        /// </summary>
        /// <param name="item">The payment to update.</param>
        /// <returns>The updated payment.</returns>

        public async Task<Payment> Update(Payment item)
        {
            var payment = await GetAsync(item.PaymentId);

            if (payment != null)
            {
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                LogInformation($"Payment Updated: {item.PaymentId}");
            }

            return payment;
        }

        /// <summary>
        /// Logs an informational message using the provided logger.
        /// </summary>
        /// <param name="message">The message to log.</param>

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public Task<Payment> GetAsync(string key)
        {
            throw new NotImplementedException();
        }
    }
}
