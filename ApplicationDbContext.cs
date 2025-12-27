// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using ShipManagement.Models;
using System.Collections.Generic;

namespace ShipManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Ship> Ships { get; set; }
        public DbSet<CargoOwner> CargoOwners { get; set; }
        public DbSet<CargoType> CargoTypes { get; set; }
        public DbSet<ShippingAgent> ShippingAgents { get; set; }
        public DbSet<Dockworker> Dockworkers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CargoType>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();
                  });
        }
    }

}