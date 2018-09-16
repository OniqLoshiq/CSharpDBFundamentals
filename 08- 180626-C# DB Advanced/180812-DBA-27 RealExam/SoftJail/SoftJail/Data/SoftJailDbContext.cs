﻿namespace SoftJail.Data
{
	using Microsoft.EntityFrameworkCore;
    using SoftJail.Data.Models;

    public class SoftJailDbContext : DbContext
	{
		public SoftJailDbContext()
		{
		}

		public SoftJailDbContext(DbContextOptions options)
			: base(options)
		{
		}

        public DbSet<Prisoner> Prisoners { get; set; }
        public DbSet<Officer> Officers { get; set; }
        public DbSet<Mail> Mails { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Cell> Cells { get; set; }
        public DbSet<OfficerPrisoner> OfficersPrisoners { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder
					.UseSqlServer(Configuration.ConnectionString);
			}
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
            builder.Entity<OfficerPrisoner>().HasKey(op => new { op.PrisonerId, op.OfficerId });

            builder.Entity<OfficerPrisoner>()
                .HasOne(op => op.Prisoner)
                .WithMany(p => p.PrisonerOfficers)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OfficerPrisoner>()
               .HasOne(op => op.Officer)
               .WithMany(o => o.OfficerPrisoners)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Department>()
                .HasMany(c => c.Cells)
                .WithOne(c => c.Department)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Department>()
                .HasMany(c => c.Officers)
                .WithOne(o => o.Department)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Cell>()
                .HasMany(c => c.Prisoners)
                .WithOne(p => p.Cell)
                .OnDelete(DeleteBehavior.Restrict);
        }
	}
}