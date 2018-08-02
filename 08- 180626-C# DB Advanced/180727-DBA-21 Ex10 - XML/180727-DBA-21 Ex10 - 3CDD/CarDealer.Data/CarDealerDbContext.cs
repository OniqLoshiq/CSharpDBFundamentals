using CarDealer.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealer.Data
{
    public class CarDealerDbContext : DbContext
    {
        public CarDealerDbContext()
        {
        }

        public CarDealerDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PartCar> PartCars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConfigurationString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PartCar>().HasKey(x => new { x.CarId, x.PartId });

            modelBuilder.Entity<PartCar>()
                .HasOne(x => x.Car)
                .WithMany(x => x.PartCars)
                .HasForeignKey(x => x.CarId);

            modelBuilder.Entity<PartCar>()
                .HasOne(x => x.Part)
                .WithMany(x => x.PartCars)
                .HasForeignKey(x => x.PartId);

            modelBuilder.Entity<Sale>(x =>
                {
                    x.HasOne(e => e.Car)
                     .WithMany(e => e.Sales)
                     .HasForeignKey(e => e.CarId);

                    x.HasOne(e => e.Customer)
                     .WithMany(e => e.Sales)
                     .HasForeignKey(e => e.CustomerId);
                 });
        }
    }
}
