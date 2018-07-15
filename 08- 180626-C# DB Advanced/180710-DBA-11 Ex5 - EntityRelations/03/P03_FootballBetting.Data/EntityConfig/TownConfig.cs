using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data.EntityConfig
{
    public class TownConfig : IEntityTypeConfiguration<Town>
    {
        public void Configure(EntityTypeBuilder<Town> builder)
        {
            builder.Property(x => x.Name)
                   .IsRequired();

            builder.HasMany(x => x.Teams)
                   .WithOne(x => x.Town)
                   .HasForeignKey(x => x.TownId);
        }
    }
}
