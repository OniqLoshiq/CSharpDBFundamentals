using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data.EntityConfig
{
    public class ResourceConfig : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.Property(x => x.Name)
                   .HasMaxLength(50)
                   .IsUnicode(true)
                   .IsRequired();

            builder.Property(x => x.Url)
                   .IsUnicode(false);
        }
    }
}
