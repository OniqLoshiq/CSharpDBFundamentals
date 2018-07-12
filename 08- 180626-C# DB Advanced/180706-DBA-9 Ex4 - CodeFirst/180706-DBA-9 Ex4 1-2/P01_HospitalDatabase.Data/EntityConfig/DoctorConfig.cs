using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data.EntityConfig
{
    public class DoctorConfig : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(x => x.DoctorId);

            builder.Property(x => x.Name)
                   .HasMaxLength(100)
                   .IsUnicode(true);

            builder.Property(x => x.Specialty)
                   .HasMaxLength(100)
                   .IsUnicode(true);

            builder.HasMany(x => x.Visitations)
                   .WithOne(x => x.Doctor)
                   .HasForeignKey(x => x.DoctorId);
        }
    }
}
