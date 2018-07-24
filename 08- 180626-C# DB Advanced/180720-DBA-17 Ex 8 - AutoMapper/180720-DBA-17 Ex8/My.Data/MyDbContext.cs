using Microsoft.EntityFrameworkCore;
using My.Models;

namespace My.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {}

        public MyDbContext(DbContextOptions options) : base(options)
        {}

        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConfigurationString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(e =>
            {
                e.HasOne(m => m.Manager)
                 .WithMany(m => m.Subordinates)
                 .HasForeignKey(m => m.ManagerId);
            });
        }
    }
}
