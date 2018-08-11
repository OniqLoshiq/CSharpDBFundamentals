namespace PetClinic.Data
{
    using Microsoft.EntityFrameworkCore;
    using PetClinic.Models;

    public class PetClinicContext : DbContext
    {
        public PetClinicContext() { }

        public PetClinicContext(DbContextOptions options)
            :base(options) { }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalAid> AnimalAids { get; set; }
        public DbSet<Passport> Passports { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<Vet> Vets { get; set; }
        public DbSet<ProcedureAnimalAid> ProceduresAnimalAids { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Passport>()
                .HasOne(p => p.Animal)
                .WithOne(p => p.Passport)
                .HasForeignKey<Animal>(p => p.PassportSerialNumber);

            builder.Entity<ProcedureAnimalAid>().HasKey(x => new { x.AnimalAidId, x.ProcedureId });

            builder.Entity<ProcedureAnimalAid>()
                .HasOne(pa => pa.AnimalAid)
                .WithMany(ai => ai.AnimalAidProcedures);

            builder.Entity<ProcedureAnimalAid>()
                .HasOne(pa => pa.Procedure)
                .WithMany(p => p.ProcedureAnimalAids);
        }
    }
}
