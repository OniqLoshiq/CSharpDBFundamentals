using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.EntityConfig
{
    public class CountryConfig : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.Property(x => x.Name)
                   .IsRequired();

            builder.HasMany(x => x.Towns)
                   .WithOne(x => x.Country)
                   .HasForeignKey(x => x.CountryId);
        }
    }
}
