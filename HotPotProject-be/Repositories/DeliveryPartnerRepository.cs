using HotPotProject.Context;
using HotPotProject.Exceptions;
using HotPotProject.Interfaces;
using HotPotProject.Models;

namespace HotPotProject.Repositories
{
    public class DeliveryPartnerRepository : IRepository<int, string, DeliveryPartner>
    {
        private readonly ApplicationTrackerContext _context;
        private readonly ILogger<DeliveryPartnerRepository> _logger;
        public DeliveryPartnerRepository(ApplicationTrackerContext context, ILogger<DeliveryPartnerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new delivery partner to the database.
        /// </summary>
        /// <param name="item">The delivery partner to add.</param>
        /// <returns>The added delivery partner.</returns>
        public async Task<DeliveryPartner> Add(DeliveryPartner item)
        {
            _context.Add(item);
            _context.SaveChanges();

            LogInformation($"Delivery Partner Added: {item.PartnerId}");

            return item;
        }
        /// <summary>
        /// Deletes an existing delivery partner from the database.
        /// </summary>
        /// <param name="key">The ID of the delivery partner to delete.</param>
        /// <returns>The deleted delivery partner, if found.</returns>
        public async Task<DeliveryPartner> Delete(int key)
        {
            var partner = await GetAsync(key);

            if (partner != null)
            {
                _context.DeliveryPartners.Remove(partner);
                _context.SaveChanges();

                LogInformation($"Delivery Partner Deleted: {partner.PartnerId}");
                return partner;
            }
            return null;

        }
        /// <summary>
        /// Retrieves a delivery partner by its ID.
        /// </summary>
        /// <param name="key">The ID of the delivery partner to retrieve.</param>
        /// <returns>The retrieved delivery partner, if found.</returns>
        public async Task<DeliveryPartner> GetAsync(int key)
        {
            var partners = await GetAsync();
            var partner = partners.FirstOrDefault(p => p.PartnerId == key);

            if (partner != null)
            {
                return partner;
            }

            throw new NoDeliveryPartnerFoundException();
        }

        /// <summary>
        /// Retrieves all delivery partners from the database.
        /// </summary>
        /// <returns>A list of all delivery partners.</returns>
        public async Task<List<DeliveryPartner>> GetAsync()
        {
            var partners = _context.DeliveryPartners.ToList();
            return partners;
        }

        /// <summary>
        /// Updates an existing delivery partner in the database.
        /// </summary>
        /// <param name="item">The delivery partner to update.</param>
        /// <returns>The updated delivery partner.</returns>
        public async Task<DeliveryPartner> Update(DeliveryPartner item)
        {
            var partner = await GetAsync(item.PartnerId);

            if (partner != null)
            {
                //_context.Entry<DeliveryPartner>(item).State = EntityState.Modified;
                partner.Name = item.Name;
                partner.Email = item.Email;
                partner.Phone = item.Phone;
                partner.CityId = item.CityId;
                partner.UserName = item.UserName;
                _context.SaveChanges();
                LogInformation($"Delivery Partner Updated: {item.PartnerId}");
                return partner;

            }

            throw new NoDeliveryPartnerFoundException();
        }

        /// <summary>
        /// Retrieves a delivery partner by its name.
        /// </summary>
        /// <param name="name">The name of the delivery partner to retrieve.</param>
        /// <returns>The retrieved delivery partner, if found.</returns>
        public async Task<DeliveryPartner> GetAsync(string name)
        {
            var partners = _context.DeliveryPartners.ToList();
            var partner = partners.FirstOrDefault(p => p.Name == name);
            if (partner != null)
            {
                return partner;
            }
            throw new NoDeliveryPartnerFoundException();
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
