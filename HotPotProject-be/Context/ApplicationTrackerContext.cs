using HotPotProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace HotPotProject.Context
{
    public class ApplicationTrackerContext:DbContext
    {
        public ApplicationTrackerContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<City>? Cities { get; set; }
        public DbSet<DeliveryPartner>? DeliveryPartners { get; set; }
        public DbSet<Menu>? Menus { get; set; }
        public DbSet<NutritionalInfo>? NutritionalInfos { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderItem>? OrderItems { get; set; }
        public DbSet<Payment>? Payments { get; set; }
        public DbSet<Restaurant>? Restaurants { get; set; }
        public DbSet<RestaurantSpeciality>? RestaurantSpecialities { get; set; }
        public DbSet<State>? States { get; set; }
        public DbSet<Customer>? Customers { get; set; }
        public DbSet<CustomerAddress>? CustomerAddresses { get; set; }
        public DbSet<CustomerReview>? CustomerReviews { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<RestaurantOwner>? RestaurantOwners { get; set; }
        public DbSet<Cart>? Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<OrderItem>()
                .HasKey(o => new { o.OrderId, o.MenuId });

        }
    }
}
